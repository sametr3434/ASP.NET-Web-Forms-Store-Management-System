using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace Saat.App_Code
{
    public static class OneriMotoru
    {
        public static DataTable OnerileriGetir(HttpContext ctx, int adet, int? haricUrunId = null)
        {
            if (ctx == null || adet <= 0) adet = 6;
            MagazaYardimcisi.OturumIdAl(ctx);

            try
            {
                int? markaId = OneCikanMarka(ctx);
                if (markaId.HasValue && markaId.Value > 0)
                {
                    var dt = MarkadanUrunler(markaId.Value, adet, haricUrunId, ctx);
                    if (dt.Rows.Count >= adet) return dt;
                    var ek = VitrinUrunler(adet - dt.Rows.Count, haricUrunId, ctx, dt);
                    return ek;
                }
            }
            catch
            {
                /* log tablosu yoksa vitrin */
            }

            return VitrinUrunler(adet, haricUrunId, ctx, null);
        }

        private static int? OneCikanMarka(HttpContext ctx)
        {
            using (var b = new BaglantiBilgileri())
            {
            string filtre = FiltreSql(ctx, "l");
            string sql = @"
                SELECT TOP 1 u.MarkaId, SUM(l.Puan) AS Toplam
                FROM UrunIlgiLog l
                INNER JOIN Urun u ON l.UrunId = u.ID
                WHERE l.Tarih >= DATEADD(day, -30, GETDATE()) AND u.MarkaId IS NOT NULL " + filtre + @"
                GROUP BY u.MarkaId
                ORDER BY SUM(l.Puan) DESC";

            using (var cmd = new SqlCommand(sql, b.Baglanti))
            {
                FiltreParametre(cmd, ctx);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read() && dr["MarkaId"] != DBNull.Value)
                        return Convert.ToInt32(dr["MarkaId"]);
                }
            }
            return null;
            }
        }

        private static DataTable MarkadanUrunler(int markaId, int adet, int? haricUrunId, HttpContext ctx)
        {
            using (var b = new BaglantiBilgileri())
            {
            var sql = new StringBuilder(@"
                SELECT TOP (@Adet) u.ID, u.UrunAdi, u.Fiyat, u.IndirimliFiyat, u.AnaResim,
                       m.MarkaAdi
                FROM Urun u
                LEFT JOIN Marka m ON u.MarkaId = m.ID
                WHERE u.Aktif = 1 AND u.StokAdedi > 0 AND u.MarkaId = @MarkaId ");
            if (haricUrunId.HasValue) sql.Append(" AND u.ID <> @Haric ");
            sql.Append(HaricBakilanlarSql(ctx, "u.ID"));
            sql.Append(" ORDER BY u.Siralama, u.ID DESC");

            using (var cmd = new SqlCommand(sql.ToString(), b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@Adet", adet);
                cmd.Parameters.AddWithValue("@MarkaId", markaId);
                if (haricUrunId.HasValue) cmd.Parameters.AddWithValue("@Haric", haricUrunId.Value);
                FiltreParametre(cmd, ctx);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                TurkceDuzelt(dt);
                return dt;
            }
            }
        }

        private static DataTable VitrinUrunler(int adet, int? haricUrunId, HttpContext ctx, DataTable mevcut)
        {
            using (var b = new BaglantiBilgileri())
            {
            var sql = new StringBuilder(@"
                SELECT TOP (@Adet) u.ID, u.UrunAdi, u.Fiyat, u.IndirimliFiyat, u.AnaResim,
                       m.MarkaAdi
                FROM Urun u
                LEFT JOIN Marka m ON u.MarkaId = m.ID
                WHERE u.Aktif = 1 AND u.StokAdedi > 0 ");
            if (haricUrunId.HasValue) sql.Append(" AND u.ID <> @Haric ");
            sql.Append(HaricBakilanlarSql(ctx, "u.ID"));
            if (mevcut != null && mevcut.Rows.Count > 0)
            {
                sql.Append(" AND u.ID NOT IN (");
                for (int i = 0; i < mevcut.Rows.Count; i++)
                {
                    if (i > 0) sql.Append(",");
                    sql.Append(mevcut.Rows[i]["ID"]);
                }
                sql.Append(") ");
            }
            sql.Append(" ORDER BY u.Vitrin DESC, u.Onerilen DESC, u.Siralama, u.ID DESC");

            using (var cmd = new SqlCommand(sql.ToString(), b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@Adet", adet);
                if (haricUrunId.HasValue) cmd.Parameters.AddWithValue("@Haric", haricUrunId.Value);
                FiltreParametre(cmd, ctx);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                var dt = mevcut ?? new DataTable();
                if (mevcut == null)
                {
                    dt.Load(cmd.ExecuteReader());
                }
                else
                {
                    var ek = new DataTable();
                    ek.Load(cmd.ExecuteReader());
                    foreach (DataRow row in ek.Rows)
                        dt.ImportRow(row);
                }
                TurkceDuzelt(dt);
                return dt;
            }
            }
        }

        private static string FiltreSql(HttpContext ctx, string alias)
        {
            if (MusteriAuth.GirisMi(ctx.Session))
                return " AND " + alias + ".MusteriId = @Kimlik ";
            return " AND " + alias + ".OturumId = @Kimlik ";
        }

        private static string HaricBakilanlarSql(HttpContext ctx, string urunKolon)
        {
            if (MusteriAuth.GirisMi(ctx.Session))
                return " AND " + urunKolon + " NOT IN (SELECT DISTINCT UrunId FROM UrunIlgiLog WHERE MusteriId = @Kimlik AND Tarih >= DATEADD(day,-30,GETDATE())) ";
            return " AND " + urunKolon + " NOT IN (SELECT DISTINCT UrunId FROM UrunIlgiLog WHERE OturumId = @Kimlik AND Tarih >= DATEADD(day,-30,GETDATE())) ";
        }

        private static void FiltreParametre(SqlCommand cmd, HttpContext ctx)
        {
            if (MusteriAuth.GirisMi(ctx.Session))
                cmd.Parameters.AddWithValue("@Kimlik", MusteriAuth.MusteriId(ctx.Session));
            else
                cmd.Parameters.AddWithValue("@Kimlik", MagazaYardimcisi.OturumIdAl(ctx));
        }

        private static void TurkceDuzelt(DataTable dt)
        {
            if (dt == null) return;
            foreach (DataRow row in dt.Rows)
            {
                if (row["UrunAdi"] != DBNull.Value)
                    row["UrunAdi"] = EncodingHelper.DuzeltTurkce(row["UrunAdi"].ToString());
                if (row.Table.Columns.Contains("MarkaAdi") && row["MarkaAdi"] != DBNull.Value)
                    row["MarkaAdi"] = EncodingHelper.DuzeltTurkce(row["MarkaAdi"].ToString());
            }
        }
    }
}
