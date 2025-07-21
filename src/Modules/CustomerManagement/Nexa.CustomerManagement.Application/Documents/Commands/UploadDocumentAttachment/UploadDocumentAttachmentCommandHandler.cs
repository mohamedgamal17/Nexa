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
//namespace Nexa.CustomerManagement.Application.Documents.Commands.UploadDocumentAttachment
//{
//    public class UploadDocumentAttachmentCommandHandler : IApplicationRequestHandler<UploadDocumentAttachmentCommand, DocumentAttachementDto>
//    {
//        private readonly ICustomerManagementRepository<Customer> _customerRepository;
//        private readonly ICustomerManagementRepository<CustomerApplication> _customerApplicationRepository;
//        private readonly IApplicationAuthorizationService _applicationAuthorizationService;
//        private readonly IDocumentAttachementResponseFactory _documentAttachementResponseFactory;
//        private readonly ISecurityContext _securityContext;
//        private readonly IKYCProvider _kycProvider;

//        public UploadDocumentAttachmentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<CustomerApplication> customerApplicationRepository, IApplicationAuthorizationService applicationAuthorizationService, IDocumentAttachementResponseFactory documentAttachementResponseFactory, ISecurityContext securityContext, IKYCProvider kycProvider)
//        {
//            _customerRepository = customerRepository;
//            _customerApplicationRepository = customerApplicationRepository;
//            _applicationAuthorizationService = applicationAuthorizationService;
//            _documentAttachementResponseFactory = documentAttachementResponseFactory;
//            _securityContext = securityContext;
//            _kycProvider = kycProvider;
//        }

//        public async Task<Result<DocumentAttachementDto>> Handle(UploadDocumentAttachmentCommand request, CancellationToken cancellationToken)
//        {
//            string userId = _securityContext.User!.Id;

//            var currentCustomerExist = await _customerRepository.AnyAsync(x => x.UserId == userId);

//            if (!currentCustomerExist)
//            {
//                return new Result<DocumentAttachementDto>(new BusinessLogicException("Current user should complete thier customer application first before creating kyc document."));
//            }

//            var customerApplication = await _customerApplicationRepository.AsQuerable()
//                .Include(x => x.Documents)
//                .SingleOrDefaultAsync(x => x.Id == request.CustomerApplicationId);

//            if (customerApplication == null)
//            {
//                return new Result<DocumentAttachementDto>(new EntityNotFoundException(typeof(CustomerApplication), request.CustomerApplicationId));
//            }

        

//            if (customerApplication.Status != CustomerApplicationStatus.Draft)
//            {
//                return new Result<DocumentAttachementDto>(new BusinessLogicException("Customer application must be in draft state to be able to attach document."));
//            }


//            var document = customerApplication.FindDocument(request.DocumentId);

//            if (document == null)
//            {
//                return new Result<DocumentAttachementDto>(new EntityNotFoundException(typeof(Document), request.DocumentId));
//            }

//            string extensions = request.Data.FileName.Split(".")[1];

//            string fileName = $"{DateTime.Now.Ticks}.{extensions}";

//            var kycRequest = PrepareKYCDocumentAttachement(fileName,request);

//            var kycResponse = await _kycProvider.UploadDocumentAttachementAsync(document.KYCExternalId, kycRequest);

//            var attachment = new DocumentAttachment(kycResponse.Id,fileName, kycResponse.Size, kycResponse.ContentType, request.Side);

//            document.AddAttachment(attachment);

//            await _customerApplicationRepository.UpdateAsync(customerApplication);

//            return await _documentAttachementResponseFactory.PrepareDto(attachment);
//        }

//        private KYCDocumentAttachmentRequest PrepareKYCDocumentAttachement(string fileName,UploadDocumentAttachmentCommand command) 
//        {
//            var imageStream = new MemoryStream();

//            command.Data.CopyTo(imageStream);

//            var request = new KYCDocumentAttachmentRequest
//            {
//                FileName = fileName,
//                Data = imageStream,
//                Side = command.Side
//            };

//            return request;
//        }
//    }
//}
