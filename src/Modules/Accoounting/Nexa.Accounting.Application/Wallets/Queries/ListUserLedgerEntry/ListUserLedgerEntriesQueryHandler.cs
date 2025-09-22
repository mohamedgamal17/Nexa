using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Vogel.BuildingBlocks.EntityFramework.Extensions;

namespace Nexa.Accounting.Application.Wallets.Queries.ListUserLedgerEntry
{
    public class ListUserLedgerEntriesQueryHandler : IApplicationRequestHandler<ListUserLedgerEntriesQuery, Paging<LedgerEntryDto>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IAccountingRepository<LedgerEntry> _ledgerEntryRepository;
        private readonly ILedgerEntryResponseFactory _ledgerEntryResponseFactory;
        private readonly ISecurityContext _securityContext;

        public ListUserLedgerEntriesQueryHandler(IWalletRepository walletRepository, IAccountingRepository<LedgerEntry> ledgerEntryRepository, ILedgerEntryResponseFactory ledgerEntryResponseFactory, ISecurityContext securityContext)
        {
            _walletRepository = walletRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
            _ledgerEntryResponseFactory = ledgerEntryResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<Paging<LedgerEntryDto>>> Handle(ListUserLedgerEntriesQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id!;

            var wallet = await _walletRepository
                .SingleOrDefaultAsync(x => x.Id == request.WalletId && x.UserId == userId);

            if(wallet == null)
            {
                return new Result<Paging<LedgerEntryDto>>(new EntityNotFoundException(typeof(Wallet), request.WalletId));
            }

            var ledgerEntries = await _ledgerEntryRepository.AsQuerable()
                .Where(x => x.WalletId == request.WalletId)
                .ToPaged(request.Skip, request.Length);


            return await _ledgerEntryResponseFactory.PreparePagingDto(ledgerEntries);
        }
    }

}
