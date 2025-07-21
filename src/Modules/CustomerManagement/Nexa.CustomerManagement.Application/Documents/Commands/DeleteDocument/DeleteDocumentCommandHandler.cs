//using MediatR;
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
//using Nexa.CustomerManagement.Shared.Enums;
//namespace Nexa.CustomerManagement.Application.Documents.Commands.DeleteDocument
//{
//    public class DeleteDocumentCommandHandler : IApplicationRequestHandler<DeleteDocumentCommand, Unit>
//    {
//        private readonly ICustomerManagementRepository<Customer> _customerRepository;
//        private readonly ICustomerManagementRepository<CustomerApplication> _customerApplicationRepository;
//        private readonly IApplicationAuthorizationService _applicationAuthorizationService;
//        private readonly IDocumentAttachementResponseFactory _documentAttachementResponseFactory;
//        private readonly ISecurityContext _securityContext;
//        private readonly IKYCProvider _kycProvider;

//        public DeleteDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<CustomerApplication> customerApplicationRepository, IApplicationAuthorizationService applicationAuthorizationService, IDocumentAttachementResponseFactory documentAttachementResponseFactory, ISecurityContext securityContext, IKYCProvider kycProvider)
//        {
//            _customerRepository = customerRepository;
//            _customerApplicationRepository = customerApplicationRepository;
//            _applicationAuthorizationService = applicationAuthorizationService;
//            _documentAttachementResponseFactory = documentAttachementResponseFactory;
//            _securityContext = securityContext;
//            _kycProvider = kycProvider;
//        }

//        public async Task<Result<Unit>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
//        {
//            string userId = _securityContext.User!.Id;

//            var currentCustomerExist = await _customerRepository.AnyAsync(x => x.UserId == userId);

//            if (!currentCustomerExist)
//            {
//                return new Result<Unit>(new BusinessLogicException("Current user should complete thier customer application first before creating kyc document."));
//            }

//            var customerApplication = await _customerApplicationRepository.AsQuerable()
//                .Include(x => x.Documents)
//                .SingleOrDefaultAsync(x => x.Id == request.CustomerApplicationId);

//            if (customerApplication == null)
//            {
//                return new Result<Unit>(new EntityNotFoundException(typeof(CustomerApplication), request.CustomerApplicationId));
//            }


//            if (customerApplication.Status != CustomerApplicationStatus.Draft)
//            {
//                return new Result<Unit>(new BusinessLogicException("Customer application must be in draft state to be able to attach document."));
//            }


//            var document = customerApplication.FindDocument(request.DocumentId);

//            if (document == null)
//            {
//                return new Result<Unit>(new EntityNotFoundException(typeof(Document), request.DocumentId));
//            }


//            customerApplication.RemoveDocument(document);

//            await _kycProvider.DeleteDocumentAsync(document.KYCExternalId);

//            await _customerApplicationRepository.UpdateAsync(customerApplication);

//            return Unit.Value;
//        }


//    }
//}
