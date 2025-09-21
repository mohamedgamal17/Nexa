namespace Nexa.Integrations.Baas.Abstractions.Contracts.Transfers
{
    public class BaasTransfer 
    {
        public string Id { get; set; }
        public string WalletId { get; set; }
        public decimal Amount { get; set; }
    }

    public class BaasBankTransfer : BaasTransfer
    {
        public string FundingResourceId { get; set; }
    }
}
