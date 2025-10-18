using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.FundingResources;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.Accounting.Application.Tests.Tokens.Commands
{
    [TestFixture]
    public class CompleteLinkTokenCommandHandlerTests : TokenTestFixture
    {
        protected IAccountingRepository<BankAccount> BankAccountRepository { get; }

        public CompleteLinkTokenCommandHandlerTests()
        {
            BankAccountRepository = ServiceProvider.GetRequiredService<IAccountingRepository<BankAccount>>();
        }


        [Test]
        public async Task Should_complete_link_token_and_create_bank_account()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var customer = FakeCustomerService.CreateRandomCustomer(userId, VerificationState.Verified);

            var command = new CompleteLinkTokenCommand { Token = Guid.NewGuid().ToString() };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var bankAccount = await BankAccountRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            bankAccount.Should().NotBeNull();
        }

        [Test]
        public async Task Should_failure_while_completing_link_token_when_user_is_not_authorized()
        {
            var command = new CompleteLinkTokenCommand { Token = Guid.NewGuid().ToString() };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));

        }

        [Test]
        public async Task Should_failure_while_completing_link_token_when_user_customer_is_not_created()
        {
            AuthenticationService.Login();

            var command = new CompleteLinkTokenCommand { Token = Guid.NewGuid().ToString() };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException));

        }

        [Test]
        public async Task Should_failure_while_completing_link_token_when_user_customer_is_not_verified()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var customer = FakeCustomerService.CreateRandomCustomer(userId);

            var command = new CompleteLinkTokenCommand { Token = Guid.NewGuid().ToString() };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));

        }
    }
}
