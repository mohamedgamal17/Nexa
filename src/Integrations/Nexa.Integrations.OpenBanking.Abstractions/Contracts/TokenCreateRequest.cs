using Nexa.Integrations.OpenBanking.Abstractions.enums;

namespace Nexa.Integrations.OpenBanking.Abstractions.Contracts
{
    public class TokenCreateRequest
    {
        public List<CountryIsoCode> CountryCodes { get; set; } = new List<CountryIsoCode>();
        public string ClientUserId { get; set; }
        public string? RedirectUri { get; set; }
    }
}
