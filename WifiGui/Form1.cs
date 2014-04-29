using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using PhoneConnections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private TestRunner tsrunner { get; set; }

        public Form1()
        {
            InitializeComponent();
            comboMods.SelectedIndex = 1;
            comboModel.SelectedIndex = 0;
            txtChanList.Enabled = rbcustom.Checked;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void txtScriptFile_Clicked(object sender, MouseEventArgs e)
        {
            using (OpenFileDialog opf = new OpenFileDialog())
            {
                opf.Multiselect = false;
                opf.CheckFileExists = true;
                opf.Title = "Please select a FSV GPIB Command File";
                opf.Filter = "Script files|*.txt;*.cfg";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    (sender as TextBox).Text = opf.FileName;
                }
            }
        }
        private void txtResFile_MouseClick(object sender, MouseEventArgs e)
        {
            using (SaveFileDialog svf = new SaveFileDialog())
            {
                svf.Title = "Please select a location for the results";
                svf.Filter = "CSV files|*.csv";
                if (svf.ShowDialog() == DialogResult.OK)
                {
                    (sender as TextBox).Text = svf.FileName;
                }
            }
        }

        private void rbcustom_CheckedChanged(object sender, EventArgs e)
        {
            txtChanList.Enabled = rbcustom.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestSetup ts = GetUIData();
            if (ts != null)
            {
                tsrunner = new TestRunner(GetUIData());
                //calculate the number of test cases
                int testcases = Math.Max(ts.temp.Length, 1) * Math.Max(ts.volt.Length, 1) * ts.channel.Length * ts.mod.Length * (ts.quick ? 1 : 3) + 1; //3 for data rate!
                progressBar1.Minimum = 0;
                progressBar1.Maximum = testcases;
                progressBar1.Value = 0;
                progressBar1.Step = 1;
                lblStatus.Text = "Preparing for test...";
                backgroundWorker1.RunWorkerAsync(tsrunner);
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            //if (backgroundWorker1.IsBusy) backgroundWorker1.CancelAsync();
            if (tsrunner != null) tsrunner.StopTest();
        }

        private TestSetup GetUIData()
        {
            TestSetup ts = new TestSetup();
            ts.spaddr = (int)numspaddr.Value;
            //Channels
            #region channels
            List<int> chan = new List<int>(8);
            if (rbcustom.Checked)
            {
                try
                {
                    chan.AddRange(txtChanList.Text.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries).
                        Select(s => int.Parse(s.Trim())));
                }
                catch
                {
                    MessageBox.Show("Wrong format for channel!"); return null;
                }
            }
            else if (rbrtte.Checked)
            {
                chan.AddRange(new int[] { 36, 64, 100, 140 });
            }
            else
            {
                chan.AddRange(new int[] { 36, 44, 48, 52, 60, 64, 100, 140, 149, 157, 161, 165 });
            }
            #endregion
            ts.channel = chan.ToArray();
            ts.comport = txtCOM.Text;
            ts.dcaddr = (int)numdcAddr.Value;
            ts.savePicLoc = txtGraph.Text;
            ts.scriptLoc = txtScriptFile.Text;
            ts.logLoc = txtResFile.Text;
            ts.imgindex = (int)numIndex.Value;
            ts.mod = comboMods.Text.ToCharArray().Where(c => char.IsLetter(c)).ToArray(); //lol
            ts.phonemodel = comboModel.Items[comboModel.SelectedIndex].ToString();
            ts.quick = chkQuick.Checked;
            #region temperature
            int[] temperature;
            if (checkBox1.Checked)
                temperature = new int[] { -30, -20, -10, 0, 10, 20, 30, 40, 50, 60 };
            else
            {
                try
                {
                    temperature = txtTempRange.Text.Split(new char[] { ',' },
                            StringSplitOptions.RemoveEmptyEntries).
                            Select(s => int.Parse(s.Trim())).ToArray();
                }
                catch
                {
                    MessageBox.Show("Wrong format for temperature!"); return null;
                }
            }
            ts.temp = temperature;
            #endregion
            ts.volt = txtVoltRange.Text.Split(new char[] { ',' },
                            StringSplitOptions.RemoveEmptyEntries).
                            Select(s => float.Parse(s.Trim())).ToArray();
            //
            return ts;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.txtTempRange.Enabled = !checkBox1.Checked;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            TestRunner t = e.Argument as TestRunner;
            t.RunTest(backgroundWorker1);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Config Files|*.bin";
            sf.Title = "Save Config";
            //
            DialogResult dr = sf.ShowDialog();
            Stream st = null;
            if (dr == DialogResult.OK)
            {
                st = File.Open(sf.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                BinaryFormatter xsl = new BinaryFormatter();
                try
                {
                    xsl.Serialize(st, GetUIData());
                } 
                catch (SerializationException ex)
                {
                    MessageBox.Show("Failed to serialize. Reason: " + ex.Message);
                    
                }
            }
            if (st != null) st.Close();
            sf.Dispose();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Config Files|*.bin";
            opf.Title = "Load Config";
            DialogResult dr = opf.ShowDialog();
            Stream st = null;
            if (dr == DialogResult.OK)
            {
                st = File.Open(opf.FileName, FileMode.Open);
                BinaryFormatter bnf = new BinaryFormatter();
                TestSetup ts;
                try
                {
                    ts = bnf.Deserialize(st) as TestSetup;
                    //Fill UI
                    rbcustom.Checked = true;
                    txtChanList.Text = String.Join(",", ts.channel.Select(i => i.ToString()).ToArray());
                    txtCOM.Text = ts.comport;
                    numspaddr.Value = ts.spaddr;
                    numdcAddr.Value = ts.dcaddr;
                    numIndex.Value = ts.imgindex;
                    txtScriptFile.Text = ts.scriptLoc;
                    txtGraph.Text = ts.savePicLoc;
                    txtResFile.Text = ts.logLoc;
                    int j;
                    if((j = comboModel.Items.IndexOf(ts.phonemodel)) > -1)
                    {
                        comboModel.SelectedIndex = j;
                    }
                    if((j = comboMods.Items.IndexOf("802.11" + new string(ts.mod))) > -1)
                    {
                        comboMods.SelectedIndex = j;
                    }
                    txtTempRange.Text = String.Join(",", ts.temp.Select(i => i.ToString()).ToArray());
                    checkBox1.Checked = false;
                    txtVoltRange.Text = String.Join(",", ts.volt.Select(i => i.ToString()).ToArray());
                    chkQuick.Checked = ts.quick;
                }
                catch
                {
                    MessageBox.Show("Invalid or corrupt config file!");
                }
                finally
                {
                    if (st != null) st.Close();
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Use step, percentage 2 is used as a special case where the label needs to be updated only.
            if (e.ProgressPercentage != 2)
            {
                this.progressBar1.PerformStep();
            }
            this.lblStatus.Text = e.UserState.ToString();
        }


    }
}
