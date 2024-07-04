using Common.Toolkit.Helper;
using System.Text;

namespace Common.Toolkit.Test
{
    [TestClass]
    public class AESTest
    {
        string testString = "hello";
        string secretKey = "M^1Sl71YK0Y8Kw7Wfa$#bAjKH5uB4vWf";

        [TestMethod]
        public void Encrypt()
        {
            //testString
            var result = AESHelper.AESEncrypt(testString, secretKey);
            Console.WriteLine(result);

        }

        [TestMethod]
        public void Decrypt()
        {
            var secretStr = AESHelper.AESEncrypt(testString, secretKey);

            var originText = AESHelper.AESDecrypt(secretStr, secretKey);
            Console.WriteLine(originText);
        }

        [TestMethod]
        public void Base64()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 8192; i++)
            {
                stringBuilder.Append("hello");
            }

            var utf8 = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            var result = Convert.ToBase64String(utf8);
            Console.WriteLine(result);
        }
    }
}

