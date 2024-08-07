﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Common.Toolkit.Helper
{
    public static partial class Extention
    {

        /// <summary>
        /// 判断内容是否为空，是返回默认值，否返回原值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        public static string StringDefaultName(this string name, string defaultName)
        {
            return string.IsNullOrEmpty(name) ? defaultName : name;
        }

        /// <summary>
        /// 转为字节数组
        /// </summary>
        /// <param name="base64Str">base64字符串</param>
        /// <returns></returns>
        public static byte[] ToBytes_FromBase64Str(this string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }

        /// <summary>
        /// 转换为MD5加密后的字符串（默认加密为32位）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5String(this string str)
        {
            string str1 = str;
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(str1);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            md5.Dispose();

            return sb.ToString();
        }

        /// <summary>
        /// Base64加密
        /// 注:默认采用UTF8编码
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(this string source)
        {
            return Base64Encode(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <param name="encoding">加密采用的编码方式</param>
        /// <returns></returns>
        public static string Base64Encode(this string source, Encoding encoding)
        {
            string encode = string.Empty;
            byte[] bytes = encoding.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密
        /// 注:默认使用UTF8编码
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(this string result)
        {
            return Base64Decode(result, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <param name="encoding">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(this string result, Encoding encoding)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encoding.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// 计算SHA1摘要
        /// 注：默认使用UTF8编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this string str)
        {
            return str.ToSHA1Bytes(Encoding.UTF8);
        }

        /// <summary>
        /// 计算SHA1摘要
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this string str, Encoding encoding)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] inputBytes = encoding.GetBytes(str);
            byte[] outputBytes = sha1.ComputeHash(inputBytes);

            return outputBytes;
        }

        /// <summary>
        /// 转为SHA1哈希加密字符串
        /// 注：默认使用UTF8编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToSHA1String(this string str)
        {
            return str.ToSHA1String(Encoding.UTF8);
        }

        /// <summary>
        /// 转为SHA1哈希
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToSHA1String(this string str, Encoding encoding)
        {
            byte[] sha1Bytes = str.ToSHA1Bytes(encoding);
            string resStr = BitConverter.ToString(sha1Bytes);
            return resStr.Replace("-", "").ToLower();
        }

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="defaultInt">错误返回值</param>
        /// <returns></returns>
        public static int ToInt(this string str, int defaultInt = -1)
        {
            //str = str.Replace("\0", "");
            if (string.IsNullOrEmpty(str))
                return defaultInt;
            if (int.TryParse(str, out int ret))
                return ret;
            else return defaultInt;
        }

        /// <summary>
        /// 二进制字符串转为Int
        /// </summary>
        /// <param name="str">二进制字符串</param>
        /// <returns></returns>
        public static int ToInt_FromBinString(this string str)
        {
            return Convert.ToInt32(str, 2);
        }

        /// <summary>
        /// 将16进制字符串转为Int
        /// </summary>
        /// <param name="str">数值</param>
        /// <returns></returns>
        public static int ToInt0X(this string str)
        {
            int num = Int32.Parse(str, NumberStyles.HexNumber);
            return num;
        }

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            return Convert.ToDouble(str);
        }

        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str)
        {
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="theEncoding">需要的编码</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str, Encoding theEncoding)
        {
            return theEncoding.GetBytes(str);
        }

        /// <summary>
        /// 将16进制字符串转为Byte数组
        /// </summary>
        /// <param name="str">16进制字符串(2个16进制字符表示一个Byte)</param>
        /// <returns></returns>
        public static byte[] To0XBytes(this string str)
        {
            List<byte> resBytes = new List<byte>();
            for (int i = 0; i < str.Length; i = i + 2)
            {
                string numStr = $@"{str[i]}{str[i + 1]}";
                resBytes.Add((byte)numStr.ToInt0X());
            }

            return resBytes.ToArray();
        }

        /// <summary>
        /// 将ASCII码形式的字符串转为对应字节数组
        /// 注：一个字节一个ASCII码字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToASCIIBytes(this string str)
        {
            return str.ToList().Select(x => (byte)x).ToArray();
        }

        /// <summary>
        /// 转换为日期格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            return Convert.ToDateTime(str);
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        /// <summary>
        /// 删除Json字符串中键中的@符号
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns></returns>
        public static string RemoveAt(this string jsonStr)
        {
            Regex reg = new Regex("\"@([^ \"]*)\"\\s*:\\s*\"(([^ \"]+\\s*)*)\"");
            string strPatten = "\"$1\":\"$2\"";
            return reg.Replace(jsonStr, strPatten);
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static object ToObject(this string jsonStr, Type type)
        {
            return JsonConvert.DeserializeObject(jsonStr, type);
        }

        /// <summary>
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xmlStr">XML字符串</param>
        /// <returns></returns>
        public static T XmlStrToObject<T>(this string xmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            string jsonJsonStr = JsonConvert.SerializeXmlNode(doc);

            return JsonConvert.DeserializeObject<T>(jsonJsonStr);
        }

        /// <summary>
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <param name="xmlStr">XML字符串</param>
        /// <returns></returns>
        public static JObject XmlStrToJObject(this string xmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            string jsonJsonStr = JsonConvert.SerializeXmlNode(doc);

            return JsonConvert.DeserializeObject<JObject>(jsonJsonStr);
        }

        /// <summary>
        /// 将Json字符串转为List'T'
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string jsonStr)
        {
            return string.IsNullOrEmpty(jsonStr) ? null : JsonConvert.DeserializeObject<List<T>>(jsonStr);
        }

        /// <summary>
        /// 将Json字符串转为DataTable
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this string jsonStr)
        {
            return jsonStr == null ? null : JsonConvert.DeserializeObject<DataTable>(jsonStr);
        }

        /// <summary>
        /// 将Json字符串转为JObject
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static JObject ToJObject(this string jsonStr)
        {
            return jsonStr == null ? JObject.Parse("{}") : JObject.Parse(jsonStr.Replace("&nbsp;", ""));
        }

        /// <summary>
        /// 将Json字符串转为JArray
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static JArray ToJArray(this string jsonStr)
        {
            return jsonStr == null ? JArray.Parse("[]") : JArray.Parse(jsonStr.Replace("&nbsp;", ""));
        }

        /// <summary>
        /// json数据转实体类,仅仅应用于单个实体类，速度非常快
        /// </summary>
        /// <typeparam name="T">泛型参数</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T ToEntity<T>(this string json)
        {
            if (json == null || json == "")
                return default(T);

            Type type = typeof(T);
            object obj = Activator.CreateInstance(type, null);

            foreach (var item in type.GetProperties())
            {
                PropertyInfo info = obj.GetType().GetProperty(item.Name);
                string pattern;
                pattern = "\"" + item.Name + "\":\"(.*?)\"";
                foreach (Match match in Regex.Matches(json, pattern))
                {
                    switch (item.PropertyType.ToString())
                    {
                        case "System.String": info.SetValue(obj, match.Groups[1].ToString(), null); break;
                        case "System.Int32": info.SetValue(obj, match.Groups[1].ToString().ToInt(), null); ; break;
                        case "System.Int64": info.SetValue(obj, Convert.ToInt64(match.Groups[1].ToString()), null); ; break;
                        case "System.DateTime": info.SetValue(obj, Convert.ToDateTime(match.Groups[1].ToString()), null); ; break;
                    }
                }
            }
            return (T)obj;
        }

        /// <summary>
        /// 转为首字母大写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToFirstUpperStr(this string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// 转为首字母小写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToFirstLowerStr(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        /// <summary>
        /// 转为网络终结点IPEndPoint
        /// </summary>=
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static IPEndPoint ToIPEndPoint(this string str)
        {
            IPEndPoint iPEndPoint = null;
            try
            {
                string[] strArray = str.Split(':').ToArray();
                string addr = strArray[0];
                int port = Convert.ToInt32(strArray[1]);
                iPEndPoint = new IPEndPoint(IPAddress.Parse(addr), port);
            }
            catch
            {
                iPEndPoint = null;
            }

            return iPEndPoint;
        }

        /// <summary>
        /// 缩写字符串
        /// </summary>
        /// <param name="text">原字符串</param>
        /// <param name="length">最大长度</param>
        /// <param name="last">超过最大长度后的缩写</param>
        /// <returns></returns>
        public static string Abbreviation(this string text, int length, string last = "...")
        {
            if (text.Length > length) text = text.Substring(0, length) + last;
            return text;
        }

        /// <summary>
        /// 转为流量字符串
        /// </summary>
        /// <param name="bytes">流量大小</param>
        /// <returns></returns>
        public static string ToTrafficString(this long bytes)
        {
            if (bytes > 0)
            {
                if (bytes < 1000) return bytes.ToString() + "B";
                if (bytes < 1000 * 1024) return (bytes / 1024D).ToString("#.##") + "KB";
                if (bytes < 1000 * 1024 * 1024) return (bytes / 1024D / 1024D).ToString("#.##") + "MB";
                if (bytes < 1000D * 1024 * 1024 * 1024) return (bytes / 1024D / 1024D / 1024D).ToString("#.##") + "GB";
                else return (bytes / 1024D / 1024D / 1024D / 1024D).ToString("#.##") + "TB";
            }
            else
            {
                if (bytes > -1000) return bytes.ToString() + "B";
                if (bytes > -1000 * 1024) return (bytes / 1024D).ToString("#.##") + "KB";
                if (bytes > -1000 * 1024 * 1024) return (bytes / 1024D / 1024D).ToString("#.##") + "MB";
                if (bytes > -1000D * 1024 * 1024 * 1024) return (bytes / 1024D / 1024D / 1024D).ToString("#.##") + "GB";
                else return (bytes / 1024D / 1024D / 1024D / 1024D).ToString("#.##") + "TB";
            }
        }

        /// <summary>
        /// 去除原字符串结尾处的所有替换字符串
        /// 如：原字符串"sdlfjdcdcd",替换字符串"cd" 返回"sdlfjd"
        /// </summary>
        /// <param name="strSrc">源字符串</param>
        /// <param name="strTrim">去除的字符串</param>
        /// <param name="isLoop">是否循环去除,默认为true</param>
        /// <returns></returns>
        public static string TrimEnd(this string strSrc, string strTrim, bool isLoop = true)
        {
            if (string.IsNullOrEmpty(strSrc) || string.IsNullOrEmpty(strTrim)) return strSrc;
            if (strSrc.EndsWith(strTrim))
            {
                string strDes = strSrc.Substring(0, strSrc.Length - strTrim.Length);
                return !isLoop ? strDes : strDes.TrimEnd(strTrim);
            }
            return strSrc;
        }

        /// <summary>
        /// 去除原字符串开始处的所有替换字符串
        /// 如：原字符串"sdlfjdcdcd",替换字符串"cd" 返回"sdlfjd"
        /// </summary>
        /// <param name="strSrc">源字符串</param>
        /// <param name="strTrim">去除的字符串</param>
        /// <param name="isLoop">是否循环去除,默认为true</param>
        /// <returns></returns>
        public static string TrimStart(this string strSrc, string strTrim, bool isLoop = true)
        {
            if (strSrc.StartsWith(strTrim))
            {
                string strDes = strSrc.Substring(0, strSrc.Length - strTrim.Length);
                return !isLoop ? strDes : strDes.TrimStart(strTrim);
            }
            return strSrc;
        }

        /// <summary>
        /// 删除字符串内的空格和回车
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveAndEnter(this string str)
        {
            return str == null ? null : str.Replace("\r", "").Replace("\n", "").Replace(" ", "");
        }

        /// <summary>
        /// 返回该字符串是否手机号
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns></returns>
        public static bool IsHandset(this string str)
        {
            return Regex.IsMatch(str, @"^1[3456789]\d{9}$");
        }

        /// <summary>
        /// string转bool
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            bool defatultBool = false;

            bool.TryParse(str, out defatultBool);

            return defatultBool;
        }


        //public static int ToInt(this string str, int defaultValue)
        //{
        //    if (!string.IsNullOrWhiteSpace(str))
        //    {
        //        int result;
        //        if (int.TryParse(str, out result))
        //            return result;
        //    }
        //    return defaultValue;
        //}

        /// <summary>
        /// 根据split分割字符串后从得到的列表中查找数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static bool Container(this string str, string data, string split)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            var listData = data.Split(split);

            return listData.Contains(data);
        }


        /// <summary>
        /// 对象集合转字符串
        /// </summary>
        /// <param name="list">对象</param>
        /// <param name="separator">集合分割字符串 默认;</param>
        /// <param name="separatorObject">对象分割字符串 默认;</param>
        /// <returns></returns>
        public static string ListToString<T>(this List<T> list, string separator = ";", string separatorObject = ",")
        {
            List<string> list2 = new List<string>();
            foreach (object item in list)
            {
                try
                {
                    list2.Add(item.ObjectToString(separatorObject));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return string.Join(separator, list2);
        }

        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="separator">分割字符串 默认,</param>
        /// <returns></returns>
        public static string ObjectToString<T>(this T obj, string separator = ",")
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            List<object> strList = new List<object>();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                strList.Add(propertyInfo.GetValue(obj));
            }
            return string.Join(separator, strList);
        }
    }
}