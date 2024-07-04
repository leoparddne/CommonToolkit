namespace Common.Toolkit.Helper
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 根据名称设置对象的指定字段
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="inData"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TIn TryParse<TIn>(TIn inData, string name, string value)
        {

            //var result = Activator.CreateInstance(outType);

            var prop = typeof(TIn).GetProperties().FirstOrDefault(f => f.Name == name);
            if (prop != null)
            {
                prop.SetValue(inData, Parse(prop.PropertyType, value));
            }

            var field = typeof(TIn).GetFields().FirstOrDefault(f => f.Name == name);
            if (field != null)
            {
                field.SetValue(inData, Parse(prop.PropertyType, value));
            }

            return inData;
        }

        /// <summary>
        /// 将字符串转为指定类型 目前支持的类型：int、double、 datetime、string
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Parse(Type type, string value)
        {
            if (type == typeof(int))
            {
                return ParseInt(value);
            }

            if (type == typeof(double))
            {
                return ParseDouble(value);
            }

            if (type == typeof(DateTime))
            {
                return ParseDateTime(value);
            }

            return value;
        }

        public static int ParseInt(string value)
        {
            return int.Parse(value);
        }

        public static DateTime ParseDateTime(string value)
        {
            return DateTime.Parse(value);
            //return DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", null);
        }

        public static double ParseDouble(string value)
        {
            return double.Parse(value);
        }
    }
}
