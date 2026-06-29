using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

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
            }
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

        private void UrunleriGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            SqlCommand com = new SqlCommand(@"
                SELECT u.ID, u.UrunAdi, u.Fiyat, u.IndirimliFiyat, u.AnaResim,
                       (SELECT MarkaAdi FROM Marka WHERE ID = u.MarkaId) AS MarkaAdi
                FROM Urun u
                WHERE u.Aktif = 1 AND u.StokAdedi > 0
                ORDER BY u.Siralama, u.ID DESC", b.Baglanti);

            if (com.Connection.State == ConnectionState.Closed) b.Baglanti.Open();
            SqlDataReader dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                rptUrunler.DataSource = dt;
                rptUrunler.DataBind();
                pnlBos.Visible = false;
            }
            else
            {
                pnlBos.Visible = true;
            }
            dr.Close();
            b.Baglanti.Close();
        }
    }
}
