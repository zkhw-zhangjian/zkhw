using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace zkhwClient.utils
{
    class httpJk
    {
        public static string getDeviceData(string idnumber,string orgcode,string userid)
        {
            string serviceAddress = "http://47.92.193.245:8080/zkhw/client/getIsPoor?idNumber=" + idnumber + "&orgCode=" + orgcode + "&userId=" + userid;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            request.Method = "GET";
            request.ContentType = "application/json;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}
