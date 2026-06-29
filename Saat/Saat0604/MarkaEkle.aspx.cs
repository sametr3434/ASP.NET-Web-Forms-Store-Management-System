
using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class MarkaEkle : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    MarkaBilgileriniGetir(id);
                }
            }
        }

        protected void lnkKaydet_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("INSERT INTO Marka (MarkaAdi, Aciklama, Siralama, Aktif, Logo) VALUES (@MarkaAdi, @Aciklama, @Siralama, @Aktif, @Logo)", b.Baglanti))
            {
                com.Parameters.AddWithValue("@MarkaAdi", txtMarkaAdi.Text);
                com.Parameters.AddWithValue("@Aciklama", txtAciklama.Text);

                int siralama;
                if (!string.IsNullOrEmpty(txtSiralama.Text) && int.TryParse(txtSiralama.Text, out siralama))
                    com.Parameters.AddWithValue("@Siralama", siralama);
                else
                    com.Parameters.AddWithValue("@Siralama", (object)DBNull.Value);

                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

                string logoAdi = "";
                if (fuLogo.HasFile)
                {
                    string uzanti = System.IO.Path.GetExtension(fuLogo.FileName).ToLower();
                    if (uzanti == ".jpg" || uzanti == ".jpeg" || uzanti == ".png")
                    {
                        logoAdi = DateTime.Now.ToString("yyyyMMddHHmmss") + uzanti;
                        fuLogo.SaveAs(Server.MapPath("~/Upload/") + logoAdi);
                    }
                }
                com.Parameters.AddWithValue("@Logo", logoAdi);

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Response.Write("<script lang='JavaScript'>alert('Marka başarıyla kaydedilmiştir!'); window.location = 'MarkaListe.aspx'</script>");
                }
                com.Connection.Close();
            }
        }

        protected void lnkGuncelle_Click(object sender, EventArgs e)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("UPDATE Marka SET MarkaAdi=@MarkaAdi, Aciklama=@Aciklama, Siralama=@Siralama, Aktif=@Aktif{0} WHERE ID=@ID".Replace("{0}", fuLogo.HasFile ? ", Logo=@Logo" : ""), b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", lblMarkaID.Text);
                com.Parameters.AddWithValue("@MarkaAdi", txtMarkaAdi.Text);
                com.Parameters.AddWithValue("@Aciklama", txtAciklama.Text);

                int siralama;
                if (!string.IsNullOrEmpty(txtSiralama.Text) && int.TryParse(txtSiralama.Text, out siralama))
                    com.Parameters.AddWithValue("@Siralama", siralama);
                else
                    com.Parameters.AddWithValue("@Siralama", (object)DBNull.Value);

                com.Parameters.AddWithValue("@Aktif", chkAktif.Checked);

                if (fuLogo.HasFile)
                {
                    string uzanti = System.IO.Path.GetExtension(fuLogo.FileName).ToLower();
                    if (uzanti == ".jpg" || uzanti == ".jpeg" || uzanti == ".png")
                    {
                        string logoAdi = DateTime.Now.ToString("yyyyMMddHHmmss") + uzanti;
                        fuLogo.SaveAs(Server.MapPath("~/Upload/") + logoAdi);
                        com.Parameters.AddWithValue("@Logo", logoAdi);
                    }
                }

                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                if (com.ExecuteNonQuery() > 0)
                {
                    Response.Write("<script lang='JavaScript'>alert('Marka başarıyla güncellenmiştir!'); window.location = 'MarkaListe.aspx'</script>");
                }
                com.Connection.Close();
            }
        }

        protected void lnkIptal_Click(object sender, EventArgs e)
        {
            Response.Redirect("MarkaListe.aspx");
        }

        private void MarkaBilgileriniGetir(int id)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT * FROM Marka WHERE ID=@ID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", id);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                SqlDataReader dr = com.ExecuteReader();
                if (dr.Read())
                {
                    lblMarkaID.Text = id.ToString();
                    txtMarkaAdi.Text = Duzelt(dr["MarkaAdi"]);
                    txtAciklama.Text = Duzelt(dr["Aciklama"]);
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
