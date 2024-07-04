using Common.Toolkit.Helper;

namespace Common.Toolkit.Test
{
    [TestClass]
    public class GZipTest
    {
        string testString = "hello";

        public GZipTest()
        {
            testString = File.ReadAllText(@"C:\Users\ivesBao\Desktop\data.txt");
        }

        [TestMethod]
        public void Encrypt()
        {
            //testString
            var result = GZipHelper.Compress(testString);
            Console.WriteLine(result);

        }

        [TestMethod]
        public void Decrypt()
        {
            var compress = GZipHelper.Compress(testString);
            var result = GZipHelper.Decompress(compress);
            Console.WriteLine(result);
        }
    }
}
