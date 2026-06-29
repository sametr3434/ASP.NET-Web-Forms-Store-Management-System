using Saat.App_Code;
using System;
using System.Web;
using System.Web.UI;

namespace Saat
{
    public partial class UyeGiris : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Üye Girişi";
                if (MusteriAuth.GirisMi(Session))
                    Response.Redirect(GeriDonusUrl());
            }
        }

        protected void btnGiris_Click(object sender, EventArgs e)
        {
            pnlHata.Visible = false;
            string kullanici = txtKullanici.Text.Trim();
            string sifre = txtSifre.Text;

            if (string.IsNullOrWhiteSpace(kullanici) || string.IsNullOrWhiteSpace(sifre))
            {
                HataGoster("Kullanıcı adı ve şifre zorunludur.");
                return;
            }

            int sonuc = MusteriAuth.GirisYap(kullanici, sifre);
            if (sonuc == -1)
            {
                HataGoster("Hesabınız pasif durumda. Lütfen mağaza ile iletişime geçin.");
                return;
            }
            if (sonuc <= 0)
            {
                HataGoster("Kullanıcı adı veya şifre hatalı.");
                return;
            }

            string adSoyad = MusteriAuth.AdSoyadAl(sonuc);
            MusteriAuth.OturumAc(Session, sonuc, adSoyad);
            SepetHelper.MisafirSepetiMerge(Context, sonuc);
            if (chkHatirla.Checked)
                MusteriAuth.HatirlaAyarla(Response, sonuc);

            Response.Redirect(GeriDonusUrl());
        }

        private string GeriDonusUrl()
        {
            string url = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("/") || url.StartsWith("//"))
                return "/hesabim";
            return url;
        }

        private void HataGoster(string mesaj)
        {
            ltrHata.Text = HttpUtility.HtmlEncode(mesaj);
            pnlHata.Visible = true;
        }
    }
}
