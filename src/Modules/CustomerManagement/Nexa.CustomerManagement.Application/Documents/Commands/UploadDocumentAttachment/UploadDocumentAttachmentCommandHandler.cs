using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Documents.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Documents.Commands.UploadDocumentAttachment
{
    public class UploadDocumentAttachmentCommandHandler : IApplicationRequestHandler<UploadDocumentAttachmentCommand, DocumentAttachementDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly IDocumentAttachementResponseFactory _documentAttachementResponseFactory;
        private readonly ISecurityContext _securityContext;
        private readonly IKYCProvider _kycProvider;

        public UploadDocumentAttachmentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, IDocumentAttachementResponseFactory documentAttachementResponseFactory, ISecurityContext securityContext, IKYCProvider kycProvider)
        {
            _customerRepository = customerRepository;
            _documentAttachementResponseFactory = documentAttachementResponseFactory;
            _securityContext = securityContext;
            _kycProvider = kycProvider;
        }

        public async Task<Result<DocumentAttachementDto>> Handle(UploadDocumentAttachmentCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository
                .AsQuerable()
                .Include(x => x.Documents)
                .ThenInclude(x => x.Attachments)
                .SingleOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return new Result<DocumentAttachementDto>(new BusinessLogicException("Current user customer is not exist."));
            }

            var document = customer.FindDocument(request.DocumentId);

            if(document == null)
            {
                return new Result<DocumentAttachementDto>(new EntityNotFoundException(typeof(Document)  ,request.DocumentId));
            }

            if (document.State == VerificationState.InReview 
                || document.State == VerificationState.Verified)
            {
                return new Result<DocumentAttachementDto>(new BusinessLogicException("Invalid document state."));
            }

            string extensions = request.Data.FileName.Split(".")[1];

            string fileName = $"{DateTime.Now.Ticks}.{extensions}";

            var attachmentRequest = PrepareKYCDocumentAttachement(fileName, request);

            var kycResponse = await _kycProvider.UploadDocumentAttachementAsync(document.KYCExternalId!, attachmentRequest);

            var attachment = new DocumentAttachment(kycResponse.Id, fileName, kycResponse.Size, kycResponse.ContentType, request.Side);

            document.AddAttachment(attachment);

            await _customerRepository.UpdateAsync(customer);

            return await _documentAttachementResponseFactory.PrepareDto(attachment);
        }

        private KYCDocumentAttachmentRequest PrepareKYCDocumentAttachement(string fileName, UploadDocumentAttachmentCommand command)
        {
            var imageStream = new MemoryStream();

            command.Data.CopyTo(imageStream);

            var request = new KYCDocumentAttachmentRequest
            {
                FileName = fileName,
                Data = imageStream,
                Side = command.Side
            };

            return request;
        }
    }
}
