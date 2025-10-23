using Bogus;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.FundingResources;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Accounting.Application.Tests.Wallets
{
    public class WalletTestFixture : AccountingTestFixture
    {
        protected IWalletRepository WalletRepository { get; }
        protected IBaasWalletService BaasWalletService { get; }
        protected IAccountingRepository<LedgerEntry> LedgerEntryRepository { get; }
        protected Faker Faker { get; }
        public WalletTestFixture()
        {
            WalletRepository = ServiceProvider.GetRequiredService<IWalletRepository>();
            LedgerEntryRepository = ServiceProvider.GetRequiredService<IAccountingRepository<LedgerEntry>>();
            BaasWalletService = ServiceProvider.GetRequiredService<IBaasWalletService>();
            Faker = new Faker();
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

        public async Task<Wallet> CreateWalletWithReservedBalanceAsync(string? userId = null, decimal balance = 0 , decimal reservedBalance =0)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<IWalletRepository>();

                var wallet = await CreateWalletAsync(userId, balance);

                wallet.Reserve(reservedBalance);

                return await repository.UpdateAsync(wallet);
            });
        }
        public async Task<Wallet> CreateWalletAsync(string? userId = null , decimal balance = 0)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<IWalletRepository>();

                var wallet = new Wallet(Guid.NewGuid().ToString(), 
                    userId ?? Guid.NewGuid().ToString(), 
                    Guid.NewGuid().ToString(),
                    balance);

                return await repository.InsertAsync(wallet);
            });
        }


        public async Task<BankAccount> CreateBankAccountAsync(string? userId = null)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<IAccountingRepository<BankAccount>>();

                var bankAccount = new BankAccount(
                        userId ?? Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        "us",
                        "usd",
                        "4444",
                        "444444444"
                    );


                return await repository.InsertAsync(bankAccount);
            });
        }
    }
}
