using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NationalInstruments.NI4882;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace PhoneConnections
{
   
    /// <summary>
    /// Modbus protocol implementation for C# 2.0.
    /// Available at: https://git.codeproject.com/distantcity/simple-modbus-protocol-in-c-net-2-0
    /// This version extends the modbus library and allows writing to single registers.
    /// </summary>
    public class Modbus
    {
        private SerialPort sp = new SerialPort();
        public string modbusStatus;

        #region Constructor / Deconstructor
        public Modbus(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            //Assign desired settings to the serial port:
            sp.PortName = portName;
            sp.BaudRate = baudRate;
            sp.DataBits = databits;
            sp.Parity = parity;
            sp.StopBits = stopBits;
            //These timeouts are default and cannot be editted through the class at this point:
            sp.ReadTimeout = 1000;
            sp.WriteTimeout = 1000;
        }
        #endregion

        #region Open / Close Procedures
 
            public bool Open()
            {
                if (!sp.IsOpen)
                {
                    try
                    {
                        sp.Open();
                    }
                    catch (Exception err)
                    {
                        modbusStatus = "Error opening " + sp.PortName + ": " + err.Message;
                        return false;
                    }
                    modbusStatus = sp.PortName + " opened successfully";
                    return true;
                }
                else
                {
                    modbusStatus = sp.PortName + " already opened";
                    return false;
                }
            }
        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (sp.IsOpen)
            {
                try
                {
                    sp.Close();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error closing " + sp.PortName + ": " + err.Message;
                    return false;
                }
                modbusStatus = sp.PortName + " closed successfully";
                return true;
            }
            else
            {
                modbusStatus = sp.PortName + " is not open";
                return false;
            }
        }
        #endregion

        #region CRC Computation
        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a  byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        #endregion

        #region Build Message
        private void BuildMessage(byte address, byte type, ushort start, ushort registers, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            message[0] = address;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)registers;

            GetCRC(message, ref CRC);
            message[message.Length - 2] = CRC[0];
            message[message.Length - 1] = CRC[1];
        }
        #endregion

        #region Check Response
        private bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                return true;
            else
                return false;
        }
        #endregion

        #region Get Response
        private void GetResponse(ref byte[] response)
        {
            //There is a bug in .Net 2. DataReceived Event that prevents people from using this
            //event as an interrupt to handle data (it doesn't fire all of the time).  Therefore
            //we have to use the ReadByte command for a fixed length as it's been shown to be reliable.
            for (int i = 0; i < response.Length; i++)
            {
                response[i] = (byte)(sp.ReadByte());
            }
        }
        #endregion

        #region Function  - Write Single Register
        public bool SendFc6(byte address, ushort register, short value)
        {
            //Ensure port is open:
            if (sp.IsOpen)
            {
                //Clear in/out buffers:
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                //Message is  addr +  fcn +  start +  reg +  count +  * reg vals +  CRC
                byte[] message = new byte[8];
                //Function  response is fixed at  bytes
                byte[] response = new byte[8];
                //Fill fixed values
                message[0] = address;
                message[1] = (byte)6;
                message[2] = (byte)(register >> 8);
                message[3] = (byte)(register);
                //Put write values into message prior to sending:
                message[4] = (byte)(value >> 8);
                message[5] = (byte)(value);
                //Build outgoing message:
                byte[] CRC = new byte[2];
                GetCRC(message, ref CRC);
                message[6] = CRC[0];
                message[7] = CRC[1];

                //Send Modbus message to Serial Port:
                try
                {
                    sp.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    modbusStatus = "Write successful";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function  - Write Multiple Registers
        public bool SendFc16(byte address, ushort start, ushort registers, short[] values)
        {
            //Ensure port is open:
            if (sp.IsOpen)
            {
                //Clear in/out buffers:
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                //Message is  addr +  fcn +  start +  reg +  count +  * reg vals +  CRC
                byte[] message = new byte[5 + 2 * registers];
                //Function  response is fixed at  bytes
                byte[] response = new byte[8];

                //Add bytecount to message:
                message[6] = (byte)(registers * 2);
                //Put write values into message prior to sending:
                for (int i = 0; i < registers; i++)
                {
                    message[7 + 2 * i] = (byte)(values[i] >> 8);
                    message[8 + 2 * i] = (byte)(values[i]);
                }
                //Build outgoing message:
                BuildMessage(address, (byte)16, start, registers, ref message);

                //Send Modbus message to Serial Port:
                try
                {
                    sp.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    modbusStatus = "Write successful";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function  - Read Input Registers
        public bool SendFc4(byte address, ushort start, ushort registers, ref short value)
        {
            //Ensure port is open:
            if (sp.IsOpen)
            {
                //Clear in/out buffers:
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                //Function  request is always  bytes:
                byte[] message = new byte[8];
                //Function  response buffer:
                byte[] response = new byte[5 + 2 * registers];
                //Build outgoing modbus message:
                BuildMessage(address, (byte)4, start, registers, ref message);
                //Send modbus message to Serial Port:
                try
                {
                    sp.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    value = response[3];
                    value <<= 8;
                    value += response[4];
                    modbusStatus = "Read successful";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }

        }
        #endregion
        #region Function  - Read Registers
        public bool SendFc3(byte address, ushort start, ushort registers, ref short[] values)
        {
            //Ensure port is open:
            if (sp.IsOpen)
            {
                //Clear in/out buffers:
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                //Function  request is always  bytes:
                byte[] message = new byte[8];
                //Function  response buffer:
                byte[] response = new byte[5 + 2 * registers];
                //Build outgoing modbus message:
                BuildMessage(address, (byte)3, start, registers, ref message);
                //Send modbus message to Serial Port:
                try
                {
                    sp.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    //Return requested register values:
                    for (int i = 0; i < (response.Length - 5) / 2; i++)
                    {
                        values[i] = response[2 * i + 3];
                        values[i] <<= 8;
                        values[i] += response[2 * i + 4];
                    }
                    modbusStatus = "Read successful";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }

        }
        #endregion
    }

    /// <summary>
    /// Extension of the modbus library for working with TestEquity temperature chambers.
    /// </summary>
    public class TempChamber : Modbus
    {
        public TempChamber(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
            : base(portName, baudRate, databits, parity, stopBits)
        {

        }
        public bool SetTemp(int temperature)
        {
            if (!this.modbusStatus.Contains("success"))
            {
                throw new NullReferenceException("COM Port Error!");
            }
            SendFc6((byte)1, 300, (Int16)(temperature * 10));
            short value = -1;
            SendFc4((byte)1, 300, 1, ref value);
            return (value == temperature * 10);
        }
        public float GetTemp()
        {
            short v = 0;
            SendFc4((byte)1, 100, 1, ref v);
            return (float)v / 10;
        }
        /// <summary>
        /// Wrapper function that allows user to set test chamber at a certain temperature and wait until the value is settled.
        /// </summary>
        /// <param name="temperature">Requested temperature value as an integer. Check out TestRunner.cs for examples</param>
        /// <param name="margin">Acceptable range of deviation from the temperature parameter that will be accepted as 'settled'.</param>
        /// <param name="cancel">Value passed by TestRunenr.cs to cancel operation if user requested to do so.</param>
        /// <param name="log">Output handle where the operation details will be written.</param>
        public void SettleTemp(int temperature, float margin, ref ManualResetEvent cancel, TextWriter log)
        {
            if (SetTemp(temperature))
            {
                bool settled = false;
                Trace.Listeners.Add(new TextWriterTraceListener(log));
                do
                {
                    float value = GetTemp();
                    if (Math.Abs(value - temperature) < margin)
                    {
                        Trace.WriteLine(String.Format("Temperature reached to an acceptable value: {0}         ", value));
                        Trace.WriteLine("Measurements will start in 5 minutes...");
                        #region Display Timer
                        AutoResetEvent autoEvent = new AutoResetEvent(false);
                        int maxcall = 5;
                        // Create an inferred delegate that invokes methods for the timer.
                        Timer timer = new Timer(new TimerCallback((o) =>
                        {
                            AutoResetEvent are = o as AutoResetEvent;
                            Trace.WriteLine(String.Format("Time remaining: {0} minute(s)  \r", maxcall--));
                            if (maxcall < 0)
                                autoEvent.Set();
                        }), autoEvent, 0, 60000);
                        autoEvent.WaitOne(new TimeSpan(0, 0, 5, 0), false);
                        timer.Dispose();
                        #endregion
                        Trace.WriteLine("Starting measurements...");
                        settled = true;
                    }
                    else
                    {
                        //log.Write("Waiting for Temperature Chamber.... Current temperature: {0}\r", GetTemp());
                        if (cancel.WaitOne(TimeSpan.FromMinutes(1))) return; //check every minute
                    }
                } while (!settled);
            }
            else
            {
                throw new IOException("Setting Temperature failed!");
            }
        }
    }
}
    /// <summary>
    /// Simple wrapper around the National Instruments Device class.
    /// Includes some neat functions and takes care of escape characters when writing and reading data.
    /// </summary>
    public class GPIBConnector :IDisposable
    {
        private Device _Gpib;
        public void Connect(Device Gpib)
        {
            this._Gpib = Gpib;
        }
        public void Connect(int gpibAddr, int instrAddr, int instrSecAddr)
        {
            _Gpib = new Device(gpibAddr, (byte)instrAddr, (byte)instrSecAddr);
            _Gpib.DefaultBufferSize = 64;
            _Gpib.IOTimeout = TimeoutValue.None;
        }
        public void SendFromFile(string fileName)
        {
            TextReader tr = null;
            try
            {
                tr = new StreamReader(fileName);
                string line = tr.ReadLine().Trim();
                while (line != null)
                {
                    this.SendData(line);
                    line = tr.ReadLine();
                }
            }
            finally
            {
                if (tr != null) tr.Dispose();
            }
        }
        public void TransferFile(string inputFile, string outputFile)
        {
            if (File.Exists(outputFile))
                throw new IOException("Output File already exists!");
            this.SendData("MMEM:DATA? " + inputFile);
            _Gpib.ReadToFile(outputFile);
        }
        public void Wait()
        {
            _Gpib.Wait(GpibStatusFlags.IOComplete);
        }
        public void SendData(string Data)
        {
            _Gpib.Write(ReplaceCommonEscapeSequences(Data));
        }
        public void ResetDevice()
        {
            this.SendData("*RST\n");
        }
        public string ReadData()
        {
            return RemoveCommonEscapeSequences(_Gpib.ReadString());
        }
        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string RemoveCommonEscapeSequences(string s)
        {
            return s.TrimEnd('\n', '\r');
        }

        #region IDisposable Members

        public void Dispose()
        {
            _Gpib.Dispose();
            _Gpib = null;
        }

        #endregion
    }
