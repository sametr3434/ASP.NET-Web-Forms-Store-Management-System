using Saat.App_Code;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                if (MusteriAuth.GirisMi(Session))
                {
                    KayitliAdresleriBagla();
                    pnlKayitliAdres.Visible = ddlKayitliAdres.Items.Count > 1;
                    int? aid = SeciliAdresId();
                    AdresFormuDoldur(aid);
                    pnlAdresKaydet.Visible = !aid.HasValue;
                }
            }
        }

        protected void ddlKayitliAdres_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? aid = SeciliAdresId();
            AdresFormuDoldur(aid);
            pnlAdresKaydet.Visible = !aid.HasValue;
        }

        protected void btnDevam_Click(object sender, EventArgs e)
        {
            if (SepetHelper.Listele(Context).Rows.Count == 0)
            {
                Response.Redirect("/sepet");
                return;
            }

            if (MusteriAuth.GirisMi(Session) && chkAdresKaydet.Checked && !SeciliAdresId().HasValue)
            {
                int mid = MusteriAuth.MusteriId(Session);
                string baslik = txtYeniAdresBaslik.Text.Trim();
                if (string.IsNullOrWhiteSpace(baslik)) baslik = "Yeni Adres";
                MusteriAdresHelper.Ekle(mid, baslik, txtAd.Text.Trim(), txtSoyad.Text.Trim(),
                    txtTelefon.Text.Trim(), txtAdres.Text.Trim(), txtIl.Text.Trim(),
                    txtIlce.Text.Trim(), txtPostaKodu.Text.Trim(), false);
            }

            var adres = new DemoCheckoutAdres
            {
                Ad = txtAd.Text.Trim(),
                Soyad = txtSoyad.Text.Trim(),
                Telefon = txtTelefon.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Adres = txtAdres.Text.Trim(),
                Il = txtIl.Text.Trim(),
                Ilce = txtIlce.Text.Trim(),
                PostaKodu = txtPostaKodu.Text.Trim()
            };
            Session[SiparisHelper.SessionAdresKey] = adres;
            Response.Redirect("/odeme-ode");
        }

        private void KayitliAdresleriBagla()
        {
            int mid = MusteriAuth.MusteriId(Session);
            var dt = MusteriAdresHelper.Listele(mid);
            ddlKayitliAdres.Items.Clear();
            ddlKayitliAdres.Items.Add(new ListItem("-- Yeni adres gir --", ""));

            foreach (DataRow row in dt.Rows)
            {
                string id = row["ID"].ToString();
                ddlKayitliAdres.Items.Add(new ListItem(MusteriAdresHelper.DropdownMetin(row), id));
                if (row["Varsayilan"] != DBNull.Value && Convert.ToBoolean(row["Varsayilan"]))
                    ddlKayitliAdres.SelectedValue = id;
            }
        }

        private int? SeciliAdresId()
        {
            int id;
            if (int.TryParse(ddlKayitliAdres.SelectedValue, out id) && id > 0)
                return id;
            return null;
        }

        private void AdresFormuDoldur(int? adresId)
        {
            int mid = MusteriAuth.MusteriId(Session);
            var bilgi = MusteriAdresHelper.CheckoutAdresiAl(mid, adresId);
            if (bilgi == null) return;

            txtAd.Text = bilgi.Ad;
            txtSoyad.Text = bilgi.Soyad;
            txtTelefon.Text = bilgi.Telefon;
            txtEmail.Text = bilgi.Email;
            txtAdres.Text = bilgi.Adres;
            txtIl.Text = bilgi.Il;
            txtIlce.Text = bilgi.Ilce;
            txtPostaKodu.Text = bilgi.PostaKodu ?? "";
        }
    }
}
