using Saat.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{
    public partial class UrunDetay : System.Web.UI.Page
    {
        private int _urunId;
        private decimal _temelFiyat;

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
                if (!UrunGetir(_urunId))
                    return;
                IncelemeSayisiArtir(_urunId);
                IlgiTakipHelper.GoruntulemeKaydet(Context, _urunId);
                OnerileriGetir(_urunId);
            }
            else
            {
                _urunId = ViewState["UrunId"] != null ? Convert.ToInt32(ViewState["UrunId"]) : UrunIdAl();
                if (ViewState["TemelFiyat"] != null)
                    _temelFiyat = Convert.ToDecimal(ViewState["TemelFiyat"]);
                if (_urunId > 0 && ViewState["VaryantSayisi"] != null && Convert.ToInt32(ViewState["VaryantSayisi"]) > 0)
                {
                    string kasaSel = Request[ddlKasa.UniqueID];
                    string renkSel = Request[ddlRenk.UniqueID];
                    VaryantlariYukle(_urunId, kasaSel, renkSel);
                }
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

        protected void VaryantSecimiDegisti(object sender, EventArgs e)
        {
            SeciliVaryantiUygula();
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

        private bool UrunGetir(int id)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (var com = new SqlCommand(@"
                SELECT u.ID, u.UrunAdi, u.Fiyat, u.IndirimliFiyat, u.AnaResim, u.KisaAciklama,
                       (SELECT MarkaAdi FROM Marka WHERE ID = u.MarkaId) AS MarkaAdi
                FROM Urun u WHERE u.ID=@ID AND u.Aktif=1", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                if (com.Connection.State == ConnectionState.Closed) b.Baglanti.Open();
                using (var dr = com.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        pnlUrun.Visible = false;
                        pnlYok.Visible = true;
                        return false;
                    }

                    ViewState["UrunId"] = id;
                    Title = EncodingHelper.DuzeltTurkce(dr["UrunAdi"].ToString());
                    ltrMarka.Text = EncodingHelper.DuzeltTurkce(dr["MarkaAdi"]?.ToString() ?? "");
                    ltrUrunAdi.Text = EncodingHelper.DuzeltTurkce(dr["UrunAdi"].ToString());
                    ltrAciklama.Text = EncodingHelper.DuzeltTurkce(dr["KisaAciklama"]?.ToString() ?? "");

                    string anaResim = dr["AnaResim"]?.ToString() ?? "";
                    ResimGalerisiOlustur(id, anaResim);

                    decimal f = Convert.ToDecimal(dr["Fiyat"]);
                    decimal? ind = dr["IndirimliFiyat"] != DBNull.Value ? (decimal?)Convert.ToDecimal(dr["IndirimliFiyat"]) : null;
                    _temelFiyat = ind.HasValue && ind.Value > 0 && ind.Value < f ? ind.Value : f;
                    ViewState["TemelFiyat"] = _temelFiyat;
                    ltrFiyat.Text = _temelFiyat.ToString("N2") + " TL";

                    pnlUrun.Visible = true;
                    pnlYok.Visible = false;
                }
            }

            VaryantlariYukle(id, null, null);
            return true;
        }

        private void ResimGalerisiOlustur(int urunId, string anaResim)
        {
            var resimler = new List<string>();
            if (!string.IsNullOrWhiteSpace(anaResim))
                resimler.Add(anaResim.Trim());

            BaglantiBilgileri b = new BaglantiBilgileri();
            using (var com = new SqlCommand(
                "SELECT Resim FROM UrunResim WHERE UrunId=@ID ORDER BY Siralama, ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", urunId);
                if (com.Connection.State == ConnectionState.Closed) b.Baglanti.Open();
                using (var dr = com.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string r = dr["Resim"]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(r) && !ResimListedeVar(resimler, r))
                            resimler.Add(r);
                    }
                }
            }

            if (resimler.Count == 0)
            {
                ltrResim.Text = "<img src='/assets/img/no-image.png' alt='' class='img-fluid' />";
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<div class='product-details-thumbnail'>");
            sb.AppendFormat("<div class='large-img mb-3'><img id='urunAnaResim' src='/Upload/{0}' alt='' class='img-fluid' /></div>",
                Server.HtmlEncode(resimler[0]));

            if (resimler.Count > 1)
            {
                sb.Append("<div class='d-flex flex-wrap'>");
                for (int i = 0; i < resimler.Count; i++)
                {
                    sb.AppendFormat(
                        "<a href='javascript:void(0)' class='mr-2 mb-2' onclick=\"document.getElementById('urunAnaResim').src='/Upload/{0}'\">" +
                        "<img src='/Upload/{0}' alt='' style='width:72px;height:72px;object-fit:cover;border:1px solid #ddd;' /></a>",
                        Server.HtmlEncode(resimler[i]));
                }
                sb.Append("</div>");
            }
            sb.Append("</div>");
            ltrResim.Text = sb.ToString();
        }

        static bool ResimListedeVar(List<string> liste, string ad)
        {
            foreach (var s in liste)
                if (string.Equals(s, ad, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        private DataTable VaryantTablosuAl(int urunId)
        {
            var dt = new DataTable();
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (var com = new SqlCommand(@"
                SELECT v.ID, v.KasaId, v.RenkId, v.StokAdedi, ISNULL(v.FiyatFarki, 0) AS FiyatFarki,
                       kb.kasano, r.RenkAdi
                FROM UrunVaryant v
                LEFT JOIN kasaboyutu kb ON v.KasaId = kb.ID
                LEFT JOIN Renk r ON v.RenkId = r.ID
                WHERE v.UrunId=@ID AND v.StokAdedi > 0
                ORDER BY kb.Siralama, kb.kasano, r.Siralama, r.RenkAdi", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", urunId);
                if (com.Connection.State == ConnectionState.Closed) b.Baglanti.Open();
                dt.Load(com.ExecuteReader());
            }
            foreach (DataRow row in dt.Rows)
            {
                if (row["kasano"] != DBNull.Value)
                    row["kasano"] = EncodingHelper.DuzeltTurkce(row["kasano"].ToString());
                if (row["RenkAdi"] != DBNull.Value)
                    row["RenkAdi"] = EncodingHelper.DuzeltTurkce(row["RenkAdi"].ToString());
            }
            return dt;
        }

        private void VaryantlariYukle(int urunId, string seciliKasaId, string seciliRenkId)
        {
            DataTable dt = VaryantTablosuAl(urunId);
            ViewState["VaryantSayisi"] = dt.Rows.Count;

            if (dt.Rows.Count == 0)
            {
                pnlVaryant.Visible = false;
                ViewState["VaryantId"] = null;
                return;
            }

            pnlVaryant.Visible = true;

            ddlKasa.Items.Clear();
            ddlKasa.Items.Add(new ListItem("— Seçiniz —", ""));
            var kasalar = new HashSet<string>();
            foreach (DataRow row in dt.Rows)
            {
                if (row["KasaId"] == DBNull.Value) continue;
                string key = row["KasaId"].ToString();
                if (!kasalar.Add(key)) continue;
                ddlKasa.Items.Add(new ListItem(
                    row["kasano"]?.ToString() ?? key,
                    key));
            }

            ddlRenk.Items.Clear();
            ddlRenk.Items.Add(new ListItem("— Seçiniz —", ""));
            var renkler = new HashSet<string>();
            foreach (DataRow row in dt.Rows)
            {
                if (row["RenkId"] == DBNull.Value) continue;
                string key = row["RenkId"].ToString();
                if (!renkler.Add(key)) continue;
                ddlRenk.Items.Add(new ListItem(
                    row["RenkAdi"]?.ToString() ?? key,
                    key));
            }

            if (!string.IsNullOrEmpty(seciliKasaId) && ddlKasa.Items.FindByValue(seciliKasaId) != null)
                ddlKasa.SelectedValue = seciliKasaId;
            else if (ddlKasa.Items.Count == 2)
                ddlKasa.SelectedIndex = 1;

            if (!string.IsNullOrEmpty(seciliRenkId) && ddlRenk.Items.FindByValue(seciliRenkId) != null)
                ddlRenk.SelectedValue = seciliRenkId;
            else if (ddlRenk.Items.Count == 2)
                ddlRenk.SelectedIndex = 1;

            SeciliVaryantiUygula();
        }

        private void SeciliVaryantiUygula()
        {
            int urunId = ViewState["UrunId"] != null ? Convert.ToInt32(ViewState["UrunId"]) : 0;
            if (urunId <= 0) return;

            _temelFiyat = ViewState["TemelFiyat"] != null ? Convert.ToDecimal(ViewState["TemelFiyat"]) : 0m;
            DataTable dt = VaryantTablosuAl(urunId);

            int? kasaId = string.IsNullOrEmpty(ddlKasa.SelectedValue) ? (int?)null : Convert.ToInt32(ddlKasa.SelectedValue);
            int? renkId = string.IsNullOrEmpty(ddlRenk.SelectedValue) ? (int?)null : Convert.ToInt32(ddlRenk.SelectedValue);

            DataRow eslesen = null;
            foreach (DataRow row in dt.Rows)
            {
                int? rKasa = row["KasaId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["KasaId"]);
                int? rRenk = row["RenkId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["RenkId"]);
                if (rKasa == kasaId && rRenk == renkId)
                {
                    eslesen = row;
                    break;
                }
            }

            if (eslesen == null)
            {
                ViewState["VaryantId"] = null;
                lblStok.Text = "Lütfen kasa çapı ve renk seçin.";
                ltrFiyat.Text = _temelFiyat.ToString("N2") + " TL";
                return;
            }

            int varyantId = Convert.ToInt32(eslesen["ID"]);
            int stok = Convert.ToInt32(eslesen["StokAdedi"]);
            decimal fark = Convert.ToDecimal(eslesen["FiyatFarki"]);
            decimal fiyat = _temelFiyat + fark;

            ViewState["VaryantId"] = varyantId;
            ltrFiyat.Text = fiyat.ToString("N2") + " TL";
            lblStok.Text = "Stok: " + stok + " adet";
        }

        protected void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            int id = ViewState["UrunId"] != null ? Convert.ToInt32(ViewState["UrunId"]) : UrunIdAl();
            if (id <= 0) return;

            int adet = 1;
            int.TryParse(txtAdet.Text, out adet);
            if (adet < 1) adet = 1;

            int varyantSayisi = ViewState["VaryantSayisi"] != null ? Convert.ToInt32(ViewState["VaryantSayisi"]) : 0;
            int? varyantId = ViewState["VaryantId"] as int?;
            if (varyantSayisi > 0 && !varyantId.HasValue)
            {
                lblMesaj.Text = "Lütfen kasa çapı ve renk seçin.";
                lblMesaj.CssClass = "text-danger d-block mt-2";
                return;
            }

            decimal birimFiyat = BirimFiyatAl(id, varyantId);
            string hata = SepetHelper.Ekle(Context, id, varyantId, adet, "TRY", birimFiyat);
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

        private decimal BirimFiyatAl(int urunId, int? varyantId)
        {
            decimal temel = ViewState["TemelFiyat"] != null ? Convert.ToDecimal(ViewState["TemelFiyat"]) : 0m;
            if (temel <= 0)
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
                                if (ind > 0 && ind < f) temel = ind;
                                else temel = f;
                            }
                            else temel = f;
                        }
                    }
                }
            }

            if (!varyantId.HasValue) return temel;

            BaglantiBilgileri b2 = new BaglantiBilgileri();
            using (var com = new SqlCommand(
                "SELECT ISNULL(FiyatFarki,0) FROM UrunVaryant WHERE ID=@ID AND UrunId=@UID", b2.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", varyantId.Value);
                com.Parameters.AddWithValue("@UID", urunId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                var r = com.ExecuteScalar();
                if (r != null && r != DBNull.Value)
                    temel += Convert.ToDecimal(r);
            }
            return temel;
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
