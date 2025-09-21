namespace Nexa.Integrations.Baas.Abstractions.Contracts.Transfers
{
    public class BaasNetworkTransfer
    {
        public string Id { get; set; }
        public string SenderWalletId { get; set; }
        public string ReciverWalletId { get; set; }
        public decimal Amount { get; set; }
    }
}
