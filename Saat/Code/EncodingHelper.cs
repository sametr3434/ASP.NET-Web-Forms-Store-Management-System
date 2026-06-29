using System;
using System.Text;

namespace Saat.App_Code
{
    /// <summary>
    /// Veritabanından veya dış kaynaktan gelen Türkçe karakter bozulmalarını düzeltir.
    /// UTF-8 yanlış okunduğunda (Latin1/Windows-1252) oluşan ArtÄ±, GiriÅŸ gibi hataları giderir.
    /// </summary>
    public static class EncodingHelper
    {
        public static string DuzeltTurkce(string metin)
        {
            if (string.IsNullOrEmpty(metin)) return metin;

            var sb = new StringBuilder(metin);
            sb.Replace("Ä±", "ı").Replace("Ä°", "İ");
            sb.Replace("Ã¶", "ö").Replace("Ã–", "Ö");
            sb.Replace("Ã¼", "ü").Replace("Ãœ", "Ü");
            sb.Replace("Ã§", "ç").Replace("Ã‡", "Ç");
            sb.Replace("ÄŸ", "ğ").Replace("Äž", "Ğ");
            sb.Replace("ÅŸ", "ş").Replace("Åž", "Ş");
            sb.Replace("Ã¢", "â").Replace("Ã®", "î").Replace("Ã»", "û");
            sb.Replace("Ã‚", "Â").Replace("ÃŽ", "Î").Replace("Ã›", "Û");
            return sb.ToString();
        }
    }
}
