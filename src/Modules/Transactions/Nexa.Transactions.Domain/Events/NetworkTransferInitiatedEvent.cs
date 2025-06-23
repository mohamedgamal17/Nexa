using Nexa.BuildingBlocks.Domain.Events;
namespace Nexa.Transactions.Domain.Events
{
    public class NetworkTransferInitiatedEvent : IEvent
    {
      

        public string Id { get; set; }
        public string Number { get; set; }
        public string WalletId { get; set; }
        public string ReciverId { get; set; }
        public decimal Amount { get; set; }
        public NetworkTransferInitiatedEvent(string id, string number, string walletId, string reciverId, decimal amount)
        {
            Id = id;
            Number = number;
            WalletId = walletId;
            ReciverId = reciverId;
            Amount = amount;
        }
    }
}
