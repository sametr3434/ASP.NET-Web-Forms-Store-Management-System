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
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSabitlerini();
            SliderGetir();
            VitrinGetir();
            EnCokTercihEdenler();
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
                this.Title = dr["sitebaslik"].ToString();
            }

            dr.Close();
            b.Baglanti.Close();

        }

        protected void SliderGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand(
                "SELECT  Yazi1,Yazi2,Yazi3,Resim,link FROM slider WHERE aktif=@aktif  ORDER BY siralama",
                b.Baglanti);

            com.Parameters.AddWithValue("@aktif", "Evet");

            if (com.Connection.State == System.Data.ConnectionState.Closed) b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                DataTable dt = new DataTable(); dt.Load(dr);
                rptSliderGetir.DataSource = dt; rptSliderGetir.DataBind();

            }
            else
            {
                dr.Close();
            }

            b.Baglanti.Close();
        }

        protected void VitrinGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand(
                "SELECT Top(6) ID,UrunAdi, Fiyat,AnaResim, (SELECT MarkaAdi FROM Marka WHERE ID = Urun.MarkaId) AS MarkaAdi FROM Urun WHERE Aktif = @aktif AND Vitrin = @Vitrin ORDER BY Siralama",
                b.Baglanti);

            com.Parameters.AddWithValue("@aktif", true);
            com.Parameters.AddWithValue("@Vitrin", true);

            if (com.Connection.State == System.Data.ConnectionState.Closed) b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                DataTable dt = new DataTable(); dt.Load(dr);
                rptVitrinGetir.DataSource = dt; rptVitrinGetir.DataBind();

            }
            else
            {
                dr.Close();
            }

            b.Baglanti.Close();
        }

        protected void EnCokTercihEdenler()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand(
                "SELECT  ID,UrunAdi,Fiyat, AnaResim, KisaAciklama, (SELECT MarkaAdi FROM Marka WHERE ID = Urun.MarkaId) AS MarkaAdi FROM Urun WHERE Aktif = @aktif  ORDER BY Siralama",
                b.Baglanti);

            com.Parameters.AddWithValue("@aktif", true);
 

            if (com.Connection.State == System.Data.ConnectionState.Closed) b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                DataTable dt = new DataTable(); dt.Load(dr);
                rptEnCokGoruntulenen.DataSource = dt; rptEnCokGoruntulenen.DataBind();

            }
            else
            {
                dr.Close();
            }

            b.Baglanti.Close();
        }

        protected void BlogGetir() 
        
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand(
                "select ID,Baslik,tarih,KisaAciklama,Resim from blog where aktif=@aktif order by siralama" , b.Baglanti);

            com.Parameters.AddWithValue("@aktif", "Evet");


            if (com.Connection.State == System.Data.ConnectionState.Closed) b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                DataTable dt = new DataTable(); dt.Load(dr);
                rptBloglarimiz.DataSource = dt;
                rptBloglarimiz.DataBind();

            }
            else
            {
                dr.Close();
            }

            b.Baglanti.Close();
        }

        protected void btnMailAdresiKayiEt_Click(object sender, EventArgs e)
        {
            if (txtKisiMailAdresi.Text != "")
            {
                BaglantiBilgileri b = new BaglantiBilgileri();
                SqlCommand com = new SqlCommand("insert into gelenler (tarihsaat, IPadresi,  mailadresi, okundu) values (@tarihsaat, @IPadresi,   @mailadresi,  @okundu)", b.Baglanti);

                com.Parameters.AddWithValue("@tarihsaat", DateTime.Now);
                com.Parameters.AddWithValue("@IPadresi", Request.UserHostAddress ?? "");

                com.Parameters.AddWithValue("@mailadresi", txtKisiMailAdresi.Text.Trim());
                com.Parameters.AddWithValue("@okundu", "Hayır");


                if (com.Connection.State == ConnectionState.Closed)
                {
                    com.Connection.Open();
                }

                if (com.ExecuteNonQuery() > 0)
                {
                    UyariGoster("Mail Adersi Başarılı Şekilde Gönderildi.");
                }
                com.Connection.Close();

            }
            else
            {
                UyariGoster("Mail Adresi Gönderilemedi. Lütfen Mail Adresi Kısmını Alanı Doldurun.");
            }
        }
        private void UyariGoster(string mesaj)
        {
            string guvenliMesaj = mesaj.Replace("'", "\\'");
            Response.Write("<script type='text/javascript'>alert('" + guvenliMesaj + "');</script>");
        }


    }
    }
