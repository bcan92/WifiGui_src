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
                TcpClient tcl = new TcpClient(Dns.GetHostEntry(IPAddr).HostName, port);

                System.IO.Stream st = tcl.GetStream();
                telnet = new TelnetStream(st);
                psychic = new Expector(telnet);
                psychic.ExpectTimeout = new TimeSpan(0, 0, 5); //5 sec timeout
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
                    return mo["DHCPServer"].ToString(); ;
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
                    case CellDevice.LAGUNA:
                    case CellDevice.LISBON:
                        appname = "wlan_cu_ti1273_mcp33";
                        break;
                    case CellDevice.LONDON:
                    case CellDevice.LIVERPOOL:
                        appname = "wlan_cu_ti1283_mcp33";
                        break;
                    case CellDevice.NEVADA:
                        FillQuery("ifconfig bcm0 up", @"if pidin | grep  -s wpa_supplicant ; then slay wpa_supplicant; fi");
                        SendData();
                        FillQuery("out","up","mpc 0","down",
                            "country ALL", "band b", "up");
                        NSendData();
                        _status = "Logged";
                        return; //rest is for L-series
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
        public void InitTx(char mod, int channel, string rate)
        {
            if (dev == CellDevice.NEVADA    )
            {
                if (rate.StartsWith("MCS"))
                    rate = rate.Substring(3, rate.Length - 3);
                NStartTx(mod, channel, Double.Parse(rate));
            }//switch
            else
            {
                StartTx(mod, channel, rate.ToString());
            }
        }
        public void EndTx()
        {
            if(dev == CellDevice.NEVADA)
                NStopTx();
            else 
                StopTx();
        }
        
        #region N-series
        private void NStartTx(char mod, int channel, double rate)
        {
            char modtype;
            //N-series devices, obsolete:
            //int band;
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
        #region L-series
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
        public enum CellDevice{LISBON = 1, LAGUNA, LONDON, LIVERPOOL, NEVADA}
    }
}
