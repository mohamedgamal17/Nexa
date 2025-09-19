namespace Nexa.Integrations.Baas.Abstractions.Contracts.Transfers
{
    public class BaasTransfer 
    {
        public string Id { get; set; }
        public string WalletId { get; set; }
        public decimal Amount { get; set; }
    }

    public class BaasDepositTransfer : BaasTransfer
    {
        public string FundingResource { get; set; }
    }
}
