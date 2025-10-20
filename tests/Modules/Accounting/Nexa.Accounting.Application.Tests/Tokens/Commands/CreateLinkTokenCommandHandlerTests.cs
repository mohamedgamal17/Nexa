using FluentAssertions;
using Nexa.Accounting.Application.Tokens.Commands.CreateLinkToken;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.Integrations.OpenBanking.Abstractions.Consts;

namespace Nexa.Accounting.Application.Tests.Tokens.Commands
{
    [TestFixture]
    public class CreateLinkTokenCommandHandlerTests : TokenTestFixture
    {
        [Test]
        public async Task Should_create_link_token()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var customer = FakeCustomerService.CreateRandomCustomer(userId, CustomerStatus.Verified);

            var command = new CreateLinkTokenCommand { RedirectUri = "https://test.com/" };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            result.Value!.Token.Should().NotBeEmpty();
        }

        [Test]
        public async Task Should_failure_while_creating_link_token_when_user_is_not_authorized()
        {
            var command = new CreateLinkTokenCommand { RedirectUri = "https://test.com/" };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(NexaUnauthorizedAccessException), GlobalErrorConsts.UnauthorizedAccess);
        }

        [Test]
        public async Task Should_failure_while_creating_link_token_when_current_user_dose_not_complete_customer_profile()
        {
            AuthenticationService.Login();

            var command = new CreateLinkTokenCommand { RedirectUri = "https://test.com/" };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException) , CustomerErrorConsts.CustomerNotExist);
        }

        [Test]
        public async Task Should_failure_while_creating_link_token_when_current_cutomer_is_not_verified()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var customer = FakeCustomerService.CreateRandomCustomer(userId);

            var command = new CreateLinkTokenCommand { RedirectUri = "https://test.com/" };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException), CustomerErrorConsts.CustomerNotVerified);
        }
    }
}
