using Saat.App_Code;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class MarkaSil : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;
                if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out id))
                {
                    Response.Redirect("MarkaListe.aspx");
                    return;
                }
                Sil(id);
            }
        }

        private void Sil(int markaId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            string logoAdi = "";
            using (SqlCommand get = new SqlCommand("SELECT Logo FROM Marka WHERE ID=@ID", b.Baglanti))
            {
                get.Parameters.AddWithValue("@ID", markaId);
                if (get.Connection.State == System.Data.ConnectionState.Closed) get.Connection.Open();
                object o = get.ExecuteScalar();
                get.Connection.Close();
                if (o != null && o != DBNull.Value) logoAdi = o.ToString();
            }

            using (SqlCommand nullUrun = new SqlCommand("UPDATE Urun SET MarkaId = NULL WHERE MarkaId=@ID", b.Baglanti))
            {
                nullUrun.Parameters.AddWithValue("@ID", markaId);
                if (nullUrun.Connection.State == System.Data.ConnectionState.Closed) nullUrun.Connection.Open();
                nullUrun.ExecuteNonQuery();
                nullUrun.Connection.Close();
            }

            using (SqlCommand del = new SqlCommand("DELETE FROM Marka WHERE ID=@ID", b.Baglanti))
            {
                del.Parameters.AddWithValue("@ID", markaId);
                if (del.Connection.State == System.Data.ConnectionState.Closed) del.Connection.Open();
                del.ExecuteNonQuery();
                del.Connection.Close();
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(logoAdi))
                {
                    string yol = Server.MapPath("~/Upload/") + logoAdi;
                    if (File.Exists(yol)) File.Delete(yol);
                }
            }
            catch { }

            Response.Write("<script>alert('Marka ve ilişkileri başarıyla silindi.'); window.location='MarkaListe.aspx';</script>");
        }
    }
}
