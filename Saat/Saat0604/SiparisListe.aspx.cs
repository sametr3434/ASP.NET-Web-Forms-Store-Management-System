using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class SiparisListe : Page
    {
        protected string TutarPB(object tutar, object paraBirimi)
        {
            if (tutar == null || tutar == DBNull.Value) return "-";
            return ParaBirimiHelper.Format(Convert.ToDecimal(tutar), paraBirimi != null && paraBirimi != DBNull.Value ? paraBirimi.ToString() : ParaBirimiHelper.Try);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSiparisler();
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void btnFiltre_Click(object sender, EventArgs e)
        {
            BindSiparisler();
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            ddlDurum.SelectedValue = "";
            txtBaslangic.Text = "";
            txtBitis.Text = "";
            BindSiparisler();
        }

        private void BindSiparisler()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            var sql = new StringBuilder(@"
                SELECT s.ID, s.SiparisNo, 
                       ISNULL(m.Ad + ' ' + m.Soyad, ISNULL(s.MisafirAd + ' ' + s.MisafirSoyad, 'Misafir')) AS MusteriAdi,
                       s.AraToplam, s.IndirimTutari, s.KargoTutari, s.ToplamTutar,
                       ISNULL(s.ParaBirimi, N'TRY') AS ParaBirimi,
                       s.OdemeDurumu, s.SiparisDurumu, s.SiparisTarihi
                FROM Siparis s
                LEFT JOIN Musteri m ON s.MusteriId = m.ID
                WHERE 1=1 ");

            DateTime d1, d2;
            if (!string.IsNullOrEmpty(ddlDurum.SelectedValue))
                sql.Append(" AND s.SiparisDurumu = @Durum ");
            if (TryParseTarih(txtBaslangic.Text, out d1))
                sql.Append(" AND s.SiparisTarihi >= @Baslangic ");
            if (TryParseTarih(txtBitis.Text, out d2))
                sql.Append(" AND s.SiparisTarihi < DATEADD(day, 1, @Bitis) ");
            sql.Append(" ORDER BY s.ID DESC");

            using (SqlCommand com = new SqlCommand(sql.ToString(), b.Baglanti))
            {
                if (!string.IsNullOrEmpty(ddlDurum.SelectedValue))
                    com.Parameters.AddWithValue("@Durum", ddlDurum.SelectedValue);
                if (TryParseTarih(txtBaslangic.Text, out d1))
                    com.Parameters.AddWithValue("@Baslangic", d1);
                if (TryParseTarih(txtBitis.Text, out d2))
                    com.Parameters.AddWithValue("@Bitis", d2);

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                rptSiparisler.DataSource = dt;
                rptSiparisler.DataBind();
            }
        }

        private bool TryParseTarih(string t, out DateTime d)
        {
            d = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(t)) return false;
            if (DateTime.TryParseExact(t.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d)) return true;
            return DateTime.TryParse(t, out d);
        }
    }
}
