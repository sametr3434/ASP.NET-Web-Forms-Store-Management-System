
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
    public partial class Hakkimizda : System.Web.UI.Page
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

            SqlCommand com = new SqlCommand("SELECT Baslik, KisaAciklama, Resim, Icerik, GoogleAnahtar, GoogleAciklama, RakamBaslik1, Rakam1, RakamBaslik2, Rakam2, RakamBaslik3, Rakam3, RakamBaslik4, Rakam4 FROM hakkimizda WHERE ID=@ID", b.Baglanti);

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
                    txtBaslik.Text = DbStr(dr["Baslik"]);
                    txtKisaAciklama.Text = DbStr(dr["KisaAciklama"]);
                    eskiresim.Value = DbStr(dr["Resim"]);
                    Image1.ImageUrl = "/upload/" + DbStr(dr["Resim"]);
                    fckIcerik.Value = DbStr(dr["Icerik"]);
                    txtGoogleAnahtar.Text = DbStr(dr["GoogleAnahtar"]);
                    txtGoogleAciklama.Text = DbStr(dr["GoogleAciklama"]);
                    txtRakamBaslik1.Text = DbStr(dr["RakamBaslik1"]);
                    txtRakam1.Text = DbStr(dr["Rakam1"]);
                    txtRakamBaslik2.Text = DbStr(dr["RakamBaslik2"]);
                    txtRakam2.Text = DbStr(dr["Rakam2"]);
                    txtRakamBaslik3.Text = DbStr(dr["RakamBaslik3"]);
                    txtRakam3.Text = DbStr(dr["Rakam3"]);
                    txtRakamBaslik4.Text = DbStr(dr["RakamBaslik4"]);
                    txtRakam4.Text = DbStr(dr["Rakam4"]);
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

            SqlCommand com = new SqlCommand("UPDATE hakkimizda SET Baslik=@baslik, KisaAciklama=@kisaaciklama, Resim=@resim, Icerik=@icerik, GoogleAnahtar=@googleanahtar, GoogleAciklama=@googleaciklama, RakamBaslik1=@rakambaslik1, Rakam1=@rakam1, RakamBaslik2=@rakambaslik2, Rakam2=@rakam2, RakamBaslik3=@rakambaslik3, Rakam3=@rakam3, RakamBaslik4=@rakambaslik4, Rakam4=@rakam4 WHERE ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", "1");
            com.Parameters.AddWithValue("@baslik", txtBaslik.Text);

            if (FileUpload1.HasFile)
            {
                //kırpma alanı hesaplama
                int x = Convert.ToInt32(X.Value.Split('.')[0]);//Gizli Input X'in value değerinin noktadan önceki kısmı
                int y = Convert.ToInt32(Y.Value.Split('.')[0]);//Gizli Input Y'in value değerinin noktadan önceki kısmı
                int x2 = Convert.ToInt32(X2.Value.Split('.')[0]);//Gizli Input X2'in value değerinin noktadan önceki kısmı
                int y2 = Convert.ToInt32(Y2.Value.Split('.')[0]);//Gizli Input Y2'in value değerinin noktadan önceki kısmı
                if (x2 < 650) x2 = 650;//min genişlik kontrolü
                if (y2 < 980) y2 = 980;//min yükseklik kontrolü
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
                                "w=650;h=980;crop=" + x + "," + y + "," + x2 + "," + y2 + ";format=jpg;mode=stretch;frame=1;page=1"));//oluşturma ayarları
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
            com.Parameters.AddWithValue("@icerik", fckIcerik.Value);
            com.Parameters.AddWithValue("@kisaaciklama", txtKisaAciklama.Text);
            com.Parameters.AddWithValue("@googleanahtar", txtGoogleAnahtar.Text);
            com.Parameters.AddWithValue("@googleaciklama", txtGoogleAciklama.Text);
            com.Parameters.AddWithValue("@rakambaslik1", txtRakamBaslik1.Text);
            com.Parameters.AddWithValue("@rakam1", txtRakam1.Text);
            com.Parameters.AddWithValue("@rakambaslik2", txtRakamBaslik2.Text);
            com.Parameters.AddWithValue("@rakam2", txtRakam2.Text);
            com.Parameters.AddWithValue("@rakambaslik3", txtRakamBaslik3.Text);
            com.Parameters.AddWithValue("@rakam3", txtRakam3.Text);
            com.Parameters.AddWithValue("@rakambaslik4", txtRakamBaslik4.Text);
            com.Parameters.AddWithValue("@rakam4", txtRakam4.Text);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Bilgiler Kaydedilmi\u015ftir!'); window.location = 'Hakkimizda.aspx'</script>");
            }

            com.Connection.Close();
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