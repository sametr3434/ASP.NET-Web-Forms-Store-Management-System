using Saat.App_Code;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat
{

    public partial class Hakkimizda : System.Web.UI.Page
    {
        public string Rakam1 = "0";
        public string Rakam2 = "0";
        public string Rakam3 = "0";
        public string Rakam4 = "0";
        public string resim = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSabitlerini();
            HakkimizdaGetir();
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
                this.Title = dr["sitebaslik"].ToString();
            }

            dr.Close();
            b.Baglanti.Close();

        }

        protected void HakkimizdaGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select Baslik, Resim, Icerik, Rakam1,RakamBaslik1,Rakam2,RakamBaslik2,Rakam3,RakamBaslik3,Rakam4,RakamBaslik4 from hakkimizda where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", "1");

            if (com.Connection.State == System.Data.ConnectionState.Closed)
            {

                b.Baglanti.Open();

            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                ltrHakkimizdaAciklama.Text = dr["Icerik"].ToString();
                ltrHakkimizdaBaslik.Text = dr["Baslik"].ToString();
                Rakam1 = dr["Rakam1"].ToString();
                Rakam2 = dr["Rakam2"].ToString();
                Rakam3 = dr["Rakam3"].ToString();
                Rakam4 = dr["Rakam4"].ToString();
                ltrRakam1Yazisi.Text = dr["RakamBaslik1"].ToString();
                ltrRakam2Yazisi.Text = dr["RakamBaslik2"].ToString();
                ltrRakam3Yazisi.Text = dr["RakamBaslik3"].ToString();
                ltrRakam4Yazisi.Text = dr["RakamBaslik4"].ToString();
                resim = dr["resim"].ToString();
            }

            dr.Close();
            b.Baglanti.Close();

        }
    }
}