using System;
using System.Data.SqlClient;
using System.Web;

namespace Saat.App_Code
{
    /// <summary>Misafir/üye ilgi olayları — basit demo öneri sistemi.</summary>
    public static class IlgiTakipHelper
    {
        public const string Goruntuleme = "Goruntuleme";
        public const string Sepet = "Sepet";
        public const string SatinAlma = "SatinAlma";

        public static void Kaydet(HttpContext ctx, int urunId, string olayTipi)
        {
            if (ctx == null || urunId <= 0) return;
            try
            {
                VeritabaniKurulum.Hazirla();
                int puan = PuanAl(olayTipi);
                MagazaYardimcisi.OturumIdAl(ctx);
                string oturumId = ctx.Request?.Cookies[MagazaYardimcisi.CookieOturumId]?.Value;
                int? musteriId = MusteriAuth.GirisMi(ctx.Session)
                    ? (int?)MusteriAuth.MusteriId(ctx.Session) : null;

            using (var b = new BaglantiBilgileri())
            using (var cmd = new SqlCommand(@"
                    INSERT INTO UrunIlgiLog (MusteriId, OturumId, UrunId, OlayTipi, Puan, Tarih)
                    VALUES (@Mid, @Oid, @Uid, @Tip, @Puan, GETDATE())", b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@Mid", musteriId.HasValue ? (object)musteriId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Oid", string.IsNullOrEmpty(oturumId) ? (object)DBNull.Value : oturumId);
                    cmd.Parameters.AddWithValue("@Uid", urunId);
                    cmd.Parameters.AddWithValue("@Tip", olayTipi ?? Goruntuleme);
                    cmd.Parameters.AddWithValue("@Puan", puan);
                    if (cmd.Connection.State == System.Data.ConnectionState.Closed) cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                /* UrunIlgiLog tablosu yoksa demo akışı devam etsin */
            }
        }

        public static void GoruntulemeKaydet(HttpContext ctx, int urunId)
        {
            Kaydet(ctx, urunId, Goruntuleme);
        }

        public static void SepetKaydet(HttpContext ctx, int urunId)
        {
            Kaydet(ctx, urunId, Sepet);
        }

        public static void SatinAlmaKaydet(HttpContext ctx, int urunId)
        {
            Kaydet(ctx, urunId, SatinAlma);
        }

        private static int PuanAl(string olayTipi)
        {
            if (olayTipi == Sepet) return 3;
            if (olayTipi == SatinAlma) return 5;
            return 1;
        }
    }
}
