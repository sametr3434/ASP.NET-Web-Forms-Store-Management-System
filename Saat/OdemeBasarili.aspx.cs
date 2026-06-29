using Saat.App_Code;
using System;
using System.Web.UI;

namespace Saat
{
    public partial class OdemeBasarili : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Sipariş Tamamlandı";
                string siparis = Request.QueryString["siparis"] ?? "";
                string yontem = Request.QueryString["yontem"] ?? "kapida";

                ltrSiparisNo.Text = Server.HtmlEncode(siparis);
                ltrYontem.Text = SiparisHelper.OdemeYontemiBaslik(yontem);

                switch ((yontem ?? "").ToLowerInvariant())
                {
                    case "eft":
                        ltrMesaj.Text = "<p class=\"mt-3\">Demo: EFT ödemeniz alınmış sayıldı. Teşekkür ederiz.</p>";
                        break;
                    case "kart":
                        ltrMesaj.Text = "<p class=\"mt-3\">Demo: Kart ödemeniz onaylandı. Teşekkür ederiz.</p>";
                        break;
                    default:
                        ltrMesaj.Text = "<p class=\"mt-3\">Kargonuz geldiğinde ödeme yapabilirsiniz. Teşekkür ederiz.</p>";
                        break;
                }
            }
        }
    }
}
