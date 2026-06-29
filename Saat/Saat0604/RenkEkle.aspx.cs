using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class RenkEkle : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    RenkBilgileriniGetir(id);
                }
            }
        }

        protected void lnkKaydet_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("INSERT INTO Renk (RenkAdi, RenkKodu, Siralama, Aktif) VALUES (@RenkAdi, @RenkKodu, @Siralama, @Aktif)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@RenkAdi", txtRenkAdi.Text);
                com.Parameters.AddWithValue("@RenkKodu", txtRenkKodu.Text);
                int siralama;
                if (!string.IsNullOrEmpty(txtSiralama.Text) && int.TryParse(txtSiralama.Text, out siralama))
                    com.Parameters.AddWithValue("@Siralama", siralama);
                else
                    com.Parameters.AddWithValue("@Siralama", (object)DBNull.Value);
                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Response.Write("<script lang='JavaScript'>alert('Renk başarıyla kaydedilmiştir!'); window.location = 'RenkListe.aspx'</script>");
                }
                com.Connection.Close();
            }
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("UPDATE Renk SET RenkAdi=@RenkAdi, RenkKodu=@RenkKodu, Siralama=@Siralama, Aktif=@Aktif WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", lblRenkID.Text);
                com.Parameters.AddWithValue("@RenkAdi", txtRenkAdi.Text);
                com.Parameters.AddWithValue("@RenkKodu", txtRenkKodu.Text);
                int siralama;
                if (!string.IsNullOrEmpty(txtSiralama.Text) && int.TryParse(txtSiralama.Text, out siralama))
                    com.Parameters.AddWithValue("@Siralama", siralama);
                else
                    com.Parameters.AddWithValue("@Siralama", (object)DBNull.Value);
                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Response.Write("<script lang='JavaScript'>alert('Renk başarıyla güncellenmiştir!'); window.location = 'RenkListe.aspx'</script>");
                }
                com.Connection.Close();
            }
        }

        protected void lnkIptal_Click(object sender, EventArgs e)
        {
            Response.Redirect("RenkListe.aspx");
        }

        private void RenkBilgileriniGetir(int id)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT * FROM Renk WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    lblRenkID.Text = id.ToString();
                    txtRenkAdi.Text = Duzelt(dr["RenkAdi"]);
                    txtRenkKodu.Text = Duzelt(dr["RenkKodu"]);
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
