using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace WebApp_Task4.Services
{
    public class EncryptionService
    {
        private readonly string _key;
        private readonly IConfigurationBuilder _configuration =
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        public EncryptionService(IConfiguration configuration)
        {
            _key = configuration["PasswordKey"]!;
        }

        public byte[] HashPassword(string password)
        {
            var key = Encoding.UTF8.GetBytes(_key);

            using (var hmac = new HMACSHA3_256(key))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
