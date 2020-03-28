using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Standard_UI.UI
{
    public class ExistParams
    {
        public bool errorFlag;

        public HObject ho_Image;                 //源图像
        public HObject ho_Region;                //检测区域
        public HTuple hv_MinGray;
        public HTuple hv_MaxGray;
        
        public HTuple hv_Min;
        public HTuple hv_Max;

        public HTuple hv_Number;

        public HObject ho_Region_Find;

        ParametersRW.XmlRW xmlRW;

        public ExistParams()
        {
            errorFlag=false;
            ho_Image = new HObject();
            HOperatorSet.GenEmptyRegion(out ho_Region);
            HOperatorSet.GenEmptyRegion(out ho_Region_Find);

            hv_MinGray = 0;
            hv_MaxGray = 120;
            hv_Min = 100;
            hv_Max = 999999999;

            hv_Number = 1;

            xmlRW = new ParametersRW.XmlRW();
        }

        public bool CopyTo(ref ExistParams existParams)
        {
            try
            {
                existParams.ho_Region = ho_Region;

                existParams.hv_MinGray = hv_MinGray;
                existParams.hv_MaxGray = hv_MaxGray;
                existParams.hv_Min = hv_Min;
                existParams.hv_Max = hv_Max;
                existParams.hv_Number = hv_Number;

                errorFlag = false;
                return true;
            }
            catch (Exception exc)
            {
                errorFlag = true;
                return false;
            }
        }

        public bool CheckIfExist()
        {
            if (ho_Image == null)
            {
                errorFlag=true;
                return false;
            }
            if (ho_Region == null)
            {
                errorFlag = true;
                return false;
            }
            try
            {
                HObject ho_ImageReduced = null;
                HOperatorSet.ReduceDomain(ho_Image, ho_Region, out ho_ImageReduced);

                HObject ho_Regions = null;
                HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, hv_MinGray, hv_MaxGray);

                //string regionPath = AppDomain.CurrentDomain.BaseDirectory + "Parameters\\test.hobj";
                //HOperatorSet.WriteRegion(ho_Region, regionPath);


                HObject ho_ConnectedRegions = null;
                HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);

                //HObject ho_SelectedRegions = null;
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_Region_Find, "area", "and", hv_Min, hv_Max);

                HTuple hv_mNumber = new HTuple();
                HOperatorSet.CountObj(ho_Region_Find, out hv_mNumber);
                if (hv_mNumber <hv_Number)
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
        
        public bool ReadParams(String xmlNode,string imageName,string regionName)
        {
            try
            {
                string shv_MinGray = "Parameters/" + xmlNode + "/hv_MinGray";
                string shv_MaxGray = "Parameters/" + xmlNode + "/hv_MaxGray";
                string shv_Min = "Parameters/" + xmlNode + "/hv_Min";
                string shv_Max = "Parameters/" + xmlNode + "/hv_Max";
                string shv_Number = "Parameters/" + xmlNode + "/hv_Number";

                hv_MinGray =Convert.ToInt32(xmlRW.Read(shv_MinGray));
                hv_MaxGray = Convert.ToInt32(xmlRW.Read(shv_MaxGray));
                hv_Min = Convert.ToInt32(xmlRW.Read(shv_Min));
                hv_Max = Convert.ToInt32(xmlRW.Read(shv_Max));
                hv_Number = Convert.ToInt32(xmlRW.Read(shv_Number));

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
        public bool WriteParams(String xmlNode, string regionName)
        {
            try
            {
                string shv_MinGray = "Parameters/" + xmlNode + "/hv_MinGray";
                string shv_MaxGray = "Parameters/" + xmlNode + "/hv_MaxGray";
                string shv_Min = "Parameters/" + xmlNode + "/hv_Min";
                string shv_Max = "Parameters/" + xmlNode + "/hv_Max";
                string shv_Number = "Parameters/" + xmlNode + "/hv_Number";

                xmlRW.Update(shv_MinGray, hv_MinGray.ToString());
                xmlRW.Update(shv_MaxGray, hv_MaxGray.ToString());
                xmlRW.Update(shv_Min, hv_Min.ToString());
                xmlRW.Update(shv_Max, hv_Max.ToString());
                xmlRW.Update(shv_Number, hv_Number.ToString());

                string regionPath = AppDomain.CurrentDomain.BaseDirectory + @".//Parameters//" + regionName;
                HOperatorSet.WriteRegion(ho_Region, regionPath);

                errorFlag=false;
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
