using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentCommandHandler : IApplicationRequestHandler<DeleteDocumentCommand, Unit>
    {
        private readonly ICustomerManagementRepository<Document> _documentRepository;
        private readonly ISecurityContext _securityContext;
        public DeleteDocumentCommandHandler(ICustomerManagementRepository<Document> documentRepository, ISecurityContext securityContext)
        {
            _documentRepository = documentRepository;
            _securityContext = securityContext;
        }

        public async Task<Result<Unit>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var document = await _documentRepository.SingleOrDefaultAsync(x => x.Id == request.Id);

            if (document == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(Document), request.Id));
            }

            var isDocumentOwner = IsDocumentOwner(userId, document);

            if (!isDocumentOwner)
            {
                return new Result<Unit>(new ForbiddenAccessException());
            }

            if (document.IsActive)
            {
                return new Result<Unit>(new BusinessLogicException("Cannot delete active processing kyc document."));
            }

            if (document.Status == Status.Approved)
            {
                return new Result<Unit>(new BusinessLogicException("Cannot delete approved kyc document."));
            }

            await _documentRepository.DeleteAsync(document);

            return Unit.Value;
        }

        private bool IsDocumentOwner(string userId, Document kYCDocument)
        {
            return userId == kYCDocument.Id;
        }
    }
}
