using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class KasaBoyutuEkleme : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    KasaBoyutuBilgileriniGetir(id);
                }
            }
        }

        protected void lnkKaydet_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("INSERT INTO kasaboyutu (kasano, Siralama, Aktif) VALUES (@kasano, @Siralama, @Aktif)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@kasano", txtKasaNo.Text);
                int siralama;
                if (!string.IsNullOrEmpty(txtSiralama.Text) && int.TryParse(txtSiralama.Text, out siralama))
                    com.Parameters.AddWithValue("@Siralama", siralama);
                else
                    com.Parameters.AddWithValue("@Siralama", (object)DBNull.Value);
                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Response.Write("<script lang='JavaScript'>alert('Kasa Boyutu başarıyla kaydedilmiştir!'); window.location = 'KasaBoyutuListe.aspx'</script>");
                }
                com.Connection.Close();
            }
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("UPDATE kasaboyutu SET kasano=@kasano, Siralama=@Siralama, Aktif=@Aktif WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", lblKasaboyutuId.Text);
                com.Parameters.AddWithValue("@kasano", txtKasaNo.Text);
                int siralama;
                if (!string.IsNullOrEmpty(txtSiralama.Text) && int.TryParse(txtSiralama.Text, out siralama))
                    com.Parameters.AddWithValue("@Siralama", siralama);
                else
                    com.Parameters.AddWithValue("@Siralama", (object)DBNull.Value);
                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Response.Write("<script lang='JavaScript'>alert('Kasa Boyutu başarıyla güncellenmiştir!'); window.location = 'KasaBoyutuListe.aspx'</script>");
                }
                com.Connection.Close();
            }
        }

        protected void lnkIptal_Click(object sender, EventArgs e)
        {
            Response.Redirect("KasaBoyutuListe.aspx");
        }

        private void KasaBoyutuBilgileriniGetir(int id)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT * FROM kasaboyutu WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    lblKasaboyutuId.Text = id.ToString();
                    txtKasaNo.Text = Duzelt(dr["kasano"]);
                    txtSiralama.Text = dr["Siralama"] != DBNull.Value ? dr["Siralama"].ToString() : "";
                    chkAktif.Checked = Convert.ToBoolean(dr["Aktif"]);
                    lnkKaydet.Visible = false;
                    lnkGuncelle.Visible = true;
                }
                com.Connection.Close();
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }
    }
}
