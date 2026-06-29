using Saat.App_Code;
using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{
    public partial class OdemeOde : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Son Ödeme";
            if (!CheckoutHazirMi(out string yonlendirme))
            {
                Response.Redirect(yonlendirme);
                return;
            }

            if (!IsPostBack)
            {
                ltrToplam.Text = SepetHelper.ToplamTutar(Context).ToString("N2") + " TL";
            }

            OdemePanelleriniGoster();
        }

        protected void rblOdeme_SelectedIndexChanged(object sender, EventArgs e)
        {
            OdemePanelleriniGoster();
        }

        private void OdemePanelleriniGoster()
        {
            string secim = rblOdeme.SelectedValue ?? "kapida";
            pnlKapida.Visible = secim == "kapida";
            pnlEft.Visible = secim == "eft";
            pnlKart.Visible = secim == "kart";
        }

        private bool CheckoutHazirMi(out string yonlendirme)
        {
            yonlendirme = "/sepet";
            if (SepetHelper.Listele(Context).Rows.Count == 0)
                return false;

            if (Session[SiparisHelper.SessionAdresKey] == null)
            {
                yonlendirme = "/odeme";
                return false;
            }

            return true;
        }

        protected void btnOnayla_Click(object sender, EventArgs e)
        {
            lblHata.Text = "";
            btnOnayla.Enabled = false;
            lblOdendi.Text = "Ödeme Başarıyla Alındı"; 
            try
            {
                if (!CheckoutHazirMi(out string yonlendirme))
                {
                    Response.Redirect(yonlendirme);
                    return;
                }

                var adres = Session[SiparisHelper.SessionAdresKey] as DemoCheckoutAdres;
                if (adres == null)
                {
                    lblHata.Text = "Teslimat bilgisi bulunamadı. Lütfen adres adımını tekrar doldurun.";
                    btnOnayla.Enabled = true;
                    return;
                }

                string yontem = rblOdeme.SelectedValue ?? "kapida";
                string siparisNo = SiparisHelper.SahteOdemeTamamla(Context, adres, yontem);
                if (string.IsNullOrEmpty(siparisNo))
                {
                    lblHata.Text = "Sepet boş veya sipariş oluşturulamadı.";
                    btnOnayla.Enabled = true;
                    return;
                }

                Response.Redirect("/odeme-basarili?siparis=" + Server.UrlEncode(siparisNo) + "&yontem=" + Server.UrlEncode(yontem));
            }

            catch (Exception ex)
            {
                lblHata.Text = "Sipariş kaydedilemedi: " + ex.Message;
                btnOnayla.Enabled = true;
            }
        }
    }
}
