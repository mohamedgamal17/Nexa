using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Helpers;
using Nexa.CustomerManagement.Application.KYC.Dtos;
using Nexa.CustomerManagement.Application.KYC.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.KYC;

namespace Nexa.CustomerManagement.Application.KYC.Commands.UploadKYCDocumentAttachment
{
    public class UploadKYCDocumentAttachmentCommandHandler : IApplicationRequestHandler<UploadKYCDocumentAttachmentCommand, KYCDocumentAttachementDto>
    {
        private readonly ICustomerManagementRepository<KYCDocument> _kycDocumentRepository;
        private readonly ICustomerManagementRepository<KYCDocumentAttachment> _kycDocumentAttachmentRepository;
        private readonly IKYCDocumentAttachementResponseFactory _kycDocumentAttachementResponseFactory;
        private readonly ISecurityContext _securityContext;

        public UploadKYCDocumentAttachmentCommandHandler(ICustomerManagementRepository<KYCDocument> kycDocumentRepository, ICustomerManagementRepository<KYCDocumentAttachment> kycDocumentAttachmentRepository, IKYCDocumentAttachementResponseFactory kycDocumentAttachementResponseFactory, ISecurityContext securityContext)
        {
            _kycDocumentRepository = kycDocumentRepository;
            _kycDocumentAttachmentRepository = kycDocumentAttachmentRepository;
            _kycDocumentAttachementResponseFactory = kycDocumentAttachementResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<KYCDocumentAttachementDto>> Handle(UploadKYCDocumentAttachmentCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var kycDocument = await _kycDocumentRepository.SingleOrDefaultAsync(x => x.Id == request.KYCDocumentId);

            if(kycDocument == null)
            {
                return new Result<KYCDocumentAttachementDto>(new EntityNotFoundException(typeof(KYCDocument), request.KYCDocumentId));
            }

            var isKYCDocumentOwner = IsKYCDocumentOwner(userId, kycDocument);

            if (!isKYCDocumentOwner)
            {
                return new Result<KYCDocumentAttachementDto>(new ForbiddenAccessException());
            }

            if (kycDocument.IsActive)
            {
                return new Result<KYCDocumentAttachementDto>(new BusinessLogicException("Cannot delete active processing kyc document."));
            }

            if (kycDocument.Status == KYCStatus.Approved)
            {
                return new Result<KYCDocumentAttachementDto>(new BusinessLogicException("Cannot delete approved kyc document."));
            }

            string extensions = Base64ImageHelper.GetImageExtension(request.Data)!;

            string fileName = $"{DateTime.Now.Ticks}.{extensions}";

            var size = request.Data.Length;

            var contentType = $"image/${extensions.Replace(".", "")}";

            var attachment = new KYCDocumentAttachment(fileName, size, contentType, Guid.NewGuid().ToString());

            kycDocument.AddAttachment(attachment);

            await _kycDocumentRepository.UpdateAsync(kycDocument);

            var response = await _kycDocumentAttachmentRepository.SingleAsync(x => x.Id == kycDocument.Id);


            return await _kycDocumentAttachementResponseFactory.PrepareDto(response);

        }

        private bool IsKYCDocumentOwner(string userId, KYCDocument kYCDocument)
        {
            return userId == kYCDocument.Id;
        }
    }
}
