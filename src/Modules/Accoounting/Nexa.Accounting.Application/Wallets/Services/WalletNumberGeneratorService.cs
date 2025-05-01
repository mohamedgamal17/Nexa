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
            var stringBuilder = new StringBuilder(digits);

            using (var rng = RandomNumberGenerator.Create())
            {
                while (stringBuilder.Length < digits)
                {
                    int value = RandomNumberGenerator.GetInt32(0, 10);

                    stringBuilder.Append(value);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
