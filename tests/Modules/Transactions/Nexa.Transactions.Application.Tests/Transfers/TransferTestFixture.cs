using Bogus;
using Bogus.Extensions.UnitedStates;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.FundingResources.Dtos;
using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Enums;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.Transactions.Application.Tests.Fakers;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Enums;
namespace Nexa.Transactions.Application.Tests.Transfers
{
    public class TransferTestFixture : TransactionsTestFixture
    {
        protected FakeWalletService FakeWalletService { get; }
  
        protected ITransferRepository TransferRepository { get; }

        protected FakeCustomerService FakeCustomerService { get; }
        protected Faker Faker { get; }
        protected FakeFundingResourceService FakeFundingResourceService { get; }

        public TransferTestFixture()
        {
            FakeWalletService = ServiceProvider.GetRequiredService<FakeWalletService>();
            TransferRepository = ServiceProvider.GetRequiredService<ITransferRepository>();
            FakeFundingResourceService = ServiceProvider.GetRequiredService<FakeFundingResourceService>();
            FakeCustomerService = ServiceProvider.GetRequiredService<FakeCustomerService>();
            Faker = new Faker();
        }
  
        public async Task<NetworkTransfer> CreateNetworkTransferAsync(string userId,string walletId , string reciverId , decimal amount)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<NetworkTransfer>>();

                var networkTransfer = new NetworkTransfer(userId, walletId, reciverId, Guid.NewGuid().ToString(), amount);

                return await transferRepository.InsertAsync(networkTransfer);
            });
        }

        public async Task<NetworkTransfer> CreateProcessNetworkTransferAsync(string userId, string walletId, string reciverId, decimal amount)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<NetworkTransfer>>();

                var networkTransfer = await CreateNetworkTransferAsync(userId, walletId, reciverId, amount);

                networkTransfer = await transferRepository.SingleAsync(x => x.Id == networkTransfer.Id);

                networkTransfer.Process();

                return await transferRepository.UpdateAsync(networkTransfer);
            });
        }

        


        public async Task<NetworkTransfer> CreateCompleteNetworkTransferAsync(string userId, string walletId, string reciverId, decimal amount)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<NetworkTransfer>>();

                var networkTransfer = await CreateProcessNetworkTransferAsync(userId, walletId, reciverId, amount);

                networkTransfer = await transferRepository.SingleAsync(x => x.Id == networkTransfer.Id);

                networkTransfer.Complete();

                return await transferRepository.UpdateAsync(networkTransfer);
            });
        }
        public async Task<BankTransfer> CreateBankTransferAsync(string userId , string walletId , string fundingResourceId  ,decimal amount , TransferDirection direction)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<BankTransfer>>();

                BankTransfer bankTransfer = new BankTransfer(userId, walletId, Guid.NewGuid().ToString(), amount, fundingResourceId, direction, BankTransferType.Ach);

                return await transferRepository.InsertAsync(bankTransfer);
            });
        }


        public async Task<BankTransfer> CreateProcessBankTransferAsync(string userId, string walletId, string fundingResourceId, decimal amount, TransferDirection direction)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<BankTransfer>>();

                var bankTransfer = await CreateBankTransferAsync(userId, walletId, fundingResourceId, amount, direction);

                bankTransfer = await transferRepository.SingleAsync(x => x.Id == bankTransfer.Id);

                bankTransfer.Process();

                await transferRepository.UpdateAsync(bankTransfer);

                return bankTransfer;
            });
        }

        public async Task<BankTransfer> CreateProcessBankTransferWithExternalIdAsync(string userId, string walletId, string fundingResourceId, decimal amount, TransferDirection direction)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<BankTransfer>>();

                var bankTransfer = await CreateProcessBankTransferAsync(userId, walletId, fundingResourceId, amount, direction);

                bankTransfer = await transferRepository.SingleAsync(x => x.Id == bankTransfer.Id);

                bankTransfer.AssignExternalTransferId(Guid.NewGuid().ToString());

                await transferRepository.UpdateAsync(bankTransfer);

                return bankTransfer;
            });
        }

        public async Task<BankTransfer> CreateCompleteBankTransferAsync(string userId, string walletId, string fundingResourceId, decimal amount, TransferDirection direction)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<BankTransfer>>();

                var bankTransfer = await CreateProcessBankTransferAsync(userId, walletId, fundingResourceId, amount, direction);

                bankTransfer = await transferRepository.SingleAsync(x => x.Id == bankTransfer.Id);

                bankTransfer.Complete();

                await transferRepository.UpdateAsync(bankTransfer);

                return bankTransfer;
            });
        }

        public async Task<WalletDto> CreateWalletAsync(string? userId = null, decimal balance = 1000, WalletState walletState = WalletState.Active)
        {
            var dto = new WalletDto
            {
                Id = Guid.NewGuid().ToString(),
                Balance = balance,
                UserId = userId ?? Guid.NewGuid().ToString(),
                Number = Guid.NewGuid().ToString(),
                ProviderWalletId = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                State = walletState
            };

            return await FakeWalletService.AddWalletAsync(dto);
        }
        public async Task<BankAccountDto> CreateFundingResourceAsync(string? userId = null )
        {
            var dto = new BankAccountDto
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId ?? Guid.NewGuid().ToString(),
                BankName = Guid.NewGuid().ToString(),
                AccountNumberLast4 = "4444",
                RoutingNumber = "4444444444",
                ProviderBankAccountId = Guid.NewGuid().ToString(),
                HolderName = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                Currency = "usd",
                Country = "us"
            };

            return await FakeFundingResourceService.AddFundingResource(dto);
        }

        public async Task<CustomerDto> CreateCustomerAsync(string? userId = null)
        {
            var dto = new CustomerDto
            {
                Id = Guid.NewGuid().ToString(),
                FintechCustomerId = Guid.NewGuid().ToString(),
                UserId = userId ?? Guid.NewGuid().ToString(),
                PhoneNumber = "+13462127336",
                State = VerificationState.Verified,
                KycCustomerId = Guid.NewGuid().ToString(),
                EmailAddress = "test@test.com",
                Info = new CustomerInfoDto
                {
                    FirstName = Faker.Person.FirstName,
                    LastName = Faker.Person.LastName,
                    Nationality = "us",
                    IdNumber = Faker.Person.Ssn(),
                    Gender = Gender.Male,
                    BirthDate = Faker.Person.DateOfBirth,
                    Address = new AddressDto
                    {
                        Country = "us",
                        City = Faker.Person.Address.City,
                        State = Faker.Person.Address.State,
                        StreetLine = Faker.Person.Address.Street,
                        PostalCode = Faker.Person.Address.ZipCode,
                        ZipCode = Faker.Person.Address.ZipCode
                    }
                },

            };

            return await FakeCustomerService.AddCustomerAsync(dto);
        }

    }
}
