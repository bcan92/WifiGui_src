namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkQuick = new System.Windows.Forms.CheckBox();
            this.comboModel = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboMods = new System.Windows.Forms.ComboBox();
            this.rbcustom = new System.Windows.Forms.RadioButton();
            this.txtChanList = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbrtte = new System.Windows.Forms.RadioButton();
            this.rbfcc = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numIndex = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.txtGraph = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtResFile = new System.Windows.Forms.TextBox();
            this.txtScriptFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numspaddr = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtVoltRange = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numdcAddr = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtCOM = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtTempRange = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numspaddr)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numdcAddr)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkQuick);
            this.groupBox1.Controls.Add(this.comboModel);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboMods);
            this.groupBox1.Controls.Add(this.rbcustom);
            this.groupBox1.Controls.Add(this.txtChanList);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rbrtte);
            this.groupBox1.Controls.Add(this.rbfcc);
            this.groupBox1.Location = new System.Drawing.Point(11, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(423, 193);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wi-fi Setup";
            // 
            // chkQuick
            // 
            this.chkQuick.AutoSize = true;
            this.chkQuick.Location = new System.Drawing.Point(248, 30);
            this.chkQuick.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkQuick.Name = "chkQuick";
            this.chkQuick.Size = new System.Drawing.Size(98, 21);
            this.chkQuick.TabIndex = 9;
            this.chkQuick.Text = "Quick Test";
            this.chkQuick.UseVisualStyleBackColor = true;
            // 
            // comboModel
            // 
            this.comboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboModel.FormattingEnabled = true;
            this.comboModel.Items.AddRange(new object[] {
            "ti1273",
            "ti1283",
            "bcm",
            "other"});
            this.comboModel.Location = new System.Drawing.Point(100, 122);
            this.comboModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboModel.Name = "comboModel";
            this.comboModel.Size = new System.Drawing.Size(137, 24);
            this.comboModel.TabIndex = 8;
            this.toolTip1.SetToolTip(this.comboModel, "Select the WLAN  Testing Procedure.");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(40, 126);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 17);
            this.label11.TabIndex = 7;
            this.label11.Text = "Device:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Modulation:";
            // 
            // comboMods
            // 
            this.comboMods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMods.FormattingEnabled = true;
            this.comboMods.Items.AddRange(new object[] {
            "802.11b",
            "802.11bgn",
            "802.11a",
            "802.11n",
            "802.11an"});
            this.comboMods.Location = new System.Drawing.Point(100, 26);
            this.comboMods.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboMods.Name = "comboMods";
            this.comboMods.Size = new System.Drawing.Size(137, 24);
            this.comboMods.TabIndex = 5;
            this.toolTip1.SetToolTip(this.comboMods, "Select the WLAN Technologies to be tested.");
            // 
            // rbcustom
            // 
            this.rbcustom.AutoSize = true;
            this.rbcustom.Checked = true;
            this.rbcustom.Location = new System.Drawing.Point(327, 59);
            this.rbcustom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbcustom.Name = "rbcustom";
            this.rbcustom.Size = new System.Drawing.Size(76, 21);
            this.rbcustom.TabIndex = 4;
            this.rbcustom.TabStop = true;
            this.rbcustom.Text = "Custom";
            this.toolTip1.SetToolTip(this.rbcustom, "Use custom channel range.");
            this.rbcustom.UseVisualStyleBackColor = true;
            this.rbcustom.CheckedChanged += new System.EventHandler(this.rbcustom_CheckedChanged);
            // 
            // txtChanList
            // 
            this.txtChanList.Location = new System.Drawing.Point(100, 59);
            this.txtChanList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtChanList.Multiline = true;
            this.txtChanList.Name = "txtChanList";
            this.txtChanList.Size = new System.Drawing.Size(137, 48);
            this.txtChanList.TabIndex = 3;
            this.txtChanList.Text = "1, 6, 11";
            this.toolTip1.SetToolTip(this.txtChanList, "Channels used for testing.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Channels:";
            // 
            // rbrtte
            // 
            this.rbrtte.AutoSize = true;
            this.rbrtte.Location = new System.Drawing.Point(247, 87);
            this.rbrtte.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbrtte.Name = "rbrtte";
            this.rbrtte.Size = new System.Drawing.Size(75, 21);
            this.rbrtte.TabIndex = 1;
            this.rbrtte.Text = "R&&TTE";
            this.toolTip1.SetToolTip(this.rbrtte, "Use channels required by the R&TTE standards");
            this.rbrtte.UseVisualStyleBackColor = true;
            // 
            // rbfcc
            // 
            this.rbfcc.AutoSize = true;
            this.rbfcc.Location = new System.Drawing.Point(247, 59);
            this.rbfcc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbfcc.Name = "rbfcc";
            this.rbfcc.Size = new System.Drawing.Size(55, 21);
            this.rbfcc.TabIndex = 0;
            this.rbfcc.Text = "FCC";
            this.toolTip1.SetToolTip(this.rbfcc, "Use channels required by the FCC standards");
            this.rbfcc.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numIndex);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtGraph);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtResFile);
            this.groupBox2.Controls.Add(this.txtScriptFile);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numspaddr);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(8, 208);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(423, 190);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "IO Settings";
            // 
            // numIndex
            // 
            this.numIndex.Location = new System.Drawing.Point(115, 153);
            this.numIndex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numIndex.Name = "numIndex";
            this.numIndex.Size = new System.Drawing.Size(51, 22);
            this.numIndex.TabIndex = 9;
            this.toolTip1.SetToolTip(this.numIndex, "Index for the first test image, if image save is enabled in the script file.");
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 155);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 17);
            this.label12.TabIndex = 8;
            this.label12.Text = "Image Index:";
            // 
            // txtGraph
            // 
            this.txtGraph.Location = new System.Drawing.Point(115, 119);
            this.txtGraph.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGraph.Name = "txtGraph";
            this.txtGraph.Size = new System.Drawing.Size(268, 22);
            this.txtGraph.TabIndex = 7;
            this.txtGraph.Text = "F:\\livspurem";
            this.toolTip1.SetToolTip(this.txtGraph, "Folder path on test equipment for saving images.\r\nPath name should be accessible " +
        "on the test equipment.");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 123);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Save Graphs:";
            // 
            // txtResFile
            // 
            this.txtResFile.Location = new System.Drawing.Point(115, 87);
            this.txtResFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtResFile.Name = "txtResFile";
            this.txtResFile.Size = new System.Drawing.Size(268, 22);
            this.txtResFile.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtResFile, "Location to save the results on the Local PC.\r\nPath name should be accessible on " +
        "this PC.");
            this.txtResFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtResFile_MouseClick);
            // 
            // txtScriptFile
            // 
            this.txtScriptFile.Location = new System.Drawing.Point(115, 55);
            this.txtScriptFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtScriptFile.Name = "txtScriptFile";
            this.txtScriptFile.Size = new System.Drawing.Size(268, 22);
            this.txtScriptFile.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtScriptFile, "Script file to be used by the tesing equipment (Spectrum Analyzer).");
            this.txtScriptFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtScriptFile_Clicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 91);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Save Results:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 59);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Script File:";
            // 
            // numspaddr
            // 
            this.numspaddr.Location = new System.Drawing.Point(128, 22);
            this.numspaddr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numspaddr.Name = "numspaddr";
            this.numspaddr.Size = new System.Drawing.Size(65, 22);
            this.numspaddr.TabIndex = 1;
            this.toolTip1.SetToolTip(this.numspaddr, "Primary GPIB Address of the Spectrum Analyzer.");
            this.numspaddr.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(9, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "FSV GPIB Addr:";
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(813, 469);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnStop);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(805, 437);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setup";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Image = global::WindowsFormsApplication1.Properties.Resources.stop_red;
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStop.Location = new System.Drawing.Point(116, 405);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 28);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.progressBar1);
            this.groupBox6.Controls.Add(this.lblStatus);
            this.groupBox6.Location = new System.Drawing.Point(439, 322);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Size = new System.Drawing.Size(357, 75);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Test Status";
            this.toolTip1.SetToolTip(this.groupBox6, "Current test step and overall progress are displayed here.");
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 41);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(343, 27);
            this.progressBar1.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(7, 20);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(112, 17);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "No tests running";
            // 
            // button1
            // 
            this.button1.Image = global::WindowsFormsApplication1.Properties.Resources.play_green;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(8, 405);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnLoad);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(441, 7);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(356, 308);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Other Instruments";
            // 
            // btnLoad
            // 
            this.btnLoad.Image = global::WindowsFormsApplication1.Properties.Resources.disk_upload;
            this.btnLoad.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnLoad.Location = new System.Drawing.Point(131, 267);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 28);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.toolTip1.SetToolTip(this.btnLoad, "Load an existing test config file (.bin).");
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::WindowsFormsApplication1.Properties.Resources.disk_download;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnSave.Location = new System.Drawing.Point(12, 267);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.toolTip1.SetToolTip(this.btnSave, "Save the current test configuration as a .bin file.");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtVoltRange);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.numdcAddr);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(12, 137);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(340, 123);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "DC Power Supply";
            // 
            // txtVoltRange
            // 
            this.txtVoltRange.Location = new System.Drawing.Point(96, 66);
            this.txtVoltRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVoltRange.Name = "txtVoltRange";
            this.txtVoltRange.Size = new System.Drawing.Size(139, 22);
            this.txtVoltRange.TabIndex = 8;
            this.txtVoltRange.Text = "3.6, 3.8, 4.35";
            this.toolTip1.SetToolTip(this.txtVoltRange, "Individual voltage values to be used in the test.\r\nLeave blank if the power suppl" +
        "y is not needed");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(31, 70);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 17);
            this.label10.TabIndex = 7;
            this.label10.Text = "Range:";
            // 
            // numdcAddr
            // 
            this.numdcAddr.Location = new System.Drawing.Point(96, 34);
            this.numdcAddr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numdcAddr.Name = "numdcAddr";
            this.numdcAddr.Size = new System.Drawing.Size(65, 22);
            this.numdcAddr.TabIndex = 3;
            this.toolTip1.SetToolTip(this.numdcAddr, "Primary GPIB Address of the Power Supply.");
            this.numdcAddr.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 37);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 17);
            this.label8.TabIndex = 2;
            this.label8.Text = "GPIB Addr:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.txtCOM);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.txtTempRange);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Location = new System.Drawing.Point(8, 25);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(340, 103);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Temperature Chamber";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(173, 28);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 26);
            this.button2.TabIndex = 9;
            this.button2.Text = "Port Settings..";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // txtCOM
            // 
            this.txtCOM.Location = new System.Drawing.Point(83, 28);
            this.txtCOM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCOM.Name = "txtCOM";
            this.txtCOM.Size = new System.Drawing.Size(81, 22);
            this.txtCOM.TabIndex = 8;
            this.txtCOM.Text = "COM1";
            this.toolTip1.SetToolTip(this.txtCOM, "Name of the COM Port used by the Temperature Chamber.");
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(231, 65);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(98, 21);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Full Range";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // txtTempRange
            // 
            this.txtTempRange.Location = new System.Drawing.Point(83, 63);
            this.txtTempRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTempRange.Name = "txtTempRange";
            this.txtTempRange.Size = new System.Drawing.Size(139, 22);
            this.txtTempRange.TabIndex = 6;
            this.txtTempRange.Text = "20,-20,55";
            this.toolTip1.SetToolTip(this.txtTempRange, "Individual temperature values to be used in the test.\r\nLeave blank if the tempera" +
        "ture chamber is not needed.");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 66);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 17);
            this.label9.TabIndex = 5;
            this.label9.Text = "Range:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 32);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "COM Port:";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 469);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Test Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numspaddr)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numdcAddr)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbcustom;
        private System.Windows.Forms.TextBox txtChanList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbrtte;
        private System.Windows.Forms.RadioButton rbfcc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboMods;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numspaddr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGraph;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtResFile;
        private System.Windows.Forms.TextBox txtScriptFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown numdcAddr;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox txtTempRange;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtVoltRange;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtCOM;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboModel;
        private System.Windows.Forms.Button button2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkQuick;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.NumericUpDown numIndex;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnStop;
    }
}

