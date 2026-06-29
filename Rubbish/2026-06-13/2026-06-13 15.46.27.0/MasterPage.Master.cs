using FiftyOne;
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
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MagazaYardimcisi.OturumIdAl(Context);
            SiteSabitlerini();
            KuponGetir();
            IletisimGetir();
            UrunListele();
            SepetOzetGoster();
        }

        protected void SepetOzetGoster()
        {
            int adet = SepetHelper.ToplamAdet(Context);
            ltrMiniSepet.Text = string.Format(
                "<a href=\"/sepet\" class=\"bordered-icon\" title=\"Sepet\"><span class=\"mini-cart__count\">{0}</span><i class=\"icon_cart_alt\"></i></a>",
                adet);
        }

        protected void SiteSabitlerini() 
        {
        BaglantiBilgileri b = new BaglantiBilgileri ();

        SqlCommand com = new SqlCommand ("select Resim,sitebaslik from sitesabitleri where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID","1");

            if (com.Connection.State == System.Data.ConnectionState.Closed) 
            {

            b.Baglanti.Open ();

            }

            SqlDataReader dr=com.ExecuteReader();

            if (dr.Read()) 
            {
                ltrLogo.Text = "<a href='/anasayfa'><img src='/upload/" + dr["Resim"].ToString() + "' alt='" + dr["sitebaslik"].ToString() + "' width='1000' height='150' /></a>";
            }

            dr.Close ();
            b.Baglanti.Close ();

        }

        protected void KuponGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select KuponKodu from Kupon where  Aktif=@Aktif", b.Baglanti);

            com.Parameters.AddWithValue("@Aktif", true);

            if (com.Connection.State == System.Data.ConnectionState.Closed)
            {
                b.Baglanti.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                ltrKuponadi.Text = dr["KuponKodu"].ToString();
            }

            dr.Close ();
            b.Baglanti.Close();


        }

        protected void IletisimGetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select telefon1,twitter,facebook,linkedin,instagram,adres,mailadresi, telefon1,telefon2 from iletisim where  ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", 1);

            if (com.Connection.State == System.Data.ConnectionState.Closed)
            {
                b.Baglanti.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                ltrTelefon1.Text = "<a href='tel:" + dr["telefon1"] + "'>" + dr["telefon1"] + "</a>";
                ltrTwitter.Text = "<a href='" + dr["twitter"].ToString() + "' target='_blank'><i class='fa fa-twitter'></i></a>";
                ltrFacebook.Text = "<a href='" + dr["facebook"].ToString() + "' target='_blank'><i class='fa fa-facebook'></i></a>"; ;
                ltrLinkedin.Text = "<a href='" + dr["linkedin"].ToString() + "' target='_blank'><i class='fa fa-linkedin'></i></a>"; ;
                ltrInstagram.Text = "<a href='" + dr["instagram"].ToString() + "' target='_blank'><i class='fa fa-instagram'></i></a>"; ;
                ltrTwitter2.Text = "<a href='" + dr["twitter"].ToString() + "' target='_blank'><i class='fa fa-twitter'></i></a>";
                ltrFacebook2.Text = "<a href='" + dr["facebook"].ToString() + "' target='_blank'><i class='fa fa-facebook'></i></a>"; ;
                ltrLinkedin2.Text = "<a href='" + dr["linkedin"].ToString() + "' target='_blank'><i class='fa fa-linkedin'></i></a>"; ;
                ltrInstagram2.Text = "<a href='" + dr["instagram"].ToString() + "' target='_blank'><i class='fa fa-instagram'></i></a>"; ;
                ltrAdres.Text = dr["adres"].ToString(); 
                ltrMailAdresi.Text = dr["mailadresi"].ToString(); 
                ltrTelefon11.Text = dr["telefon1"].ToString();
                ltrTelefon22.Text = dr["telefon2"].ToString();
            }

            dr.Close();
            b.Baglanti.Close();


        }

        protected void UrunListele()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand(
                "SELECT TOP(5) ID, UrunAdi FROM urun WHERE aktif=@aktif  ORDER BY siralama",
                b.Baglanti);

            com.Parameters.AddWithValue("@aktif", true);

            if (com.Connection.State == System.Data.ConnectionState.Closed) b.Baglanti.Open();

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                DataTable dt = new DataTable(); dt.Load(dr);
                rptUrunListele.DataSource = dt; rptUrunListele.DataBind();

            }
            else 
            { 
                dr.Close();
            }

            b.Baglanti.Close();
        }

        public string UrunLink(object id, object ad)
        {
            if (id == null || ad == null) return "/urunler";
            return string.Format("/urundetay/{0}/{1}", id, UrunSeoSlug.Olustur(ad.ToString()));
        }
    }
}