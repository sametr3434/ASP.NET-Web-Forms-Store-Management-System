using Saat.App_Code;
using System;
using System.Web;
using System.Web.UI;

namespace Saat
{
    public partial class UyeKayit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Üye Kayıt";
                if (MusteriAuth.GirisMi(Session))
                    Response.Redirect("/hesabim");
            }
        }

        protected void btnKayit_Click(object sender, EventArgs e)
        {
            pnlHata.Visible = false;
            string ad = txtAd.Text.Trim();
            string soyad = txtSoyad.Text.Trim();
            string eposta = txtEposta.Text.Trim();
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string sifre = txtSifre.Text;
            string sifreTekrar = txtSifreTekrar.Text;

            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad) ||
                string.IsNullOrWhiteSpace(eposta) || string.IsNullOrWhiteSpace(sifre))
            {
                HataGoster("Ad, soyad, e-posta ve şifre zorunludur.");
                return;
            }
            if (sifre.Length < 4)
            {
                HataGoster("Şifre en az 4 karakter olmalıdır.");
                return;
            }
            if (sifre != sifreTekrar)
            {
                HataGoster("Şifreler eşleşmiyor.");
                return;
            }
            if (MusteriAuth.EPostaKayitli(eposta))
            {
                HataGoster("Bu e-posta adresi zaten kayıtlı.");
                return;
            }
            if (!string.IsNullOrWhiteSpace(kullaniciAdi) && MusteriAuth.KullaniciAdiKayitli(kullaniciAdi))
            {
                HataGoster("Bu kullanıcı adı zaten alınmış.");
                return;
            }

            int yeniId = MusteriAuth.Kayit(ad, soyad, eposta, telefon, sifre, kullaniciAdi);
            if (yeniId <= 0)
            {
                HataGoster("Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.");
                return;
            }

            string adSoyad = (ad + " " + soyad).Trim();
            MusteriAuth.OturumAc(Session, yeniId, adSoyad);
            SepetHelper.MisafirSepetiMerge(Context, yeniId);
            Response.Redirect("/hesabim");
        }

        private void HataGoster(string mesaj)
        {
            ltrHata.Text = HttpUtility.HtmlEncode(mesaj);
            pnlHata.Visible = true;
        }
    }
}
