using Common.Toolkit.Helper;

namespace Common.Toolkit.Test
{

    [TestClass]
    public class HttpHelperTest
    {
        [TestMethod]
        public void NotFoundTest()
        {
            try
            {
                //var x = HttpHelper.PostAsync("http://192.168.2.49/testx/xxx", new
                var x = HttpHelper.PostAsync("http://127.0.0.1:7012/api/WMS/AccountIn/Test", new
                {
                    Ax = "xx"
                });

                x.Wait();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [TestMethod]
        public void Test()
        {
            var x = HttpHelper.PostAsync("http://192.168.2.49/test", new
            {
                Ax = "xx"
            });

            x.Wait();

        }

        [TestMethod]
        public void Form()
        {
            //var x = HttpHelper.PostFormDataAsync("http://192.168.2.242:9999/MobileService.asmx/login", null,
            var x = HttpHelper.PostFormDataAsync("http://127.0.0.1:7026/api/integration/WoMaterialBill/Test", null,
                new Dictionary<string, object>
                {
                    { "name","admin"},
                    { "password","admin@123"},
                    { "pilot","PILOT2000"}
                }, 1000, true, true);

            x.Wait();
            Console.WriteLine(x.Result);
        }
    }
}
