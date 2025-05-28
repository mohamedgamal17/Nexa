using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Documents.Dtos;
using Nexa.CustomerManagement.Application.Documents.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.KYC;
namespace Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument
{
    public class CreateDocumentCommandHandler : IApplicationRequestHandler<CreateDocumentCommand, DocumentDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerManagementRepository<Document> _documentRepository;
        private readonly ISecurityContext _securityContext;
        private readonly IDocumentResponseFactory _documentResponseFactory;
        private readonly IKYCProvider _kycProvider;

        public CreateDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<Document> documentRepository, ISecurityContext securityContext, IDocumentResponseFactory documentResponseFactory, IKYCProvider kycProvider)
        {
            _customerRepository = customerRepository;
            _documentRepository = documentRepository;
            _securityContext = securityContext;
            _documentResponseFactory = documentResponseFactory;
            _kycProvider = kycProvider;
        }

        public async Task<Result<DocumentDto>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = _securityContext.User!.Id;

            var currentCustomer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (currentCustomer == null)
            {
                return new Result<DocumentDto>(new BusinessLogicException("Current user should complete thier customer application first before creating kyc document."));
            }

            var isUserHasActiveKYC = await _documentRepository.AnyAsync(x => x.IsActive);

            if (isUserHasActiveKYC)
            {
                return new Result<DocumentDto>(new BusinessLogicException("Current user has already active kyc document verification , please wait unitl verification process complete."));
            }

            var isUserHasApprovedKYC = await _documentRepository.AnyAsync(x => x.Status == Status.Approved);

            if (isUserHasApprovedKYC)
            {
                return new Result<DocumentDto>(new BusinessLogicException("Current user has already approved kyc document."));
            }

            var kycReqeust = PrepareKYCDocumentRequest(currentCustomer.ExternalId, request);

            var kycResponse = await _kycProvider.CreateDocumentAsync(kycReqeust);

            var kycDocument = new Document(currentCustomer.Id, userId, request.IssuingCountry, kycResponse.Id, request.Type);

            await _documentRepository.InsertAsync(kycDocument);

            var response = await _documentRepository.SingleAsync(x => x.Id == kycDocument.Id);

            return await _documentResponseFactory.PrepareDto(response);
        }

        private KYCDocumentRequest PrepareKYCDocumentRequest(string clientId,CreateDocumentCommand command)
        {
            var request = new KYCDocumentRequest
            {
                ClientId = clientId,
                IssuingCountry = command.IssuingCountry,
                Type = command.Type

            };

            return request;
        }
    }
}
