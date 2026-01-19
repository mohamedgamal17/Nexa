using Microsoft.EntityFrameworkCore;
using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;

namespace Nexa.Accounting.Application.Wallets.Queries.GetWalletById
{
    public class GetWalletByIdQueryHandler : IApplicationRequestHandler<GetWalletByIdQuery, WalletListDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletResponseFactory _walletResponseFactory;

        public GetWalletByIdQueryHandler(IWalletRepository walletRepository, IWalletResponseFactory walletResponseFactory)
        {
            _walletRepository = walletRepository;
            _walletResponseFactory = walletResponseFactory;
        }

        public async Task<Result<WalletListDto>> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {

            var result = await _walletRepository
                .QueryView()
                .SingleOrDefaultAsync(x => x.Id == request.WalletId);

            if (result == null)
            {
                return new Result<WalletListDto>(new EntityNotFoundException(typeof(Wallet), request.WalletId));
            }

            return await _walletResponseFactory.PrepareWalletListDto(result);
        }
    }

}
