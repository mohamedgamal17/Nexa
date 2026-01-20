using MassTransit;
using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Services;
using Nexa.CustomerManagement.Shared.Services;
using Nexa.Integrations.Baas.Abstractions.Contracts.Transfers;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;

namespace Nexa.Transactions.Application.Transfers.Consumer
{
    public class ProcessBankTransferIntegrationEventConsumer : IConsumer<ProcessBankTransferIntegrationEvent>
    {
        private readonly IBaasTransferService _baasTransferService;

        private readonly ICustomerService _customerService;

        private readonly IWalletService _walletService;

        private readonly IFundingResourceService _fundingResourceService;

        private readonly ITransactionRepository<BankTransfer> _bankTransferRepository;

        public ProcessBankTransferIntegrationEventConsumer(IBaasTransferService baasTransferService, ICustomerService customerService, IWalletService walletService, IFundingResourceService fundingResourceService, ITransactionRepository<BankTransfer> bankTransferRepository)
        {
            _baasTransferService = baasTransferService;
            _customerService = customerService;
            _walletService = walletService;
            _fundingResourceService = fundingResourceService;
            _bankTransferRepository = bankTransferRepository;
        }

        public async Task Consume(ConsumeContext<ProcessBankTransferIntegrationEvent> context)
        {
            var wallet = await _walletService.GetWalletById(context.Message.WalletId);

            var customer = await _customerService.GetByUserId(context.Message.UserId);

            var fundingResource = await _fundingResourceService.GetFundingResourceById(context.Message.FundingResourceId);

            var transfer = await _bankTransferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            var bankTransferRequest = new BankTransferRequest
            {
                WalletId = wallet.ProviderWalletId,
                AccountId = customer!.FintechCustomerId!,
                FundingResourceId = fundingResource!.ProviderBankAccountId,
                Amount = context.Message.Amount,
                ClientTransferId = context.Message.TransferId
            };

            var externalTransfer = transfer.Direction == TransferDirection.Credit
                ? await _baasTransferService.Deposit(bankTransferRequest)
                : await _baasTransferService.Withdraw(bankTransferRequest);

            transfer.AssignExternalTransferId(externalTransfer.Id);

            await _bankTransferRepository.UpdateAsync(transfer);
        }
    }
}
