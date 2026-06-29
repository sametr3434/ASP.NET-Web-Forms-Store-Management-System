using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{
    public partial class Hesabim : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!MusteriAuth.GirisMi(Session))
            {
                MusteriAuth.GirisGerekli(Context, "/hesabim");
                return;
            }

            if (!IsPostBack)
            {
                Title = "Hesabım";
                BilgileriYukle();
                AdresleriYukle();
                SiparisleriYukle();
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void rptAdresler_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int mid = MusteriAuth.MusteriId(Session);
            int adresId;
            if (!int.TryParse(e.CommandArgument?.ToString(), out adresId) || adresId <= 0) return;

            if (e.CommandName == "Sil")
                MusteriAdresHelper.Sil(mid, adresId);
            else if (e.CommandName == "Varsayilan")
                MusteriAdresHelper.VarsayilanYap(mid, adresId);

            BilgileriYukle();
            AdresleriYukle();
        }

        protected void btnAdresEkle_Click(object sender, EventArgs e)
        {
            int mid = MusteriAuth.MusteriId(Session);
            string baslik = txtAdresBaslik.Text.Trim();
            string adres = txtYeniAdres.Text.Trim();
            if (string.IsNullOrWhiteSpace(baslik) || string.IsNullOrWhiteSpace(adres))
                return;

            string ad = "", soyad = "", tel = txtYeniTelefon.Text.Trim();
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand("SELECT Ad, Soyad, Telefon FROM Musteri WHERE ID=@id", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", mid);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        ad = rdr["Ad"]?.ToString() ?? "";
                        soyad = rdr["Soyad"]?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(tel))
                            tel = rdr["Telefon"]?.ToString() ?? "";
                    }
                }
            }

            MusteriAdresHelper.Ekle(mid, baslik, ad, soyad, tel, adres,
                txtYeniIl.Text.Trim(), txtYeniIlce.Text.Trim(), txtYeniPostaKodu.Text.Trim(),
                chkVarsayilan.Checked);

            txtAdresBaslik.Text = "";
            txtYeniAdres.Text = "";
            txtYeniIl.Text = "";
            txtYeniIlce.Text = "";
            txtYeniPostaKodu.Text = "";
            txtYeniTelefon.Text = "";
            chkVarsayilan.Checked = false;

            BilgileriYukle();
            AdresleriYukle();
        }

        private void BilgileriYukle()
        {
            int mid = MusteriAuth.MusteriId(Session);
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(@"
SELECT Ad, Soyad, EPosta, Telefon
FROM Musteri WHERE ID=@id", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", mid);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return;
                    ltrAdSoyad.Text = Duzelt(rdr["Ad"]) + " " + Duzelt(rdr["Soyad"]);
                    ltrEposta.Text = Duzelt(rdr["EPosta"]);
                    ltrTelefon.Text = string.IsNullOrWhiteSpace(rdr["Telefon"]?.ToString())
                        ? "—" : Duzelt(rdr["Telefon"]);
                }
            }

            var varsayilan = MusteriAdresHelper.CheckoutAdresiAl(mid, null);
            if (varsayilan == null || string.IsNullOrWhiteSpace(varsayilan.Adres))
            {
                ltrAdres.Text = "—";
                return;
            }
            string tam = Duzelt(varsayilan.Adres);
            if (!string.IsNullOrWhiteSpace(varsayilan.Ilce))
                tam += ", " + Duzelt(varsayilan.Ilce);
            if (!string.IsNullOrWhiteSpace(varsayilan.Il))
                tam += " / " + Duzelt(varsayilan.Il);
            ltrAdres.Text = tam;
        }

        private void AdresleriYukle()
        {
            int mid = MusteriAuth.MusteriId(Session);
            var dt = MusteriAdresHelper.Listele(mid);
            if (dt.Rows.Count == 0)
            {
                pnlAdresYok.Visible = true;
                pnlAdresler.Visible = false;
                return;
            }
            pnlAdresYok.Visible = false;
            pnlAdresler.Visible = true;
            rptAdresler.DataSource = dt;
            rptAdresler.DataBind();
        }

        private void SiparisleriYukle()
        {
            int mid = MusteriAuth.MusteriId(Session);
            var b = new BaglantiBilgileri();
            var dt = new DataTable();
            using (var cmd = new SqlCommand(@"
SELECT s.SiparisNo, s.SiparisTarihi, s.ToplamTutar, s.OdemeDurumu, s.SiparisDurumu,
       STUFF((
           SELECT N', ' + d.UrunAdi + N' x' + CAST(d.Adet AS NVARCHAR(10))
           FROM SiparisDetay d WHERE d.SiparisId = s.ID
           FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, N'') AS UrunOzeti
FROM Siparis s
WHERE s.MusteriId=@id
ORDER BY s.SiparisTarihi DESC", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", mid);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                new SqlDataAdapter(cmd).Fill(dt);
            }

            if (dt.Rows.Count == 0)
            {
                pnlSiparisYok.Visible = true;
                pnlSiparisler.Visible = false;
                return;
            }

            pnlSiparisYok.Visible = false;
            pnlSiparisler.Visible = true;
            rptSiparisler.DataSource = dt;
            rptSiparisler.DataBind();
        }
    }
}
