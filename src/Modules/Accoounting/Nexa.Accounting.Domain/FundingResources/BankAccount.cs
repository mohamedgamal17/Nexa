using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.FundingResources
{
    public class BankAccount : BaseEntity
    {
     
        public string UserId { get; private set; }
        public string CustomerId { get; private set; }
        public string ProviderBankAccountId { get; private set; }
        public string HolderName { get; private set; }
        public string BankName { get; private set; }
        public string Country { get; private set; }
        public string Currency { get; private set; }
        public string AccountNumberLast4 { get; private set; }
        public string RoutingNumber { get; private set; }
        private BankAccount() { }
        public BankAccount(string userId, string customerId, string providerBankAccountId, string holderName, string bankName, string country, string currency, string accountNumberLast4, string routingNumber)
        {
            UserId = userId;
            CustomerId = customerId;
            ProviderBankAccountId = providerBankAccountId;
            HolderName = holderName;
            BankName = bankName;
            Country = country;
            Currency = currency;
            AccountNumberLast4 = accountNumberLast4;
            RoutingNumber = routingNumber;
        }

    }
}
