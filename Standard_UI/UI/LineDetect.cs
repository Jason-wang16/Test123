using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace Standard_UI.UI
{
    public partial class LineDetect : Form
    {
        HTuple hv_StartX;
        HTuple hv_StartY;
        HTuple hv_ZoomFactor;
        HTuple hv_ImageWindow;
        HTuple hv_XldHomMat2D;

        public LineParams lineParams;

        public LineDetect()
        {
            InitializeComponent();

            this.ControlBox = false;

            hv_StartX = 0;
            hv_StartY = 0;
            hv_ZoomFactor = 1;
            hv_ImageWindow = hWindowControl1.HalconID;
            HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);

            lineParams = new LineParams();

            Initialize();
        }

        private void Initialize()
        {
            try
            {
                nudMeasureLength1.Value = lineParams.hv_MeasureLength1;
                nudMeasureLength2.Value = lineParams.hv_MeasureLength2;
                nudMeasureSigma.Value = Convert.ToDecimal(lineParams.hv_MeasureSigma.ToString());
                nudMeasureThreshold.Value = lineParams.hv_MeasureThreshold;

                dudGenParamName.Text = lineParams.hv_GenParamName;
                dudGenParamValue.Text = lineParams.hv_GenParamValue;

                nudModel.Value = lineParams.hv_Model;
                nudRegionMin.Value = lineParams.hv_RegionMin;
                nudRegionMax.Value = lineParams.hv_RegionMax;
            }
            catch (Exception exc)
            {
                MessageBox.Show("初始化参数失败！" + exc.ToString());
            }
        }

        private bool FindCorner()
        {
            if (lineParams.ho_Image == null)
            {
                MessageBox.Show("图像为空");
                return false;
            }
            try
            {
                Show2HWindow(lineParams.ho_Image);
                HOperatorSet.ApplyMetrologyModel(lineParams.ho_Image, lineParams.hv_MetrologyHandle);

                HOperatorSet.GetMetrologyObjectResult(lineParams.hv_MetrologyHandle, "all", "all", "result_type", "row_begin", out lineParams.hv_LineRowBegin);
                HOperatorSet.GetMetrologyObjectResult(lineParams.hv_MetrologyHandle, "all", "all", "result_type", "column_begin", out lineParams.hv_LineColumnBegin);
                HOperatorSet.GetMetrologyObjectResult(lineParams.hv_MetrologyHandle, "all", "all", "result_type", "row_end", out lineParams.hv_LineRowEnd);
                HOperatorSet.GetMetrologyObjectResult(lineParams.hv_MetrologyHandle, "all", "all", "result_type", "column_end", out lineParams.hv_LineColumnEnd);

                if (lineParams.hv_LineRowBegin.Length != 1)
                {
                    MessageBox.Show("直线数量错误！");
                    return false;
                }

                HObject ho_MeasuredLine;
                HOperatorSet.GenEmptyObj(out ho_MeasuredLine);
                HOperatorSet.GetMetrologyObjectResultContour(out ho_MeasuredLine, lineParams.hv_MetrologyHandle, "all", "all", 1.5);

                HOperatorSet.AffineTransContourXld(ho_MeasuredLine, out ho_MeasuredLine, hv_XldHomMat2D);

                HOperatorSet.SetColor(hv_ImageWindow, "red");
                HOperatorSet.DispObj(ho_MeasuredLine, hv_ImageWindow);

                MessageBox.Show("查找直线模型成功！");
                return true;
            }
            catch (Exception exc)
            {
                MessageBox.Show("查找直线模型失败！" + exc.ToString());
                return false;
            }
        }

        private void Show2HWindow(HObject ho_HObject)
        {
            //缩放显示图像，并且计算缩放比例和窗口起始位置
            HOperatorSet.ClearWindow(hv_ImageWindow);
            HTuple hv_Width = new HTuple();
            HTuple hv_Height = new HTuple();
            HOperatorSet.GetImageSize(ho_HObject, out hv_Width, out hv_Height);
            int imgWidth = int.Parse(hv_Width.ToString());
            int imgHeight = int.Parse(hv_Height.ToString());
            double imgAspectRatio = (double)imgWidth / (double)imgHeight;
            int winWidth = this.hWindowControl1.WindowSize.Width;
            int winHeight = this.hWindowControl1.WindowSize.Height;
            double winAspectRatio = (double)winWidth / (double)winHeight;

            HOperatorSet.SetSystem("int_zooming", "false");//图像缩放之前最好将此参数设置为false.

            HTuple hv_Para = new HTuple("constant");
            HObject ho_ZoomImage;
            HOperatorSet.GenEmptyObj(out ho_ZoomImage);

            if (winWidth < imgWidth && imgAspectRatio > winAspectRatio)
            {
                //超宽图像               
                HOperatorSet.ZoomImageSize(ho_HObject, out ho_ZoomImage, winWidth, winWidth / imgAspectRatio, hv_Para);
                hv_StartX = 0 - (winHeight - winWidth / imgAspectRatio) / 2;
                hv_StartY = 0;
                hv_ZoomFactor = (double)winWidth / (double)imgWidth;
            }
            else if (winHeight < imgHeight && imgAspectRatio < winAspectRatio)
            {
                //超高图像                
                HOperatorSet.ZoomImageSize(ho_HObject, out ho_ZoomImage, winHeight * imgAspectRatio, winHeight, hv_Para);
                hv_StartX = 0;
                hv_StartY = 0 - (winWidth - winHeight * imgAspectRatio) / 2;
                hv_ZoomFactor = (double)winHeight / (double)imgHeight;
            }

            try
            {
                HOperatorSet.SetPart(hv_ImageWindow, hv_StartX, hv_StartY, winHeight - 1 + hv_StartX, winWidth - 1 + hv_StartY);//设置居中显示
                HOperatorSet.DispObj(ho_ZoomImage, hv_ImageWindow);

                HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);
                HOperatorSet.HomMat2dScale(hv_XldHomMat2D, hv_ZoomFactor, hv_ZoomFactor, 0, 0, out hv_XldHomMat2D);
            }
            catch (Exception exc)
            {
                MessageBox.Show("图像显示错误！" + exc.ToString());
            }
        }

        private void tsmiReadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "图片|*.bmp;*.jpg;*.png";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.ReadImage(out lineParams.ho_Image, filePath);
                    Show2HWindow(lineParams.ho_Image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取静态图像失败！" + exc.ToString());
                }
            }
        }

        private void tsmiSelectLine_Click(object sender, EventArgs e)
        {
            HTuple hv_Row1 = null;
            HTuple hv_Column1 = null;
            HTuple hv_Row2 = null;
            HTuple hv_Column2 = null;

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawLine(hv_ImageWindow, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);

            hv_Row1 = hv_Row1 / hv_ZoomFactor;
            hv_Column1 = hv_Column1 / hv_ZoomFactor;
            hv_Row2 = hv_Row2 / hv_ZoomFactor;
            hv_Column2 = hv_Column2 / hv_ZoomFactor;

            lineParams.hv_Row1 = hv_Row1;
            lineParams.hv_Column1= hv_Column1;
            lineParams.hv_Row2 = hv_Row2;
            lineParams.hv_Column2 = hv_Column2;

            MessageBox.Show("绘制水平直线成功！");
        }

        private void tsmiStaticDetect_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "图片|*.bmp;*.jpg;*.png";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.ReadImage(out lineParams.ho_Image, filePath);

                    FindCorner();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("静态检测失败！" + exc.ToString());
                }
            }
        }

        private void tsmiHideWindow_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSaveLineParams_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.ClearMetrologyModel(lineParams.hv_MetrologyHandle);
                HOperatorSet.CreateMetrologyModel(out lineParams.hv_MetrologyHandle);
                if (lineParams.ho_Image == null)
                {
                    MessageBox.Show("图像为空！");
                    return;
                }

                HOperatorSet.GetImageSize(lineParams.ho_Image, out lineParams.hv_Width, out lineParams.hv_Height);
                HOperatorSet.SetMetrologyModelImageSize(lineParams.hv_MetrologyHandle, lineParams.hv_Width, lineParams.hv_Height);

                if (lineParams.hv_Row1.Length != 1)
                {
                    MessageBox.Show("直线模型错误！");
                    return;
                }

                HOperatorSet.AddMetrologyObjectLineMeasure(lineParams.hv_MetrologyHandle, lineParams.hv_Row1, lineParams.hv_Column1, lineParams.hv_Row2, lineParams.hv_Column2,
                    lineParams.hv_MeasureLength1, lineParams.hv_MeasureLength2, lineParams.hv_MeasureSigma, lineParams.hv_MeasureThreshold,
                    lineParams.hv_GenParamName, lineParams.hv_GenParamValue, out lineParams.hv_Index);


                if (!FindCorner())
                {
                    MessageBox.Show("直线模型查找失败！");
                    return;
                }

                MessageBox.Show("保存直线模型成功！");
            }
            catch (Exception exc)
            {
                MessageBox.Show("保存直线模型失败！" + exc.ToString());
            }
        }

        private void nudMeasureLength1_ValueChanged(object sender, EventArgs e)
        {
            lineParams.hv_MeasureLength1 = Convert.ToInt32(nudMeasureLength1.Value);
        }

        private void nudMeasureLength2_ValueChanged(object sender, EventArgs e)
        {
            lineParams.hv_MeasureLength2 = Convert.ToInt32(nudMeasureLength2.Value);
        }

        private void nudMeasureSigma_ValueChanged(object sender, EventArgs e)
        {
            lineParams.hv_MeasureSigma = Convert.ToDouble(nudMeasureSigma.Value);
        }

        private void nudMeasureThreshold_ValueChanged(object sender, EventArgs e)
        {
            lineParams.hv_MeasureThreshold = Convert.ToInt32(nudMeasureThreshold.Value);
        }

        private void dudGenParamName_SelectedItemChanged(object sender, EventArgs e)
        {
            lineParams.hv_GenParamName = dudGenParamName.Text;
        }

        private void dudGenParamValue_SelectedItemChanged(object sender, EventArgs e)
        {
            lineParams.hv_GenParamValue = dudGenParamValue.Text;
        }

        private void nudModel_ValueChanged(object sender, EventArgs e)
        {
            lineParams.hv_Model = Convert.ToInt32(nudModel.Value);
        }

        private void nudRegionMin_ValueChanged(object sender, EventArgs e)
        {
            lineParams.hv_RegionMin = Convert.ToInt32(nudRegionMin.Value);
        }

        private void nudRegionMax_ValueChanged(object sender, EventArgs e)
        {
            lineParams.hv_RegionMax = Convert.ToInt32(nudRegionMax.Value);
        }
    }
}
