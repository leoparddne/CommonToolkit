using System.Security.Cryptography;
using System.Text;

namespace Common.Toolkit.Helper
{
    public class FileHelper
    {
        #region FileExist
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool FileExist(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region GetMD5
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
        #endregion

        #region DirectoryExists
        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns></returns>
        public static bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region CreateDirectory
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }
        #endregion

        #region CreateIfNotExists
        /// <summary>
        /// 如果目录不存在则创建目录
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void CreateIfNotExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion
    }
}
