namespace Common.Toolkit.Helper.Jwt
{
    public class JwtSignModel
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Sub { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 超时时长默认一小时
        /// </summary>
        public TimeSpan ExpireHour { get; set; } = new TimeSpan(1, 0, 0);

        /// <summary>
        /// jwt包含的用户数据-可为空
        /// </summary>
        public object Data { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
