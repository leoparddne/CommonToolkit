using System.ComponentModel;
using System.Reflection;

namespace Common.Toolkit.Helper
{
    public class EnumInfo
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Value { get; set; }
    }
    public static class EnumHelper
    {

        /// <summary>
        /// 获取枚举信息(枚举名称、描述、值)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDesc(this Enum value)
        {
            var type = value.GetType();
            var names = Enum.GetNames(type).ToList();

            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo item in fields)
            {
                if (!names.Contains(item.Name))
                {
                    continue;
                }
                if (value.ToString() != item.Name)
                {
                    continue;
                }
                DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])item.
            GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    return EnumAttributes[0].Description;

                }
                else
                {
                    return "";
                }
            }

            return "";
        }

        /// <summary>
        /// 获取枚举信息(枚举名称、描述、值)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<EnumInfo> GetEnumDescList(Type type)
        {
            List<EnumInfo> result = new List<EnumInfo>();
            //Type type = value.GetType();

            var names = Enum.GetNames(type).ToList();

            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo item in fields)
            {
                if (!names.Contains(item.Name))
                {
                    continue;
                }
                DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])item.
            GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    result.Add(new EnumInfo
                    {
                        Name = item.Name,
                        Desc = EnumAttributes[0].Description,
                        Value = (int)item.GetValue(type)
                    });
                }
                else
                {
                    result.Add(new EnumInfo
                    {
                        Name = item.Name,
                        Value = (int)item.GetValue(type)
                    });
                }
            }

            return result;
        }
    }
}
