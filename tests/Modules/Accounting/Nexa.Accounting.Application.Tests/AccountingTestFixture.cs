using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Application.Tests;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Respawn.Graph;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Application.Tests.Fakers;
using MassTransit.Testing;
namespace Nexa.Accounting.Application.Tests
{

    public  class AccountingTestFixture : TestFixture
    {
        protected ITestHarness TestHarness { get; private set; } = null!;
        protected override Task SetupAsync(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallModule<AccountingApplicationTestModuleInstaller>(configuration);
            return Task.CompletedTask;
        }
        protected override async Task InitializeAsync(IServiceProvider services)
        {
            TestHarness = services.GetTestHarness();
            await ResetSqlDb(services);
            await services.RunModulesBootstrapperAsync();
            await SeedData(services);
        }

      
        public async Task SeedData(IServiceProvider services)
        {
            var wallets =  await SeedWallets(services);

            var intenralTransctions = await SeedInternalTransactions(services,wallets);

            var entries = await SeedLedgerEntires(services, intenralTransctions);
        }
     

        private async Task<List<Wallet>> SeedWallets(IServiceProvider services)
        {
            var walletRepository = services.GetRequiredService<IAccountingRepository<Wallet>>();

            var numberGeneratorServices = services.GetRequiredService<IWalletNumberGeneratorService>();

            var wallets = new WalletFaker(numberGeneratorService: numberGeneratorServices)
                .Generate(15);

            await walletRepository.InsertManyAsync(wallets);

            return wallets;
        } 


        private async Task<List<InternalTransaction>> SeedInternalTransactions(IServiceProvider services , List<Wallet> wallets)
        {
            var faker = new Faker();

            var transactionRepository = services.GetRequiredService<IAccountingRepository<InternalTransaction>>();

            List<InternalTransaction> transactions = new List<InternalTransaction>();

            foreach (var wallet in wallets)
            {
                var randomWallets = wallets.PickRandom(10);

                List<InternalTransaction> walletTransactions = new List<InternalTransaction>();

                foreach (var reciverWallet in randomWallets)
                {
                    var transction = new InternalTransaction(wallet.Id, reciverWallet.Id, Ulid.NewUlid().ToString(), faker.Random.Decimal(1, 500),faker.PickRandom<TransactionStatus>());

                    walletTransactions.Add(transction);
                }


                transactions.AddRange(walletTransactions);
            }


            await transactionRepository.InsertManyAsync(transactions);

            return transactions;
        }

        private async Task<List<LedgerEntry>> SeedLedgerEntires(IServiceProvider services, List<InternalTransaction> transactions)
        {
            var entryRepository = services.GetRequiredService<IAccountingRepository<LedgerEntry>>();

            List<LedgerEntry> entries = new List<LedgerEntry>();

            var completedTransctions = transactions.Where(x => x.Status == TransactionStatus.Completed);

            foreach (var transaction in completedTransctions)
            {
                var senderEntry = new LedgerEntry(transaction.WalletId, transaction.Amount, TransactionType.Internal, TransactionDirection.Depit, transaction.Id, transaction.CompletedAt ?? DateTime.UtcNow);


                var reciverEntry = new LedgerEntry(transaction.ReciverId, transaction.Amount, TransactionType.Internal, TransactionDirection.Credit, transaction.Id, transaction.CompletedAt ?? DateTime.UtcNow);

                entries.Add(senderEntry);

                entries.Add(reciverEntry);
            }

            await entryRepository.InsertManyAsync(entries);

            return entries;
        }
        protected override async Task ShutdownAsync(IServiceProvider services)
        {
            await ResetSqlDb(services);
        }

        protected async Task ResetSqlDb(IServiceProvider services)
        {
            var config = services.GetRequiredService<IConfiguration>();

            var respwan = await Respawn.Respawner.CreateAsync(config.GetConnectionString("Default")!, new Respawn.RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                  "sysdiagrams",
                  "tblUser",
                  "tblObjectType",
                  "__EFMigrationsHistory"
                },
                SchemasToInclude = new string[]
                {
                    "Accounting"
                }

            });

            await respwan.ResetAsync(config.GetConnectionString("Default")!);
        }
    }
}
