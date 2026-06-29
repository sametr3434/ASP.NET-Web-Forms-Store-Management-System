using DevExpress.Web.ASPxClasses.Design;
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
    public partial class AdminTanim : System.Web.UI.Page
    {
        string resim;

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

                        listegetir();
                    }
                    else
                    {

                    }
                }
                catch (Exception)
                {

                }
            }
        }

        protected void lnkYeniKayit_Click(object sender, EventArgs e)
        {
            if (Panel1.Visible == false)
            {
                Panel1.Visible = true;
                lnkKaydet.Visible = true;
                lnkGuncelle.Visible = false;
            }
            else
            {
                Response.Redirect(Request.Url.AbsolutePath);
            }
        }

        protected void lnkKaydet_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("insert into admin (kullaniciadi, sifre, mailadresi) values (@kullaniciadi, @sifre, @mailadresi)", b.Baglanti);

            com.Parameters.AddWithValue("@kullaniciadi", txtKullaniciAdi.Text);
            com.Parameters.AddWithValue("@sifre", txtSifre.Text);
            com.Parameters.AddWithValue("@mailadresi", txtMailAdresi.Text);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Bilgiler Kaydedilmiştir!'); window.location = 'AdminTanim.aspx'</script>");
            }

            com.Connection.Close();
        }

        private string DbStr(object o) { return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString()); }

        protected void listegetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select ID, kullaniciadi, mailadresi from admin order by ID desc", b.Baglanti);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            var dt = new DataTable();
            dt.Load(com.ExecuteReader());
            com.Connection.Close();

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (row[col] != DBNull.Value && row[col] != null && col.DataType == typeof(string))
                        row[col] = EncodingHelper.DuzeltTurkce(row[col].ToString());
                }
            }
            rptListe.DataSource = dt;
            rptListe.DataBind();
        }

        protected void lnkDegistir_Click(object sender, EventArgs e)
        {
            LinkButton tiklanan = (LinkButton)sender;
            lblDegistirID.Text = tiklanan.CommandArgument.ToString();

            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select kullaniciadi, sifre, mailadresi from admin where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", lblDegistirID.Text);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                Panel1.Visible = true;
                lnkKaydet.Visible = false;
                lnkGuncelle.Visible = true;

                while (dr.Read())
                {
                    txtKullaniciAdi.Text = DbStr(dr["kullaniciadi"]);
                    txtSifre.Text = DbStr(dr["sifre"]);
                    txtMailAdresi.Text = DbStr(dr["mailadresi"]);
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

            SqlCommand com = new SqlCommand("update admin set kullaniciadi=@kullaniciadi, sifre=@sifre, mailadresi=@mailadresi where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("ID", lblDegistirID.Text);
            com.Parameters.AddWithValue("@kullaniciadi", txtKullaniciAdi.Text);
            com.Parameters.AddWithValue("@sifre", txtSifre.Text);
            com.Parameters.AddWithValue("@mailadresi", txtMailAdresi.Text);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Bilgiler Kaydedilmiştir!'); window.location = 'AdminTanim.aspx'</script>");
            }

            com.Connection.Close();
        }

        protected void lnkSil_Click(object sender, EventArgs e)
        {
            LinkButton tiklanan = (LinkButton)sender;
            lblSilinecekID.Text = tiklanan.CommandArgument.ToString();

            BaglantiBilgileri b2 = new BaglantiBilgileri();

            SqlCommand com2 = new SqlCommand("delete from admin where ID=@ID", b2.Baglanti);

            com2.Parameters.AddWithValue("@ID", lblSilinecekID.Text);

            if (com2.Connection.State == ConnectionState.Closed)
            {
                com2.Connection.Open();
            }

            if (com2.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Kayıt Silinmiştir!'); window.location = 'AdminTanim.aspx'</script>");
            }

            com2.Connection.Close();
        }
    }
}