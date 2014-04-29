using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelnetExpect;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Management;

namespace PhoneConnections
{
    /* This class uses the open source Telnet and Expect library available at:
     * https://telnetexpect.codeplex.com/
     * I have added the DLL for simplicity but the source code is available at the link above
     */
    public class WifiTel
    { 
        private TelnetStream telnet;
        private Expector psychic;
        private CellDevice dev;
        private Queue<string> commands = new Queue<string>();
        private string _status;
        public string StatusMsg { get { return _status; } }
        /// <summary>
        /// Initializes the wifi telnet session
        /// </summary>
        /// <param name="dev">Type of cellular device</param>
        public WifiTel(CellDevice dev){
            this.dev = dev;
        }
        /// <summary>
        /// Creates a new Telnet instance
        /// </summary>
        /// <param name="IPAddr">IP Address of the Device</param>
        /// <param name="port">Telnet port for the listening device</param>
        /// <returns> Bool value representing the success of connection</returns>
        public void Connect(IPAddress IPAddr, int port) 
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(IPAddr, port);
                telnet = new TelnetStream(new NetworkStream(socket));
                psychic = new Expector(telnet);
                psychic.ExpectTimeout = new TimeSpan(0, 0, 8); //5 sec timeout
                _status = "Online";
            }
            catch (SocketException)
            {
                _status = "Connection Error";
            }
            catch
            {
                _status = "Error";
            }
        }
        public void Connect(IPAddress IPAddr)
        {
            this.Connect(IPAddr, TelnetStream.DefaultPort);
        }
        public void AutoConnect()
        {
            this.Connect(IPAddress.Parse(GetDhcpIP()));
        }
        private string GetDhcpIP()
        {
            string Key = "Win32_NetworkAdapterConfiguration";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + Key);
            foreach (ManagementObject mo in searcher.Get())
            {
                if (mo["DHCPServer"] != null && mo["DHCPServer"].ToString().StartsWith("169.254."))
                    return mo["DHCPServer"].ToString();
            }
            return null;
        }
        public string[] GetDhcpIPList()
        {
            List<string> dhcp = new List<string>(2);
            string Key = "Win32_NetworkAdapterConfiguration";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + Key);
            foreach (ManagementObject mo in searcher.Get())
            {
                if (mo["DHCPServer"] != null && mo["DHCPServer"].ToString().StartsWith("169.254."))
                    dhcp.Add(mo["DHCPServer"].ToString());
            }
            return dhcp.ToArray();
        }
        /// <summary>
        /// Function for opening a telnet session on the mobile device.
        /// </summary>
        /// <param name="user">An accepted username. Preferably root.</param>
        /// <param name="pass">Password for the username</param>
        public void Login(string user, string pass)
        {
            if (psychic == null || telnet == null)
            {
                _status = "Error";
                return;
            }
            try
            {
                psychic.Expect("login:");
                psychic.SendLine(user);
                psychic.Expect("Password:");
                psychic.SendLine(pass);
                psychic.Expect("#");
                //
                string appname = "";
                switch (dev)
                {
                    case CellDevice.ti1273:
                        appname = "wlan_cu_ti1273_mcp33";
                        break;
                    case CellDevice.ti1283:
                        appname = "wlan_cu_ti1283_mcp33";
                        break;
                    case CellDevice.bcm:
                        FillQuery("out","up","mpc 0","down",
                            "country ALL", "band b", "up");
                        NSendData();
                        _status = "Logged";
                        return; //rest is for ti12xx drivers
                    case CellDevice.other:
                        FillQuery("fw_pltenable::1", "change_cc::GB");
                        ESendData();
                        return;//rest is for ti12xx
                    default:
                        throw new ArgumentException("Unsupported Device");
                }
                psychic.SendLine(appname);
                psychic.Expect(appname);
                _status = "Logged";
            }
            catch
            {
                _status = "Login Error";
            }
        }

        /// <summary>
        /// Public function for starting Test mode transmission. 
        /// </summary>
        /// <param name="mod">Single character description. Valid chars: a/b/g/n</param>
        /// <param name="channel">Channel number for the transmission</param>
        /// <param name="rate">Data rate, you might want to look up valid rates for each 802.11 technology.</param>
        public void InitTx(char mod, int channel, string rate)
        {
            if (dev == CellDevice.bcm)
            {
                if (rate.StartsWith("MCS"))
                    rate = rate.Substring(3, rate.Length - 3);
                NStartTx(mod, channel, Double.Parse(rate));
            }//switch
            else if (dev == CellDevice.other)
            {
                if (rate == "0" || rate == "7" || rate == "4")
                {
                    rate = "MCS" + rate;
                }
                EStartTx(mod, channel, rate);
            }
            else
            {
                StartTx(mod, channel, rate.ToString());
            }
        }
        public void EndTx()
        {
            if(dev == CellDevice.bcm)
                NStopTx();
            else if (dev == CellDevice.other)
                EStopTx();
            else 
                StopTx();
        }
        
        #region Broadcom
        private void NStartTx(char mod, int channel, double rate)
        {
            char modtype;
            //Broadcom4330(?) drivers:
            switch (mod)
            {
                case 'b':
                case 'g':
                    modtype = 'b';
                    //band = 2;
                    if (!(channel <= 14 && channel > 0))
                        throw new ArgumentException("Invalid Channel Value!");
                    break;
                case 'a':
                    modtype = 'a';
                    //band = 5;
                    if (channel > 216)
                        throw new ArgumentException("Invalid Channel Value!");
                    break;
                case 'n':
                    if (channel >= 36)
                        goto case 'a';
                    else
                        goto case 'b';
                default:
                    modtype = 'b';
                    //band = 2;
                    break;
            }
            FillQuery("pkteng_stop tx", "down", "mpc 0", "up",
            "band " + modtype,
                //String.Format("chanspec -c {0} -b {1} -w 20 -s 0", channel, band),
            "channel " + channel,
            ((mod == 'n') ? "nrate -m " : "rate ") + rate, //nrate for 802.11n
            "txpwr1 -1",
            "pkteng_start 00:11:22:33:44:55 tx 50 1024 0"
            );
            NSendData();
        }
        private void NStopTx()
        {
            FillQuery(@"pkteng_stop tx");
            NSendData();
        }

        private void NSendData()
        {
            StreamWriter wtr = new StreamWriter(telnet);
            wtr.NewLine = "\r\n"; //ensure this
            wtr.AutoFlush = true; //important! we don't want hangs due to buffering!
            while (commands.Count > 0)
            {
                string c = commands.Dequeue();
                wtr.WriteLine("wl_bcm4334 "+ c);
                try
                {
                    psychic.Expect("#");
                }
                catch(Exception e)
                {
                    _status = "Error";
                    throw e;
                }
            }
            _status = "Sent";
        }
        #endregion

        #region tl12xx
        public void Calibrate()
        {
            if (psychic == null || telnet == null)
            {
                _status = "Error";
                return;
            }
            FillQuery(@"/ w p 1 f 2",
                        @"/ t r h 1 6",
                        @"/ t r t n 50 1 1000 0 20000 0 3 0 0 1 0 0 1 0 11:22:33:44:55:66",
                        @"/ t r t s",
                        @"/ t b v 21",
                        @"/ t b t 1 1 1 1 1 1 1 1"
                        );
            SendData();
        }
        private void StartTx(char mod, int channel, string rate)
        {
            int preamble, powerlvl, ratelvl;
            int freqlvl = 0;
            switch (mod)
            {
                case 'b':
                    preamble = 0;
                    break;
                case 'g':
                    preamble = 4;
                    break;
                case 'n':
                    preamble = 6;
                    rate.Insert(0, "MCS");
                    if (channel >= 36) freqlvl = 1; //5GHz Wifi-n
                    break;
                case 'a':
                    preamble = 4;
                    freqlvl = 1;
                    break;
                default:
                    preamble = 0;
                    break;
            }
            using (StreamReader sr = new StreamReader("powerlvl.txt"))
            {
                string data = sr.ReadToEnd();
                Match n = Regex.Match(@data,
                    "^" + rate + @"(?:\:(\d+))+.*$", RegexOptions.Multiline); //skip LF value in the end!
                if (!n.Success) throw new Exception("HOW?");
                ratelvl = Int32.Parse(n.Groups[1].Captures[0].Value); // to be fair, i don't know how to do this in Perl
                powerlvl = 20000; //getting each captured phrase in a quantified group that is.
                FillQuery(
                    @"/ t r t s",
                    @"/ w p 1 f 2",
                    String.Format(@"/ t r h  {0} {1} ", freqlvl, channel),
                    String.Format(@"/ t r t n 50 {0} 1000 0 {1} 0 3 0 0 {2} 0 0 1 0 00:11:22:33:44:55",
                        ratelvl, powerlvl, preamble));
            }
            SendData();
        }
        private void StopTx()
        {
            FillQuery(@"/ t r t s");
            SendData();
        }
        private void FillQuery(params string[] data)
        {
            foreach (string d in data) commands.Enqueue(d);
        }
        private void SendData()
        {
            StreamWriter wtr = new StreamWriter(telnet);
            wtr.NewLine = "\r\n"; //ensure this
            wtr.AutoFlush = true; //important! we don't want hangs due to buffering!
            while(commands.Count >0)
            {
                string c = commands.Dequeue();
                wtr.WriteLine(c);
            }
            _status = "Sent";
        }
        #endregion

        ///This part is a device specific method, unlike the other two (driver specific)...
        #region Other
        private void EStartTx(char mod, int channel, string rate)
        {
            //A-series devices:
            //Channels 1-13 and 36-140 = GB
            //Channels 149-165 = US
            string countryCode = "GB";
            switch (mod)
            {
                case 'b':
                case 'g':
                    if (!(channel <= 14 && channel > 0))
                        throw new ArgumentException("Invalid Channel Value!");
                    break;
                case 'a':
                    if (channel <= 140)
                        countryCode = "GB";
                    else if (channel <= 165)
                        countryCode = "US";
                    else
                        throw new ArgumentException("Invalid Channel Value!");
                    break;
                case 'n':
                    rate = "MCS" + rate;
                    if (channel >= 36)
                        goto case 'a';
                    else
                        goto case 'b';
                default:
                    throw new ArgumentException(
                        "Whatever extra-terrestrial technology you are trying to run is not implemented in this code yet."
                        );
            }
            if (!Regex.IsMatch(rate, @"/^\d+$/"))
                rate = "n:" + rate.ToUpper();
            else
                rate = ":" + rate;

            FillQuery("plt_txcont::0",
            "plt_radiotune:n:" + channel,
            "change_cc::" + countryCode,
            "plt_bitrate:" + rate,
            "plt_txcont::1"
            );
            ESendData();
        }
        private void EStopTx()
        {
            FillQuery(@"plt_txcont::0");
            ESendData();
        }

        private void ESendData()
        {
            StreamWriter wtr = new StreamWriter(telnet);
            wtr.NewLine = "\r\n"; //ensure this
            wtr.AutoFlush = true; //important! we don't want hangs due to buffering!
            while (commands.Count > 0)
            {
                string c = commands.Dequeue();
                c = String.Format("echo \"{0}\" >> /pps/services/wifi/escreen", c);
                wtr.WriteLine(c);
                try
                {
                    psychic.Expect("#");
                }
                catch (Exception e)
                {
                    _status = "Error";
                    throw e;
                }
            }
            _status = "Sent";
        }
        #endregion
        //At the time, I needed different device configurations due to different drivers etc, hence this enum.
        public enum CellDevice { ti1273 = 1, ti1283, bcm, other }

    }
}
