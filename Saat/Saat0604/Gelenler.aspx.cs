

using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class Gelenler : System.Web.UI.Page
    {
        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    HttpCookie cerezOku = Request.Cookies["saatpanelcerez"];

                    if (cerezOku != null && cerezOku["saatpanel"] == "SaatPanel")
                    {
                        lblKullaniciID.Text = cerezOku["KullaniciID"]?.ToString() ?? "";
                        lblKullaniciAdi.Text = Duzelt(cerezOku["KullaniciAdi"]);
                        SabitleriGetir();
                        GelenleriGetir();
                    }
                }
                catch { }
            }
        }

        private void SabitleriGetir()
        {
            var b = new BaglantiBilgileri();
            var com = new SqlCommand("SELECT SiteBaslik FROM SiteSabitleri WHERE ID=1", b.Baglanti);
            if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
            try
            {
                using (var dr = com.ExecuteReader())
                {
                    if (dr.HasRows && dr.Read())
                        Title = Duzelt(dr["SiteBaslik"]) + " | Gelenler";
                }
            }
            finally { com.Connection.Close(); }
        }

        private void GelenleriGetir()
        {
            try
            {
                var b = new BaglantiBilgileri();
                var com = new SqlCommand("SELECT ID, TarihSaat, Isim, Telefon, MailAdresi, Okundu FROM Gelenler ORDER BY ID DESC", b.Baglanti);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                try
                {
                    var dt = new DataTable();
                    dt.Load(com.ExecuteReader());
                    rptGelenler.DataSource = dt;
                    rptGelenler.DataBind();
                    pnlBos.Visible = dt.Rows.Count == 0;
                }
                finally { com.Connection.Close(); }
            }
            catch
            {
                pnlBos.Visible = true;
            }
        }

        protected void lnkIncele_Click(object sender, EventArgs e)
        {
            var lnk = (LinkButton)sender;
            lblInceleID.Text = lnk.CommandArgument;
            var b = new BaglantiBilgileri();
            var com = new SqlCommand("SELECT TarihSaat, IPAdresi, Isim, Telefon, MailAdresi, Mesaj, Okundu FROM Gelenler WHERE ID=@ID", b.Baglanti);
            com.Parameters.AddWithValue("@ID", lblInceleID.Text);
            if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
            try
            {
                using (var dr = com.ExecuteReader())
                {
                    if (dr.HasRows && dr.Read())
                    {
                        pnlIncele.Visible = true;
                        ltrTarihSaat.Text = dr["TarihSaat"] != DBNull.Value ? Convert.ToDateTime(dr["TarihSaat"]).ToString("dd.MM.yyyy HH:mm") : "";
                        ltrIPAdresi.Text = Duzelt(dr["IPAdresi"]);
                        ltrIsimSoyisim.Text = Duzelt(dr["Isim"]);
                        var tel = Duzelt(dr["Telefon"]);
                        ltrTelefon.Text = string.IsNullOrEmpty(tel) ? "-" : string.Format("<a href='tel:{0}'>{0}</a>", tel);
                        var mail = Duzelt(dr["MailAdresi"]);
                        ltrMailAdresi.Text = string.IsNullOrEmpty(mail) ? "-" : string.Format("<a href='mailto:{0}'>{0}</a>", mail);
                        ltrMesaj.Text = Duzelt(dr["Mesaj"]).Replace("\n", "<br/>");
                        var okundu = Duzelt(dr["Okundu"]);
                        lnkOkunduYap.Visible = (okundu == "Hayır");
                    }
                }
            }
            finally { com.Connection.Close(); }
        }

        protected void lnkOkunduYap_Click(object sender, EventArgs e)
        {
            var b = new BaglantiBilgileri();
            var com = new SqlCommand("UPDATE Gelenler SET Okundu=N'Evet' WHERE ID=@ID", b.Baglanti);
            com.Parameters.AddWithValue("@ID", lblInceleID.Text);
            if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
            try
            {
                com.ExecuteNonQuery();
            }
            finally { com.Connection.Close(); }
            pnlIncele.Visible = false;
            GelenleriGetir();
        }

        protected void lnkKapat_Click(object sender, EventArgs e)
        {
            pnlIncele.Visible = false;
        }

        protected void lnkSil_Click(object sender, EventArgs e)
        {
            var lnk = (LinkButton)sender;
            var id = lnk.CommandArgument;
            var b = new BaglantiBilgileri();
            var com = new SqlCommand("DELETE FROM Gelenler WHERE ID=@ID", b.Baglanti);
            com.Parameters.AddWithValue("@ID", id);
            if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
            try
            {
                com.ExecuteNonQuery();
            }
            finally { com.Connection.Close(); }
            pnlIncele.Visible = false;
            GelenleriGetir();
        }
    }
}
