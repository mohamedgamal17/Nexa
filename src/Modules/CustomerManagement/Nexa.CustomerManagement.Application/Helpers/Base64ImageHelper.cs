
using System.Drawing;
namespace Nexa.CustomerManagement.Application.Helpers
{
    public class Base64ImageHelper
    {
        public static string? GetImageExtension(string base64String)
        {
            if (base64String.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                var mimeType = base64String.Substring(5, base64String.IndexOf(";") - 5);
                return MimeTypeToExtension(mimeType);
            }

            return null;
        }
        public static bool IsValidBase64Image(string base64String)
        {
            try
            {
                var base64Data = base64String.Contains(",")
                    ? base64String.Substring(base64String.IndexOf(",") + 1)
                    : base64String;

                byte[] imageBytes = Convert.FromBase64String(base64Data);

                using var ms = new MemoryStream(imageBytes);

                using var img = Image.FromStream(ms);

                return true; 
            }
            catch
            {
                return false; 
            }
        }
        private static string? MimeTypeToExtension(string mimeType)
        {
            return mimeType.ToLower() switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "image/bmp" => ".bmp",
                "image/webp" => ".webp",
                "image/svg+xml" => ".svg",
                _ => null
            };
        }
    }
}
