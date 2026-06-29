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
    public partial class KategoriEkle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Önce dropdown'u doldur
                UstKategorileriDoldur();

                if (Request.QueryString["id"] != null)
                {
                    int kategoriId = Convert.ToInt32(Request.QueryString["id"]);
                    if (lnkYeni != null) lnkYeni.Visible = false;
                    KategoriBilgileriniGetir(kategoriId);
                    UrunleriListele(kategoriId);
                    UrunSecenekleriniDoldur(kategoriId);
                }
                else
                {
                    if (lnkYeni != null) lnkYeni.Visible = true;
                    pnlBilgi.Visible = true;
                    pnlForm.Visible = false;
                    // Yeni kategori eklerken ürün listesi gösterme
                    pnlUrunler.Visible = false;
                    pnlUrunYok.Visible = false;
                }

                IstatistikleriYukle();
                KategoriAgaciniOlustur();
            }
        }

        protected void lnkYeni_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            pnlBilgi.Visible = false;
            lnkKaydet.Visible = true;
            lnkGuncelle.Visible = false;
            TemizleForm();
        }

        protected void lnkKaydet_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("INSERT INTO Kategori (KategoriAdi, Aciklama, Siralama, UstId, Aktif, Resim) VALUES (@KategoriAdi, @Aciklama, @Siralama, @UstId, @Aktif, @Resim)", b.Baglanti);

            com.Parameters.AddWithValue("@KategoriAdi", txtKategoriAdi.Text);
            com.Parameters.AddWithValue("@Aciklama", txtAciklama.Text);

            int siralama = 0;
            if (!string.IsNullOrEmpty(txtSiralama.Text) && int.TryParse(txtSiralama.Text, out siralama))
            {
                com.Parameters.AddWithValue("@Siralama", siralama);
            }
            else
            {
                com.Parameters.AddWithValue("@Siralama", (object)DBNull.Value);
            }

            if (ddlUstKategori.SelectedValue != "")
            {
                com.Parameters.AddWithValue("@UstId", Convert.ToInt32(ddlUstKategori.SelectedValue));
            }
            else
            {
                com.Parameters.AddWithValue("@UstId", (object)DBNull.Value);
            }

            com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

            // Resim yükleme
            string resimAdi = "";
            if (fuResim.HasFile)
            {
                string dosyaUzantisi = System.IO.Path.GetExtension(fuResim.FileName).ToLower();
                if (dosyaUzantisi == ".jpg" || dosyaUzantisi == ".jpeg" || dosyaUzantisi == ".png")
                {
                    resimAdi = DateTime.Now.ToString("yyyyMMddHHmmss") + dosyaUzantisi;
                    fuResim.SaveAs(Server.MapPath("~/Upload/") + resimAdi);
                }
            }
            com.Parameters.AddWithValue("@Resim", resimAdi);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Kategori başarıyla kaydedilmiştir!'); window.location = 'KategoriEkle.aspx'</script>");
            }

            com.Connection.Close();
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("UPDATE Kategori SET KategoriAdi=@KategoriAdi, Aciklama=@Aciklama, Siralama=@Siralama, UstId=@UstId, Aktif=@Aktif WHERE ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", lblKategoriID.Text);
            com.Parameters.AddWithValue("@KategoriAdi", txtKategoriAdi.Text);
            com.Parameters.AddWithValue("@Aciklama", txtAciklama.Text);

            int siralama = 0;
            if (!string.IsNullOrEmpty(txtSiralama.Text) && int.TryParse(txtSiralama.Text, out siralama))
            {
                com.Parameters.AddWithValue("@Siralama", siralama);
            }
            else
            {
                com.Parameters.AddWithValue("@Siralama", (object)DBNull.Value);
            }

            if (ddlUstKategori.SelectedValue != "")
            {
                com.Parameters.AddWithValue("@UstId", Convert.ToInt32(ddlUstKategori.SelectedValue));
            }
            else
            {
                com.Parameters.AddWithValue("@UstId", (object)DBNull.Value);
            }

            com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

            // Resim güncelleme (yeni resim yüklendiyse)
            if (fuResim.HasFile)
            {
                string dosyaUzantisi = System.IO.Path.GetExtension(fuResim.FileName).ToLower();
                if (dosyaUzantisi == ".jpg" || dosyaUzantisi == ".jpeg" || dosyaUzantisi == ".png")
                {
                    string resimAdi = DateTime.Now.ToString("yyyyMMddHHmmss") + dosyaUzantisi;
                    fuResim.SaveAs(Server.MapPath("~/Upload/") + resimAdi);
                    com.Parameters.AddWithValue("@Resim", resimAdi);
                }
                else
                {
                    com.Parameters.AddWithValue("@Resim", (object)DBNull.Value);
                }
            }

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Kategori başarıyla güncellenmiştir!'); window.location = 'KategoriListe.aspx'</script>");
            }

            com.Connection.Close();
        }

        protected void lnkIptal_Click(object sender, EventArgs e)
        {
            Response.Redirect("KategoriEkle.aspx");
        }

        protected void lbDuzenle_Click(object sender, EventArgs e)
        {
            LinkButton tiklanan = (LinkButton)sender;
            int kategoriId = Convert.ToInt32(tiklanan.CommandArgument);
            Response.Redirect("KategoriEkle.aspx?id=" + kategoriId);
        }

        private void KategoriBilgileriniGetir(int kategoriId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("SELECT * FROM Kategori WHERE ID=@ID", b.Baglanti);
            com.Parameters.AddWithValue("@ID", kategoriId);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            SqlDataReader dr = com.ExecuteReader();
            if (dr.Read())
            {
                lblKategoriID.Text = kategoriId.ToString();
                txtKategoriAdi.Text = Duzelt(dr["KategoriAdi"]);
                txtAciklama.Text = Duzelt(dr["Aciklama"]);
                txtSiralama.Text = dr["Siralama"] != DBNull.Value ? dr["Siralama"].ToString() : "";
                int ustIdDeger = 0;
                if (dr["UstId"] != DBNull.Value && int.TryParse(dr["UstId"].ToString(), out ustIdDeger) && ustIdDeger > 0)
                {
                    ddlUstKategori.SelectedValue = ustIdDeger.ToString();
                }
                else
                {
                    ddlUstKategori.SelectedValue = "";
                }
                // Eğer üst kategori dropdown'da yoksa, boş seç
                if (ddlUstKategori.Items.FindByValue(ddlUstKategori.SelectedValue) == null)
                {
                    ddlUstKategori.SelectedIndex = 0;
                }
                chkAktif.Checked = Convert.ToBoolean(dr["Aktif"]);

                pnlForm.Visible = true;
                pnlBilgi.Visible = false;
                lnkKaydet.Visible = false;
                lnkGuncelle.Visible = true;
            }

            com.Connection.Close();
        }

        private void UstKategorileriDoldur()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("SELECT ID, KategoriAdi FROM Kategori WHERE UstId IS NULL OR UstId = 0 ORDER BY KategoriAdi", b.Baglanti);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            DataTable dt = new DataTable();
            dt.Load(com.ExecuteReader());
            com.Connection.Close();

            ddlUstKategori.DataSource = dt;
            ddlUstKategori.DataTextField = "KategoriAdi";
            ddlUstKategori.DataValueField = "ID";
            ddlUstKategori.DataBind();

            // Ana Kategori seçeneği mevcut değilse ekle
            if (ddlUstKategori.Items.FindByValue("") == null)
            {
                ddlUstKategori.Items.Insert(0, new ListItem("-- Ana Kategori --", ""));
            }

            // Türkçe karakter düzeltmesi (kural 13)
            for (int i = 0; i < ddlUstKategori.Items.Count; i++)
            {
                ddlUstKategori.Items[i].Text = EncodingHelper.DuzeltTurkce(ddlUstKategori.Items[i].Text);
            }
        }

        private void KategorileriListele()
        {
            // Bu metod artık kullanılmıyor, ürünler listeleniyor
        }

        private void UrunleriListele(int kategoriId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand(@"
                SELECT u.ID, u.UrunAdi, u.UrunKodu, u.Fiyat, u.StokAdedi, u.Aktif, m.MarkaAdi
                FROM Urun u
                LEFT JOIN Marka m ON u.MarkaId = m.ID
                INNER JOIN UrunKategori uk ON uk.UrunId = u.ID
                WHERE uk.KategoriId = @KategoriId
                ORDER BY u.UrunAdi ASC", b.Baglanti);

            com.Parameters.AddWithValue("@KategoriId", kategoriId);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            DataTable dt = new DataTable();
            dt.Load(com.ExecuteReader());
            com.Connection.Close();

            if (dt.Rows.Count > 0)
            {
                pnlUrunler.Visible = true;
                pnlUrunYok.Visible = false;
                rptUrunler.DataSource = dt;
                rptUrunler.DataBind();
            }
            else
            {
                pnlUrunler.Visible = false;
                pnlUrunYok.Visible = true;
            }
        }

        private void UrunSecenekleriniDoldur(int kategoriId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"
                SELECT u.ID, u.UrunAdi
                FROM Urun u
                WHERE u.Aktif = 1
                  AND NOT EXISTS (SELECT 1 FROM UrunKategori uk WHERE uk.UrunId = u.ID AND uk.KategoriId = @KategoriId)
                ORDER BY u.UrunAdi ASC", b.Baglanti))
            {
                com.Parameters.AddWithValue("@KategoriId", kategoriId);
                if (com.Connection.State == ConnectionState.Closed)
                    com.Connection.Open();
                DataTable dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();

                ddlUrunSec.DataSource = dt;
                ddlUrunSec.DataTextField = "UrunAdi";
                ddlUrunSec.DataValueField = "ID";
                ddlUrunSec.DataBind();
                ddlUrunSec.Items.Insert(0, new ListItem("-- Ürün seçiniz --", ""));

                // Türkçe karakter düzeltmesi (kural 13)
                for (int i = 0; i < ddlUrunSec.Items.Count; i++)
                {
                    ddlUrunSec.Items[i].Text = EncodingHelper.DuzeltTurkce(ddlUrunSec.Items[i].Text);
                }
            }
        }

        protected void lnkKategoriyeEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblKategoriID.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "uyari", "alert('Önce kategoriyi kaydediniz veya mevcut bir kategoriyi düzenleyiniz.');", true);
                return;
            }
            if (string.IsNullOrEmpty(ddlUrunSec.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "uyari2", "alert('Lütfen eklenecek ürünü seçiniz.');", true);
                return;
            }

            int kategoriId = Convert.ToInt32(lblKategoriID.Text);
            int urunId = Convert.ToInt32(ddlUrunSec.SelectedValue);

            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("INSERT INTO UrunKategori (UrunId, KategoriId) VALUES (@UrunId, @KategoriId)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@UrunId", urunId);
                com.Parameters.AddWithValue("@KategoriId", kategoriId);
                if (com.Connection.State == ConnectionState.Closed)
                    com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
            }

            // Listeyi ve dropdown'u güncelle
            UrunleriListele(kategoriId);
            UrunSecenekleriniDoldur(kategoriId);
            ScriptManager.RegisterStartupScript(this, GetType(), "basarili", "$('#urunEkleModal').modal('hide');", true);
        }

        private void IstatistikleriYukle()
        {
            // İstatistikler kaldırıldı, bu metod boş
        }

        private void KategoriAgaciniOlustur()
        {
            // Kategori hiyerarşisi kaldırıldı, bu metod boş
        }

        private void TemizleForm()
        {
            txtKategoriAdi.Text = "";
            txtAciklama.Text = "";
            txtSiralama.Text = "";
            ddlUstKategori.SelectedIndex = 0;
            chkAktif.Checked = true;
            lblKategoriID.Text = "";
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void rptKategoriler_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                int kategoriId = Convert.ToInt32(e.CommandArgument);
                BaglantiBilgileri b = new BaglantiBilgileri();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Kategori WHERE ID=@ID", b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@ID", kategoriId);
                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal(\"Başarılı!\", \"Kategori başarıyla silindi.\", \"success\");", true);
                KategorileriListele();
                IstatistikleriYukle();
                KategoriAgaciniOlustur();
            }
        }

        protected void rptUrunler_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Kaldir")
            {
                if (string.IsNullOrEmpty(lblKategoriID.Text)) return;
                int kategoriId = Convert.ToInt32(lblKategoriID.Text);
                int urunId = Convert.ToInt32(e.CommandArgument);

                BaglantiBilgileri b = new BaglantiBilgileri();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM UrunKategori WHERE UrunId=@UrunId AND KategoriId=@KategoriId", b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@UrunId", urunId);
                    cmd.Parameters.AddWithValue("@KategoriId", kategoriId);
                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

                UrunleriListele(kategoriId);
                UrunSecenekleriniDoldur(kategoriId);
            }
        }
    }
}
