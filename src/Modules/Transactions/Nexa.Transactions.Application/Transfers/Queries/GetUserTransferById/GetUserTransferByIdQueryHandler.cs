using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Application.Transfers.Factories;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Consts;
namespace Nexa.Transactions.Application.Transfers.Queries.GetUserTransferById
{
    public class GetUserTransferByIdQueryHandler : IApplicationRequestHandler<GetUserTransferByIdQuery, TransferDto>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly ITransactionResponseFactory _transactionResponseFactory;
        private readonly ISecurityContext _securityContext;

        public GetUserTransferByIdQueryHandler(ITransferRepository transferRepository, ITransactionResponseFactory transactionResponseFactory, ISecurityContext securityContext)
        {
            _transferRepository = transferRepository;
            _transactionResponseFactory = transactionResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<TransferDto>> Handle(GetUserTransferByIdQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var result = await _transferRepository.QueryView()
                .Where(x => x.UserId == userId)
                .SingleOrDefaultAsync(x => x.Id == request.TransferId);


            if(result == null)
            {
                return new EntityNotFoundException(TransferErrorConsts.TransferNotExist);
            }


            return await _transactionResponseFactory.PrepareDto(result);
        }
    }
}
