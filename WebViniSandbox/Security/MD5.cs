using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace WebViniSandbox.Security
{
    public static class MD5Helper
    {
        private static string hashToString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string StringToMD5(string data)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(data);
            return ByteToMD5(bytes);
        }

        public static string ByteToMD5(byte[] data)
        {
            MD5 m = MD5.Create();
            byte[] rm = m.ComputeHash(data);
            return hashToString(rm);
        }
    }
}