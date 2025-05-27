using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Documents.Dtos;
using Nexa.CustomerManagement.Application.Documents.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument
{
    public class CreateDocumentCommandHandler : IApplicationRequestHandler<CreateDocumentCommand, DocumentDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerManagementRepository<Document> _documentRepository;
        private readonly ISecurityContext _securityContext;
        private readonly IDocumentResponseFactory _documentResponseFactory;

        public CreateDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<Document> documentRepository, ISecurityContext securityContext, IDocumentResponseFactory documentResponseFactory)
        {
            _customerRepository = customerRepository;
            _documentRepository = documentRepository;
            _securityContext = securityContext;
            _documentResponseFactory = documentResponseFactory;
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

            string externalId = Guid.NewGuid().ToString(); // will be replaced with real exterenal id for 3rd party provider

            var kycDocument = new Document(currentCustomer.Id, userId, request.IssuingCountry, externalId, request.Type);

            await _documentRepository.InsertAsync(kycDocument);

            var response = await _documentRepository.SingleAsync(x => x.Id == kycDocument.Id);

            return await _documentResponseFactory.PrepareDto(response);
        }
    }
}
