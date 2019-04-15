using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace zkhwClient.utils
{
    class Md5
    {
        private static MD5 md5 = MD5.Create();         
        /**/
        /// <summary>
        /// 将字符串加密
        /// </summary>
        /// <param name="sourceString">需要加密的字符串</param>
        /// <returns>MD5加密后字符串</returns>
        public static string HashString(string sourceString)
        {
            return HashString("gb2312", sourceString);
        }
        /**/
        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="codeName">编码类型</param>
        /// <param name="sourceString">需要加密的字符串</param>
        /// <returns>MD5加密后字符串</returns>
        public static string HashString(string codeName, string sourceString)
        {
            byte[] source = md5.ComputeHash(Encoding.GetEncoding(codeName).GetBytes(sourceString));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
