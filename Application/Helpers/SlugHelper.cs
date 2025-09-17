using System.Text;
using System.Text.RegularExpressions;

namespace Application.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
                return string.Empty;

            string str = phrase.ToLowerInvariant();

            // Türkçe karakterleri vs. ASCII’ye indirgeme
            str = str.Replace("ı", "i")
                     .Replace("ç", "c")
                     .Replace("ğ", "g")
                     .Replace("ö", "o")
                     .Replace("ş", "s")
                     .Replace("ü", "u");

            // Alfasayısal olmayan her şeyi tire ile değiştir
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // Boşlukları tek tireye çevir
            str = Regex.Replace(str, @"\s+", "-").Trim('-');

            // Çoklu tireleri teke indir
            str = Regex.Replace(str, @"-+", "-");

            return str;
        }
    }
}
