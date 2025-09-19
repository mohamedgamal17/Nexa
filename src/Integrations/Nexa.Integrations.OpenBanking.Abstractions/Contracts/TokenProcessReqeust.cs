using Nexa.Integrations.OpenBanking.Abstractions.enums;

namespace Nexa.Integrations.OpenBanking.Abstractions.Contracts
{
    public class TokenProcessReqeust
    {
        public string ClientUserId { get; set; }
        public string  Token { get; set; }
        public ProcessorProvider Provider { get; set; }
    }
}
