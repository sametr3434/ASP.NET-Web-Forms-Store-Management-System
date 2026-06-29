using Saat.App_Code;
using System;
using System.Web.UI;

namespace Saat
{
    public partial class Odeme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Ödeme";
                if (SepetHelper.Listele(Context).Rows.Count == 0)
                {
                    Response.Redirect("/sepet");
                    return;
                }
                ltrOzet.Text = "<p>Toplam: <strong>" + SepetHelper.ToplamTutar(Context).ToString("N2") + " TL</strong></p>";
            }
        }

        protected void btnDevam_Click(object sender, EventArgs e)
        {
            if (SepetHelper.Listele(Context).Rows.Count == 0)
            {
                Response.Redirect("/sepet");
                return;
            }
            var adres = new DemoCheckoutAdres
            {
                Ad = txtAd.Text.Trim(),
                Soyad = txtSoyad.Text.Trim(),
                Telefon = txtTelefon.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Adres = txtAdres.Text.Trim(),
                Il = txtIl.Text.Trim(),
                Ilce = txtIlce.Text.Trim()
            };
            Session[SiparisHelper.SessionAdresKey] = adres;
            Response.Redirect("/odeme-ode");
        }
    }
}
