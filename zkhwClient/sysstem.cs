using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks; 

namespace zkhwClient
{
    public class sysstem
    {
        public static string skey = "/.$%@";
        public static string defaultDt = "2000-1-1 1:1:1";

        #region 声明读写INI文件的API函数 
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);
        #endregion
        private static string Encrypt(string content, string secretKey)
        {
            string str = ProcessHandler(content, secretKey);
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            return  Convert.ToBase64String(inputByteArray.ToArray());
        }
        private static  string ProcessHandler(string content, string secretKey)
        {
            char[] data = content.ToCharArray();
            char[] key = secretKey.ToCharArray();
            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= key[i % key.Length];
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i]);
            }
            return builder.ToString();
        }

        private static string Decrypt(string content, string secretKey)
        {
            try
            {
                byte[] a = Convert.FromBase64String(content);
                string b = Encoding.UTF8.GetString(a);
                string str = ProcessHandler(b, secretKey);
                return str;
            }
            catch
            {
                return "";
            }
        }

        private static string IniReadValue(string section, string key,string path)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, path);
            return temp.ToString();
        }

        private  static void IniWriteValue(string section, string key, string iValue, string path)
        {
            WritePrivateProfileString(section, key, iValue, path);
        }
        
        public  static bool IsDate(string a,out DateTime b)
        {
            b = DateTime.Parse(defaultDt);
            bool flag = false;
            if(a.Length >=14)
            {
                string a0 = a.Substring(0, 4) + "-" + a.Substring(4, 2) + "-" + a.Substring(6, 2) + " " + a.Substring(8, 2) + ":" + a.Substring(10, 2) + ":" + a.Substring(12, 2);
                flag = DateTime.TryParse(a0, out b);
            }
            return flag;
        }
        /// <summary>
        /// 入口 
        /// </summary>
        /// <param name="fpath">文件对应的路径</param>
        /// <returns>
        /// 0：正常
        /// -1：文件不存在
        /// 1：到期了
        /// 2：日期出错
        /// 3：用户改动系统时间
        /// </returns>
        public static int JudgeExpirationDate(string fpath)
        {
            int iResult = -1;
            if(File.Exists(fpath))   //判断文件是否存在
            {
                string a0 = IniReadValue("system", "a", fpath);
                string b0 = IniReadValue("system", "b", fpath);
                string a = Decrypt(a0, skey);
                string b = Decrypt(b0, skey);
                string c = b.Substring(4);
                if(a== "20000101010101")
                { 
                    //第一次用软件默认自动给添加一个月
                    string s0 = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string e0= "zkhw"+DateTime.Now.AddMonths(1).ToString("yyyyMMdd")+"235959";
                    string s = Encrypt(s0,skey);
                    string e = Encrypt(e0, skey);
                    IniWriteValue("system", "a", s, fpath);
                    IniWriteValue("system", "b", e, fpath);
                    iResult = 0; 
                }
                else
                {
                    DateTime s0=DateTime.Parse(defaultDt);
                    DateTime e0 = DateTime.Parse(defaultDt);
                    bool t = IsDate(a, out s0);
                    bool t1= IsDate(c, out e0);
                    if (t == false || t1 == false)
                    {
                        iResult = 2;
                    }
                    else
                    { 
                        if (DateTime.Now < s0)
                        {
                            iResult = 3;
                        }
                        else
                        {
                            if(DateTime.Now>e0)  
                            {
                                iResult = 1;
                            }
                            else
                            {
                                if (s0 >= e0)
                                {
                                    iResult = 1;
                                }
                                else
                                {
                                    iResult = 0;
                                }
                            } 
                        } 
                    } 
                }
            } 
            return iResult;
        }

        public static void UpdateInfo(string fpath)
        { 
            string s0 = DateTime.Now.ToString("yyyyMMddHHmmss");
            string s = Encrypt(s0, skey);
            IniWriteValue("system", "a", s, fpath); 
        } 

        public static bool EmpowerInfo(string fpath,string s)
        {
            bool flag = false;
            try
            {
                IniWriteValue("system", "b", s, fpath);
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static string GetINIInfo(string fpath,int iType)
        {
            string str = "";
            if(iType==0)
            {
                string a0 = IniReadValue("system", "a", fpath);
                str = Decrypt(a0, skey);
            }
            else
            {
                string b0 = IniReadValue("system", "b", fpath);
                str = Decrypt(b0, skey);
            }
            return str;
        }

        public static bool EmpowerValid(string a)
        {
            bool flag = false;
            string str = Decrypt(a, skey);
            if(str.Length>=18)
            {
                if(str.Substring(0,4)=="zkhw")
                {
                    DateTime s0 = DateTime.Parse(defaultDt);
                    bool b = IsDate(str.Substring(4), out s0);
                    if(b==true)
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }
    }
}
