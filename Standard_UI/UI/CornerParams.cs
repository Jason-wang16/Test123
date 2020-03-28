using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Standard_UI.UI
{
    public class CornerParams
    {
        public bool errorFlag;

        #region 直线模型参数
        public HObject ho_Image;                 //源图像

        public HTuple hv_MetrologyHandle;        //测量句柄

        public HTuple hv_Width;
        public HTuple hv_Height;

        public HTuple hv_Row1_Horizon;  //水平线
        public HTuple hv_Column1_Horizon;
        public HTuple hv_Row2_Horizon;
        public HTuple hv_Column2_Horizon;

        public HTuple hv_Row1_Vertical;  //竖直线
        public HTuple hv_Column1_Vertical;
        public HTuple hv_Row2_Vertical;
        public HTuple hv_Column2_Vertical;

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

        public HTuple hv_PointRow;
        public HTuple hv_PointColumn;

        public HTuple hv_LineRowBegin_Real;     
        public HTuple hv_LineColumnBegin_Real;
        public HTuple hv_LineRowEnd_Real;
        public HTuple hv_LineColumnEnd_Real;

        public HTuple hv_PointRow_Real;
        public HTuple hv_PointColumn_Real;

        public HTuple hv_Number;

        public HTuple hv_Model;
        public HTuple hv_RegionMin;
        public HTuple hv_RegionMax;

        ParametersRW.XmlRW xmlRW;

        #endregion

        #region 构造函数
        public CornerParams()
        {
            errorFlag = false;

            HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);

            hv_Row1_Horizon = new HTuple();
            hv_Column1_Horizon = new HTuple();
            hv_Row2_Horizon = new HTuple();
            hv_Column2_Horizon = new HTuple();

            hv_Row1_Vertical = new HTuple();
            hv_Column1_Vertical = new HTuple();
            hv_Row2_Vertical = new HTuple();
            hv_Column2_Vertical = new HTuple();

            hv_MeasureLength1 = 40;
            hv_MeasureLength2 = 5;

            hv_MeasureSigma = 1.0;
            hv_MeasureThreshold = 30;
            //hv_GenParamName = "measure_transition";
            hv_GenParamName = "measure_transition";
            hv_GenParamValue = "negative";   //白到黑，根据绘制箭头方向，如箭头朝右，上白下黑

            hv_Index = null;

            hv_Number = 2;

            hv_Model=0;
            hv_RegionMin=0;
            hv_RegionMax=255;

            xmlRW = new ParametersRW.XmlRW();
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

                if (hv_Row1_Horizon.Length < 1)
                {
                    return false;
                }

                if (hv_Row1_Vertical.Length < 1)
                {
                    return false;
                }

                HTuple hv_Row1 = null;
                HTuple hv_Column1 = null;
                HTuple hv_Row2 = null;
                HTuple hv_Column2 = null;

                HOperatorSet.TupleInsert(hv_Row1_Horizon, 0, hv_Row1_Vertical, out hv_Row1);
                HOperatorSet.TupleInsert(hv_Column1_Horizon, 0, hv_Column1_Vertical, out hv_Column1);
                HOperatorSet.TupleInsert(hv_Row2_Horizon, 0, hv_Row2_Vertical, out hv_Row2);
                HOperatorSet.TupleInsert(hv_Column2_Horizon, 0, hv_Column2_Vertical, out hv_Column2);

                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_Row1, hv_Column1, hv_Row2, hv_Column2,
                    hv_MeasureLength1, hv_MeasureLength2,hv_MeasureSigma, hv_MeasureThreshold,
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

                if (hv_Row1_Horizon.Length < 1)
                {
                    return false;
                }

                if (hv_Row1_Vertical.Length < 1)
                {
                    return false;
                }

                HTuple hv_Row1 = null;
                HTuple hv_Column1 = null;
                HTuple hv_Row2 = null;
                HTuple hv_Column2 = null;

                HTuple mhv_Row1_Horizon;  //水平线
                HTuple mhv_Column1_Horizon;
                HTuple mhv_Row2_Horizon;
                HTuple mhv_Column2_Horizon;

                HTuple mhv_Row1_Vertical;  //竖直线
                HTuple mhv_Column1_Vertical;
                HTuple mhv_Row2_Vertical;
                HTuple mhv_Column2_Vertical;

                HOperatorSet.AffineTransPixel(hv_HomMat2D, hv_Row1_Horizon, hv_Column1_Horizon,out mhv_Row1_Horizon,out mhv_Column1_Horizon);
                HOperatorSet.AffineTransPixel(hv_HomMat2D, hv_Row2_Horizon, hv_Column2_Horizon, out mhv_Row2_Horizon, out mhv_Column2_Horizon);
                HOperatorSet.AffineTransPixel(hv_HomMat2D, hv_Row1_Vertical, hv_Column1_Vertical, out mhv_Row1_Vertical, out mhv_Column1_Vertical);
                HOperatorSet.AffineTransPixel(hv_HomMat2D, hv_Row2_Vertical, hv_Column2_Vertical, out mhv_Row2_Vertical, out mhv_Column2_Vertical);

                HOperatorSet.TupleInsert(mhv_Row1_Horizon, 0, mhv_Row1_Vertical, out hv_Row1);
                HOperatorSet.TupleInsert(mhv_Column1_Horizon, 0, mhv_Column1_Vertical, out hv_Column1);
                HOperatorSet.TupleInsert(mhv_Row2_Horizon, 0, mhv_Row2_Vertical, out hv_Row2);
                HOperatorSet.TupleInsert(mhv_Column2_Horizon, 0, mhv_Column2_Vertical, out hv_Column2);

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
        public bool FindCorner()
        {
            if (ho_Image == null)
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

                if (hv_LineRowBegin.Length != hv_Number)
                {
                    errorFlag = true;
                    return false;
                }
                HTuple hv_IsOverlapping = null;
                HOperatorSet.IntersectionLines(hv_LineRowBegin[0], hv_LineColumnBegin[0], hv_LineRowEnd[0], hv_LineColumnEnd[0],
                    hv_LineRowBegin[1], hv_LineColumnBegin[1], hv_LineRowEnd[1], hv_LineColumnEnd[1],out hv_PointRow,out hv_PointColumn,
                    out hv_IsOverlapping);

                if (hv_PointRow.Length!=1)
                {
                    errorFlag = true;
                    return false;
                }

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }

        public bool FindCorner(HTuple hv_HomMat2D,HTuple hv_HomMat2DPix)
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

                if (hv_LineRowBegin.Length != hv_Number|| hv_LineRowEnd.Length!=hv_Number)
                {
                    errorFlag = true;
                    return false;
                }
                HTuple hv_IsOverlapping = null;
                HOperatorSet.IntersectionLines(hv_LineRowBegin[0], hv_LineColumnBegin[0], hv_LineRowEnd[0], hv_LineColumnEnd[0],
                    hv_LineRowBegin[1], hv_LineColumnBegin[1], hv_LineRowEnd[1], hv_LineColumnEnd[1], out hv_PointRow, out hv_PointColumn,
                    out hv_IsOverlapping);

                if (hv_PointRow.Length != 1)
                {
                    errorFlag = true;
                    return false;
                }

                HOperatorSet.AffineTransPixel(hv_HomMat2DPix, hv_LineRowBegin, hv_LineColumnBegin, out hv_LineRowBegin_Real, out hv_LineColumnBegin_Real);
                HOperatorSet.AffineTransPixel(hv_HomMat2DPix, hv_LineRowEnd, hv_LineColumnEnd, out hv_LineRowEnd_Real, out hv_LineColumnEnd_Real);

                HOperatorSet.AffineTransPixel(hv_HomMat2DPix, hv_PointRow, hv_PointColumn, out hv_PointRow_Real, out hv_PointColumn_Real);

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }
        public bool CopyTo(ref CornerParams cornerParams)
        {
            try
            {
                cornerParams.ho_Image = ho_Image;
                cornerParams.hv_MetrologyHandle = hv_MetrologyHandle;

                cornerParams.hv_Width = hv_Width;
                cornerParams.hv_Height = hv_Height;

                cornerParams.hv_Row1_Horizon = hv_Row1_Horizon;
                cornerParams.hv_Column1_Horizon = hv_Column1_Horizon;
                cornerParams.hv_Row2_Horizon = hv_Row2_Horizon;
                cornerParams.hv_Column2_Horizon = hv_Column2_Horizon;

                cornerParams.hv_MeasureLength1 = hv_MeasureLength1;
                cornerParams.hv_MeasureLength2 = hv_MeasureLength2;

                cornerParams.hv_MeasureSigma = hv_MeasureSigma;
                cornerParams.hv_MeasureThreshold = hv_MeasureThreshold;
                cornerParams.hv_GenParamName = hv_GenParamName;
                cornerParams.hv_GenParamValue = hv_GenParamValue;
                cornerParams.hv_Index = hv_Index;

                cornerParams.hv_Number = hv_Number;

                cornerParams.hv_Row1_Horizon = hv_Row1_Horizon;
                cornerParams.hv_Column1_Horizon = hv_Column1_Horizon;
                cornerParams.hv_Row2_Horizon = hv_Row2_Horizon;
                cornerParams.hv_Column2_Horizon = hv_Column2_Horizon;

                cornerParams.hv_Row1_Vertical = hv_Row1_Vertical;
                cornerParams.hv_Column1_Vertical = hv_Column1_Vertical;
                cornerParams.hv_Row2_Vertical = hv_Row2_Vertical;
                cornerParams.hv_Column2_Vertical = hv_Column2_Vertical;

                cornerParams.hv_Model = hv_Model;
                cornerParams.hv_RegionMin = hv_RegionMin;
                cornerParams.hv_RegionMax = hv_RegionMax;

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }
        public bool ReadLineModel(string lineModelPath)
        {
            try
            {
                HOperatorSet.ReadMetrologyModel(lineModelPath,out hv_MetrologyHandle);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public bool ReadParams(String xmlNode, string regionName)
        {
            try
            {
                string shv_MeasureLength1 = "Parameters/" + xmlNode + "/hv_MeasureLength1";
                string shv_MeasureLength2 = "Parameters/" + xmlNode + "/hv_MeasureLength2";
                string shv_MeasureSigma = "Parameters/" + xmlNode + "/hv_MeasureSigma";
                string shv_MeasureThreshold = "Parameters/" + xmlNode + "/hv_MeasureThreshold";
                string shv_GenParamName = "Parameters/" + xmlNode + "/hv_GenParamName";
                string shv_GenParamValue = "Parameters/" + xmlNode + "/hv_GenParamValue";
                string shv_Number = "Parameters/" + xmlNode + "/hv_Number";

                string shv_Row1_Horizon = "Parameters/" + xmlNode + "/hv_Row1_Horizon";
                string shv_Column1_Horizon = "Parameters/" + xmlNode + "/hv_Column1_Horizon";
                string shv_Row2_Horizon = "Parameters/" + xmlNode + "/hv_Row2_Horizon";
                string shv_Column2_Horizon = "Parameters/" + xmlNode + "/hv_Column2_Horizon";

                string shv_Row1_Vertical = "Parameters/" + xmlNode + "/hv_Row1_Vertical";
                string shv_Column1_Vertical = "Parameters/" + xmlNode + "/hv_Column1_Vertical";
                string shv_Row2_Vertical = "Parameters/" + xmlNode + "/hv_Row2_Vertical";
                string shv_Column2_Vertical = "Parameters/" + xmlNode + "/hv_Column2_Vertical";

                string shv_Model= "Parameters/" + xmlNode + "/hv_Model";
                string shv_RegionMin = "Parameters/" + xmlNode + "/hv_RegionMin";
                string shv_RegionMax = "Parameters/" + xmlNode + "/hv_RegionMax";

                hv_MeasureLength1 = Convert.ToInt32(xmlRW.Read(shv_MeasureLength1));
                hv_MeasureLength2 = Convert.ToInt32(xmlRW.Read(shv_MeasureLength2));
                hv_MeasureSigma = Convert.ToDouble(xmlRW.Read(shv_MeasureSigma));
                hv_MeasureThreshold = Convert.ToInt32(xmlRW.Read(shv_MeasureThreshold));
                hv_GenParamName = xmlRW.Read(shv_GenParamName);
                hv_GenParamValue = xmlRW.Read(shv_GenParamValue);
                hv_Number = Convert.ToInt32(xmlRW.Read(shv_Number));

                hv_Row1_Horizon = Convert.ToDouble(xmlRW.Read(shv_Row1_Horizon));
                hv_Column1_Horizon = Convert.ToDouble(xmlRW.Read(shv_Column1_Horizon));
                hv_Row2_Horizon = Convert.ToDouble(xmlRW.Read(shv_Row2_Horizon));
                hv_Column2_Horizon = Convert.ToDouble(xmlRW.Read(shv_Column2_Horizon));

                hv_Row1_Vertical = Convert.ToDouble(xmlRW.Read(shv_Row1_Vertical));
                hv_Column1_Vertical = Convert.ToDouble(xmlRW.Read(shv_Column1_Vertical));
                hv_Row2_Vertical = Convert.ToDouble(xmlRW.Read(shv_Row2_Vertical));
                hv_Column2_Vertical = Convert.ToDouble(xmlRW.Read(shv_Column2_Vertical));

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
        public bool WriteParams(String xmlNode, string regionName)
        {
            try
            {
                string shv_MeasureLength1 = "Parameters/" + xmlNode + "/hv_MeasureLength1";
                string shv_MeasureLength2 = "Parameters/" + xmlNode + "/hv_MeasureLength2";
                string shv_MeasureSigma = "Parameters/" + xmlNode + "/hv_MeasureSigma";
                string shv_MeasureThreshold = "Parameters/" + xmlNode + "/hv_MeasureThreshold";
                string shv_GenParamName = "Parameters/" + xmlNode + "/hv_GenParamName";
                string shv_GenParamValue = "Parameters/" + xmlNode + "/hv_GenParamValue";
                string shv_Number = "Parameters/" + xmlNode + "/hv_Number";

                string shv_Row1_Horizon = "Parameters/" + xmlNode + "/hv_Row1_Horizon";
                string shv_Column1_Horizon = "Parameters/" + xmlNode + "/hv_Column1_Horizon";
                string shv_Row2_Horizon = "Parameters/" + xmlNode + "/hv_Row2_Horizon";
                string shv_Column2_Horizon = "Parameters/" + xmlNode + "/hv_Column2_Horizon";

                string shv_Row1_Vertical = "Parameters/" + xmlNode + "/hv_Row1_Vertical";
                string shv_Column1_Vertical = "Parameters/" + xmlNode + "/hv_Column1_Vertical";
                string shv_Row2_Vertical = "Parameters/" + xmlNode + "/hv_Row2_Vertical";
                string shv_Column2_Vertical = "Parameters/" + xmlNode + "/hv_Column2_Vertical";


                xmlRW.Update(shv_MeasureLength1, hv_MeasureLength1.ToString());
                xmlRW.Update(shv_MeasureLength2, hv_MeasureLength2.ToString());
                xmlRW.Update(shv_MeasureSigma, hv_MeasureSigma.ToString());
                xmlRW.Update(shv_MeasureThreshold, hv_MeasureThreshold.ToString());
                xmlRW.Update(shv_GenParamName, hv_GenParamName);
                xmlRW.Update(shv_GenParamValue, hv_GenParamValue);
                xmlRW.Update(shv_Number, hv_Number.ToString());

                xmlRW.Update(shv_Row1_Horizon, hv_Row1_Horizon.ToString());
                xmlRW.Update(shv_Column1_Horizon, hv_Column1_Horizon.ToString());
                xmlRW.Update(shv_Row2_Horizon, hv_Row2_Horizon.ToString());
                xmlRW.Update(shv_Column2_Horizon, hv_Column2_Horizon.ToString());

                xmlRW.Update(shv_Row1_Vertical, hv_Row1_Vertical.ToString());
                xmlRW.Update(shv_Column1_Vertical, hv_Column1_Vertical.ToString());
                xmlRW.Update(shv_Row2_Vertical, hv_Row2_Vertical.ToString());
                xmlRW.Update(shv_Column2_Vertical, hv_Column2_Vertical.ToString());

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
