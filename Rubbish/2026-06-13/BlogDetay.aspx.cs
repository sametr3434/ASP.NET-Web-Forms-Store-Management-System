using Saat.App_Code;
using Saat.Saat0604;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{
    public partial class BlogDetay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string gelenID = null;
            if (Page.RouteData.Values["ID"] != null)
            {
                gelenID = Page.RouteData.Values["ID"].ToString();
            }

            if (string.IsNullOrEmpty(gelenID))
            {
                gelenID = Request.QueryString["ID"];
            }

            if (string.IsNullOrEmpty(gelenID))
            {
                Response.Redirect("/blog");
            }

            SiteSabitlerini(gelenID);
            BlogGetir(gelenID);
            BlogListesiGetir();
        }

        protected void SiteSabitlerini(string gelenID)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select Resim,sitebaslik,(select baslik from blog where ID=@GelenID) as blogadi from sitesabitleri where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", "1");
            com.Parameters.AddWithValue("@GelenID", gelenID);

            if (com.Connection.State == System.Data.ConnectionState.Closed)
            {

                b.Baglanti.Open();

            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                this.Title = dr["sitebaslik"].ToString() + " " + "Blog" +" "+ dr["blogadi"];
            }

            dr.Close();
            b.Baglanti.Close();
  }
        protected void BlogGetir(string gelenID)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("SELECT  ID, Baslik, tarih, KisaAciklama, Resim, aciklama, GoogleAnahtar, GoogleAciklama FROM Blog WHERE aktif=@aktif and ID=@GelenID  ORDER BY siralama", b.Baglanti);

            com.Parameters.AddWithValue("@GelenID", gelenID);
            com.Parameters.AddWithValue("@aktif", "Evet");

            if (com.Connection.State == System.Data.ConnectionState.Closed)
            {

                b.Baglanti.Open();

            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                ltrBlogBaslik.Text = dr["baslik"].ToString();
                ltrKisaAciklama.Text = dr["KisaAciklama"].ToString();
                ltrTarih.Text = dr["tarih"].ToString();
                ltrAciklama.Text = dr["aciklama"].ToString();
                ltrResim.Text = "<img src='/upload/" + dr["Resim"].ToString() + "' alt='" + dr["baslik"].ToString() + "' /></a>";
            }

            dr.Close();
            b.Baglanti.Close();

        }
      
        protected void BlogListesiGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand(
                "SELECT top 3 ID, Baslik, tarih, KisaAciklama, Resim, aciklama, GoogleAnahtar, GoogleAciklama FROM Blog WHERE aktif=@aktif  ORDER BY siralama",
                b.Baglanti);

            com.Parameters.AddWithValue("@aktif", "Evet");

            if (com.Connection.State == System.Data.ConnectionState.Closed) b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                DataTable dt = new DataTable(); dt.Load(dr);
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