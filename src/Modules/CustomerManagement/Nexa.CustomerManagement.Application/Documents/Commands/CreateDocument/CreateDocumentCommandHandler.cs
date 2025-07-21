//using Microsoft.EntityFrameworkCore;
//using Nexa.BuildingBlocks.Application.Abstractions.Security;
//using Nexa.BuildingBlocks.Application.Requests;
//using Nexa.BuildingBlocks.Domain.Exceptions;
//using Nexa.BuildingBlocks.Domain.Results;
//using Nexa.CustomerManagement.Application.Documents.Factories;
//using Nexa.CustomerManagement.Domain;
//using Nexa.CustomerManagement.Domain.CustomerApplications;
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
//        private readonly IApplicationAuthorizationService _applicationAuthorizationService;

//        public CreateDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository , ICustomerManagementRepository<Document> documentRepository, ISecurityContext securityContext, IDocumentResponseFactory documentResponseFactory, IKYCProvider kycProvider, IApplicationAuthorizationService applicationAuthorizationService)
//        {
//            _customerRepository = customerRepository;
//            _documentRepository = documentRepository;
//            _securityContext = securityContext;
//            _documentResponseFactory = documentResponseFactory;
//            _kycProvider = kycProvider;
//            _applicationAuthorizationService = applicationAuthorizationService;
//        }

//        public async Task<Result<DocumentDto>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
//        {
//            var userId = _securityContext.User!.Id;

//            var currentCustomerExist = await _customerRepository.AnyAsync(x => x.UserId == userId);

//            if (!currentCustomerExist)
//            {
//                return new Result<DocumentDto>(new BusinessLogicException("Current user should complete thier customer application first before creating kyc document."));
//            }


            
//            if(customerApplication.Status != CustomerApplicationStatus.Draft)
//            {
//                return new Result<DocumentDto>(new BusinessLogicException("Customer application must be in draft state to be able to attach document."));
//            }


//            var kycReqeust = PrepareKYCDocumentRequest(customerApplication.KycExternalId, request);

//            var kycResponse = await _kycProvider.CreateDocumentAsync(kycReqeust);

//            var kycDocument = new Document(request.IssuingCountry, kycResponse.Id, request.Type);

//            customerApplication.AddDocument(kycDocument);

//            await _customerApplicationRepository.UpdateAsync(customerApplication);

//            var response = await _documentRepository.SingleAsync(x => x.Id == kycDocument.Id);

//            return await _documentResponseFactory.PrepareDto(response);
//        }

//        private KYCDocumentRequest PrepareKYCDocumentRequest(string kycClientId, CreateDocumentCommand command)
//        {
//            var request = new KYCDocumentRequest
//            {
//                ClientId = kycClientId,
//                IssuingCountry = command.IssuingCountry,
//                Type = command.Type

//            };

//            return request;
//        }
//    }
//}
