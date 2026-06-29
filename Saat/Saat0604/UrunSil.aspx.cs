
using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace Saat.Saat0604
{
    public partial class UrunSil : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;
                if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out id))
                {
                    Response.Redirect("UrunListe.aspx");
                    return;
                }
                Sil(id);
            }
        }

        private static string UploadFizikselYol(Page sayfa, string veritabaniDosyaAdi)
        {
            if (string.IsNullOrWhiteSpace(veritabaniDosyaAdi)) return null;
            var ad = Path.GetFileName(veritabaniDosyaAdi.Trim().Replace('/', '\\'));
            if (string.IsNullOrEmpty(ad)) return null;
            return Path.Combine(sayfa.Server.MapPath("~/Upload"), ad);
        }

        private void Sil(int urunId)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            try
            {
                using (var chk = new SqlCommand("SELECT COUNT(*) FROM SiparisDetay WHERE UrunId=@ID", b.Baglanti))
                {
                    chk.Parameters.AddWithValue("@ID", urunId);
                    if (chk.Connection.State == ConnectionState.Closed) chk.Connection.Open();
                    int sipSay = Convert.ToInt32(chk.ExecuteScalar());
                    chk.Connection.Close();
                    if (sipSay > 0)
                    {
                        UyariVeListeye("Bu \u00fcr\u00fcn en az bir sipari\u015fte ge\u00e7ti\u011fi i\u00e7in veritaban\u0131 kural\u0131 gere\u011fi silinemez. \u00dcr\u00fcn\u00fc listeden kald\u0131rmak i\u00e7in d\u00fczenle sayfas\u0131nda Aktif se\u00e7ene\u011fini kapat\u0131n.");
                        return;
                    }
                }

                string anaResim = "";
                using (SqlCommand get = new SqlCommand("SELECT AnaResim FROM Urun WHERE ID=@ID", b.Baglanti))
                {
                    get.Parameters.AddWithValue("@ID", urunId);
                    if (get.Connection.State == ConnectionState.Closed) get.Connection.Open();
                    object o = get.ExecuteScalar();
                    get.Connection.Close();
                    if (o != null && o != DBNull.Value) anaResim = o.ToString();
                }

                using (SqlCommand delSep = new SqlCommand("DELETE FROM Sepet WHERE UrunId=@ID", b.Baglanti))
                {
                    delSep.Parameters.AddWithValue("@ID", urunId);
                    if (delSep.Connection.State == ConnectionState.Closed) delSep.Connection.Open();
                    delSep.ExecuteNonQuery();
                    delSep.Connection.Close();
                }

                using (SqlCommand delVar = new SqlCommand("DELETE FROM UrunVaryant WHERE UrunId=@ID", b.Baglanti))
                {
                    delVar.Parameters.AddWithValue("@ID", urunId);
                    if (delVar.Connection.State == ConnectionState.Closed) delVar.Connection.Open();
                    delVar.ExecuteNonQuery();
                    delVar.Connection.Close();
                }
                using (SqlCommand delKat = new SqlCommand("DELETE FROM UrunKategori WHERE UrunId=@ID", b.Baglanti))
                {
                    delKat.Parameters.AddWithValue("@ID", urunId);
                    if (delKat.Connection.State == ConnectionState.Closed) delKat.Connection.Open();
                    delKat.ExecuteNonQuery();
                    delKat.Connection.Close();
                }

                using (SqlCommand getRes = new SqlCommand("SELECT Resim FROM UrunResim WHERE UrunId=@ID", b.Baglanti))
                {
                    getRes.Parameters.AddWithValue("@ID", urunId);
                    if (getRes.Connection.State == ConnectionState.Closed) getRes.Connection.Open();
                    using (SqlDataReader rdr = getRes.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var r = rdr["Resim"] as string;
                            try
                            {
                                var yol = UploadFizikselYol(Page, r);
                                if (yol != null && File.Exists(yol)) File.Delete(yol);
                            }
                            catch { }
                        }
                    }
                    getRes.Connection.Close();
                }
                using (SqlCommand delRes = new SqlCommand("DELETE FROM UrunResim WHERE UrunId=@ID", b.Baglanti))
                {
                    delRes.Parameters.AddWithValue("@ID", urunId);
                    if (delRes.Connection.State == ConnectionState.Closed) delRes.Connection.Open();
                    delRes.ExecuteNonQuery();
                    delRes.Connection.Close();
                }
                using (SqlCommand del = new SqlCommand("DELETE FROM Urun WHERE ID=@ID", b.Baglanti))
                {
                    del.Parameters.AddWithValue("@ID", urunId);
                    if (del.Connection.State == ConnectionState.Closed) del.Connection.Open();
                    del.ExecuteNonQuery();
                    del.Connection.Close();
                }
                try
                {
                    var yolAna = UploadFizikselYol(Page, anaResim);
                    if (yolAna != null && File.Exists(yolAna)) File.Delete(yolAna);
                }
                catch { }

                UyariVeListeye("\u00dcr\u00fcn ve ili\u015fkileri ba\u015far\u0131yla silindi.");
            }
            catch (SqlException ex)
            {
                UyariVeListeye("Silme i\u015flemi veritaban\u0131 hatas\u0131: " + ex.Message);
            }
            catch (Exception ex)
            {
                UyariVeListeye("Silme i\u015flemi hatas\u0131: " + ex.Message);
            }
        }

        private void UyariVeListeye(string mesaj)
        {
            string js = System.Web.HttpUtility.JavaScriptStringEncode(mesaj, true);
            Response.Write("<script>alert(" + js + "); window.location='UrunListe.aspx';</script>");
        }
    }
}
