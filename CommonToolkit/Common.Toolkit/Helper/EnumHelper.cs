using Common.Toolkit.Extention;
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
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取枚举信息列表(枚举名称、描述、值)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<EnumInfo> GetEnumDescList(this Enum enumObj)
        {
            Type type = enumObj.GetType();
            var result = new List<EnumInfo>();

            var names = Enum.GetNames(type).ToList();

            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo item in fields)
            {
                if (item == null)
                {
                    continue;
                }
                if (!names.Contains(item.Name))
                {
                    continue;
                }

                var attrbutes = item.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrbutes.IsNullOrEmpty())
                {
                    break;
                }

                DescriptionAttribute? firstDescAttr = null;

                foreach (var attr in attrbutes)
                {
                    if (attr is DescriptionAttribute descAttr)
                    {
                        firstDescAttr = descAttr;
                        break;
                    }
                }


                var value = item.GetValue(type);
                if (value is not int intValue)
                {
                    continue;
                }

                if (firstDescAttr != null)
                {
                    result.Add(new EnumInfo
                    {
                        Name = item.Name,
                        Desc = firstDescAttr.Description,
                        Value = intValue
                    });
                }
                else
                {
                    result.Add(new EnumInfo
                    {
                        Name = item.Name,
                        Value = intValue
                    });
                }
            }

            return result;
        }
    }
}
