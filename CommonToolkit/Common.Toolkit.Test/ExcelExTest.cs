using Common.Toolkit.Helper.ExcelEx;
using Common.Toolkit.Test.Model;
using Newtonsoft.Json;

namespace Common.Toolkit.Test
{

    [TestClass]
    public class ExcelExTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = new ExcelHelperEx<ExcelExModel>();
            //var model = x.ParseFile("C:\\Users\\ivesBao\\Desktop\\a.xlsx");
            var model = x.ParseFile("C:\\Users\\ivesBao\\Desktop\\c.xlsx");
            //var model = x.ParseFile("C:\\Users\\ivesBao\\Desktop\\b.xlsx");
            Console.WriteLine(JsonConvert.SerializeObject(model));
        }
        [TestMethod]
        public void BllTest()
        {
            var x = new ExcelHelperEx<ExcelExTestBllModel>();
            //var model = x.ParseFile("C:\\Users\\ivesBao\\Desktop\\a.xlsx");
            var model = x.ParseFile("C:\\Users\\ivesBao\\Desktop\\方案---4020009494-原始数据.XLSX");
            //var model = x.ParseFile("C:\\Users\\ivesBao\\Desktop\\b.xlsx");
            Console.WriteLine(JsonConvert.SerializeObject(model));
        }

        [TestMethod]
        public void GetTemplate()
        {
            var x = new ExcelHelperEx<ExcelExModel>();
            x.GetImportTemplate<ExcelExModel>("C:\\Users\\ivesBao\\Desktop\\result.xlsx");
        }
    }
}