
using Saat.App_Code;
using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class KasaBoyutuSil : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;
                if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out id))
                {
                    Response.Redirect("KasaBoyutuListe.aspx");
                    return;
                }
                Sil(id);
            }
        }

        private void Sil(int bedenId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            using (SqlCommand nullVaryant = new SqlCommand("UPDATE UrunVaryant SET BedenId = NULL WHERE BedenId=@ID", b.Baglanti))
            {
                nullVaryant.Parameters.AddWithValue("@ID", bedenId);
                if (nullVaryant.Connection.State == System.Data.ConnectionState.Closed) nullVaryant.Connection.Open();
                nullVaryant.ExecuteNonQuery();
                nullVaryant.Connection.Close();
            }

            using (SqlCommand del = new SqlCommand("DELETE FROM kasaboyutu WHERE ID=@ID", b.Baglanti))
            {
                del.Parameters.AddWithValue("@ID", bedenId);
                if (del.Connection.State == System.Data.ConnectionState.Closed) del.Connection.Open();
                del.ExecuteNonQuery();
                del.Connection.Close();
            }

            Response.Write("<script>alert('Kasa Boyutu başarıyla silindi.'); window.location='KasaBoyutuListe.aspx';</script>");
        }
    }
}
