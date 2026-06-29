using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

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
                SiparisleriYukle();
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        private void BilgileriYukle()
        {
            int mid = MusteriAuth.MusteriId(Session);
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(@"
SELECT Ad, Soyad, EPosta, Telefon, Adres, Il, Ilce
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
                    string adres = Duzelt(rdr["Adres"]);
                    string il = Duzelt(rdr["Il"]);
                    string ilce = Duzelt(rdr["Ilce"]);
                    string tam = adres;
                    if (!string.IsNullOrWhiteSpace(ilce)) tam += (tam.Length > 0 ? ", " : "") + ilce;
                    if (!string.IsNullOrWhiteSpace(il)) tam += (tam.Length > 0 ? " / " : "") + il;
                    ltrAdres.Text = string.IsNullOrWhiteSpace(tam) ? "—" : tam;
                }
            }
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
