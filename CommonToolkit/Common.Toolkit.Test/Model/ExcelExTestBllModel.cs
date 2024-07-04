using Common.Toolkit.Helper.ExcelEx;

namespace Common.Toolkit.Test.Model
{

    public class ExcelExTestBllModel
    {
        [ExcelExIndexAttrbute(3)]
        public string PartNo { get; set; }


        [ExcelExIndexAttrbute(20)]
        public string Reason { get; set; }
    }
}
