using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Nexa.Accounting.Application.Wallets.Queries.GetUserLedgerEntryByIdQuery
{
    public class GetUserLedgerEntryByIdQueryHandler : IApplicationRequestHandler<GetUserLedgerEntryByIdQuery, LedgerEntryDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IAccountingRepository<LedgerEntry> _ledgerEntryRepository;
        private readonly ILedgerEntryResponseFactory _ledgerEntryResponseFactory;
        private readonly ISecurityContext _securityContext;

        public GetUserLedgerEntryByIdQueryHandler(IWalletRepository walletRepository, IAccountingRepository<LedgerEntry> ledgerEntryRepository, ILedgerEntryResponseFactory ledgerEntryResponseFactory, ISecurityContext securityContext)
        {
            _walletRepository = walletRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
            _ledgerEntryResponseFactory = ledgerEntryResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<LedgerEntryDto>> Handle(GetUserLedgerEntryByIdQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var wallet = await _walletRepository
                    .SingleOrDefaultAsync(x => x.Id == request.WalletId && x.UserId == userId);

            if (wallet == null)
            {
                return new Result<LedgerEntryDto>(new EntityNotFoundException(typeof(Wallet), request.WalletId));
            }

            var ledgerEntry = await _ledgerEntryRepository
                .AsQuerable()
                .Where(x => x.WalletId == request.WalletId)
                .SingleOrDefaultAsync(x => x.Id == request.LedgerEntryId);

            if (ledgerEntry == null)
            {
                return new Result<LedgerEntryDto>(new EntityNotFoundException(typeof(LedgerEntry), request.LedgerEntryId));

            }

            return await _ledgerEntryResponseFactory.PrepareDto(ledgerEntry);
        }
    }
}
