using Saat.App_Code;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{
    public partial class Sepet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Sepet";
                SepetiYukle();
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        private void SepetiYukle()
        {
            DataTable dt = SepetHelper.Listele(Context);
            if (dt.Rows.Count > 0)
            {
                rptSepet.DataSource = dt;
                rptSepet.DataBind();
                ltrToplam.Text = SepetHelper.ToplamTutar(Context).ToString("N2") + " TL";
                pnlSepet.Visible = true;
                pnlBos.Visible = false;
            }
            else
            {
                pnlSepet.Visible = false;
                pnlBos.Visible = true;
            }
        }

        protected void rptSepet_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int sepetId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Sil")
            {
                SepetHelper.Sil(Context, sepetId);
            }
            else if (e.CommandName == "Guncelle")
            {
                var txt = e.Item.FindControl("txtAdet") as TextBox;
                int adet = 1;
                if (txt != null) int.TryParse(txt.Text, out adet);
                SepetHelper.AdetGuncelle(Context, sepetId, adet);
            }
            SepetiYukle();
        }

        protected void btnOdeme_Click(object sender, EventArgs e)
        {
            if (SepetHelper.Listele(Context).Rows.Count == 0)
            {
                SepetiYukle();
                return;
            }
            Response.Redirect("/odeme");
        }
    }
}
