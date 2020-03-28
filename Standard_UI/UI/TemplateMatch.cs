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
    public partial class TemplateMatch : Form
    {
        #region 控制参数
        HTuple hv_StartX;
        HTuple hv_StartY;
        HTuple hv_ZoomFactor;
        HTuple hv_ImageWindow;
        HTuple hv_XldHomMat2D;

        public TemplateParams templateParams;

        public List<HObject> ho_ModelRegions ;   //模板区域集合
        public HObject ho_ModelRegion;         //模板区域

        #endregion

        public TemplateMatch()
        {
            InitializeComponent();
            this.ControlBox = false;

            hv_StartX = 0;
            hv_StartY = 0;
            hv_ZoomFactor = 1;
            hv_ImageWindow = hWindowControl1.HalconID;
            templateParams = new TemplateParams();
            ho_ModelRegions = new List<HObject>();
            ho_ModelRegion = new HObject();

            Initialize();
        }

        private void Initialize()
        {
            try
            {
                dudScaleMethod.Text = templateParams.hv_ScaleMethod;
                nudNumLevels_Create.Value = templateParams.hv_NumLevels_Create;
                nudAngleStart_Create.Value = Convert.ToDecimal(templateParams.hv_AngleStart_Create.ToString());
                nudAngleExtent_Create.Value = Convert.ToDecimal(templateParams.hv_AngleExtent_Create.ToString());
                nudAngleStep_Create.Value = Convert.ToDecimal(templateParams.hv_AngleStep_Create.ToString());
                nudScaleRMin_Create.Value = Convert.ToDecimal(templateParams.hv_ScaleRMin_Create.ToString());
                nudScaleRMax_Create.Value = Convert.ToDecimal(templateParams.hv_ScaleRMax_Create.ToString());
                nudScaleRStep_Create.Value = Convert.ToDecimal(templateParams.hv_ScaleRStep_Create.ToString());
                nudScaleCMin_Create.Value = Convert.ToDecimal(templateParams.hv_ScaleCMin_Create.ToString());
                nudScaleCMax_Create.Value = Convert.ToDecimal(templateParams.hv_ScaleCMax_Create.ToString());
                nudScaleCStep_Create.Value = Convert.ToDecimal(templateParams.hv_ScaleCStep_Create.ToString());
                dudOptimization_Create.Text = templateParams.hv_Optimization_Create;
                dudMetric_Create.Text = templateParams.hv_Metric_Create;
                nudContrast_Create.Value = templateParams.hv_Contrast_Create;
                nudMinContrast_Create.Value = templateParams.hv_MinContrast_Create;

                nudAngleStart_Find.Value = Convert.ToDecimal(templateParams.hv_AngleStart_Find.ToString());
                nudAngleExtent_Find.Value = Convert.ToDecimal(templateParams.hv_AngleExtent_Find.ToString());
                nudScaleRMin_Find.Value = Convert.ToDecimal(templateParams.hv_ScaleRMin_Find.ToString());
                nudScaleRMax_Find.Value = Convert.ToDecimal(templateParams.hv_ScaleRMax_Find.ToString());
                nudScaleCMin_Find.Value = Convert.ToDecimal(templateParams.hv_ScaleCMin_Find.ToString());
                nudScaleCMax_Find.Value = Convert.ToDecimal(templateParams.hv_ScaleCMax_Find.ToString());
                nudMinScore_Find.Value = Convert.ToDecimal(templateParams.hv_MinScore_Find.ToString());
                nudNumMatches_Find.Value = templateParams.hv_NumMatches_Find;
                nudMaxOverlap_Find.Value = Convert.ToDecimal(templateParams.hv_MaxOverlap_Find.ToString());
                dudSubPixel_Find.Text = templateParams.hv_SubPixel_Find;
                nudNumLevels_Find.Value = templateParams.hv_NumLevels_Find;
                nudGreediness_Find.Value = Convert.ToDecimal(templateParams.hv_Greediness_Find.ToString());
            }
            catch (Exception exc)
            {
                MessageBox.Show("初始化参数失败！" + exc.ToString());
            }
        }
        private bool CreateTemplate()
        {
            switch (templateParams.hv_ScaleMethod.S)
            {
                case "auto_none":
                    try
                    {
                        HOperatorSet.CreateShapeModel(templateParams.ho_Template, "auto", templateParams.hv_AngleStart_Create, templateParams.hv_AngleExtent_Create,
                        "auto", "auto", templateParams.hv_Metric_Create, "auto", "auto", out templateParams.hv_ModelID);
                        MessageBox.Show("创建模板成功！");
                        return true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("创建模板失败！" + exc.ToString());
                        return false;
                    }
                    //break;
                case "none":
                    try
                    {
                        HOperatorSet.CreateShapeModel(templateParams.ho_Template, templateParams.hv_NumLevels_Create, templateParams.hv_AngleStart_Create, templateParams.hv_AngleExtent_Create,
                        templateParams.hv_AngleStep_Create, templateParams.hv_Optimization_Create, templateParams.hv_Metric_Create, templateParams.hv_Contrast_Create, templateParams.hv_MinContrast_Create,
                        out templateParams.hv_ModelID);

                        MessageBox.Show("创建模板成功！");
                        return true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("创建模板失败！" + exc.ToString());
                        return false;
                    }
                    //break;
                case "auto_scaled":
                    try
                    {
                        HOperatorSet.CreateScaledShapeModel(templateParams.ho_Template, "auto", templateParams.hv_AngleStart_Create, templateParams.hv_AngleExtent_Create,
                        "auto", templateParams.hv_ScaleRMin_Create, templateParams.hv_ScaleRMax_Create, "auto", "auto", templateParams.hv_Metric_Create, "auto", "auto",
                        out templateParams.hv_ModelID);
                        MessageBox.Show("创建模板成功！");
                        return true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("创建模板失败！" + exc.ToString());
                        return false;
                    }
                    //break;
                case "scaled":
                    try
                    {
                        HOperatorSet.CreateScaledShapeModel(templateParams.ho_Template, templateParams.hv_NumLevels_Create, templateParams.hv_AngleStart_Create, templateParams.hv_AngleExtent_Create,
                        templateParams.hv_ScaleRStep_Create, templateParams.hv_ScaleRMin_Create, templateParams.hv_ScaleRMax_Create, templateParams.hv_ScaleRStep_Create, templateParams.hv_Optimization_Create,
                        templateParams.hv_Metric_Create, "auto", "auto", out templateParams.hv_ModelID);
                        MessageBox.Show("创建模板成功！");
                        return true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("创建模板失败！" + exc.ToString());
                        return false;
                    }
                    //break;
                case "auto_aniso":
                    try
                    {
                        HOperatorSet.CreateAnisoShapeModel(templateParams.ho_Template, "auto", templateParams.hv_AngleStart_Create, templateParams.hv_AngleExtent_Create,
                        "auto", templateParams.hv_ScaleRMin_Create, templateParams.hv_ScaleRMax_Create, "auto", templateParams.hv_ScaleCMin_Create, templateParams.hv_ScaleCMax_Create,
                        "auto", "auto", templateParams.hv_Metric_Create, "auto", "auto", out templateParams.hv_ModelID);
                        MessageBox.Show("创建模板成功！");
                        return true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("创建模板失败！" + exc.ToString());
                        return false;
                    }
                    //break;
                case "aniso":
                    try
                    {
                        HOperatorSet.CreateAnisoShapeModel(templateParams.ho_Template, templateParams.hv_NumLevels_Create, templateParams.hv_AngleStart_Create, templateParams.hv_AngleExtent_Create,
                        templateParams.hv_AngleStep_Create, templateParams.hv_ScaleRMin_Create, templateParams.hv_ScaleRMax_Create, templateParams.hv_ScaleRStep_Create, templateParams.hv_ScaleCMin_Create,
                        templateParams.hv_ScaleCMax_Create, templateParams.hv_ScaleCStep_Create, templateParams.hv_Optimization_Create, templateParams.hv_Metric_Create, templateParams.hv_Contrast_Create, templateParams.hv_MinContrast_Create,
                        out templateParams.hv_ModelID);
                        MessageBox.Show("创建模板成功！");
                        return true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("创建模板失败！" + exc.ToString());
                        return false;
                    }
                    //break;
                default:
                    {
                        MessageBox.Show("模板生产方案选择错误！");
                        return false;
                    }                    
                    //break;
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
            double winAspectRatio =(double) winWidth / (double)winHeight;

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
                HOperatorSet.SetPart(hv_ImageWindow, hv_StartX, hv_StartY, winHeight - 1 + hv_StartX,winWidth - 1 + hv_StartY);//设置居中显示
                HOperatorSet.DispObj(ho_ZoomImage, hv_ImageWindow);

                HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);
                HOperatorSet.HomMat2dScale(hv_XldHomMat2D, hv_ZoomFactor, hv_ZoomFactor, 0, 0, out hv_XldHomMat2D);
            }
            catch (Exception exc)
            {
                MessageBox.Show("图像显示错误！"+exc.ToString());
            }
        }

        private bool FindTemplate(HObject ho_testImage)
        {
            if (ho_testImage == null)
            {
                MessageBox.Show("图像为空！");          
                return false;
            }
            if (templateParams.hv_ModelID < 0)
            {
                MessageBox.Show("模板为空！");           
                return false;
            }

            HObject ho_Contour;
            HOperatorSet.GenEmptyObj(out ho_Contour);

            switch (templateParams.hv_ScaleMethod.S)
            {
                case "auto_none":
                    try
                    {
                        HOperatorSet.FindShapeModel(ho_testImage, templateParams.hv_ModelID, templateParams.hv_AngleStart_Find, templateParams.hv_AngleExtent_Find,
                            templateParams.hv_MinScore_Find, templateParams.hv_NumMatches_Find, templateParams.hv_MaxOverlap_Find, templateParams.hv_SubPixel_Find,
                            templateParams.hv_NumLevels_Find, templateParams.hv_Greediness_Find, out templateParams.hv_Row_Find, out templateParams.hv_Column_Find, out templateParams.hv_Angle_Find,
                            out templateParams.hv_Score_Find);
                        
                        if (templateParams.hv_Row_Find.Length != 1)
                        {
                            MessageBox.Show("匹配数量错误！");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("匹配成功！");

                            HOperatorSet.VectorAngleToRigid(0, 0, 0, templateParams.hv_Row_Find, templateParams.hv_Column_Find, templateParams.hv_Angle_Find,
                                    out templateParams.hv_HomMat2D);

                            HOperatorSet.GetShapeModelContours(out ho_Contour, templateParams.hv_ModelID, 1); //获取模板轮廓

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, templateParams.hv_HomMat2D);

                            //HOperatorSet.AreaCenter(ho_ModelRegion, out templateParams.hv_Area, out templateParams.hv_CenterRow, out templateParams.hv_CenterCol);  //计算模板区域中心

                            Show2HWindow(ho_testImage);

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, hv_XldHomMat2D);

                            HOperatorSet.SetColor(hv_ImageWindow, "red");
                            HOperatorSet.DispObj(ho_Contour, hv_ImageWindow);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("查找模板失败！错误码：" + exc.ToString());
                        return false;
                    }
                    break;
                case "none":
                    try
                    {
                        HOperatorSet.FindShapeModel(ho_testImage, templateParams.hv_ModelID, templateParams.hv_AngleStart_Find, templateParams.hv_AngleExtent_Find,
                            templateParams.hv_MinScore_Find, templateParams.hv_NumMatches_Find, templateParams.hv_MaxOverlap_Find, templateParams.hv_SubPixel_Find,
                            templateParams.hv_NumLevels_Find, templateParams.hv_Greediness_Find, out templateParams.hv_Row_Find, out templateParams.hv_Column_Find, out templateParams.hv_Angle_Find,
                            out templateParams.hv_Score_Find);

                        if (templateParams.hv_Row_Find.Length != 1)
                        {
                            MessageBox.Show("匹配数量错误！");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("匹配成功！" );

                            HOperatorSet.VectorAngleToRigid(0, 0, 0, templateParams.hv_Row_Find, templateParams.hv_Column_Find, templateParams.hv_Angle_Find,
                                    out templateParams.hv_HomMat2D);

                            HOperatorSet.GetShapeModelContours(out ho_Contour, templateParams.hv_ModelID, 1); //获取模板轮廓
                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, templateParams.hv_HomMat2D);

                            HOperatorSet.AreaCenter(ho_ModelRegion, out templateParams.hv_Area, out templateParams.hv_CenterRow, out templateParams.hv_CenterCol);  //计算模板区域中心

                            templateParams.hv_CenterRow = templateParams.hv_Row_Find;
                            templateParams.hv_CenterCol = templateParams.hv_Column_Find;
                            //HOperatorSet.VectorAngleToRigid(templateParams.hv_CenterRow, templateParams.hv_CenterCol, 0, templateParams.hv_Row_Find, templateParams.hv_Column_Find,
                            //    templateParams.hv_Angle_Find, out templateParams.hv_ROIHomMat2D);            //产生ROI仿射变换矩阵

                            Show2HWindow(ho_testImage);

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, hv_XldHomMat2D);

                            HOperatorSet.SetColor(hv_ImageWindow, "red");
                            HOperatorSet.DispObj(ho_Contour, hv_ImageWindow);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("查找模板失败！" + exc.ToString());
                        return false;
                    }
                    break;
                case "auto_scaled":
                    try
                    {
                        HOperatorSet.FindScaledShapeModel(ho_testImage, templateParams.hv_ModelID, templateParams.hv_AngleStart_Find, templateParams.hv_AngleExtent_Find,
                        templateParams.hv_ScaleRMin_Find, templateParams.hv_ScaleRMax_Find, templateParams.hv_MinScore_Find, templateParams.hv_NumMatches_Find, templateParams.hv_MaxOverlap_Find,
                        templateParams.hv_SubPixel_Find, templateParams.hv_NumLevels_Find, templateParams.hv_Greediness_Find, out templateParams.hv_Row_Find, out templateParams.hv_Column_Find,
                        out templateParams.hv_Angle_Find, out templateParams.hv_ScaleR_Find, out templateParams.hv_Score_Find);
                        if (templateParams.hv_Row_Find.Length != 1)
                        {
                            MessageBox.Show("匹配数量错误！");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("匹配成功！");

                            HOperatorSet.VectorAngleToRigid(0, 0, 0, templateParams.hv_Row_Find, templateParams.hv_Column_Find, templateParams.hv_Angle_Find,
                                    out templateParams.hv_HomMat2D);

                            HOperatorSet.GetShapeModelContours(out ho_Contour, templateParams.hv_ModelID, 1); //获取模板轮廓

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, templateParams.hv_HomMat2D);

                            HOperatorSet.AreaCenter(ho_ModelRegion, out templateParams.hv_Area, out templateParams.hv_CenterRow, out templateParams.hv_CenterCol);  //计算模板区域中心

                            Show2HWindow(ho_testImage);

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, hv_XldHomMat2D);

                            HOperatorSet.SetColor(hv_ImageWindow, "red");
                            HOperatorSet.DispObj(ho_Contour, hv_ImageWindow);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("查找模板失败！" + exc.ToString());
                        return false;
                    }
                    break;
                case "scaled":
                    try
                    {
                        HOperatorSet.FindScaledShapeModel(ho_testImage, templateParams.hv_ModelID, templateParams.hv_AngleStart_Find, templateParams.hv_AngleExtent_Find,
                        templateParams.hv_ScaleRMin_Find, templateParams.hv_ScaleRMax_Find, templateParams.hv_MinScore_Find, templateParams.hv_NumMatches_Find, templateParams.hv_MaxOverlap_Find,
                        templateParams.hv_SubPixel_Find, templateParams.hv_NumLevels_Find, templateParams.hv_Greediness_Find, out templateParams.hv_Row_Find, out templateParams.hv_Column_Find,
                        out templateParams.hv_Angle_Find, out templateParams.hv_ScaleR_Find, out templateParams.hv_Score_Find);
                        if (templateParams.hv_Row_Find.Length != 1)
                        {
                            MessageBox.Show("匹配数量错误！");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("匹配成功！");

                            HOperatorSet.VectorAngleToRigid(0, 0, 0, templateParams.hv_Row_Find, templateParams.hv_Column_Find, templateParams.hv_Angle_Find,
                                    out templateParams.hv_HomMat2D);

                            HOperatorSet.GetShapeModelContours(out ho_Contour, templateParams.hv_ModelID, 1); //获取模板轮廓

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, templateParams.hv_HomMat2D);

                            HOperatorSet.AreaCenter(ho_ModelRegion, out templateParams.hv_Area, out templateParams.hv_CenterRow, out templateParams.hv_CenterCol);  //计算模板区域中心

                            Show2HWindow(ho_testImage);

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, hv_XldHomMat2D);

                            HOperatorSet.SetColor(hv_ImageWindow, "red");
                            HOperatorSet.DispObj(ho_Contour, hv_ImageWindow);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("查找模板失败！" + exc.ToString());
                        return false;
                    }
                    break;
                case "auto_aniso":
                    try
                    {
                        HOperatorSet.FindAnisoShapeModel(ho_testImage, templateParams.hv_ModelID, templateParams.hv_AngleStart_Find, templateParams.hv_AngleExtent_Find,
                            templateParams.hv_ScaleRMin_Find, templateParams.hv_ScaleRMax_Find, templateParams.hv_ScaleCMin_Find, templateParams.hv_ScaleCMax_Find, templateParams.hv_MinScore_Find,
                            templateParams.hv_NumMatches_Find, templateParams.hv_MaxOverlap_Find, templateParams.hv_SubPixel_Find, templateParams.hv_NumLevels_Find, templateParams.hv_Greediness_Find,
                            out templateParams.hv_Row_Find, out templateParams.hv_Column_Find, out templateParams.hv_Angle_Find, out templateParams.hv_ScaleR_Find, out templateParams.hv_ScaleC_Find,
                            out templateParams.hv_Score_Find);
                        if (templateParams.hv_Row_Find.Length != 1)
                        {
                            MessageBox.Show("匹配数量错误！");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("匹配成功！");

                            HOperatorSet.VectorAngleToRigid(0, 0, 0, templateParams.hv_Row_Find, templateParams.hv_Column_Find, templateParams.hv_Angle_Find,
                                    out templateParams.hv_HomMat2D);

                            HOperatorSet.GetShapeModelContours(out ho_Contour, templateParams.hv_ModelID, 1); //获取模板轮廓

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, templateParams.hv_HomMat2D);

                            HOperatorSet.AreaCenter(ho_ModelRegion, out templateParams.hv_Area, out templateParams.hv_CenterRow, out templateParams.hv_CenterCol);  //计算模板区域中心

                            Show2HWindow(ho_testImage);

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, hv_XldHomMat2D);

                            HOperatorSet.SetColor(hv_ImageWindow, "red");
                            HOperatorSet.DispObj(ho_Contour, hv_ImageWindow);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("查找模板失败！" + exc.ToString());
                        return false;
                    }
                    break;
                case "aniso":
                    try
                    {
                        HOperatorSet.FindAnisoShapeModel(ho_testImage, templateParams.hv_ModelID, templateParams.hv_AngleStart_Find, templateParams.hv_AngleExtent_Find,
                            templateParams.hv_ScaleRMin_Find, templateParams.hv_ScaleRMax_Find, templateParams.hv_ScaleCMin_Find, templateParams.hv_ScaleCMax_Find, templateParams.hv_MinScore_Find,
                            templateParams.hv_NumMatches_Find, templateParams.hv_MaxOverlap_Find, templateParams.hv_SubPixel_Find, templateParams.hv_NumLevels_Find, templateParams.hv_Greediness_Find,
                            out templateParams.hv_Row_Find, out templateParams.hv_Column_Find, out templateParams.hv_Angle_Find, out templateParams.hv_ScaleR_Find, out templateParams.hv_ScaleC_Find,
                            out templateParams.hv_Score_Find);
                        if (templateParams.hv_Row_Find.Length != 1)
                        {
                            MessageBox.Show("匹配数量错误！");
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("匹配成功！");

                            HOperatorSet.VectorAngleToRigid(0, 0, 0, templateParams.hv_Row_Find, templateParams.hv_Column_Find, templateParams.hv_Angle_Find,
                                    out templateParams.hv_HomMat2D);

                            HOperatorSet.GetShapeModelContours(out ho_Contour, templateParams.hv_ModelID, 1); //获取模板轮廓

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, templateParams.hv_HomMat2D);

                            HOperatorSet.AreaCenter(ho_ModelRegion, out templateParams.hv_Area, out templateParams.hv_CenterRow, out templateParams.hv_CenterCol);  //计算模板区域中心

                            Show2HWindow(ho_testImage);

                            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, hv_XldHomMat2D);

                            HOperatorSet.SetColor(hv_ImageWindow, "red");
                            HOperatorSet.DispObj(ho_Contour, hv_ImageWindow);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("查找模板失败！" + exc.ToString());                     
                        return false;
                    }
                    break;
            }
            return true;
        }

        private void btnSaveModel_Click(object sender, EventArgs e)
        {
            if (templateParams.ho_Template != null)
            {
                templateParams.ho_Template.Dispose();
            }
            if (templateParams.ho_Image == null)
            {
                MessageBox.Show("图像为空！");
                return;
            }

            try
            {
                HOperatorSet.ReduceDomain(templateParams.ho_Image, ho_ModelRegion, out templateParams.ho_Template);
            }
            catch (Exception exc)
            {
                MessageBox.Show("区域为空！");
                return;
            }
            
            if (!CreateTemplate())
            {
                MessageBox.Show("创建模板失败！");
            }
            if (!FindTemplate(templateParams.ho_Image))
            {
                MessageBox.Show("查找模板失败！");
            }
        }

        private void tsmiSelectCircle_Click(object sender, EventArgs e)
        {
            HObject ho_ROI = null;
            HTuple hv_Row = null;
            HTuple hv_Column= null;
            HTuple hv_Radius = null;

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawCircle(hv_ImageWindow, out hv_Row, out hv_Column, out hv_Radius);

            HTuple hv_ROI_Row = null;
            HTuple hv_ROI_Column = null;
            HTuple hv_ROI_Radius = null;

            hv_ROI_Row = hv_Row / hv_ZoomFactor;
            hv_ROI_Column = hv_Column / hv_ZoomFactor;
            hv_ROI_Radius = hv_Radius / hv_ZoomFactor;

            HOperatorSet.GenCircle(out ho_ROI, hv_ROI_Row,hv_ROI_Column,hv_ROI_Radius);

            ho_ModelRegions.Add(ho_ROI);

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
            HOperatorSet.DrawEllipse(hv_ImageWindow,out hv_Row, out hv_Column, out hv_Phi, out hv_Radius1, out hv_Radius2);

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

            ho_ModelRegions.Add(ho_ROI);

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

            HOperatorSet.GenRectangle1(out ho_ROI, hv_ROI_Row1, hv_ROI_Column1,hv_ROI_Row2, hv_ROI_Column2);

            ho_ModelRegions.Add(ho_ROI);

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
            HOperatorSet.DrawRectangle2(hv_ImageWindow, out hv_Row, out hv_Column,out hv_Phi, out hv_Length1, out hv_Length2);

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

            ho_ModelRegions.Add(ho_ROI);

            MessageBox.Show("框取旋转矩形ROI成功！");
        }

        private void tsmiClearROI_Click(object sender, EventArgs e)
        {
            ho_ModelRegions.Clear();
            if (ho_ModelRegion != null)
            {
                ho_ModelRegion.Dispose();
            }          
            MessageBox.Show("清除ROI成功！");
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
                    HOperatorSet.ReadImage(out templateParams.ho_Image, filePath);
                    Show2HWindow(templateParams.ho_Image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取静态图像失败！" + exc.ToString());
                }
            }
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
                    HOperatorSet.ReadImage(out templateParams.ho_Image, filePath);
                    FindTemplate(templateParams.ho_Image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("静态检测失败！" + exc.ToString());
                }
            }
        }

        private void tsmiSaveTemplate_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择保存路径";
            dialog.Filter = "SHMM格式（*.shm）|.shm;";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.WriteShapeModel(templateParams.hv_ModelID,filePath);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("模板保存失败！"+exc.ToString());
                }
            }
        }

        private void tsmiHideWindow_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void tsmiReadTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "SHMM文件|*.shm";
            string filePath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    filePath = dialog.FileName;
                    HOperatorSet.ReadShapeModel(filePath,out templateParams.hv_ModelID);
                    MessageBox.Show("模板读取成功");
                }
                catch (Exception exc)
                {
                    MessageBox.Show("模板读取失败！" + exc.ToString());
                }
            }
        }

        private void tsmiUnionROI_Click(object sender, EventArgs e)
        {
            if (ho_ModelRegions.Count() == 0)
            {
                MessageBox.Show("ROI为空!");
                return ;
            }
            else if (ho_ModelRegions.Count() == 1)
            {
                ho_ModelRegion = ho_ModelRegions.ElementAt(0);
                MessageBox.Show("合并ROI区域成功，ROI数量为" + ho_ModelRegions.Count().ToString() + "!");
                return ;
            }
            else
            {
                ho_ModelRegion = ho_ModelRegions.ElementAt(0);
                for (int i = 1; i < ho_ModelRegions.Count(); i++)
                {
                    HOperatorSet.Union2(ho_ModelRegion, ho_ModelRegions.ElementAt(i), out ho_ModelRegion);
                }
                MessageBox.Show("合并ROI区域成功，ROI数量为" + ho_ModelRegions.Count().ToString() + "!");
                return ;
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

        private void tsmiSelectTemplateRegion_Click(object sender, EventArgs e)
        {
            //HObject ho_ROI = null;
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

            HOperatorSet.GenRectangle1(out templateParams.ho_Region, hv_ROI_Row1, hv_ROI_Column1, hv_ROI_Row2, hv_ROI_Column2);

            MessageBox.Show("选取模板匹配区域成功！");
        }

        private void dudScaleMethod_SelectedItemChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleMethod = dudScaleMethod.Text;
        }

        private void nudNumLevels_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_NumLevels_Create = Convert.ToInt32(nudNumLevels_Create.Value);
        }

        private void nudAngleStart_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_AngleStart_Create = Convert.ToDouble(nudAngleStart_Create.Value);
        }

        private void nudAngleExtent_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_AngleExtent_Create = Convert.ToDouble(nudAngleExtent_Create.Value);
        }

        private void nudAngleStep_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_AngleStep_Create = Convert.ToDouble(nudAngleStep_Create.Value);
        }

        private void nudScaleRMin_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleRMin_Create = Convert.ToDouble(nudScaleRMin_Create.Value);
        }

        private void nudScaleRMax_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleRMax_Create = Convert.ToDouble(nudScaleRMax_Create.Value);
        }

        private void nudScaleRStep_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleRStep_Create = Convert.ToDouble(nudScaleRStep_Create.Value);
        }

        private void nudScaleCMin_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleCMin_Create = Convert.ToDouble(nudScaleCMin_Create.Value);
        }

        private void nudScaleCMax_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleCMax_Create = Convert.ToDouble(nudScaleCMax_Create.Value);
        }

        private void nudScaleCStep_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleCStep_Create = Convert.ToDouble(nudScaleCStep_Create.Value);
        }

        private void dudOptimization_Create_SelectedItemChanged(object sender, EventArgs e)
        {
            templateParams.hv_Optimization_Create = dudOptimization_Create.Text;
        }

        private void dudMetric_Create_SelectedItemChanged(object sender, EventArgs e)
        {
            templateParams.hv_Metric_Create = dudMetric_Create.Text;
        }

        private void nudContrast_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_Contrast_Create = Convert.ToInt32(nudContrast_Create.Value);
        }

        private void nudMinContrast_Create_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_MinContrast_Create = Convert.ToInt32(nudMinContrast_Create.Value);
        }

        private void nudAngleStart_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_AngleStart_Find = Convert.ToDouble(nudAngleStart_Find.Value);
        }

        private void nudAngleExtent_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_AngleExtent_Find = Convert.ToDouble(nudAngleExtent_Find.Value);
        }

        private void nudScaleRMin_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleRMin_Find = Convert.ToDouble(nudScaleRMin_Find.Value);
        }

        private void nudScaleRMax_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleRMax_Find = Convert.ToDouble(nudScaleRMax_Find.Value);
        }

        private void nudScaleCMin_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleCMin_Find = Convert.ToDouble(nudScaleCMin_Find.Value);
        }

        private void nudScaleCMax_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_ScaleCMax_Find = Convert.ToDouble(nudScaleCMax_Find.Value);
        }

        private void nudMinScore_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_MinScore_Find = Convert.ToDouble(nudMinScore_Find.Value);
        }

        private void nudNumMatches_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_NumMatches_Find = Convert.ToInt32(nudNumMatches_Find.Value);
        }

        private void nudMaxOverlap_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_MaxOverlap_Find = Convert.ToDouble(nudMaxOverlap_Find.Value);
        }

        private void dudSubPixel_Find_SelectedItemChanged(object sender, EventArgs e)
        {
            templateParams.hv_SubPixel_Find = dudSubPixel_Find.Text;
        }

        private void nudNumLevels_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_NumLevels_Find = Convert.ToInt32(nudNumLevels_Find.Value);
        }

        private void nudGreediness_Find_ValueChanged(object sender, EventArgs e)
        {
            templateParams.hv_Greediness_Find = Convert.ToDouble(nudGreediness_Find.Value);
        }
    }
}
