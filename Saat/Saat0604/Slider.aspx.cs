

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
    public partial class Slider : System.Web.UI.Page
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
                    }
                    else
                    {

                    }

                    listegetir();
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

            SqlCommand com = new SqlCommand("insert into slider (yazi1, yazi2, yazi3, resim, aktif, linkvaryok, link, siralama) values (@yazi1, @yazi2, @yazi3, @resim, @aktif, @linkvaryok, @link, @siralama)", b.Baglanti);

            com.Parameters.AddWithValue("@yazi1", txtYazi1.Text);
            com.Parameters.AddWithValue("@yazi2", txtYazi2.Text);
            com.Parameters.AddWithValue("@yazi3", txtYazi3.Text);

            if (FileUpload1.HasFile)
            {
                //kırpma alanı hesaplama
                int x = Convert.ToInt32(X.Value.Split('.')[0]);//Gizli Input X'in value değerinin noktadan önceki kısmı
                int y = Convert.ToInt32(Y.Value.Split('.')[0]);//Gizli Input Y'in value değerinin noktadan önceki kısmı
                int x2 = Convert.ToInt32(X2.Value.Split('.')[0]);//Gizli Input X2'in value değerinin noktadan önceki kısmı
                int y2 = Convert.ToInt32(Y2.Value.Split('.')[0]);//Gizli Input Y2'in value değerinin noktadan önceki kısmı
                if (x2 < 1300) x2 = 1300;//min genişlik kontrolü
                if (y2 < 975) y2 = 975;//min yükseklik kontrolü
                x2 = x + x2;//jCrop x ve ona olan pixel uzaklığını veriyor x+x2 ise ikinci kırpma noktası
                y2 = y + y2;//jCrop y ve ona olan pixel uzaklığını veriyor y+y2 ise ikinci kırpma noktası
                //kırpma alanı hesaplama SON
                try
                {
                    //dosya yazma
                    Guid benzersiz1 = Guid.NewGuid();
                    ImageResizer.ImageJob i =
                        new ImageResizer.ImageJob(FileUpload1.FileBytes,//yüklenecek resim
                            "~/upload/" + benzersiz1 + ".<ext>", new ImageResizer.ResizeSettings( //resmin yazılacağı yer
                                "w=1300;h=975;crop=" + x + "," + y + "," + x2 + "," + y2 + ";format=jpg;mode=stretch;frame=1;page=1"));//oluşturma ayarları
                    i.CreateParentDirectory = false;
                    //Auto-create the uploads directory.
                    i.Build();
                    //Eğer yeni resim var ise orijinal resim ismini güncelle
                    resim = benzersiz1 + ".jpg";

                    try
                    {
                        if (eskiresim.Value == "ResimYok.png")
                        {

                        }
                        else
                        {
                            var file = Server.MapPath("~/Upload/" + eskiresim.Value);
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                        //ignore 
                    }
                } //dosya yazma SON
                catch
                {
                    //dosya oluşturulamazsa (resim değilse)
                    //ltrUyari.Text = "<span style='font-size:14px;' class='label label-danger'>Resim yüklemede Hata Sadece JPG, GIF, PNG kullanın!</span>";
                    Response.Write("<script lang='JavaScript'>alert('Lütfen JPG, GIF, PNG, JPEG Dosyalarını Kullanınız!'); </script>");

                } //dosya oluşturulamazsa (resim değilse) SON
            }
            else
            {
                resim = "ResimYok.png";
            }

            com.Parameters.AddWithValue("@resim", resim);
            com.Parameters.AddWithValue("@aktif", rbAktif.SelectedItem.Text);
            com.Parameters.AddWithValue("@linkvaryok", rbLinkVarYok.SelectedItem.Text);
            com.Parameters.AddWithValue("@link", txtLink.Text);

            try
            {
                int test = Convert.ToInt32(txtSiralama.Text);
                com.Parameters.AddWithValue("@siralama", txtSiralama.Text);
            }
            catch (Exception)
            {
                com.Parameters.AddWithValue("@siralama", 999);
            }

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Bilgiler Kaydedilmiştir!'); window.location = 'Slider.aspx'</script>");
            }

            com.Connection.Close();
        }

        private string DbStr(object o) { return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString()); }

        protected void listegetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select ID, yazi1, resim from slider order by ID desc", b.Baglanti);

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

            SqlCommand com = new SqlCommand("select yazi1, yazi2, yazi3, resim, aktif, linkvaryok, link, siralama from slider where ID=@ID", b.Baglanti);

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
                    txtYazi1.Text = DbStr(dr["yazi1"]);
                    txtYazi2.Text = DbStr(dr["yazi2"]);
                    txtYazi3.Text = DbStr(dr["yazi3"]);
                    eskiresim.Value = DbStr(dr["resim"]);
                    Image1.ImageUrl = "/upload/" + DbStr(dr["resim"]);

                    if (DbStr(dr["aktif"]) == "Evet")
                    {
                        rbAktif.SelectedIndex = 0;
                    }
                    else
                    {
                        rbAktif.SelectedIndex = 1;
                    }

                    if (DbStr(dr["linkvaryok"]) == "Evet")
                    {
                        rbLinkVarYok.SelectedIndex = 0;
                    }
                    else
                    {
                        rbLinkVarYok.SelectedIndex = 1;
                    }

                    txtLink.Text = DbStr(dr["link"]);
                    txtSiralama.Text = DbStr(dr["Siralama"]);

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

            SqlCommand com = new SqlCommand("update slider set yazi1=@yazi1, yazi2=@yazi2, yazi3=@yazi3, resim=@resim, aktif=@aktif, linkvaryok=@linkvaryok, link=@link, siralama=@siralama where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("ID", lblDegistirID.Text);
            com.Parameters.AddWithValue("@yazi1", txtYazi1.Text);
            com.Parameters.AddWithValue("@yazi2", txtYazi2.Text);
            com.Parameters.AddWithValue("@yazi3", txtYazi3.Text);

            if (FileUpload1.HasFile)
            {
                //kırpma alanı hesaplama
                int x = Convert.ToInt32(X.Value.Split('.')[0]);//Gizli Input X'in value değerinin noktadan önceki kısmı
                int y = Convert.ToInt32(Y.Value.Split('.')[0]);//Gizli Input Y'in value değerinin noktadan önceki kısmı
                int x2 = Convert.ToInt32(X2.Value.Split('.')[0]);//Gizli Input X2'in value değerinin noktadan önceki kısmı
                int y2 = Convert.ToInt32(Y2.Value.Split('.')[0]);//Gizli Input Y2'in value değerinin noktadan önceki kısmı
                if (x2 < 1300) x2 = 1300;//min genişlik kontrolü
                if (y2 < 975) y2 = 975;//min yükseklik kontrolü
                x2 = x + x2;//jCrop x ve ona olan pixel uzaklığını veriyor x+x2 ise ikinci kırpma noktası
                y2 = y + y2;//jCrop y ve ona olan pixel uzaklığını veriyor y+y2 ise ikinci kırpma noktası
                //kırpma alanı hesaplama SON
                try
                {
                    //dosya yazma
                    Guid benzersiz1 = Guid.NewGuid();
                    ImageResizer.ImageJob i =
                        new ImageResizer.ImageJob(FileUpload1.FileBytes,//yüklenecek resim
                            "~/upload/" + benzersiz1 + ".<ext>", new ImageResizer.ResizeSettings( //resmin yazılacağı yer
                                "w=1300;h=975;crop=" + x + "," + y + "," + x2 + "," + y2 + ";format=jpg;mode=stretch;frame=1;page=1"));//oluşturma ayarları
                    i.CreateParentDirectory = false;
                    //Auto-create the uploads directory.
                    i.Build();
                    //Eğer yeni resim var ise orijinal resim ismini güncelle
                    resim = benzersiz1 + ".jpg";

                    try
                    {
                        if (eskiresim.Value == "ResimYok.png")
                        {

                        }
                        else
                        {
                            var file = Server.MapPath("~/Upload/" + eskiresim.Value);
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                        //ignore
                    }
                } //dosya yazma SON
                catch
                {
                    //dosya oluşturulamazsa (resim değilse)
                    Response.Write("<script lang='JavaScript'>alert('Lütfen JPG, GIF, PNG, JPEG Dosyalarını Kullanınız!'); </script>");
                } //dosya oluşturulamazsa (resim değilse) SON
            }
            else
            {
                resim = eskiresim.Value;
            }

            com.Parameters.AddWithValue("@resim", resim);
            com.Parameters.AddWithValue("@aktif", rbAktif.SelectedItem.Text);
            com.Parameters.AddWithValue("@linkvaryok", rbLinkVarYok.SelectedItem.Text);
            com.Parameters.AddWithValue("@link", txtLink.Text);

            try
            {
                int test = Convert.ToInt32(txtSiralama.Text);
                com.Parameters.AddWithValue("@siralama", txtSiralama.Text);
            }
            catch (Exception)
            {
                com.Parameters.AddWithValue("@siralama", 999);
            }

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Bilgiler Kaydedilmiştir!'); window.location = 'Slider.aspx'</script>");
            }

            com.Connection.Close();
        }

        protected void lnkSil_Click(object sender, EventArgs e)
        {
            LinkButton tiklanan = (LinkButton)sender;
            lblSilinecekID.Text = tiklanan.CommandArgument.ToString();
            string resimadi = "";

            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select resim from slider where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", lblSilinecekID.Text);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    resimadi = DbStr(dr["resim"]);
                }
            }
            else
            {

            }

            dr.Close();
            com.Connection.Close();

            BaglantiBilgileri b2 = new BaglantiBilgileri();

            SqlCommand com2 = new SqlCommand("delete from slider where ID=@ID", b2.Baglanti);

            com2.Parameters.AddWithValue("@ID", lblSilinecekID.Text);

            if (com2.Connection.State == ConnectionState.Closed)
            {
                com2.Connection.Open();
            }

            if (com2.ExecuteNonQuery() > 0)
            {
                dosyasil(resimadi);
                Response.Write("<script lang='JavaScript'>alert('Kayıt Silinmiştir!'); window.location = 'Slider.aspx'</script>");
            }

            com2.Connection.Close();
        }

        protected void dosyasil(string dosyaadi)
        {
            try
            {
                if (dosyaadi == "ResimYok.png")
                {

                }
                else
                {
                    var file = Server.MapPath("~/Upload/" + dosyaadi);
                    File.Delete(file);
                }
            }
            catch
            {
                //ignore 
            }
        }
    }
}