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
    public partial class Iletisim : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSabitlerini();
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
                this.Title = dr["sitebaslik"].ToString()+" "+"İletisişim";
            }

            dr.Close();
            b.Baglanti.Close();

        }

        protected void btnMesajGonder_Click(object sender, EventArgs e)
        {
            if (txtAdi.Text != "" && txtEmail.Text != "" && txtMesaj.Text != "")
            {
                BaglantiBilgileri b = new BaglantiBilgileri();
                SqlCommand com = new SqlCommand("insert into gelenler (tarihsaat, Isim, IPadresi, telefon, MailAdresi, Mesaj, okundu) values (@tarihsaat,@Isim, @IPadresi, @telefon, @MailAdresi,  @Mesaj, @okundu)", b.Baglanti);

                com.Parameters.AddWithValue("@tarihsaat", DateTime.Now);
                com.Parameters.AddWithValue("@IPadresi", Request.UserHostAddress ?? "");

                com.Parameters.AddWithValue("@MailAdresi", txtEmail.Text);
                com.Parameters.AddWithValue("@Isim", txtAdi.Text);
                com.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                com.Parameters.AddWithValue("@Mesaj", txtMesaj.Text);
                com.Parameters.AddWithValue("@okundu", "Hayır");


                if (com.Connection.State == ConnectionState.Closed)
                {
                    com.Connection.Open();
                }

                if (com.ExecuteNonQuery() > 0)
                {
                    UyariGoster("Mesaj Başarılı Şekilde Gönderildi.");
                }
                com.Connection.Close();

            }
            else
            {
                UyariGoster("Uyarı Mesaj Gönderilemedi. Lütfen Alanları Tamanını Doldurun.");
            }
        }
        private void UyariGoster(string mesaj)
        {
            string guvenliMesaj = mesaj.Replace("'", "\\'");
            Response.Write("<script type='text/javascript'>alert('" + guvenliMesaj + "');</script>");
        }
    }
}
