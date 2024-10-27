using System.ComponentModel;
using System.Reflection;

namespace Common.Toolkit.Helper
{
    public static class MapperHelper
    {
        /// <summary>
        /// 将数据映射到指定的对象中
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="obj"></param>
        /// <param name="outObj"></param>
        /// <param name="ignorDesc"></param>
        /// <returns></returns>
        public static TOut? AutoMap<TIn, TOut>(TIn obj, TOut outObj, bool ignorDesc = true) where TOut : new()
        {
            return AutoMap(obj, ignorDesc, outObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="obj"></param>
        /// <param name="ignorDesc">忽略description描述信息</param>
        /// <returns></returns>
        public static TOut? AutoMap<TIn, TOut>(TIn obj, bool ignorDesc = true) where TOut : new()
        {
            var result = new TOut();
            return AutoMap(obj, ignorDesc, result);
        }

        private static TOut? AutoMap<TIn, TOut>(TIn obj, bool ignorDesc, TOut result) where TOut : new()
        {
            PropertyInfo[] properties = typeof(TIn).GetProperties();
            if (properties == null)
            {
                return default;
            }

            //存储源对象属性
            Dictionary<string, PropertyInfo> propertiesDic = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo item in properties)
            {
                if (item == null)
                {
                    continue;
                }
                var hasSameKey = properties.Count(f => f.Name == item.Name) > 1;
                MethodInfo? methodInfo = item.GetGetMethod();
                if (methodInfo == null)
                {
                    continue;
                }
                var isHideBySig = methodInfo.IsHideBySig;

                //避免因为子类添加new修饰符符导致出现重名字段
                if (hasSameKey && isHideBySig && item.DeclaringType != typeof(TIn))
                {
                    continue;
                }
                propertiesDic.Add(item.Name, item);
            }

            PropertyInfo[] resultProperties = typeof(TOut).GetProperties();

            foreach (PropertyInfo j in resultProperties)
            {
                try
                {
                    //自定义属性处理别名
                    var descAttr = j.GetCustomAttributes(false).FirstOrDefault(f => f.GetType() == typeof(DescriptionAttribute));
                    if (descAttr == null)
                    {
                        continue;
                    }
                    if (descAttr is not DescriptionAttribute desc)
                    {
                        continue;
                    }
                    if (desc != null && !ignorDesc)
                    {
                        string desName = desc.Description;
                        if (propertiesDic.ContainsKey(desName))
                        {
                            j.SetValue(result, propertiesDic[desName].GetValue(obj));
                            continue;
                        }
                    }
                    else
                    {
                        if (propertiesDic.ContainsKey(j.Name))
                        {
                            j.SetValue(result, propertiesDic[j.Name].GetValue(obj));
                        }
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        j.SetValue(result, Activator.CreateInstance(j.PropertyType));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("转换前后类型不一致");
                    }
                }
            }
            return result;
        }

        public static IList<TOut?> AutoMap<TIn, TOut>(this List<TIn> list, bool ignorDesc = true) where TOut : new()
        {
            var result = new List<TOut?>();
            foreach (TIn item in list)
            {
                try
                {
                    var itemResult = AutoMap<TIn, TOut>(item, ignorDesc);
                    result.Add(itemResult);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// 将对象转为目标类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="outType"></param>
        /// <param name="ignorDesc"></param>
        /// <returns></returns>
        public static object? AutoMapByType(object obj, Type outType, bool ignorDesc = false)
        {
            var result = Activator.CreateInstance(outType);

            PropertyInfo[] properties = obj.GetType().GetProperties();

            //存储源对象属性
            Dictionary<string, PropertyInfo> propertiesDic = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo item in properties)
            {
                propertiesDic.Add(item.Name, item);
            }

            PropertyInfo[] resultProperties = outType.GetProperties();

            foreach (PropertyInfo j in resultProperties)
            {
                try
                {
                    var attrList = j.GetCustomAttributes(false).FirstOrDefault(f => f.GetType() == typeof(DescriptionAttribute));
                    if (attrList == null)
                    {
                        continue;
                    }
                    //自定义属性处理别名
                    if (attrList is not DescriptionAttribute desc)
                    {
                        continue;
                    }
                    if (desc != null && !ignorDesc)
                    {
                        string desName = desc.Description;
                        if (propertiesDic.ContainsKey(desName))
                        {
                            j.SetValue(result, propertiesDic[desName].GetValue(obj));
                            continue;
                        }
                    }
                    else
                    {
                        if (propertiesDic.ContainsKey(j.Name))
                        {
                            j.SetValue(result, propertiesDic[j.Name].GetValue(obj));
                        }
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        j.SetValue(result, Activator.CreateInstance(j.PropertyType));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("转换前后类型不一致");
                    }
                }
            }
            return result;
        }
    }
}
