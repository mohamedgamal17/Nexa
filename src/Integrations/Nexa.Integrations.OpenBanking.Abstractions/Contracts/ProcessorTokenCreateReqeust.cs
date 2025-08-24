using Nexa.Integrations.OpenBanking.Abstractions.enums;

namespace Nexa.Integrations.OpenBanking.Abstractions.Contracts
{
    public class ProcessorTokenCreateReqeust
    {
        public string  AccessToken { get; set; }
        public ProcessorProvider Provider { get; set; }
    }
}
