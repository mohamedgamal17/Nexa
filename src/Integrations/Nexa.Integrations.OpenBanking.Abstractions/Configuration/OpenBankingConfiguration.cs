namespace Nexa.Integrations.OpenBanking.Abstractions.Configuration
{
    public class OpenBankingConfiguration
    {
        public static string SectionName = "OpenBanking";

        public string ClientId { get; set; } 
        public string ClientSecret { get; set; }
        public OpenBankingConfiguration()
        {
        }
    }
}
