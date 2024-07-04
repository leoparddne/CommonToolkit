using Common.Toolkit.Helper.ExcelEx;
using Newtonsoft.Json;
using System.Data;

namespace Common.Toolkit.Test
{

    [TestClass]
    public class ExcelTest
    {
        ExcelHelper x;


        public ExcelTest()
        {
            x = new ExcelHelper("C:\\Users\\ivesBao\\Desktop\\c.xlsx", true);
            //x = new ExcelHelper("C:\\Users\\ivesBao\\Desktop\\c.xlsx");
        }


        [TestMethod]
        public void ISMergeCell()
        {
            var cell = x.GetCell(1, 1);
            Console.WriteLine(cell.IsMergedCell);



            cell = x.GetCell(5, 1);
            Console.WriteLine(cell.IsMergedCell);
        }


        [TestMethod]
        public void GetMergeRange()
        {
            //var cell = x.GetMergeInfo(1, 1);
            //var cell = x.GetMergeInfo(5, 1);
            var cell = x.GetMergeInfo(9, 0);
            Console.WriteLine(JsonConvert.SerializeObject(cell));
        }

        [TestMethod]
        public void Export()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("供应商名称", typeof(string));
            dataTable.Columns.Add("项目名称", typeof(string));
            dataTable.Columns.Add("产品型号", typeof(string));

            for (int i = 0; i < 10; i++)
            {

                var row = dataTable.NewRow();
                row["供应商名称"] = "立林";
                row["项目名称"] = i.ToString();
                row["产品型号"] = i.ToString();

                dataTable.Rows.Add(row);
            }

            x.ExportDataToExcel(dataTable);
            x.Export();
        }
    }
}