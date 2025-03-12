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
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<EnumInfo> GetEnumDescList(Type type)
        {
            List<EnumInfo> result = new List<EnumInfo>();
            List<string> nameList = Enum.GetNames(type).ToList();
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                if (nameList.Contains(fieldInfo.Name))
                {
                    DescriptionAttribute[] array = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                    if (array.Length != 0)
                    {
                        result.Add(new EnumInfo
                        {
                            Name = fieldInfo.Name,
                            Desc = array[0].Description,
                            Value = (int)fieldInfo.GetValue(type)
                        });
                    }
                    else
                    {
                        result.Add(new EnumInfo
                        {
                            Name = fieldInfo.Name,
                            Value = (int)fieldInfo.GetValue(type)
                        });
                    }
                }
            }

            return result;
        }
    }
}

    }
}
