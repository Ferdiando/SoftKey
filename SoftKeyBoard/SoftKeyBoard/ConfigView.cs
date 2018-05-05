using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SoftKeyBoard
{
    class ConfigView
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static String CollocateFilePath = "";
        public static Dictionary<String, int> dCollocate = new Dictionary<string,int>();
        public static int sWidth = 0;
        public static int sHeight = 0;
        public static Point pLocation = new Point(0,0);
        public static Size sForm = new Size(1920,1080);
        static String strRegPath = Pub.DistinguishSystemBit.Distinguish64or32System() ? "SOFTWARE\\WonderTac" : "SOFTWARE\\Wow6432Node\\WonderTac"; 

        //读取注册表
        public static void setConfigView() 
        {
            try
            {
                CollocateFilePath = Pub.Register.getValueRegEdit(strRegPath, "CollocateFilePath");
                if (CollocateFilePath == "")
                {
                    CollocateFilePath = "D:\\BOBRVT\\Version3.3\\Terminal\\CollocateFile.ini";
                }
                log.Info("ReadRegeditConfig Success,CollocateFilePath:" + CollocateFilePath);
            }
            catch(Exception ex)
            {
                log.Error("ReadRegeditConfig Error：" + ex.ToString());
            }
        }

    }
}
