using Saat.App_Code;
using Saat.Saat0604;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{
    public partial class Urunler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Ürünler";
                UrunleriGetir();
                KategoriddlDoldur();
            }
        }

        protected void KategoriddlDoldur()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            SqlCommand com = new SqlCommand("SELECT ID, KategoriAdi FROM Kategori WHERE aktif=@aktif  and  UstId=@UstId ORDER BY siralama", b.Baglanti);

            com.Parameters.AddWithValue("@aktif", true);
            com.Parameters.AddWithValue("@UstId", 0);

            if (com.Connection.State == System.Data.ConnectionState.Closed)
            {
                b.Baglanti.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            ddlKategoriEkle.Items.Clear();
            ddlKategoriEkle.Items.Add(new ListItem("Filtrele", "0"));
            while (dr.Read())
            {
                ddlKategoriEkle.Items.Add(new ListItem(EncodingHelper.DuzeltTurkce(dr["KategoriAdi"].ToString()), dr["ID"].ToString()));
            }

            dr.Close();
            b.Baglanti.Close();
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected string UrunLink(object id, object ad)
        {
            if (id == null || ad == null) return "/urunler";
            return string.Format("/urundetay/{0}/{1}", id, UrunSeoSlug.Olustur(ad.ToString()));
        }

        protected string FiyatGoster(object fiyat, object indirimli)
        {
            decimal f = fiyat != null && fiyat != DBNull.Value ? Convert.ToDecimal(fiyat) : 0m;
            decimal? ind = indirimli != null && indirimli != DBNull.Value ? (decimal?)Convert.ToDecimal(indirimli) : null;
            if (ind.HasValue && ind.Value > 0 && ind.Value < f)
                return ind.Value.ToString("N2") + " TL";
            return f.ToString("N2") + " TL";
        }

        protected bool RenkVarMi(object renkOzet)
        {
            return renkOzet != null && renkOzet != DBNull.Value && !string.IsNullOrWhiteSpace(renkOzet.ToString());
        }

        protected string RenkSwatchHtml(object renkKodlari)
        {
            if (renkKodlari == null || renkKodlari == DBNull.Value) return "";
            var sb = new System.Text.StringBuilder();
            foreach (var kod in renkKodlari.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var k = kod.Trim();
                if (string.IsNullOrEmpty(k)) continue;
                if (!k.StartsWith("#")) k = "#" + k;
                sb.AppendFormat(
                    "<span title='Renk' style='display:inline-block;width:16px;height:16px;border-radius:50%;background:{0};border:1px solid #ccc;margin-right:4px;'></span>",
                    HttpUtility.HtmlAttributeEncode(k));
            }
            return sb.ToString();
        }

        private static string AramaMetniAl()
        {
            string q = HttpContext.Current?.Request?.QueryString["q"];
            if (string.IsNullOrWhiteSpace(q))
                q = HttpContext.Current?.Request?.QueryString["search"];
            if (string.IsNullOrWhiteSpace(q)) return "";
            return q.Trim();
        }

        private void UrunleriGetir()
        {
            string arama = AramaMetniAl();
            bool aramaVar = !string.IsNullOrEmpty(arama);

            if (aramaVar)
            {
                ltrBaslik.Text = "Arama Sonuçları";
                pnlAramaOzet.Visible = true;
                ltrAramaOzet.Text = "Aranan: <strong>" + HttpUtility.HtmlEncode(arama) + "</strong> — "
                    + "<a href=\"/urunler\">Tüm ürünleri göster</a>";
            }

            using (var b = new BaglantiBilgileri())
            using (var com = new SqlCommand(@"
                SELECT u.ID, u.UrunAdi, u.Fiyat, u.IndirimliFiyat, u.AnaResim,
                       ISNULL(m.MarkaAdi, N'') AS MarkaAdi,
                       ISNULL((
                           SELECT STUFF((
                               SELECT DISTINCT N', ' + r.RenkAdi
                               FROM UrunVaryant v
                               INNER JOIN Renk r ON v.RenkId = r.ID
                               WHERE v.UrunId = u.ID AND r.Aktif = 1
                               FOR XML PATH(''), TYPE
                           ).value('.', 'NVARCHAR(MAX)'), 1, 2, N'')
                       ), N'') AS RenkOzet,
                       ISNULL((
                           SELECT STUFF((
                               SELECT DISTINCT N',' + ISNULL(NULLIF(LTRIM(RTRIM(r.RenkKodu)), N''), N'#999')
                               FROM UrunVaryant v
                               INNER JOIN Renk r ON v.RenkId = r.ID
                               WHERE v.UrunId = u.ID AND r.Aktif = 1
                               FOR XML PATH(''), TYPE
                           ).value('.', 'NVARCHAR(MAX)'), 1, 1, N'')
                       ), N'') AS RenkKodlari
                FROM Urun u
                LEFT JOIN Marka m ON u.MarkaId = m.ID
                WHERE u.Aktif = 1
                  AND (u.StokAdedi > 0 OR EXISTS (
                      SELECT 1 FROM UrunVaryant v WHERE v.UrunId = u.ID AND v.StokAdedi > 0))
                  AND (@Arama = N'' OR u.UrunAdi LIKE @AramaLike OR m.MarkaAdi LIKE @AramaLike)
                ORDER BY u.Siralama, u.ID DESC", b.Baglanti))
            {
                com.Parameters.AddWithValue("@Arama", aramaVar ? arama : (object)"");
                com.Parameters.AddWithValue("@AramaLike", aramaVar ? "%" + arama + "%" : (object)"%");

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                var dt = new DataTable();
                dt.Load(com.ExecuteReader());

                if (dt.Rows.Count > 0)
                {
                    rptUrunler.DataSource = dt;
                    rptUrunler.DataBind();
                    pnlBos.Visible = false;
                }
                else
                {
                    rptUrunler.DataSource = null;
                    rptUrunler.DataBind();
                    pnlBos.Visible = true;
                    ltrBosMesaj.Text = aramaVar
                        ? "Aramanızla eşleşen ürün bulunamadı."
                        : "Şu an listelenecek ürün bulunmuyor.";
                }
            }
        }

        private void UrunleriGetir(string KategoriID)
        {
            string arama = AramaMetniAl();
            bool aramaVar = !string.IsNullOrEmpty(arama);

            if (aramaVar)
            {
                ltrBaslik.Text = "Arama Sonuçları";
                pnlAramaOzet.Visible = true;
                ltrAramaOzet.Text = "Aranan: <strong>" + HttpUtility.HtmlEncode(arama) + "</strong> — "
                    + "<a href=\"/urunler\">Tüm ürünleri göster</a>";
            }

            using (var b = new BaglantiBilgileri())
            using (var com = new SqlCommand(@"
    SELECT u.ID, u.UrunAdi, u.Fiyat, u.IndirimliFiyat, u.AnaResim,
           ISNULL(m.MarkaAdi, N'') AS MarkaAdi,
           ISNULL((
               SELECT STUFF((
                   SELECT DISTINCT N', ' + r.RenkAdi
                   FROM UrunVaryant v
                   INNER JOIN Renk r ON v.RenkId = r.ID
                   WHERE v.UrunId = u.ID AND r.Aktif = 1
                   FOR XML PATH(''), TYPE
               ).value('.', 'NVARCHAR(MAX)'), 1, 2, N'')
           ), N'') AS RenkOzet,
           ISNULL((
               SELECT STUFF((
                   SELECT DISTINCT N',' + ISNULL(NULLIF(LTRIM(RTRIM(r.RenkKodu)), N''), N'#999')
                   FROM UrunVaryant v
                   INNER JOIN Renk r ON v.RenkId = r.ID
                   WHERE v.UrunId = u.ID AND r.Aktif = 1
                   FOR XML PATH(''), TYPE
               ).value('.', 'NVARCHAR(MAX)'), 1, 1, N'')
           ), N'') AS RenkKodlari
    FROM Urun u
    LEFT JOIN Marka m ON u.MarkaId = m.ID
    WHERE u.Aktif = 1
      AND (@KategoriID = N'' OR EXISTS (
          SELECT 1
          FROM UrunKategori uk
          WHERE uk.UrunId = u.ID
            AND uk.KategoriId = @KategoriID
      ))
      AND (u.StokAdedi > 0 OR EXISTS (
          SELECT 1 FROM UrunVaryant v WHERE v.UrunId = u.ID AND v.StokAdedi > 0
      ))
      AND (@Arama = N'' OR u.UrunAdi LIKE @AramaLike OR m.MarkaAdi LIKE @AramaLike)
    ORDER BY u.Siralama, u.ID DESC", b.Baglanti))
            {
                com.Parameters.AddWithValue("@KategoriID", string.IsNullOrEmpty(KategoriID) ? "" : KategoriID);
                com.Parameters.AddWithValue("@Arama", aramaVar ? arama : (object)"");
                com.Parameters.AddWithValue("@AramaLike", aramaVar ? "%" + arama + "%" : (object)"%");

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                var dt = new DataTable();
                dt.Load(com.ExecuteReader());

                if (dt.Rows.Count > 0)
                {
                    rptUrunler.DataSource = dt;
                    rptUrunler.DataBind();
                    pnlBos.Visible = false;
                }
                else
                {
                    rptUrunler.DataSource = null;
                    rptUrunler.DataBind();
                    pnlBos.Visible = true;
                    ltrBosMesaj.Text = aramaVar
                        ? "Aramanızla eşleşen ürün bulunamadı."
                        : "Şu an listelenecek ürün bulunmuyor.";
                }
            }
        }

        protected void ddlKategoriEkle_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblKategoriID.Text = ddlKategoriEkle.SelectedValue;

            if (lblKategoriID.Text == "0") 
            {
                UrunleriGetir();
            }
            else 
            {
            UrunleriGetir(lblKategoriID.Text);           
            }

        }
    }
}
