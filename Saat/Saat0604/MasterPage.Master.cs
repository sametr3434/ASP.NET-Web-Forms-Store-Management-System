
using Saat.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class MasterPage : System.Web.UI.MasterPage
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
                        ltrKullaniciAdi.Text = lblKullaniciAdi.Text;
                    }
                    else
                    {
                        Response.Redirect("/Saat0604");
                    }
                }
                catch (Exception)
                {
                    Response.Redirect("/Saat0604");
                }
            }

            sabitlerigetir();
            okunmamismesajsayisi();
        }

        private string DbStr(object o) { return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString()); }

        protected void sabitlerigetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select Resim, SiteBaslik from SiteSabitleri where ID='1'", b.Baglanti);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string resim = DbStr(dr["Resim"]);
                    string baslik = DbStr(dr["SiteBaslik"]);
                    string baslikAlt = DbStr(dr["sitebaslik"]);
                    ltrUstResim1.Text = "<img src='/Upload/" + resim + "' alt='" + baslik.Replace("'", "&#39;") + "' style='height:2.0rem !important;' />";
                    ltrUstResim2.Text = "<img src='/Upload/" + resim + "' alt='" + baslik.Replace("'", "&#39;") + "' style='height:2.0rem !important;' />";
                    ltrUstResim3.Text = "<img src='/Upload/" + resim + "' alt='" + baslik.Replace("'", "&#39;") + "' style='height:2.0rem !important;' />";
                    ltrFooterBaslik.Text = baslikAlt;
                }
            }
            else
            {

            }

            dr.Close();
            com.Connection.Close();
        }

        protected void lnkCikis_Click(object sender, EventArgs e)
        {
            Session["KullaniciAdi"] = null;
            Session.Abandon();

            if (Request.Cookies["saatpanelcerez"] != null)
            {
                Response.Cookies["saatpanelcerez"].Expires = DateTime.Now.AddDays(-1);
            }

            Response.Redirect("/Saat0604/");
        }

        protected void okunmamismesajsayisi()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select count(*) as Sayi from Gelenler where Okundu=@Okundu", b.Baglanti);

            com.Parameters.AddWithValue("@Okundu", "Hayır");

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    try
                    {
                        int sayi = Convert.ToInt32(dr["Sayi"].ToString());
                        ltrOkunmamisMesajSayisi.Text = dr["Sayi"].ToString();
                        ltrOkunmamisMesajSayisi2.Text = dr["Sayi"].ToString();
                    }
                    catch (Exception)
                    {
                        ltrOkunmamisMesajSayisi.Text = "0";
                        ltrOkunmamisMesajSayisi2.Text = "0";
                    }
                }
            }
            else
            {
                ltrOkunmamisMesajSayisi.Text = "0";
                ltrOkunmamisMesajSayisi2.Text = "0";
            }

            dr.Close();
            com.Connection.Close();
        }
    }
}