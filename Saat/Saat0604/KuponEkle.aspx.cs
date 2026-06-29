using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class KuponEkle : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    KuponBilgileriniGetir(id);
                }
            }
        }

        protected void lnkKaydet_Click(object sender, EventArgs e)
        {
            decimal indDeger = 0, minTutar = 0;
            decimal.TryParse(txtIndirimDegeri.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out indDeger);
            decimal.TryParse(txtMinTutar.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out minTutar);
            DateTime? baslangic = ParseTarih(txtBaslangic.Text);
            DateTime? bitis = ParseTarih(txtBitis.Text);
            int? limit = ParseInt(txtLimit.Text);

            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"INSERT INTO Kupon (KuponKodu, IndirimTipi, IndirimDegeri, MinSiparisTutari, BaslangicTarihi, BitisTarihi, KullanimLimiti, Aktif)
                                                     VALUES (@KuponKodu, @IndirimTipi, @IndirimDegeri, @Min, @Baslangic, @Bitis, @Limit, @Aktif)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@KuponKodu", txtKuponKodu.Text);
                com.Parameters.AddWithValue("@IndirimTipi", ddlIndirimTipi.SelectedValue);
                com.Parameters.AddWithValue("@IndirimDegeri", indDeger);
                com.Parameters.AddWithValue("@Min", minTutar == 0 ? (object)DBNull.Value : minTutar);
                com.Parameters.AddWithValue("@Baslangic", baslangic.HasValue ? (object)baslangic.Value : DBNull.Value);
                com.Parameters.AddWithValue("@Bitis", bitis.HasValue ? (object)bitis.Value : DBNull.Value);
                com.Parameters.AddWithValue("@Limit", limit.HasValue ? (object)limit.Value : DBNull.Value);
                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
            Response.Redirect("KuponListe.aspx");
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            decimal indDeger = 0, minTutar = 0;
            decimal.TryParse(txtIndirimDegeri.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out indDeger);
            decimal.TryParse(txtMinTutar.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out minTutar);
            DateTime? baslangic = ParseTarih(txtBaslangic.Text);
            DateTime? bitis = ParseTarih(txtBitis.Text);
            int? limit = ParseInt(txtLimit.Text);

            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"UPDATE Kupon SET KuponKodu=@KuponKodu, IndirimTipi=@IndirimTipi, IndirimDegeri=@IndirimDegeri, MinSiparisTutari=@Min, 
                                                     BaslangicTarihi=@Baslangic, BitisTarihi=@Bitis, KullanimLimiti=@Limit, Aktif=@Aktif WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", lblKuponID.Text);
                com.Parameters.AddWithValue("@KuponKodu", txtKuponKodu.Text);
                com.Parameters.AddWithValue("@IndirimTipi", ddlIndirimTipi.SelectedValue);
                com.Parameters.AddWithValue("@IndirimDegeri", indDeger);
                com.Parameters.AddWithValue("@Min", minTutar == 0 ? (object)DBNull.Value : minTutar);
                com.Parameters.AddWithValue("@Baslangic", baslangic.HasValue ? (object)baslangic.Value : DBNull.Value);
                com.Parameters.AddWithValue("@Bitis", bitis.HasValue ? (object)bitis.Value : DBNull.Value);
                com.Parameters.AddWithValue("@Limit", limit.HasValue ? (object)limit.Value : DBNull.Value);
                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
            Response.Redirect("KuponListe.aspx");
        }

        protected void lnkIptal_Click(object sender, EventArgs e)
        {
            Response.Redirect("KuponListe.aspx");
        }

        private void KuponBilgileriniGetir(int id)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT * FROM Kupon WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    lblKuponID.Text = id.ToString();
                    txtKuponKodu.Text = Duzelt(dr["KuponKodu"]);
                    ddlIndirimTipi.SelectedValue = Duzelt(dr["IndirimTipi"]);
                    txtIndirimDegeri.Text = dr["IndirimDegeri"] != DBNull.Value ? Convert.ToDecimal(dr["IndirimDegeri"]).ToString(CultureInfo.InvariantCulture) : "";
                    txtMinTutar.Text = dr["MinSiparisTutari"] != DBNull.Value ? Convert.ToDecimal(dr["MinSiparisTutari"]).ToString(CultureInfo.InvariantCulture) : "";
                    txtBaslangic.Text = dr["BaslangicTarihi"] != DBNull.Value ? Convert.ToDateTime(dr["BaslangicTarihi"]).ToString("dd.MM.yyyy") : "";
                    txtBitis.Text = dr["BitisTarihi"] != DBNull.Value ? Convert.ToDateTime(dr["BitisTarihi"]).ToString("dd.MM.yyyy") : "";
                    txtLimit.Text = dr["KullanimLimiti"] != DBNull.Value ? dr["KullanimLimiti"].ToString() : "";
                    chkAktif.Checked = dr["Aktif"] != DBNull.Value && Convert.ToBoolean(dr["Aktif"]);
                    lnkKaydet.Visible = false;
                    lnkGuncelle.Visible = true;
                }
                com.Connection.Close();
            }
        }

        private DateTime? ParseTarih(string t)
        {
            if (string.IsNullOrWhiteSpace(t)) return null;
            DateTime d;
            if (DateTime.TryParseExact(t.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                return d;
            if (DateTime.TryParse(t, out d)) return d;
            return null;
        }

        private int? ParseInt(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            int v; if (int.TryParse(s, out v)) return v;
            return null;
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }
    }
}
