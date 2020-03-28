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
    public partial class Measure : Form
    {
        #region 控制参数
        HTuple hv_StartX;
        HTuple hv_StartY;
        HTuple hv_ZoomFactor;
        HTuple hv_ImageWindow;

        Measure1DParams measureParams;

        #endregion
        public Measure()
        {
            InitializeComponent();

            hv_StartX = 0;
            hv_StartY = 0;
            hv_ZoomFactor = 1;
            hv_ImageWindow = hWindowControl1.HalconID;

            measureParams = new Measure1DParams();
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
            }
            catch (Exception exc)
            {
                MessageBox.Show("图像显示错误！" + exc.ToString());
            }
        }

        private void tsmiDetectLineDistance_Click(object sender, EventArgs e)
        {
            HObject ho_Line;
            HOperatorSet.GenEmptyObj(out ho_Line);
            HObject ho_Cross1;
            HOperatorSet.GenEmptyObj(out ho_Cross1);
            HObject ho_Cross2;
            HOperatorSet.GenEmptyObj(out ho_Cross2);

            HObject ho_ROI = null;
            HTuple hv_Row = null;
            HTuple hv_Column = null;
            HTuple hv_Phi = null;
            HTuple hv_Length1 = null;
            HTuple hv_Length2 = null;

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawRectangle2(hv_ImageWindow, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2);

            measureParams.hv_Row = hv_Row / hv_ZoomFactor;
            measureParams.hv_Column = hv_Column / hv_ZoomFactor;
            measureParams.hv_Phi = hv_Phi;
            measureParams.hv_Length1 = hv_Length1 / hv_ZoomFactor;
            measureParams.hv_Length2 = hv_Length2 / hv_ZoomFactor;
            HOperatorSet.GetImageSize(measureParams.ho_Image, out measureParams.hv_Width, out measureParams.hv_Height);

            HOperatorSet.GenMeasureRectangle2(measureParams.hv_Row, measureParams.hv_Column, measureParams.hv_Phi, measureParams.hv_Length1,
                measureParams.hv_Length2, measureParams.hv_Width, measureParams.hv_Height, measureParams.hv_Interpolation,
                out measureParams.hv_MeasureHandle);

            HOperatorSet.MeasurePairs(measureParams.ho_Image, measureParams.hv_MeasureHandle, measureParams.hv_Sigma, measureParams.hv_Threshold,
                measureParams.hv_Transition, measureParams.hv_Select, out measureParams.hv_RowEdgeFirst, out measureParams.hv_ColumnEdgeFirst, out measureParams.hv_AmplitudeFirst,
                out measureParams.hv_RowEdgeSecond, out measureParams.hv_ColumnEdgeSecond, out measureParams.hv_AmplitudeSecond,
                out measureParams.hv_IntraDistance, out measureParams.hv_InterDistance);
            //HOperatorSet.MeasurePos(measureParams.ho_Image, measureParams.hv_MeasureHandle, measureParams.hv_Sigma, measureParams.hv_Threshold,
            //    measureParams.hv_Transition, measureParams.hv_Select, out measureParams.hv_RowEdgeFirst, out measureParams.hv_ColumnEdgeFirst,
            //    out measureParams.hv_AmplitudeFirst, out measureParams.hv_InterDistance);

            if (measureParams.hv_RowEdgeFirst.Length < 1)
            {
                MessageBox.Show("未找到边缘对！");
            }

            HOperatorSet.GenCrossContourXld(out ho_Cross1, measureParams.hv_RowEdgeFirst, measureParams.hv_ColumnEdgeFirst, 20, (new HTuple(45)).TupleRad());
            HOperatorSet.GenCrossContourXld(out ho_Cross2, measureParams.hv_RowEdgeSecond, measureParams.hv_ColumnEdgeSecond, 20, (new HTuple(45)).TupleRad());
            HOperatorSet.GenRegionLine(out ho_Line, measureParams.hv_RowEdgeFirst*hv_ZoomFactor, measureParams.hv_ColumnEdgeFirst * hv_ZoomFactor, measureParams.hv_RowEdgeSecond * hv_ZoomFactor, 
                measureParams.hv_ColumnEdgeSecond * hv_ZoomFactor);

            //HOperatorSet.ConcatObj(ho_Line, ho_Cross1, out ho_Cross1);
            //HOperatorSet.ConcatObj(ho_Line, ho_Cross2, out ho_Line);

            HTuple hv_XldHomMat2D;
            HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);

            HOperatorSet.HomMat2dScale(hv_XldHomMat2D, hv_ZoomFactor, hv_ZoomFactor, 0, 0, out hv_XldHomMat2D);

            HOperatorSet.AffineTransContourXld(ho_Cross1, out ho_Cross1, hv_XldHomMat2D);
            HOperatorSet.AffineTransContourXld(ho_Cross2, out ho_Cross2, hv_XldHomMat2D);
            //HOperatorSet.AffineTransContourXld(ho_Line, out ho_Line, hv_XldHomMat2D);

            Show2HWindow(measureParams.ho_Image);

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DispObj(ho_Cross1, hv_ImageWindow);
            HOperatorSet.DispObj(ho_Cross2, hv_ImageWindow);
            HOperatorSet.DispObj(ho_Line, hv_ImageWindow);
        }

        private void tsmiDetectCircleDistance_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveMeasureParams_Click(object sender, EventArgs e)
        {

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
                    HOperatorSet.ReadImage(out measureParams.ho_Image, filePath);
                    Show2HWindow(measureParams.ho_Image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取静态图像失败！" + exc.ToString());
                }
            }
        }
    }
}
