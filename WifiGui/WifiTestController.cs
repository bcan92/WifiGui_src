using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoneConnections;
using System.Threading;
namespace WindowsFormsApplication1
{
    class WifiTestController
    {
#region fields
        WifiTel _connector;
        GPIBConnector spa;
        //GPIBConnector dc;
        //Modbus temp;
        char mod;
        List<int> channels;
        string[] spd;
        Dictionary<string, string> variables;
        ManualResetEvent pause = new ManualResetEvent(true);
#endregion
        //test only spa and phone
        public WifiTestController(WifiTel.CellDevice dev, char mod, int[] channels, int spaPrimAddr)
        {
            _connector = new WifiTel(dev);
            this.mod = mod;
            PickChannels(mod);
            this.channels = new List<int>();
            this.channels.AddRange(channels);
            spa = new GPIBConnector();
            spa.Connect(0, spaPrimAddr, 0);
            variables = new Dictionary<string, string>();
        }
        private void PickChannels(char mod)
        {
            if (mod == 'a' || mod == 'g')
                spd = new string[3] { "6", "24", "54" };
            else if (mod == 'n')
                spd = new string[3] { "MCS0", "MCS4", "MCS7" };
            else
                spd = new string[3] { "1", "5.5", "11" }; 
        }

        public void RunTest()
        {
            _connector.AutoConnect();
            if (_connector.StatusMsg == "error") 
                throw new Exception("Can't connect to device!");
            _connector.Login("root", "root");

            foreach (int ch in channels)
            {
                pause.WaitOne();
                foreach (string s in spd)
                {
                    _connector.StopTx();
                    _connector.StartTx(mod, ch, s);
                    //Update Vars
                    variables["channel"] = ch.ToString();
                    variables["spd"] = s;
                    //
                    ParseScript();
                }
            }

        }
        private void ParseScript(){}
    }
}
