using Newtonsoft.Json;
using System.Text.RegularExpressions;


namespace Common.Toolkit.Helper
{
    public static class JsonExtention
    {
        public static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            MaxDepth = 2,
        };

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, jsonSerializerSettings);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="jsonConverterArray"></param>
        /// <param name="ignoreNullValue">是否忽略值为null的字段</param>
        /// <returns></returns>
        public static string ToJson(this object obj, JsonConverter[] jsonConverterArray, bool ignoreNullValue = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = ignoreNullValue ? NullValueHandling.Ignore : NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 2,
                Converters = jsonConverterArray
            };
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }

        /// <summary> 
        /// Json格式转换成键值对，键值对中的Key需要区分大小写 
        /// </summary> 
        /// <param name="JsonData">需要转换的Json文本数据</param> 
        /// <returns></returns> 
        public static Dictionary<string, object> ToDictionary(string JsonData)
        {
            object Data = null;
            Dictionary<string, object> Dic = new Dictionary<string, object>();
            if (JsonData.StartsWith("["))
            {
                //如果目标直接就为数组类型，则将会直接输出一个Key为List的List<Dictionary<string, object>>集合 
                //使用示例List<Dictionary<string, object>> ListDic = (List<Dictionary<string, object>>)Dic["List"]; 
                List<Dictionary<string, object>> List = new List<Dictionary<string, object>>();
                MatchCollection ListMatch = Regex.Matches(JsonData, @"{[\s\S]+?}");//使用正则表达式匹配出JSON数组 
                foreach (Match ListItem in ListMatch)
                {
                    List.Add(ToDictionary(ListItem.ToString()));//递归调用 
                }
                Data = List;
                Dic.Add("List", Data);
            }
            else
            {
                MatchCollection Match = Regex.Matches(JsonData, @"""(.+?)"": {0,1}(\[[\s\S]+?\]|null|"".+?""|-{0,1}\d*)");//使用正则表达式匹配出JSON数据中的键与值 
                foreach (Match item in Match)
                {
                    try
                    {
                        if (item.Groups[2].ToString().StartsWith("["))
                        {
                            //如果目标是数组，将会输出一个Key为当前Json的List<Dictionary<string, object>>集合 
                            //使用示例List<Dictionary<string, object>> ListDic = (List<Dictionary<string, object>>)Dic["Json中的Key"]; 
                            List<Dictionary<string, object>> List = new List<Dictionary<string, object>>();
                            MatchCollection ListMatch = Regex.Matches(item.Groups[2].ToString(), @"{[\s\S]+?}");//使用正则表达式匹配出JSON数组 
                            foreach (Match ListItem in ListMatch)
                            {
                                List.Add(ToDictionary(ListItem.ToString()));//递归调用 
                            }
                            Data = List;
                        }
                        else if (item.Groups[2].ToString().ToLower() == "null") Data = null;//如果数据为null(字符串类型),直接转换成null 
                        else Data = item.Groups[2].ToString(); //数据为数字、字符串中的一类，直接写入 
                        Dic.Add(item.Groups[1].ToString(), Data);
                    }
                    catch { }
                }
            }
            return Dic;
        }
    }

    /// <summary>
    /// 自定义转换类(实现忽略特定类型的字段)
    /// </summary>
    public class JsonCustomConverter<T> : JsonConverter<T>
    {
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            //遇到指定的类型时返回null,也可以使用JsonWriter writer内部字段根据名称进行扩展...
            writer.WriteNull();
        }
    }
}
