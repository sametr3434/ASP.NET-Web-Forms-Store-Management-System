
using Saat.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class Iletisim : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    HttpCookie cerezOku = Request.Cookies["saatpanelcerez"];

                    if (cerezOku != null && cerezOku["saatpanel"] == "SaatPanel")
                    {
                        int KullaniciID = Convert.ToInt32(cerezOku["KullaniciID"].ToString());
                        lblKullaniciID.Text = KullaniciID.ToString();
                        lblKullaniciAdi.Text = cerezOku["KullaniciAdi"].ToString();
                    }
                    else
                    {

                    }

                    icerikgetir();
                }
                catch (Exception)
                {

                }
            }
        }

        private string DbStr(object o) { return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString()); }

        protected void icerikgetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            // iletisim.instagram: veritabaninda kucuk harf kolon adi (Instagram PascalCase kullanilmaz)
            SqlCommand com = new SqlCommand("select telefon1, telefon2, mailadresi, adres, facebook, twitter, instagram, linkedin, whatsapp from iletisim where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", "1");

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    txtTelefon1.Text = DbStr(dr["telefon1"]);
                    txtTelefon2.Text = DbStr(dr["telefon2"]);
                    txtMailAdresi.Text = DbStr(dr["mailadresi"]);
                    txtAdres.Text = DbStr(dr["adres"]);
                    txtFacebook.Text = DbStr(dr["facebook"]);
                    txtTwitter.Text = DbStr(dr["twitter"]);
                    txtInstagram.Text = DbStr(dr["instagram"]);
                    txtLinkedin.Text = DbStr(dr["linkedin"]);
                    txtWhatsapp.Text = DbStr(dr["whatsapp"]);
                }
            }
            else
            {

            }

            dr.Close();
            com.Connection.Close();
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            // instagram kolonu kucuk harf (yukaridaki select ile ayni)
            SqlCommand com = new SqlCommand("update iletisim set telefon1=@telefon1, telefon2=@telefon2, mailadresi=@mailadresi, adres=@adres, facebook=@facebook, twitter=@twitter, instagram=@instagram, linkedin=@linkedin, whatsapp=@whatsapp where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("ID", "1");
            com.Parameters.AddWithValue("@telefon1", txtTelefon1.Text);
            com.Parameters.AddWithValue("@telefon2", txtTelefon2.Text);
            com.Parameters.AddWithValue("@mailadresi", txtMailAdresi.Text);
            com.Parameters.AddWithValue("@adres", txtAdres.Text);
            com.Parameters.AddWithValue("@facebook", txtFacebook.Text);
            com.Parameters.AddWithValue("@twitter", txtTwitter.Text);
            com.Parameters.AddWithValue("@instagram", txtInstagram.Text);
            com.Parameters.AddWithValue("@whatsapp", txtWhatsapp.Text);
            com.Parameters.AddWithValue("@linkedin", txtLinkedin.Text);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Bilgiler Kaydedilmiştir!'); window.location = 'Iletisim.aspx'</script>");
            }

            com.Connection.Close();
        }
    }
}