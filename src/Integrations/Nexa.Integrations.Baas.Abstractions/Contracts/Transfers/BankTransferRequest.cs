namespace Nexa.Integrations.Baas.Abstractions.Contracts.Transfers
{
    public class BankTransferRequest
    {
        public string ClientTransferId { get; set; }
        public string AccountId { get; set; }
        public string WalletId { get; set; }
        public string FundingResourceId { get; set; }
        public decimal Amount { get; set; }
    }
}
