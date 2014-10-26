using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PhoneConnections;
using System.Threading;
using System.Xml.Serialization;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// What the name says, outputs bogus steps prior to test start. 
    /// Always entertaining when a co-worker reports that the application attempting to 'login to Facebook'.
    /// </summary>
    class LolTimer : System.Timers.Timer
    {
        public object Param1 { get; set; }
    }

    /// <summary>
    /// The brains of the operation. Nested loops, gotos... This function has them all!
    /// </summary>
    class TestRunner
    {
        private ManualResetEvent _Cancel;
        private TestSetup config;
        Parser parser;
        /// <summary>
        /// Main function for creating a basic test loop.
        /// </summary>
        /// <param name="config">Test config struct that contains the test information</param>
        public TestRunner(TestSetup config)
        {
            _Cancel = new ManualResetEvent(false);
            this.config = config;
            Scanner sc;
            try
            {
                sc = new Scanner();
                sc.ScanScript(config.scriptLoc);
            }
            catch (Exception e)
            {
                throw e;
            }
            parser = new Parser(sc.FirstToken);
        }
        /// <summary>
        /// Run the test!
        /// </summary>
        /// <param name="bgw">Background worker instance, so that we can report progress on the UI.</param>
        public void RunTest(System.ComponentModel.BackgroundWorker bgw)
        {
            if (bgw == null) throw new ArgumentException("Background Worker must be defined!");
            //Some humor:
            LolTimer _loltimer = new LolTimer();
            _loltimer.AutoReset = true;
            _loltimer.Interval = 2500;
            _loltimer.Param1 = bgw;
            _loltimer.Elapsed += new System.Timers.ElapsedEventHandler(_loltimer_Elapsed);
            _loltimer.Start();
            GPIBConnector dc = null;
            TempChamber tch = null;
            var T_Queue = new Queue<int>(config.temp); int temperature = 0;
            var V_Queue = new Queue<float>(config.volt); float voltage = 0.0f;
            //int t_count = 0, v_count = 0; //counters for temp and volt

            /*
             * I am almost certain that there is a better way to create classes on the fly, rather than 
             * issuing a set of conditionals and loops. Until then, I will keep this mess here...
             */
            if (config.volt.Length > 0)
            {
                dc = new GPIBConnector();
                dc.Connect(0, config.dcaddr, 0);
                dc.SendData("CURR 2.0"); //set current
                dc.SendData("VOLT 3.8"); // set volt
                dc.SendData("OUTPUT:STATE ON");
            }
            if (config.temp.Length > 0)
            {
                tch = new TempChamber(config.comport, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
                tch.Open();
                if (!tch.modbusStatus.Contains("successful"))
                {
                    throw new Exception("Cannot connect to Temperature Chamber. Exiting...");
                }
            }
            //

            //
            GPIBConnector SPA = new GPIBConnector();
            SPA.Connect(0, config.spaddr, 0);
            SPA.ResetDevice();
            WifiTel.CellDevice dev;
            if (Enum.IsDefined(typeof(WifiTel.CellDevice), config.phonemodel))
            {
                dev = (WifiTel.CellDevice)Enum.Parse(typeof(WifiTel.CellDevice), config.phonemodel, true);
            }
            else
            {
                throw new Exception("Invalid Phone Model!");
            }
            WifiTel telnet = new WifiTel((WifiTel.CellDevice)Enum.Parse(typeof(WifiTel.CellDevice), config.phonemodel)); //FIX
            telnet.AutoConnect();
            if (!telnet.StatusMsg.Contains("Online"))
                throw new IOException("Wifi Telnet Session Failure!");
            telnet.Login("root", "root");
            //Set up variables
            //
            string[] spdrate;
            //Report start
            _loltimer.Stop();
            bgw.ReportProgress(0, "Starting Test...");
            //Start Test-Loop
            if (parser == null)
            {
                throw new Exception("How?");
            }
            parser.AddData("i", config.imgindex.ToString());
            //
            StreamWriter logger = new StreamWriter(config.logLoc, true);
            logger.AutoFlush = true;
            //
            parser.AddData("imgpath", @config.savePicLoc);
            //used for temperature testing, empty string otherwise
            string envinfo = String.Empty;
            do
            {
                if (tch != null)
                {
                    temperature = T_Queue.Dequeue();
                    bgw.ReportProgress(2, String.Format("Settling temperature at {0}\u00B0C", temperature));
                    tch.SettleTemp(temperature, 0.3f, ref this._Cancel, new StreamWriter(Stream.Null));
                    parser.AddData("temp", temperature.ToString());
                }
                do
                {
                    if (dc != null)
                    {
                        voltage = V_Queue.Dequeue();
                        bgw.ReportProgress(2, String.Format("Setting voltage to {0}V", voltage));
                        dc.SendData("VOLT " + voltage);
                        dc.SendData("MEAS:VOLT?");
                        double current_volt = Convert.ToDouble(dc.ReadData());
                        if (Math.Abs(current_volt - voltage) > 0.02)
                        {
                            throw new Exception("Cannot change voltage!");
                        }
                        parser.AddData("volt", voltage.ToString());
                        envinfo = String.Format("{0},{1}", temperature, voltage);
                    }
                    //
                    foreach (int c in config.channel)
                    {
                        foreach (char m in config.mod)
                        {
                            if (m == 'a' || m == 'g')
                                spdrate = new string[] { "6", "24", "54" };
                            else if (m == 'n')
                                spdrate = new string[] { "MCS0", "MCS4", "MCS7" };
                            else
                                spdrate = new string[] { "1", "5.5", "11" };
                            //If Quick Test Mode:
                            if (config.quick)
                            {
                                spdrate = spdrate.Take(1).ToArray();
                            }
                            parser.AddData("channel", c.ToString());
                            parser.AddData("frequency", this.ConvertToFrequency(c));
                            foreach (string spd in spdrate)
                            {
                                parser.AddData("speed", spd.ToString());
                                telnet.InitTx(m, c, spd);
                                //I DON'T LIKE GOTO EITHER, BUT I DON'T WANNA USE 5 CONSECUTIVE BREAK STATEMENTS EITHER
                                if (_Cancel.WaitOne(1000)) goto Cleanup; //used thread.sleep before, might as well merge them

                                //Report upcoming test
                                bgw.ReportProgress(
                                    1,
                                    String.Format("Running: 802.11{0}, Ch. {1}, {2} Mbps", m, c, spd)
                                    );
                                /*
                                 * This delegate type is defined in Parser.cs
                                 * Let's hope this works!
                                 */
                                ProcessLineDelegate pdel = new ProcessLineDelegate((s, p) =>
                                {
                                    SPA.SendData(s.Trim() + @"\n");
                                    Thread.Sleep(80);
                                    if (s.Contains('?'))
                                    {
                                        string res = SPA.ReadData().Trim();
                                        p.AddData("_", res);
                                        logger.WriteLine("{0},{1},{2},{3}", envinfo, c, spd, res);
                                    }
                                });
                                parser.Interpret(pdel);
                            }
                        }
                    }
                } while (V_Queue.Count() > 0);
                V_Queue = new Queue<float>(config.volt);
            } while (T_Queue.Count() > 0);
            /*
             * HEY LOOK BELOW, IT IS A LABEL
             * YES, THAT MEANS THERE IS GOTO STATEMENT LURKING AROUND HERE SOMEWHERE...
             */
            //Clean-up -> GOTO STATEMENT FROM THE DATA RATE LOOP
            Cleanup:
            //Report completion
            if (_Cancel.WaitOne(0))
                bgw.ReportProgress(100, "Test Cancelled");
            else
                bgw.ReportProgress(100, "Test Complete");
            if (dc != null)
            {
                dc.SendData("OUTPUT:STATE OFF");
                dc.Dispose();
            }//telnet close func!
            SPA.Dispose();
            if (tch != null)
            {
                tch.SetTemp(20);
                tch.Close();
            }
            logger.Close();
        }

        public void TestGPIBScript()
        {
            GPIBConnector SPA = new GPIBConnector();
            SPA.Connect(0, config.spaddr, 0);
            Scanner sc = new Scanner();
            sc.ScanScript(config.scriptLoc);
            Parser parser = new Parser(sc.FirstToken);
            parser.Interpret((s, o) =>
            {
                SPA.SendData(s.Trim());
                if (s.Contains('?'))
                {
                    string res = SPA.ReadData().Trim();
                    (o as Parser).AddData("_", res);
                }
            });
        }
        /// <summary>
        /// Private function for converting 802.11 channels to their corresponding frequencies.
        /// Derived this on the fly, may not be robust.
        /// </summary>
        /// <param name="channel">Channel number to be converted</param>
        /// <returns>Frequency values in MHz</returns>
        private string ConvertToFrequency(int channel)
        {
            if (channel < 14)
            {
                return (2412 + (channel - 1) * 5).ToString();
            }
            else if (channel >= 36 && channel <= 165)
            {
                return (5180 + (channel - 36) * 5).ToString();
            }
            else
            {
                throw new ArgumentException("Unsupported channel!");
            }
        }

        public void StopTest()
        {
            _Cancel.Set();
        }

        #region UnnecessaryCode
        //String array to be used by the LOLTimer class. Modify it as you please, don't forget to be creative!
        private string[] cake = {"Figuring out what to do next...", "Formatting test equipment...", "Starting a phone call...",
            "Retrieving Facebook API Token...", "Starting music playback...", "Scrambling Tx Power values...", "Doing science stuff..."};
        void _loltimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Random r = new Random();
            ((sender as LolTimer).Param1 as System.ComponentModel.BackgroundWorker)
                .ReportProgress(2, cake[r.Next(0, cake.Length)]);
        }
        #endregion
    }

    /// <summary>
    /// Class implementation for saving/loading test configurations
    /// </summary>
    [Serializable]
    public class TestSetup
    {
        public string comport, scriptLoc, savePicLoc, logLoc, phonemodel;
        public int spaddr, dcaddr,imgindex;
        public int[] temp;
        public int[] channel;
        public float[] volt;
        public char[] mod;
        public bool quick;
    }
}
