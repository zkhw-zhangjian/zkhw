using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
