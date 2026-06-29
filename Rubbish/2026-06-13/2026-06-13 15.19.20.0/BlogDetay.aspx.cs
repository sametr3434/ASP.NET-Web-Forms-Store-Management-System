using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Saat
{
    public partial class BlogDetay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string gelenID = null;
            if (Page.RouteData.Values["ID"] != null)
                gelenID = Page.RouteData.Values["ID"].ToString();

            if (string.IsNullOrEmpty(gelenID))
                gelenID = Request.QueryString["ID"];

            if (string.IsNullOrEmpty(gelenID))
            {
                Response.Redirect("/blog");
                return;
            }

            SiteSabitlerini(gelenID);
            BlogGetir(gelenID);
            BlogListesiGetir(gelenID);
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void SiteSabitlerini(string gelenID)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            SqlCommand com = new SqlCommand(
                "SELECT Resim, sitebaslik, (SELECT baslik FROM blog WHERE ID=@GelenID) AS blogadi FROM sitesabitleri WHERE ID=@ID",
                b.Baglanti);
            com.Parameters.AddWithValue("@ID", "1");
            com.Parameters.AddWithValue("@GelenID", gelenID);

            if (com.Connection.State == ConnectionState.Closed)
                b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();
            if (dr.Read())
            {
                Title = EncodingHelper.DuzeltTurkce(dr["sitebaslik"].ToString()) + " - Blog - "
                    + EncodingHelper.DuzeltTurkce(dr["blogadi"]?.ToString() ?? "");
            }
            dr.Close();
            b.Baglanti.Close();
        }

        protected void BlogGetir(string gelenID)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            SqlCommand com = new SqlCommand(@"
                SELECT ID, Baslik, tarih, KisaAciklama, Resim, aciklama
                FROM Blog WHERE aktif=@aktif AND ID=@GelenID", b.Baglanti);
            com.Parameters.AddWithValue("@GelenID", gelenID);
            com.Parameters.AddWithValue("@aktif", "Evet");

            if (com.Connection.State == ConnectionState.Closed)
                b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();
            if (dr.Read())
            {
                string baslik = EncodingHelper.DuzeltTurkce(dr["baslik"].ToString());
                ltrBlogBaslik.Text = baslik;
                ltrKisaAciklama.Text = EncodingHelper.DuzeltTurkce(dr["KisaAciklama"]?.ToString() ?? "");
                ltrTarih.Text = dr["tarih"]?.ToString() ?? "";
                ltrAciklama.Text = dr["aciklama"]?.ToString() ?? "";
                ltrResim.Text = "<img src='/Upload/" + dr["Resim"] + "' alt='" + baslik.Replace("'", "&#39;") + "' class='img-fluid' />";
                ltrYorumAdeti.Text = "0";
            }
            dr.Close();
            b.Baglanti.Close();
        }

        protected void BlogListesiGetir(string gelenID)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            SqlCommand com = new SqlCommand(@"
                SELECT TOP 3 ID, Baslik, tarih, KisaAciklama, Resim
                FROM Blog WHERE aktif=@aktif AND ID<>@GelenID
                ORDER BY siralama", b.Baglanti);
            com.Parameters.AddWithValue("@GelenID", gelenID);
            com.Parameters.AddWithValue("@aktif", "Evet");

            if (com.Connection.State == ConnectionState.Closed) b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                rptBlogGetir.DataSource = dt;
                rptBlogGetir.DataBind();
            }
            else
            {
                dr.Close();
            }
            b.Baglanti.Close();
        }
    }
}
