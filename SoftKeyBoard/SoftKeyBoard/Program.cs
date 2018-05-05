using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Drawing;

namespace SoftKeyBoard
{
    static class Program
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void ReadIniFile()
        {
            try
            {
                log.Info("读取配置文件ReadIniFile()......");
                String line;//读文件
                FileStream fs = new FileStream(ConfigView.CollocateFilePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(ConfigView.CollocateFilePath, Encoding.Default);
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.IndexOf("FormWigth") >= 0)
                        ConfigView.sWidth = Convert.ToInt32(line.Substring("FormWigth=".Length));
                    else if (line.IndexOf("FormHeigth") >= 0)
                        ConfigView.sHeight = Convert.ToInt32(line.Substring("FormHeigth=".Length));
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception err)
            {
                log.Error(err.ToString());
            }
        }
        public static float ConvertFloat(float i)
        {
            float iy = (float)Math.Round((i * ConfigView.sWidth / 1920.0), 0);
            return iy;
        }
        public static int ConvertInt(int i)
        {
            int iy = (int)Math.Round((i * ConfigView.sWidth / 1.5 / 1920.0), 0);
            return iy;
        }
        public static Size ConVertSize(Size rc)
        {
            Size rNew = new Size(ConvertInt(rc.Width), ConvertInt(rc.Height));
            return rNew;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                log.Info("SoftKeyBoard Main() Start...");
                ConfigView.setConfigView();
                ReadIniFile();   //读取路径以及相关信息
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                log.Error("SoftKeyBoard Main() Exception:" + ex.ToString());
            }
        }
    }
}
