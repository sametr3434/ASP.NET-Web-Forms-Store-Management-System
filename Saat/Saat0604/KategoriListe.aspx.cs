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
    public partial class KategoriListe : System.Web.UI.Page
    {
        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindKategoriler();
            }
        }

        protected void lnkYeniKayit_Click(object sender, EventArgs e)
        {
            if (PanelForm.Visible == false)
            {
                PanelForm.Visible = true;
                lnkKaydet.Visible = true;
                lnkGuncelle.Visible = false;
                TemizleForm();
            }
            else
            {
                PanelForm.Visible = false;
                TemizleForm();
            }
        }

        protected void lnkKaydet_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("INSERT INTO Kategori (KategoriAdi, Aciklama, Siralama, Aktif) VALUES (@KategoriAdi, @Aciklama, @Siralama, @Aktif)", b.Baglanti);

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

            com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

            if (com.Connection.State == ConnectionState.Closed)
            {
                com.Connection.Open();
            }

            if (com.ExecuteNonQuery() > 0)
            {
                Response.Write("<script lang='JavaScript'>alert('Kategori başarıyla kaydedilmiştir!'); window.location = 'KategoriListe.aspx'</script>");
            }

            com.Connection.Close();
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("UPDATE Kategori SET KategoriAdi=@KategoriAdi, Aciklama=@Aciklama, Siralama=@Siralama, Aktif=@Aktif WHERE ID=@ID", b.Baglanti);

            com.Parameters.AddWithValue("@ID", lblDegistirID.Text);
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

            com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

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

        private void BindKategoriler()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();

            SqlCommand com = new SqlCommand("SELECT ID, KategoriAdi, Aciklama, Siralama, Aktif FROM Kategori ORDER BY Siralama ASC, KategoriAdi ASC", b.Baglanti);

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
            rptKategoriler.DataSource = dt;
            rptKategoriler.DataBind();
        }

        private void TemizleForm()
        {
            txtKategoriAdi.Text = "";
            txtAciklama.Text = "";
            txtSiralama.Text = "";
            chkAktif.Checked = true;
            lblDegistirID.Text = "";
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
                // Başarı mesajı göster
                ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal(\"Başarılı!\", \"Kategori başarıyla silindi.\", \"success\");", true);
                BindKategoriler(); // Listeyi güncelle
            }
        }

        protected void lbSil_Click(object sender, EventArgs e)
        {
            LinkButton tiklanan = (LinkButton)sender;
            int kategoriId = Convert.ToInt32(tiklanan.CommandArgument);

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
            // Başarı mesajı göster
            ScriptManager.RegisterStartupScript(this, GetType(), "sweetalert", "swal(\"Başarılı!\", \"Kategori başarıyla silindi.\", \"success\");", true);
            BindKategoriler(); // Listeyi güncelle
        }
    }
}
