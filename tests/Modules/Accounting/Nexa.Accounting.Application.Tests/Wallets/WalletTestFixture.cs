using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Domain.Wallets;

namespace Nexa.Accounting.Application.Tests.Wallets
{
    public class WalletTestFixture : AccountingTestFixture
    {
        protected IWalletRepository WalletRepository { get; }

        public WalletTestFixture()
        {
            WalletRepository = ServiceProvider.GetRequiredService<IWalletRepository>();
        }

        protected override async Task InitializeAsync(IServiceProvider services)
        {
            await base.InitializeAsync(services);

            await TestHarness.Start();
        }

        protected override async Task ShutdownAsync(IServiceProvider services)
        {
            await base.ShutdownAsync(services);

            await TestHarness.Stop();
        }
        public async Task<Wallet> CreateWalletAsync(string? userId = null , decimal balance = 0)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<IWalletRepository>();

                var wallet = new Wallet(Guid.NewGuid().ToString(), userId ?? Guid.NewGuid().ToString(), balance);

                return await repository.InsertAsync(wallet);
            });
        }
    }
}
