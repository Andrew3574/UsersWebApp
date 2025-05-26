using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApp_Tak4.Data;
using WebApp_Tak4.Models;
using WebApp_Task4.Repositories;

namespace WebApp_Tak4.Services.Auth
{
    public class JwtAuthenticationService
    {
        private static readonly ConfigurationBuilder _configurationBuilder = (ConfigurationBuilder)new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        private static readonly ConfigurationRoot _configurationRoot = (ConfigurationRoot)_configurationBuilder.Build();
        public static string GenerateJSONWebToken(User userInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtHeaderParameterNames.Jku, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim("UserState", userInfo.UserState.ToString())
                //клейм для роли по необходимости
            };
            var token = new JwtSecurityToken(
              issuer: "MyAuthServer",
              audience: "MyAuthClient",
              claims: claims,
              expires: DateTime.Now.AddDays(7),
              signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationRoot["JwtKey"]!)), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configurationRoot["JwtKey"]!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var identity = new ClaimsIdentity(jwtToken.Claims.ToList(), "jwt");
                var principals = new ClaimsPrincipal(identity);
                return principals;
            }
            catch
            {
                return null;
            }
        }
    }
}
