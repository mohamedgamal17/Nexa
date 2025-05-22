using MassTransit;
using Microsoft.Extensions.Logging;
using Nexa.Accounting.Application.Transactions.Events;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Wallets;

namespace Nexa.Accounting.Application.Transactions.Consumers
{
    public class ProcessInternalTransactionIntgertationEventConsumer : IConsumer<ProcessInternalTransactionIntgertationEvent>
    {
        private readonly IAccountingRepository<InternalTransaction> _transactionRepository;
        private readonly IAccountingRepository<Wallet> _walletRepository;
        private readonly ILogger<ProcessInternalTransactionIntgertationEventConsumer> logger;
        public ProcessInternalTransactionIntgertationEventConsumer(IAccountingRepository<InternalTransaction> transactionRepository, IAccountingRepository<Wallet> walletRepository, ILogger<ProcessInternalTransactionIntgertationEventConsumer> logger)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<ProcessInternalTransactionIntgertationEvent> context)
        {
            var transaction = await _transactionRepository.SingleAsync(x => x.Id == context.Message.TransactionId);

            var senderWallet = await _walletRepository.SingleAsync(x => x.Id == transaction.WalletId);

            var reciverWallet = await _walletRepository.SingleAsync(x => x.Id == transaction.ReciverId);

            senderWallet.Depit(transaction.Amount);

            reciverWallet.Credit(transaction.Amount);

            transaction.Complete();

            await _transactionRepository.UpdateAsync(transaction);

            await _walletRepository.UpdateAsync(senderWallet);

            await _walletRepository.UpdateAsync(reciverWallet);

            logger.LogInformation("Consumer Completed");

        }
    }
}
