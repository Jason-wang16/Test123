using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Standard_UI.RecordsWrite
{
    class CsvWrite
    {
        public static void Write(String[] StrArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            DateTime time = DateTime.Now;
            //string sData = time.ToString("yyyy-MM-dd");
            string path = AppDomain.CurrentDomain.BaseDirectory + @".//CSV/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }


            string fileFullPath = path + time.ToString("yyyy -MM-dd") + ".System.csv";

            bool flag = false;
            if (!File.Exists(fileFullPath))
            {
                flag = true;
            }

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(fileFullPath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();
            StringBuilder DstTxtString0 = new StringBuilder();
            if (flag)
            {
                flag = false;
                DstTxtString0.Append("Time");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AC-Gap_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AD-Gap_In");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AC-Gap_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("AD-Gap_Out");
                DstTxtString0.Append(",");
                DstTxtString0.Append("InCount");
                DstTxtString0.Append(",");
                DstTxtString0.Append("OutCount");
                DstTxtString0.Append(",");
                DstTxtString0.Append("Result");
                DstTxtString0.Append(",");
                CsvTxtWriter.WriteLine(DstTxtString0);
            }
            //拼接字符串
            for (int i = 0; i < StrArray.Length; i++)
            {
                DstTxtString.Append(StrArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Byte[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Int16[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Int32[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Int64[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Single[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

        public static void Write(String CsvFilePath, Double[] ValueArray, Boolean IsAppend = true)
        {
            FileStream CsvFileStream;   //CSV文件流
            StreamWriter CsvTxtWriter;  //CSV TXT文件操作类

            //打开文件
            try
            {
                //创建文件流对象，无则创建,有则追加
                CsvFileStream = new FileStream(CsvFilePath, IsAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);

                //创建文件流写入对象，绑定文件流对象
                CsvTxtWriter = new StreamWriter(CsvFileStream);
            }
            catch (Exception)
            {
                throw;
            }

            //创建拼接字符串类
            StringBuilder DstTxtString = new StringBuilder();

            //拼接字符串
            for (int i = 0; i < ValueArray.Length; i++)
            {
                DstTxtString.Append(ValueArray[i]);
                DstTxtString.Append(",");
            }

            //写入文件
            CsvTxtWriter.WriteLine(DstTxtString);

            //关闭文件
            CsvTxtWriter.Close();

        }

    }

}
