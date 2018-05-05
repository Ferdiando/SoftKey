using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Pub
{
    class Register
    {
        private static Microsoft.Win32.RegistryKey regLocalMachine = Microsoft.Win32.Registry.LocalMachine;

        private static String m_strErrMessage = "";

        public static String GetErrorMessage()
        {
            return m_strErrMessage;
        }

        /// <summary>
        /// 不仅获得要找到的注册表项，而且如果路径正确，会创建相应的注册表路径
        /// </summary>
        /// <param name="path">注册表路经</param>
        /// <returns>返回注册表对象</returns>
        public static bool CreateItemRegEdit(string path)
        {
            try
            {
                //Microsoft.Win32.RegistryKey obj = Microsoft.Win32.Registry.LocalMachine;

                regLocalMachine.CreateSubKey(path);

                return true;
            }
            catch (Exception e)
            {
                m_strErrMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 设置注册表项下面的值，默认设置为字符串类型
        /// </summary>
        /// <param name="path">路经</param>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public static int SetValueRegEdit(string path, string name, string value)
        {
            try
            {
                //Microsoft.Win32.RegistryKey obj = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey objItem = regLocalMachine.OpenSubKey(path, true);
                objItem.SetValue(name, value, Microsoft.Win32.RegistryValueKind.String);
                //objItem.SetValue(name, value, Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception e)
            {
                m_strErrMessage = e.Message;
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 查看注册表指定项的值，默认返回字符串
        /// </summary>
        /// <param name="path">路经</param>
        /// <param name="name">项名称</param>
        /// <returns>项值</returns>
        public static string getValueRegEdit(string path, string name)
        {
            string value;
            try
            {
                //Microsoft.Win32.RegistryKey obj = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey objItem = regLocalMachine.OpenSubKey(path);
                value = objItem.GetValue(name, "", Microsoft.Win32.RegistryValueOptions.None).ToString();
            }
            catch (Exception e)
            {
                m_strErrMessage = e.Message;
                return "";
            }
            return value;
        }


        /// <summary>
        /// 查看注册表项下是否存在其他路径
        /// </summary>
        /// <param name="value">路经</param>
        /// <param name="name">项名称</param>
        /// <returns>是否存在</returns>
        public static bool SearchItemRegEdit(string path, string name)
        {
            string[] subkeyNames;
            //Microsoft.Win32.RegistryKey hkml = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey software = regLocalMachine.OpenSubKey(path);
            subkeyNames = software.GetSubKeyNames();
            //取得该项下所有子项的名称的序列，并传递给预定的数组中   
            foreach (string keyName in subkeyNames)   //遍历整个数组   
            {
                if (keyName.ToUpper() == name.ToUpper()) //判断子项的名称   
                {
                    regLocalMachine.Close();
                    return true;
                }
            }
            regLocalMachine.Close();
            return false;
        }

        /// <summary>
        /// 查看注册表的值是否存在
        /// </summary>
        /// <param name="value">路经</param>
        /// <param name="value">查看的值</param>
        /// <returns>是否成功</returns>
        public static bool SearchValueRegEdit(string path, string value)
        {
            string[] subkeyNames;
            //Microsoft.Win32.RegistryKey hkml = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey software = regLocalMachine.OpenSubKey(path);

            subkeyNames = software.GetValueNames();
            //取得该项下所有键值的名称的序列，并传递给预定的数组中   
            foreach (string keyName in subkeyNames)
            {
                if (keyName.ToUpper() == value.ToUpper())   //判断键值的名称   
                {
                    regLocalMachine.Close();
                    return true;
                }
            }
            regLocalMachine.Close();
            return false;
        }
    }
}
