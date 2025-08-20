namespace Nexa.Integrations.Baas.Abstractions.Configuration
{
    public class BaasConfiguration
    {
        public static string SectionName = "Baas";
        public string ApiKey { get; set; }

        public string WebHookSecret { get; set; }
    }
}
