using Domain.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.CodeAuth
{
    public class TokenHelper
    {
        public static TokenValidationParameters GetValidationParameters(string apiSecret)
        {
            byte[] key =Encoding.ASCII.GetBytes(apiSecret);
            return new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }

        public static string CreateToken(IEnumerable<Claim> claims,int minutesTillExpires, string tokenSecret,string tokenIssuer, string tokenAudience)
        {
            JwtSecurityTokenHandler tokenHandler= new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(tokenSecret);

            ClaimsIdentity subject = new ClaimsIdentity(claims);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor();

            tokenDescriptor.Issuer = tokenIssuer;
            tokenDescriptor.Audience = tokenAudience;
            tokenDescriptor.Subject = subject;
            tokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(minutesTillExpires);
            tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static List<Claim> CreateUserTokenClaims(UserModel model)
        {
            return new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,model.Id.ToString()),
                new Claim(ClaimTypes.Actor,model.Id.ToString()),
                new Claim(ClaimTypes.Name, model.Id.ToString()),
                new Claim(ClaimTypes.Email, model.Email),

            }.ToList();
        }
    }
}
