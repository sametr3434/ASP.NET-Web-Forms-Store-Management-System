using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class SiparisEtiket : Page
    {
        public string SiparisNo { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
            {
                ltrGonderen.Text = "Geçersiz sipariş.";
                return;
            }
            if (!int.TryParse(Request.QueryString["id"], out int siparisId))
            {
                ltrGonderen.Text = "Geçersiz sipariş ID.";
                return;
            }
            EtiketVerileriniGetir(siparisId);
        }

        private string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        private void EtiketVerileriniGetir(int siparisId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            string gonderenAdres = "";
            string siteBaslik = "";
            using (SqlCommand com = new SqlCommand("SELECT TOP 1 SiteBaslik FROM sitesabitleri WHERE ID=1", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                var dr = com.ExecuteReader();
                if (dr.Read()) siteBaslik = Duzelt(dr["SiteBaslik"]);
                com.Connection.Close();
            }
            using (SqlCommand com = new SqlCommand("SELECT TOP 1 Adres, Telefon1 FROM iletisim WHERE ID=1", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    gonderenAdres = Duzelt(dr["Adres"]);
                    string tel = Duzelt(dr["Telefon1"]);
                    gonderenAdres += (string.IsNullOrEmpty(gonderenAdres) ? "" : "<br/>") + tel;
                }
                com.Connection.Close();
            }
            ltrGonderen.Text = string.Format("<strong>{0}</strong><br/>{1}", string.IsNullOrEmpty(siteBaslik) ? "Mağaza" : siteBaslik, string.IsNullOrEmpty(gonderenAdres) ? "-" : gonderenAdres);

            using (SqlCommand com = new SqlCommand(@"
                SELECT s.SiparisNo, s.KargoTakipNo,
                    s.TeslimatAdres, s.TeslimatIl, s.TeslimatIlce, s.TeslimatPostaKodu,
                    s.MisafirAd, s.MisafirSoyad, s.MisafirTelefon,
                    m.Ad AS MusteriAd, m.Soyad AS MusteriSoyad, m.Telefon AS MusteriTelefon
                FROM Siparis s
                LEFT JOIN Musteri m ON s.MusteriId = m.ID
                WHERE s.ID = @ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", siparisId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    ltrAlici.Text = "Sipariş bulunamadı.";
                    com.Connection.Close();
                    return;
                }
                SiparisNo = Duzelt(dr["SiparisNo"]);
                string aliciAd = dr["MusteriAd"] != DBNull.Value && dr["MusteriAd"] != null ? (Duzelt(dr["MusteriAd"]) + " " + Duzelt(dr["MusteriSoyad"])).Trim() : "";
                if (string.IsNullOrWhiteSpace(aliciAd))
                    aliciAd = (Duzelt(dr["MisafirAd"]) + " " + Duzelt(dr["MisafirSoyad"])).Trim();
                if (string.IsNullOrWhiteSpace(aliciAd)) aliciAd = "Alıcı";
                ltrAlici.Text = aliciAd;

                var adresSb = new StringBuilder();
                string adres = Duzelt(dr["TeslimatAdres"]);
                string il = Duzelt(dr["TeslimatIl"]);
                string ilce = Duzelt(dr["TeslimatIlce"]);
                string pk = Duzelt(dr["TeslimatPostaKodu"]);
                if (!string.IsNullOrEmpty(adres)) adresSb.Append(adres);
                if (!string.IsNullOrEmpty(ilce) || !string.IsNullOrEmpty(il))
                {
                    if (adresSb.Length > 0) adresSb.Append("<br/>");
                    adresSb.Append(ilce);
                    if (!string.IsNullOrEmpty(ilce) && !string.IsNullOrEmpty(il)) adresSb.Append(" / ");
                    adresSb.Append(il);
                }
                if (!string.IsNullOrEmpty(pk)) { if (adresSb.Length > 0) adresSb.Append(" "); adresSb.Append(pk); }
                ltrAdres.Text = adresSb.Length > 0 ? adresSb.ToString() : "-";

                string telefon = (dr["MusteriTelefon"] != DBNull.Value && dr["MusteriTelefon"] != null && !string.IsNullOrWhiteSpace(dr["MusteriTelefon"].ToString()))
                    ? Duzelt(dr["MusteriTelefon"]) : Duzelt(dr["MisafirTelefon"]);
                ltrTelefon.Text = string.IsNullOrEmpty(telefon) ? "-" : telefon;

                ltrSiparisNo.Text = SiparisNo;
                ltrKargoTakip.Text = string.IsNullOrEmpty(Duzelt(dr["KargoTakipNo"])) ? "-" : Duzelt(dr["KargoTakipNo"]);
                com.Connection.Close();
            }

            KalemleriGetir(siparisId);
        }

        private void KalemleriGetir(int siparisId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            var sb = new StringBuilder();
            using (SqlCommand com = new SqlCommand(@"
                SELECT UrunAdi, UrunCapi, Renk, Adet
                FROM SiparisDetay WHERE SiparisId=@ID ORDER BY ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", siparisId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    string urun = Duzelt(dr["UrunAdi"]);
                    string beden = Duzelt(dr["UrunCapi"]);
                    string renk = Duzelt(dr["Renk"]);
                    int adet = dr["Adet"] != DBNull.Value ? Convert.ToInt32(dr["Adet"]) : 1;
                    string detay = urun;
                    if (!string.IsNullOrEmpty(beden) || !string.IsNullOrEmpty(renk))
                        detay += " (" + (beden + " " + renk).Trim() + ")";
                    detay += " x" + adet;
                    sb.Append(sb.Length > 0 ? "<br/>" : "").Append(detay);
                }
                com.Connection.Close();
            }
            ltrSiparisIcerik.Text = sb.Length > 0 ? sb.ToString() : "-";
        }
    }
}
