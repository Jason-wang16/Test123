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
using System.IO.Ports;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Standard_UI
{
    public partial class FormMain : Form
    {
        #region 检测标记
        bool autoModel;   //手动和自动模式切换
        bool test;  //静态测试本地图片

        string windowName;
        string chartName;

        bool btnLineParams_InA_ClickFlag;
        bool btnLineParams_InC_ClickFlag;
        bool btnLineParams_InDUp_ClickFlag;
        bool btnLineParams_InDDown_ClickFlag;

        bool btnLineParams_OutA_ClickFlag;
        bool btnLineParams_OutC_ClickFlag;
        bool btnLineParams_OutDUp_ClickFlag;
        bool btnLineParams_OutDDown_ClickFlag;

        bool firstFlag1;//第一次检测标志，用于记录基准点
        bool lastFlag1; //最后一次检测标志
        bool firstFlag2;//第一次检测标志，用于记录基准点
        bool lastFlag2; //最后一次检测标志

        bool highLevel;
        #endregion

        #region 功能类
        CameraControl.CameraBasler cameraBasler01;
        CameraControl.CameraBasler cameraBasler02;

        RecordsWrite.CsvWrite csvWrite;
        
        ParametersRW.XmlRW xmlRW;

        Comunication.Modbus_TCP modbus_TCP;
        System.Windows.Threading.DispatcherTimer timer1;//定时扫描PLC寄存器的值

        #endregion

        #region 通用参数/xml文件参数
        private string ip;
        private string port;

        string cameraBasler01_name;
        int cameraBasler01_offsetX;
        int cameraBasler01_offsetY;
        int cameraBasler01_offsetW;
        int cameraBasler01_offsetH;

        string cameraBasler02_name;
        int cameraBasler02_offsetX;
        int cameraBasler02_offsetY;
        int cameraBasler02_offsetW;
        int cameraBasler02_offsetH;

        double upK1;
        double upB1;
        double downK1;
        double downB1;
        double upK2;
        double upB2;
        double downK2;
        double downB2;

        double inADmin;
        double inADmax;
        double inCDmin;
        double inCDmax;
        double inACmin;
        double inACmax;
        double outADmin;
        double outADmax;
        double outCDmin;
        double outCDmax;
        double outACmin;
        double outACmax;
        #endregion

        #region 窗口显示相关
        HTuple hv_StartX1;
        HTuple hv_StartY1;
        HTuple hv_ZoomFactor1;
        HTuple hv_ImageWindow1;
        HTuple hv_XldHomMat2D1;

        HTuple hv_StartX2;
        HTuple hv_StartY2;
        HTuple hv_ZoomFactor2;
        HTuple hv_ImageWindow2;
        HTuple hv_XldHomMat2D2;

        HObject ho_Image1;//相机1图像
        HObject ho_Image2;//相机2图像
        #endregion

        #region 线程任务
        Task tLineParams_InA;
        Task tLineParams_InC;
        Task tLineParams_InDUp;
        Task tLineParams_InDDown;
        Task tLineParams_OutA;
        Task tLineParams_OutC;
        Task tLineParams_OutDUp;
        Task tLineParams_OutDDown;
        #endregion

        #region 检测对象
        UI.LineDetect lineDetect;

        UI.LineParams lineParams_InA;
        UI.LineParams lineParams_InC;
        UI.LineParams lineParams_InDUp;
        UI.LineParams lineParams_InDDown;
        UI.LineParams lineParams_OutA;
        UI.LineParams lineParams_OutC;
        UI.LineParams lineParams_OutDUp;
        UI.LineParams lineParams_OutDDown;
        #endregion

        #region 检测结果
        bool result1;
        bool result2;
        bool result1Flag;
        bool result2Flag;
        bool totalResult;

        int iInAD;
        int iInCD;
        int iInAC;
        int iOutAD;
        int iOutCD;
        int iOutAC;

        double dInAD;
        double dInCD;
        double dInAC;
        double dOutAD;
        double dOutCD;
        double dOutAC;

        List<double> lInAD;
        List<double> lInCD;
        List<double> lInAC;
        List<double> lOutAD;
        List<double> lOutCD;
        List<double> lOutAC;

        double inA2Datum;
        double inC2Datum;
        double inD2DatumUp;
        double inD2DatumDown;
        double outA2Datum;
        double outC2Datum;
        double outD2DatumUp;
        double outD2DatumDown;

        string getData;
        string sendData;

        int totalNum;
        int okNum;
        int ngNum;
        double okRate;

        //List<HObject> lImage1;
        //List<HObject> lImage2;
        List<HObject> lScreen1;
        List<HObject> lScreen2;
        List<bool> lOKOrNG1;
        List<bool> lOKOrNG2;

        String[] csvArray;  //保存生产记录为CSV文件
        #endregion

        #region 基准点
        //内部上基准
        HTuple hv_InUp_Row1;
        HTuple hv_InUp_Column1;
        HTuple hv_InUp_Row2;
        HTuple hv_InUp_Column2;
        //内部下基准
        HTuple hv_InDown_Row1;
        HTuple hv_InDown_Column1;
        HTuple hv_InDown_Row2;
        HTuple hv_InDown_Column2;
        //外部上基准
        HTuple hv_OutUp_Row1;
        HTuple hv_OutUp_Column1;
        HTuple hv_OutUp_Row2;
        HTuple hv_OutUp_Column2;
        //外部下基准
        HTuple hv_OutDown_Row1;
        HTuple hv_OutDown_Column1;
        HTuple hv_OutDown_Row2;
        HTuple hv_OutDown_Column2;
        #endregion

        public FormMain()
        {
            InitializeComponent();
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            #region 参数赋初值
            lineDetect = new UI.LineDetect();

            lineParams_InA = new UI.LineParams();
            lineParams_InC = new UI.LineParams();
            lineParams_InDUp = new UI.LineParams();
            lineParams_InDDown = new UI.LineParams();
            lineParams_OutA = new UI.LineParams();
            lineParams_OutC = new UI.LineParams();
            lineParams_OutDUp = new UI.LineParams();
            lineParams_OutDDown = new UI.LineParams();

            csvWrite = new RecordsWrite.CsvWrite();
            xmlRW = new ParametersRW.XmlRW();

            hv_ImageWindow1 = hWindowControl1.HalconID;
            hv_ImageWindow2 = hWindowControl2.HalconID;

            totalNum = 0;
            okNum = 0;
            ngNum = 0;

            btnLineParams_InA_ClickFlag = false;
            btnLineParams_InC_ClickFlag = false;
            btnLineParams_InDUp_ClickFlag = false;
            btnLineParams_InDDown_ClickFlag = false;

            btnLineParams_OutA_ClickFlag = false;
            btnLineParams_OutC_ClickFlag = false;
            btnLineParams_OutDUp_ClickFlag = false;
            btnLineParams_OutDDown_ClickFlag = false;

            firstFlag1 = true;
            firstFlag2 = true;

            highLevel = false;

            lInAD = new List<double>();
            lInCD = new List<double>();
            lInAC = new List<double>();
            lOutAD = new List<double>();
            lOutCD = new List<double>();
            lOutAC = new List<double>();

            lScreen1 = new List<HObject>();
            lScreen2 = new List<HObject>();
            lOKOrNG1 = new List<bool>();
            lOKOrNG2 = new List<bool>();

            hv_InUp_Row1 = 0;
            hv_InUp_Column1 =0;
            hv_InUp_Row2 =0;
            hv_InUp_Column2 = 0;
            //内部下基准
            hv_InDown_Row1 = 0;
            hv_InDown_Column1 = 0;
            hv_InDown_Row2 = 0;
            hv_InDown_Column2 = 0;
            //外部上基准
            hv_OutUp_Row1 =0;
            hv_OutUp_Column1 = 0;
            hv_OutUp_Row2 = 0;
            hv_OutUp_Column2 = 0;
            //外部下基准
            hv_OutDown_Row1 = 0;
            hv_OutDown_Column1 = 0;
            hv_OutDown_Row2 = 0;
            hv_OutDown_Column2 = 0;

            ho_Image1=new HObject();
            ho_Image2 = new HObject();

            csvArray= new string[8];  //保存生产记录为CSV文件
            #endregion

            if (!ReadParams())
            {
                MessageBox.Show("读取XML参数失败！");
                return;
            }
            MessageBox.Show("读取XML参数成功！");

            if (!Initialize())
            {
                MessageBox.Show("初始化失败！");
                return;
            }
            MessageBox.Show("初始化成功！");

            timer1 = new System.Windows.Threading.DispatcherTimer();
            timer1.Tick += new EventHandler(SignalDetect);
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer1.Start();

            modbus_TCP.Modbus_Write_Single_Holding_Register(1,1102,1); //发送软件打开信号给PLC
        }
        private void CameraBasler01_OverHandle(HObject ho_Image)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Standard_UI.CameraControl.CameraBasler.delegateProcessHImage(CameraBasler01_OverHandle), ho_Image);
                return;
            }
            ho_Image1 = ho_Image;
            showHalconImage(ho_Image1, hv_ImageWindow1, ref hv_StartX1, ref hv_StartY1, ref hv_ZoomFactor1, ref hv_XldHomMat2D1, 1);

            TaskSet1();
            tLineParams_InA.Start();
            tLineParams_InC.Start();
            tLineParams_InDUp.Start();
            tLineParams_InDDown.Start();
            Judgement1();
        }

        private void CameraBasler02_OverHandle(HObject ho_Image)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Standard_UI.CameraControl.CameraBasler.delegateProcessHImage(CameraBasler02_OverHandle), ho_Image);
                return;
            }
            ho_Image2 = ho_Image;
            showHalconImage(ho_Image2, hv_ImageWindow2, ref hv_StartX2, ref hv_StartY2, ref hv_ZoomFactor2, ref hv_XldHomMat2D2, 2);

            TaskSet2();
            tLineParams_OutA.Start();
            tLineParams_OutC.Start();
            tLineParams_OutDUp.Start();
            tLineParams_OutDDown.Start();
            Judgement2();
        }

        private void TaskSet1()
        {
            lineParams_InA.ho_Image = ho_Image1;
            lineParams_InC.ho_Image = ho_Image1;
            lineParams_InDUp.ho_Image = ho_Image1;
            lineParams_InDDown.ho_Image = ho_Image1;

            tLineParams_InA = new Task(() =>
            {
                lineParams_InA.FindLine();
            });
            tLineParams_InC = new Task(() =>
            {
                lineParams_InC.FindLine();
            });
            tLineParams_InDUp = new Task(() =>
            {
                lineParams_InDUp.FindLine();
            });
            tLineParams_InDDown = new Task(() =>
            {
                lineParams_InDDown.FindLine();
            });
        }
        private void TaskSet2()
        {
            lineParams_OutA.ho_Image = ho_Image2;
            lineParams_OutC.ho_Image = ho_Image2;
            lineParams_OutDUp.ho_Image = ho_Image2;
            lineParams_OutDDown.ho_Image = ho_Image2;

            tLineParams_OutA = new Task(() =>
            {
                lineParams_OutA.FindLine();
            });
            tLineParams_OutC = new Task(() =>
            {
                lineParams_OutC.FindLine();
            });
            tLineParams_OutDUp = new Task(() =>
            {
                lineParams_OutDUp.FindLine();
            });
            tLineParams_OutDDown = new Task(() =>
            {
                lineParams_OutDDown.FindLine();
            });
        }

        private void Show2Window1()
        {
            if (!lineParams_InA.errorFlag)
            {
                ShowLine(hv_ImageWindow1, hv_ZoomFactor1, lineParams_InA, "red");
            }
            if (!lineParams_InC.errorFlag)
            {
                ShowLine(hv_ImageWindow1, hv_ZoomFactor1, lineParams_InC, "green");
            }
            if (!lineParams_InDUp.errorFlag)
            {
                ShowLine(hv_ImageWindow1, hv_ZoomFactor1, lineParams_InDUp, "blue");
            }
            if (!lineParams_InDDown.errorFlag)
            {
                ShowLine(hv_ImageWindow1, hv_ZoomFactor1, lineParams_InDDown, "blue");
            }
        }

        private void Show2Window2()
        {
            if (!lineParams_OutA.errorFlag)
            {
                ShowLine(hv_ImageWindow2, hv_ZoomFactor2, lineParams_OutA, "red");
            }
            if (!lineParams_OutC.errorFlag)
            {
                ShowLine(hv_ImageWindow2, hv_ZoomFactor2, lineParams_OutC, "green");
            }
            if (!lineParams_OutDUp.errorFlag)
            {
                ShowLine(hv_ImageWindow2, hv_ZoomFactor2, lineParams_OutDUp, "blue");
            }
            if (!lineParams_OutDDown.errorFlag)
            {
                ShowLine(hv_ImageWindow2, hv_ZoomFactor2, lineParams_OutDDown, "blue");
            }
        }

        private void ShowLine(HTuple hv_ImageWindow, HTuple hv_ZoomFactor, UI.LineParams lineParams, HTuple color)
        {
            HTuple hv_LineRowBegin = new HTuple();
            HTuple hv_LineColumnBegin = new HTuple();
            HTuple hv_LineRowEnd = new HTuple();
            HTuple hv_LineColumnEnd = new HTuple();

            HTuple hv_PointRow = new HTuple();
            HTuple hv_PointColumn = new HTuple();

            hv_LineRowBegin = lineParams.hv_LineRowBegin * hv_ZoomFactor;
            hv_LineColumnBegin = lineParams.hv_LineColumnBegin * hv_ZoomFactor;
            hv_LineRowEnd = lineParams.hv_LineRowEnd * hv_ZoomFactor;
            hv_LineColumnEnd = lineParams.hv_LineColumnEnd * hv_ZoomFactor;

            HObject ho_MeasuredLine;
            HOperatorSet.GenRegionLine(out ho_MeasuredLine, hv_LineRowBegin, hv_LineColumnBegin, hv_LineRowEnd, hv_LineColumnEnd);

            HOperatorSet.SetColor(hv_ImageWindow, color);
            HOperatorSet.DispObj(ho_MeasuredLine, hv_ImageWindow);
        }

        private bool showHalconImage(HObject ho_Image, HTuple hv_ImageWindow, ref HTuple hv_StartX,
         ref HTuple hv_StartY, ref HTuple hv_ZoomFactor, ref HTuple hv_XldHomMat2D, int cameraSerial)
        {
            HOperatorSet.ClearWindow(hv_ImageWindow);
            //获取图像大小及纵横比
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            int im_width = int.Parse(hv_Width.ToString());
            int im_height = int.Parse(hv_Height.ToString());
            double im_AspectRatio = (double)(im_width) / (double)(im_height);
            double halconWindowHeight = 0;
            double halconWindowWidth = 0;
            if (cameraSerial == 1)
            {
                halconWindowHeight = this.hWindowControl1.WindowSize.Height;
                halconWindowWidth = this.hWindowControl1.WindowSize.Width;
            }
            else if (cameraSerial == 2)
            {
                halconWindowHeight = this.hWindowControl2.WindowSize.Height;
                halconWindowWidth = this.hWindowControl2.WindowSize.Width;
            }
            double w_AspectRatio = halconWindowWidth / halconWindowHeight;
            HOperatorSet.SetSystem("int_zooming", "false");//图像缩放之前最好将此参数设置为false.

            HTuple para = new HTuple("constant");
            HObject ho_zoomImage;
            HOperatorSet.GenEmptyObj(out ho_zoomImage);

            ho_zoomImage.Dispose();
            HOperatorSet.ClearWindow(hv_ImageWindow);

            if (halconWindowWidth < im_width && im_AspectRatio > w_AspectRatio)
            {
                //超宽图像               
                HOperatorSet.ZoomImageSize(ho_Image, out ho_zoomImage, halconWindowWidth, halconWindowWidth / im_AspectRatio, para);
                hv_ZoomFactor = halconWindowWidth / im_width;
                hv_StartX = 0 - (halconWindowHeight - halconWindowWidth / im_AspectRatio) / 2;
                hv_StartY = 0;
            }
            else if (halconWindowHeight < im_height && im_AspectRatio < w_AspectRatio)
            {
                //超高图像                
                HOperatorSet.ZoomImageSize(ho_Image, out ho_zoomImage, halconWindowHeight * im_AspectRatio, halconWindowHeight, para);
                hv_ZoomFactor = halconWindowHeight / im_height;
                hv_StartX = 0;
                hv_StartY = 0 - (halconWindowWidth - halconWindowHeight * im_AspectRatio) / 2;
            }
            try
            {
                HOperatorSet.SetPart(hv_ImageWindow, hv_StartX, hv_StartY, halconWindowHeight - 1 + hv_StartX,
                    halconWindowWidth - 1 + hv_StartY);//设置居中显示
                HOperatorSet.DispObj(ho_zoomImage, hv_ImageWindow);

                //string Path = AppDomain.CurrentDomain.BaseDirectory + @".//Parameters//test.bmp" ;
                //HOperatorSet.WriteImage(ho_Image,"bmp",0, Path);

                HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);
                HOperatorSet.HomMat2dScale(hv_XldHomMat2D, hv_ZoomFactor, hv_ZoomFactor, 0, 0, out hv_XldHomMat2D);
            }
            catch (Exception exc)
            {
                MessageBox.Show("图像显示错误！");
                return false;
            }
            return true;
        }

        private void SaveWindow2Image(HTuple cameraWindow, string node)
        {
            string addPath = DateTime.Now.ToString("yyyy-MM-dd");
            string path = AppDomain.CurrentDomain.BaseDirectory + node + "\\" + addPath + "\\";
            string picPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                HObject hv_SaveImage;
                HOperatorSet.GenEmptyObj(out hv_SaveImage);
                HOperatorSet.DumpWindowImage(out hv_SaveImage, cameraWindow);
                picPath = path + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff");
                HOperatorSet.WriteImage(hv_SaveImage, "png", 0, picPath);
            }
            catch (Exception exc)
            {
                throw (exc);
            }
        }

        private void Judgement1()
        {
            Task.WaitAll(tLineParams_InA, tLineParams_InC, tLineParams_InDUp, tLineParams_InDDown);
            result1 = true;
            if (lineParams_InC.errorFlag || lineParams_InDUp.errorFlag || lineParams_InA.errorFlag || lineParams_InDDown.errorFlag)
            {
                inA2Datum = 10;
                inC2Datum = 10;
                inD2DatumUp = 10;
                inD2DatumDown = 10;

                dInAD = 10;
                dInCD = 10;
                dInAC = 10;

                result1 = false;
                result1Flag = false;
                totalResult = false;
            }
            else
            {
                if (firstFlag1)
                {
                    firstFlag1 = false;

                    hv_InUp_Row1 = lineParams_InDUp.hv_LineRowBegin;
                    hv_InUp_Column1 = lineParams_InDUp.hv_LineColumnBegin - 100;
                    hv_InUp_Row2 = lineParams_InDUp.hv_LineRowEnd;
                    hv_InUp_Column2 = lineParams_InDUp.hv_LineColumnEnd - 100;

                    hv_InDown_Row1 = lineParams_InDDown.hv_LineRowBegin;
                    hv_InDown_Column1 = lineParams_InDDown.hv_LineColumnBegin- 100;
                    hv_InDown_Row2 = lineParams_InDDown.hv_LineRowEnd;
                    hv_InDown_Column2 = lineParams_InDDown.hv_LineColumnEnd - 100;
                }

                inA2Datum = L2LDistance(lineParams_InA.hv_LineRowBegin, lineParams_InA.hv_LineColumnBegin, lineParams_InA.hv_LineRowEnd, lineParams_InA.hv_LineColumnEnd,
                    hv_InDown_Row1, hv_InDown_Column1, hv_InDown_Row2, hv_InDown_Column2) * downK1 + downB1;
                inC2Datum = L2LDistance(lineParams_InC.hv_LineRowBegin, lineParams_InC.hv_LineColumnBegin, lineParams_InC.hv_LineRowEnd, lineParams_InC.hv_LineColumnEnd,
                    hv_InUp_Row1, hv_InUp_Column1, hv_InUp_Row2, hv_InUp_Column2) * upK1 + upB1;
                inD2DatumUp = L2LDistance(lineParams_InDUp.hv_LineRowBegin, lineParams_InDUp.hv_LineColumnBegin, lineParams_InDUp.hv_LineRowEnd, lineParams_InDUp.hv_LineColumnEnd,
                    hv_InUp_Row1, hv_InUp_Column1, hv_InUp_Row2, hv_InUp_Column2) * upK1 + upB1;
                inD2DatumDown = L2LDistance(lineParams_InDDown.hv_LineRowBegin, lineParams_InDDown.hv_LineColumnBegin, lineParams_InDDown.hv_LineRowEnd, lineParams_InDDown.hv_LineColumnEnd,
                    hv_InDown_Row1, hv_InDown_Column1, hv_InDown_Row2, hv_InDown_Column2) * downK1 + downB1;

                dInAD = inA2Datum - inD2DatumDown;
                dInCD = inC2Datum - inD2DatumUp;
                dInAC = dInCD - dInAD;
            }
            chart1.Series["seriesA"].Points.AddY(inA2Datum);
            chart1.Series["seriesC"].Points.AddY(inC2Datum);
            chart1.Series["seriesDUp"].Points.AddY(inD2DatumUp);
            chart1.Series["seriesDDown"].Points.AddY(inD2DatumDown);

            if (dInAD < inADmin || dInAD > inADmax)
            {
                result1 = false;
                result1Flag = false;
                totalResult = false;
            }
            if (dInCD < inCDmin || dInCD > inCDmax)
            {
                result1 = false;
                result1Flag = false;
                totalResult = false;
            }
            if (dInAC < inACmin || dInAC > inACmax)
            {
                result1 = false;
                result1Flag = false;
                totalResult = false;
            }


            lInAD.Add(dInAD);
            lInCD.Add(dInCD);
            lInAC.Add(dInAC);

            if (result1)
            {
                HOperatorSet.SetTposition(hv_ImageWindow1, 0, hv_StartY1);
                HOperatorSet.SetColor(hv_ImageWindow1, "green");              
                HOperatorSet.WriteString(hv_ImageWindow1, "OK");
            }
            else
            {
                HOperatorSet.SetTposition(hv_ImageWindow1, 0, hv_StartY1);
                HOperatorSet.SetColor(hv_ImageWindow1, "red");
                HOperatorSet.WriteString(hv_ImageWindow1, "NG");
            }

            HOperatorSet.SetColor(hv_ImageWindow1, "blue");
            HOperatorSet.SetTposition(hv_ImageWindow1, 24, hv_StartY1);          
            HOperatorSet.WriteString(hv_ImageWindow1, "阳极-隔膜:"+dInAD.ToString("0.####"));
            HOperatorSet.SetTposition(hv_ImageWindow1, 48, hv_StartY1);
            HOperatorSet.WriteString(hv_ImageWindow1, "阳极-阴极:"+dInAC.ToString("0.####"));

            Show2Window1();

            HObject ho_Screen1;
            HOperatorSet.GenEmptyObj(out ho_Screen1);
            HOperatorSet.DumpWindowImage(out ho_Screen1,hv_ImageWindow1);
            lScreen1.Add(ho_Screen1);
            lOKOrNG1.Add(result1);
        }

        private void Judgement2()
        {
            Task.WaitAll(tLineParams_OutA, tLineParams_OutC, tLineParams_OutDUp, tLineParams_OutDDown);
            result2 = true;
            if (lineParams_OutC.errorFlag || lineParams_OutDUp.errorFlag || lineParams_OutA.errorFlag || lineParams_OutDDown.errorFlag)
            {
                outA2Datum = 10;
                outC2Datum = 10;
                outD2DatumUp = 10;
                outD2DatumDown = 10;

                dOutAD = 10;
                dOutCD = 10;
                dOutAC = 10;

                result2 = false;
                result2Flag = false;
                totalResult = false;
            }
            else
            {
                if (firstFlag2)
                {
                    firstFlag2 = false;

                    hv_OutUp_Row1 = lineParams_OutDUp.hv_LineRowBegin;
                    hv_OutUp_Column1 = lineParams_OutDUp.hv_LineColumnBegin + 100;
                    hv_OutUp_Row2 = lineParams_OutDUp.hv_LineRowEnd;
                    hv_OutUp_Column2 = lineParams_OutDUp.hv_LineColumnEnd + 100;

                    hv_OutDown_Row1 = lineParams_OutDDown.hv_LineRowBegin;
                    hv_OutDown_Column1 = lineParams_OutDDown.hv_LineColumnBegin + 100;
                    hv_OutDown_Row2 = lineParams_OutDDown.hv_LineRowEnd;
                    hv_OutDown_Column2 = lineParams_OutDDown.hv_LineColumnEnd + 100;
                }

                outA2Datum = L2LDistance(lineParams_OutA.hv_LineRowBegin, lineParams_OutA.hv_LineColumnBegin, lineParams_OutA.hv_LineRowEnd, lineParams_OutA.hv_LineColumnEnd,
                    hv_OutDown_Row1, hv_OutDown_Column1, hv_OutDown_Row2, hv_OutDown_Column2) * downK2 + downB2;
                outC2Datum = L2LDistance(lineParams_OutC.hv_LineRowBegin, lineParams_OutC.hv_LineColumnBegin, lineParams_OutC.hv_LineRowEnd, lineParams_OutC.hv_LineColumnEnd,
                    hv_OutUp_Row1, hv_OutUp_Column1, hv_OutUp_Row2, hv_OutUp_Column2) * upK2 + upB2;
                outD2DatumUp = L2LDistance(lineParams_OutDUp.hv_LineRowBegin, lineParams_OutDUp.hv_LineColumnBegin, lineParams_OutDUp.hv_LineRowEnd, lineParams_OutDUp.hv_LineColumnEnd,
                    hv_OutUp_Row1, hv_OutUp_Column1, hv_OutUp_Row2, hv_OutUp_Column2) * upK2 + upB2;
                outD2DatumDown = L2LDistance(lineParams_OutDDown.hv_LineRowBegin, lineParams_OutDDown.hv_LineColumnBegin, lineParams_OutDDown.hv_LineRowEnd, lineParams_OutDDown.hv_LineColumnEnd,
                    hv_OutDown_Row1, hv_OutDown_Column1, hv_OutDown_Row2, hv_OutDown_Column2) * downK2 + downB2;

                dOutAD = outA2Datum - outD2DatumDown;
                dOutCD = outC2Datum - outD2DatumUp;
                dOutAC = dOutCD - dOutAD;
            }
            chart2.Series["seriesA"].Points.AddY(outA2Datum);
            chart2.Series["seriesC"].Points.AddY(outC2Datum);
            chart2.Series["seriesDUp"].Points.AddY(outD2DatumUp);
            chart2.Series["seriesDDown"].Points.AddY(outD2DatumDown);

            if (dOutAD < outADmin || dOutAD > outADmax)
            {
                result2 = false;
                result2Flag = false;
                totalResult = false;
            }
            if (dOutCD < outCDmin || dOutCD > outCDmax)
            {
                result2 = false;
                result2Flag = false;
                totalResult = false;
            }
            if (dOutAC < outACmin || dOutAC > outACmax)
            {
                result2 = false;
                result2Flag = false;
                totalResult = false;
            }
            lOutAD.Add(dOutAD);
            lOutCD.Add(dOutCD);
            lOutAC.Add(dOutAC);

            if (result2)
            {
                HOperatorSet.SetTposition(hv_ImageWindow2, 0, hv_StartY2);
                HOperatorSet.SetColor(hv_ImageWindow2, "green");
                HOperatorSet.WriteString(hv_ImageWindow2, "OK");
            }
            else
            {
                HOperatorSet.SetTposition(hv_ImageWindow2, 0, hv_StartY2);
                HOperatorSet.SetColor(hv_ImageWindow2, "red");
                HOperatorSet.WriteString(hv_ImageWindow2, "NG");
            }

            HOperatorSet.SetColor(hv_ImageWindow2, "blue");
            HOperatorSet.SetTposition(hv_ImageWindow2, 24, hv_StartY2);
            HOperatorSet.WriteString(hv_ImageWindow2, "阳极-隔膜:" + dOutAD.ToString("0.####"));
            HOperatorSet.SetTposition(hv_ImageWindow2, 48, hv_StartY2);
            HOperatorSet.WriteString(hv_ImageWindow2, "阳极-阴极:" + dOutAC.ToString("0.####"));

            Show2Window2();

            HObject ho_Screen2;
            HOperatorSet.DumpWindowImage(out ho_Screen2, hv_ImageWindow2);
            lScreen2.Add(ho_Screen2);
            lOKOrNG2.Add(result2);
        }

        private bool Initialize()
        {
            bool initializeFlag = true;
            cameraBasler01 = new CameraControl.CameraBasler(cameraBasler01_name);
            if (!cameraBasler01.OpenCam())
            {
                MessageBox.Show("相机1打开失败！");
                initializeFlag = false;
            }
            cameraBasler01.eventProcessImage += CameraBasler01_OverHandle;
            cameraBasler01.eventComputeGrabTime += computerGrabTime1;
            //cameraBasler01.SetGain(95);
            cameraBasler01.SetExternTrigger();


           cameraBasler02 = new CameraControl.CameraBasler(cameraBasler02_name);
            if (!cameraBasler02.OpenCam())
            {
                MessageBox.Show("相机2打开失败！");
                initializeFlag = false;
            }
            cameraBasler02.eventProcessImage += CameraBasler02_OverHandle;
            cameraBasler02.eventComputeGrabTime += computerGrabTime2;
            //cameraBasler02.SetGain(95);
            cameraBasler02.SetExternTrigger();

            try
            {
                modbus_TCP = new Comunication.Modbus_TCP(ip, port);
                if (modbus_TCP.Modbus_TCP_Open() == "连接成功！")
                {
                    txtDetectSignal.Text= "连接Modbus服务器成功";
                }
                else
                {
                    txtDetectSignal.Text = "连接Modbus服务器失败";
                    MessageBox.Show("连接Modbus服务器失败！");
                }
                //socketServer = new SocketServer(ip, port);
                //startTcpServer();
            }
            catch (Exception exc)
            {
                MessageBox.Show("连接Modbus服务器失败！" + exc.ToString());
            }

            return initializeFlag;
        }

        private bool ReadParams()
        {
            if (!lineParams_InA.ReadParams("LineParams_InA"))
            {
                return false;
            }
            if (!lineParams_InC.ReadParams("LineParams_InC"))
            {
                return false;
            }
            if (!lineParams_InDUp.ReadParams("LineParams_InDUp"))
            {
                return false;
            }
            if (!lineParams_InDDown.ReadParams("LineParams_InDDown"))
            {
                return false;
            }
            if (!lineParams_OutA.ReadParams("LineParams_OutA"))
            {
                return false;
            }
            if (!lineParams_OutC.ReadParams("LineParams_OutC"))
            {
                return false;
            }
            if (!lineParams_OutDUp.ReadParams("LineParams_OutDUp"))
            {
                return false;
            }
            if (!lineParams_OutDDown.ReadParams("LineParams_OutDDown"))
            {
                return false;
            }
            try
            {
                ip = xmlRW.Read("Parameters/TCPIP/ip");
                port = xmlRW.Read("Parameters/TCPIP/port");

                cameraBasler01_name = xmlRW.Read("Parameters/CameraBasler01/name");
                cameraBasler01_offsetX = Convert.ToInt32(xmlRW.Read("Parameters/CameraBasler01/offsetX"));
                cameraBasler01_offsetY = Convert.ToInt32(xmlRW.Read("Parameters/CameraBasler01/offsetY"));
                cameraBasler01_offsetW = Convert.ToInt32(xmlRW.Read("Parameters/CameraBasler01/offsetW"));
                cameraBasler01_offsetH = Convert.ToInt32(xmlRW.Read("Parameters/CameraBasler01/offsetH"));

                cameraBasler02_name = xmlRW.Read("Parameters/CameraBasler02/name");
                cameraBasler02_offsetX = Convert.ToInt32(xmlRW.Read("Parameters/CameraBasler02/offsetX"));
                cameraBasler02_offsetY = Convert.ToInt32(xmlRW.Read("Parameters/CameraBasler02/offsetY"));
                cameraBasler02_offsetW = Convert.ToInt32(xmlRW.Read("Parameters/CameraBasler02/offsetW"));
                cameraBasler02_offsetH = Convert.ToInt32(xmlRW.Read("Parameters/CameraBasler02/offsetH"));

                inADmin = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_in_AD_min"));
                inADmax = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_in_AD_max"));
                inCDmin = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_in_CD_min"));
                inCDmax = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_in_CD_max"));
                inACmin = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_in_AC_min"));
                inACmax = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_in_AC_max"));

                outADmin = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_out_AD_min"));
                outADmax = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_out_AD_max"));
                outCDmin = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_out_CD_min"));
                outCDmax = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_out_CD_max"));
                outACmin = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_out_AC_min"));
                outACmax = Convert.ToDouble(xmlRW.Read("Parameters/Standard/standard_out_AC_max"));

                upK1 = Convert.ToDouble(xmlRW.Read("Parameters/Calibrate/upK1"));
                upB1 = Convert.ToDouble(xmlRW.Read("Parameters/Calibrate/upB1"));
                downK1 = Convert.ToDouble(xmlRW.Read("Parameters/Calibrate/downK1"));
                downB1 = Convert.ToDouble(xmlRW.Read("Parameters/Calibrate/downB1"));

                upK2 = Convert.ToDouble(xmlRW.Read("Parameters/Calibrate/upK2"));
                upB2 = Convert.ToDouble(xmlRW.Read("Parameters/Calibrate/upB2"));
                downK2 = Convert.ToDouble(xmlRW.Read("Parameters/Calibrate/downK2"));
                downB2 = Convert.ToDouble(xmlRW.Read("Parameters/Calibrate/downB2"));

                //参数显示到UI界面
                txtIp.Text = xmlRW.Read("Parameters/TCPIP/ip");
                txtPort.Text = xmlRW.Read("Parameters/TCPIP/port");

                txtCameraBasler01_name.Text = xmlRW.Read("Parameters/CameraBasler01/name");
                txtCameraBasler01_offsetX.Text = xmlRW.Read("Parameters/CameraBasler01/offsetX");
                txtCameraBasler01_offsetY.Text = xmlRW.Read("Parameters/CameraBasler01/offsetY");
                txtCameraBasler01_offsetW.Text = xmlRW.Read("Parameters/CameraBasler01/offsetW");
                txtCameraBasler01_offsetH.Text = xmlRW.Read("Parameters/CameraBasler01/offsetH");

                txtCameraBasler02_name.Text = xmlRW.Read("Parameters/CameraBasler02/name");
                txtCameraBasler02_offsetX.Text = xmlRW.Read("Parameters/CameraBasler02/offsetX");
                txtCameraBasler02_offsetY.Text = xmlRW.Read("Parameters/CameraBasler02/offsetY");
                txtCameraBasler02_offsetW.Text = xmlRW.Read("Parameters/CameraBasler02/offsetW");
                txtCameraBasler02_offsetH.Text = xmlRW.Read("Parameters/CameraBasler02/offsetH");

                txtInADMin.Text = xmlRW.Read("Parameters/Standard/standard_in_AD_min");
                txtInADMax.Text = xmlRW.Read("Parameters/Standard/standard_in_AD_max");
                txtOutADMin.Text = xmlRW.Read("Parameters/Standard/standard_out_AD_min");
                txtOutADMax.Text = xmlRW.Read("Parameters/Standard/standard_out_AD_max");

                txtInCDMin.Text = xmlRW.Read("Parameters/Standard/standard_in_CD_min");
                txtInCDMax.Text = xmlRW.Read("Parameters/Standard/standard_in_CD_max");
                txtOutCDMin.Text = xmlRW.Read("Parameters/Standard/standard_out_CD_min");
                txtOutCDMax.Text = xmlRW.Read("Parameters/Standard/standard_out_CD_max");

                txtInACMin.Text = xmlRW.Read("Parameters/Standard/standard_in_AC_min");
                txtInACMax.Text = xmlRW.Read("Parameters/Standard/standard_in_AC_max");
                txtOutACMin.Text = xmlRW.Read("Parameters/Standard/standard_out_AC_min");
                txtOutACMax.Text = xmlRW.Read("Parameters/Standard/standard_out_AC_max");

                txtUpK1.Text = xmlRW.Read("Parameters/Calibrate/upK1");
                txtUpB1.Text = xmlRW.Read("Parameters/Calibrate/upB1");
                txtDownK1.Text = xmlRW.Read("Parameters/Calibrate/downK1");
                txtDownB1.Text = xmlRW.Read("Parameters/Calibrate/downB1");

                txtUpK2.Text = xmlRW.Read("Parameters/Calibrate/upK2");
                txtUpB2.Text = xmlRW.Read("Parameters/Calibrate/upB2");
                txtDownK2.Text = xmlRW.Read("Parameters/Calibrate/downK2");
                txtDownB2.Text = xmlRW.Read("Parameters/Calibrate/downB2");


                return true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
                return false;
            }
        }

        private void btnStaticDetect1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ho_Image1.IsInitialized())
                {
                    HOperatorSet.ReadImage(out ho_Image1, AppDomain.CurrentDomain.BaseDirectory + "Parameters\\静态检测图片\\1.bmp");
                }            
            }
            catch (Exception exc)
            {
                ActiveBotton();
                MessageBox.Show("读取静态图片失败！" + exc.ToString());
                return;
            }

            CameraBasler01_OverHandle(ho_Image1);
        }

        private void btnStaticDetect2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ho_Image2.IsInitialized())
                {
                    HOperatorSet.ReadImage(out ho_Image2, AppDomain.CurrentDomain.BaseDirectory + "Parameters\\静态检测图片\\2.bmp");
                }            
            }
            catch (Exception exc)
            {
                ActiveBotton();
                MessageBox.Show("读取静态图片失败！" + exc.ToString());
                return;
            }

            CameraBasler02_OverHandle(ho_Image2);
        }

        private void tsmiSaveImage_Click(object sender, EventArgs e)
        {
            if (windowName == "hWindowControl1")
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "请选择保存路径";
                dialog.Filter = "BMP格式（*.bmp）|.bmp;";
                string filePath;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        filePath = dialog.FileName;
                        HOperatorSet.WriteImage(ho_Image1, "bmp", 0, filePath);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("图像1_1保存失败！");
                    }
                }
            }
            else if (windowName == "hWindowControl2")
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "请选择保存路径";
                dialog.Filter = "BMP格式（*.bmp）|.bmp;";
                string filePath;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        filePath = dialog.FileName;
                        HOperatorSet.WriteImage(ho_Image2, "bmp", 0, filePath);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("图像1_2保存失败！");
                    }
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            windowName = (sender as ContextMenuStrip).SourceControl.Name;
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            chartName = (sender as ContextMenuStrip).SourceControl.Name;
        }

        private void tsmiClearLine_Click(object sender, EventArgs e)
        {
            result1Flag = true;
            result2Flag = true;
            totalResult = true;
            if (chartName == "chart1")
            {
                lInAD.Clear();
                lInCD.Clear();
                lInAC.Clear();
                firstFlag1 = true;
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
            }
            else if (chartName == "chart2")
            {
                lOutAD.Clear();
                lOutCD.Clear();
                lOutAC.Clear();
                firstFlag2 = true;
                foreach (var series in chart2.Series)
                {
                    series.Points.Clear();
                }
            }
        }

        private void tsmiClearProRecord_Click(object sender, EventArgs e)
        {
            totalNum = 0;
            okNum = 0;
            ngNum = 0;
            txtProRecord.Text = "总产量:0  良品:0  良率:100%";
        }

        private void ActiveBotton() 
        {

        }

        private void InvalidBotton()
        {

        }

        private double L2LDistance(HTuple hv_Line1Row1, HTuple hv_Line1Column1, HTuple hv_Line1Row2, HTuple hv_Line1Column2,
            HTuple hv_Line2Row1, HTuple hv_Line2Column1, HTuple hv_Line2Row2, HTuple hv_Line2Column2)
        {
            HTuple hv_DistanceMin1;
            HTuple hv_DistanceMax1;
            HTuple hv_DistanceMin2;
            HTuple hv_DistanceMax2;

            HOperatorSet.DistanceSl(hv_Line1Row1, hv_Line1Column1, hv_Line1Row2, hv_Line1Column2,
                        hv_Line2Row1, hv_Line2Column1, hv_Line2Row2, hv_Line2Column2,
                        out hv_DistanceMin1, out hv_DistanceMax1);
            HOperatorSet.DistanceSl(hv_Line2Row1, hv_Line2Column1, hv_Line2Row2, hv_Line2Column2,
                       hv_Line1Row1, hv_Line1Column1, hv_Line1Row2, hv_Line1Column2,
                       out hv_DistanceMin2, out hv_DistanceMax2);

            return ((hv_DistanceMin1 + hv_DistanceMax1 + hv_DistanceMin2 + hv_DistanceMax2) / 4);
        }

        private void btnSaveParams1_Click(object sender, EventArgs e)
        {
            try
            {
                cameraBasler01_name = txtCameraBasler01_name.Text;
                cameraBasler01_offsetX = Convert.ToInt32(txtCameraBasler01_offsetX.Text);
                cameraBasler01_offsetY = Convert.ToInt32(txtCameraBasler01_offsetY.Text);
                cameraBasler01_offsetW = Convert.ToInt32(txtCameraBasler01_offsetW.Text);
                cameraBasler01_offsetH = Convert.ToInt32(txtCameraBasler01_offsetH.Text);

                cameraBasler02_name = txtCameraBasler02_name.Text;
                cameraBasler02_offsetX = Convert.ToInt32(txtCameraBasler02_offsetX.Text);
                cameraBasler02_offsetY = Convert.ToInt32(txtCameraBasler02_offsetY.Text);
                cameraBasler02_offsetW = Convert.ToInt32(txtCameraBasler02_offsetW.Text);
                cameraBasler02_offsetH = Convert.ToInt32(txtCameraBasler02_offsetH.Text);

                ip = txtIp.Text;
                port = txtPort.Text;

                //更新到XML文件
                xmlRW.Update("Parameters/CameraBasler01/name", txtCameraBasler01_name.Text);
                xmlRW.Update("Parameters/CameraBasler01/offsetX", txtCameraBasler01_offsetX.Text);
                xmlRW.Update("Parameters/CameraBasler01/offsetY", txtCameraBasler01_offsetY.Text);
                xmlRW.Update("Parameters/CameraBasler01/offsetW", txtCameraBasler01_offsetW.Text);
                xmlRW.Update("Parameters/CameraBasler01/offsetH", txtCameraBasler01_offsetH.Text);

                xmlRW.Update("Parameters/CameraBasler02/name", txtCameraBasler02_name.Text);
                xmlRW.Update("Parameters/CameraBasler02/offsetX", txtCameraBasler02_offsetX.Text);
                xmlRW.Update("Parameters/CameraBasler02/offsetY", txtCameraBasler02_offsetY.Text);
                xmlRW.Update("Parameters/CameraBasler02/offsetW", txtCameraBasler02_offsetW.Text);
                xmlRW.Update("Parameters/CameraBasler02/offsetH", txtCameraBasler02_offsetH.Text);

                xmlRW.Update("Parameters/TCPIP/ip", txtIp.Text);
                xmlRW.Update("Parameters/TCPIP/port", txtPort.Text);

                MessageBox.Show("保存参数成功！");
            }
            catch (Exception exc)
            {
                MessageBox.Show("保存参数失败！" + exc.ToString());
            }
        }

        private void btnSaveParams2_Click(object sender, EventArgs e)
        {
            try
            {
                inADmin = Convert.ToDouble(txtInADMin.Text);
                inADmax = Convert.ToDouble(txtInADMax.Text);
                outADmin = Convert.ToDouble(txtOutADMin.Text);
                outADmax = Convert.ToDouble(txtOutADMax.Text);

                inCDmin = Convert.ToDouble(txtInCDMin.Text);
                inCDmax = Convert.ToDouble(txtInCDMax.Text);
                outCDmin = Convert.ToDouble(txtOutCDMin.Text);
                outCDmax = Convert.ToDouble(txtOutCDMax.Text);

                inACmin = Convert.ToDouble(txtInACMin.Text);
                inACmax = Convert.ToDouble(txtInACMax.Text);
                outACmin = Convert.ToDouble(txtOutACMin.Text);
                outACmax = Convert.ToDouble(txtOutACMax.Text);

                upK1 = Convert.ToDouble(txtUpK1.Text);
                upB1 = Convert.ToDouble(txtUpB1.Text);
                downK1 = Convert.ToDouble(txtDownK1.Text);
                downB1 = Convert.ToDouble(txtDownB1.Text);

                upK2 = Convert.ToDouble(txtUpK2.Text);
                upB2 = Convert.ToDouble(txtUpB2.Text);
                downK2 = Convert.ToDouble(txtDownK2.Text);
                downB2 = Convert.ToDouble(txtDownB2.Text);

                xmlRW.Update("Parameters/Standard/standard_in_AD_min", txtInADMin.Text);
                xmlRW.Update("Parameters/Standard/standard_in_AD_max", txtInADMax.Text);
                xmlRW.Update("Parameters/Standard/standard_out_AD_min", txtOutADMin.Text);
                xmlRW.Update("Parameters/Standard/standard_out_AD_max", txtOutADMax.Text);

                xmlRW.Update("Parameters/Standard/standard_in_CD_min", txtInCDMin.Text);
                xmlRW.Update("Parameters/Standard/standard_in_CD_max", txtInCDMax.Text);
                xmlRW.Update("Parameters/Standard/standard_out_CD_min", txtOutCDMin.Text);
                xmlRW.Update("Parameters/Standard/standard_out_CD_max", txtOutCDMax.Text);

                xmlRW.Update("Parameters/Standard/standard_in_AC_min", txtInACMin.Text);
                xmlRW.Update("Parameters/Standard/standard_in_AC_max", txtInACMax.Text);
                xmlRW.Update("Parameters/Standard/standard_out_AC_min", txtOutACMin.Text);
                xmlRW.Update("Parameters/Standard/standard_out_AC_max", txtOutACMax.Text);

                xmlRW.Update("Parameters/Calibrate/upK1", txtUpK1.Text);
                xmlRW.Update("Parameters/Calibrate/upB1", txtUpB1.Text);
                xmlRW.Update("Parameters/Calibrate/upK2", txtUpK2.Text);
                xmlRW.Update("Parameters/Calibrate/upB2", txtUpB2.Text);

                xmlRW.Update("Parameters/Calibrate/downK1", txtDownK1.Text);
                xmlRW.Update("Parameters/Calibrate/downB1", txtDownB1.Text);
                xmlRW.Update("Parameters/Calibrate/downK2", txtDownK2.Text);
                xmlRW.Update("Parameters/Calibrate/downB2", txtDownB2.Text);

                MessageBox.Show("保存参数成功！");
            }
            catch (Exception exc)
            {
                MessageBox.Show("保存参数失败！" + exc.ToString());
            }
        }

        private void btnLineParams_InA_Click(object sender, EventArgs e)
        {
            InvalidBotton();
            btnLineParams_InA.Enabled = true;
            if (!btnLineParams_InA_ClickFlag)
            {
                btnLineParams_InA_ClickFlag = true;
                btnLineParams_InA.BackColor = System.Drawing.Color.Red;
                lineDetect.Show();
            }
            else
            {
                ActiveBotton();
                btnLineParams_InA_ClickFlag = false;
                btnLineParams_InA.BackColor = System.Drawing.Color.Lime;
                if (!lineDetect.lineParams.CopyTo(ref lineParams_InA))
                {
                    MessageBox.Show("参数复制失败!");
                    return;
                }
                if (!lineParams_InA.WriteParams("LineParams_InA"))
                {
                    MessageBox.Show("参数写入失败!");
                    return;
                }
                MessageBox.Show("参数保存成功！");
            }
        }

        private void btnLineParams_InC_Click(object sender, EventArgs e)
        {
            InvalidBotton();
            btnLineParams_InC.Enabled = true;
            if (!btnLineParams_InC_ClickFlag)
            {
                btnLineParams_InC_ClickFlag = true;
                btnLineParams_InC.BackColor = System.Drawing.Color.Red;
                lineDetect.Show();
            }
            else
            {
                ActiveBotton();
                btnLineParams_InC_ClickFlag = false;
                btnLineParams_InC.BackColor = System.Drawing.Color.Lime;
                if (!lineDetect.lineParams.CopyTo(ref lineParams_InC))
                {
                    MessageBox.Show("参数复制失败!");
                    return;
                }
                if (!lineParams_InC.WriteParams("LineParams_InC"))
                {
                    MessageBox.Show("参数写入失败!");
                    return;
                }
                MessageBox.Show("参数保存成功！");
            }
        }

        private void btnLineParams_InDUp_Click(object sender, EventArgs e)
        {
            InvalidBotton();
            btnLineParams_InDUp.Enabled = true;
            if (!btnLineParams_InDUp_ClickFlag)
            {
                btnLineParams_InDUp_ClickFlag = true;
                btnLineParams_InDUp.BackColor = System.Drawing.Color.Red;
                lineDetect.Show();
            }
            else
            {
                ActiveBotton();
                btnLineParams_InDUp_ClickFlag = false;
                btnLineParams_InDUp.BackColor = System.Drawing.Color.Lime;
                if (!lineDetect.lineParams.CopyTo(ref lineParams_InDUp))
                {
                    MessageBox.Show("参数复制失败!");
                    return;
                }
                if (!lineParams_InDUp.WriteParams("LineParams_InDUp"))
                {
                    MessageBox.Show("参数写入失败!");
                    return;
                }
                MessageBox.Show("参数保存成功！");
            }
        }

        private void btnLineParams_InDDown_Click(object sender, EventArgs e)
        {
            InvalidBotton();
            btnLineParams_InDDown.Enabled = true;
            if (!btnLineParams_InDDown_ClickFlag)
            {
                btnLineParams_InDDown_ClickFlag = true;
                btnLineParams_InDDown.BackColor = System.Drawing.Color.Red;
                lineDetect.Show();
            }
            else
            {
                ActiveBotton();
                btnLineParams_InDDown_ClickFlag = false;
                btnLineParams_InDDown.BackColor = System.Drawing.Color.Lime;
                if (!lineDetect.lineParams.CopyTo(ref lineParams_InDDown))
                {
                    MessageBox.Show("参数复制失败!");
                    return;
                }
                if (!lineParams_InDDown.WriteParams("LineParams_InDDown"))
                {
                    MessageBox.Show("参数写入失败!");
                    return;
                }
                MessageBox.Show("参数保存成功！");
            }
        }

        private void btnLineParams_OutA_Click(object sender, EventArgs e)
        {
            InvalidBotton();
            btnLineParams_OutA.Enabled = true;
            if (!btnLineParams_OutA_ClickFlag)
            {
                btnLineParams_OutA_ClickFlag = true;
                btnLineParams_OutA.BackColor = System.Drawing.Color.Red;
                lineDetect.Show();
            }
            else
            {
                ActiveBotton();
                btnLineParams_OutA_ClickFlag = false;
                btnLineParams_OutA.BackColor = System.Drawing.Color.Lime;
                if (!lineDetect.lineParams.CopyTo(ref lineParams_OutA))
                {
                    MessageBox.Show("参数复制失败!");
                    return;
                }
                if (!lineParams_OutA.WriteParams("LineParams_OutA"))
                {
                    MessageBox.Show("参数写入失败!");
                    return;
                }
                MessageBox.Show("参数保存成功！");
            }
        }

        private void btnLineParams_OutC_Click(object sender, EventArgs e)
        {
            InvalidBotton();
            btnLineParams_OutC.Enabled = true;
            if (!btnLineParams_OutC_ClickFlag)
            {
                btnLineParams_OutC_ClickFlag = true;
                btnLineParams_OutC.BackColor = System.Drawing.Color.Red;
                lineDetect.Show();
            }
            else
            {
                ActiveBotton();
                btnLineParams_OutC_ClickFlag = false;
                btnLineParams_OutC.BackColor = System.Drawing.Color.Lime;
                if (!lineDetect.lineParams.CopyTo(ref lineParams_OutC))
                {
                    MessageBox.Show("参数复制失败!");
                    return;
                }
                if (!lineParams_OutC.WriteParams("LineParams_OutC"))
                {
                    MessageBox.Show("参数写入失败!");
                    return;
                }
                MessageBox.Show("参数保存成功！");
            }
        }

        private void btnLineParams_OutDUp_Click(object sender, EventArgs e)
        {
            InvalidBotton();
            btnLineParams_OutDUp.Enabled = true;
            if (!btnLineParams_OutDUp_ClickFlag)
            {
                btnLineParams_OutDUp_ClickFlag = true;
                btnLineParams_OutDUp.BackColor = System.Drawing.Color.Red;
                lineDetect.Show();
            }
            else
            {
                ActiveBotton();
                btnLineParams_OutDUp_ClickFlag = false;
                btnLineParams_OutDUp.BackColor = System.Drawing.Color.Lime;
                if (!lineDetect.lineParams.CopyTo(ref lineParams_OutDUp))
                {
                    MessageBox.Show("参数复制失败!");
                    return;
                }
                if (!lineParams_OutDUp.WriteParams("LineParams_OutDUp"))
                {
                    MessageBox.Show("参数写入失败!");
                    return;
                }
                MessageBox.Show("参数保存成功！");
            }
        }

        private void btnLineParams_OutDDown_Click(object sender, EventArgs e)
        {
            InvalidBotton();
            btnLineParams_OutDDown.Enabled = true;
            if (!btnLineParams_OutDDown_ClickFlag)
            {
                btnLineParams_OutDDown_ClickFlag = true;
                btnLineParams_OutDDown.BackColor = System.Drawing.Color.Red;
                lineDetect.Show();
            }
            else
            {
                ActiveBotton();
                btnLineParams_OutDDown_ClickFlag = false;
                btnLineParams_OutDDown.BackColor = System.Drawing.Color.Lime;
                if (!lineDetect.lineParams.CopyTo(ref lineParams_OutDDown))
                {
                    MessageBox.Show("参数复制失败!");
                    return;
                }
                if (!lineParams_OutDDown.WriteParams("LineParams_OutDDown"))
                {
                    MessageBox.Show("参数写入失败!");
                    return;
                }
                MessageBox.Show("参数保存成功！");
            }
        }

        public void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
      HTuple hv_Bold, HTuple hv_Slant)
        {
            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_Fonts = new HTuple();
            HTuple hv_Style = null, hv_Exception = new HTuple(), hv_AvailableFonts = null;
            HTuple hv_Fdx = null, hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Restore previous behaviour
                hv_Size_COPY_INP_TMP = ((1.13677 * hv_Size_COPY_INP_TMP)).TupleInt();
            }
            else
            {
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP.TupleInt();
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Courier";
                hv_Fonts[1] = "Courier 10 Pitch";
                hv_Fonts[2] = "Courier New";
                hv_Fonts[3] = "CourierNew";
                hv_Fonts[4] = "Liberation Mono";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Consolas";
                hv_Fonts[1] = "Menlo";
                hv_Fonts[2] = "Courier";
                hv_Fonts[3] = "Courier 10 Pitch";
                hv_Fonts[4] = "FreeMono";
                hv_Fonts[5] = "Liberation Mono";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Luxi Sans";
                hv_Fonts[1] = "DejaVu Sans";
                hv_Fonts[2] = "FreeSans";
                hv_Fonts[3] = "Arial";
                hv_Fonts[4] = "Liberation Sans";
            }
            else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
            {
                hv_Fonts = new HTuple();
                hv_Fonts[0] = "Times New Roman";
                hv_Fonts[1] = "Luxi Serif";
                hv_Fonts[2] = "DejaVu Serif";
                hv_Fonts[3] = "FreeSerif";
                hv_Fonts[4] = "Utopia";
                hv_Fonts[5] = "Liberation Serif";
            }
            else
            {
                hv_Fonts = hv_Font_COPY_INP_TMP.Clone();
            }
            hv_Style = "";
            if ((int)(new HTuple(hv_Bold.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Bold";
            }
            else if ((int)(new HTuple(hv_Bold.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Bold";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Slant.TupleEqual("true"))) != 0)
            {
                hv_Style = hv_Style + "Italic";
            }
            else if ((int)(new HTuple(hv_Slant.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Slant";
                throw new HalconException(hv_Exception);
            }
            if ((int)(new HTuple(hv_Style.TupleEqual(""))) != 0)
            {
                hv_Style = "Normal";
            }
            HOperatorSet.QueryFont(hv_WindowHandle, out hv_AvailableFonts);
            hv_Font_COPY_INP_TMP = "";
            for (hv_Fdx = 0; (int)hv_Fdx <= (int)((new HTuple(hv_Fonts.TupleLength())) - 1); hv_Fdx = (int)hv_Fdx + 1)
            {
                hv_Indices = hv_AvailableFonts.TupleFind(hv_Fonts.TupleSelect(hv_Fdx));
                if ((int)(new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(((hv_Indices.TupleSelect(0))).TupleGreaterEqual(0))) != 0)
                    {
                        hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_Fdx);
                        break;
                    }
                }
            }
            if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(""))) != 0)
            {
                throw new HalconException("Wrong value of control parameter Font");
            }
            hv_Font_COPY_INP_TMP = (((hv_Font_COPY_INP_TMP + "-") + hv_Style) + "-") + hv_Size_COPY_INP_TMP;
            HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP);
            // dev_set_preferences(...); only in hdevelop

            return;
        }

        private void computerGrabTime1(long time)
        {
            //txtCameraTime1.Text = time.ToString() + "ms";
        }
        private void computerGrabTime2(long time)
        {
            //txtCameraTime1.Text = time.ToString() + "ms";
        }

        private void SignalDetect(object sender, EventArgs e)
        {
            if(modbus_TCP.Modbus_Read_Holding_Register(1,1100,1)[0]==1)    
            {
                if (highLevel == false) //检测上升沿信号
                {
                    highLevel = true;
                    DetectStart();
                }
            }
            else
            {
                if (highLevel == true)  //检测下降沿信号
                {
                    highLevel = false;
                    DetectStop();
                }
            }
        }

        private void DetectStart()
        {
            set_display_font(hv_ImageWindow1, 12, "mono", "true", "false");
            set_display_font(hv_ImageWindow2, 12, "mono", "true", "false");

            lInAD.Clear();
            lInCD.Clear();
            lInAC.Clear();
            lOutAD.Clear();
            lOutCD.Clear();
            lOutAC.Clear();

            lScreen1.Clear();
            lScreen2.Clear();
            lOKOrNG1.Clear();
            lOKOrNG2.Clear();

            result1Flag = true;
            result2Flag = true;
            totalResult = true;

            firstFlag1 = true;
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            firstFlag2 = true;
            foreach (var series in chart2.Series)
            {
                series.Points.Clear();
            }

            cameraBasler01.StartGrabbing();
            cameraBasler02.StartGrabbing();
        }

        private void DetectStop()
        {
            cameraBasler01.StopGrabbing();
            cameraBasler02.StopGrabbing();
            totalNum++;
            if (result1Flag)
            {
                txtResult1.BackColor = Color.Green;
                txtResult1.Text = "OK";
            }
            else
            {
                txtResult1.BackColor = Color.Red;
                txtResult1.Text = "NG";
            }
            if (result2Flag)
            {
                txtResult2.BackColor = Color.Green;
                txtResult2.Text = "OK";
            }
            else
            {
                txtResult2.BackColor = Color.Red;
                txtResult2.Text = "NG";
            }
            totalResult = result1Flag;
            if (lInAD.Count > 0 && lOutAD.Count > 0)  //有数据才发送数据进行处理
            {
                if (!totalResult)//最终检测的NG结果
                {
                    modbus_TCP.Modbus_Write_Single_Holding_Register(1, 1101, 1); //发送NG信号
                    txtTotalResult.BackColor = Color.Red;
                    txtTotalResult.Text = "NG";
                    ngNum++;
                    rtboxProLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   " + "NG\n");
                    rtboxProLog.SelectionStart = rtboxProLog.Text.Length;
                    rtboxProLog.ScrollToCaret();

                   
                }
                else
                {
                    modbus_TCP.Modbus_Write_Single_Holding_Register(1, 1101, 0); //发送OK信号
                    txtTotalResult.BackColor = Color.Green;
                    txtTotalResult.Text = "OK";
                    okNum++;
                    rtboxProLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   " + "OK\n");
                    rtboxProLog.SelectionStart = rtboxProLog.Text.Length;
                    rtboxProLog.ScrollToCaret();
                }

                int sum = 0;
                for (int i = 0; i < lInAD.Count; i++)
                {
                    sum += (int)(lInAD[i] * 100);
                }
                iInAD = sum / lInAD.Count;

                sum = 0;
                for (int i = 0; i < lInAC.Count; i++)
                {
                    sum += (int)(lInAC[i] * 100);
                }
                iInAC = sum / lInAC.Count;

                sum = 0;
                for (int i = 0; i < lOutAD.Count; i++)
                {
                    sum += (int)(lOutAD[i] * 100);
                }
                iOutAD = sum / lOutAD.Count;

                sum = 0;
                for (int i = 0; i < lOutAC.Count; i++)
                {
                    sum += (int)(lOutAC[i] * 100);
                }
                iOutAC = sum / lOutAC.Count;

                int[] sendData = new int[4];
                sendData[0] = iInAD;
                sendData[1] = iInAC;
                sendData[2] = iOutAD;
                sendData[3] = iOutAC;
                if (iInAD < 0)
                {
                    iInAD = 1000;
                }
                if (iInAC < 0)
                {
                    iInAC = 1000;
                }
                if (iOutAD < 0)
                {
                    iOutAD = 1000;
                }
                if (iOutAC < 0)
                {
                    iOutAC = 1000;
                }
                modbus_TCP.Modbus_Write_Single_Holding_Register(1, 1120, iInAD); //发送软件打开信号给PLC
                modbus_TCP.Modbus_Write_Single_Holding_Register(1, 1121, iInAC); //发送软件打开信号给PLC
                modbus_TCP.Modbus_Write_Single_Holding_Register(1, 1122, iOutAD); //发送软件打开信号给PLC
                modbus_TCP.Modbus_Write_Single_Holding_Register(1, 1123, iOutAC); //发送软件打开信号给PLC
                //modbus_TCP.Modbus_Write_Holding_Registers(1, 1120, sendData);

                string path = AppDomain.CurrentDomain.BaseDirectory + "Screen\\"+ DateTime.Now.ToString("yyyy-MM-dd")+"\\"+
                    DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff") + "\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //if (lScreen1.Count == lScreen2.Count)
                //{
                //    for (int i = 0; i < lOKOrNG1.Count; i++)
                //    {
                //        if (!(lOKOrNG1[i] && lOKOrNG2[i]))
                //        {
                //            string screenPath1 = path + "1_" + i.ToString();
                //            string screenPath2 = path + "2_" + i.ToString();
                //            HOperatorSet.WriteImage(lScreen1[i], "png", 0, screenPath1);
                //            HOperatorSet.WriteImage(lScreen2[i], "png", 0, screenPath2);
                //        }
                //    }
                //}

                for (int i = 0; i < lOKOrNG1.Count; i++)
                {
                    if (!lOKOrNG1[i])
                    {
                        string screenPath1 = path + "1_" + i.ToString();
                        //string screenPath2 = path + "2_" + i.ToString();
                        HOperatorSet.WriteImage(lScreen1[i], "png", 0, screenPath1);
                        //HOperatorSet.WriteImage(lScreen2[i], "png", 0, screenPath2);
                    }
                }
            }
            else
            {
                modbus_TCP.Modbus_Write_Single_Holding_Register(1, 1101, 1); //发送NG信号
                txtTotalResult.BackColor = Color.Red;
                txtTotalResult.Text = "NG";
                ngNum++;
                rtboxProLog.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   " + "NG\n");
                rtboxProLog.SelectionStart = rtboxProLog.Text.Length;
                rtboxProLog.ScrollToCaret();
            }
            csvArray[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            csvArray[1] = iInAC.ToString();
            csvArray[2] = iInAD.ToString();
            csvArray[3] = iOutAC.ToString();
            csvArray[4] = iOutAD.ToString();
            csvArray[5] = lInAD.Count.ToString();
            csvArray[6] = lOutAD.Count.ToString();
            csvArray[7] = txtTotalResult.Text;
            RecordsWrite.CsvWrite.Write(csvArray, true);

            okRate = (double)okNum / totalNum;
            txtProRecord.Text = "总产量:" + totalNum.ToString() + "  良品:" +okNum.ToString() + "  良率:" +okRate.ToString("0.###%");
        }

        private void btnDetectImage1_Click(object sender, EventArgs e)
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
                    HOperatorSet.ReadImage(out ho_Image1, filePath);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取本地图像失败！" + exc.ToString());
                }
                CameraBasler01_OverHandle(ho_Image1);
            }
        }

        private void btnDetectImage2_Click(object sender, EventArgs e)
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
                    HOperatorSet.ReadImage(out ho_Image2, filePath);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取本地图像失败！" + exc.ToString());
                }
                CameraBasler02_OverHandle(ho_Image2);
            }
        }
    }
}
