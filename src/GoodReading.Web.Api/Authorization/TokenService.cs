using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GoodReading.Web.Api.Authorization
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfig _tokenConfig;

        public TokenService(IOptions<TokenConfig> tokenConfig)
        {
            _tokenConfig = tokenConfig.Value;
        }

        public string GetToken()
        {
            var key = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(_tokenConfig.Secret?.Trim()) ? "GoodReadingSecretKey1" : _tokenConfig.Secret.Trim());
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
