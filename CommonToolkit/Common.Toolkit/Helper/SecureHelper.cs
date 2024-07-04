using System.Security.Cryptography;
using System.Text;

namespace Common.Toolkit.Helper
{
    public class SecureHelper
    {
        #region MD5
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static string MD5EncryptFor16Bit(string inputValue)
        {
            MD5 md5 = MD5.Create();

            string encryptionValue = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(inputValue)), 4, 8);
            encryptionValue = encryptionValue.Replace("-", string.Empty);

            return encryptionValue;
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static string MD5EncryptFor32Bit(string inputValue)
        {
            string encryptionValue = string.Empty;
            MD5 md5 = MD5.Create();

            byte[] encryptionArray = md5.ComputeHash(Encoding.UTF8.GetBytes(inputValue));

            foreach (byte item in encryptionArray)
            {
                encryptionValue = string.Concat(encryptionValue, item.ToString("X2"));
            }

            return encryptionValue;
        }

        /// <summary>
        /// 64位MD5加密
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static string MD5EncryptFor64Bit(string inputValue)
        {
            MD5 md5 = MD5.Create();

            byte[] encryptionArray = md5.ComputeHash(Encoding.UTF8.GetBytes(inputValue));

            return Convert.ToBase64String(encryptionArray);
        }
        #endregion
    }
}
