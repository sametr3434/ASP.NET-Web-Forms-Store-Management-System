using Saat.App_Code;
using System;
using System.Web.UI;

namespace Saat
{
    public partial class UyeCikis : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int mid = MusteriAuth.MusteriId(Session);
            MusteriAuth.HatirlaSil(Request, Response, mid);
            MusteriAuth.OturumKapat(Session);
            Response.Redirect("/");
        }
    }
}
