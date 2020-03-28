namespace Standard_UI.UI
{
    partial class CornerDetect
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.cmsROI = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiReadImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSelectHorizonLine = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSelectVerticalLine = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClearROI = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStaticDetect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveLineModel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHideWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudMeasureLength1 = new System.Windows.Forms.NumericUpDown();
            this.nudMeasureLength2 = new System.Windows.Forms.NumericUpDown();
            this.nudMeasureSigma = new System.Windows.Forms.NumericUpDown();
            this.nudMeasureThreshold = new System.Windows.Forms.NumericUpDown();
            this.dudGenParamName = new System.Windows.Forms.DomainUpDown();
            this.dudGenParamValue = new System.Windows.Forms.DomainUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nudNumber = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudModel = new System.Windows.Forms.NumericUpDown();
            this.nudRegionMin = new System.Windows.Forms.NumericUpDown();
            this.nudRegionMax = new System.Windows.Forms.NumericUpDown();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveCornerParams = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.cmsROI.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureLength1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureLength2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureSigma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegionMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegionMax)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(978, 644);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ContextMenuStrip = this.cmsROI;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(3, 3);
            this.hWindowControl1.Name = "hWindowControl1";
            this.tableLayoutPanel1.SetRowSpan(this.hWindowControl1, 2);
            this.hWindowControl1.Size = new System.Drawing.Size(678, 638);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(678, 638);
            // 
            // cmsROI
            // 
            this.cmsROI.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsROI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiReadImage,
            this.tsmiSelectHorizonLine,
            this.tsmiSelectVerticalLine,
            this.tsmiClearROI,
            this.tsmiStaticDetect,
            this.tsmiSaveLineModel,
            this.tsmiSaveWindow,
            this.tsmiHideWindow});
            this.cmsROI.Name = "cmsROI";
            this.cmsROI.Size = new System.Drawing.Size(279, 228);
            // 
            // tsmiReadImage
            // 
            this.tsmiReadImage.Name = "tsmiReadImage";
            this.tsmiReadImage.Size = new System.Drawing.Size(278, 28);
            this.tsmiReadImage.Text = "读取静态图片";
            this.tsmiReadImage.Click += new System.EventHandler(this.tsmiReadImage_Click);
            // 
            // tsmiSelectHorizonLine
            // 
            this.tsmiSelectHorizonLine.Name = "tsmiSelectHorizonLine";
            this.tsmiSelectHorizonLine.Size = new System.Drawing.Size(278, 28);
            this.tsmiSelectHorizonLine.Text = "绘制水平直线模型";
            this.tsmiSelectHorizonLine.Click += new System.EventHandler(this.tsmiSelectHorizonLine_Click);
            // 
            // tsmiSelectVerticalLine
            // 
            this.tsmiSelectVerticalLine.Name = "tsmiSelectVerticalLine";
            this.tsmiSelectVerticalLine.Size = new System.Drawing.Size(278, 28);
            this.tsmiSelectVerticalLine.Text = "绘制竖直直线模型";
            this.tsmiSelectVerticalLine.Click += new System.EventHandler(this.tsmiSelectVerticalLine_Click);
            // 
            // tsmiClearROI
            // 
            this.tsmiClearROI.Name = "tsmiClearROI";
            this.tsmiClearROI.Size = new System.Drawing.Size(278, 28);
            this.tsmiClearROI.Text = "清除直线模型";
            this.tsmiClearROI.Click += new System.EventHandler(this.tsmiClearROI_Click);
            // 
            // tsmiStaticDetect
            // 
            this.tsmiStaticDetect.Name = "tsmiStaticDetect";
            this.tsmiStaticDetect.Size = new System.Drawing.Size(278, 28);
            this.tsmiStaticDetect.Text = "检测静态图片";
            this.tsmiStaticDetect.Click += new System.EventHandler(this.tsmiStaticDetect_Click);
            // 
            // tsmiSaveLineModel
            // 
            this.tsmiSaveLineModel.Name = "tsmiSaveLineModel";
            this.tsmiSaveLineModel.Size = new System.Drawing.Size(278, 28);
            this.tsmiSaveLineModel.Text = "保存测量模型到指定路径";
            this.tsmiSaveLineModel.Click += new System.EventHandler(this.tsmiSaveLineModel_Click);
            // 
            // tsmiSaveWindow
            // 
            this.tsmiSaveWindow.Name = "tsmiSaveWindow";
            this.tsmiSaveWindow.Size = new System.Drawing.Size(278, 28);
            this.tsmiSaveWindow.Text = "保存窗口";
            this.tsmiSaveWindow.Click += new System.EventHandler(this.tsmiSaveWindow_Click);
            // 
            // tsmiHideWindow
            // 
            this.tsmiHideWindow.Name = "tsmiHideWindow";
            this.tsmiHideWindow.Size = new System.Drawing.Size(278, 28);
            this.tsmiHideWindow.Text = "隐藏窗口";
            this.tsmiHideWindow.Click += new System.EventHandler(this.tsmiHideWindow_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(687, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(288, 573);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(280, 541);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "常规参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.nudMeasureLength1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.nudMeasureLength2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.nudMeasureSigma, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.nudMeasureThreshold, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.dudGenParamName, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.dudGenParamValue, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.nudNumber, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.nudModel, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.nudRegionMin, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.nudRegionMax, 1, 9);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 15;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(274, 535);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "宽度";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "步长";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 35);
            this.label3.TabIndex = 2;
            this.label3.Text = "平滑（Sigma）";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 35);
            this.label4.TabIndex = 3;
            this.label4.Text = "阈值";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 35);
            this.label5.TabIndex = 4;
            this.label5.Text = "位置";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 35);
            this.label6.TabIndex = 5;
            this.label6.Text = "变换";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudMeasureLength1
            // 
            this.nudMeasureLength1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMeasureLength1.Location = new System.Drawing.Point(167, 3);
            this.nudMeasureLength1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMeasureLength1.Name = "nudMeasureLength1";
            this.nudMeasureLength1.Size = new System.Drawing.Size(104, 28);
            this.nudMeasureLength1.TabIndex = 6;
            this.nudMeasureLength1.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMeasureLength1.ValueChanged += new System.EventHandler(this.nudMeasureLength1_ValueChanged);
            // 
            // nudMeasureLength2
            // 
            this.nudMeasureLength2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMeasureLength2.Location = new System.Drawing.Point(167, 38);
            this.nudMeasureLength2.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudMeasureLength2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeasureLength2.Name = "nudMeasureLength2";
            this.nudMeasureLength2.Size = new System.Drawing.Size(104, 28);
            this.nudMeasureLength2.TabIndex = 7;
            this.nudMeasureLength2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeasureLength2.ValueChanged += new System.EventHandler(this.nudMeasureLength2_ValueChanged);
            // 
            // nudMeasureSigma
            // 
            this.nudMeasureSigma.DecimalPlaces = 1;
            this.nudMeasureSigma.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMeasureSigma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudMeasureSigma.Location = new System.Drawing.Point(167, 73);
            this.nudMeasureSigma.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.nudMeasureSigma.Name = "nudMeasureSigma";
            this.nudMeasureSigma.Size = new System.Drawing.Size(104, 28);
            this.nudMeasureSigma.TabIndex = 8;
            this.nudMeasureSigma.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeasureSigma.ValueChanged += new System.EventHandler(this.nudMeasureSigma_ValueChanged);
            // 
            // nudMeasureThreshold
            // 
            this.nudMeasureThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMeasureThreshold.Location = new System.Drawing.Point(167, 108);
            this.nudMeasureThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMeasureThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMeasureThreshold.Name = "nudMeasureThreshold";
            this.nudMeasureThreshold.Size = new System.Drawing.Size(104, 28);
            this.nudMeasureThreshold.TabIndex = 9;
            this.nudMeasureThreshold.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudMeasureThreshold.ValueChanged += new System.EventHandler(this.nudMeasureThreshold_ValueChanged);
            // 
            // dudGenParamName
            // 
            this.dudGenParamName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dudGenParamName.Items.Add("distance_threshold");
            this.dudGenParamName.Items.Add("instances_outside_measure_regions");
            this.dudGenParamName.Items.Add("max_num_iterations");
            this.dudGenParamName.Items.Add("measure_distance");
            this.dudGenParamName.Items.Add("measure_interpolation");
            this.dudGenParamName.Items.Add("measure_select");
            this.dudGenParamName.Items.Add("measure_transition");
            this.dudGenParamName.Items.Add("min_score\'");
            this.dudGenParamName.Items.Add("num_instances");
            this.dudGenParamName.Items.Add("num_measures");
            this.dudGenParamName.Items.Add("rand_seed");
            this.dudGenParamName.Location = new System.Drawing.Point(167, 143);
            this.dudGenParamName.Name = "dudGenParamName";
            this.dudGenParamName.Size = new System.Drawing.Size(104, 28);
            this.dudGenParamName.TabIndex = 10;
            this.dudGenParamName.Text = "domainUpDown1";
            this.dudGenParamName.SelectedItemChanged += new System.EventHandler(this.dudGenParamName_SelectedItemChanged);
            // 
            // dudGenParamValue
            // 
            this.dudGenParamValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dudGenParamValue.Items.Add("positive");
            this.dudGenParamValue.Items.Add("negative");
            this.dudGenParamValue.Location = new System.Drawing.Point(167, 178);
            this.dudGenParamValue.Name = "dudGenParamValue";
            this.dudGenParamValue.Size = new System.Drawing.Size(104, 28);
            this.dudGenParamValue.TabIndex = 11;
            this.dudGenParamValue.Text = "domainUpDown2";
            this.dudGenParamValue.SelectedItemChanged += new System.EventHandler(this.dudGenParamValue_SelectedItemChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 35);
            this.label7.TabIndex = 12;
            this.label7.Text = "直线数量";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudNumber
            // 
            this.nudNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudNumber.Location = new System.Drawing.Point(167, 213);
            this.nudNumber.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumber.Name = "nudNumber";
            this.nudNumber.Size = new System.Drawing.Size(104, 28);
            this.nudNumber.TabIndex = 13;
            this.nudNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumber.ValueChanged += new System.EventHandler(this.nudNumber_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 245);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(158, 35);
            this.label8.TabIndex = 14;
            this.label8.Text = "模式";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 280);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(158, 35);
            this.label9.TabIndex = 15;
            this.label9.Text = "最小灰度";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 315);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(158, 35);
            this.label10.TabIndex = 16;
            this.label10.Text = "最大灰度";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudModel
            // 
            this.nudModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudModel.Location = new System.Drawing.Point(167, 248);
            this.nudModel.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudModel.Name = "nudModel";
            this.nudModel.Size = new System.Drawing.Size(104, 28);
            this.nudModel.TabIndex = 17;
            this.nudModel.ValueChanged += new System.EventHandler(this.nudModel_ValueChanged);
            // 
            // nudRegionMin
            // 
            this.nudRegionMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudRegionMin.Location = new System.Drawing.Point(167, 283);
            this.nudRegionMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRegionMin.Name = "nudRegionMin";
            this.nudRegionMin.Size = new System.Drawing.Size(104, 28);
            this.nudRegionMin.TabIndex = 18;
            this.nudRegionMin.ValueChanged += new System.EventHandler(this.nudRegionMin_ValueChanged);
            // 
            // nudRegionMax
            // 
            this.nudRegionMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudRegionMax.Location = new System.Drawing.Point(167, 318);
            this.nudRegionMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudRegionMax.Name = "nudRegionMax";
            this.nudRegionMax.Size = new System.Drawing.Size(104, 28);
            this.nudRegionMax.TabIndex = 19;
            this.nudRegionMax.ValueChanged += new System.EventHandler(this.nudRegionMax_ValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(280, 541);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "高级参数";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnSaveCornerParams, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(687, 582);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(288, 59);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btnSaveCornerParams
            // 
            this.btnSaveCornerParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveCornerParams.Location = new System.Drawing.Point(60, 4);
            this.btnSaveCornerParams.Name = "btnSaveCornerParams";
            this.btnSaveCornerParams.Size = new System.Drawing.Size(166, 50);
            this.btnSaveCornerParams.TabIndex = 0;
            this.btnSaveCornerParams.Text = "保存检测参数";
            this.btnSaveCornerParams.UseVisualStyleBackColor = true;
            this.btnSaveCornerParams.Click += new System.EventHandler(this.btnSaveCornerParams_Click);
            // 
            // CornerDetect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 644);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CornerDetect";
            this.Text = "边缘检测2D";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.cmsROI.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureLength1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureLength2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureSigma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeasureThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegionMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegionMax)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSaveCornerParams;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip cmsROI;
        private System.Windows.Forms.ToolStripMenuItem tsmiReadImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectHorizonLine;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearROI;
        private System.Windows.Forms.ToolStripMenuItem tsmiStaticDetect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveWindow;
        private System.Windows.Forms.NumericUpDown nudMeasureLength1;
        private System.Windows.Forms.NumericUpDown nudMeasureLength2;
        private System.Windows.Forms.NumericUpDown nudMeasureSigma;
        private System.Windows.Forms.NumericUpDown nudMeasureThreshold;
        private System.Windows.Forms.DomainUpDown dudGenParamName;
        private System.Windows.Forms.DomainUpDown dudGenParamValue;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveLineModel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudNumber;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectVerticalLine;
        private System.Windows.Forms.ToolStripMenuItem tsmiHideWindow;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudModel;
        private System.Windows.Forms.NumericUpDown nudRegionMin;
        private System.Windows.Forms.NumericUpDown nudRegionMax;
    }
}