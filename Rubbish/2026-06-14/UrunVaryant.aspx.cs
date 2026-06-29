using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class UrunVaryant : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == null)
                {
                    Response.Redirect("UrunListe.aspx");
                    return;
                }
                BedenleriDoldur();
                RenkleriDoldur();
                VaryantlariGetir();
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        private void BedenleriDoldur()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT ID, BedenNo FROM Beden WHERE Aktif=1 ORDER BY Siralama, BedenNo", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                ddlBeden.DataSource = dt;
                ddlBeden.DataTextField = "BedenNo";
                ddlBeden.DataValueField = "ID";
                ddlBeden.DataBind();
                for (int i = 0; i < ddlBeden.Items.Count; i++)
                    ddlBeden.Items[i].Text = EncodingHelper.DuzeltTurkce(ddlBeden.Items[i].Text);
            }
        }

        private void RenkleriDoldur()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT ID, RenkAdi FROM Renk WHERE Aktif=1 ORDER BY Siralama, RenkAdi", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                ddlRenk.DataSource = dt;
                ddlRenk.DataTextField = "RenkAdi";
                ddlRenk.DataValueField = "ID";
                ddlRenk.DataBind();
                for (int i = 0; i < ddlRenk.Items.Count; i++)
                    ddlRenk.Items[i].Text = EncodingHelper.DuzeltTurkce(ddlRenk.Items[i].Text);
            }
        }

        protected void btnEkle_Click(object sender, EventArgs e)
        {
            int urunId = Convert.ToInt32(Request.QueryString["id"]);
            int? bedenId = string.IsNullOrEmpty(ddlBeden.SelectedValue) ? (int?)null : Convert.ToInt32(ddlBeden.SelectedValue);
            int? renkId = string.IsNullOrEmpty(ddlRenk.SelectedValue) ? (int?)null : Convert.ToInt32(ddlRenk.SelectedValue);
            int stok = 0; int.TryParse(txtStok.Text, out stok);
            var inv = CultureInfo.InvariantCulture;
            decimal fark = 0, fUsd = 0, fEur = 0;
            decimal.TryParse(txtFiyatFarki.Text.Replace(",", "."), NumberStyles.Any, inv, out fark);
            decimal.TryParse(txtFiyatFarkiUSD.Text.Replace(",", "."), NumberStyles.Any, inv, out fUsd);
            decimal.TryParse(txtFiyatFarkiEUR.Text.Replace(",", "."), NumberStyles.Any, inv, out fEur);
            string barkod = txtBarkod.Text;

            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                INSERT INTO UrunVaryant (UrunId, BedenId, RenkId, StokAdedi, Barkod, FiyatFarki, FiyatFarkiUSD, FiyatFarkiEUR)
                VALUES (@UrunId, @BedenId, @RenkId, @Stok, @Barkod, @FiyatFarki, @FiyatFarkiUSD, @FiyatFarkiEUR)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@UrunId", urunId);
                com.Parameters.AddWithValue("@BedenId", (object)bedenId ?? DBNull.Value);
                com.Parameters.AddWithValue("@RenkId", (object)renkId ?? DBNull.Value);
                com.Parameters.AddWithValue("@Stok", stok);
                com.Parameters.AddWithValue("@Barkod", string.IsNullOrEmpty(barkod) ? (object)DBNull.Value : barkod);
                com.Parameters.AddWithValue("@FiyatFarki", fark == 0 ? (object)DBNull.Value : fark);
                com.Parameters.AddWithValue("@FiyatFarkiUSD", fUsd == 0 ? (object)DBNull.Value : fUsd);
                com.Parameters.AddWithValue("@FiyatFarkiEUR", fEur == 0 ? (object)DBNull.Value : fEur);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
            VaryantlariGetir();
        }

        private void VaryantlariGetir()
        {
            int urunId = Convert.ToInt32(Request.QueryString["id"]);
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                SELECT v.ID, b.BedenNo, r.RenkAdi, v.StokAdedi, v.Barkod, v.FiyatFarki, v.FiyatFarkiUSD, v.FiyatFarkiEUR
                FROM UrunVaryant v
                LEFT JOIN Beden b ON v.BedenId = b.ID
                LEFT JOIN Renk r ON v.RenkId = r.ID
                WHERE v.UrunId=@UrunId
                ORDER BY b.Siralama, r.Siralama", b.Baglanti))
            {
                com.Parameters.AddWithValue("@UrunId", urunId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["BedenNo"] != DBNull.Value) row["BedenNo"] = EncodingHelper.DuzeltTurkce(row["BedenNo"].ToString());
                    if (row["RenkAdi"] != DBNull.Value) row["RenkAdi"] = EncodingHelper.DuzeltTurkce(row["RenkAdi"].ToString());
                }
                rptVaryantlar.DataSource = dt;
                rptVaryantlar.DataBind();
            }
        }

        protected void rptVaryantlar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            BaglantiBilgileri b = new BaglantiBilgileri();
            if (e.CommandName == "Sil")
            {
                using (SqlCommand com = new SqlCommand("DELETE FROM UrunVaryant WHERE ID=@ID", b.Baglanti))
                {
                    com.Parameters.AddWithValue("@ID", id);
                    if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Connection.Close();
                }
                VaryantlariGetir();
            }
            else if (e.CommandName == "Guncelle")
            {
                var txtStokRow = e.Item.FindControl("txtStokRow") as TextBox;
                var txtBarkodRow = e.Item.FindControl("txtBarkodRow") as TextBox;
                var txtFarkRow = e.Item.FindControl("txtFarkRow") as TextBox;
                var txtFarkRowUSD = e.Item.FindControl("txtFarkRowUSD") as TextBox;
                var txtFarkRowEUR = e.Item.FindControl("txtFarkRowEUR") as TextBox;
                int stok = 0; if (txtStokRow != null) int.TryParse(txtStokRow.Text, out stok);
                string barkod = txtBarkodRow != null ? txtBarkodRow.Text : null;
                var inv = CultureInfo.InvariantCulture;
                decimal fark = 0, fUsd = 0, fEur = 0;
                if (txtFarkRow != null) decimal.TryParse(txtFarkRow.Text.Replace(",", "."), NumberStyles.Any, inv, out fark);
                if (txtFarkRowUSD != null) decimal.TryParse(txtFarkRowUSD.Text.Replace(",", "."), NumberStyles.Any, inv, out fUsd);
                if (txtFarkRowEUR != null) decimal.TryParse(txtFarkRowEUR.Text.Replace(",", "."), NumberStyles.Any, inv, out fEur);
                using (SqlCommand com = new SqlCommand("UPDATE UrunVaryant SET StokAdedi=@Stok, Barkod=@Barkod, FiyatFarki=@FiyatFarki, FiyatFarkiUSD=@FiyatFarkiUSD, FiyatFarkiEUR=@FiyatFarkiEUR WHERE ID=@ID", b.Baglanti))
                {
                    com.Parameters.AddWithValue("@ID", id);
                    com.Parameters.AddWithValue("@Stok", stok);
                    com.Parameters.AddWithValue("@Barkod", string.IsNullOrEmpty(barkod) ? (object)DBNull.Value : barkod);
                    com.Parameters.AddWithValue("@FiyatFarki", fark == 0 ? (object)DBNull.Value : fark);
                    com.Parameters.AddWithValue("@FiyatFarkiUSD", fUsd == 0 ? (object)DBNull.Value : fUsd);
                    com.Parameters.AddWithValue("@FiyatFarkiEUR", fEur == 0 ? (object)DBNull.Value : fEur);
                    if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Connection.Close();
                }
                VaryantlariGetir();
            }
        }
    }
}
