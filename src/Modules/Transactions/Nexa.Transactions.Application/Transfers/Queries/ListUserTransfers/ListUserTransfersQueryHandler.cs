using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Application.Transfers.Factories;
using Nexa.Transactions.Domain.Transfers;
using Vogel.BuildingBlocks.EntityFramework.Extensions;
namespace Nexa.Transactions.Application.Transfers.Queries.ListUserTransfers
{
    public class ListUserTransfersQueryHandler : IApplicationRequestHandler<ListUserTransfersQuery, Paging<TransferDto>>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly ITransactionResponseFactory _transactionResponseFactory;
        private readonly ISecurityContext _securityContext;

        public ListUserTransfersQueryHandler(ITransferRepository transferRepository, ITransactionResponseFactory transactionResponseFactory, ISecurityContext securityContext)
        {
            _transferRepository = transferRepository;
            _transactionResponseFactory = transactionResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<Paging<TransferDto>>> Handle(ListUserTransfersQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var query = _transferRepository.QueryView()
                .Where(x => x.UserId == userId);

            if(request.WalletId != null)
            {
                query = query.Where(x => x.WalletId == request.WalletId);
            }

            var result = await query.ToPaged(request.Skip, request.Length);

            return await _transactionResponseFactory.PreparePagingDto(result);
        }
    }
}
