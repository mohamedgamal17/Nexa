using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateDocument
{
    public class UpdateDocumentCommandHandler : IApplicationRequestHandler<UpdateDocumentCommand, CustomerDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly IKYCProvider _kycProvider;
        private readonly ISecurityContext _securityContext;
        private readonly ICustomerResponseFactory _customerResponseFactory;

        public UpdateDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, IKYCProvider kycProvider, ISecurityContext securityContext, ICustomerResponseFactory customerResponseFactory)
        {
            _customerRepository = customerRepository;
            _kycProvider = kycProvider;
            _securityContext = securityContext;
            _customerResponseFactory = customerResponseFactory;
        }

        public async Task<Result<CustomerDto>> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(customer == null)
            {
                return new EntityNotFoundException(GlobalErrorConsts.ResourceNotFound);
            }

            if(customer.Info == null)
            {
                return new BusinessLogicException(CustomerErrorConsts.IncompleteCustomerInfo);
            }

            var kycResult = await CreateOrUpdateKycDocument(customer, request);

            if (kycResult.IsFailure)
            {
                return new Result<CustomerDto>(kycResult.Exception!);
            }

            var kycDocument = kycResult.Value!;

            var document = Document.Create(request.Type, request.IssuingCountry, kycDocument.Id);

            if(kycDocument.Attachements != null)
            {
                kycDocument.Attachements.ForEach((kycAttachment) =>
                {
                    var documentAttachment = new DocumentAttachment(
                                kycAttachment.Id,
                                kycAttachment.FileName,
                                kycAttachment.Size,
                                kycAttachment.ContentType,
                                kycAttachment.Side
                            );


                    document.AddAttachment(documentAttachment);
                });
            }
            customer.UpdateDocument(document);

            await _customerRepository.UpdateAsync(customer);

            return await _customerResponseFactory.PrepareDto(customer);
        }

        private async Task<Result<KYCDocument>> CreateOrUpdateKycDocument(Customer customer , UpdateDocumentCommand command)
        {
            if (command.KycDocumentId != null)
            {
                var kycDocument = await _kycProvider.GetDocumentAsync(command.KycDocumentId);

                if(!IsKycDocumentOwner(kycDocument, customer))
                {
                    return new ForbiddenAccessException(GlobalErrorConsts.ForbiddenAccess);
                }

                return kycDocument;
            }

            var kycRequest = PrepareKycDocumentRequest(customer.KycCustomerId!, command);

            if(customer.Document != null)
            {
               return await _kycProvider.UpdateDocumentAsync(customer.Document.KycDocumentId, kycRequest);
            }
            else
            {
                return await _kycProvider.CreateDocumentAsync(kycRequest);
            }

        }

        private KYCDocumentRequest PrepareKycDocumentRequest(string clientId ,UpdateDocumentCommand command)
        {
            var request = new KYCDocumentRequest
            {
                ClientId = clientId,
                Type = command.Type,
                IssuingCountry = command.IssuingCountry
            };

            return request;
        }

        private bool IsKycDocumentOwner(KYCDocument kycDocument , Customer customer)
        {
            return kycDocument.ClientId == customer.KycCustomerId;
        }
    }
}
