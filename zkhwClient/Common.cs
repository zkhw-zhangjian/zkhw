using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace zkhwClient
{
    public class FileTimeInfo
    {
        public string FileName;  //文件名
        public DateTime FileCreateTime; //创建时间
    }
    public class Common
    {
        public static List<FileTimeInfo> GetLatestFileTimeInfo(string dir, string filename)
        {
            List<FileTimeInfo> list = new List<FileTimeInfo>();
            DirectoryInfo d = new DirectoryInfo(dir);
            DateTime dt = DateTime.Parse("2001-1-1"); 
            foreach (FileInfo fi in d.GetFiles())
            {
                string fname=Path.GetFileNameWithoutExtension(fi.FullName);
                string[] a = fname.Split('-');
                if (a[1].ToString() == filename)
                {
                    if(fi.CreationTime>dt)
                    {
                        list.Clear();
                        dt = fi.CreationTime;
                        list.Add(new FileTimeInfo()
                        {
                            FileName = fi.FullName,
                            FileCreateTime = fi.CreationTime
                        });
                    } 
                } 
            }
            return list; 
        }
    }
}
