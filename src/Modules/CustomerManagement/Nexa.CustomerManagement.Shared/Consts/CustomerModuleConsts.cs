
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Shared.Consts
{
    public static class CustomerModuleConsts
    {
        public static List<string> SupportedRegions = ["US"];

        public static List<DocumentType> DocumentRequireIssuingCountries = [DocumentType.DrivingLicense];

        public static List<string> AllowedImageExtensions = [".jpg", ".jpeg", ".png"];
    }
}
