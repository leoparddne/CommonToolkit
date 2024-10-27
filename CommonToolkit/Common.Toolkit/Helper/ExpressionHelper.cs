using System.Linq.Expressions;

namespace Common.Toolkit.Helper
{

    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> True<T>() { return param => true; }
        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        #region 与或非封装
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            //return one.Compose(another, Expression.AndAlso);
            return one.Compose(another, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            return one.Compose(another, Expression.Or);
            //return one.Compose(another, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> Compose<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another, Func<Expression, Expression, Expression> action)
        {
            //创建新参数
            var newParameter = Expression.Parameter(typeof(T));

            var parameterReplacer = new ParameterReplacer(newParameter);
            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);

            var body = action(left, right);

            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        /// <summary>
        /// 参数替换
        /// </summary>
        internal class ParameterReplacer : ExpressionVisitor
        {
            public ParameterReplacer(ParameterExpression paramExpr)
            {
                Parameter = paramExpr;
            }

            //新的表达式参数
            private ParameterExpression Parameter { get; set; }

            public Expression Replace(Expression expr)
            {
                return this.Visit(expr);
            }

            /// <summary>
            /// 覆盖父方法，返回新的参数
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                return Parameter;
            }
        }
        #endregion

        /// <summary>
        /// 获取表达式调用的字段名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            switch (expr.Body)
            {
                case MemberExpression memberExpression:
                    return memberExpression.Member.Name;
                case UnaryExpression unaryExpression:
                    if (unaryExpression.Operand is MemberExpression expression)
                    {
                        return expression.Member.Name;
                    }

                    break;
                default:
                    break;
            }

            return string.Empty;
        }

        public static string GetPropertyNameByLambda<TIn>(TIn data, Expression<Func<TIn, object>> exp)
        {
            return GetPropertyName(exp);
        }

        public static Type? GetFieldTypeByName<TIn>(string fieldName)
        {
            var propertyInfo = typeof(TIn).GetProperties().FirstOrDefault(f => f.Name == fieldName);

            if (propertyInfo != null)
            {
                return propertyInfo.PropertyType;
            }

            var fieldInfo = typeof(TIn).GetField(fieldName);
            if (fieldInfo != null)
            {
                return fieldInfo.FieldType;
            }

            return null;
        }

        public static object? GetDataByName<TIn>(TIn data, string fieldName)//, Expression<Func<TIn, object>> exp)
        {
            if (data == null)
            {
                return null;
            }

            var type = data.GetType();

            var propertyInfo = type.GetProperties().FirstOrDefault(f => f.Name == fieldName);

            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(data);
                return value;
            }

            var fieldInfo = type.GetField(fieldName);
            if (fieldInfo != null)
            {
                var value = fieldInfo.GetValue(data);
                return value;
            }

            return null;
        }

        public static TOut? GetDataByName<TIn, TOut>(TIn data, string fieldName)
        {
            var result = GetDataByName<TIn>(data, fieldName);
            if (result == null)
            {
                return default;
            }
            if (result.GetType() == typeof(TOut))
            {
                return (TOut)result;
            }

            return default;
        }
    }
}
