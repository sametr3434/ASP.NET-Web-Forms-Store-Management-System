
using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class MusteriDetay : Page
    {
        private int _musteriId;

        /// <summary>Sipariş listesi: toplam + para birimi.</summary>
        protected string SiparisToplamGoster(object tutar, object paraBirimi)
        {
            if (tutar == null || tutar == DBNull.Value) return "-";
            var pb = paraBirimi != null && paraBirimi != DBNull.Value
                ? ParaBirimiHelper.Normalize(paraBirimi.ToString())
                : ParaBirimiHelper.Try;
            return ParaBirimiHelper.Format(Convert.ToDecimal(tutar), pb);
        }

        /// <summary>Sepet satırı: adet * birim fiyat.</summary>
        protected string SepetSatirTutar(object adet, object birimFiyat, object paraBirimi)
        {
            if (adet == null || adet == DBNull.Value || birimFiyat == null || birimFiyat == DBNull.Value) return "-";
            var pb = paraBirimi != null && paraBirimi != DBNull.Value
                ? ParaBirimiHelper.Normalize(paraBirimi.ToString())
                : ParaBirimiHelper.Try;
            decimal d = Convert.ToDecimal(adet) * Convert.ToDecimal(birimFiyat);
            return ParaBirimiHelper.Format(d, pb);
        }

        protected string SepetBirimGoster(object birimFiyat, object paraBirimi)
        {
            if (birimFiyat == null || birimFiyat == DBNull.Value) return "-";
            var pb = paraBirimi != null && paraBirimi != DBNull.Value
                ? ParaBirimiHelper.Normalize(paraBirimi.ToString())
                : ParaBirimiHelper.Try;
            return ParaBirimiHelper.Format(Convert.ToDecimal(birimFiyat), pb);
        }

        /// <summary>Ödeme: orijinal PB tutarı varsa; yoksa TL (TutarTRY / Tutar).</summary>
        protected string OdemeTutarGoster(object tutarOrijinalPb, object paraBirimi, object tutarTry, object tutarEski)
        {
            decimal tryTutar = 0;
            if (tutarTry != null && tutarTry != DBNull.Value) tryTutar = Convert.ToDecimal(tutarTry);
            else if (tutarEski != null && tutarEski != DBNull.Value) tryTutar = Convert.ToDecimal(tutarEski);

            if (tutarOrijinalPb != null && tutarOrijinalPb != DBNull.Value
                && paraBirimi != null && paraBirimi != DBNull.Value)
            {
                var pb = ParaBirimiHelper.Normalize(paraBirimi.ToString());
                var orij = Convert.ToDecimal(tutarOrijinalPb);
                var s = ParaBirimiHelper.Format(orij, pb);
                if (pb != ParaBirimiHelper.Try && tryTutar > 0)
                    s += " (PayTR " + ParaBirimiHelper.Format(tryTutar, ParaBirimiHelper.Try) + ")";
                return s;
            }
            return ParaBirimiHelper.Format(tryTutar, ParaBirimiHelper.Try);
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect("MusteriListe.aspx");
                    return;
                }
                if (!int.TryParse(Request.QueryString["id"], out _musteriId))
                {
                    pnlMusteriYok.Visible = true;
                    pnlIcerik.Visible = false;
                    return;
                }
                if (!MusteriBilgileriniGetir())
                    return;
                SiparisleriGetir();
                SepetiGetir();
                OdemeGecmisiniGetir();
            }
        }

        private bool MusteriBilgileriniGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(
                "SELECT ID, Ad, Soyad, EPosta, Telefon, Adres, Il, Ilce, PostaKodu, KayitTarihi, SonGirisTarihi, Aktif FROM Musteri WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", _musteriId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    pnlMusteriYok.Visible = true;
                    pnlIcerik.Visible = false;
                    com.Connection.Close();
                    return false;
                }
                string ad = Duzelt(dr["Ad"]);
                string soyad = Duzelt(dr["Soyad"]);
                ltrMusteriAdi.Text = ad + " " + soyad;
                ltrAdSoyad.Text = ad + " " + soyad;
                string eposta = Duzelt(dr["EPosta"]);
                ltrEPosta.Text = string.IsNullOrEmpty(eposta) ? "-" : string.Format("<a href='mailto:{0}'>{1}</a>", dr["EPosta"], eposta);
                string tel = Duzelt(dr["Telefon"]);
                ltrTelefon.Text = string.IsNullOrEmpty(tel) ? "-" : string.Format("<a href='tel:{0}'>{1}</a>", dr["Telefon"], tel);
                string adres = Duzelt(dr["Adres"]);
                ltrAdres.Text = string.IsNullOrEmpty(adres) ? "-" : adres;
                string il = Duzelt(dr["Il"]);
                string ilce = Duzelt(dr["Ilce"]);
                string pk = Duzelt(dr["PostaKodu"]);
                ltrIlIlce.Text = string.IsNullOrEmpty(il) && string.IsNullOrEmpty(ilce) && string.IsNullOrEmpty(pk) ? "-" : il + " / " + ilce + " / " + pk;
                ltrKayitTarihi.Text = dr["KayitTarihi"] != DBNull.Value ? Convert.ToDateTime(dr["KayitTarihi"]).ToString("dd.MM.yyyy HH:mm") : "-";
                ltrSonGiris.Text = dr["SonGirisTarihi"] != DBNull.Value ? Convert.ToDateTime(dr["SonGirisTarihi"]).ToString("dd.MM.yyyy HH:mm") : "-";
                bool aktif = dr["Aktif"] != DBNull.Value && Convert.ToBoolean(dr["Aktif"]);
                badgeAktif.Visible = aktif;
                badgePasif.Visible = !aktif;
                com.Connection.Close();
            }
            return true;
        }

        private void SiparisleriGetir()
        {
            try
            {
                SiparisleriGetirInner();
            }
            catch
            {
                pnlSiparisBos.Visible = true;
                rptSiparisler.DataSource = null;
                rptSiparisler.DataBind();
            }
        }

        private void SiparisleriGetirInner()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(
                @"SELECT ID, SiparisNo, SiparisTarihi, ToplamTutar, OdemeDurumu, SiparisDurumu, N'TRY' AS ParaBirimi
                  FROM Siparis WHERE MusteriId=@ID ORDER BY SiparisTarihi DESC", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", _musteriId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                        if (row[col] != DBNull.Value && row[col] != null && col.DataType == typeof(string))
                            row[col] = EncodingHelper.DuzeltTurkce(row[col].ToString());
                }
                rptSiparisler.DataSource = dt;
                rptSiparisler.DataBind();
                pnlSiparisBos.Visible = dt.Rows.Count == 0;
            }
        }

        private void SepetiGetir()
        {
            try
            {
                SepetiGetirInner();
            }
            catch
            {
                pnlSepetBos.Visible = true;
                rptSepet.DataSource = null;
                rptSepet.DataBind();
            }
        }

        private void SepetiGetirInner()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(
                @"SELECT s.ID, s.Adet, s.EklenmeTarihi, u.UrunAdi,
                         ISNULL(s.BirimFiyat, 0) AS BirimFiyat, N'TRY' AS ParaBirimi
                  FROM Sepet s
                  INNER JOIN Urun u ON s.UrunId = u.ID
                  WHERE s.MusteriId=@ID
                  ORDER BY s.EklenmeTarihi DESC", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", _musteriId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                foreach (DataRow row in dt.Rows)
                    if (row["UrunAdi"] != DBNull.Value) row["UrunAdi"] = EncodingHelper.DuzeltTurkce(row["UrunAdi"].ToString());
                rptSepet.DataSource = dt;
                rptSepet.DataBind();
                pnlSepetBos.Visible = dt.Rows.Count == 0;
            }
        }

        private void OdemeGecmisiniGetir()
        {
            try
            {
                OdemeGecmisiniGetirInner();
            }
            catch
            {
                pnlOdemeBos.Visible = true;
                rptOdeme.DataSource = null;
                rptOdeme.DataBind();
            }
        }

        private void OdemeGecmisiniGetirInner()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(
                @"SELECT o.ID, s.SiparisNo, o.OdemeTarihi, o.KayitTarihi, o.Tutar, o.OdemeDurumu,
                         o.TutarTRY, o.TutarOrijinalPB, N'TRY' AS ParaBirimi
                  FROM OdemeKaydi o
                  INNER JOIN Siparis s ON o.SiparisId = s.ID
                  WHERE s.MusteriId=@ID
                  ORDER BY ISNULL(o.OdemeTarihi, o.KayitTarihi) DESC", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", _musteriId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                foreach (DataRow row in dt.Rows)
                    if (row["SiparisNo"] != DBNull.Value) row["SiparisNo"] = EncodingHelper.DuzeltTurkce(row["SiparisNo"].ToString());
                rptOdeme.DataSource = dt;
                rptOdeme.DataBind();
                pnlOdemeBos.Visible = dt.Rows.Count == 0;
            }
        }
    }
}
