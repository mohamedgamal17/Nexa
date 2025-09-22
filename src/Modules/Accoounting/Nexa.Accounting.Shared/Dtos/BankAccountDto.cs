using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Shared.Dtos
{
    public class BankAccountDto : EntityDto
    {
        public string UserId { get; set; }
        public string CustomerId { get; set; }
        public string ProviderBankAccountId { get; set; }
        public string HolderName { get; set; }
        public string BankName { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string AccountNumberLast4 { get; set; }
        public string RoutingNumber { get; set; }
    }
}
