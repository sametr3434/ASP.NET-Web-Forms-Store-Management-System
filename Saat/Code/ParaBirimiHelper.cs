using System;
using System.Data.SqlClient;
using System.Globalization;

namespace Saat.App_Code
{
    /// <summary>
    /// TRY / USD / EUR gösterimi ve PayTR öncesi TL dönüşümü için yardımcı.
    /// Kurlar sitesabitleri.KurUSD, KurEUR: 1 birim yabancı para = X TL.
    /// </summary>
    public static class ParaBirimiHelper
    {
        public const string Try = "TRY";
        public const string Usd = "USD";
        public const string Eur = "EUR";

        public static string Normalize(string kod)
        {
            if (string.IsNullOrWhiteSpace(kod)) return Try;
            var k = kod.Trim().ToUpperInvariant();
            if (k == "TL" || k == "TRY") return Try;
            if (k == "USD" || k == "DOLAR" || k == "US$") return Usd;
            if (k == "EUR" || k == "EURO" || k == "€") return Eur;
            return k.Length > 3 ? Try : k;
        }

        public static string Sembol(string kod)
        {
            switch (Normalize(kod))
            {
                case Usd: return "$";
                case Eur: return "\u20AC";
                default: return "\u20BA";
            }
        }

        public static string Format(decimal tutar, string paraBirimi)
        {
            var pb = Normalize(paraBirimi);
            var s = Sembol(pb);
            return string.Format(CultureInfo.GetCultureInfo("tr-TR"), "{0:N2} {1}", tutar, s);
        }

        /// <summary>
        /// Sipariş/sepet tutarını PayTR için TL'ye çevirir (kuru yoksa tutarı aynen döner — TL varsayılır).
        /// </summary>
        public static decimal ToTry(decimal tutar, string paraBirimi, decimal? kurUsd, decimal? kurEur)
        {
            var pb = Normalize(paraBirimi);
            if (pb == Try) return tutar;
            if (pb == Usd && kurUsd.HasValue && kurUsd.Value > 0) return Math.Round(tutar * kurUsd.Value, 2, MidpointRounding.AwayFromZero);
            if (pb == Eur && kurEur.HasValue && kurEur.Value > 0) return Math.Round(tutar * kurEur.Value, 2, MidpointRounding.AwayFromZero);
            return tutar;
        }

        /// <summary>
        /// sitesabitleri satırından KurUSD/KurEUR okur (BaglantiBilgileri ile).
        /// </summary>
        public static void KurlariOku(SqlConnection conn, out decimal? kurUsd, out decimal? kurEur)
        {
            kurUsd = null;
            kurEur = null;
            if (conn == null) return;
            var wasClosed = conn.State == System.Data.ConnectionState.Closed;
            try
            {
                if (wasClosed) conn.Open();
                using (var cmd = new SqlCommand("SELECT KurUSD, KurEUR FROM sitesabitleri WHERE ID=1", conn))
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;
                    if (r["KurUSD"] != DBNull.Value) kurUsd = Convert.ToDecimal(r["KurUSD"]);
                    if (r["KurEUR"] != DBNull.Value) kurEur = Convert.ToDecimal(r["KurEUR"]);
                }
            }
            finally
            {
                if (wasClosed && conn.State == System.Data.ConnectionState.Open) conn.Close();
            }
        }
    }
}
