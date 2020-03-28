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
    public partial class ExistDetect : Form
    {
        HTuple hv_StartX;
        HTuple hv_StartY;
        HTuple hv_ZoomFactor;
        HTuple hv_ImageWindow;

        HTuple hv_XldHomMat2D;
        public ExistParams existParams;

        HTuple hv_Interpolate;

        List<HObject> ho_Regions;   //模板区域集合

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

        public ExistDetect()
        {
            InitializeComponent();
            this.ControlBox = false;

            hv_StartX = 0;
            hv_StartY = 0;
            hv_ZoomFactor = 1;
            hv_ImageWindow = hWindowControl1.HalconID;
            HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);

            existParams = new ExistParams();

            hv_Interpolate = "constant";

            ho_Regions = new List<HObject>();

            Initialize();
        }
        private void Initialize()
        {
            try
            {
                nudMinGray.Value = existParams.hv_MinGray;
                nudMaxGray.Value = existParams.hv_MaxGray;
                nudMin.Value = existParams.hv_Min;
                nudMax.Value = existParams.hv_Max;
                nudNumber.Value = existParams.hv_Number;
                dudInterpolate.Text = hv_Interpolate;
            }
            catch (Exception exc)
            {
                MessageBox.Show("初始化参数失败！" + exc.ToString());
            }

        }
        public bool CheckIfExist()
        {
            if (existParams.ho_Image == null)
            {
                MessageBox.Show("图像为空！");
                return false;
            }
            Show2HWindow(existParams.ho_Image);
            if (existParams.ho_Region == null)
            {
                MessageBox.Show("区域为空！");
                return false;
            }
            try
            {
                HObject ho_ImageReduced = null;
                HOperatorSet.ReduceDomain(existParams.ho_Image, existParams.ho_Region, out ho_ImageReduced);

                HObject ho_Regions = null;
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, existParams.hv_MinGray, existParams.hv_MaxGray);

                HObject ho_ConnectedRegions = null;
                HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);

                HObject ho_SelectedRegions = null;
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", existParams.hv_Min, existParams.hv_Max);

                HTuple hv_Number = new HTuple();
                HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);
                if (hv_Number< existParams.hv_Number)
                {
                    MessageBox.Show("匹配数量过少");
                    return false;
                }              
                HOperatorSet.AffineTransRegion(ho_SelectedRegions,out ho_SelectedRegions, hv_XldHomMat2D, hv_Interpolate);

                HOperatorSet.SetColor(hv_ImageWindow, "red");
                HOperatorSet.DispObj(ho_SelectedRegions, hv_ImageWindow);
                MessageBox.Show("检测成功，检测匹配个数："+ hv_Number.ToString()+"!");
                return true;
            }
            catch (Exception exc)
            {
                MessageBox.Show("检测失败！"+exc.ToString());
                return false;
            }     
        }


        private void btnSaveExistParams_Click(object sender, EventArgs e)
        {
            CheckIfExist();
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
                    HOperatorSet.ReadImage(out existParams.ho_Image, filePath);
                    Show2HWindow(existParams.ho_Image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取静态图像失败！" + exc.ToString());
                }
            }
        }

        private void tsmiSelectCircle_Click(object sender, EventArgs e)
        {
            HObject ho_ROI = null;
            HTuple hv_Row = null;
            HTuple hv_Column = null;
            HTuple hv_Radius = null;

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawCircle(hv_ImageWindow, out hv_Row, out hv_Column, out hv_Radius);

            HTuple hv_ROI_Row = null;
            HTuple hv_ROI_Column = null;
            HTuple hv_ROI_Radius = null;

            hv_ROI_Row = hv_Row / hv_ZoomFactor;
            hv_ROI_Column = hv_Column / hv_ZoomFactor;
            hv_ROI_Radius = hv_Radius / hv_ZoomFactor;

            HOperatorSet.GenCircle(out ho_ROI, hv_ROI_Row, hv_ROI_Column, hv_ROI_Radius);

            ho_Regions.Add(ho_ROI);

            MessageBox.Show("框取圆形ROI成功！");
        }

        private void tsmiSelectEllipse_Click(object sender, EventArgs e)
        {
            HObject ho_ROI = null;
            HTuple hv_Row = null;
            HTuple hv_Column = null;
            HTuple hv_Phi = null;
            HTuple hv_Radius1 = null;
            HTuple hv_Radius2 = null;

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawEllipse(hv_ImageWindow, out hv_Row, out hv_Column, out hv_Phi, out hv_Radius1, out hv_Radius2);

            HTuple hv_ROI_Row = null;
            HTuple hv_ROI_Column = null;
            HTuple hv_ROI_Phi = null;
            HTuple hv_ROI_Radius1 = null;
            HTuple hv_ROI_Radius2 = null;

            hv_ROI_Row = hv_Row / hv_ZoomFactor;
            hv_ROI_Column = hv_Column / hv_ZoomFactor;
            hv_ROI_Phi = hv_Phi;
            hv_ROI_Radius1 = hv_Radius1 / hv_ZoomFactor;
            hv_ROI_Radius2 = hv_Radius2 / hv_ZoomFactor;

            HOperatorSet.GenEllipse(out ho_ROI, hv_ROI_Row, hv_ROI_Column, hv_ROI_Phi, hv_ROI_Radius1, hv_ROI_Radius2);

            ho_Regions.Add(ho_ROI);

            MessageBox.Show("框取椭圆ROI成功！");
        }

        private void tsmiSelectRectangle_Click(object sender, EventArgs e)
        {
            HObject ho_ROI = null;
            HTuple hv_Row1 = null;
            HTuple hv_Column1 = null;
            HTuple hv_Row2 = null;
            HTuple hv_Column2 = null;
            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawRectangle1(hv_ImageWindow, out hv_Row1, out hv_Column1, out hv_Row2, out hv_Column2);

            HTuple hv_ROI_Row1 = null;
            HTuple hv_ROI_Column1 = null;
            HTuple hv_ROI_Row2 = null;
            HTuple hv_ROI_Column2 = null;

            hv_ROI_Row1 = hv_Row1 / hv_ZoomFactor;
            hv_ROI_Column1 = hv_Column1 / hv_ZoomFactor;
            hv_ROI_Row2 = hv_Row2 / hv_ZoomFactor;
            hv_ROI_Column2 = hv_Column2 / hv_ZoomFactor;

            HOperatorSet.GenRectangle1(out ho_ROI, hv_ROI_Row1, hv_ROI_Column1, hv_ROI_Row2, hv_ROI_Column2);

            ho_Regions.Add(ho_ROI);

            MessageBox.Show("框取矩形ROI成功！");
        }

        private void tsmiSelectRevolveRectangle_Click(object sender, EventArgs e)
        {
            HObject ho_ROI = null;
            HTuple hv_Row = null;
            HTuple hv_Column = null;
            HTuple hv_Phi = null;
            HTuple hv_Length1 = null;
            HTuple hv_Length2 = null;

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawRectangle2(hv_ImageWindow, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2);

            HTuple hv_ROI_Row = null;
            HTuple hv_ROI_Column = null;
            HTuple hv_ROI_Phi = null;
            HTuple hv_ROI_Length1 = null;
            HTuple hv_ROI_Length2 = null;

            hv_ROI_Row = hv_Row / hv_ZoomFactor;
            hv_ROI_Column = hv_Column / hv_ZoomFactor;
            hv_ROI_Phi = hv_Phi;
            hv_ROI_Length1 = hv_Length1 / hv_ZoomFactor;
            hv_ROI_Length2 = hv_Length1 / hv_ZoomFactor;

            HOperatorSet.GenRectangle2(out ho_ROI, hv_ROI_Row, hv_ROI_Column, hv_ROI_Phi, hv_ROI_Length1, hv_ROI_Length2);

            ho_Regions.Add(ho_ROI);

            MessageBox.Show("框取旋转矩形ROI成功！");
        }

        private void tsmiClearROI_Click(object sender, EventArgs e)
        {
            ho_Regions.Clear();
            if (existParams.ho_Region != null)
            {
                existParams.ho_Region.Dispose();
            }
            MessageBox.Show("清除ROI成功！");
        }

        private void tsmiStaticDetect_Click(object sender, EventArgs e)
        {

        }

        private void nudMinGray_ValueChanged(object sender, EventArgs e)
        {
            existParams.hv_MinGray = Convert.ToInt32(nudMinGray.Value);
        }

        private void nudMaxGray_ValueChanged(object sender, EventArgs e)
        {
            existParams.hv_MaxGray = Convert.ToInt32(nudMaxGray.Value);
        }

        private void nudMin_ValueChanged(object sender, EventArgs e)
        {
            existParams.hv_Min = Convert.ToInt32(nudMin.Value);
        }

        private void nudMax_ValueChanged(object sender, EventArgs e)
        {
            existParams.hv_Max = Convert.ToInt32(nudMax.Value);
        }

        private void nudNumber_ValueChanged(object sender, EventArgs e)
        {
            existParams.hv_Number = Convert.ToInt32(nudNumber.Value);
        }

        private void dudInterpolate_SelectedItemChanged(object sender, EventArgs e)
        {
            hv_Interpolate = dudInterpolate.Text;
        }

        private void tsmiHideWindow_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void tsmiSaveRegion_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择保存路径";
            dialog.Filter = "HOBJ格式（*.hobj）|.hobj;";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;

                    HOperatorSet.WriteRegion(existParams.ho_Region, filePath);

                    MessageBox.Show("区域保存成功！");
                }
                catch (Exception exc)
                {
                    MessageBox.Show("区域保存失败！" + exc.ToString());
                }
            }
        }

        private void tsmiReadRegion_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "区域|*.hobj";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.ReadRegion(out existParams.ho_Region,filePath);

                    string regionPath = AppDomain.CurrentDomain.BaseDirectory + "Parameters\\kkkk.hobj";
                    HOperatorSet.WriteRegion(existParams.ho_Region, regionPath);
                    MessageBox.Show("区域读取成功"); 
                }
                catch (Exception exc)
                {
                    MessageBox.Show("区域读取失败！" + exc.ToString());
                }
            }
        }

        private void tsmiUnionROI_Click(object sender, EventArgs e)
        {
            if (ho_Regions.Count() == 0)
            {
                MessageBox.Show("ROI为空!");
                return;
            }
            else if (ho_Regions.Count() == 1)
            {
                existParams.ho_Region = ho_Regions.ElementAt(0);
                MessageBox.Show("合并ROI区域成功，ROI数量为" + ho_Regions.Count().ToString() + "!");
                return;
            }
            else
            {
                existParams.ho_Region = ho_Regions.ElementAt(0);
                for (int i = 1; i < ho_Regions.Count(); i++)
                {
                    HOperatorSet.Union2(existParams.ho_Region, ho_Regions.ElementAt(i), out existParams.ho_Region);
                }
                MessageBox.Show("合并ROI区域成功，ROI数量为" + ho_Regions.Count().ToString() + "!");
                return;
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
                    MessageBox.Show("窗口保存失败！" + exc.ToString());
                }
            }
        }
    }
}
