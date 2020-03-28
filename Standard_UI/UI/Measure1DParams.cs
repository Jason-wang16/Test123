using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Standard_UI.UI
{
    class Measure1DParams
    {
        public string detectMethod;              //"Line"为找直线，"Circle"为找圆弧

        public HObject ho_Image;                 //源图像
        //生成查找ROI参数
        public HTuple hv_Row;
        public HTuple hv_Column;
        public HTuple hv_Phi;
        public HTuple hv_Length1;
        public HTuple hv_Length2;
        public HTuple hv_Width;
        public HTuple hv_Height;
        public HTuple hv_Interpolation;
        public HTuple hv_MeasureHandle;

        //找圆弧多的参数
        public HTuple hv_Radius;
        public HTuple hv_AngleStart;
        public HTuple hv_AngleExtent;
        public HTuple hv_AnnulusRadius;

        //查找参数
        public HTuple hv_Sigma;
        public HTuple hv_Threshold;
        public HTuple hv_Transition;
        public HTuple hv_Select;
        public HTuple hv_RowEdgeFirst;
        public HTuple hv_ColumnEdgeFirst;
        public HTuple hv_AmplitudeFirst;
        public HTuple hv_RowEdgeSecond;
        public HTuple hv_ColumnEdgeSecond;
        public HTuple hv_AmplitudeSecond;
        public HTuple hv_IntraDistance;
        public HTuple hv_InterDistance;

        public Measure1DParams()
        {
            detectMethod = "Line";

            hv_Interpolation = "nearest_neighbor";
            hv_Sigma = 1.0;
            hv_Threshold = 30.0;
            hv_Transition = "all";
            hv_Select = "all";
        }

        public bool CopyTo(ref Measure1DParams measureParams)
        {
            return true;
        }

        public bool Initialize(string templateImagePath)
        {

            return true;
        }
    }
}
