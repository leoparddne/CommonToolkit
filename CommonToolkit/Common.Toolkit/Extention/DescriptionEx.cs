using Common.Toolkit.Helper;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Toolkit.Extention
{
    public static class DescriptionEx
    {
        /// <summary>
        /// 获取DescriptionAttribute信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDesc(this object obj, bool nameInstead = true)
        {
            object[] attr = obj.GetType().GetCustomAttributes(false);
            DescriptionAttribute descAttr = (DescriptionAttribute)attr.FirstOrDefault(f => f.GetType() == typeof(DescriptionAttribute));
            if (descAttr == null)
            {
                return obj.ToString();
            }
            else
            {
                return descAttr.Description;
            }
        }

        /// <summary>
        /// 获取类属性上方DescriptionAttribute信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string GetDesc<T>(this object obj, Expression<Func<T, object>> expr)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(typeof(T))[ExpressionHelper.GetPropertyName(expr)];
            //PropertyDescriptor pd = TypeDescriptor.GetProperties(typeof(T))[name];
            DescriptionAttribute description = pd == null ? null : pd.Attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
            return description == null ? "" : description.Description;
        }


        /// <summary>
        /// 获取枚举DescriptionAttribute信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        /// <returns></returns>
        public static string GetDesc(this Enum value, bool nameInstead = true)
        {
            Type type = value.GetType();
            string name = System.Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute?.Description;
        }
    }
}
