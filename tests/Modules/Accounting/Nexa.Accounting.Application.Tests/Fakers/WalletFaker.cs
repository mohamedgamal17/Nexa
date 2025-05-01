using Bogus;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.Accounting.Domain.Wallets;
namespace Nexa.Accounting.Application.Tests.Fakers
{
    public class WalletFaker : Faker<Wallet>
    {
        public WalletFaker(string? userId = null ,decimal? balance = null,IWalletNumberGeneratorService? numberGeneratorService= null)
        {
            CustomInstantiator(f => new Wallet(
                    numberGeneratorService?.Generate() ?? Ulid.NewUlid().ToString(),
                    userId ?? Guid.NewGuid().ToString() ,
                    balance ?? f.Random.Decimal(0, 5000)
               ));
        }
    }
}
