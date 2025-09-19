namespace Nexa.Integrations.Baas.Abstractions.Contracts.Transfers
{
    public class DepositTransferRequest
    {
        public string ClinetTransferId { get; set; }
        public string AccountId { get; set; }
        public string WalletId { get; set; }
        public string FundingResourceId { get; set; }
        public decimal Amount { get; set; }
    }
}
