using System;
using HslCommunication;
using System.Windows.Forms;
using HslCommunication.ModBus;

namespace Standard_UI.Comunication
{
    class ModBus_Hsl
    {
        private ModbusTcpNet busTcpClient;

        public object lockObj1 = new object();

        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <param name="IPAddress"></param>
        public bool connectPLC(string IPAddress)
        {
            busTcpClient = new ModbusTcpNet(IPAddress);   // 端口号502，站号1
            OperateResult write = busTcpClient.ConnectServer();
            if (write.IsSuccess)
            {
                MessageBox.Show("连接PLC成功！！!");
                //NetLog.WriteTextLog("连接PLC成功！！!");
                return true;
            }
            else
            {
                MessageBox.Show(write.ToMessageShowString());
                //NetLog.WriteTextLog("连接PLC失败！！!" + write.ToMessageShowString());
                return false;
            }

        }

        /// <summary>
        /// 写入指令
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Value"></param>
        public bool writePLC(string Address,string Value)
        {
            try
            {
                lock (lockObj1)
                {
                    OperateResult write = busTcpClient.WriteOneRegister(Address, short.Parse(Value));
                    if (write.IsSuccess)
                    {
                        // 写入成功
                        //label5.Text = "写入成功";
                        //NetLog.WriteTextLog("写入成功");
                        return true;
                    }
                    else
                    {
                        //MessageBox.Show(write.ToMessageShowString());
                        //NetLog.WriteTextLog("写入失败" + write.ToMessageShowString());
                        return false;
                    }
                }
                   
            }
            catch(Exception ee)
            {
                //NetLog.WriteTextLog("写入失败" + ee.Message);
                return false;
            }

        }

        /// <summary>
        /// 读取指令
        /// </summary>
        /// <param name="Address"></param>
        public short readPLC(string Address)
        {
            try
            {
                lock (lockObj1)
                {
                    OperateResult<byte[]> read = busTcpClient.Read(Address, 10);
                    if (read.IsSuccess)
                    {
                        // 共返回20个字节，每个数据2个字节，高位在前，低位在后
                        // 在数据解析前需要知道里面到底存了什么类型的数据，所以需要进行一些假设：
                        // 前两个字节是short数据类型
                        short value1 = busTcpClient.ByteTransform.TransInt16(read.Content, 0);


                        //textBox4.Text = value1.ToString();

                        //NetLog.WriteTextLog("读取地址" + Address + "值为：" + value1.ToString());

                        //// 接下来的2个字节是ushort类型
                        //ushort value2 = busTcpClient.ByteTransform.TransUInt16(read.Content, 2);
                        //// 接下来的4个字节是int类型
                        //int value3 = busTcpClient.ByteTransform.TransInt32(read.Content, 4);
                        //// 接下来的4个字节是float类型
                        //float value4 = busTcpClient.ByteTransform.TransFloat(read.Content, 8);
                        //// 接下来的全部字节，共8个字节是规格信息
                        //string speci = Encoding.ASCII.GetString(read.Content, 12, 8);

                        // 已经提取完所有的数据

                        return value1;
                    }
                    else
                    {
                        //MessageBox.Show(read.ToMessageShowString());
                        //NetLog.WriteTextLog("读取地址失败" + read.ToMessageShowString());
                        return 0;
                    }
                }
                
            }
            catch(Exception ee)
            {
                //NetLog.WriteTextLog("读取地址失败" + ee.Message);
                return 0;
            }

        }

        /// <summary>
        /// 关闭PLC
        /// </summary>
        public void closePLC()
        {
            //busTcpClient.ConnectClose();
            busTcpClient = null;
        }

        #region
        //public bool OpenPLC(string IPAddress)
        //{
        //    try
        //    {

        //        _SiementsTcpNet = new SiemensS7Net(SiemensPLCS.S1200, IPAddress);
        //        _SiementsTcpNet.ConnectTimeOut = 5000;
        //        _SiementsTcpNet.Port = 102;
        //        OperateResult result = _SiementsTcpNet.ConnectServer();

        //        MessageBox.Show((result.IsSuccess).ToString());

        //        if (result.IsSuccess)
        //        {
        //            //AtuoRun = true;
        //            MessageBox.Show("连接PLC成功");
        //            return true;
        //        }
        //        else
        //        {
        //            MessageBox.Show("连接PLC失败");
        //            return false;
        //        }
                

        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //        return false;
        //    }
        //}

        //public void ClosePLC()
        //{
        //    if (_SiementsTcpNet != null)
        //    {
        //        _SiementsTcpNet.ConnectClose();
        //        _SiementsTcpNet = null;
        //    }
        //}

        //public bool writeOrder(string address, string writeValue)
        //{

        //    lock (lockObj1)
        //    {
        //        OperateResult result = _SiementsTcpNet.Write(address, int.Parse(writeValue));

        //        ////OperateResult result = _SiementsTcpNet.Write(Address, Convert.ToUInt32(writeValue));
        //        ////MessageBox.Show(result.IsSuccess.ToString());
        //        ////lblWriteStatus.Content = result.IsSuccess;
        //        //if (result.IsSuccess == false)
        //        //{
        //        //    //MessageBox.Show("写入指令不成功，请检查PLC是否连接上");
        //        //}
        //        return result.IsSuccess;
        //    }


        //}

        //public int readOrder(string Address)
        //{

        //    lock (lockObj1)
        //    {
        //        //OperateResult<UInt32> result = _SiementsTcpNet.ReadUInt32(Address);
        //        OperateResult<int> result1 = _SiementsTcpNet.ReadInt32(Address);

        //        //uint readValue = result.Content;

        //        return result1.Content; ;
        //    }

        //}



        //public Socket newclient;


        ///// <summary>
        ///// 连接PLC
        ///// </summary>
        ///// <param name="serverIp"></param>
        ///// <param name="serverPort"></param>
        ///// <returns></returns>
        //public bool Connect(string serverIp, string serverPort)
        //{
        //    byte[] data = new byte[1024];
        //    newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    string ipadd = serverIp.Trim();
        //    int port = Convert.ToInt32(serverPort.Trim());
        //    IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ipadd), port);
        //    try
        //    {
        //        newclient.Connect(ie);
        //        System.Windows.Forms.MessageBox.Show("连接成功");
        //    }
        //    catch (SocketException e)
        //    {
        //        //链接失败
        //        System.Windows.Forms.MessageBox.Show("链接失败");
        //        return false;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 读消息
        ///// </summary>
        //public string ReceiveMsg()
        //{
        //    //while (true)
        //    //{
        //        byte[] data = new byte[1024];
        //        string RecvMsg;
        //        try
        //        {
        //            int recv = newclient.Receive(data);
        //            RecvMsg = Encoding.UTF8.GetString(data, 0, recv);
        //        return RecvMsg;
        //        }
        //        catch (SocketException e)
        //        {

        //            newclient.Dispose();
        //         RecvMsg = null;
        //        return RecvMsg;
        //            //break;
        //        }
        //    //}
        //}

        ///// <summary>
        ///// 发消息
        ///// </summary>
        ///// <param name="StrMsg"></param>
        //public void SendMsg(string StrMsg)
        //{
        //    int m_length = StrMsg.Length;
        //    byte[] data = new byte[m_length];
        //    data = Encoding.UTF8.GetBytes(StrMsg);
        //    int i = newclient.Send(data);
        //}

        ///// <summary>
        ///// 断开连接
        ///// </summary>
        //public void mAbort()
        //{
        //    if (newclient != null)
        //    {
        //        newclient.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 连接状态
        ///// </summary>
        ///// <returns></returns>
        //public bool IsConnected()
        //{
        //    if (newclient != null)
        //        return newclient.Connected;
        //    else
        //        return false;
        //}
        ///// <summary>
        ///// PLC监视
        ///// </summary>
        //public void Monitor()
        //{
        //    string readPLC;
        //    string OneReadPLC;
        //    byte[] data = new byte[1024];

        //    int recv = newclient.Receive(data);
        //    readPLC = OneReadPLC = Encoding.UTF8.GetString(data, 0, recv);

        //    while (true )
        //    {
        //        OneReadPLC = Encoding.UTF8.GetString(data, 0, recv);
        //        if (readPLC != OneReadPLC)
        //        {
        //            break;
        //        }
        //    }
        //}
        #endregion
    }
}
