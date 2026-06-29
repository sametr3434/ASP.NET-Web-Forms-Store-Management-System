
using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class UrunDuzenle : Page
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
                int urunId = Convert.ToInt32(Request.QueryString["id"]);
                lblUrunID.Text = urunId.ToString();
                MarkalariDoldur();
                KategorileriDoldur();
                KasaCapiDoldur();
                RenkleriDoldur();
                UrunBilgileriniGetir(urunId);
                UrunKategorileriniGetir(urunId);
                VaryantlariGetir(urunId);
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        private static bool KolonVar(SqlDataReader dr, string name)
        {
            try { dr.GetOrdinal(name); return true; }
            catch { return false; }
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
            }
        }

        private void KasaCapiDoldur()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT ID, kasano FROM kasaboyutu WHERE Aktif=1 ORDER BY Siralama, kasano", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                ddlKasaBoyu.DataSource = dt;
                ddlKasaBoyu.DataTextField = "kasano";
                ddlKasaBoyu.DataValueField = "ID";
                ddlKasaBoyu.DataBind();
                for (int i = 0; i < ddlKasaBoyu.Items.Count; i++)
                    ddlKasaBoyu.Items[i].Text = EncodingHelper.DuzeltTurkce(ddlKasaBoyu.Items[i].Text);
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

        private void UrunBilgileriniGetir(int urunId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT * FROM Urun WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", urunId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    txtUrunAdi.Text = Duzelt(dr["UrunAdi"]);
                    txtUrunKodu.Text = Duzelt(dr["UrunKodu"]);
                    txtBarkod.Text = Duzelt(dr["Barkod"]);
                    ddlMarka.SelectedValue = dr["MarkaId"] != DBNull.Value ? dr["MarkaId"].ToString() : "";
                    txtFiyat.Text = dr["Fiyat"] != DBNull.Value ? Convert.ToDecimal(dr["Fiyat"]).ToString(CultureInfo.InvariantCulture) : "0";
                    txtIndirimli.Text = dr["IndirimliFiyat"] != DBNull.Value ? Convert.ToDecimal(dr["IndirimliFiyat"]).ToString(CultureInfo.InvariantCulture) : "";
                    txtFiyatUSD.Text = KolonVar(dr, "FiyatUSD") && dr["FiyatUSD"] != DBNull.Value ? Convert.ToDecimal(dr["FiyatUSD"]).ToString(CultureInfo.InvariantCulture) : "";
                    txtFiyatEUR.Text = KolonVar(dr, "FiyatEUR") && dr["FiyatEUR"] != DBNull.Value ? Convert.ToDecimal(dr["FiyatEUR"]).ToString(CultureInfo.InvariantCulture) : "";
                    txtIndirimliUSD.Text = KolonVar(dr, "IndirimliFiyatUSD") && dr["IndirimliFiyatUSD"] != DBNull.Value ? Convert.ToDecimal(dr["IndirimliFiyatUSD"]).ToString(CultureInfo.InvariantCulture) : "";
                    txtIndirimliEUR.Text = KolonVar(dr, "IndirimliFiyatEUR") && dr["IndirimliFiyatEUR"] != DBNull.Value ? Convert.ToDecimal(dr["IndirimliFiyatEUR"]).ToString(CultureInfo.InvariantCulture) : "";
                    txtStok.Text = dr["StokAdedi"] != DBNull.Value ? dr["StokAdedi"].ToString() : "0";
                    chkAktif.Checked = dr["Aktif"] != DBNull.Value && Convert.ToBoolean(dr["Aktif"]);
                    chkVitrin.Checked = dr["Vitrin"] != DBNull.Value && Convert.ToBoolean(dr["Vitrin"]);
                    chkOnerilen.Checked = dr["Onerilen"] != DBNull.Value && Convert.ToBoolean(dr["Onerilen"]);
                }
                com.Connection.Close();
            }
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            int urunId = Convert.ToInt32(lblUrunID.Text);
            decimal fiyat = 0, indirimli = 0, fUsd = 0, fEur = 0, iUsd = 0, iEur = 0;
            int stok = 0;
            var inv = CultureInfo.InvariantCulture;
            decimal.TryParse(txtFiyat.Text.Replace(",", "."), NumberStyles.Any, inv, out fiyat);
            decimal.TryParse(txtIndirimli.Text.Replace(",", "."), NumberStyles.Any, inv, out indirimli);
            decimal.TryParse(txtFiyatUSD.Text.Replace(",", "."), NumberStyles.Any, inv, out fUsd);
            decimal.TryParse(txtFiyatEUR.Text.Replace(",", "."), NumberStyles.Any, inv, out fEur);
            decimal.TryParse(txtIndirimliUSD.Text.Replace(",", "."), NumberStyles.Any, inv, out iUsd);
            decimal.TryParse(txtIndirimliEUR.Text.Replace(",", "."), NumberStyles.Any, inv, out iEur);
            int.TryParse(txtStok.Text, out stok);

            string anaResimSet = "";
            string yeniAnaResimDosyaAdi = null;
            if (fuAnaResim.HasFile)
            {
                string uzanti = Path.GetExtension(fuAnaResim.FileName).ToLowerInvariant();
                if (uzanti == ".jpg" || uzanti == ".jpeg" || uzanti == ".png")
                {
                    yeniAnaResimDosyaAdi = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + uzanti;
                    string uploadDir = Server.MapPath("~/Upload");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);
                    fuAnaResim.SaveAs(Path.Combine(uploadDir, yeniAnaResimDosyaAdi));
                    anaResimSet = ", AnaResim=@AnaResim";
                }
            }

            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand($@"
                UPDATE Urun SET MarkaId=@MarkaId, UrunAdi=@UrunAdi, UrunKodu=@UrunKodu, Barkod=@Barkod,
                    Fiyat=@Fiyat, IndirimliFiyat=@IndirimliFiyat, FiyatUSD=@FiyatUSD, FiyatEUR=@FiyatEUR,
                    IndirimliFiyatUSD=@IndirimliFiyatUSD, IndirimliFiyatEUR=@IndirimliFiyatEUR,
                    StokAdedi=@Stok, Aktif=@Aktif, Vitrin=@Vitrin, Onerilen=@Onerilen {anaResimSet}
                WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", urunId);
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
                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);
                com.Parameters.AddWithValue("@Vitrin", chkVitrin.Checked);
                com.Parameters.AddWithValue("@Onerilen", chkOnerilen.Checked);
                if (!string.IsNullOrEmpty(anaResimSet) && !string.IsNullOrEmpty(yeniAnaResimDosyaAdi))
                {
                    com.Parameters.AddWithValue("@AnaResim", yeniAnaResimDosyaAdi);
                }
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
        }

        private void UrunKategorileriniGetir(int urunId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                SELECT uk.ID as UKID, k.ID, k.KategoriAdi 
                FROM UrunKategori uk INNER JOIN Kategori k ON uk.KategoriId = k.ID
                WHERE uk.UrunId=@UrunId
                ORDER BY k.KategoriAdi", b.Baglanti))
            {
                com.Parameters.AddWithValue("@UrunId", urunId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                foreach (DataRow row in dt.Rows)
                {
                    row["KategoriAdi"] = EncodingHelper.DuzeltTurkce(row["KategoriAdi"].ToString());
                }
                rptKategoriler.DataSource = dt;
                rptKategoriler.DataBind();
            }
        }

        protected void btnKategoriEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlKategori.SelectedValue)) return;
            int urunId = Convert.ToInt32(lblUrunID.Text);
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("IF NOT EXISTS (SELECT 1 FROM UrunKategori WHERE UrunId=@UrunId AND KategoriId=@KategoriId) INSERT INTO UrunKategori (UrunId, KategoriId) VALUES (@UrunId, @KategoriId)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@UrunId", urunId);
                com.Parameters.AddWithValue("@KategoriId", ddlKategori.SelectedValue);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
            UrunKategorileriniGetir(urunId);
        }

        protected void rptKategoriler_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                int ukid = Convert.ToInt32(e.CommandArgument);
                int urunId = Convert.ToInt32(lblUrunID.Text);
                BaglantiBilgileri b = new BaglantiBilgileri();
                using (SqlCommand com = new SqlCommand("DELETE FROM UrunKategori WHERE ID=@ID", b.Baglanti))
                {
                    com.Parameters.AddWithValue("@ID", ukid);
                    if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Connection.Close();
                }
                UrunKategorileriniGetir(urunId);
            }
        }

        private void VaryantlariGetir(int urunId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                SELECT v.ID, b.kasano, r.RenkAdi, v.StokAdedi, v.Barkod, v.FiyatFarki, v.FiyatFarkiUSD, v.FiyatFarkiEUR
                FROM UrunVaryant v
                LEFT JOIN kasaboyutu b ON v.KasaId = b.ID
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
                    if (row["kasano"] != DBNull.Value) row["kasano"] = EncodingHelper.DuzeltTurkce(row["kasano"].ToString());
                    if (row["RenkAdi"] != DBNull.Value) row["RenkAdi"] = EncodingHelper.DuzeltTurkce(row["RenkAdi"].ToString());
                }
                rptVaryantlar.DataSource = dt;
                rptVaryantlar.DataBind();
            }
        }

        protected void btnVaryantEkle_Click(object sender, EventArgs e)
        {
            int urunId = Convert.ToInt32(lblUrunID.Text);
            int? kasaId = string.IsNullOrEmpty(ddlKasaBoyu.SelectedValue) ? (int?)null : Convert.ToInt32(ddlKasaBoyu.SelectedValue);
            int? renkId = string.IsNullOrEmpty(ddlRenk.SelectedValue) ? (int?)null : Convert.ToInt32(ddlRenk.SelectedValue);
            int stok = 0; int.TryParse(txtVaryantStok.Text, out stok);
            var inv = CultureInfo.InvariantCulture;
            decimal fiyatFarki = 0; decimal.TryParse(txtFiyatFarki.Text.Replace(",", "."), NumberStyles.Any, inv, out fiyatFarki);
            decimal fUsd = 0, fEur = 0;
            decimal.TryParse(txtFiyatFarkiUSD.Text.Replace(",", "."), NumberStyles.Any, inv, out fUsd);
            decimal.TryParse(txtFiyatFarkiEUR.Text.Replace(",", "."), NumberStyles.Any, inv, out fEur);
            string barkod = txtVaryantBarkod.Text;

            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                INSERT INTO UrunVaryant (UrunId, KasaId, RenkId, StokAdedi, Barkod, FiyatFarki, FiyatFarkiUSD, FiyatFarkiEUR)
                VALUES (@UrunId, @KasaId, @RenkId, @Stok, @Barkod, @FiyatFarki, @FiyatFarkiUSD, @FiyatFarkiEUR)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@UrunId", urunId);
                com.Parameters.AddWithValue("@KasaId", (object)kasaId ?? DBNull.Value);
                com.Parameters.AddWithValue("@RenkId", (object)renkId ?? DBNull.Value);
                com.Parameters.AddWithValue("@Stok", stok);
                com.Parameters.AddWithValue("@Barkod", string.IsNullOrEmpty(barkod) ? (object)DBNull.Value : barkod);
                com.Parameters.AddWithValue("@FiyatFarki", fiyatFarki == 0 ? (object)DBNull.Value : fiyatFarki);
                com.Parameters.AddWithValue("@FiyatFarkiUSD", fUsd == 0 ? (object)DBNull.Value : fUsd);
                com.Parameters.AddWithValue("@FiyatFarkiEUR", fEur == 0 ? (object)DBNull.Value : fEur);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
            VaryantlariGetir(urunId);
        }

        protected void rptVaryantlar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                int urunId = Convert.ToInt32(lblUrunID.Text);
                BaglantiBilgileri b = new BaglantiBilgileri();
                using (SqlCommand com = new SqlCommand("DELETE FROM UrunVaryant WHERE ID=@ID", b.Baglanti))
                {
                    com.Parameters.AddWithValue("@ID", id);
                    if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Connection.Close();
                }
                VaryantlariGetir(urunId);
            }
        }
    }
}
