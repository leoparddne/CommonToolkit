using Common.Toolkit.ExceptionEx;

namespace Common.Toolkit.Helper
{
    public static class ExceptionHelper
    {

        /// <summary>
        /// check为true时候自动触发exception
        /// </summary>
        /// <param name="check"></param>
        /// <param name="errMsg"></param>
        /// <param name="httpCode"></param>
        public static void CheckException(bool check, string errMsg, int? httpCode = null)
        {
            if (check)
            {
                Exec(new Exception(errMsg), httpCode);
            }
        }

        /// <summary>
        /// 如果check为true自动触发exception并且获取枚举上方description作为exception的错误信息
        /// </summary>
        /// <param name="check"></param>
        /// <param name="enumValue"></param>
        /// <param name="httpCode"></param>
        public static void CheckException(bool check, Enum enumValue, int? httpCode = null)
        {
            CheckException(check, enumValue.GetDesc(), httpCode);
        }

        /// <summary>
        ///  check为true时候自动触发exception
        /// </summary>
        /// <param name="check"></param>
        /// <param name="exception"></param>
        /// <param name="httpCode"></param>
        public static void CheckException(bool check, Exception exception, int? httpCode = null)
        {
            if (check)
            {
                Exec(exception, httpCode);
            }
        }

        public static void Exec(Exception exception, int? httpCode = null)
        {
            if (httpCode == null)
            {
                throw exception;
            }

            throw new HttpCodeException(httpCode!.Value, exception.Message);
        }
    }
}
