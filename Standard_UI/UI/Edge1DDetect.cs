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
    public partial class Edge1DDetect : Form
    {
        #region 控制参数
        HTuple hv_StartX;
        HTuple hv_StartY;
        HTuple hv_ZoomFactor;
        HTuple hv_ImageWindow;

        Edge1DParams edgeParams;
 
        #endregion

        public Edge1DDetect()
        {
            InitializeComponent();

            hv_StartX = 0;
            hv_StartY = 0;
            hv_ZoomFactor = 1;
            hv_ImageWindow = hWindowControl1.HalconID;

            edgeParams = new Edge1DParams();

            //edgeParams.eventSaveEdgeParams += save();
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
            }
            catch (Exception exc)
            {
                MessageBox.Show("图像显示错误！" + exc.ToString());
            }
        }

        private void btnSaveEdgeParams_Click(object sender, EventArgs e)
        {
            
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
                    HOperatorSet.ReadImage(out edgeParams.ho_Image, filePath);
                    Show2HWindow(edgeParams.ho_Image);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("读取静态图像失败！" + exc.ToString());
                }
            }
        }

        private void tsmiSelectRectangle_Click(object sender, EventArgs e)
        {
            HTuple hv_Row ;
            HTuple hv_Column ;
            HTuple hv_Phi ;
            HTuple hv_Length1 ;
            HTuple hv_Length2 ;

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DrawRectangle2(hv_ImageWindow, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2);

            edgeParams.hv_Row = hv_Row / hv_ZoomFactor;
            edgeParams.hv_Column = hv_Column / hv_ZoomFactor;
            edgeParams.hv_Phi = hv_Phi;
            edgeParams.hv_Length1 = hv_Length1 / hv_ZoomFactor;
            edgeParams.hv_Length2 = hv_Length2 / hv_ZoomFactor;

            HOperatorSet.GetImageSize(edgeParams.ho_Image, out edgeParams.hv_Width, out edgeParams.hv_Height);

            DetectPoints();
        }

        private bool DetectPoints()
        {
            edgeParams.hv_MeasureHandles.Clear();

            edgeParams.hv_RowEdges.Clear();
            edgeParams.hv_ColumnEdges.Clear();

            HTuple hv_Row = null;
            HTuple hv_Column = null;
            HTuple hv_Phi = null;
            HTuple hv_Length1 = null;
            HTuple hv_Length2 = null;
            
            hv_Phi = edgeParams.hv_Phi;
            hv_Length1 = edgeParams.hv_Length1;
            HOperatorSet.TupleInt(edgeParams.hv_Length2/ edgeParams.divideParts, out hv_Length2);

            if (hv_Length2 < 1)
            {
                MessageBox.Show("ROI过窄，请重新选择");
                return false;
            }
            
            HObject ho_DspXld;
            HOperatorSet.GenEmptyObj(out ho_DspXld);

            for (int i=0;i< edgeParams.divideParts;i++)
            {
                hv_Row = edgeParams.hv_Row- hv_Length2*2 *hv_Phi.TupleCos()*((edgeParams.divideParts+1)/2-i);
                hv_Column = edgeParams.hv_Column - hv_Length2*2* hv_Phi.TupleSin() * ((edgeParams.divideParts + 1) / 2 - i);

                HOperatorSet.GenMeasureRectangle2(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2, edgeParams.hv_Width, edgeParams.hv_Height,
                    edgeParams.hv_Interpolation,out edgeParams.hv_MeasureHandle);

                HOperatorSet.MeasurePos(edgeParams.ho_Image, edgeParams.hv_MeasureHandle, edgeParams.hv_Sigma, edgeParams.hv_Threshold, edgeParams.hv_Transition,
                    edgeParams.hv_Select,out edgeParams.hv_RowEdge, out edgeParams.hv_ColumnEdge, out edgeParams.hv_Amplitude, out edgeParams.hv_Distance);

                if (edgeParams.hv_RowEdge.Length !=1)
                {
                    continue;
                }

                edgeParams.hv_RowEdges.Add(edgeParams.hv_RowEdge);
                edgeParams.hv_ColumnEdges.Add(edgeParams.hv_ColumnEdge);

                edgeParams.hv_MeasureHandles.Add(edgeParams.hv_MeasureHandle);
                //HOperatorSet.CloseMeasure(hv_MeasureHandle);

                HObject ho_Cross;
                HOperatorSet.GenEmptyObj(out ho_Cross);

                HOperatorSet.GenCrossContourXld(out ho_Cross, edgeParams.hv_RowEdge, edgeParams.hv_ColumnEdge, 10, (new HTuple(45)).TupleRad());
                HOperatorSet.ConcatObj(ho_DspXld, ho_Cross, out ho_DspXld);
            }

            HTuple hv_XldHomMat2D;
            HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);

            HOperatorSet.HomMat2dScale(hv_XldHomMat2D, hv_ZoomFactor, hv_ZoomFactor, 0, 0, out hv_XldHomMat2D);

            HOperatorSet.AffineTransContourXld(ho_DspXld, out ho_DspXld, hv_XldHomMat2D);

            Show2HWindow(edgeParams.ho_Image);

            HOperatorSet.SetColor(hv_ImageWindow, "red");
            HOperatorSet.DispObj(ho_DspXld, hv_ImageWindow);

            FitLine(edgeParams.hv_RowEdges, edgeParams.hv_ColumnEdges);

            return true;
        }

        private bool FitLine(List<HTuple> hv_RowEdges,List<HTuple> hv_ColumnEdges)
        {
            if (hv_RowEdges.Count < 2 ||  hv_RowEdges.Count != hv_ColumnEdges.Count)
            {
                //MessageBox.Show("样本点数据错误");
                return false;
            }
            HTuple hv_Mx = null;
            HTuple hv_My = null;
            for (int i = 0; i < hv_RowEdges.Count; i++)
            {
                //HOperatorSet.TupleInsert(hv_Mx,i, hv_RowEdges[i],out hv_Mx);
                //HOperatorSet.TupleInsert(hv_My, i, hv_ColumnEdges[i], out hv_My);

                HOperatorSet.TupleInsert(hv_My, i, hv_RowEdges[i], out hv_My);
                HOperatorSet.TupleInsert(hv_Mx, i, hv_ColumnEdges[i], out hv_Mx);
            }
            HTuple hv_x = null;
            HTuple hv_y = null;
            HOperatorSet.CreateMatrix(hv_RowEdges.Count, 2, 1, out hv_x);
            HOperatorSet.CreateMatrix(hv_RowEdges.Count, 1, hv_My, out hv_y);

            HOperatorSet.SetValueMatrix(hv_x, HTuple.TupleGenSequence(0, hv_RowEdges.Count - 1, 1), HTuple.TupleGenConst(hv_RowEdges.Count, 0), hv_Mx);

            HTuple hv_xtx = null;
            HTuple hv_xty = null;
            HOperatorSet.MultMatrix(hv_x, hv_x, "ATB", out hv_xtx);
            HOperatorSet.MultMatrix(hv_x, hv_y, "ATB", out hv_xty);

            HTuple hv_invxtx = null;
            HOperatorSet.InvertMatrix(hv_xtx, "general", 0, out hv_invxtx);

            HTuple hv_beta = null;
            HOperatorSet.MultMatrix(hv_invxtx, hv_xty, "AB", out hv_beta);
         
            HTuple hv_Values = null;
            HOperatorSet.GetFullMatrix(hv_beta, out hv_Values);

            HTuple hv_Newy = null;
            hv_Newy = ((hv_Values.TupleSelect(0)) * (new HTuple(10)).TupleConcat(800)) + (hv_Values.TupleSelect(1));


            HObject ho_Contour;
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Newy, (new HTuple(10)).TupleConcat(800));


            HTuple hv_XldHomMat2D;
            HOperatorSet.HomMat2dIdentity(out hv_XldHomMat2D);

            HOperatorSet.HomMat2dScale(hv_XldHomMat2D, hv_ZoomFactor, hv_ZoomFactor, 0, 0, out hv_XldHomMat2D);

            HOperatorSet.AffineTransContourXld(ho_Contour, out ho_Contour, hv_XldHomMat2D);

            HOperatorSet.DispObj(ho_Contour, hv_ImageWindow);
            //HOperatorSet.AffineTransPoint2d();
            return true;
        }
        private void save(Edge1DParams edgeParams)
        {

        }
    }
}
