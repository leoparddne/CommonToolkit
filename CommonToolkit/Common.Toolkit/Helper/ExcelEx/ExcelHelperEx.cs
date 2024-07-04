using Common.Toolkit.Extention;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Common.Toolkit.Helper.ExcelEx
{
    public class ExcelHelperEx<T>
    {
        /// <summary>
        /// 根据对象获取导入模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void GetImportTemplate<T>(string fileName)
        {
            var titleDic = ParseTitleDic();
            ExceptionHelper.CheckException(titleDic == null || titleDic.Count == 0, "TemplateWithoutProperty");

            var excelHelper = new ExcelHelper(fileName, true);

            //标题导出
            foreach (var item in titleDic)
            {
                excelHelper.SetValue(0, item.Key, item.Value.Title);
            }
            excelHelper.Export();
            excelHelper.Dispose();
        }

        /// <summary>
        /// 解析标题字典
        /// </summary>
        /// <param name="typeInfo">key为列索引</param>
        /// <returns></returns>
        private static Dictionary<int, ExcelParseModel> ParseTitleDic()
        {
            var typeInfo = typeof(T).GetProperties();
            ExceptionHelper.CheckException(typeInfo.IsNullOrEmpty(), "TemplateWithoutProperty");

            var titleDic = new Dictionary<int, ExcelParseModel>();
            int index = 0;

            //标题获取
            foreach (var item in typeInfo)
            {
                string propName = item.Name;

                var attrList = item.GetCustomAttributes(false);
                ExcelExIndexAttrbute indexAttr = null;
                if (!attrList.IsNullOrEmpty())
                {
                    DescriptionAttribute descAttr = null;

                    //解析需要的特性
                    foreach (var attr in attrList)
                    {
                        if (attr is DescriptionAttribute descAttrData)
                        {
                            descAttr = descAttrData;
                        }
                        if (attr is ExcelExIndexAttrbute indexAttrData)
                        {
                            indexAttr = indexAttrData;
                        }
                    }

                    if (descAttr != null)
                    {
                        propName = descAttr.Description;
                    }
                }

                var finalIndex = 0;

                if (indexAttr != null)
                {
                    finalIndex = indexAttr.ColIndex;
                }
                else
                {
                    finalIndex = index++;
                }
                titleDic.Add(finalIndex, new ExcelParseModel { Title = propName, PropInfo = item });
            }

            return titleDic;
        }

        public List<T> ParseFile(string fileName, bool skipTitle = true)
        {
            int startRow = 0;
            if (skipTitle)
            {
                startRow++;
            }
            var result = new List<T>();
            var titleDic = ParseTitleDic();
            ExceptionHelper.CheckException(titleDic == null || titleDic.Count == 0, "TemplateWithoutProperty");


            var excelHelper = new ExcelHelper(fileName);
            var lastRowNo = excelHelper.GetLastRowNo();
            for (int i = startRow; i < lastRowNo + 1; i++)
            {
                //var ins = Activator.CreateInstance<T>();
                var tempData = new Dictionary<string, object>();
                foreach (var item in titleDic)
                {
                    object finalData = null;
                    var data = excelHelper.GetValueAuto(i, item.Key);
                    finalData = data;

                    if (item.Value.PropInfo.PropertyType == typeof(string))
                    {
                        var intValue = Convert.ToString(data);
                        finalData = intValue;
                    }

                    if (item.Value.PropInfo.PropertyType == typeof(double))
                    {
                        var intValue = Convert.ToDouble(data);
                        finalData = intValue;
                    }

                    if (item.Value.PropInfo.PropertyType == typeof(int))
                    {
                        var intValue = Convert.ToInt32(data);
                        finalData = intValue;
                    }

                    tempData.Add(item.Value.PropInfo.Name, finalData);


                    //try
                    //{
                    //    item.Value.PropInfo.SetValue(ins, data);
                    //}
                    //catch (Exception e)
                    //{
                    //    throw new Exception($"转换前后类型不一致:{e.Message}");
                    //}
                }
                var jsonData = JsonConvert.SerializeObject(tempData);
                var convertData = JsonConvert.DeserializeObject<T>(jsonData);

                result.Add(convertData);
            }

            excelHelper.Dispose();


            return result;
        }
    }
}
