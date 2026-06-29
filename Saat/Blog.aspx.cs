using Saat.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{
    public partial class Blog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSabitlerini();
            BlogGetir();    
        }

        protected void SiteSabitlerini()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select Resim,sitebaslik from sitesabitleri where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", "1");

            if (com.Connection.State == System.Data.ConnectionState.Closed)
            {

                b.Baglanti.Open();

            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                this.Title = dr["sitebaslik"].ToString() + " " + "Blog";
            }

            dr.Close();
            b.Baglanti.Close();

        }

        protected void BlogGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand(
                "SELECT  ID, Baslik, tarih, KisaAciklama, Resim, aciklama, GoogleAnahtar, GoogleAciklama FROM Blog WHERE aktif=@aktif  ORDER BY siralama",
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