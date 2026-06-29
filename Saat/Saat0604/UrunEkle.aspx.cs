
using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class UrunEkle : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MarkalariDoldur();
            }
        }

        private void MarkalariDoldur()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT ID, MarkaAdi FROM Marka WHERE Aktif=1 ORDER BY MarkaAdi", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                ddlMarka.DataSource = dt;
                ddlMarka.DataTextField = "MarkaAdi";
                ddlMarka.DataValueField = "ID";
                ddlMarka.DataBind();
                ddlMarka.Items.Insert(0, new ListItem("Seçiniz", ""));
                for (int i = 0; i < ddlMarka.Items.Count; i++)
                    ddlMarka.Items[i].Text = EncodingHelper.DuzeltTurkce(ddlMarka.Items[i].Text);
            }
        }

        protected void lnkKaydet_Click(object sender, EventArgs e)
        {
            decimal fiyat = 0, indirimli = 0, fUsd = 0, fEur = 0, iUsd = 0, iEur = 0;
            int stok = 0;
            var inv = System.Globalization.CultureInfo.InvariantCulture;
            decimal.TryParse(txtFiyat.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out fiyat);
            decimal.TryParse(txtIndirimli.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out indirimli);
            decimal.TryParse(txtFiyatUSD.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out fUsd);
            decimal.TryParse(txtFiyatEUR.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out fEur);
            decimal.TryParse(txtIndirimliUSD.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out iUsd);
            decimal.TryParse(txtIndirimliEUR.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, inv, out iEur);
            int.TryParse(txtStok.Text, out stok);

            string anaResim = "";
            if (fuAnaResim.HasFile)
            {
                string uzanti = Path.GetExtension(fuAnaResim.FileName).ToLowerInvariant();
                if (uzanti == ".jpg" || uzanti == ".jpeg" || uzanti == ".png")
                {
                    anaResim = DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture) + uzanti;
                    string uploadDir = Server.MapPath("~/Upload");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);
                    fuAnaResim.SaveAs(Path.Combine(uploadDir, anaResim));
                }
            }

            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                INSERT INTO Urun (MarkaId, UrunAdi, UrunKodu, Barkod, Fiyat, IndirimliFiyat, FiyatUSD, FiyatEUR, IndirimliFiyatUSD, IndirimliFiyatEUR, StokAdedi, AnaResim, Aktif, Vitrin, Onerilen)
                VALUES (@MarkaId, @UrunAdi, @UrunKodu, @Barkod, @Fiyat, @IndirimliFiyat, @FiyatUSD, @FiyatEUR, @IndirimliFiyatUSD, @IndirimliFiyatEUR, @Stok, @AnaResim, @Aktif, @Vitrin, @Onerilen);
                SELECT CAST(SCOPE_IDENTITY() AS INT);", b.Baglanti))
            {
                com.Parameters.AddWithValue("@MarkaId", string.IsNullOrEmpty(ddlMarka.SelectedValue) ? (object)DBNull.Value : ddlMarka.SelectedValue);
                com.Parameters.AddWithValue("@UrunAdi", txtUrunAdi.Text);
                com.Parameters.AddWithValue("@UrunKodu", (object)txtUrunKodu.Text ?? DBNull.Value);
                com.Parameters.AddWithValue("@Barkod", (object)txtBarkod.Text ?? DBNull.Value);
                com.Parameters.AddWithValue("@Fiyat", fiyat);
                com.Parameters.AddWithValue("@IndirimliFiyat", indirimli == 0 ? (object)DBNull.Value : indirimli);
                com.Parameters.AddWithValue("@FiyatUSD", fUsd == 0 ? (object)DBNull.Value : fUsd);
                com.Parameters.AddWithValue("@FiyatEUR", fEur == 0 ? (object)DBNull.Value : fEur);
                com.Parameters.AddWithValue("@IndirimliFiyatUSD", iUsd == 0 ? (object)DBNull.Value : iUsd);
                com.Parameters.AddWithValue("@IndirimliFiyatEUR", iEur == 0 ? (object)DBNull.Value : iEur);
                com.Parameters.AddWithValue("@Stok", stok);
                com.Parameters.AddWithValue("@AnaResim", anaResim);
                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);
                com.Parameters.AddWithValue("@Vitrin", chkVitrin.Checked);
                com.Parameters.AddWithValue("@Onerilen", chkOnerilen.Checked);

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                int yeniId = Convert.ToInt32(com.ExecuteScalar());
                com.Connection.Close();
                Response.Redirect("UrunDuzenle.aspx?id=" + yeniId);
            }
        }
    }
}
