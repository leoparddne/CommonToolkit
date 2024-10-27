using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Common.Toolkit.Helper.Jwt
{
    public static class JwtHelper
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="key">加密使用的key</param>
        /// <param name="model"></param>
        /// <param name="expires">过期时间，默认1小时(无用，当前使用redis做相关限制)</param>
        /// <returns></returns>
        public static string GenerateToken(JwtSignModel model)
        {
            //创建claim
            Claim[] authClaims = new[] {
                new Claim(ClaimTypes.Name,model.Name ),
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(model.Data) ),
                new Claim(JwtRegisteredClaimNames.Sub,model.Sub),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            IdentityModelEventSource.ShowPII = true;

            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model.Key));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: model.Issuer,// "YLHPlatform",
                    audience: model.Audience,//"YLHPlatform",
                   expires: DateTime.Now.Add(model.ExpireHour),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
