using System;
using System.Security.Cryptography;
using System.Text;

namespace EChat.CoreLayer.Utilities.Security
{
    public static class EncodeMd5
    {
        public static string EncodePasswordMd5(this string text) //Encrypt using MD5   
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)   
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(text);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string   
            return BitConverter.ToString(encodedBytes);
        }
    }
}