using Common.Toolkit.Helper.ExcelEx;

namespace Common.Toolkit.Test.Model
{
    public class ExcelExModel
    {
        [System.ComponentModel.Description("X")]
        public string A { get; set; }
        public string G { get; set; }
        public string B { get; set; }
        public int C { get; set; }


        [ExcelExIndexAttrbute(6)]
        public double D { get; set; }

        public int F { get; set; }

    }
}
