using Common.Toolkit.Extention;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Common.Toolkit.Helper.ExcelEx
{

    public class ExcelHelper
    {
        public IWorkbook obook;
        public ISheet osheet;
        private string filePath;
        private bool isXls = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isWrite">是否为读取excel</param>
        /// <exception cref="NotSupportedException"></exception>
        public ExcelHelper(string filePath, bool isWrite = false)
        {
            ExceptionHelper.CheckException(!isWrite && !File.Exists(filePath), "FileNotExist");
            this.filePath = filePath;
            string extension = System.IO.Path.GetExtension(filePath);

            switch (extension.ToLower())
            {
                case ".xls":
                    isXls = true;
                    obook = new HSSFWorkbook();
                    break;
                case ".xlsx":
                    obook = new XSSFWorkbook();
                    break;
                default:
                    throw new NotSupportedException();

            }
            if (isWrite)
            {

                osheet = obook.CreateSheet("sheet1");
            }
            else
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    if (isXls)
                    {
                        obook = new HSSFWorkbook(fs); //把xls文件中的数据写入wk中
                    }
                    else
                    {
                        obook = new XSSFWorkbook(fs);//把xlsx文件中的数据写入wk中
                    }
                }

                osheet = obook.GetSheetAt(0);
            }
        }

        /// <summary>
        /// 切换sheet
        /// </summary>
        /// <param name="sheetNo"></param>
        public void ChangeSheet(int sheetNo)
        {
            osheet = obook.GetSheetAt(sheetNo);
        }


        /// <summary>
        /// 切换sheet
        /// </summary>
        /// <param name="sheetNo"></param>
        public void ChangeSheetBySheetName(string sheetName)
        {
            osheet = obook.GetSheet(sheetName);
        }

        /// <summary>
        /// 创建sheet
        /// </summary>
        /// <param name="sheetNo"></param>
        public void CreateSheet(string sheetName)
        {
            obook.CreateSheet(sheetName);
        }


        /// <summary>
        /// 获取指定行列的数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public ICell GetCell(int row, int col)
        {
            return osheet.GetRow(row).GetCell(col);
        }

        public string GetString(int row, int col)
        {
            return GetCell(row, col).StringCellValue;
        }

        public double GetDouble(int row, int col)
        {
            return GetCell(row, col).NumericCellValue;
        }

        public DateTime? GetDateTime(int row, int col)
        {
            return GetCell(row, col).DateCellValue;
        }

        public bool GetBool(int row, int col)
        {
            return GetCell(row, col).BooleanCellValue;
        }

        public int GetLastRowNo()
        {
            return osheet.LastRowNum;
        }

        public bool IsCellNull(int row, int col)
        {
            return GetCell(row, col) == null;
        }

        public bool IsCellBlank(int row, int col)
        {
            if (IsCellNull(row, col))
            {
                return true;
            }
            else
            {
                return GetCell(row, col).CellType == CellType.Blank;
            }
        }



        public string GetValue(int row, int col)
        {
            ICell cell = GetCell(row, col);

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString(); ;
                case CellType.Error:
                    return cell.ErrorCellValue.ToString(); ;
                case CellType.Numeric:
                    return (DateUtil.IsCellDateFormatted(cell)) ? cell.DateCellValue.ToString() : cell.NumericCellValue.ToString(); ;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula: //公式
                    try
                    {
                        IFormulaEvaluator formulaEvaluator;
                        if (isXls)
                        {
                            formulaEvaluator = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        }
                        else
                        {
                            formulaEvaluator = new XSSFFormulaEvaluator(cell.Sheet.Workbook);
                        }

                        var evaluatCell = formulaEvaluator.EvaluateInCell(cell);
                        return evaluatCell.ToString();
                    }
                    catch
                    {
                        return "CellType: Formula. " + cell.NumericCellValue.ToString();
                    }
                case CellType.Unknown: //无法识别类型
                default: //默认类型
                    return cell.ToString();
            }
        }


        public object GetValueAuto(int row, int col)
        {
            ICell cell = GetCell(row, col);

            return GetByCell(cell);
        }

        /// <summary>
        /// 获取合并区域信息
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private List<CellRangeAddress> GetMergedCellRegion(ISheet sheet)
        {
            int mergedRegionCellCount = sheet.NumMergedRegions;
            var returnList = new List<CellRangeAddress>();

            for (int i = 0; i < mergedRegionCellCount; i++)
            {
                returnList.Add(sheet.GetMergedRegion(i));
            }

            return returnList;
        }

        public MergeCellInfo GetMergeInfo(int row, int col)
        {
            var cell = GetCell(row, col);
            if (cell == null)
            {
                return null;
            }

            if (!cell.IsMergedCell)
            {
                return null;
            }
            var mergedCellList = GetMergedCellRegion(osheet);
            if (mergedCellList.IsNullOrEmpty())
            {
                return null;
            }

            foreach (var mergedCell in mergedCellList)
            {
                if ((cell.RowIndex >= mergedCell.FirstRow && cell.RowIndex <= mergedCell.LastRow) &&
                    (cell.ColumnIndex >= mergedCell.FirstColumn && cell.ColumnIndex <= mergedCell.LastColumn))
                {
                    return new MergeCellInfo
                    {
                        FirstRow = mergedCell.FirstRow,
                        FirstColumn = mergedCell.FirstColumn,
                        LastRow = mergedCell.LastRow,
                        LastColumn = mergedCell.LastColumn
                    };
                }
            }

            return null;
        }


        private object GetByCell(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue; ;
                case CellType.Error:
                    return cell.ErrorCellValue.ToString(); ;
                case CellType.Numeric:
                    return (DateUtil.IsCellDateFormatted(cell)) ? cell.DateCellValue : cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula: //公式
                    try
                    {
                        IFormulaEvaluator formulaEvaluator;
                        if (isXls)
                        {
                            formulaEvaluator = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        }
                        else
                        {
                            formulaEvaluator = new XSSFFormulaEvaluator(cell.Sheet.Workbook);
                        }

                        formulaEvaluator.EvaluateInCell(cell);

                        var evaluatCell = formulaEvaluator.EvaluateInCell(cell);
                        return GetByCell(evaluatCell);
                    }
                    catch
                    {
                        return "CellType: Formula. " + cell.StringCellValue;
                    }
                case CellType.Unknown: //无法识别类型
                default: //默认类型
                    return cell.ToString();
            }
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="r1">左上角单元格行标(从0开始，下同)</param>
        /// <param name="c1">左上角单元格列标</param>
        /// <param name="r2">右下角单元格行标</param>
        /// <param name="c2">右下角单元格列标</param>
        public void Merge(int r1, int c1, int r2, int c2)
        {
            osheet.AddMergedRegion(new CellRangeAddress(r1, c1, r2, c2));
        }

        /// <summary>
        /// 设置单元格内容
        /// </summary>
        /// <param name="row">单元格行标(从0开始，下同)</param>
        /// <param name="col">单元格列标</param>
        /// <param name="o">写入内容</param>
        public void SetValue(int r, int c, object o)
        {
            if (o != null)
            {
                if (r <= osheet.LastRowNum)
                {
                    IRow row = osheet.GetRow(r);
                    if (row == null)
                    {
                        row = osheet.CreateRow(r);
                        row.HeightInPoints = 14;
                    }
                    if (c <= row.LastCellNum)
                    {
                        ICell cell = row.CreateCell(c);
                        cell.SetCellValue(o.ToString());
                    }
                    else
                    {
                        for (int j = row.LastCellNum; j < c; j++)
                        {
                            row.CreateCell(j + 1);
                            ICell cell22 = row.GetCell(j + 1);
                            ICellStyle style = obook.CreateCellStyle();
                            cell22.CellStyle = style;
                        }
                        ICell cell = row.GetCell(c);
                        cell.SetCellValue(o.ToString());
                    }
                }
                else
                {
                    for (int i = osheet.LastRowNum; i < r; i++)
                    {
                        IRow row22 = osheet.CreateRow(i + 1);
                        row22.HeightInPoints = 14;
                    }
                    IRow row = osheet.GetRow(r);
                    for (int j = row.LastCellNum; j < c; j++)
                    {
                        row.CreateCell(j + 1); ;
                        ICell cell22 = row.GetCell(j + 1);
                        ICellStyle style = obook.CreateCellStyle();
                        cell22.CellStyle = style;
                    }
                    ICell cell = row.GetCell(c);
                    cell.SetCellValue(o.ToString());

                }

            }
        }

        /// <summary>
        /// 设置行高
        /// </summary>
        /// <param name="r">行数</param>
        /// <param name="height">高度</param>
        public void SetRowHeight(int r, int height)
        {
            if (r <= osheet.LastRowNum)
            {
                IRow row = osheet.GetRow(r);
                if (row != null)
                {
                    row.HeightInPoints = height;
                }
            }
        }

        /// <summary>
        /// 设置字体宽度
        /// </summary>
        /// <param name="c">列标</param>
        /// <param name="width">宽度值（例如设置为1，表示一个英文字符的宽度）</param>
        public void SetCollumWdith(int c, int width)
        {
            osheet.SetColumnWidth(c, 256 * width);
        }

        /// <summary>
        /// 设置单元格对齐方式
        /// </summary>
        /// <param name="r">行标</param>
        /// <param name="c">列标</param>
        /// <param name="align">对齐方式（'L',左对齐；'C'居中；'R'右对齐）</param>
        public void SetCellAlignment(int r, int c, char align)
        {
            if (r <= osheet.LastRowNum)
            {
                IRow row = osheet.GetRow(r);
                if (row != null)
                {
                    if (c <= row.LastCellNum)
                    {
                        ICell cell = row.GetCell(c);
                        ICellStyle style = cell.CellStyle;
                        //设置单元格的样式：水平对齐居中
                        if (align == 'C')
                            style.Alignment = HorizontalAlignment.Center;
                        else if (align == 'L')
                            style.Alignment = HorizontalAlignment.Left;
                        else if (align == 'R')
                            style.Alignment = HorizontalAlignment.Right;
                        cell.CellStyle = style;
                    }
                }
            }
        }

        /// <summary>
        /// 设置字体样式
        /// </summary>
        /// <param name="r">行标</param>
        /// <param name="c">列标</param>
        /// <param name="size">字体大小，0为默认</param>
        /// <param name="f">字体样式（‘B’加粗，‘I’斜体）</param>
        /// <param name="color">字体颜色('R'红,'B'蓝,'G'绿,'Y'黄,'P'粉,'O'橙,'W'白)</param>
        public void SetCellFont(int r, int c, int size, char f, char color)
        {
            if (r <= osheet.LastRowNum)
            {
                IRow row = osheet.GetRow(r);
                if (row != null)
                {
                    if (c <= row.LastCellNum)
                    {
                        ICell cell = row.GetCell(c);
                        ICellStyle style = cell.CellStyle;
                        //新建一个字体样式对象
                        IFont font = obook.CreateFont();
                        //设置字体大小
                        if (size > 0)
                            font.FontHeightInPoints = Convert.ToInt16(size);
                        switch (f)
                        {
                            case 'B':
                                {
                                    //设置字体加粗样式
                                    font.IsBold = true;
                                }
                                break;
                            case 'I':
                                {
                                    //设置字体加粗样式
                                    font.IsItalic = true;
                                }
                                break;
                        }
                        switch (color)
                        {
                            case 'R': { font.Color = HSSFColor.Red.Index; } break;
                            case 'B': { font.Color = HSSFColor.Blue.Index; } break;
                            case 'G': { font.Color = HSSFColor.Green.Index; } break;
                            case 'Y': { font.Color = HSSFColor.Yellow.Index; } break;
                            case 'P': { font.Color = HSSFColor.Pink.Index; } break;
                            case 'O': { font.Color = HSSFColor.Orange.Index; } break;
                            case 'W': { font.Color = HSSFColor.White.Index; } break;
                        }
                        //使用SetFont方法将字体样式添加到单元格样式中 
                        style.SetFont(font);
                        //将新的样式赋给单元格
                        cell.CellStyle = style;
                    }
                }
            }
        }

        /// <summary>
        /// 写入到Excel文档
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Export(string path)
        {
            // 写入到客户端  
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                obook.Write(fs);
            }
            return true;
        }

        /// <summary>
        /// 使用构造对象时的文件名保存文件
        /// </summary>
        /// <returns></returns>
        public bool Export()
        {
            return Export(filePath);
        }

        /// <summary>
        /// 导出数据设置
        /// </summary>
        /// <param name="dt">源数据</param>
        ///  <param name="IsHead">是否需要表头</param>
        public void ExportDataToExcel(DataTable dt, bool IsHead = true)
        {
            // 设置表头
            if (IsHead)
            {
                IRow headerRow = osheet.CreateRow(0);
                foreach (DataColumn column in dt.Columns)
                {
                    int columnIndex = column.Ordinal;
                    headerRow.CreateCell(columnIndex).SetCellValue(column.Caption);
                    osheet.SetColumnWidth(columnIndex, 50 * 128);
                }

                // 固定首行
                osheet.CreateFreezePane(0, 1, 0, dt.Columns.Count - 1);
            }

            var rowIndex = (IsHead ? 1 : 0);

            for (var i = 0; i < dt.Rows.Count; i++, rowIndex++)
            {
                // 填充数据
                IRow dataRow = osheet.CreateRow(rowIndex);
                foreach (DataColumn column in dt.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(Convert.ToString(dt.Rows[i][column]));
                    //dataRow.GetCell(column.Ordinal).CellStyle = styleText;
                }
            }
        }


        public void Dispose()
        {
            obook.Close();
            GC.Collect();
        }
    }
}
