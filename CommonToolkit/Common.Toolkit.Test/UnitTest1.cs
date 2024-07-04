using Common.Toolkit.Helper;
using Newtonsoft.Json;

namespace Common.Toolkit.Test
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
        {


            Dictionary<string, object>? getPara = new Dictionary<string, object>();
            getPara.Add("para", "test");
            Test getResult = await HttpHelper.GetAsync<Test>("http://localhost:56761/api/WMS/Warehouse/Test", getPara);
            System.Console.WriteLine(JsonConvert.SerializeObject(getResult));

            var getStr = await HttpHelper.GetAsync("http://localhost:56761/api/WMS/Warehouse/Test", getPara);

            System.Console.WriteLine(getStr);



            Dictionary<string, object>? postPara = new Dictionary<string, object>();
            postPara.Add("para", "test");
            var obj = new
            {
                Label = "x",
                Value = "v",
                Name = "xx"
            };

            Test postResult = await HttpHelper.PostAsync<Test>("http://localhost:56761/api/WMS/Warehouse/TestPost", obj);
            System.Console.WriteLine(JsonConvert.SerializeObject(postResult));
            var postStr = await HttpHelper.PostAsync("http://localhost:56761/api/WMS/Warehouse/TestPost", obj);
            System.Console.WriteLine(postStr);

        }
    }
}