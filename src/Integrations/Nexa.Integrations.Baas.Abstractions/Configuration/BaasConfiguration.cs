namespace Nexa.Integrations.Baas.Abstractions.Configuration
{
    public class BaasConfiguration
    {
        public static string SectionName = "Baas";
        public string ApiKey { get; set; }
        public string WebhookSecret { get; set; }
        public FinancialAccounts FinancialAccounts { get; set; } = new FinancialAccounts();
    }

    public class FinancialAccounts : Dictionary<string, string>
    {
        public string? Main => this.GetValueOrDefault(nameof(Main));

    }
}
