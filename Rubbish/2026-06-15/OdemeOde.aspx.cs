using Saat.App_Code;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{
    public partial class OdemeOde : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Son Ödeme";
                if (Session[SiparisHelper.SessionAdresKey] == null || SepetHelper.Listele(Context).Rows.Count == 0)
                {
                    Response.Redirect("/sepet");
                    return;
                }
                ltrToplam.Text = SepetHelper.ToplamTutar(Context).ToString("N2") + " TL";
                OdemePanelleriniGoster();
            }
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

        protected void btnOnayla_Click(object sender, EventArgs e)
        {
            var adres = Session[SiparisHelper.SessionAdresKey] as DemoCheckoutAdres;
            if (adres == null || SepetHelper.Listele(Context).Rows.Count == 0)
            {
                Response.Redirect("/sepet");
                return;
            }

            try
            {
                string yontem = rblOdeme.SelectedValue ?? "kapida";
                string siparisNo = SiparisHelper.SahteOdemeTamamla(Context, adres, yontem);
                if (string.IsNullOrEmpty(siparisNo))
                {
                    lblHata.Text = "Sipariş oluşturulamadı.";
                    return;
                }
                Response.Redirect("/odeme-basarili?siparis=" + Server.UrlEncode(siparisNo) + "&yontem=" + Server.UrlEncode(yontem));
            }
            catch (Exception ex)
            {
                lblHata.Text = "Hata: " + ex.Message;
            }
        }
    }
}
