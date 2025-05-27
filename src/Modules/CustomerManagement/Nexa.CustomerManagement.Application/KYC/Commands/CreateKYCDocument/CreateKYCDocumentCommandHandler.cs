using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.KYC.Dtos;
using Nexa.CustomerManagement.Application.KYC.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
namespace Nexa.CustomerManagement.Application.KYC.Commands.CreateKYCDocument
{
    public class CreateKYCDocumentCommandHandler : IApplicationRequestHandler<CreateKYCDocumentCommand, KYCDocumentDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerManagementRepository<KYCDocument> _kYCDocumentRepository;
        private readonly ISecurityContext _securityContext;
        private readonly IKYCDocumentResponseFactory _kYCDocumentResponseFactory;

        public CreateKYCDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<KYCDocument> kYCDocumentRepository, ISecurityContext securityContext, IKYCDocumentResponseFactory kYCDocumentResponseFactory)
        {
            _customerRepository = customerRepository;
            _kYCDocumentRepository = kYCDocumentRepository;
            _securityContext = securityContext;
            _kYCDocumentResponseFactory = kYCDocumentResponseFactory;
        }

        public async Task<Result<KYCDocumentDto>> Handle(CreateKYCDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = _securityContext.User!.Id;

            var currentCustomer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(currentCustomer == null)
            {
                return new Result<KYCDocumentDto>(new BusinessLogicException("Current user should complete thier customer application first before creating kyc document."));
            }

            var isUserHasActiveKYC = await _kYCDocumentRepository.AnyAsync(x => x.IsActive );

            if (isUserHasActiveKYC)
            {
                return new Result<KYCDocumentDto>(new BusinessLogicException("Current user has already active kyc document verification , please wait unitl verification process complete."));
            }

            var isUserHasApprovedKYC = await _kYCDocumentRepository.AnyAsync(x => x.Status == KYCStatus.Approved);

            if (isUserHasApprovedKYC)
            {
                return new Result<KYCDocumentDto>(new BusinessLogicException("Current user has already approved kyc document."));
            }

            string externalId = Guid.NewGuid().ToString(); // will be replaced with real exterenal id for 3rd party provider

            var kycDocument = new KYCDocument(currentCustomer.Id, userId, request.IssuingCountry,externalId ,request.Type);

            await _kYCDocumentRepository.InsertAsync(kycDocument);

            var response = await _kYCDocumentRepository.SingleAsync(x => x.Id == kycDocument.Id);

            return await _kYCDocumentResponseFactory.PrepareDto(response);
        }
    }
}
