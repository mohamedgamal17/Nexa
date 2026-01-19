using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Nexa.Accounting.Application.Wallets.Services
{
    public class WalletNumberGeneratorService : IWalletNumberGeneratorService
    {
        const string PERFIX = "WAL";

        private readonly IConfiguration _configuration;

        public WalletNumberGeneratorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generate()
        {
            return GenerateSecureRandomNumber();
        }


        private  string GenerateSecureRandomNumber()
        {
            int timeSlice = (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 60 % 10_000);

            int r1 = RandomNumberGenerator.GetInt32(0, 10_000);

            int r2 = RandomNumberGenerator.GetInt32(0, 10_000);

            string core = $"{timeSlice:D4}{r1:D4}{r2:D4}";

            int checksum = SecureChecksum(core);

            return $"{timeSlice:D4}{r1:D4}{r2:D4}{checksum:D2}";
        }

        private  int SecureChecksum(string input)
        {
            var secret = _configuration.GetValue<string>("WalletHmacSecret")
                         ?? throw new InvalidOperationException("Hmac secret key is missing. Please provide hmac secret key in config file");

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));

            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));

            return BitConverter.ToUInt16(hash, 0) % 100;
        }
    }
}
