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
using System.IO;

namespace Standard_UI.UI
{
    public partial class CornerDetect : Form
    {
        #region 控制参数
        HTuple hv_StartX;
        HTuple hv_StartY;
        HTuple hv_ZoomFactor;
        HTuple hv_ImageWindow;
        HTuple hv_XldHomMat2D;

        public CornerParams cornerParams;
         #endregion
        public CornerDetect()
        {
            InitializeComponent();
            this.ControlBox = false;

            hv_StartX = 0;
            hv_StartY = 0;
            hv_ZoomFactor = 1;
            hv_ImageWindow = hWindowControl1.HalconID;
            HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);

            cornerParams = new CornerParams();

            Initialize();
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

        private bool FindCorner()
        {
            if (cornerParams.ho_Image == null)
            {
                MessageBox.Show("图像为空");
                return false;
            }
            try
            {
                Show2HWindow(cornerParams.ho_Image);
                HOperatorSet.ApplyMetrologyModel(cornerParams.ho_Image, cornerParams.hv_MetrologyHandle);

                HOperatorSet.GetMetrologyObjectResult(cornerParams.hv_MetrologyHandle, "all", "all", "result_type", "row_begin", out cornerParams.hv_LineRowBegin);
                HOperatorSet.GetMetrologyObjectResult(cornerParams.hv_MetrologyHandle, "all", "all", "result_type", "column_begin", out cornerParams.hv_LineColumnBegin);
                HOperatorSet.GetMetrologyObjectResult(cornerParams.hv_MetrologyHandle, "all", "all", "result_type", "row_end", out cornerParams.hv_LineRowEnd);
                HOperatorSet.GetMetrologyObjectResult(cornerParams.hv_MetrologyHandle, "all", "all", "result_type", "column_end", out cornerParams.hv_LineColumnEnd);

                if (cornerParams.hv_LineRowBegin.Length!= cornerParams.hv_Number)
                {
                    MessageBox.Show("直线数量错误！");
                    return false;
                }

                HTuple hv_IsOverlapping = null;
                HOperatorSet.IntersectionLines(cornerParams.hv_LineRowBegin[0], cornerParams.hv_LineColumnBegin[0], cornerParams.hv_LineRowEnd[0], cornerParams.hv_LineColumnEnd[0],
                    cornerParams.hv_LineRowBegin[1], cornerParams.hv_LineColumnBegin[1], cornerParams.hv_LineRowEnd[1], cornerParams.hv_LineColumnEnd[1], out cornerParams.hv_PointRow, out cornerParams.hv_PointColumn,
                    out hv_IsOverlapping);

                if (cornerParams.hv_PointRow.Length != 1)
                {
                    MessageBox.Show("直线交点错误！");
                    return false;
                }
                HObject ho_MeasuredCross;
                HOperatorSet.GenCrossContourXld(out ho_MeasuredCross, cornerParams.hv_PointRow, cornerParams.hv_PointColumn, 40, (new HTuple(45)).TupleRad());//在模板中心产生一个x，表示模板中心

                HObject ho_MeasuredLines;
                HOperatorSet.GenEmptyObj(out ho_MeasuredLines);
                HOperatorSet.GetMetrologyObjectResultContour(out ho_MeasuredLines, cornerParams.hv_MetrologyHandle, "all", "all", 1.5);

                HOperatorSet.AffineTransContourXld(ho_MeasuredLines, out ho_MeasuredLines, hv_XldHomMat2D);
                HOperatorSet.AffineTransContourXld(ho_MeasuredCross, out ho_MeasuredCross, hv_XldHomMat2D);

                HOperatorSet.SetColor(hv_ImageWindow, "red");
                HOperatorSet.DispObj(ho_MeasuredLines, hv_ImageWindow);
                HOperatorSet.DispObj(ho_MeasuredCross, hv_ImageWindow);

                MessageBox.Show("查找直线模型成功！");
                return true;
            }
            catch (Exception exc)
            {
                MessageBox.Show("查找直线模型失败！"+exc.ToString());
                return false;
            }
        }

        private void Initialize()
        {
            try
            {
                nudMeasureLength1.Value = cornerParams.hv_MeasureLength1;
                nudMeasureLength2.Value = cornerParams.hv_MeasureLength2;
                nudMeasureSigma.Value = Convert.ToDecimal(cornerParams.hv_MeasureSigma.ToString());
                nudMeasureThreshold.Value = cornerParams.hv_MeasureThreshold;

                dudGenParamName.Text = cornerParams.hv_GenParamName;
                dudGenParamValue.Text = cornerParams.hv_GenParamValue;
                nudNumber.Value = cornerParams.hv_Number;

                nudModel.Value= cornerParams.hv_Model;
                nudRegionMin.Value = cornerParams.hv_RegionMin;
                nudRegionMax.Value = cornerParams.hv_RegionMax;
            }
            catch (Exception exc)
            {
                MessageBox.Show("初始化参数失败！"+exc.ToString());
            }         
        }

        private void btnSaveCornerParams_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.ClearMetrologyModel(cornerParams.hv_MetrologyHandle);
                HOperatorSet.CreateMetrologyModel(out cornerParams.hv_MetrologyHandle);
                if (cornerParams.ho_Image == null)
                {
                    MessageBox.Show("图像为空！");
                    return;
                }

                HOperatorSet.GetImageSize(cornerParams.ho_Image, out cornerParams.hv_Width, out cornerParams.hv_Height);
                HOperatorSet.SetMetrologyModelImageSize(cornerParams.hv_MetrologyHandle, cornerParams.hv_Width, cornerParams.hv_Height);

                if (cornerParams.hv_Row1_Horizon.Length < 1)
                {
                    MessageBox.Show("水平直线模型为空！");
                    return;
                }

                //if (cornerParams.hv_Row1_Vertical.Length < 1)
                //{
                //    MessageBox.Show("竖直直线模型为空！");
                //    return;
                //}

                HTuple hv_Row1=null;
                HTuple hv_Column1=null;
                HTuple hv_Row2=null;
                HTuple hv_Column2=null;

                HOperatorSet.TupleInsert(cornerParams.hv_Row1_Horizon,0, cornerParams.hv_Row1_Vertical,out hv_Row1);
                HOperatorSet.TupleInsert(cornerParams.hv_Column1_Horizon,0, cornerParams.hv_Column1_Vertical, out hv_Column1);
                HOperatorSet.TupleInsert(cornerParams.hv_Row2_Horizon, 0,cornerParams.hv_Row2_Vertical, out hv_Row2);
                HOperatorSet.TupleInsert(cornerParams.hv_Column2_Horizon,0, cornerParams.hv_Column2_Vertical, out hv_Column2);

                HOperatorSet.AddMetrologyObjectLineMeasure(cornerParams.hv_MetrologyHandle, hv_Row1, hv_Column1, hv_Row2, hv_Column2,
                    cornerParams.hv_MeasureLength1, cornerParams.hv_MeasureLength2, cornerParams.hv_MeasureSigma, cornerParams.hv_MeasureThreshold,
                    cornerParams.hv_GenParamName, cornerParams.hv_GenParamValue,out cornerParams.hv_Index);


                if (!FindCorner())
                {
                    MessageBox.Show("直线模型查找失败！");
                    return;
                }

                MessageBox.Show("保存直线模型成功！");
            }
            catch (Exception exc)
            {
                MessageBox.Show("保存直线模型失败！"+exc.ToString());
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
                    HOperatorSet.ReadImage(out cornerParams.ho_Image, filePath);
                    Show2HWindow(cornerParams.ho_Image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取静态图像失败！" + exc.ToString());
                }
            }
        }

        private void tsmiSelectHorizonLine_Click(object sender, EventArgs e)
        {
            HTuple hv_Row1=null;
            HTuple hv_Column1 = null;
            HTuple hv_Row2 = null;
            HTuple hv_Column2 = null;

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawLine(hv_ImageWindow,out hv_Row1,out hv_Column1,out hv_Row2,out hv_Column2);

            hv_Row1 = hv_Row1 / hv_ZoomFactor;
            hv_Column1 = hv_Column1 / hv_ZoomFactor;
            hv_Row2 = hv_Row2 / hv_ZoomFactor;
            hv_Column2 = hv_Column2 / hv_ZoomFactor;

            cornerParams.hv_Row1_Horizon = hv_Row1;
            cornerParams.hv_Column1_Horizon = hv_Column1;
            cornerParams.hv_Row2_Horizon = hv_Row2;
            cornerParams.hv_Column2_Horizon = hv_Column2;

            MessageBox.Show("绘制水平直线成功！");
        }

        private void tsmiClearROI_Click(object sender, EventArgs e)
        {
            cornerParams.hv_Row1_Horizon = new HTuple();
            cornerParams.hv_Column1_Horizon = new HTuple();
            cornerParams.hv_Row2_Horizon = new HTuple();
            cornerParams.hv_Column2_Horizon = new HTuple();

            cornerParams.hv_Row1_Vertical = new HTuple();
            cornerParams.hv_Column1_Vertical = new HTuple();
            cornerParams.hv_Row2_Vertical = new HTuple();
            cornerParams.hv_Column2_Vertical = new HTuple();

            cornerParams.hv_LineRowBegin = null;
            cornerParams.hv_LineColumnBegin = null;
            cornerParams.hv_LineRowEnd = null;
            cornerParams.hv_LineColumnEnd = null;

            HOperatorSet.ClearMetrologyModel(cornerParams.hv_MetrologyHandle);
            HOperatorSet.CreateMetrologyModel(out cornerParams.hv_MetrologyHandle);

            MessageBox.Show("清除直线模型成功！");
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
                    HOperatorSet.ReadImage(out cornerParams.ho_Image, filePath);

                    FindCorner();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("静态检测失败！" + exc.ToString());
                }
            }
        }

        private void tsmiSaveWindow_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择保存路径";
            dialog.Filter = "BMP格式（*.png）|.png;";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    HObject hv_SaveImage;
                    HOperatorSet.GenEmptyObj(out hv_SaveImage);
                    HOperatorSet.DumpWindowImage(out hv_SaveImage, hv_ImageWindow);

                    filePath = dialog.FileName;

                    HOperatorSet.WriteImage(hv_SaveImage, "tiff", 0, filePath);

                    MessageBox.Show("窗口保存成功！");
                }
                catch (Exception exc)
                {
                    MessageBox.Show("窗口保存失败！"+exc.ToString());
                }
            }
        }

        private void dudGenParamName_SelectedItemChanged(object sender, EventArgs e)
        {
            cornerParams.hv_GenParamName = dudGenParamName.Text;
        }

        private void nudMeasureLength1_ValueChanged(object sender, EventArgs e)
        {
            cornerParams.hv_MeasureLength1 = Convert.ToInt32(nudMeasureLength1.Value);
        }

        private void nudMeasureLength2_ValueChanged(object sender, EventArgs e)
        {
            cornerParams.hv_MeasureLength2 = Convert.ToInt32(nudMeasureLength2.Value);
        }

        private void nudMeasureThreshold_ValueChanged(object sender, EventArgs e)
        {
            cornerParams.hv_MeasureThreshold = Convert.ToInt32(nudMeasureThreshold.Value);
        }

        private void nudMeasureSigma_ValueChanged(object sender, EventArgs e)
        {
            cornerParams.hv_MeasureSigma = Convert.ToDouble(nudMeasureSigma.Value);
        }

        private void dudGenParamValue_SelectedItemChanged(object sender, EventArgs e)
        {
            cornerParams.hv_GenParamValue = dudGenParamValue.Text;
        }

        private void tsmiSaveLineModel_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择保存路径";
            dialog.Filter = "MTR格式（*.mtr）|.mtr;";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;

                    HOperatorSet.WriteMetrologyModel(cornerParams.hv_MetrologyHandle, filePath);

                    MessageBox.Show("直线模型保存成功！");
                }
                catch (Exception exc)
                {
                    MessageBox.Show("直线模型保存失败！" + exc.ToString());
                }
            }
        }

        private void tsmiSelectVerticalLine_Click(object sender, EventArgs e)
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

            cornerParams.hv_Row1_Vertical = hv_Row1;
            cornerParams.hv_Column1_Vertical = hv_Column1;
            cornerParams.hv_Row2_Vertical = hv_Row2;
            cornerParams.hv_Column2_Vertical = hv_Column2;

            MessageBox.Show("绘制竖直直线成功！");
        }

        private void tsmiHideWindow_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void nudNumber_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nudModel_ValueChanged(object sender, EventArgs e)
        {
            cornerParams.hv_Model = Convert.ToInt32(nudModel.Value);
        }

        private void nudRegionMin_ValueChanged(object sender, EventArgs e)
        {
            cornerParams.hv_RegionMin= Convert.ToInt32(nudRegionMin.Value);
        }

        private void nudRegionMax_ValueChanged(object sender, EventArgs e)
        {
            cornerParams.hv_RegionMax = Convert.ToInt32(nudRegionMax.Value);
        }
    }
}
