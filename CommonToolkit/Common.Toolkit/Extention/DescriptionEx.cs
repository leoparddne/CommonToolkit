using Common.Toolkit.Helper;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Common.Toolkit.Extention
{
    public static class DescriptionEx
    {
        /// <summary>
        /// 获取DescriptionAttribute信息
        /// 如果无法获取则返回obj.ToString()
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string? GetDescOrDefaultName(this object obj)
        {
            object[] attr = obj.GetType().GetCustomAttributes(false);
            var descAttr = (DescriptionAttribute?)attr.FirstOrDefault(f => f.GetType() == typeof(DescriptionAttribute));
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
        public static string? GetDesc<T>(this object obj, Expression<Func<T, object>> expr)
        {
            var pd = TypeDescriptor.GetProperties(typeof(T))[ExpressionHelper.GetPropertyName(expr)];
            //PropertyDescriptor pd = TypeDescriptor.GetProperties(typeof(T))[name];
            var description = pd == null ? null : pd.Attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
            return description == null ? "" : description.Description;
        }


        /// <summary>
        /// 获取枚举DescriptionAttribute信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        /// <returns></returns>
        public static string? GetDesc(this Enum value, bool nameInstead = true)
        {
            Type type = value.GetType();
            var name = System.Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            var field = type.GetField(name);
            if (field == null)
            {
                return null;
            }
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute?.Description;
        }
    }
}
