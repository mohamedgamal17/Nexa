using Microsoft.EntityFrameworkCore;
using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;

namespace Nexa.Accounting.Application.Wallets.Queries.GetUserWalletById
{
    public class GetUserWalletByIdQueryHandler : IApplicationRequestHandler<GetUserWalletByIdQuery, WalletDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletResponseFactory _walletResponseFactory;
        private readonly ISecurityContext _securityContext;

        public GetUserWalletByIdQueryHandler(IWalletRepository walletRepository, IWalletResponseFactory walletResponseFactory, ISecurityContext securityContext)
        {
            _walletRepository = walletRepository;
            _walletResponseFactory = walletResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<WalletDto>> Handle(GetUserWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _securityContext.User!.Id!;

            var result = await _walletRepository
                .QueryView()
                .Where(x=> x.UserId == userId)
                .SingleOrDefaultAsync(x => x.Id == request.WalletId);

            if(result == null)
            {
                return new Result<WalletDto>(new EntityNotFoundException(typeof(Wallet), request.WalletId));
            }

            return await _walletResponseFactory.PrepareDto(result);
        }
    }
}
