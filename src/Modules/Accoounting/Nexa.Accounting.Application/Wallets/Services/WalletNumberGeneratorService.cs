using System.Security.Cryptography;
using System.Text;

namespace Nexa.Accounting.Application.Wallets.Services
{
    public class WalletNumberGeneratorService : IWalletNumberGeneratorService
    {
        const string PERFIX = "WAL";
        public string Generate()
        {
       
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");

            string randomPart = GenerateSecureRandomNumber(6);

            return $"{PERFIX}{datePart}{randomPart}";
        }


        private static string GenerateSecureRandomNumber(int digits)
        {
            int bytesNeeded = (int)Math.Ceiling(digits / 2.0);

            byte[] randomBytes = new byte[bytesNeeded];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var stringBuilder = new StringBuilder();

            foreach (byte b in randomBytes)
            {
                stringBuilder.Append((b % 10).ToString()); 
            }

            return stringBuilder.ToString().Substring(0, digits);
        }
    }
}
