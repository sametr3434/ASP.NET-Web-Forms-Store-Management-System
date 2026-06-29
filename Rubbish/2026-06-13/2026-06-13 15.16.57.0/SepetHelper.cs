using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Saat.App_Code
{
    /// <summary>
    /// Sepet CRUD: ekle, sil, adet g\u00fcncelle, misafir-\u00fcye birle\u015ftirme, stok kontrol\u00fc.
    /// Stok kayna\u011f\u0131: UrunVaryant.StokAdedi (varyant varsa) > Urun.StokAdedi (fallback).
    /// </summary>
    public static class SepetHelper
    {
        // ── Stok ─────────────────────────────────────────────────────────

        public static int StokAl(int urunId, int? varyantId)
        {
            var b = new BaglantiBilgileri();
            if (varyantId.HasValue && varyantId.Value > 0)
            {
                using (var cmd = new SqlCommand(
                    "SELECT StokAdedi FROM UrunVaryant WHERE ID=@vid AND UrunId=@uid", b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@vid", varyantId.Value);
                    cmd.Parameters.AddWithValue("@uid", urunId);
                    if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                    var r = cmd.ExecuteScalar();
                    if (r != null && r != DBNull.Value)
                        return Convert.ToInt32(r);
                }
            }
            using (var cmd = new SqlCommand(
                "SELECT StokAdedi FROM Urun WHERE ID=@uid", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@uid", urunId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                var r = cmd.ExecuteScalar();
                return r == null || r == DBNull.Value ? 0 : Convert.ToInt32(r);
            }
        }

        // ── Sepete Ekle ───────────────────────────────────────────────────

        /// <summary>
        /// Sepete \u00fcr\u00fcn ekler (varsa adet artt\u0131r\u0131r). null d\u00f6n\u00fc\u015f = ba\u015far\u0131l\u0131; string = hata mesaj\u0131.
        /// </summary>
        public static string Ekle(HttpContext ctx,
            int urunId, int? varyantId, int adet, string paraBirimi, decimal birimFiyat)
        {
            if (adet <= 0) return "Ge\u00e7ersiz adet.";
            int stok = StokAl(urunId, varyantId);
            if (stok <= 0) return "Bu \u00fcr\u00fcn stokta bulunmamaktad\u0131r.";

            string oturumId = MagazaYardimcisi.OturumIdAl(ctx);
            int? musteriId = MusteriAuth.GirisMi(ctx.Session)
                ? (int?)MusteriAuth.MusteriId(ctx.Session) : null;

            var b = new BaglantiBilgileri();
            // Mevcut sat\u0131r? (ayn\u0131 \u00fcr\u00fcn + varyant)
            string chkSql = musteriId.HasValue
                ? "SELECT ID, Adet FROM Sepet WHERE MusteriId=@mid AND UrunId=@uid AND " +
                  (varyantId.HasValue ? "UrunVaryantId=@vid" : "UrunVaryantId IS NULL")
                : "SELECT ID, Adet FROM Sepet WHERE OturumId=@oid AND (MusteriId IS NULL OR MusteriId=0) AND UrunId=@uid AND " +
                  (varyantId.HasValue ? "UrunVaryantId=@vid" : "UrunVaryantId IS NULL");

            int satirId = 0, mevcutAdet = 0;
            using (var cmd = new SqlCommand(chkSql, b.Baglanti))
            {
                if (musteriId.HasValue) cmd.Parameters.AddWithValue("@mid", musteriId.Value);
                else cmd.Parameters.AddWithValue("@oid", oturumId);
                cmd.Parameters.AddWithValue("@uid", urunId);
                if (varyantId.HasValue) cmd.Parameters.AddWithValue("@vid", varyantId.Value);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        satirId = (int)rdr["ID"];
                        mevcutAdet = (int)rdr["Adet"];
                    }
                }
            }

            int yeniAdet = Math.Min(mevcutAdet + adet, stok);
            if (yeniAdet <= 0) return "Bu \u00fcr\u00fcn stokta bulunmamaktad\u0131r.";

            if (satirId > 0)
            {
                using (var cmd = new SqlCommand(
                    "UPDATE Sepet SET Adet=@a WHERE ID=@id", b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@a", yeniAdet);
                    cmd.Parameters.AddWithValue("@id", satirId);
                    if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                using (var cmd = new SqlCommand(
                    @"INSERT INTO Sepet(OturumId,MusteriId,UrunId,UrunVaryantId,Adet,EklenmeTarihi,ParaBirimi,BirimFiyat)
                      VALUES(@oid,@mid,@uid,@vid,@adet,@et,@pb,@bf)", b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@oid", oturumId);
                    cmd.Parameters.AddWithValue("@mid",
                        musteriId.HasValue ? (object)musteriId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@uid", urunId);
                    cmd.Parameters.AddWithValue("@vid",
                        varyantId.HasValue ? (object)varyantId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@adet", yeniAdet);
                    cmd.Parameters.AddWithValue("@et", DateTime.Now);
                    cmd.Parameters.AddWithValue("@pb",
                        string.IsNullOrWhiteSpace(paraBirimi) ? "TRY" : paraBirimi);
                    cmd.Parameters.AddWithValue("@bf", birimFiyat);
                    if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return null;
        }

        // ── Misafir → \u00dcye Birle\u015ftirme ──────────────────────────────────────

        /// <summary>Giri\u015f sonras\u0131: misafir oturumundaki sepeti \u00fcyeye ta\u015f\u0131r / birle\u015ftirir.</summary>
        public static void MisafirSepetiMerge(HttpContext ctx, int musteriId)
        {
            // Cookie'den oku (Session kaybedilmis olabilir)
            var oturumId = ctx.Request?.Cookies[MagazaYardimcisi.CookieOturumId]?.Value
                ?? ctx.Session?[MagazaYardimcisi.SessionOturumAnahtari] as string;
            if (string.IsNullOrEmpty(oturumId)) return;

            var b = new BaglantiBilgileri();
            var misafir = new DataTable();
            using (var cmd = new SqlCommand(
                @"SELECT ID, UrunId, UrunVaryantId, Adet, ParaBirimi, BirimFiyat
                  FROM Sepet WHERE OturumId=@oid AND (MusteriId IS NULL OR MusteriId=0)", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@oid", oturumId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                new SqlDataAdapter(cmd).Fill(misafir);
            }

            foreach (DataRow row in misafir.Rows)
            {
                int urunId = (int)row["UrunId"];
                int? varyantId = row["UrunVaryantId"] == DBNull.Value
                    ? (int?)null : (int)row["UrunVaryantId"];
                int adet = (int)row["Adet"];
                string pb = row["ParaBirimi"]?.ToString() ?? "TRY";
                decimal bf = (decimal)row["BirimFiyat"];
                int misafirSatirId = (int)row["ID"];

                string chkSql = "SELECT ID, Adet FROM Sepet WHERE MusteriId=@mid AND UrunId=@uid AND " +
                    (varyantId.HasValue ? "UrunVaryantId=@vid" : "UrunVaryantId IS NULL");

                int uyeSatirId = 0, uyeAdet = 0;
                using (var cmd = new SqlCommand(chkSql, b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@mid", musteriId);
                    cmd.Parameters.AddWithValue("@uid", urunId);
                    if (varyantId.HasValue) cmd.Parameters.AddWithValue("@vid", varyantId.Value);
                    if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            uyeSatirId = (int)rdr["ID"];
                            uyeAdet = (int)rdr["Adet"];
                        }
                    }
                }

                int stok = StokAl(urunId, varyantId);
                int hedef = stok > 0 ? Math.Min(uyeAdet + adet, stok) : uyeAdet + adet;

                if (uyeSatirId > 0)
                {
                    using (var cmd = new SqlCommand(
                        "UPDATE Sepet SET Adet=@a WHERE ID=@id", b.Baglanti))
                    {
                        cmd.Parameters.AddWithValue("@a", hedef);
                        cmd.Parameters.AddWithValue("@id", uyeSatirId);
                        if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = new SqlCommand(
                        "DELETE FROM Sepet WHERE ID=@id", b.Baglanti))
                    {
                        cmd.Parameters.AddWithValue("@id", misafirSatirId);
                        if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var cmd = new SqlCommand(
                        "UPDATE Sepet SET MusteriId=@mid WHERE ID=@id", b.Baglanti))
                    {
                        cmd.Parameters.AddWithValue("@mid", musteriId);
                        cmd.Parameters.AddWithValue("@id", misafirSatirId);
                        if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // ── Sayac ─────────────────────────────────────────────────────────

        /// <summary>Toplam adet (master mini-cart say\u0131s\u0131 i\u00e7in).</summary>
        public static int ToplamAdet(HttpContext ctx)
        {
            if (ctx == null) return 0;
            MagazaYardimcisi.OturumIdAl(ctx);
            var b = new BaglantiBilgileri();
            if (MusteriAuth.GirisMi(ctx.Session))
            {
                int mid = MusteriAuth.MusteriId(ctx.Session);
                using (var cmd = new SqlCommand(
                    "SELECT ISNULL(SUM(Adet),0) FROM Sepet WHERE MusteriId=@mid", b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@mid", mid);
                    if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            var oid = MagazaYardimcisi.OturumIdAl(ctx);
            if (string.IsNullOrEmpty(oid)) return 0;
            using (var cmd = new SqlCommand(
                "SELECT ISNULL(SUM(Adet),0) FROM Sepet WHERE OturumId=@oid AND (MusteriId IS NULL OR MusteriId=0)", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@oid", oid);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static DataTable Listele(HttpContext ctx)
        {
            MagazaYardimcisi.OturumIdAl(ctx);
            var b = new BaglantiBilgileri();
            string sql = @"SELECT s.ID, s.UrunId, s.Adet, s.BirimFiyat, s.ParaBirimi,
                u.UrunAdi, u.AnaResim, ISNULL(m.MarkaAdi, '') AS MarkaAdi
                FROM Sepet s
                INNER JOIN Urun u ON s.UrunId = u.ID
                LEFT JOIN Marka m ON u.MarkaId = m.ID
                WHERE ";
            if (MusteriAuth.GirisMi(ctx.Session))
            {
                sql += "s.MusteriId=@kimlik";
            }
            else
            {
                sql += "s.OturumId=@kimlik AND (s.MusteriId IS NULL OR s.MusteriId=0)";
            }
            sql += " ORDER BY s.ID DESC";

            using (var cmd = new SqlCommand(sql, b.Baglanti))
            {
                if (MusteriAuth.GirisMi(ctx.Session))
                    cmd.Parameters.AddWithValue("@kimlik", MusteriAuth.MusteriId(ctx.Session));
                else
                    cmd.Parameters.AddWithValue("@kimlik", MagazaYardimcisi.OturumIdAl(ctx));
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }

        public static decimal ToplamTutar(HttpContext ctx)
        {
            var dt = Listele(ctx);
            decimal t = 0m;
            foreach (DataRow row in dt.Rows)
                t += Convert.ToDecimal(row["BirimFiyat"]) * Convert.ToInt32(row["Adet"]);
            return t;
        }

        public static void Sil(HttpContext ctx, int sepetId)
        {
            MagazaYardimcisi.OturumIdAl(ctx);
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand("DELETE FROM Sepet WHERE ID=@id", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", sepetId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void AdetGuncelle(HttpContext ctx, int sepetId, int adet)
        {
            if (adet <= 0) { Sil(ctx, sepetId); return; }
            MagazaYardimcisi.OturumIdAl(ctx);
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand("UPDATE Sepet SET Adet=@a WHERE ID=@id", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@a", adet);
                cmd.Parameters.AddWithValue("@id", sepetId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void Temizle(HttpContext ctx)
        {
            MagazaYardimcisi.OturumIdAl(ctx);
            var b = new BaglantiBilgileri();
            string sql;
            if (MusteriAuth.GirisMi(ctx.Session))
                sql = "DELETE FROM Sepet WHERE MusteriId=@kimlik";
            else
                sql = "DELETE FROM Sepet WHERE OturumId=@kimlik AND (MusteriId IS NULL OR MusteriId=0)";
            using (var cmd = new SqlCommand(sql, b.Baglanti))
            {
                if (MusteriAuth.GirisMi(ctx.Session))
                    cmd.Parameters.AddWithValue("@kimlik", MusteriAuth.MusteriId(ctx.Session));
                else
                    cmd.Parameters.AddWithValue("@kimlik", MagazaYardimcisi.OturumIdAl(ctx));
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
