using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Standard_UI.UI
{
    public class TemplateParams
    {
        public bool errorFlag;

        #region 创建模板参数
        public HTuple hv_ScaleMethod;            //比例模式:"none"无缩放,"scaled"同步缩放,"aniso"异步缩放,"auto"自动

        public HObject ho_Image;                 //源图像
        public HObject ho_Template;         //模板图像

        public HTuple hv_NumLevels_Create;              //金字塔级别
        public HTuple hv_AngleStart_Create;             //起始角度
        public HTuple hv_AngleExtent_Create;            //角度范围
        public HTuple hv_AngleStep_Create;              //角度步长
        public HTuple hv_ScaleRMin_Create;              //最小行缩放
        public HTuple hv_ScaleRMax_Create;              //最大行缩放
        public HTuple hv_ScaleRStep_Create;             //行方向缩放步长

        public HTuple hv_ScaleCMin_Create;              //最小列缩放
        public HTuple hv_ScaleCMax_Create;              //最大列缩放
        public HTuple hv_ScaleCStep_Create;             //列方向缩放步长
        public HTuple hv_Optimization_Create;           //优化算法
        public HTuple hv_Metric_Create;                 //极性/度量
        public HTuple hv_Contrast_Create;               //对比度
        public HTuple hv_MinContrast_Create;            //最小对比度

        public HTuple hv_ModelID;                //模板句柄     
        #endregion

        #region 查找模板参数
        public HTuple hv_AngleStart_Find;             //匹配最小角度
        public HTuple hv_AngleExtent_Find;            //匹配最大角度（一起就是匹配角度范围）
        public HTuple hv_ScaleRMin_Find;              //最小行放大倍数
        public HTuple hv_ScaleRMax_Find;              //最大行放大倍数
        public HTuple hv_ScaleCMin_Find;              //最小列放大倍数
        public HTuple hv_ScaleCMax_Find;              //最大列放大倍数
        public HTuple hv_MinScore_Find;               //放大倍数步长
        public HTuple hv_NumMatches_Find;             //匹配个数，0则自动选择，100则最多匹配100个
        public HTuple hv_MaxOverlap_Find;             //要找到模型实例的最大重叠
        public HTuple hv_SubPixel_Find;               //亚像素精度
        public HTuple hv_NumLevels_Find;              //图像金字塔
        public HTuple hv_Greediness_Find;             //贪婪系数

        public HTuple hv_Row_Find;                    //中心点（X）
        public HTuple hv_Column_Find;                 //中心点（Y）
        public HTuple hv_Angle_Find;                  //角度
        public HTuple hv_ScaleR_Find;                 //行缩放
        public HTuple hv_ScaleC_Find;                 //列缩放
        public HTuple hv_Score_Find;                  //匹配分数

        public HTuple hv_Area;
        public HTuple hv_CenterRow;
        public HTuple hv_CenterCol;

        public HTuple hv_HomMat2D;
        public HTuple hv_ROIHomMat2D;

        ParametersRW.XmlRW xmlRW;

        public HObject ho_Region;                //框取的模板匹配区域，缩短匹配时间
        #endregion

        #region 构造函数
        public TemplateParams()
        {
            errorFlag = false;

            hv_ScaleMethod = "none";

            hv_NumLevels_Create = 10;
            hv_AngleStart_Create = -0.08;
            hv_AngleExtent_Create = 0.08;
            hv_AngleStep_Create = 0.0175;
            hv_ScaleRMin_Create = 1;
            hv_ScaleRMax_Create = 1;
            hv_ScaleRStep_Create = 0.2;

            hv_ScaleCMin_Create = 1;
            hv_ScaleCMax_Create = 1;
            hv_ScaleCStep_Create = 0.2;             
            hv_Optimization_Create = "none";
            hv_Metric_Create = "use_polarity";
            hv_Contrast_Create = 30;
            hv_MinContrast_Create = 4;

            hv_AngleStart_Find = -0.08;                   //匹配最小角度
            hv_AngleExtent_Find = 0.08;                   //匹配最大角度（一起就是匹配角度范围）
            hv_ScaleRMin_Find = 1;                      //最小行放大倍数
            hv_ScaleRMax_Find = 1;                      //最大行放大倍数
            hv_ScaleCMin_Find = 1;                      //最小列放大倍数
            hv_ScaleCMax_Find = 1;                      //最大列放大倍数
            hv_MinScore_Find = 0.90;                    //匹配分数
            hv_NumMatches_Find = 1;                     //匹配个数，0则自动选择，100则最多匹配100个
            hv_MaxOverlap_Find = 0;                   //要找到模型实例的最大重叠
            hv_SubPixel_Find = "none";         //像素精度
            hv_NumLevels_Find = 1;                      //图像金字塔
            hv_Greediness_Find = 0;                   //贪婪系数

            xmlRW = new ParametersRW.XmlRW();

            HOperatorSet.GenEmptyRegion(out ho_Region);
        }
        #endregion

        public bool ReadTemplateImage(string templateImagePath)
        {
            HOperatorSet.ReadImage(out ho_Template, templateImagePath);
            if (ho_Template == null)
            {
                return false;
            }
            if (!CreateTemplate())
            {
                return false;
            }
            return true;
        }

        public bool CopyTo(ref TemplateParams templateParams)
        {
            try
            {
                templateParams.hv_ScaleMethod =hv_ScaleMethod;

                templateParams.ho_Template= ho_Template;

                templateParams.hv_NumLevels_Create = hv_NumLevels_Create;              //金字塔级别
                templateParams.hv_AngleStart_Create = hv_AngleStart_Create;             //起始角度
                templateParams.hv_AngleExtent_Create = hv_AngleExtent_Create;            //角度范围
                templateParams.hv_AngleStep_Create = hv_AngleStep_Create;              //角度步长
                templateParams.hv_ScaleRMin_Create = hv_ScaleRMin_Create;              //最小行缩放
                templateParams.hv_ScaleRMax_Create = hv_ScaleRMax_Create;              //最大行缩放
                templateParams.hv_ScaleRStep_Create = hv_ScaleRStep_Create;             //行方向缩放步长

                templateParams.hv_ScaleCMin_Create = hv_ScaleCMin_Create;              //最小列缩放
                templateParams.hv_ScaleCMax_Create = hv_ScaleCMax_Create;              //最大列缩放
                templateParams.hv_ScaleCStep_Create = hv_ScaleCStep_Create;             //列方向缩放步长
                templateParams.hv_Optimization_Create = hv_Optimization_Create;           //优化算法
                templateParams.hv_Metric_Create = hv_Metric_Create;                 //极性/度量
                templateParams.hv_Contrast_Create = hv_Contrast_Create;               //对比度
                templateParams.hv_MinContrast_Create = hv_MinContrast_Create;            //最小对比度

                templateParams.hv_ModelID = hv_ModelID;                //模板句柄   

                templateParams.hv_AngleStart_Find= hv_AngleStart_Find;             //匹配最小角度
                templateParams.hv_AngleExtent_Find= hv_AngleExtent_Find;            //匹配最大角度（一起就是匹配角度范围）
                templateParams.hv_ScaleRMin_Find= hv_ScaleRMin_Find;              //最小行放大倍数
                templateParams.hv_ScaleRMax_Find= hv_ScaleRMax_Find;              //最大行放大倍数
                templateParams.hv_ScaleCMin_Find= hv_ScaleCMin_Find;              //最小列放大倍数
                templateParams.hv_ScaleCMax_Find= hv_ScaleCMax_Find;              //最大列放大倍数
                templateParams.hv_MinScore_Find= hv_MinScore_Find;               //放大倍数步长
                templateParams.hv_NumMatches_Find= hv_NumMatches_Find;             //匹配个数，0则自动选择，100则最多匹配100个
                templateParams.hv_MaxOverlap_Find= hv_MaxOverlap_Find;             //要找到模型实例的最大重叠
                templateParams.hv_SubPixel_Find= hv_SubPixel_Find;               //亚像素精度
                templateParams.hv_NumLevels_Find= hv_NumLevels_Find;              //图像金字塔
                templateParams.hv_Greediness_Find= hv_Greediness_Find;             //贪婪系数

                templateParams.hv_CenterRow = hv_CenterRow;
                templateParams.hv_CenterCol = hv_CenterCol;

                templateParams.ho_Region = ho_Region;

                errorFlag =true;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = false;
                return false;
            }
            
        }

        public bool FindTemplate()
        {
            if (ho_Image == null)
            {
                errorFlag = true;
                return false;
            }
            if (hv_ModelID < 0)
            {
                errorFlag = true;
                return false;
            }
            if (ho_Region == null)
            {
                errorFlag = true;
                return false;
            }
            HObject mho_Image;
            HOperatorSet.ReduceDomain(ho_Image, ho_Region,out mho_Image);
            switch (hv_ScaleMethod.S)
            {
                case "auto_none":
                    try
                    {
                        HOperatorSet.FindShapeModel(ho_Image,hv_ModelID, hv_AngleStart_Find, hv_AngleExtent_Find,
                            hv_MinScore_Find, hv_NumMatches_Find, hv_MaxOverlap_Find, hv_SubPixel_Find,
                            hv_NumLevels_Find, hv_Greediness_Find, out hv_Row_Find, out hv_Column_Find, out hv_Angle_Find,
                            out hv_Score_Find);

                        if (hv_Row_Find.Length < 1)
                        {
                            errorFlag = true;
                            return false;
                        }


                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception exc)
                    {
                        errorFlag = true;
                        return false;
                    }
                    break;
                case "none":
                    try
                    {
                        HOperatorSet.FindShapeModel(mho_Image, hv_ModelID, hv_AngleStart_Find, hv_AngleExtent_Find,
                            hv_MinScore_Find, hv_NumMatches_Find, hv_MaxOverlap_Find, hv_SubPixel_Find,
                            hv_NumLevels_Find, hv_Greediness_Find, out hv_Row_Find, out hv_Column_Find, out hv_Angle_Find,
                            out hv_Score_Find);

                        if (hv_Row_Find.Length != 1)
                        {
                            errorFlag = true;
                            return false;
                        }
                        else
                        {
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row_Find, hv_Column_Find, hv_Angle_Find, out hv_HomMat2D);

                            HOperatorSet.VectorAngleToRigid(hv_CenterRow,hv_CenterCol, 0, hv_Row_Find, hv_Column_Find,hv_Angle_Find,
                                out hv_ROIHomMat2D);            //产生ROI仿射变换矩阵

                            errorFlag = false;
                            return true;
                        }
                    }
                    catch (Exception exc)
                    {
                        errorFlag = true;
                        return false;
                    }
                    break;
                case "auto_scaled":
                    try
                    {
                        HOperatorSet.FindScaledShapeModel(ho_Image, hv_ModelID, hv_AngleStart_Find, hv_AngleExtent_Find,
                        hv_ScaleRMin_Find, hv_ScaleRMax_Find, hv_MinScore_Find, hv_NumMatches_Find, hv_MaxOverlap_Find,
                        hv_SubPixel_Find, hv_NumLevels_Find, hv_Greediness_Find, out hv_Row_Find, out hv_Column_Find,
                        out hv_Angle_Find, out hv_ScaleR_Find, out hv_Score_Find);
                        if (hv_Row_Find.Length < 1)
                        {
                            errorFlag = true;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception exc)
                    {
                        errorFlag = true;
                        return false;
                    }
                    break;
                case "scaled":
                    try
                    {
                        HOperatorSet.FindScaledShapeModel(ho_Image, hv_ModelID, hv_AngleStart_Find, hv_AngleExtent_Find,
                        hv_ScaleRMin_Find, hv_ScaleRMax_Find, hv_MinScore_Find, hv_NumMatches_Find, hv_MaxOverlap_Find,
                        hv_SubPixel_Find, hv_NumLevels_Find, hv_Greediness_Find, out hv_Row_Find, out hv_Column_Find,
                        out hv_Angle_Find, out hv_ScaleR_Find, out hv_Score_Find);
                        if (hv_Row_Find.Length < 1)
                        {
                            errorFlag = true;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception exc)
                    {
                        errorFlag = true;
                        return false;
                    }
                    break;
                case "auto_aniso":
                    try
                    {
                        HOperatorSet.FindAnisoShapeModel(ho_Image, hv_ModelID, hv_AngleStart_Find, hv_AngleExtent_Find,
                            hv_ScaleRMin_Find, hv_ScaleRMax_Find, hv_ScaleCMin_Find, hv_ScaleCMax_Find, hv_MinScore_Find,
                            hv_NumMatches_Find, hv_MaxOverlap_Find, hv_SubPixel_Find, hv_NumLevels_Find, hv_Greediness_Find,
                            out hv_Row_Find, out hv_Column_Find, out hv_Angle_Find, out hv_ScaleR_Find, out hv_ScaleC_Find,
                            out hv_Score_Find);
                        if (hv_Row_Find.Length < 1)
                        {
                            errorFlag = true;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception exc)
                    {
                        errorFlag = true;
                        return false;
                    }
                    break;
                case "aniso":
                    try
                    {
                        HOperatorSet.FindAnisoShapeModel(ho_Image, hv_ModelID, hv_AngleStart_Find, hv_AngleExtent_Find,
                            hv_ScaleRMin_Find, hv_ScaleRMax_Find, hv_ScaleCMin_Find, hv_ScaleCMax_Find, hv_MinScore_Find,
                            hv_NumMatches_Find, hv_MaxOverlap_Find, hv_SubPixel_Find, hv_NumLevels_Find, hv_Greediness_Find,
                            out hv_Row_Find, out hv_Column_Find, out hv_Angle_Find, out hv_ScaleR_Find, out hv_ScaleC_Find,
                            out hv_Score_Find);
                        if (hv_Row_Find.Length < 1)
                        {
                            errorFlag = true;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch (Exception exc)
                    {
                        errorFlag = true;
                        return false;
                    }
                    break;
                default:
                    {
                        errorFlag = true;
                        return false;
                    }
                    
            }
        }

        public bool CreateTemplate()
        {
            switch (hv_ScaleMethod.S)
            {
                case "auto_none":
                    try
                    {
                        HOperatorSet.CreateShapeModel(ho_Template, "auto", hv_AngleStart_Create, hv_AngleExtent_Create,
                        "auto", "auto", hv_Metric_Create, "auto", "auto", out hv_ModelID);
                        return true;
                    }
                    catch (Exception exc)
                    {
                        return false;
                    }
                //break;
                case "none":
                    try
                    {                      
                        HOperatorSet.CreateShapeModel(ho_Template, hv_NumLevels_Create, hv_AngleStart_Create, hv_AngleExtent_Create,
                        hv_AngleStep_Create, hv_Optimization_Create, hv_Metric_Create, hv_Contrast_Create, hv_MinContrast_Create,
                        out hv_ModelID);

                        return true;
                    }
                    catch (Exception exc)
                    {
                        return false;
                    }
                //break;
                case "auto_scaled":
                    try
                    {
                        HOperatorSet.CreateScaledShapeModel(ho_Template, "auto", hv_AngleStart_Create, hv_AngleExtent_Create,
                        "auto", hv_ScaleRMin_Create, hv_ScaleRMax_Create, "auto", "auto", hv_Metric_Create, "auto", "auto",
                        out hv_ModelID);
                        return true;
                    }
                    catch (Exception exc)
                    {
                        return false;
                    }
                //break;
                case "scaled":
                    try
                    {
                        HOperatorSet.CreateScaledShapeModel(ho_Template, hv_NumLevels_Create, hv_AngleStart_Create, hv_AngleExtent_Create,
                        hv_ScaleRStep_Create, hv_ScaleRMin_Create, hv_ScaleRMax_Create, hv_ScaleRStep_Create, hv_Optimization_Create,
                        hv_Metric_Create, "auto", "auto", out hv_ModelID);
                        return true;
                    }
                    catch (Exception exc)
                    {
                        return false;
                    }
                //break;
                case "auto_aniso":
                    try
                    {
                        HOperatorSet.CreateAnisoShapeModel(ho_Template, "auto", hv_AngleStart_Create, hv_AngleExtent_Create,
                        "auto", hv_ScaleRMin_Create, hv_ScaleRMax_Create, "auto", hv_ScaleCMin_Create, hv_ScaleCMax_Create,
                        "auto", "auto", hv_Metric_Create, "auto", "auto", out hv_ModelID);
                        return true;
                    }
                    catch (Exception exc)
                    {
                        return false;
                    }
                //break;
                case "aniso":
                    try
                    {
                        HOperatorSet.CreateAnisoShapeModel(ho_Template, hv_NumLevels_Create, hv_AngleStart_Create, hv_AngleExtent_Create,
                        hv_AngleStep_Create, hv_ScaleRMin_Create, hv_ScaleRMax_Create, hv_ScaleRStep_Create, hv_ScaleCMin_Create,
                        hv_ScaleCMax_Create, hv_ScaleCStep_Create, hv_Optimization_Create, hv_Metric_Create, hv_Contrast_Create, hv_MinContrast_Create,
                        out hv_ModelID);
                        return true;
                    }
                    catch (Exception exc)
                    {
                        return false;
                    }
                //break;
                default:
                    {
                        return false;
                    }
                    //break;
            }
        }

        public bool WriteParams(String xmlNode, string templateName, string regionName)
        {
            try
            {
                string shv_ScaleMethod = "Parameters/" + xmlNode + "/hv_ScaleMethod";

                string shv_NumLevels_Create = "Parameters/" + xmlNode + "/hv_NumLevels_Create";
                string shv_AngleStart_Create = "Parameters/" + xmlNode + "/hv_AngleStart_Create";
                string shv_AngleExtent_Create = "Parameters/" + xmlNode + "/hv_AngleExtent_Create";
                string shv_AngleStep_Create = "Parameters/" + xmlNode + "/hv_AngleStep_Create";
                string shv_ScaleRMin_Create = "Parameters/" + xmlNode + "/hv_ScaleRMin_Create";
                string shv_ScaleRMax_Create = "Parameters/" + xmlNode + "/hv_ScaleRMax_Create";
                string shv_ScaleRStep_Create = "Parameters/" + xmlNode + "/hv_ScaleRStep_Create";

                string shv_ScaleCMin_Create = "Parameters/" + xmlNode + "/hv_ScaleCMin_Create";
                string shv_ScaleCMax_Create = "Parameters/" + xmlNode + "/hv_ScaleCMax_Create";
                string shv_ScaleCStep_Create = "Parameters/" + xmlNode + "/hv_ScaleCStep_Create";
                string shv_Optimization_Create = "Parameters/" + xmlNode + "/hv_Optimization_Create";
                string shv_Metric_Create = "Parameters/" + xmlNode + "/hv_Metric_Create";
                string shv_Contrast_Create = "Parameters/" + xmlNode + "/hv_Contrast_Create";
                string shv_MinContrast_Create = "Parameters/" + xmlNode + "/hv_MinContrast_Create";

                string shv_AngleStart_Find = "Parameters/" + xmlNode + "/hv_AngleStart_Find";
                string shv_AngleExtent_Find = "Parameters/" + xmlNode + "/hv_AngleExtent_Find";
                string shv_ScaleRMin_Find = "Parameters/" + xmlNode + "/hv_ScaleRMin_Find";
                string shv_ScaleRMax_Find = "Parameters/" + xmlNode + "/hv_ScaleRMax_Find";
                string shv_ScaleCMin_Find = "Parameters/" + xmlNode + "/hv_ScaleCMin_Find";
                string shv_ScaleCMax_Find = "Parameters/" + xmlNode + "/hv_ScaleCMax_Find";
                string shv_MinScore_Find = "Parameters/" + xmlNode + "/hv_MinScore_Find";
                string shv_NumMatches_Find = "Parameters/" + xmlNode + "/hv_NumMatches_Find";
                string shv_MaxOverlap_Find = "Parameters/" + xmlNode + "/hv_MaxOverlap_Find";
                string shv_SubPixel_Find = "Parameters/" + xmlNode + "/hv_SubPixel_Find";
                string shv_NumLevels_Find = "Parameters/" + xmlNode + "/hv_NumLevels_Find";
                string shv_Greediness_Find = "Parameters/" + xmlNode + "/hv_Greediness_Find";

                string shv_CenterRow = "Parameters/" + xmlNode + "/hv_CenterRow";
                string shv_CenterCol = "Parameters/" + xmlNode + "/hv_CenterCol";

                xmlRW.Update(shv_ScaleMethod, hv_ScaleMethod);

                xmlRW.Update(shv_NumLevels_Create, hv_NumLevels_Create.ToString());
                xmlRW.Update(shv_AngleStart_Create, hv_AngleStart_Create.ToString());
                xmlRW.Update(shv_AngleExtent_Create, hv_AngleExtent_Create.ToString());
                xmlRW.Update(shv_AngleStep_Create, hv_AngleStep_Create.ToString());
                xmlRW.Update(shv_ScaleRMin_Create, hv_ScaleRMin_Create.ToString());
                xmlRW.Update(shv_ScaleRMax_Create, hv_ScaleRMax_Create.ToString());
                xmlRW.Update(shv_ScaleRStep_Create, hv_ScaleRStep_Create.ToString());
                xmlRW.Update(shv_ScaleCMin_Create, hv_ScaleCMin_Create.ToString());
                xmlRW.Update(shv_ScaleCMax_Create, hv_ScaleCMax_Create.ToString());
                xmlRW.Update(shv_ScaleCStep_Create, hv_ScaleCStep_Create.ToString());
                xmlRW.Update(shv_Optimization_Create, hv_Optimization_Create);
                xmlRW.Update(shv_Metric_Create, hv_Metric_Create);
                xmlRW.Update(shv_Contrast_Create, hv_Contrast_Create.ToString());
                xmlRW.Update(shv_MinContrast_Create, hv_MinContrast_Create.ToString());

                xmlRW.Update(shv_AngleStart_Find, hv_AngleStart_Find.ToString());
                xmlRW.Update(shv_AngleExtent_Find, hv_AngleExtent_Find.ToString());
                xmlRW.Update(shv_ScaleRMin_Find, hv_ScaleRMin_Find.ToString());
                xmlRW.Update(shv_ScaleRMax_Find, hv_ScaleRMax_Find.ToString());
                xmlRW.Update(shv_ScaleCMin_Find, hv_ScaleCMin_Find.ToString());
                xmlRW.Update(shv_ScaleCMax_Find, hv_ScaleCMax_Find.ToString());
                xmlRW.Update(shv_MinScore_Find, hv_MinScore_Find.ToString());
                xmlRW.Update(shv_NumMatches_Find, hv_NumMatches_Find.ToString());
                xmlRW.Update(shv_MaxOverlap_Find, hv_MaxOverlap_Find.ToString());
                xmlRW.Update(shv_SubPixel_Find, hv_SubPixel_Find);
                xmlRW.Update(shv_NumLevels_Find, hv_NumLevels_Find.ToString());
                xmlRW.Update(shv_Greediness_Find, hv_Greediness_Find.ToString());

                xmlRW.Update(shv_CenterRow, hv_CenterRow.ToString());
                xmlRW.Update(shv_CenterCol, hv_CenterCol.ToString());

                string modelIDPath = AppDomain.CurrentDomain.BaseDirectory + @".//Parameters//" + templateName;
                HOperatorSet.WriteShapeModel(hv_ModelID,modelIDPath);

                string regionPath = AppDomain.CurrentDomain.BaseDirectory + @".//Parameters//" + regionName;
                HOperatorSet.WriteRegion(ho_Region, regionPath);

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }

        public bool ReadParams(String xmlNode, string templateName, string imageName, string regionName)
        {
            try
            {
                string shv_ScaleMethod = "Parameters/" + xmlNode + "/hv_ScaleMethod";

                string shv_NumLevels_Create = "Parameters/" + xmlNode + "/hv_NumLevels_Create";
                string shv_AngleStart_Create = "Parameters/" + xmlNode + "/hv_AngleStart_Create";             
                string shv_AngleExtent_Create = "Parameters/" + xmlNode + "/hv_AngleExtent_Create";           
                string shv_AngleStep_Create = "Parameters/" + xmlNode + "/hv_AngleStep_Create";             
                string shv_ScaleRMin_Create = "Parameters/" + xmlNode + "/hv_ScaleRMin_Create";              
                string shv_ScaleRMax_Create = "Parameters/" + xmlNode + "/hv_ScaleRMax_Create";             
                string shv_ScaleRStep_Create = "Parameters/" + xmlNode + "/hv_ScaleRStep_Create";             

                string shv_ScaleCMin_Create = "Parameters/" + xmlNode + "/hv_ScaleCMin_Create";             
                string shv_ScaleCMax_Create = "Parameters/" + xmlNode + "/hv_ScaleCMax_Create";             
                string shv_ScaleCStep_Create = "Parameters/" + xmlNode + "/hv_ScaleCStep_Create";            
                string shv_Optimization_Create = "Parameters/" + xmlNode + "/hv_Optimization_Create";          
                string shv_Metric_Create = "Parameters/" + xmlNode + "/hv_Metric_Create";                
                string shv_Contrast_Create = "Parameters/" + xmlNode + "/hv_Contrast_Create";             
                string shv_MinContrast_Create = "Parameters/" + xmlNode + "/hv_MinContrast_Create";          

                string shv_AngleStart_Find = "Parameters/" + xmlNode + "/hv_AngleStart_Find";             
                string shv_AngleExtent_Find = "Parameters/" + xmlNode + "/hv_AngleExtent_Find";            
                string shv_ScaleRMin_Find = "Parameters/" + xmlNode + "/hv_ScaleRMin_Find";              
                string shv_ScaleRMax_Find = "Parameters/" + xmlNode + "/hv_ScaleRMax_Find";            
                string shv_ScaleCMin_Find = "Parameters/" + xmlNode + "/hv_ScaleCMin_Find";             
                string shv_ScaleCMax_Find = "Parameters/" + xmlNode + "/hv_ScaleCMax_Find";            
                string shv_MinScore_Find = "Parameters/" + xmlNode + "/hv_MinScore_Find";               
                string shv_NumMatches_Find = "Parameters/" + xmlNode + "/hv_NumMatches_Find";             
                string shv_MaxOverlap_Find = "Parameters/" + xmlNode + "/hv_MaxOverlap_Find";            
                string shv_SubPixel_Find = "Parameters/" + xmlNode + "/hv_SubPixel_Find";               
                string shv_NumLevels_Find = "Parameters/" + xmlNode + "/hv_NumLevels_Find";              
                string shv_Greediness_Find = "Parameters/" + xmlNode + "/hv_Greediness_Find";

                string shv_CenterRow = "Parameters/" + xmlNode + "/hv_CenterRow";
                string shv_CenterCol = "Parameters/" + xmlNode + "/hv_CenterCol";

                hv_ScaleMethod = xmlRW.Read(shv_ScaleMethod);
                hv_NumLevels_Create = Convert.ToInt32(xmlRW.Read(shv_NumLevels_Create));
                hv_AngleStart_Create = Convert.ToDouble(xmlRW.Read(shv_AngleStart_Create));
                hv_AngleExtent_Create = Convert.ToDouble(xmlRW.Read(shv_AngleExtent_Create));
                hv_AngleStep_Create = Convert.ToDouble(xmlRW.Read(shv_AngleStep_Create));
                hv_ScaleRMin_Create = Convert.ToDouble(xmlRW.Read(shv_ScaleRMin_Create));
                hv_ScaleRMax_Create = Convert.ToDouble(xmlRW.Read(shv_ScaleRMax_Create));
                hv_ScaleRStep_Create = Convert.ToDouble(xmlRW.Read(shv_ScaleRStep_Create));

                hv_ScaleCMin_Create = Convert.ToDouble(xmlRW.Read(shv_ScaleCMin_Create));
                hv_ScaleCMax_Create = Convert.ToDouble(xmlRW.Read(shv_ScaleCMax_Create));
                hv_ScaleCStep_Create = Convert.ToDouble(xmlRW.Read(shv_ScaleCStep_Create));
                hv_Optimization_Create = xmlRW.Read(shv_Optimization_Create);
                hv_Metric_Create =xmlRW.Read(shv_Metric_Create);
                hv_Contrast_Create = Convert.ToInt32(xmlRW.Read(shv_Contrast_Create));
                hv_MinContrast_Create = Convert.ToInt32(xmlRW.Read(shv_MinContrast_Create));

                hv_AngleStart_Find = Convert.ToDouble(xmlRW.Read(shv_AngleStart_Find));
                hv_AngleExtent_Find = Convert.ToDouble(xmlRW.Read(shv_AngleExtent_Find));
                hv_ScaleRMin_Find = Convert.ToDouble(xmlRW.Read(shv_ScaleRMin_Find));
                hv_ScaleRMax_Find = Convert.ToDouble(xmlRW.Read(shv_ScaleRMax_Find));
                hv_ScaleCMin_Find = Convert.ToDouble(xmlRW.Read(shv_ScaleCMin_Find));
                hv_ScaleCMax_Find = Convert.ToDouble(xmlRW.Read(shv_ScaleCMax_Find));
                hv_MinScore_Find = Convert.ToDouble(xmlRW.Read(shv_MinScore_Find));
                hv_NumMatches_Find = Convert.ToInt32(xmlRW.Read(shv_NumMatches_Find));
                hv_MaxOverlap_Find = Convert.ToDouble(xmlRW.Read(shv_MaxOverlap_Find));
                hv_SubPixel_Find =xmlRW.Read(shv_SubPixel_Find);
                hv_NumLevels_Find = Convert.ToInt32(xmlRW.Read(shv_NumLevels_Find));
                hv_Greediness_Find = Convert.ToDouble(xmlRW.Read(shv_Greediness_Find));

                hv_CenterRow = Convert.ToDouble(xmlRW.Read(shv_CenterRow));
                hv_CenterCol = Convert.ToDouble(xmlRW.Read(shv_CenterCol));

                string modelIDPath = AppDomain.CurrentDomain.BaseDirectory + @".//Parameters//" + templateName;
                HOperatorSet.ReadShapeModel(modelIDPath,out hv_ModelID);

                string imagePath = AppDomain.CurrentDomain.BaseDirectory + "Parameters\\" + imageName;
                HOperatorSet.ReadImage(out ho_Image, imagePath);

                string regionPath = AppDomain.CurrentDomain.BaseDirectory + "Parameters\\" + regionName;
                HOperatorSet.ReadRegion(out ho_Region, regionPath);

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }
    }
}
