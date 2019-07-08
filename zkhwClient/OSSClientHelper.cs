using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS;
using System.IO;
using Aliyun.OSS.Common;

namespace zkhwClient
{
    public class OSSClientHelper
    {
        const string accessKeyId = "LTAIo8TBkgNzm4jk";
        const string accessKeySecret = "iiiHjeaJUhg2ZCNNlm4OA47YRdbbr9";
        const string endpoint = "http://oss-cn-beijing.aliyuncs.com"; 
        public static OssClient GetClient()
        {
            return new OssClient(endpoint, accessKeyId, accessKeySecret);
        }
        public  static bool CreateBucket(string bucketName)
        {
            bool flag = false;
            try
            {
                var client = GetClient();
                var bucket = client.CreateBucket(bucketName);
                flag = true;
            }
            catch(Exception ex)
            {

            }
            return flag;
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="filebyte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool PushImg(byte[] filebyte, string fileName,string bucketName)
        {
            try
            {
                var client = GetClient(); 
                MemoryStream stream = new MemoryStream(filebyte, 0, filebyte.Length);
                return client.PutObject(bucketName, fileName, stream).HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            { }
            return false;
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public static bool DownImg(string bucketName,string key, string downdir)
        {
            bool flag = false; 
            try
            {
                var client = GetClient();  
                var result = client.GetObject(bucketName, key);
                using (var requestStream = result.Content)
                {
                    using (var fs = File.Open(downdir, FileMode.OpenOrCreate))
                    {
                        int length = 4 * 1024;
                        var buf = new byte[length];
                        do
                        {
                            length = requestStream.Read(buf, 0, length);
                            fs.Write(buf, 0, length);
                        } while (length != 0);
                    }
                }
                flag = true;
            }
            catch (OssException ex)
            {
                //Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                //    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
            return flag;
        }

        public static bool DeleteImg(string bucketName,string key)
        {
            bool flag = false;
            try
            {
                var client = GetClient();
                client.DeleteObject(bucketName, key);
                flag = true;
            }
            catch (Exception)
            { }
            return flag;
        }
    }
}
