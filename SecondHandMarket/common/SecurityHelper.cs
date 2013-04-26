using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SecondHandMarket.common
{
    public class SecurityHelper
    {
        //4、TripleDES加密、解密  
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TripleDESCrypto(string str, string key = "secondhandmarket")
        {
            byte[] data = Encoding.Unicode.GetBytes(str);//如果加密中文，不能用ASCII码  
            byte[] keys = Encoding.ASCII.GetBytes(key);

            var des = new TripleDESCryptoServiceProvider();
            des.Key = keys;//key的长度必须为16位或24位，否则报错“指定键的大小对于此算法无效。”，des.Key不支持中文  
            des.Mode = CipherMode.ECB;//设置运算模式  
            ICryptoTransform cryp = des.CreateEncryptor();//加密  

            return HttpContext.Current.Server.UrlEncode(Convert.ToBase64String(cryp.TransformFinalBlock(data, 0, data.Length)));
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string TripleDESCryptoDe(string str, string key = "secondhandmarket")
        {
            byte[] data = Convert.FromBase64String(str);
            byte[] keys = Encoding.ASCII.GetBytes(key);

            var des = new TripleDESCryptoServiceProvider();
            des.Key = keys;
            des.Mode = CipherMode.ECB;//设置运算模式  
            des.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryp = des.CreateDecryptor();//解密  

            return Encoding.Unicode.GetString(cryp.TransformFinalBlock(data, 0, data.Length));
        }  
    }
}