using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class UrunResim : Page
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
                ResimleriGetir();
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void lnkYukle_Click(object sender, EventArgs e)
        {
            int urunId = Convert.ToInt32(Request.QueryString["id"]);
            if (!fuResim.HasFile) return;
            string uzanti = Path.GetExtension(fuResim.FileName).ToLower();
            if (uzanti != ".jpg" && uzanti != ".jpeg" && uzanti != ".png") return;
            string dosyaAdi = DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture) + uzanti;
            string uploadDir = Server.MapPath("~/Upload");
            if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);
            fuResim.SaveAs(Path.Combine(uploadDir, dosyaAdi));
            int sira = 999;
            if (txtSira != null) int.TryParse(txtSira.Text, out sira);
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("INSERT INTO UrunResim (UrunId, Resim, Baslik, Siralama) VALUES (@UrunId, @Resim, @Baslik, @Siralama)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@UrunId", urunId);
                com.Parameters.AddWithValue("@Resim", dosyaAdi);
                string baslik = txtBaslik != null ? txtBaslik.Text : null;
                com.Parameters.AddWithValue("@Baslik", string.IsNullOrWhiteSpace(baslik) ? (object)DBNull.Value : baslik);
                com.Parameters.AddWithValue("@Siralama", sira);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }
            ResimleriGetir();
        }

        private void ResimleriGetir()
        {
            int urunId = Convert.ToInt32(Request.QueryString["id"]);
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT ID, Resim, Baslik, Siralama FROM UrunResim WHERE UrunId=@UrunId ORDER BY Siralama, ID DESC", b.Baglanti))
            {
                com.Parameters.AddWithValue("@UrunId", urunId);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
                rptResimler.DataSource = dt;
                rptResimler.DataBind();
            }
        }

        protected void rptResimler_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string resimAdi = "";
                BaglantiBilgileri b = new BaglantiBilgileri();
                using (SqlCommand get = new SqlCommand("SELECT Resim FROM UrunResim WHERE ID=@ID", b.Baglanti))
                {
                    get.Parameters.AddWithValue("@ID", id);
                    if (get.Connection.State == ConnectionState.Closed) get.Connection.Open();
                    object o = get.ExecuteScalar();
                    get.Connection.Close();
                    if (o != null && o != DBNull.Value) resimAdi = o.ToString();
                }
                using (SqlCommand del = new SqlCommand("DELETE FROM UrunResim WHERE ID=@ID", b.Baglanti))
                {
                    del.Parameters.AddWithValue("@ID", id);
                    if (del.Connection.State == ConnectionState.Closed) del.Connection.Open();
                    del.ExecuteNonQuery();
                    del.Connection.Close();
                }
                try
                {
                    if (!string.IsNullOrWhiteSpace(resimAdi))
                    {
                        string yol = Server.MapPath("~/Upload/") + resimAdi;
                        if (File.Exists(yol)) File.Delete(yol);
                    }
                }
                catch { }
                ResimleriGetir();
            }
            else if (e.CommandName == "Guncelle")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var txtBaslikRow = e.Item.FindControl("txtBaslikRow") as TextBox;
                var txtSiraRow = e.Item.FindControl("txtSiraRow") as TextBox;
                string baslik = txtBaslikRow != null ? txtBaslikRow.Text : null;
                int sira = 999;
                if (txtSiraRow != null) int.TryParse(txtSiraRow.Text, out sira);
                BaglantiBilgileri b = new BaglantiBilgileri();
                using (SqlCommand com = new SqlCommand("UPDATE UrunResim SET Baslik=@Baslik, Siralama=@Siralama WHERE ID=@ID", b.Baglanti))
                {
                    com.Parameters.AddWithValue("@ID", id);
                    com.Parameters.AddWithValue("@Baslik", string.IsNullOrWhiteSpace(baslik) ? (object)DBNull.Value : baslik);
                    com.Parameters.AddWithValue("@Siralama", sira);
                    if (com.Connection.State == System.Data.ConnectionState.Closed) com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Connection.Close();
                }
                ResimleriGetir();
            }
        }
    }
}
