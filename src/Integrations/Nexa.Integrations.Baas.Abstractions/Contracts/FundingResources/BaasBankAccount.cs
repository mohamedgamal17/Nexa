namespace Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources
{
    public class BaasBankAccount
    {
        public string Id { get; set; }
        public string HolderName { get; set; }
        public string BankName { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string AccountNumberLast4 { get; set; }
        public string RoutingNumber { get; set; }
    }
}
