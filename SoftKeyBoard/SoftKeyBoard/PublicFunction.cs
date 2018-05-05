using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace Pub
{
    class PublicFunction
    {
        [DllImport("kernel32", EntryPoint = "WinExec")]
        public static extern int WinExec(string lpCmdLine, int nCmdShow);

        public const int WM_COPYDATA = 0x004A;

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, int flags);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        //[DllImport("kernel32", EntryPoint = "GetTickCount")]
        //public static extern uint GetTickCount(); 

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, // handle to destination window
                                              int Msg, // message
                                              int wParam, // first message parameter
                                              ref COPYDATASTRUCT lParam // second message parameter
                                              );

        public static Boolean SetWindowPosTopMost(IntPtr hWnd, int X, int Y, int cx, int cy)
        {
            Boolean bRet = SetWindowPos(hWnd, new IntPtr(-1), X, Y, cx, cy, 0);
            //log.Info("SetWindowPosTopMost,bRet=" + bRet);
            return bRet;
        }

        public static void KillProcess(string processName)
        {
            //log.Info("KillProcess,processName=" + processName);
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程
            try
            {
                foreach (Process thisproc in Process.GetProcessesByName(processName)) //循环查找
                {
                    if (!thisproc.CloseMainWindow())
                    {
                        //log.Info("KillProcess,终止进程" + processName);
                        thisproc.Kill();
                    }
                }
                Thread.Sleep(100);//20160704   新版软电话
                bool bFlag = Pub.PublicFunction.SearchProc(processName);
                if (bFlag)
                    Pub.PublicFunction.KillProcess(processName);
            }
            catch (Exception ex)
            {
                //log.Info("终止进程" + processName + "出现异常，异常信息:" + ex.Message);
            }
        }

        /// <summary>  
        /// 判断是否存在进程  精确  
        /// </summary>  
        /// <param name="strProcName">精确进程名</param>  
        /// <returns>是否包含</returns>  
        public static bool SearchProc(string strProcName)
        {
            try
            {
                //精确进程名  用GetProcessesByName  
                Process[] ps = Process.GetProcessesByName(strProcName);
                if (ps.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void FindFileAndDelete(string FoldPath, string filter)//要查找的文件夹和文件类型
        {
            try
            {
                DirectoryInfo thefolder = new DirectoryInfo(FoldPath);

                //目前只遍历一个层级
                //foreach (DirectoryInfo nextfolder in thefolder.GetDirectories())
                //{
                //    FindFileAndDelete(nextfolder.FullName, filter);
                //}

                foreach (FileInfo nextfile in thefolder.GetFiles(filter))
                {
                    String strFile = nextfile.FullName;
                    System.Console.WriteLine(strFile);
                    System.IO.File.Delete(nextfile.FullName);
                    //log.Info("删除文件" + nextfile.FullName);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

        }

        public static long lPrintType = 1;  //0为默认为打印合约类型，此种类型打印不给打印提示

        public static Boolean bExitFlag = false;
        //退出程序

        //判断是否全为数字
        public static bool IsNum(String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] < '0') || (str[i] > '9')) //&& (str[i] != '-') && (str[i] != '.') && (str[i] != '%'))
                    return false;
            }
            return true;
        }

        //登录ID校验
        public static String VerifyMobileBankLoginID(String strLoginID, out long lVerifyResult)
        {
            lVerifyResult = 0;
            if (strLoginID.Length == 0)
            {
                lVerifyResult = 1;
                return "07:② 登录ID为空";
            }
            else if ((strLoginID.Length < 3) || (strLoginID.Length > 12))
            {
                lVerifyResult = 1;
                return "07:② 登录ID的长度不为3-12位，不符合长度要求";
            }
            return "② 登录ID校验合法有效";
        }

        //校验手机号码的有效性
        public static String VerifyMobileNumber(String strMobileNumber, out long lVerifyResult)
        {
            //首先判断长度
            if (strMobileNumber.Length != 11)
            {
                lVerifyResult = 1;
                return "01:① 用户输入的手机号长度为" + strMobileNumber.Length + ",不为11位，无效手机号码";
            }

            if (!IsNum(strMobileNumber))
            {
                lVerifyResult = 2;
                return "02:① 用户的手机号码" + strMobileNumber + "中含有非数字字符,无效手机号码";
            }

            lVerifyResult = 0;
            return "① 用户输入的手机号码正确有效";
        }

        public static String ParseNumericColumn(String strSource, Boolean bIfNeedPercent, out long lParseResult)
        {
            try
            {
                strSource = strSource.Trim();
                if ((strSource.Length == 0) || strSource == "-")//字段长度为0，置空
                {
                    strSource = "NULL";
                    lParseResult = 0;
                    return strSource;
                }
                else if (!PublicFunction.IsNum(strSource))//含有非数字字符
                {
                    if ((strSource == "NA") || strSource == "-")
                    {
                        strSource = "NULL";
                        lParseResult = 0;
                        return strSource;
                    }
                    else
                    {
                        lParseResult = -1;
                        return "字段栏位中中含有排除外的非数字字符，不能进行导入";
                    }
                }
                else if (bIfNeedPercent && !strSource.Contains("%"))//此列如果需要百分号，但是没有
                {
                    lParseResult = -2;
                    return "必须含有百分号，请检查数据后重新导入";
                }
                else if (!bIfNeedPercent && strSource.Contains("%"))//此列不能添加百分号，但是有了
                {
                    lParseResult = -3;
                    return "不能含有百分号，请检查数据后重新导入";
                }
                else if (strSource.Contains("%"))//含有百分号，去掉百分号除以100
                {
                    //注意精度问题
                    strSource = strSource.Trim('%');
                    if (strSource.Length > 0)
                    {
                        //strSource = (Convert.ToDecimal(strSource)).ToString();
                        strSource = (Convert.ToDecimal(strSource) / 100).ToString("f4");
                    }
                    else
                        strSource = "NULL";
                }
            }
            catch (Exception ex)
            {
                lParseResult = -1;
                return "字段栏位中中含有排除外的非数字字符，不能进行导入，" + ex.Message;
            }
            lParseResult = 0;
            return strSource;
        }
        public static Boolean CheckMouseInRectangle(MouseEventArgs e, Rectangle rect)
        {
            Boolean bRlt = false;
            try
            {
                Boolean bIsXIn = e.Location.X <= rect.X + rect.Width &&
                    e.Location.X >= rect.X;
                Boolean bIsYIn = e.Location.Y <= rect.Y + rect.Height &&
                    e.Location.Y >= rect.Y;
                bRlt = bIsYIn && bIsXIn;

            }
            catch (System.Exception ex)
            {

            }

            return bRlt;
        }
    }
}
