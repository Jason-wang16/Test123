using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Standard_UI.UI
{
    class Edge1DParams
    {

        public delegate void SaveEdgeParams(Edge1DParams edgeParams);
        public event SaveEdgeParams eventSaveEdgeParams;

        //使用halcon measurepos找边缘直线点
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
        public List<HTuple> hv_MeasureHandles = new List<HTuple>();

        //查找参数
        public HTuple hv_Sigma;
        public HTuple hv_Threshold;
        public HTuple hv_Transition;
        public HTuple hv_Select;
        public HTuple hv_RowEdge;
        public HTuple hv_ColumnEdge;
        public HTuple hv_Amplitude;
        public HTuple hv_Distance;

        public List<HTuple> hv_RowEdges = new List<HTuple>();
        public List<HTuple> hv_ColumnEdges = new List<HTuple>();

        //自定义参数，用于找到边缘点后的点数据滤除、直线拟合
        public int divideParts;
        public double minDistance;
        public int minPointsNumm;  //点的最小个数
        public double minPointsScore;  //点的最小占比
        ////框取的矩形ROI参数
        //public HTuple hv_RectangleRow = null;
        //public HTuple hv_RectangleColumn = null;
        //public HTuple hv_RectanglePhi = null;
        //public HTuple hv_RectangleLength1 = null;
        //public HTuple hv_RectangleLength2 = null;

        public Edge1DParams()
        {
            hv_Interpolation = "nearest_neighbor";
            hv_Sigma = 1.0;
            hv_Threshold = 30.0;
            hv_Transition = "negative";//"negative"白到黑,"positive"黑到白,"all"所有
            hv_Select = "all";

            divideParts = 11;
            minDistance = 5;
            minPointsNumm = 6;
            minPointsScore = 0.6;
        }

        public bool CopyTo(Edge1DParams edgeParams)
        {
            eventSaveEdgeParams(edgeParams);
            return true;
        }

        public bool Initialize(string templateImagePath)
        {

            return true;
        }

        public bool FitLine()
        {

            return true;
        }
    }
}
