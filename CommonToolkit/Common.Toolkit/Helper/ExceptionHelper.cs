namespace Common.Toolkit.Helper
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="errMsg"></param>
        /// <param name="httpCode"></param>
        public static void CheckNull(object obj, string errMsg)
        {
            Check(obj == null, errMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enumValue"></param>
        /// <param name="httpCode"></param>
        public static void CheckNull(object obj, Enum enumValue)
        {
            Check(obj == null, enumValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exceptio"></param>
        /// <param name="httpCode"></param>
        public static void CheckNull(object obj, Exception exceptio)
        {
            Check(obj == null, exceptio);
        }

        /// <summary>
        /// check为true时候自动触发exception
        /// </summary>
        /// <param name="check"></param>
        /// <param name="errMsg"></param>
        /// <param name="httpCode"></param>
        public static void Check(bool check, string errMsg)
        {
            if (!check)
            {
                return;
            }
            Exec(new Exception(errMsg));
        }

        /// <summary>
        /// 如果check为true自动触发exception并且获取枚举上方description作为exception的错误信息
        /// </summary>
        /// <param name="check"></param>
        /// <param name="enumValue"></param>
        /// <param name="httpCode"></param>
        public static void Check(bool check, Enum enumValue)
        {
            Check(check, enumValue.GetDesc());
        }

        /// <summary>
        ///  check为true时候自动触发exception
        /// </summary>
        /// <param name="check"></param>
        /// <param name="exception"></param>
        /// <param name="httpCode"></param>
        public static void Check(bool check, Exception exception)
        {
            if (!check)
            {
                return;
            }
            Exec(exception);
        }


        public static void Exec(Exception exception)
        {
            throw exception;
        }
    }
}
