using System;
using System.Security.Cryptography;
using System.Text;

namespace HelloGithub.Libs
{
    public class HashHelper
    {
        public static string GetMd5Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public static bool CompareMd5Hash(string input, string input2)
        {
            return GetMd5Hash(input).Equals(GetMd5Hash(input2), StringComparison.OrdinalIgnoreCase);
        }
    }
}
