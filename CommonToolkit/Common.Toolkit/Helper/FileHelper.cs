using System.Security.Cryptography;
using System.Text;

namespace Common.Toolkit.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 获取文件MD5
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMD5(string path)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string md5String;
            FileStream fs1 = File.Open(path, FileMode.Open);
            byte[] retVal = md5.ComputeHash(fs1);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++) sb.Append(retVal[i].ToString("x2"));
            md5String = sb.ToString().ToUpper();
            fs1.Close();
            fs1.Dispose();
            return md5String;
        }


        /// <summary>
        /// 如果目录不存在则创建目录
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void CreateIfNotExists(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                return;
            }
            Directory.CreateDirectory(directoryPath);
        }
    }
}
