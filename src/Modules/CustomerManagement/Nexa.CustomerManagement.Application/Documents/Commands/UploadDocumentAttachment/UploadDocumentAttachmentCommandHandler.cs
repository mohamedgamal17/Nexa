using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Documents.Dtos;
using Nexa.CustomerManagement.Application.Documents.Factories;
using Nexa.CustomerManagement.Application.Helpers;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.KYC;

namespace Nexa.CustomerManagement.Application.Documents.Commands.UploadDocumentAttachment
{
    public class UploadDocumentAttachmentCommandHandler : IApplicationRequestHandler<UploadDocumentAttachmentCommand, DocumentAttachementDto>
    {
        private readonly ICustomerManagementRepository<Document> _documentRepository;
        private readonly ICustomerManagementRepository<DocumentAttachment> _documentAttachmentRepository;
        private readonly IDocumentAttachementResponseFactory _documentAttachementResponseFactory;
        private readonly ISecurityContext _securityContext;
        private readonly IKYCProvider _kycProvider;
        public UploadDocumentAttachmentCommandHandler(ICustomerManagementRepository<Document> documentRepository, ICustomerManagementRepository<DocumentAttachment> documentAttachmentRepository, IDocumentAttachementResponseFactory documentAttachementResponseFactory, ISecurityContext securityContext, IKYCProvider kycProvider)
        {
            _documentRepository = documentRepository;
            _documentAttachmentRepository = documentAttachmentRepository;
            _documentAttachementResponseFactory = documentAttachementResponseFactory;
            _securityContext = securityContext;
            _kycProvider = kycProvider;
        }

        public async Task<Result<DocumentAttachementDto>> Handle(UploadDocumentAttachmentCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var document = await _documentRepository.SingleOrDefaultAsync(x => x.Id == request.DocumentId);

            if (document == null)
            {
                return new Result<DocumentAttachementDto>(new EntityNotFoundException(typeof(Document), request.DocumentId));
            }

            var isDocumentOwner = IsDocumentOwner(userId, document);

            if (!isDocumentOwner)
            {
                return new Result<DocumentAttachementDto>(new ForbiddenAccessException());
            }

            if (document.IsActive)
            {
                return new Result<DocumentAttachementDto>(new BusinessLogicException("Cannot delete active processing kyc document."));
            }

            if (document.Status == DocumentStatus.Approved)
            {
                return new Result<DocumentAttachementDto>(new BusinessLogicException("Cannot delete approved kyc document."));
            }
            string extensions = request.Data.FileName.Split(".")[1];

            string fileName = $"{DateTime.Now.Ticks}.{extensions}";

            var kycRequest = PrepareKYCDocumentAttachement(fileName,request);

            var kycResponse = await _kycProvider.UploadDocumentAttachementAsync(document.KYCExternalId, kycRequest);

            var attachment = new DocumentAttachment(kycResponse.Id,fileName, kycResponse.Size, kycResponse.ContentType);

            document.AddAttachment(attachment);

            await _documentRepository.UpdateAsync(document);

            var response = await _documentAttachmentRepository.SingleAsync(x => x.Id == attachment.Id);

            return await _documentAttachementResponseFactory.PrepareDto(response);

        }

        private bool IsDocumentOwner(string userId, Document document)
        {
            return userId == document.UserId;
        }

        private KYCDocumentAttachmentRequest PrepareKYCDocumentAttachement(string fileName,UploadDocumentAttachmentCommand command) 
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
