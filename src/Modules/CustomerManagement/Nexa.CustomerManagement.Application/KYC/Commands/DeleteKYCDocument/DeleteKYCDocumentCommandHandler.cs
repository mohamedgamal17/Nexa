using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.KYC;
namespace Nexa.CustomerManagement.Application.KYC.Commands.DeleteKYCDocument
{
    public class DeleteKYCDocumentCommandHandler : IApplicationRequestHandler<DeleteKYCDocumentCommand, Unit>
    {
        private readonly ICustomerManagementRepository<KYCDocument> _kYCDocumentRepository;
        private readonly ISecurityContext _securityContext;
        public DeleteKYCDocumentCommandHandler(ICustomerManagementRepository<KYCDocument> kYCDocumentRepository,  ISecurityContext securityContext)
        {
            _kYCDocumentRepository = kYCDocumentRepository;
            _securityContext = securityContext;
        }

        public async Task<Result<Unit>> Handle(DeleteKYCDocumentCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var kycDocument = await _kYCDocumentRepository.SingleOrDefaultAsync(x => x.Id == request.Id);

            if(kycDocument == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(KYCDocument), request.Id));
            }

            var isKYCDocumentOwner = IsKYCDocumentOwner(userId, kycDocument);

            if (!isKYCDocumentOwner)
            {
                return new Result<Unit>(new ForbiddenAccessException());
            }

            if (kycDocument.IsActive)
            {
                return new Result<Unit>(new BusinessLogicException("Cannot delete active processing kyc document."));
            }

            if(kycDocument.Status == KYCStatus.Approved)
            {
                return new Result<Unit>(new BusinessLogicException("Cannot delete approved kyc document."));
            }

            await _kYCDocumentRepository.DeleteAsync(kycDocument);

            return Unit.Value;
        }

        private bool IsKYCDocumentOwner(string userId ,KYCDocument kYCDocument)
        {
            return userId == kYCDocument.Id;
        }
    }
}
