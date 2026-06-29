using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class UrunListe : Page
    {
        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        /// <summary>Liste + indirimli TL g&#246;sterimi.</summary>
        protected string FiyatListeTL(object liste, object indirimli)
        {
            var l = liste != null && liste != DBNull.Value ? Convert.ToDecimal(liste) : 0m;
            var ind = indirimli != null && indirimli != DBNull.Value ? (decimal?)Convert.ToDecimal(indirimli) : null;
            if (ind.HasValue && l > 0m && ind.Value > 0m && ind.Value < l)
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "<span class=\"text-muted\"><del>{0:N2} \u20ba</del></span> {1:N2} \u20ba", l, ind.Value);
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:N2} \u20ba", l);
        }

        protected string FiyatListeUsd(object liste, object indirimli)
        {
            if (liste == null || liste == DBNull.Value) return "-";
            var l = Convert.ToDecimal(liste);
            if (l <= 0m) return "-";
            var ind = indirimli != null && indirimli != DBNull.Value ? (decimal?)Convert.ToDecimal(indirimli) : null;
            if (ind.HasValue && ind.Value > 0m && ind.Value < l)
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "<span class=\"text-muted\"><del>{0:N2} $</del></span> {1:N2} $", l, ind.Value);
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:N2} $", l);
        }

        protected string FiyatListeEur(object liste, object indirimli)
        {
            if (liste == null || liste == DBNull.Value) return "-";
            var l = Convert.ToDecimal(liste);
            if (l <= 0m) return "-";
            var ind = indirimli != null && indirimli != DBNull.Value ? (decimal?)Convert.ToDecimal(indirimli) : null;
            if (ind.HasValue && ind.Value > 0m && ind.Value < l)
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "<span class=\"text-muted\"><del>{0:N2} \u20ac</del></span> {1:N2} \u20ac", l, ind.Value);
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:N2} \u20ac", l);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                KategorileriDoldur();
                MarkalariDoldur();
                BindUrunler();
            }
        }

        private void KategorileriDoldur()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT ID, KategoriAdi FROM Kategori WHERE Aktif=1 ORDER BY KategoriAdi", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                ddlKategori.DataSource = dt;
                ddlKategori.DataTextField = "KategoriAdi";
                ddlKategori.DataValueField = "ID";
                ddlKategori.DataBind();
                for (int i = 0; i < ddlKategori.Items.Count; i++)
                    ddlKategori.Items[i].Text = EncodingHelper.DuzeltTurkce(ddlKategori.Items[i].Text);
                // Duplicates (empty value) cleanup: keep only the first
                for (int i = ddlKategori.Items.Count - 1; i >= 1; i--)
                {
                    if (ddlKategori.Items[i].Value == "")
                        ddlKategori.Items.RemoveAt(i);
                }
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
                for (int i = 0; i < ddlMarka.Items.Count; i++)
                    ddlMarka.Items[i].Text = EncodingHelper.DuzeltTurkce(ddlMarka.Items[i].Text);
                // Duplicates (empty value) cleanup: keep only the first
                for (int i = ddlMarka.Items.Count - 1; i >= 1; i--)
                {
                    if (ddlMarka.Items[i].Value == "")
                        ddlMarka.Items.RemoveAt(i);
                }
            }
        }

        private void BindUrunler()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            string sql = @"
                SELECT u.ID, u.UrunAdi, u.UrunKodu, u.Fiyat, u.IndirimliFiyat, u.FiyatUSD, u.IndirimliFiyatUSD, u.FiyatEUR, u.IndirimliFiyatEUR, u.StokAdedi, u.Aktif, m.MarkaAdi
                FROM Urun u
                LEFT JOIN Marka m ON u.MarkaId = m.ID
                WHERE 1=1 ";

            if (!string.IsNullOrEmpty(ddlMarka.SelectedValue))
                sql += " AND u.MarkaId = @MarkaId ";
            if (!string.IsNullOrEmpty(ddlKategori.SelectedValue))
                sql += " AND EXISTS (SELECT 1 FROM UrunKategori uk WHERE uk.UrunId = u.ID AND uk.KategoriId = @KategoriId) ";
            sql += " ORDER BY u.ID DESC";

            using (SqlCommand com = new SqlCommand(sql, b.Baglanti))
            {
                if (!string.IsNullOrEmpty(ddlMarka.SelectedValue))
                    com.Parameters.AddWithValue("@MarkaId", ddlMarka.SelectedValue);
                if (!string.IsNullOrEmpty(ddlKategori.SelectedValue))
                    com.Parameters.AddWithValue("@KategoriId", ddlKategori.SelectedValue);

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (row[col] != DBNull.Value && row[col] != null && col.DataType == typeof(string))
                            row[col] = EncodingHelper.DuzeltTurkce(row[col].ToString());
                    }
                }
                rptUrunler.DataSource = dt;
                rptUrunler.DataBind();
            }
        }

        protected void btnFiltrele_Click(object sender, EventArgs e)
        {
            BindUrunler();
        }

        protected void btnTemizle_Click(object sender, EventArgs e)
        {
            ddlKategori.SelectedValue = "";
            ddlMarka.SelectedValue = "";
            BindUrunler();
        }

        protected void rptUrunler_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("UrunSil.aspx?id=" + id);
            }
        }
    }
}
