using FluentValidation;
using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Domain.Results;
using Vogel.BuildingBlocks.EntityFramework.Extensions;

namespace Nexa.Accounting.Application.Wallets.Queries.ListWallets
{
    public class ListWalletQueryHandler : IApplicationRequestHandler<ListWalletQuery, Paging<WalletListDto>>
    {
        private readonly IWalletRepository _walletRepository;

        private readonly IWalletResponseFactory _walletResponseFactory;

        private readonly ISecurityContext _securityContext;
        public ListWalletQueryHandler(IWalletRepository walletRepository, IWalletResponseFactory walletResponseFactory, ISecurityContext securityContext)
        {
            _walletRepository = walletRepository;
            _walletResponseFactory = walletResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<Paging<WalletListDto>>> Handle(ListWalletQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var query = _walletRepository.QueryView();

            if (request.Number != null)
            {
                query = query.Where(x =>
                    x.Number.StartsWith(request.Number) ||
                    x.Number == request.Number
                );
            }

            if (request.ExcludeOwned)
            {
                query = query.Where(x => x.UserId != userId);
            }

            var results = await query.ToPaged(request.Skip, request.Length);

            return await _walletResponseFactory.PreparePagingWalletListDto(results);
        }
    }
}
