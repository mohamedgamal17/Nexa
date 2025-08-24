using Nexa.Integrations.OpenBanking.Abstractions.enums;

namespace Nexa.Integrations.OpenBanking.Abstractions.Contracts
{
    public class LinkTokenCreateRequest
    {
        public string ClinetName { get; set; }
        public LanguageIsoCode Language { get; set; } = LanguageIsoCode.en;
        public List<CountryIsoCode> CountryCodes { get; set; } = new List<CountryIsoCode>();
        public LinkTokenUser User { get; set; }
        public string RedirectUri { get; set; }
    }
}
