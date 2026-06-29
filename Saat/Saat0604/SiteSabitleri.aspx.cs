
using Saat.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class SiteSabitleri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    HttpCookie cerezOku = Request.Cookies["saatpanelcerez"];

                    if (cerezOku != null && cerezOku["saatpanel"] == "SaatPanel")
                    {
                        int KullaniciID = Convert.ToInt32(cerezOku["KullaniciID"].ToString());
                        lblKullaniciID.Text = KullaniciID.ToString();
                        lblKullaniciAdi.Text = cerezOku["KullaniciAdi"].ToString();

                        sabitlerigetir();
                        bilgigetir();
                    }
                    else
                    {
                        Response.Redirect("Default.aspx");
                    }
                }
                catch (Exception)
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }

        private string DbStr(object o) { return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString()); }

        protected void sabitlerigetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select SiteBaslik, Resim from SiteSabitleri where ID='1'", b.Baglanti);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    this.Title = DbStr(dr["SiteBaslik"]) + " | Site Sabitleri";
                }
            }
            else
            {

            }

            dr.Close();
            com.Connection.Close();
        }

        protected void bilgigetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select sitebaslik, resim, googleanahtar, googleaciklama from sitesabitleri where ID='1'", b.Baglanti);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    txtSiteBaslik.Text = DbStr(dr["sitebaslik"]);
                    txtLogo.Text = DbStr(dr["resim"]);
                    txtGoogleAnahtar.Text = DbStr(dr["googleanahtar"]);
                    txtGoogleAciklama.Text = DbStr(dr["googleaciklama"]);
                }
            }
            dr.Close();
            com.Connection.Close();

            txtPayTRMagazaParola.Attributes["placeholder"] = "Değiştirmek için yeni değer girin";
            txtPayTRGizliAnahtar.Attributes["placeholder"] = "Değiştirmek için yeni değer girin";
            PayTRBilgiGetir();
        }

        private void PayTRBilgiGetir()
        {
            try
            {
                var b = new BaglantiBilgileri();
                var com = new SqlCommand("SELECT PayTR_MagazaNo, PayTR_MagazaParola, PayTR_MagazaGizliAnahtar, PayTR_TestModu, KurUSD, KurEUR, PayTR_YurtdisiAktif, KargoUcreti, BedavaKargoLimiti FROM sitesabitleri WHERE ID=1", b.Baglanti);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                try
                {
                    using (var dr = com.ExecuteReader())
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            txtPayTRMagazaNo.Text = DbStr(dr["PayTR_MagazaNo"]);
                            chkPayTRTestModu.Checked = (dr["PayTR_TestModu"] != DBNull.Value && dr["PayTR_TestModu"] != null) && Convert.ToBoolean(dr["PayTR_TestModu"]);
                            try
                            {
                                txtKurUSD.Text = dr["KurUSD"] != DBNull.Value && dr["KurUSD"] != null
                                    ? Convert.ToDecimal(dr["KurUSD"]).ToString(System.Globalization.CultureInfo.InvariantCulture) : "";
                                txtKurEUR.Text = dr["KurEUR"] != DBNull.Value && dr["KurEUR"] != null
                                    ? Convert.ToDecimal(dr["KurEUR"]).ToString(System.Globalization.CultureInfo.InvariantCulture) : "";
                                chkPayTRYurtdisi.Checked = dr["PayTR_YurtdisiAktif"] != DBNull.Value && dr["PayTR_YurtdisiAktif"] != null && Convert.ToBoolean(dr["PayTR_YurtdisiAktif"]);

                                txtKargoUcreti.Text = dr["KargoUcreti"] != DBNull.Value && dr["KargoUcreti"] != null
                                    ? Convert.ToDecimal(dr["KargoUcreti"]).ToString(System.Globalization.CultureInfo.InvariantCulture) : "50.00";
                                txtBedavaKargoLimiti.Text = dr["BedavaKargoLimiti"] != DBNull.Value && dr["BedavaKargoLimiti"] != null
                                    ? Convert.ToDecimal(dr["BedavaKargoLimiti"]).ToString(System.Globalization.CultureInfo.InvariantCulture) : "1000.00";
                            }
                            catch { }
                        }
                    }
                }
                finally { com.Connection.Close(); }
            }
            catch { chkPayTRTestModu.Checked = true; }
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            var b = new BaglantiBilgileri();
            var resim = txtLogo.Text;
            if (fuLogo.HasFile)
            {
                var f = new FileInfo(fuLogo.FileName);
                resim = f.Name;
                fuLogo.SaveAs(Server.MapPath("/upload/" + "\\" + resim));
            }
            if (string.IsNullOrEmpty(resim)) resim = "ResimYok.png";

            decimal kurUsd = 0, kurEur = 0, kargoUcreti = 50, bedavaKargoLimiti = 1000;
            var inv = System.Globalization.CultureInfo.InvariantCulture;
            decimal.TryParse(txtKurUSD.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out kurUsd);
            decimal.TryParse(txtKurEUR.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out kurEur);
            decimal.TryParse(txtKargoUcreti.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out kargoUcreti);
            decimal.TryParse(txtBedavaKargoLimiti.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out bedavaKargoLimiti);

            var updated = false;
            try
            {
                var com = new SqlCommand(@"
                    UPDATE sitesabitleri SET sitebaslik=@sitebaslik, resim=@resim, googleanahtar=@googleanahtar, googleaciklama=@googleaciklama,
                        PayTR_MagazaNo=@PayTR_MagazaNo,
                        PayTR_MagazaParola=ISNULL(NULLIF(@PayTR_MagazaParola,''), PayTR_MagazaParola),
                        PayTR_MagazaGizliAnahtar=ISNULL(NULLIF(@PayTR_MagazaGizliAnahtar,''), PayTR_MagazaGizliAnahtar),
                        PayTR_TestModu=@PayTR_TestModu,
                        KurUSD=@KurUSD, KurEUR=@KurEUR, PayTR_YurtdisiAktif=@PayTR_YurtdisiAktif,
                        KargoUcreti=@KargoUcreti, BedavaKargoLimiti=@BedavaKargoLimiti
                    WHERE ID=@ID
                ", b.Baglanti);
                com.Parameters.AddWithValue("@ID", "1");
                com.Parameters.AddWithValue("@sitebaslik", txtSiteBaslik.Text);
                com.Parameters.AddWithValue("@resim", resim);
                com.Parameters.AddWithValue("@googleanahtar", txtGoogleAnahtar.Text);
                com.Parameters.AddWithValue("@googleaciklama", txtGoogleAciklama.Text);
                com.Parameters.AddWithValue("@PayTR_MagazaNo", txtPayTRMagazaNo.Text ?? "");
                com.Parameters.AddWithValue("@PayTR_MagazaParola", txtPayTRMagazaParola.Text ?? "");
                com.Parameters.AddWithValue("@PayTR_MagazaGizliAnahtar", txtPayTRGizliAnahtar.Text ?? "");
                com.Parameters.AddWithValue("@PayTR_TestModu", chkPayTRTestModu.Checked);
                com.Parameters.AddWithValue("@KurUSD", kurUsd == 0 ? (object)DBNull.Value : kurUsd);
                com.Parameters.AddWithValue("@KurEUR", kurEur == 0 ? (object)DBNull.Value : kurEur);
                com.Parameters.AddWithValue("@PayTR_YurtdisiAktif", chkPayTRYurtdisi.Checked);
                com.Parameters.AddWithValue("@KargoUcreti", kargoUcreti);
                com.Parameters.AddWithValue("@BedavaKargoLimiti", bedavaKargoLimiti);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                try { updated = com.ExecuteNonQuery() > 0; }
                finally { com.Connection.Close(); }
            }
            catch
            {
                var com = new SqlCommand("UPDATE sitesabitleri SET sitebaslik=@sitebaslik, resim=@resim, googleanahtar=@googleanahtar, googleaciklama=@googleaciklama WHERE ID=@ID", b.Baglanti);
                com.Parameters.AddWithValue("@ID", "1");
                com.Parameters.AddWithValue("@sitebaslik", txtSiteBaslik.Text);
                com.Parameters.AddWithValue("@resim", resim);
                com.Parameters.AddWithValue("@googleanahtar", txtGoogleAnahtar.Text);
                com.Parameters.AddWithValue("@googleaciklama", txtGoogleAciklama.Text);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                try { updated = com.ExecuteNonQuery() > 0; }
                finally { com.Connection.Close(); }
            }
            if (updated) Response.Redirect(HttpContext.Current.Request.RawUrl);
        }
    }
}