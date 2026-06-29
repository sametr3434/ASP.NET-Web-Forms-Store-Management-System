using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Saat
{
    public partial class BlogDetay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string gelenID = null;
            if (Page.RouteData.Values["ID"] != null)
                gelenID = Page.RouteData.Values["ID"].ToString();

            if (string.IsNullOrEmpty(gelenID))
                gelenID = Request.QueryString["ID"];

            if (string.IsNullOrEmpty(gelenID))
            {
                Response.Redirect("/blog");
                return;
            }

            if (!BlogGetir(gelenID))
            {
                Response.Redirect("/blog");
                return;
            }

            SiteSabitlerini(gelenID);
            BlogListesiGetir(gelenID);
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void SiteSabitlerini(string gelenID)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (var com = new SqlCommand(
                "SELECT Resim, sitebaslik, (SELECT baslik FROM blog WHERE ID=@GelenID) AS blogadi FROM sitesabitleri WHERE ID=@ID",
                b.Baglanti))
            {
                com.Parameters.AddWithValue("@ID", "1");
                com.Parameters.AddWithValue("@GelenID", gelenID);

                if (com.Connection.State == ConnectionState.Closed)
                    b.Baglanti.Open();

                using (var dr = com.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        Title = EncodingHelper.DuzeltTurkce(dr["sitebaslik"].ToString()) + " - Blog - "
                            + EncodingHelper.DuzeltTurkce(dr["blogadi"]?.ToString() ?? "");
                    }
                }
            }
        }

        protected bool BlogGetir(string gelenID)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (var com = new SqlCommand(@"
                SELECT ID, baslik, tarih, kisaaciklama, Resim, aciklama
                FROM Blog WHERE aktif=@aktif AND ID=@GelenID", b.Baglanti))
            {
                com.Parameters.AddWithValue("@GelenID", gelenID);
                com.Parameters.AddWithValue("@aktif", "Evet");

                if (com.Connection.State == ConnectionState.Closed)
                    b.Baglanti.Open();

                using (var dr = com.ExecuteReader())
                {
                    if (!dr.Read())
                        return false;

                    string baslik = EncodingHelper.DuzeltTurkce(dr["baslik"].ToString());
                    ltrBlogBaslik.Text = baslik;
                    ltrKisaAciklama.Text = EncodingHelper.DuzeltTurkce(dr["kisaaciklama"]?.ToString() ?? "");
                    ltrTarih.Text = dr["tarih"]?.ToString() ?? "";
                    ltrAciklama.Text = dr["aciklama"]?.ToString() ?? "";

                    string resim = dr["Resim"]?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(resim))
                        ltrResim.Text = "<img src='/Upload/" + Server.HtmlEncode(resim) + "' alt='" + Server.HtmlEncode(baslik).Replace("'", "&#39;") + "' class='img-fluid' />";
                    else
                        ltrResim.Text = "";

                    ltrYorumAdeti.Text = "0";
                }
            }
            return true;
        }

        protected void BlogListesiGetir(string gelenID)
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (var com = new SqlCommand(@"
                SELECT TOP 3 ID, baslik, tarih, kisaaciklama, Resim
                FROM Blog WHERE aktif=@aktif AND ID<>@GelenID
                ORDER BY siralama", b.Baglanti))
            {
                com.Parameters.AddWithValue("@GelenID", gelenID);
                com.Parameters.AddWithValue("@aktif", "Evet");

                if (com.Connection.State == ConnectionState.Closed) b.Baglanti.Open();

                using (var dr = com.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["baslik"] != DBNull.Value)
                                row["baslik"] = EncodingHelper.DuzeltTurkce(row["baslik"].ToString());
                        }
                        rptBlogGetir.DataSource = dt;
                        rptBlogGetir.DataBind();
                    }
                }
            }
        }
    }
}
