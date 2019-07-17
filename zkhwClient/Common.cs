using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient
{
    public class PersonExport
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string RiQi { get; set; }
        public string ZhuangTai { get; set; }
        public string Memo { get; set; }
    }
    public class FileTimeInfo
    {
        public string FileName;  //文件名
        public DateTime FileCreateTime; //创建时间
    }
    public class Common
    {
        public static void txtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                TextBox b = (TextBox)sender;
                int kc = (int)e.KeyChar;
                if ((kc < 48 || kc > 57) && kc != 8 && kc != 46)
                    e.Handled = true;
                if (kc == 46)                       //小数点
                {
                    if (b.Text.Length <= 0)
                        e.Handled = true;           //小数点不能在第一位
                    else
                    {
                        float f;
                        float oldf;
                        bool b1 = false, b2 = false;
                        b1 = float.TryParse(b.Text, out oldf);
                        b2 = float.TryParse(b.Text + e.KeyChar.ToString(), out f);
                        if (b2 == false)
                        {
                            if (b1 == true)
                                e.Handled = true;
                            else
                                e.Handled = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        /// <summary>
        /// 判断是不是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public static string GetCreateTime(string s)
        {
            if(s=="" || s==null)
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                return DateTime.Parse(s).ToString("yyyy-MM-dd");
            }
        }
        public static List<FileTimeInfo> GetLatestFileTimeInfo(string dir, string filename)
        {
            List<FileTimeInfo> list = new List<FileTimeInfo>();
            DirectoryInfo d = new DirectoryInfo(dir);
            DateTime dt = DateTime.Parse("2001-1-1"); 
            foreach (FileInfo fi in d.GetFiles())
            {
                string fname=Path.GetFileNameWithoutExtension(fi.FullName);
                string[] a = fname.Split('-');
                string _namefile = "";
                bool isret = false;
                if(a.Length<=1)
                {
                    _namefile = fname;
                    if(fname.IndexOf(filename) > -1)
                    {
                        isret = true;
                    }
                }
                else
                {
                    _namefile = a[1].ToString();
                    if(_namefile==filename)
                    {
                        isret = true;
                    }
                }
                if (isret==true)
                {
                    if(fi.LastAccessTime>dt)
                    {
                        list.Clear();
                        dt = fi.LastAccessTime;
                        list.Add(new FileTimeInfo()
                        {
                            FileName = fi.FullName,
                            FileCreateTime = fi.LastAccessTime
                        });
                    } 
                } 
            }
            return list; 
        }
    }
}
