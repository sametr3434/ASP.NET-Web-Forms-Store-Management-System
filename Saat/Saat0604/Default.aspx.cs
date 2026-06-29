
using Saat.App_Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
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

                        Response.Redirect("Anasayfa.aspx");
                    }
                    else
                    {

                    }
                }
                catch (Exception)
                {

                }
            }

            sabitlerigetir();
        }

        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

        protected void sabitlerigetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select sitebaslik, resim from sitesabitleri where ID='1'", b.Baglanti);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string baslik = EncodingHelper.DuzeltTurkce(dr["sitebaslik"] != DBNull.Value ? dr["sitebaslik"].ToString() : "");
                    string resim = EncodingHelper.DuzeltTurkce(dr["Resim"] != DBNull.Value ? dr["Resim"].ToString() : "ResimYok.png");
                    this.Title = baslik + " | Giriş";
                    ltrLogo.Text = "<img class='img-circle' src='/Upload/" + resim + "' alt='" + baslik.Replace("'", "&#39;") + "' width='100%' />";
                    ltrFooterBaslik.Text = baslik;
                }
            }
            else
            {

            }

            dr.Close();
            com.Connection.Close();
        }

        protected void lnkGiris_Click(object sender, EventArgs e)
        {
            var encodedResponse = Request.Form["g-Recaptcha-Response"];
            var isCaptchaValid = ReCaptcha.Validate(encodedResponse);
            //isCaptchaValid
            if (true)
            {
                BaglantiBilgileri b = new BaglantiBilgileri();

                SqlCommand com = new SqlCommand("select ID from admin where kullaniciadi=@kullaniciadi and sifre=@sifre", b.Baglanti);

                com.Parameters.AddWithValue("@kullaniciadi", txtKullaniciAdi.Text);
                com.Parameters.AddWithValue("@sifre", txtSifre.Text);

                if (com.Connection.State == ConnectionState.Closed)
                {
                    com.Connection.Open();
                }

                SqlDataReader dr = com.ExecuteReader();

                if (dr.HasRows && dr.HasRows != null)
                {
                    while (dr.Read())
                    {
                        HttpCookie cerezYaz = new HttpCookie("saatpanelcerez"); //çerez oluşturuyoruz
                        cerezYaz["saatpanel"] = "SaatPanel"; //çerezin adını ve değerini veriyoruz
                        cerezYaz["KullaniciID"] = dr["ID"].ToString();
                        cerezYaz["KullaniciAdi"] = txtKullaniciAdi.Text;

                        cerezYaz.Expires = DateTime.Now.AddDays(180); //çerezin geçerlilik süresini belirtiyoruz
                        Response.Cookies.Add(cerezYaz); //oluşturduğumuz çerezi tarayıcıya ekliyoruz

                        lblKullaniciID.Text = dr["ID"].ToString();

                        Response.Redirect("Anasayfa.aspx");
                    }
                }
            }
            else
            {
                txtKullaniciAdi.Text = "";
                txtSifre.Text = "";
                txtKullaniciAdi.Focus();
                Response.Write("<script lang='JavaScript'>alert('Bilgileri Kontrol Ederek Yeniden Deneyiniz!'); window.location = 'Default.aspx'</script>");
            }
        }
    }
}