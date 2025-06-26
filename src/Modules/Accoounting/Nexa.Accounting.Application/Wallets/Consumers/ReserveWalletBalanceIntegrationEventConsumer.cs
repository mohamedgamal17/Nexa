using MassTransit;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Events;

namespace Nexa.Accounting.Application.Wallets.Consumers
{
    public class ReserveWalletBalanceIntegrationEventConsumer : IConsumer<ReserveWalletBalanceIntegrationEvent>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public ReserveWalletBalanceIntegrationEventConsumer(IWalletRepository walletRepository, IPublishEndpoint publishEndpoint)
        {
            _walletRepository = walletRepository;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<ReserveWalletBalanceIntegrationEvent> context)
        {
            var wallet = await _walletRepository.SingleAsync(x => x.Id == context.Message.WalletId);

            if (wallet.Balance < context.Message.Amount)
            {
                var message = new WalletBalanceReservationFailedIntegrationEvent()
                {
                    TransferId = context.Message.TransferId,
                    WalletId = context.Message.WalletId,
                    Amount = context.Message.Amount,
                    Reason = "Insufficient balance to be reserved."
                };

                await _publishEndpoint.Publish(message);
            }
            else
            {
                wallet.Reserve(context.Message.Amount);

                await _walletRepository.UpdateAsync(wallet);

                var message = new WalletBalanceReservedIntegrationEvent
                {
                    TransferId = context.Message.TransferId,
                    WalletId = context.Message.WalletId,
                    Amount = context.Message.Amount
                };

                await _publishEndpoint.Publish(message);
            }
        }
    }
}
