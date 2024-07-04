namespace Common.Toolkit.Helper.ExcelEx
{
    public class ExcelExIndexAttrbute : Attribute
    {
        /// <summary>
        /// 列索引 - 从0开始
        /// </summary>
        public int ColIndex { get; set; }


        public ExcelExIndexAttrbute(int colIndex)
        {
            ColIndex = colIndex;
        }
    }
}
