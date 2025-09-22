using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Domain.Results;
using Vogel.BuildingBlocks.EntityFramework.Extensions;

namespace Nexa.Accounting.Application.Wallets.Queries.ListUserWallets
{
    public class ListUserWalletsQueryHandler : IApplicationRequestHandler<ListUserWalletsQuery, Paging<WalletDto>>
    {
        private readonly IWalletRepository _walletRepository;

        private readonly IWalletResponseFactory _walletResponseFactory;

        private readonly ISecurityContext _securityContext;

        public ListUserWalletsQueryHandler(IWalletRepository walletRepository, IWalletResponseFactory walletResponseFactory, ISecurityContext securityContext)
        {
            _walletRepository = walletRepository;
            _walletResponseFactory = walletResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<Paging<WalletDto>>> Handle(ListUserWalletsQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id!;

            var results = await _walletRepository.QueryView()
                .Where(x => x.UserId == userId)
                .ToPaged(request.Skip, request.Length);

            return await _walletResponseFactory.PreparePagingDto(results);
        }
    }
}
