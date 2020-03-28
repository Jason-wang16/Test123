namespace Standard_UI.UI
{
    partial class TabDetect
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
            this.tsmiSelectCircle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSelectEllipse = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSelectRectangle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSelectRevolveRectangle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUnionROI = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClearROI = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStaticDetect = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveRegion = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReadRegion = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHideWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudMinGray = new System.Windows.Forms.NumericUpDown();
            this.nudMaxGray = new System.Windows.Forms.NumericUpDown();
            this.nudMin = new System.Windows.Forms.NumericUpDown();
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.nudNumber = new System.Windows.Forms.NumericUpDown();
            this.dudInterpolate = new System.Windows.Forms.DomainUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveTabParams = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.cmsROI.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinGray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxGray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumber)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
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
            this.tsmiSelectCircle,
            this.tsmiSelectEllipse,
            this.tsmiSelectRectangle,
            this.tsmiSelectRevolveRectangle,
            this.tsmiUnionROI,
            this.tsmiClearROI,
            this.tsmiStaticDetect,
            this.tsmiSaveRegion,
            this.tsmiReadRegion,
            this.tsmiSaveWindow,
            this.tsmiHideWindow});
            this.cmsROI.Name = "cmsROI";
            this.cmsROI.Size = new System.Drawing.Size(243, 340);
            // 
            // tsmiReadImage
            // 
            this.tsmiReadImage.Name = "tsmiReadImage";
            this.tsmiReadImage.Size = new System.Drawing.Size(242, 28);
            this.tsmiReadImage.Text = "读取静态图片";
            this.tsmiReadImage.Click += new System.EventHandler(this.tsmiReadImage_Click);
            // 
            // tsmiSelectCircle
            // 
            this.tsmiSelectCircle.Name = "tsmiSelectCircle";
            this.tsmiSelectCircle.Size = new System.Drawing.Size(242, 28);
            this.tsmiSelectCircle.Text = "框取圆形ROI";
            this.tsmiSelectCircle.Click += new System.EventHandler(this.tsmiSelectCircle_Click);
            // 
            // tsmiSelectEllipse
            // 
            this.tsmiSelectEllipse.Name = "tsmiSelectEllipse";
            this.tsmiSelectEllipse.Size = new System.Drawing.Size(242, 28);
            this.tsmiSelectEllipse.Text = "框取椭圆ROI";
            this.tsmiSelectEllipse.Click += new System.EventHandler(this.tsmiSelectEllipse_Click);
            // 
            // tsmiSelectRectangle
            // 
            this.tsmiSelectRectangle.Name = "tsmiSelectRectangle";
            this.tsmiSelectRectangle.Size = new System.Drawing.Size(242, 28);
            this.tsmiSelectRectangle.Text = "框取矩形ROI";
            this.tsmiSelectRectangle.Click += new System.EventHandler(this.tsmiSelectRectangle_Click);
            // 
            // tsmiSelectRevolveRectangle
            // 
            this.tsmiSelectRevolveRectangle.Name = "tsmiSelectRevolveRectangle";
            this.tsmiSelectRevolveRectangle.Size = new System.Drawing.Size(242, 28);
            this.tsmiSelectRevolveRectangle.Text = "框取旋转矩形ROI";
            this.tsmiSelectRevolveRectangle.Click += new System.EventHandler(this.tsmiSelectRevolveRectangle_Click);
            // 
            // tsmiUnionROI
            // 
            this.tsmiUnionROI.Name = "tsmiUnionROI";
            this.tsmiUnionROI.Size = new System.Drawing.Size(242, 28);
            this.tsmiUnionROI.Text = "合并ROI";
            this.tsmiUnionROI.Click += new System.EventHandler(this.tsmiUnionROI_Click);
            // 
            // tsmiClearROI
            // 
            this.tsmiClearROI.Name = "tsmiClearROI";
            this.tsmiClearROI.Size = new System.Drawing.Size(242, 28);
            this.tsmiClearROI.Text = "清除ROI";
            this.tsmiClearROI.Click += new System.EventHandler(this.tsmiClearROI_Click);
            // 
            // tsmiStaticDetect
            // 
            this.tsmiStaticDetect.Name = "tsmiStaticDetect";
            this.tsmiStaticDetect.Size = new System.Drawing.Size(242, 28);
            this.tsmiStaticDetect.Text = "检测静态图片";
            this.tsmiStaticDetect.Click += new System.EventHandler(this.tsmiStaticDetect_Click);
            // 
            // tsmiSaveRegion
            // 
            this.tsmiSaveRegion.Name = "tsmiSaveRegion";
            this.tsmiSaveRegion.Size = new System.Drawing.Size(242, 28);
            this.tsmiSaveRegion.Text = "保存区域到指定路径";
            this.tsmiSaveRegion.Click += new System.EventHandler(this.tsmiSaveRegion_Click);
            // 
            // tsmiReadRegion
            // 
            this.tsmiReadRegion.Name = "tsmiReadRegion";
            this.tsmiReadRegion.Size = new System.Drawing.Size(242, 28);
            this.tsmiReadRegion.Text = "读取区域文件";
            this.tsmiReadRegion.Click += new System.EventHandler(this.tsmiReadRegion_Click);
            // 
            // tsmiSaveWindow
            // 
            this.tsmiSaveWindow.Name = "tsmiSaveWindow";
            this.tsmiSaveWindow.Size = new System.Drawing.Size(242, 28);
            this.tsmiSaveWindow.Text = "保存窗口";
            this.tsmiSaveWindow.Click += new System.EventHandler(this.tsmiSaveWindow_Click);
            // 
            // tsmiHideWindow
            // 
            this.tsmiHideWindow.Name = "tsmiHideWindow";
            this.tsmiHideWindow.Size = new System.Drawing.Size(242, 28);
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
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(280, 541);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "常规参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.nudMinGray, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.nudMaxGray, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.nudMin, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.nudMax, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.nudNumber, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.dudInterpolate, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 15;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.666667F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(274, 535);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "二值化最小值";
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
            this.label2.Text = "二值化最大值";
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
            this.label3.Text = "最小面积";
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
            this.label4.Text = "最大面积";
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
            this.label5.Text = "最小匹配个数";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudMinGray
            // 
            this.nudMinGray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMinGray.Location = new System.Drawing.Point(167, 3);
            this.nudMinGray.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMinGray.Name = "nudMinGray";
            this.nudMinGray.Size = new System.Drawing.Size(104, 28);
            this.nudMinGray.TabIndex = 5;
            this.nudMinGray.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinGray.ValueChanged += new System.EventHandler(this.nudMinGray_ValueChanged);
            // 
            // nudMaxGray
            // 
            this.nudMaxGray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMaxGray.Location = new System.Drawing.Point(167, 38);
            this.nudMaxGray.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMaxGray.Name = "nudMaxGray";
            this.nudMaxGray.Size = new System.Drawing.Size(104, 28);
            this.nudMaxGray.TabIndex = 6;
            this.nudMaxGray.ValueChanged += new System.EventHandler(this.nudMaxGray_ValueChanged);
            // 
            // nudMin
            // 
            this.nudMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMin.Location = new System.Drawing.Point(167, 73);
            this.nudMin.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudMin.Name = "nudMin";
            this.nudMin.Size = new System.Drawing.Size(104, 28);
            this.nudMin.TabIndex = 7;
            this.nudMin.ValueChanged += new System.EventHandler(this.nudMin_ValueChanged);
            // 
            // nudMax
            // 
            this.nudMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMax.Location = new System.Drawing.Point(167, 108);
            this.nudMax.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudMax.Name = "nudMax";
            this.nudMax.Size = new System.Drawing.Size(104, 28);
            this.nudMax.TabIndex = 8;
            this.nudMax.ValueChanged += new System.EventHandler(this.nudMax_ValueChanged);
            // 
            // nudNumber
            // 
            this.nudNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudNumber.Location = new System.Drawing.Point(167, 143);
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
            this.nudNumber.TabIndex = 9;
            this.nudNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumber.ValueChanged += new System.EventHandler(this.nudNumber_ValueChanged);
            // 
            // dudInterpolate
            // 
            this.dudInterpolate.Items.Add("constant");
            this.dudInterpolate.Items.Add("nearest_neighbor");
            this.dudInterpolate.Location = new System.Drawing.Point(167, 178);
            this.dudInterpolate.Name = "dudInterpolate";
            this.dudInterpolate.Size = new System.Drawing.Size(104, 28);
            this.dudInterpolate.TabIndex = 10;
            this.dudInterpolate.Text = "domainUpDown1";
            this.dudInterpolate.SelectedItemChanged += new System.EventHandler(this.dudInterpolate_SelectedItemChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 35);
            this.label6.TabIndex = 11;
            this.label6.Text = "插值方法";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.btnSaveTabParams, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(687, 582);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(288, 59);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // btnSaveTabParams
            // 
            this.btnSaveTabParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveTabParams.Location = new System.Drawing.Point(60, 4);
            this.btnSaveTabParams.Name = "btnSaveTabParams";
            this.btnSaveTabParams.Size = new System.Drawing.Size(166, 50);
            this.btnSaveTabParams.TabIndex = 0;
            this.btnSaveTabParams.Text = "保存检测参数";
            this.btnSaveTabParams.UseVisualStyleBackColor = true;
            this.btnSaveTabParams.Click += new System.EventHandler(this.btnSaveTabParams_Click);
            // 
            // TabDetect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 644);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TabDetect";
            this.Text = "极耳检测";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.cmsROI.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinGray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxGray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumber)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnSaveTabParams;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudMinGray;
        private System.Windows.Forms.NumericUpDown nudMaxGray;
        private System.Windows.Forms.NumericUpDown nudMin;
        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.NumericUpDown nudNumber;
        private System.Windows.Forms.ContextMenuStrip cmsROI;
        private System.Windows.Forms.DomainUpDown dudInterpolate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem tsmiReadImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectCircle;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectEllipse;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectRectangle;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectRevolveRectangle;
        private System.Windows.Forms.ToolStripMenuItem tsmiUnionROI;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearROI;
        private System.Windows.Forms.ToolStripMenuItem tsmiStaticDetect;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveRegion;
        private System.Windows.Forms.ToolStripMenuItem tsmiReadRegion;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveWindow;
        private System.Windows.Forms.ToolStripMenuItem tsmiHideWindow;
    }
}