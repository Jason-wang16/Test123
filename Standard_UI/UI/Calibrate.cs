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
    public partial class Calibrate : Form
    {
        HTuple hv_StartX;
        HTuple hv_StartY;
        HTuple hv_ZoomFactor;
        HTuple hv_ImageWindow;
        HTuple hv_XldHomMat2D;

        HObject ho_Image;

        public Calibrate()
        {
            InitializeComponent();
            this.ControlBox = false;

            hv_StartX = 0;
            hv_StartY = 0;
            hv_ZoomFactor = 1;
            hv_ImageWindow = hWindowControl1.HalconID;
            HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);
            ho_Image = new HObject();
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

        private void get_calibarate_data(HTuple cameraWindow, TextBox x, TextBox y)
        {
            try
            {
                HTuple hv_Row1 = null;
                HTuple hv_Column1 = null;
                HTuple hv_Row2 = null;
                HTuple hv_Column2 = null;
                HOperatorSet.SetColor(cameraWindow, "red");
                HOperatorSet.DrawRectangle1(cameraWindow, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);

                HTuple roi_hv_Row1 = null;
                HTuple roi_hv_Column1 = null;
                HTuple roi_hv_Row2 = null;
                HTuple roi_hv_Column2 = null;

                roi_hv_Row1 = hv_Row1 / hv_ZoomFactor;
                roi_hv_Column1 = hv_Column1 / hv_ZoomFactor;
                roi_hv_Row2 = hv_Row2 / hv_ZoomFactor;
                roi_hv_Column2 = hv_Column2 / hv_ZoomFactor;

                HObject ho_ROI=new HObject();
                HOperatorSet.GenRectangle1(out ho_ROI, roi_hv_Row1, roi_hv_Column1, roi_hv_Row2, roi_hv_Column2);

                HTuple hv_Row_camera = null;
                HTuple hv_Column_camera = null;

                if (!findCircle(ho_Image, ho_ROI, ref hv_Row_camera, ref hv_Column_camera))
                {
                    MessageBox.Show("查找圆心失败！");
                    return;
                }
                HObject ho_MeasuredPoint;
                HOperatorSet.GenCrossContourXld(out ho_MeasuredPoint, hv_Row_camera, hv_Column_camera, 40, (new HTuple(45)).TupleRad());

                x.Text = hv_Row_camera.ToString();
                y.Text = hv_Column_camera.ToString();

                HOperatorSet.AffineTransContourXld(ho_MeasuredPoint, out ho_MeasuredPoint, hv_XldHomMat2D);

                HOperatorSet.DispObj(ho_MeasuredPoint, cameraWindow);

            }
            catch (Exception)
            {
                MessageBox.Show("框取区域失败！");
            }
        }

        public bool findCircle(HObject ho_ImageIn, HObject ho_ROIIn,ref HTuple hv_RowIn,ref HTuple hv_ColumnIn) //找外角点(最长水平、垂直线的交点)
        {
            try
            {
                HObject ho_ImageReduced = null;
                HOperatorSet.ReduceDomain(ho_ImageIn, ho_ROIIn, out ho_ImageReduced);

                HObject ho_Edges = null;
                HOperatorSet.EdgesSubPix(ho_ImageReduced, out ho_Edges, "canny", 1, 20, 40);

                HTuple hv_Number = null;
                HOperatorSet.CountObj(ho_Edges, out hv_Number);

                if (hv_Number < 1)
                {
                    return false;
                }

                HTuple hv_Row;
                HTuple hv_Column;
                HTuple hv_Radius;
                HTuple hv_StartPhi;
                HTuple hv_EndPhi;
                HTuple hv_PointOrder;
                HOperatorSet.FitCircleContourXld(ho_Edges, "algebraic", -1, 0, 0, 3, 2, out hv_Row,
                    out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);

                if (hv_Number > 0)
                {
                    for (int i = 0; i < hv_Number; i++)
                    {
                        for (int j = 1; j < hv_Number; j++)
                        {
                            if (hv_Radius[i] < hv_Radius[j])
                            {
                                HTuple a = hv_Row[i];
                                HTuple b = hv_Column[i];
                                HTuple c = hv_Radius[i];

                                hv_Row[i] = hv_Row[j];
                                hv_Column[i] = hv_Column[j];
                                hv_Radius[i] = hv_Radius[j];

                                hv_Row[j] = a;
                                hv_Column[j] = b;
                                hv_Radius[j] = c;
                            }
                        }
                    }
                    hv_RowIn = hv_Row[0];
                    hv_ColumnIn = hv_Column[0];
                }
                return true;
            }
            catch (Exception exc)
            {
                return false;
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
                    HOperatorSet.ReadImage(out ho_Image, filePath);
                    Show2HWindow(ho_Image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取静态图像失败！" + exc.ToString());
                }
            }
        }

        private void tsmiHideWindow_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnGetCameraPoint1_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix1X, txtPix1Y);
        }

        private void btnGetCameraPoint2_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix2X, txtPix2Y);
        }

        private void btnGetCameraPoint3_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix3X, txtPix3Y);
        }

        private void btnGetCameraPoint4_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix4X, txtPix4Y);
        }

        private void btnGetCameraPoint5_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix5X, txtPix5Y);
        }

        private void btnGetCameraPoint6_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix6X, txtPix6Y);
        }

        private void btnGetCameraPoint7_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix7X, txtPix7Y);
        }

        private void btnGetCameraPoint8_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix8X, txtPix8Y);
        }

        private void btnGetCameraPoint9_Click(object sender, EventArgs e)
        {
            get_calibarate_data(hv_ImageWindow, txtPix9X, txtPix9Y);
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple hv_camera_x = new HTuple();
                HTuple hv_camera_y = new HTuple();
                HTuple hv_real_x = new HTuple();
                HTuple hv_real_y = new HTuple();

                hv_camera_x[0] = Convert.ToDouble(txtPix1X.Text);
                hv_camera_x[1] = Convert.ToDouble(txtPix2X.Text);
                hv_camera_x[2] = Convert.ToDouble(txtPix3X.Text);
                hv_camera_x[3] = Convert.ToDouble(txtPix4X.Text);
                hv_camera_x[4] = Convert.ToDouble(txtPix5X.Text);
                hv_camera_x[5] = Convert.ToDouble(txtPix6X.Text);
                hv_camera_x[6] = Convert.ToDouble(txtPix7X.Text);
                hv_camera_x[7] = Convert.ToDouble(txtPix8X.Text);
                hv_camera_x[8] = Convert.ToDouble(txtPix9X.Text);

                hv_camera_y[0] = Convert.ToDouble(txtPix1Y.Text);
                hv_camera_y[1] = Convert.ToDouble(txtPix2Y.Text);
                hv_camera_y[2] = Convert.ToDouble(txtPix3Y.Text);
                hv_camera_y[3] = Convert.ToDouble(txtPix4Y.Text);
                hv_camera_y[4] = Convert.ToDouble(txtPix5Y.Text);
                hv_camera_y[5] = Convert.ToDouble(txtPix6Y.Text);
                hv_camera_y[6] = Convert.ToDouble(txtPix7Y.Text);
                hv_camera_y[7] = Convert.ToDouble(txtPix8Y.Text);
                hv_camera_y[8] = Convert.ToDouble(txtPix9Y.Text);

                hv_real_x[0] = Convert.ToDouble(txtReal1X.Text);
                hv_real_x[1] = Convert.ToDouble(txtReal2X.Text);
                hv_real_x[2] = Convert.ToDouble(txtReal3X.Text);
                hv_real_x[3] = Convert.ToDouble(txtReal4X.Text);
                hv_real_x[4] = Convert.ToDouble(txtReal5X.Text);
                hv_real_x[5] = Convert.ToDouble(txtReal6X.Text);
                hv_real_x[6] = Convert.ToDouble(txtReal7X.Text);
                hv_real_x[7] = Convert.ToDouble(txtReal8X.Text);
                hv_real_x[8] = Convert.ToDouble(txtReal9X.Text);

                hv_real_y[0] = Convert.ToDouble(txtReal1Y.Text);
                hv_real_y[1] = Convert.ToDouble(txtReal2Y.Text);
                hv_real_y[2] = Convert.ToDouble(txtReal3Y.Text);
                hv_real_y[3] = Convert.ToDouble(txtReal4Y.Text);
                hv_real_y[4] = Convert.ToDouble(txtReal5Y.Text);
                hv_real_y[5] = Convert.ToDouble(txtReal6Y.Text);
                hv_real_y[6] = Convert.ToDouble(txtReal7Y.Text);
                hv_real_y[7] = Convert.ToDouble(txtReal8Y.Text);
                hv_real_y[8] = Convert.ToDouble(txtReal9Y.Text);

                HTuple hv_HomMat2D = null;
                HOperatorSet.VectorToHomMat2d(hv_camera_x, hv_camera_y, hv_real_x, hv_real_y, out hv_HomMat2D);

                string hv_HomMat2DPath = AppDomain.CurrentDomain.BaseDirectory + "Calibration.tuple";

                HOperatorSet.WriteTuple(hv_HomMat2D, hv_HomMat2DPath);

                MessageBox.Show("标定成功！");
            }
            catch (Exception)
            {
                MessageBox.Show("标定失败，请确认标定数据是否正确！");
            }
        }
    }
}
