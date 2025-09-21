using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Accounting.Shared.Services;
using Nexa.CustomerManagement.Shared.Services;
using Nexa.Integrations.Baas.Abstractions.Contracts.Transfers;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Consumer
{
    public class ProcessNetworkTransferIntegrationEventConsumer : IConsumer<ProcessNetworkTransferIntegrationEvent>
    {

        private readonly ITransactionRepository<NetworkTransfer> _networkTransferRepository;
        private readonly ICustomerService _customerService;
        private readonly IWalletService _walletService;
        private readonly IBaasTransferService _baasTransferService;

        public ProcessNetworkTransferIntegrationEventConsumer(ITransactionRepository<NetworkTransfer> networkTransferRepository, ICustomerService customerService, IWalletService walletService, IBaasTransferService baasTransferService)
        {
            _networkTransferRepository = networkTransferRepository;
            _customerService = customerService;
            _walletService = walletService;
            _baasTransferService = baasTransferService;
        }

        public async Task Consume(ConsumeContext<ProcessNetworkTransferIntegrationEvent> context)
        {
            var transfer = await _networkTransferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            var senderCustomer = await _customerService.GetCustomerByUserId(transfer.UserId);

            var senderWallet = await _walletService.GetWalletById(context.Message.WalletId);

            var reciverWallet = await _walletService.GetWalletById(context.Message.ReciverId);

            var networkTransferRequest = new NetworkTransferRequest
            {
                ClientTransferId = transfer.Id,
                SenderAccountId = senderCustomer!.FintechCustomerId!,
                SenderWalletId = senderWallet!.ProviderWalletId!,
                ReciverWalletId = reciverWallet!.Id,
                Amount = context.Message.Amount
            };

            var baasTransfer = await _baasTransferService.NetworkTransfer(networkTransferRequest);

            transfer.AssignExternalTransferId(baasTransfer.Id);

            await _networkTransferRepository.UpdateAsync(transfer);
        }
    }
}
