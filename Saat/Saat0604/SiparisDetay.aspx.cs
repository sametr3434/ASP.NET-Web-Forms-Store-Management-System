using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class SiparisDetay : Page
    {
        private string _siparisPara = ParaBirimiHelper.Try;

        protected string TutarKalem(object tutar, object paraBirimi)
        {
            if (tutar == null || tutar == DBNull.Value) return "-";
            var pb = paraBirimi != null && paraBirimi != DBNull.Value ? paraBirimi.ToString() : _siparisPara;
            return ParaBirimiHelper.Format(Convert.ToDecimal(tutar), pb);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect("SiparisListe.aspx");
                    return;
                }
                int id = Convert.ToInt32(Request.QueryString["id"]);
                SiparisBilgileriniGetir(id);
                KalemleriGetir(id);
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        private void SiparisBilgileriniGetir(int id)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                SELECT s.*, ISNULL(m.Ad + ' ' + m.Soyad, ISNULL(s.MisafirAd + ' ' + s.MisafirSoyad, 'Misafir')) AS MusteriAdi
                FROM Siparis s
                LEFT JOIN Musteri m ON s.MusteriId = m.ID
                WHERE s.ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    lblSiparisNo.Text = Duzelt(dr["SiparisNo"]);
                    lblTarih.Text = dr["SiparisTarihi"] != DBNull.Value ? Convert.ToDateTime(dr["SiparisTarihi"]).ToString("dd.MM.yyyy HH:mm") : "";
                    lblMusteri.Text = Duzelt(dr["MusteriAdi"]);
                    lblOdeme.Text = Duzelt(dr["OdemeDurumu"]);
                    ddlSiparisDurumu.SelectedValue = Duzelt(dr["SiparisDurumu"]);
                    txtKargo.Text = Duzelt(dr["KargoTakipNo"]);
                    try
                    {
                        _siparisPara = ParaBirimiHelper.Normalize(dr["ParaBirimi"] != DBNull.Value && dr["ParaBirimi"] != null ? dr["ParaBirimi"].ToString() : ParaBirimiHelper.Try);
                    }
                    catch { _siparisPara = ParaBirimiHelper.Try; }
                    var kurTxt = new System.Text.StringBuilder();
                    kurTxt.Append("Para birimi: ").Append(_siparisPara);
                    try
                    {
                        if (dr["KurUSD"] != DBNull.Value && dr["KurUSD"] != null)
                            kurTxt.Append(" | 1 USD = ").Append(Convert.ToDecimal(dr["KurUSD"]).ToString("N4", CultureInfo.InvariantCulture)).Append(" TL");
                        if (dr["KurEUR"] != DBNull.Value && dr["KurEUR"] != null)
                            kurTxt.Append(" | 1 EUR = ").Append(Convert.ToDecimal(dr["KurEUR"]).ToString("N4", CultureInfo.InvariantCulture)).Append(" TL");
                        if (dr["OdemeTutarTRY"] != DBNull.Value && dr["OdemeTutarTRY"] != null)
                            kurTxt.Append(" | PayTR tahsilat: ").Append(Convert.ToDecimal(dr["OdemeTutarTRY"]).ToString("N2", CultureInfo.InvariantCulture)).Append(" TL");
                    }
                    catch { }
                    lblParaBirimi.Text = kurTxt.ToString();
                }
                com.Connection.Close();
            }
        }

        private void KalemleriGetir(int id)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                SELECT sd.ID, sd.UrunAdi, sd.UrunCapi, sd.Renk, sd.Adet, sd.BirimFiyat, sd.ToplamFiyat,
                       ISNULL(sd.ParaBirimi, ISNULL(s.ParaBirimi, N'TRY')) AS ParaBirimi
                FROM SiparisDetay sd
                INNER JOIN Siparis s ON sd.SiparisId = s.ID
                WHERE sd.SiparisId=@ID ORDER BY sd.ID ASC", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["UrunAdi"] != DBNull.Value) row["UrunAdi"] = Duzelt(row["UrunAdi"]);
                    if (row["UrunCapi"] != DBNull.Value) row["UrunCapi"] = Duzelt(row["UrunCapi"]);
                    if (row["Renk"] != DBNull.Value) row["Renk"] = Duzelt(row["Renk"]);
                }
                rptKalemler.DataSource = dt;
                rptKalemler.DataBind();
                decimal ara = 0, ind = 0, kargo = 0, toplam = 0;
                using (SqlCommand c2 = new SqlCommand("SELECT AraToplam, IndirimTutari, KargoTutari, ToplamTutar, ISNULL(ParaBirimi,N'TRY') AS ParaBirimi FROM Siparis WHERE ID=@ID", b.Baglanti))
                {
                    c2.Parameters.AddWithValue("@ID", id);
                    if (c2.Connection.State == ConnectionState.Closed) c2.Connection.Open();
                    var r = c2.ExecuteReader();
                    if (r.Read())
                    {
                        ara = r["AraToplam"] != DBNull.Value ? Convert.ToDecimal(r["AraToplam"]) : 0;
                        ind = r["IndirimTutari"] != DBNull.Value ? Convert.ToDecimal(r["IndirimTutari"]) : 0;
                        kargo = r["KargoTutari"] != DBNull.Value ? Convert.ToDecimal(r["KargoTutari"]) : 0;
                        toplam = r["ToplamTutar"] != DBNull.Value ? Convert.ToDecimal(r["ToplamTutar"]) : 0;
                        try
                        {
                            if (r["ParaBirimi"] != DBNull.Value && r["ParaBirimi"] != null)
                                _siparisPara = ParaBirimiHelper.Normalize(r["ParaBirimi"].ToString());
                        }
                        catch { }
                    }
                    c2.Connection.Close();
                }
                lblOzet.Text = "Ara: " + ParaBirimiHelper.Format(ara, _siparisPara)
                    + " | \u0130ndirim: " + ParaBirimiHelper.Format(ind, _siparisPara)
                    + " | Kargo: " + ParaBirimiHelper.Format(kargo, _siparisPara)
                    + " | Toplam: " + ParaBirimiHelper.Format(toplam, _siparisPara);
            }
        }

        protected void btnDurumGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("UPDATE Siparis SET SiparisDurumu=@Durum WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                com.Parameters.AddWithValue("@Durum", ddlSiparisDurumu.SelectedValue);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
            SiparisBilgileriniGetir(id);
            KalemleriGetir(id);
        }

        protected void btnKargoKaydet_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("UPDATE Siparis SET KargoTakipNo=@Kargo WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                com.Parameters.AddWithValue("@Kargo", string.IsNullOrWhiteSpace(txtKargo.Text) ? (object)DBNull.Value : txtKargo.Text);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
            SiparisBilgileriniGetir(id);
            KalemleriGetir(id);
        }
    }
}
