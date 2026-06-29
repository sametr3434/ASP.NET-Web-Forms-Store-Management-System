using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Saat.App_Code
{
    /// <summary>
    /// Urun detay URL'leri icin ASCII slug (SEO). Turkce karakterler donusturulur.
    /// </summary>
    public static class UrunSeoSlug
    {
        /// <summary>Urun adindan URL parcasi; bos ise "urun".</summary>
        public static string Olustur(string urunAdi)
        {
            var s = EncodingHelper.DuzeltTurkce(urunAdi ?? "").Trim();
            if (string.IsNullOrEmpty(s))
                return "urun";

            var tr = new CultureInfo("tr-TR");
            s = s.ToLower(tr);
            s = s.Replace('ı', 'i').Replace('ğ', 'g').Replace('ü', 'u').Replace('ş', 's')
                 .Replace('ö', 'o').Replace('ç', 'c').Replace('â', 'a').Replace('î', 'i')
                 .Replace('û', 'u');

            s = Regex.Replace(s, @"[^a-z0-9]+", "-");
            s = Regex.Replace(s, "-{2,}", "-").Trim('-');
            if (string.IsNullOrEmpty(s))
                return "urun";
            if (s.Length > 80)
                s = s.Substring(0, 80).TrimEnd('-');
            return s;
        }
    }
}
