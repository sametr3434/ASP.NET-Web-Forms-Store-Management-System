
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
    public partial class Blog : System.Web.UI.Page
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

            SqlCommand com = new SqlCommand("insert into blog (baslik,tarih, resim, kisaaciklama, aciklama, googleanahtar, googleaciklama, aktif, siralama) values (@baslik,@tarih, @resim, @kisaaciklama, @aciklama, @googleanahtar, @googleaciklama, @aktif, @siralama)", b.Baglanti);

            com.Parameters.AddWithValue("@baslik", txtBaslik.Text);
            com.Parameters.AddWithValue("@tarih", txtTarih.Text);

            if (FileUpload1.HasFile)
            {
                //kırpma alanı hesaplama
                int x = Convert.ToInt32(X.Value.Split('.')[0]);//Gizli Input X'in value değerinin noktadan önceki kısmı
                int y = Convert.ToInt32(Y.Value.Split('.')[0]);//Gizli Input Y'in value değerinin noktadan önceki kısmı
                int x2 = Convert.ToInt32(X2.Value.Split('.')[0]);//Gizli Input X2'in value değerinin noktadan önceki kısmı
                int y2 = Convert.ToInt32(Y2.Value.Split('.')[0]);//Gizli Input Y2'in value değerinin noktadan önceki kısmı
                if (x2 < 1200) x2 = 1200;//min genişlik kontrolü
                if (y2 < 800) y2 = 800;//min yükseklik kontrolü
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
                                "w=1200;h=800;crop=" + x + "," + y + "," + x2 + "," + y2 + ";format=jpg;mode=stretch;frame=1;page=1"));//oluşturma ayarları
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
            com.Parameters.AddWithValue("@kisaaciklama", txtKisaAciklama.Text);
            com.Parameters.AddWithValue("@aciklama", FCKeditor1.Value);
            com.Parameters.AddWithValue("@googleanahtar", txtGoogleAnahtar.Text);
            com.Parameters.AddWithValue("@googleaciklama", txtGoogleAciklama.Text);
            com.Parameters.AddWithValue("@aktif", rbAktif.SelectedItem.Text);

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
                Response.Write("<script lang='JavaScript'>alert('Bilgiler Kaydedilmiştir!'); window.location = 'Blog.aspx'</script>");
            }

            com.Connection.Close();
        }

        protected void listegetir()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select ID, baslik, resim, aktif from blog order by ID desc", b.Baglanti);


            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();

            if (dr.HasRows)
            {
                rptListe.DataSource = dr;
                rptListe.DataBind();
            }
            else
            {

            }

            dr.Close();
            com.Connection.Close();
        }

        protected void lnkDegistir_Click(object sender, EventArgs e)
        {
            LinkButton tiklanan = (LinkButton)sender;
            lblDegistirID.Text = tiklanan.CommandArgument.ToString();

            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select baslik, resim, kisaaciklama, aciklama, googleanahtar, googleaciklama, aktif, siralama,tarih from blog where ID=@ID", b.Baglanti);

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
                    txtTarih.Text = dr["tarih"].ToString();
                    txtBaslik.Text = dr["baslik"].ToString();
                    eskiresim.Value = dr["resim"].ToString();
                    Image1.ImageUrl = "/upload/" + dr["resim"].ToString();
                    txtKisaAciklama.Text = dr["kisaaciklama"].ToString();
                    FCKeditor1.Value = dr["aciklama"].ToString();
                    txtGoogleAnahtar.Text = dr["googleanahtar"].ToString();
                    txtGoogleAciklama.Text = dr["googleaciklama"].ToString();

                    if (dr["aktif"].ToString() == "Evet")
                    {
                        rbAktif.SelectedIndex = 0;
                    }
                    else
                    {
                        rbAktif.SelectedIndex = 1;
                    }

                    txtSiralama.Text = dr["Siralama"].ToString();
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

            SqlCommand com = new SqlCommand("update blog set baslik=@baslik, resim=@resim, kisaaciklama=@kisaaciklama, aciklama=@aciklama, googleanahtar=@googleanahtar, googleaciklama=@googleaciklama, aktif=@aktif, siralama=@siralama,tarih=@tarih where ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("ID", lblDegistirID.Text);
            com.Parameters.AddWithValue("@baslik", txtBaslik.Text);
            com.Parameters.AddWithValue("@tarih", txtTarih.Text);
            if (FileUpload1.HasFile)
            {
                //kırpma alanı hesaplama
                int x = Convert.ToInt32(X.Value.Split('.')[0]);//Gizli Input X'in value değerinin noktadan önceki kısmı
                int y = Convert.ToInt32(Y.Value.Split('.')[0]);//Gizli Input Y'in value değerinin noktadan önceki kısmı
                int x2 = Convert.ToInt32(X2.Value.Split('.')[0]);//Gizli Input X2'in value değerinin noktadan önceki kısmı
                int y2 = Convert.ToInt32(Y2.Value.Split('.')[0]);//Gizli Input Y2'in value değerinin noktadan önceki kısmı
                if (x2 < 1200) x2 = 1200;//min genişlik kontrolü
                if (y2 < 800) y2 = 800;//min yükseklik kontrolü
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
                                "w=1200;h=800;crop=" + x + "," + y + "," + x2 + "," + y2 + ";format=jpg;mode=stretch;frame=1;page=1"));//oluşturma ayarları
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
            com.Parameters.AddWithValue("@kisaaciklama", txtKisaAciklama.Text);
            com.Parameters.AddWithValue("@aciklama", FCKeditor1.Value);
            com.Parameters.AddWithValue("@googleanahtar", txtGoogleAnahtar.Text);
            com.Parameters.AddWithValue("@googleaciklama", txtGoogleAciklama.Text);
            com.Parameters.AddWithValue("@aktif", rbAktif.SelectedItem.Text);

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
                Response.Write("<script lang='JavaScript'>alert('Bilgiler Kaydedilmiştir!'); window.location = 'Blog.aspx'</script>");
            }

            com.Connection.Close();
        }

        protected void lnkSil_Click(object sender, EventArgs e)
        {
            LinkButton tiklanan = (LinkButton)sender;
            lblSilinecekID.Text = tiklanan.CommandArgument.ToString();
            string resimadi = "";

            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("select resim from blog where ID=@ID", b.Baglanti);

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
                    resimadi = dr["resim"].ToString();
                }
            }
            else
            {

            }

            dr.Close();
            com.Connection.Close();

            BaglantiBilgileri b2 = new BaglantiBilgileri();

            SqlCommand com2 = new SqlCommand("delete from blog where ID=@ID", b2.Baglanti);

            com2.Parameters.AddWithValue("@ID", lblSilinecekID.Text);

            if (com2.Connection.State == ConnectionState.Closed)
            {
                com2.Connection.Open();
            }

            if (com2.ExecuteNonQuery() > 0)
            {
                dosyasil(resimadi);
                Response.Write("<script lang='JavaScript'>alert('Kayıt Silinmiştir!'); window.location = 'Blog.aspx'</script>");
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