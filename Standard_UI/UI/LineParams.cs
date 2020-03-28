using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Standard_UI.UI
{
    public class LineParams
    {
        public bool errorFlag;

        public HObject ho_Image;                 //源图像

        //测量参数
        public HTuple hv_MetrologyHandle;        //测量句柄
        public HTuple hv_Width;
        public HTuple hv_Height;

        public HTuple hv_Row1;  //直线模型
        public HTuple hv_Column1;
        public HTuple hv_Row2;
        public HTuple hv_Column2;

        public HTuple hv_MeasureLength1;
        public HTuple hv_MeasureLength2;
        public HTuple hv_MeasureSigma;
        public HTuple hv_MeasureThreshold;
        public HTuple hv_GenParamName;
        public HTuple hv_GenParamValue;
        public HTuple hv_Index;

        public HTuple hv_LineRowBegin;     //检测到的直线参数
        public HTuple hv_LineColumnBegin;
        public HTuple hv_LineRowEnd;
        public HTuple hv_LineColumnEnd;

        public HTuple hv_LineRowBegin_Real;     //检测到的直线参数
        public HTuple hv_LineColumnBegin_Real;
        public HTuple hv_LineRowEnd_Real;
        public HTuple hv_LineColumnEnd_Real;

        public HTuple hv_Model;
        public HTuple hv_RegionMin;
        public HTuple hv_RegionMax;

        ParametersRW.XmlRW xmlRW;

        #region 构造函数
        public LineParams()
        {
            errorFlag = false;

            hv_Row1=new HTuple();
            hv_Column1 = new HTuple();
            hv_Row2 = new HTuple();
            hv_Column2 = new HTuple();

            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
            hv_MeasureLength1 = 40;
            hv_MeasureLength2 = 5;
            hv_MeasureSigma = 1.0;
            hv_MeasureThreshold = 30;
            //hv_GenParamName = "measure_transition";
            hv_GenParamName = "measure_transition";
            hv_GenParamValue = "negative";   //白到黑，根据绘制箭头方向，如箭头朝右，上白下黑
            hv_Index = null;

            hv_Model = 0;
            hv_RegionMin = 0;
            hv_RegionMax = 255;

            xmlRW = new ParametersRW.XmlRW();

            hv_LineRowBegin=0;     //检测到的直线参数
            hv_LineColumnBegin = 0;
            hv_LineRowEnd = 0;
            hv_LineColumnEnd = 0;

            hv_LineRowBegin_Real = 0;     //检测到的直线参数
            hv_LineColumnBegin_Real = 0;
            hv_LineRowEnd_Real = 0;
            hv_LineColumnEnd_Real = 0;
    }
        #endregion

        public bool CreateLineModel()
        {
            try
            {
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                if (ho_Image == null)
                {
                    return false;
                }

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                if (hv_Row1.Length < 1)
                {
                    return false;
                }

                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row1, hv_Column1, hv_Row2, hv_Column2,
                    hv_MeasureLength1, hv_MeasureLength2, hv_MeasureSigma, hv_MeasureThreshold,
                    hv_GenParamName, hv_GenParamValue, out hv_Index);

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }

        public bool CreateLineModel(HTuple hv_HomMat2D)
        {
            try
            {
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle);
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                if (ho_Image == null)
                {
                    return false;
                }

                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                if (hv_Row1.Length < 1)
                {
                    return false;
                }

                HTuple mhv_Row1;
                HTuple mhv_Column1;
                HTuple mhv_Row2;
                HTuple mhv_Column2;

                HOperatorSet.AffineTransPixel(hv_HomMat2D, hv_Row1, hv_Column1, out mhv_Row1, out mhv_Column1);
                HOperatorSet.AffineTransPixel(hv_HomMat2D, hv_Row2, hv_Column2, out mhv_Row2, out mhv_Column2);

                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, mhv_Row1, mhv_Column1, mhv_Row2, mhv_Column2,
                    hv_MeasureLength1, hv_MeasureLength2, hv_MeasureSigma, hv_MeasureThreshold,
                    hv_GenParamName, hv_GenParamValue, out hv_Index);

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }

        public bool FindLine()
        {
            if (ho_Image == null)
            {
                errorFlag = true;
                return false;
            }

            if (hv_Model != 0)
            {
                HObject ho_Region;

                HOperatorSet.Threshold(ho_Image, out ho_Region, hv_RegionMin, hv_RegionMax);

                HOperatorSet.ReduceDomain(ho_Image, ho_Region, out ho_Image);
            }

            if (!CreateLineModel())
            {
                errorFlag = true;
                return false;
            }

            try
            {
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);

                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "row_begin", out hv_LineRowBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "column_begin", out hv_LineColumnBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "row_end", out hv_LineRowEnd);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "column_end", out hv_LineColumnEnd);

                if (hv_LineRowBegin.Length != 1)
                {
                    errorFlag = true;
                    return false;
                }
                hv_LineRowBegin_Real = hv_LineRowBegin;
                hv_LineColumnBegin_Real = hv_LineColumnBegin;
                hv_LineRowEnd_Real = hv_LineRowEnd;
                hv_LineColumnEnd_Real = hv_LineColumnEnd;

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }

        public bool FindLine(HTuple hv_HomMat2D, HTuple hv_HomMat2DPix)
        {
            if (ho_Image == null)
            {
                errorFlag = true;
                return false;
            }

            if (hv_Model != 0)
            {
                //HTuple hv_Width;
                //HTuple hv_Height;
                //HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //for (int i = 0; i < hv_Height; i++)
                //{

                //    for (int j = 0; j < hv_Width; j++)
                //    {
                //        HTuple hv_Grayval;
                //        HOperatorSet.GetGrayval(ho_Image, i, j, out hv_Grayval);
                //        if (hv_Grayval <= hv_RegionMax && hv_Grayval >= hv_RegionMin)
                //        {
                //            HOperatorSet.SetGrayval(ho_Image, i, j, 255);
                //        }
                //    }
                //}

                HObject ho_Region;

                HOperatorSet.Threshold(ho_Image, out ho_Region, hv_RegionMin, hv_RegionMax);

                HOperatorSet.ReduceDomain(ho_Image, ho_Region, out ho_Image);
                //string Path = AppDomain.CurrentDomain.BaseDirectory + @".//Parameters//test"+ DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff");
                //HOperatorSet.WriteImage(ho_Image,"bmp",0, Path);
            }

            if (!CreateLineModel(hv_HomMat2D))
            {
                errorFlag = true;
                return false;
            }

            try
            {
                HOperatorSet.ApplyMetrologyModel(ho_Image, hv_MetrologyHandle);

                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "row_begin", out hv_LineRowBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "column_begin", out hv_LineColumnBegin);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "row_end", out hv_LineRowEnd);
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "column_end", out hv_LineColumnEnd);

                if (hv_LineRowBegin.Length != 1)
                {
                    errorFlag = true;
                    return false;
                }

                HOperatorSet.AffineTransPixel(hv_HomMat2DPix, hv_LineRowBegin, hv_LineColumnBegin, out hv_LineRowBegin_Real, out hv_LineColumnBegin_Real);
                HOperatorSet.AffineTransPixel(hv_HomMat2DPix, hv_LineRowEnd, hv_LineColumnEnd, out hv_LineRowEnd_Real, out hv_LineColumnEnd_Real);

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }

        public bool ReadParams(String xmlNode)
        {
            try
            {
                string shv_MeasureLength1 = "Parameters/" + xmlNode + "/hv_MeasureLength1";
                string shv_MeasureLength2 = "Parameters/" + xmlNode + "/hv_MeasureLength2";
                string shv_MeasureSigma = "Parameters/" + xmlNode + "/hv_MeasureSigma";
                string shv_MeasureThreshold = "Parameters/" + xmlNode + "/hv_MeasureThreshold";
                string shv_GenParamName = "Parameters/" + xmlNode + "/hv_GenParamName";
                string shv_GenParamValue = "Parameters/" + xmlNode + "/hv_GenParamValue";

                string shv_Row1 = "Parameters/" + xmlNode + "/hv_Row1";
                string shv_Column1 = "Parameters/" + xmlNode + "/hv_Column1";
                string shv_Row2= "Parameters/" + xmlNode + "/hv_Row2";
                string shv_Column2 = "Parameters/" + xmlNode + "/hv_Column2";

                string shv_Model = "Parameters/" + xmlNode + "/hv_Model";
                string shv_RegionMin = "Parameters/" + xmlNode + "/hv_RegionMin";
                string shv_RegionMax = "Parameters/" + xmlNode + "/hv_RegionMax";

                hv_MeasureLength1 = Convert.ToInt32(xmlRW.Read(shv_MeasureLength1));
                hv_MeasureLength2 = Convert.ToInt32(xmlRW.Read(shv_MeasureLength2));
                hv_MeasureSigma = Convert.ToDouble(xmlRW.Read(shv_MeasureSigma));
                hv_MeasureThreshold = Convert.ToInt32(xmlRW.Read(shv_MeasureThreshold));
                hv_GenParamName = xmlRW.Read(shv_GenParamName);
                hv_GenParamValue = xmlRW.Read(shv_GenParamValue);

                hv_Row1 = Convert.ToDouble(xmlRW.Read(shv_Row1));
                hv_Column1= Convert.ToDouble(xmlRW.Read(shv_Column1));
                hv_Row2= Convert.ToDouble(xmlRW.Read(shv_Row2));
                hv_Column2 = Convert.ToDouble(xmlRW.Read(shv_Column2));

                hv_Model = Convert.ToInt32(xmlRW.Read(shv_Model));
                hv_RegionMin = Convert.ToInt32(xmlRW.Read(shv_RegionMin));
                hv_RegionMax = Convert.ToInt32(xmlRW.Read(shv_RegionMax));

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }
        public bool WriteParams(String xmlNode)
        {
            try
            {
                string shv_MeasureLength1 = "Parameters/" + xmlNode + "/hv_MeasureLength1";
                string shv_MeasureLength2 = "Parameters/" + xmlNode + "/hv_MeasureLength2";
                string shv_MeasureSigma = "Parameters/" + xmlNode + "/hv_MeasureSigma";
                string shv_MeasureThreshold = "Parameters/" + xmlNode + "/hv_MeasureThreshold";
                string shv_GenParamName = "Parameters/" + xmlNode + "/hv_GenParamName";
                string shv_GenParamValue = "Parameters/" + xmlNode + "/hv_GenParamValue";

                string shv_Row1 = "Parameters/" + xmlNode + "/hv_Row1";
                string shv_Column1 = "Parameters/" + xmlNode + "/hv_Column1";
                string shv_Row2 = "Parameters/" + xmlNode + "/hv_Row2";
                string shv_Column2 = "Parameters/" + xmlNode + "/hv_Column2";

                xmlRW.Update(shv_MeasureLength1, hv_MeasureLength1.ToString());
                xmlRW.Update(shv_MeasureLength2, hv_MeasureLength2.ToString());
                xmlRW.Update(shv_MeasureSigma, hv_MeasureSigma.ToString());
                xmlRW.Update(shv_MeasureThreshold, hv_MeasureThreshold.ToString());
                xmlRW.Update(shv_GenParamName, hv_GenParamName);
                xmlRW.Update(shv_GenParamValue, hv_GenParamValue);

                xmlRW.Update(shv_Row1, hv_Row1.ToString());
                xmlRW.Update(shv_Column1, hv_Column1.ToString());
                xmlRW.Update(shv_Row2, hv_Row2.ToString());
                xmlRW.Update(shv_Column2, hv_Column2.ToString());

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }

        public bool CopyTo(ref LineParams lineParams)
        {
            try
            {
                lineParams.ho_Image = ho_Image;
                lineParams.hv_MetrologyHandle = hv_MetrologyHandle;

                lineParams.hv_Width = hv_Width;
                lineParams.hv_Height = hv_Height;

                lineParams.hv_Row1 = hv_Row1;
                lineParams.hv_Column1 = hv_Column1;
                lineParams.hv_Row2 = hv_Row2;
                lineParams.hv_Column2 = hv_Column2;

                lineParams.hv_MeasureLength1 = hv_MeasureLength1;
                lineParams.hv_MeasureLength2 = hv_MeasureLength2;

                lineParams.hv_MeasureSigma = hv_MeasureSigma;
                lineParams.hv_MeasureThreshold = hv_MeasureThreshold;
                lineParams.hv_GenParamName = hv_GenParamName;
                lineParams.hv_GenParamValue = hv_GenParamValue;
                lineParams.hv_Index = hv_Index;

                lineParams.hv_Model = hv_Model;
                lineParams.hv_RegionMin = hv_RegionMin;
                lineParams.hv_RegionMax = hv_RegionMax;

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
