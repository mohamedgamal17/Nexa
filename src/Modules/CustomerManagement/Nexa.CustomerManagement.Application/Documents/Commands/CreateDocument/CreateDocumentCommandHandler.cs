//using Microsoft.EntityFrameworkCore;
//using Nexa.BuildingBlocks.Application.Abstractions.Security;
//using Nexa.BuildingBlocks.Application.Requests;
//using Nexa.BuildingBlocks.Domain.Exceptions;
//using Nexa.BuildingBlocks.Domain.Results;
//using Nexa.CustomerManagement.Application.Documents.Factories;
//using Nexa.CustomerManagement.Domain;
//using Nexa.CustomerManagement.Domain.Customers;
//using Nexa.CustomerManagement.Domain.Documents;
//using Nexa.CustomerManagement.Domain.KYC;
//using Nexa.CustomerManagement.Shared.Dtos;
//using Nexa.CustomerManagement.Shared.Enums;
//namespace Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument
//{
//    public class CreateDocumentCommandHandler : IApplicationRequestHandler<CreateDocumentCommand, DocumentDto>
//    {
//        private readonly ICustomerManagementRepository<Customer> _customerRepository;
//        private readonly ICustomerManagementRepository<Document> _documentRepository;
//        private readonly ISecurityContext _securityContext;
//        private readonly IDocumentResponseFactory _documentResponseFactory;
//        private readonly IKYCProvider _kycProvider;

//        public CreateDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<Document> documentRepository, ISecurityContext securityContext, IDocumentResponseFactory documentResponseFactory, IKYCProvider kycProvider)
//        {
//            _customerRepository = customerRepository;
//            _documentRepository = documentRepository;
//            _securityContext = securityContext;
//            _documentResponseFactory = documentResponseFactory;
//            _kycProvider = kycProvider;
//        }

//        public async Task<Result<DocumentDto>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
//        {
//            var userId = _securityContext.User!.Id;

//            var customer = await _customerRepository
//                .AsQuerable()
//                .Include(x=> x.Documents)
//                .ThenInclude(x=> x.Attachments)
//                .SingleOrDefaultAsync(x => x.UserId == userId);
            
//            if (customer == null)
//            {
//                return new Result<DocumentDto>(new BusinessLogicException("Current user customer is not exist."));
//            }

//            if(customer.InfoVerificationState != VerificationState.Verified)
//            {
//                return new Result<DocumentDto>(new BusinessLogicException("Can not accept documents until the customer peronal info is verified"));
//            }

//            var document = new Document(request.Type);

//            if(request.KycDocumentId != null)
//            {
//                var kycDocument = await _kycProvider.GetDocumentAsync(request.KycDocumentId);

//                if(!IsKycDocumentOwner(kycDocument, customer))
//                {
//                    return new Result<DocumentDto>(new ForbiddenAccessException());
//                }

//                document.AddKycExternalId(kycDocument.Id);

//                if(kycDocument.Attachements != null)
//                {
//                    kycDocument.Attachements.ForEach((attach) =>
//                    {
//                        var documentAttachment = new DocumentAttachment(
//                                attach.Id,
//                                attach.FileName,
//                                attach.Size,
//                                attach.ContentType,
//                                attach.Side
//                            );


//                        document.AddAttachment(documentAttachment);
//                    });
//                }
//            }
//            else
//            {
//                var kycDocumentRequest = new KYCDocumentRequest
//                {
//                    ClientId = customer.KycCustomerId!,
//                    Type = request.Type
//                };

//                var kycDocument = await _kycProvider.CreateDocumentAsync(kycDocumentRequest);

//                document.AddKycExternalId(kycDocument.Id);
//            }


//            customer.AddDocument(document);

//            await _customerRepository.UpdateAsync(customer);

//            var data = await _documentRepository.AsQuerable()
//                .Include(x => x.Attachments)
//                .SingleAsync(x => x.Id == document.Id);

//            return await _documentResponseFactory.PrepareDto(data);

//        }

//        public bool IsKycDocumentOwner(KYCDocument kYCDocument , Customer customer)
//        {
//            return kYCDocument.ClientId == customer.KycCustomerId;
//        }
//    }
//}
