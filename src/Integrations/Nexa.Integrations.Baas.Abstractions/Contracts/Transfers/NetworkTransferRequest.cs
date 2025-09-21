namespace Nexa.Integrations.Baas.Abstractions.Contracts.Transfers
{
    public class NetworkTransferRequest 
    {
        public string ClientTransferId { get; set; }
        public string SenderAccountId { get; set; }
        public string SenderWalletId { get; set; }
        public string ReciverWalletId { get; set; }
        public decimal Amount { get; set; }
    }
}
