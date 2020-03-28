using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Standard_UI.Comunication
{
    class Modbus_TCP
    {
        private delegate string CSD(IPEndPoint ipep, Socket s);
        private Socket newclient;
        private string IP_Address;
        private string PC_Port;

        public Modbus_TCP(string address)
        {
            IP_Address = address;
            PC_Port = "502";
        }

        public Modbus_TCP(string address, string port)
        {
            IP_Address = address;
            PC_Port = port;
        }

        private string ConnectSocket(IPEndPoint ipep, Socket s)
        {
            string m = "";
            try
            {
                s.Connect(ipep);
            }
            catch (System.Exception ex)
            {
                m = ex.Message;
            }
            return m;
        }

        /**
         * 
         * Modbus_TCP_Open()；创建TCP连接
         * 返回连接信息
         * 
         * */
        public String Modbus_TCP_Open()
        {
            int Timeout = 500;
            //创建一个套接字   
            //将服务器IP地址存放在字符串
            //将端口号强制为32位整型，存放在port中
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(IP_Address), Convert.ToInt32(PC_Port));
            newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            CSD d = ConnectSocket;
            IAsyncResult iaresult = d.BeginInvoke(ie, newclient, null, null);
            bool connectSuccess = iaresult.AsyncWaitHandle.WaitOne(Timeout, false);
            /**
            //将套接字与远程服务器地址相连  
            try
            {
                newclient.Connect(ie);
            }
            catch (SocketException e)
            {
                MessageBox.Show("连接服务器失败  " + e.Message);
                return;
            }
            **/
            if (!connectSuccess)
            {
                MessageBox.Show(string.Format("失败！错误信息：{0}", "连接超时"));
                return string.Format("失败！错误信息：{0}", "连接超时");
            }
            string m = d.EndInvoke(iaresult);
            if (!string.IsNullOrEmpty(m))
            {
                MessageBox.Show(string.Format("失败！错误信息：{0}", m));
                return string.Format("失败！错误信息：{0}", m);
            }
            return "连接成功！";
        }

        /**
         * 
         * Modbus_TCP_Close();关闭TCP连接；
         * 
         * */
        public void Modbus_TCP_Close()
        {
            newclient.Close();
        }

        /**
         * 
         * Modbus-RTU通讯协议 0x01指令 读取多个线圈值
         *id：设备ID；address：线圈起始地址；number：线圈数量
         * 
        * */
        public bool[] Modbus_Read_Coils_Value(int id, int address, int number)
        {
            int n = 0;
            bool[] coils = new bool[number];
            if (number % 8 > 0)
            {
                n = number / 8 + 1;
            }
            else
            {
                n = number / 8;
            }
            byte[] data = new byte[n + 9];//定义数据接收数组  
            byte[] receive = new byte[n];
            byte[] instruction = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, Convert.ToByte(id), 0x01, Convert.ToByte(address / 256),
                Convert.ToByte(address % 256), Convert.ToByte(number / 256), Convert.ToByte(number % 256) };
            newclient.Send(instruction);
            newclient.Receive(data);
            for (int i = 0; i < n; i++)
            {
                receive[i] = data[i + 9];
                int data_receive = receive[i];
                for (int y = 0; ((i * 8 + y) < number) && (y < 8); y++)
                {
                    if ((data_receive & 0x01) == 1)
                    {
                        coils[i * 8 + y] = true;
                    }
                    else
                    {
                        coils[i * 8 + y] = false;
                    }
                    data_receive = (data_receive >> 1);
                }
            }
            return coils;
        }

        /**
        * 
        * Modbus-RTU通讯协议 0x03指令 读取多个保持寄存器
        * id：设备ID；address：保持寄存器起始地址；number：线圈数量
        * 
        * */
        public int[] Modbus_Read_Holding_Register(int id, int address, int number)
        {
            byte[] data = new byte[number * 2 + 9];//定义数据接收数组  
            int[] word = new int[number];
            byte[] instruction = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, Convert.ToByte(id), 0x03, Convert.ToByte(address / 256),
                Convert.ToByte(address % 256), Convert.ToByte(number / 256), Convert.ToByte(number % 256) };
            newclient.Send(instruction);
            newclient.Receive(data);
            for (int i = 0; i < number; i++)
            {
                word[i] = data[9 + 2 * i] * 256 + data[9 + 2 * i + 1];
            }
            return word;
        }

        /**
        * 
        * Modbus-RTU通讯协议 0x05指令 写单个线圈
        * s:TCP句柄；id：设备ID；address：线圈地址；value：线圈值
        * 
        * */
        public bool Modbus_Write_Single_Coil(int id, int address, bool value)
        {
            byte[] data = new byte[12];
            byte coil_value;
            bool coil_return_value;
            if (value)
            {
                coil_value = 0xFF;
            }
            else
            {
                coil_value = 0x00;
            }
            byte[] instruction = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, Convert.ToByte(id), 0x05, Convert.ToByte(address / 256),
                Convert.ToByte(address % 256), coil_value, 0x00};
            newclient.Send(instruction);
            newclient.Receive(data);
            if (data[10] == 0xFF)
            {
                coil_return_value = true;
            }
            else
            {
                coil_return_value = false;
            }
            return coil_return_value;
        }

        /**
        * 
        * Modbus-RTU通讯协议 0x06指令 写单个保持寄存器
        * s:TCP句柄；id：设备ID；address：保持寄存器地址；value：值
        * 
        * */
        public int Modbus_Write_Single_Holding_Register(int id, int address, int value)
        {
            int single_word;
            byte[] data = new byte[12];
            byte[] instruction = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, Convert.ToByte(id), 0x06, Convert.ToByte(address / 256),
                Convert.ToByte(address % 256), Convert.ToByte(value / 256), Convert.ToByte(value % 256) };
            newclient.Send(instruction);
            newclient.Receive(data);
            single_word = data[10] * 256 + data[11];
            return single_word;
        }

        /**
        * 
        * Modbus-RTU通讯协议 0x0F指令 写多个线圈
        * s:TCP句柄；id：设备ID；address：线圈起始地址；value：线圈值
        * 
        * */
        public byte[] Modbus_Write_Coils_Value(int id, int address, bool[] value)
        {
            int n;
            byte[] data = new byte[12];
            if (value.Length % 8 > 0)
            {
                n = value.Length / 8 + 1;
            }
            else
            {
                n = value.Length / 8;
            }
            int[] input_value = new int[n];
            for (int i = 0; i < n; i++)
            {
                for (int y = 0; (y < 8) && ((i * 8 + y) < value.Length); y++)
                {
                    if (value[i * 8 + y])
                    {
                        input_value[i] += (1 << y);
                    }
                    else
                    {
                        input_value[i] += (0 << y);
                    }
                }
            }
            byte[] instruction = new byte[13 + n];
            byte[] normol_instruction = { 0x00, 0x01, 0x00, 0x00, 0x00, Convert.ToByte(7 + n), Convert.ToByte(id), 0x0F, Convert.ToByte(address / 256),
                Convert.ToByte(address % 256), Convert.ToByte(value.Length / 256), Convert.ToByte(value.Length % 256), Convert.ToByte(n)};
            for (int i = 0; i < normol_instruction.Length; i++)
            {
                instruction[i] = normol_instruction[i];
            }
            for (int i = n; i > 0; i--)
            {
                instruction[13 + n - i] = (Convert.ToByte(input_value[i - 1]));
            }
            newclient.Send(instruction);
            newclient.Receive(data);
            return data;
        }

        /**
        * 
        * Modbus-RTU通讯协议 0x10指令 写多个保持寄存器
        * s:TCP句柄；id：设备ID；address：保持寄存器起始地址；value_bitarray：寄存器值数组
        * 
        * */
        public byte[] Modbus_Write_Holding_Registers(int id, int address, int[] value)
        {
            byte[] data = new byte[12];
            byte[] instruction = new byte[13 + value.Length * 2];
            byte[] normol_instruction = { 0x00, 0x01, 0x00, 0x00, 0x00, Convert.ToByte(7 + value.Length * 2), Convert.ToByte(id), 0x10, Convert.ToByte(address / 256),
                Convert.ToByte(address % 256), Convert.ToByte(value.Length / 256), Convert.ToByte(value.Length % 256), Convert.ToByte(value.Length*2)};
            for (int i = 0; i < normol_instruction.Length; i++)
            {
                instruction[i] = normol_instruction[i];
            }
            for (int i = 0; i < value.Length; i++)
            {
                instruction[13 + i * 2] = (Convert.ToByte(value[i] / 256));
                instruction[13 + i * 2 + 1] = (Convert.ToByte(value[i] % 256));
            }
            newclient.Send(instruction);
            newclient.Receive(data);
            return data;
        }

    }
}
