using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Saat
{
    public partial class UrunDetay : System.Web.UI.Page
    {
        private int _urunId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _urunId = UrunIdAl();
                if (_urunId <= 0)
                {
                    pnlUrun.Visible = false;
                    pnlYok.Visible = true;
                    return;
                }
                UrunGetir(_urunId);
                IncelemeSayisiArtir(_urunId);
                IlgiTakipHelper.GoruntulemeKaydet(Context, _urunId);
                OnerileriGetir(_urunId);
            }
        }

        public string UrunLink(object id, object ad)
        {
            if (id == null || ad == null) return "/urunler";
            return string.Format("/urundetay/{0}/{1}", id, UrunSeoSlug.Olustur(ad.ToString()));
        }

        public string FiyatGoster(object fiyat, object indirimli)
        {
            decimal f = fiyat != null && fiyat != DBNull.Value ? Convert.ToDecimal(fiyat) : 0m;
            decimal? ind = indirimli != null && indirimli != DBNull.Value ? (decimal?)Convert.ToDecimal(indirimli) : null;
            if (ind.HasValue && ind.Value > 0 && ind.Value < f)
                return ind.Value.ToString("N2") + " TL";
            return f.ToString("N2") + " TL";
        }

        private void OnerileriGetir(int urunId)
        {
            DataTable dt = OneriMotoru.OnerileriGetir(Context, 4, urunId);
            rptOneriler.DataSource = dt;
            rptOneriler.DataBind();
            pnlOneriler.Visible = dt.Rows.Count > 0;
        }

        private int UrunIdAl()
        {
            if (Page.RouteData.Values["ID"] != null)
            {
                int id;
                if (int.TryParse(Page.RouteData.Values["ID"].ToString(), out id)) return id;
            }
            int q;
            if (int.TryParse(Request.QueryString["id"], out q)) return q;
            return 0;
        }

        private void UrunGetir(int id)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            SqlCommand com = new SqlCommand(@"
                SELECT u.ID, u.UrunAdi, u.Fiyat, u.IndirimliFiyat, u.AnaResim, u.KisaAciklama,
                       (SELECT MarkaAdi FROM Marka WHERE ID = u.MarkaId) AS MarkaAdi
                FROM Urun u WHERE u.ID=@ID AND u.Aktif=1", b.Baglanti);
            com.Parameters.AddWithValue("@ID", id);

            if (com.Connection.State == ConnectionState.Closed) b.Baglanti.Open();
            SqlDataReader dr = com.ExecuteReader();
            if (dr.Read())
            {
                ViewState["UrunId"] = id;
                Title = EncodingHelper.DuzeltTurkce(dr["UrunAdi"].ToString());
                ltrMarka.Text = EncodingHelper.DuzeltTurkce(dr["MarkaAdi"]?.ToString() ?? "");
                ltrUrunAdi.Text = EncodingHelper.DuzeltTurkce(dr["UrunAdi"].ToString());
                ltrAciklama.Text = EncodingHelper.DuzeltTurkce(dr["KisaAciklama"]?.ToString() ?? "");
                ltrResim.Text = "<img src='/Upload/" + dr["AnaResim"] + "' alt='' class='img-fluid' />";
                decimal f = Convert.ToDecimal(dr["Fiyat"]);
                decimal? ind = dr["IndirimliFiyat"] != DBNull.Value ? (decimal?)Convert.ToDecimal(dr["IndirimliFiyat"]) : null;
                ltrFiyat.Text = (ind.HasValue && ind.Value > 0 && ind.Value < f ? ind.Value : f).ToString("N2") + " TL";
                pnlUrun.Visible = true;
                pnlYok.Visible = false;
            }
            else
            {
                pnlUrun.Visible = false;
                pnlYok.Visible = true;
            }
            dr.Close();
            b.Baglanti.Close();
        }

        protected void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            int id = ViewState["UrunId"] != null ? Convert.ToInt32(ViewState["UrunId"]) : UrunIdAl();
            if (id <= 0) return;

            int adet = 1;
            int.TryParse(txtAdet.Text, out adet);
            if (adet < 1) adet = 1;

            decimal birimFiyat = BirimFiyatAl(id);
            string hata = SepetHelper.Ekle(Context, id, null, adet, "TRY", birimFiyat);
            if (hata == null)
            {
                lblMesaj.Text = "Ürün sepete eklendi.";
                lblMesaj.CssClass = "text-success d-block mt-2";
            }
            else
            {
                lblMesaj.Text = hata;
                lblMesaj.CssClass = "text-danger d-block mt-2";
            }
        }

        private decimal BirimFiyatAl(int urunId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (var com = new SqlCommand("SELECT Fiyat, IndirimliFiyat FROM Urun WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", urunId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                using (var dr = com.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        decimal f = Convert.ToDecimal(dr["Fiyat"]);
                        if (dr["IndirimliFiyat"] != DBNull.Value)
                        {
                            decimal ind = Convert.ToDecimal(dr["IndirimliFiyat"]);
                            if (ind > 0 && ind < f) return ind;
                        }
                        return f;
                    }
                }
            }
            return 0m;
        }

        private void IncelemeSayisiArtir(int urunId)
        {
            try
            {
                BaglantiBilgileri b = new BaglantiBilgileri();
                using (var com = new SqlCommand(
                    "UPDATE Urun SET IncelemeSayisi = ISNULL(IncelemeSayisi,0) + 1 WHERE ID=@ID", b.Baglanti))
                {
                    com.Parameters.AddWithValue("@ID", urunId);
                    if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                    com.ExecuteNonQuery();
                }
            }
            catch
            {
                /* kolon henüz yoksa sessizce geç */
            }
        }
    }
}
