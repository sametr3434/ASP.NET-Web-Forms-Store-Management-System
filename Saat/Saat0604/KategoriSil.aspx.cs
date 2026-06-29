using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class KategoriSil : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;
                if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out id))
                {
                    Response.Redirect("KategoriListe.aspx");
                    return;
                }

                Sil(id);
            }
        }

        private void Sil(int kategoriId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            // Alt kategori kontrolü
            using (SqlCommand chk = new SqlCommand("SELECT COUNT(1) FROM Kategori WHERE UstId = @ID", b.Baglanti))
            {
                chk.Parameters.AddWithValue("@ID", kategoriId);
                if (chk.Connection.State == ConnectionState.Closed) chk.Connection.Open();
                int altSayisi = Convert.ToInt32(chk.ExecuteScalar());
                chk.Connection.Close();
                if (altSayisi > 0)
                {
                    Response.Write("<script>alert('Bu kategoriye bağlı alt kategoriler var. Lütfen önce alt kategorileri kaldırın.'); window.location='KategoriListe.aspx';</script>");
                    return;
                }
            }

            string resimAdi = "";
            using (SqlCommand get = new SqlCommand("SELECT Resim FROM Kategori WHERE ID=@ID", b.Baglanti))
            {
                get.Parameters.AddWithValue("@ID", kategoriId);
                if (get.Connection.State == ConnectionState.Closed) get.Connection.Open();
                object o = get.ExecuteScalar();
                get.Connection.Close();
                if (o != null && o != DBNull.Value) resimAdi = o.ToString();
            }

            // İlişkileri kaldır
            using (SqlCommand delRel = new SqlCommand("DELETE FROM UrunKategori WHERE KategoriId=@ID", b.Baglanti))
            {
                delRel.Parameters.AddWithValue("@ID", kategoriId);
                if (delRel.Connection.State == ConnectionState.Closed) delRel.Connection.Open();
                delRel.ExecuteNonQuery();
                delRel.Connection.Close();
            }

            // Kategori kaydını sil
            using (SqlCommand del = new SqlCommand("DELETE FROM Kategori WHERE ID=@ID", b.Baglanti))
            {
                del.Parameters.AddWithValue("@ID", kategoriId);
                if (del.Connection.State == ConnectionState.Closed) del.Connection.Open();
                del.ExecuteNonQuery();
                del.Connection.Close();
            }

            // Resim dosyasını sil
            try
            {
                if (!string.IsNullOrWhiteSpace(resimAdi))
                {
                    string yol = Server.MapPath("~/Upload/") + resimAdi;
                    if (File.Exists(yol))
                    {
                        File.Delete(yol);
                    }
                }
            }
            catch { /* dosya silme hataları yutulur */ }

            Response.Write("<script>alert('Kategori ve ilişkileri başarıyla silindi.'); window.location='KategoriListe.aspx';</script>");
        }
    }
}
