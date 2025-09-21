using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Tokens.Dtos;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;

namespace Nexa.CustomerManagement.Application.Tokens.Commands
{
    public class CreateKycTokenCommandHandler : IApplicationRequestHandler<CreateKycSdkTokenCommand, TokenDto>
    {
        private readonly IKYCProvider _kycProvider;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ISecurityContext _securtiyContext;

        public CreateKycTokenCommandHandler(IKYCProvider kycProvider, ICustomerManagementRepository<Customer> customerRepository, ISecurityContext securtiyContext)
        {
            _kycProvider = kycProvider;
            _customerRepository = customerRepository;
            _securtiyContext = securtiyContext;
        }

        public async Task<Result<TokenDto>> Handle(CreateKycSdkTokenCommand request, CancellationToken cancellationToken)
        {
            string userId = _securtiyContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return new Result<TokenDto>(new BusinessLogicException("User should complete customer info first."));
            }

            if (customer.KycCustomerId == null)
            {
                return new Result<TokenDto>(new BusinessLogicException("Customer info is incomplete."));
            }
            var sdkTokenRequest = new KYCSdkTokenRequest
            {
                ClientId = customer.KycCustomerId,
                Referrer = request.Referrer!
            };

            var response = await _kycProvider.CreateSdkToken(sdkTokenRequest);

            var result = new TokenDto { Token = response };

            return result;
        }
    }
}
