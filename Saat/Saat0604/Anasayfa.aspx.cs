

using Saat.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class Anasayfa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    HttpCookie cerezOku = Request.Cookies["saatpanelcerez"];

                    if (cerezOku != null && cerezOku["saatpanel"] == "SaatPanel")
                    {
                        int kullaniciID = Convert.ToInt32(cerezOku["KullaniciID"].ToString());
                        lblKullaniciID.Text = kullaniciID.ToString();
                        lblKullaniciAdi.Text = EncodingHelper.DuzeltTurkce(cerezOku["KullaniciAdi"]?.ToString() ?? "");

                        SabitleriGetir();
                        IstatistikleriYukle();
                        KritikStokGetir();
                        SonSiparisleriGetir();
                        GrafikVerileriniHazirla();
                    }
                }
                catch (Exception) { }
            }
        }

        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        private void SabitleriGetir()
        {
            var b = new BaglantiBilgileri();
            var com = new SqlCommand("SELECT SiteBaslik, Resim FROM SiteSabitleri WHERE ID=1", b.Baglanti);
            if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
            try
            {
                using (var dr = com.ExecuteReader())
                {
                    if (dr.HasRows && dr.Read())
                        Title = Duzelt(dr["SiteBaslik"]) + " | Panel Anasayfa";
                }
            }
            finally { com.Connection.Close(); }
        }

        private void IstatistikleriYukle()
        {
            var b = new BaglantiBilgileri();
            try
            {
                if (b.Baglanti.State == ConnectionState.Closed) b.Baglanti.Open();

                var siparisSayisi = 0;
                try
                {
                    var comSiparis = new SqlCommand("SELECT COUNT(*) FROM Siparis", b.Baglanti);
                    siparisSayisi = Convert.ToInt32(comSiparis.ExecuteScalar());
                }
                catch { }
                ltrToplamSiparis.Text = siparisSayisi.ToString("N0");

                var ciro = 0m;
                try
                {
                    var comCiro = new SqlCommand(@"
                        SELECT ISNULL(SUM(ToplamTutar), 0) FROM Siparis 
                        WHERE OdemeDurumu IN (N'Tamamlandı', N'Ödendi', N'Odendi')
                          AND ISNULL(ParaBirimi, N'TRY') = N'TRY'
                    ", b.Baglanti);
                    ciro = Convert.ToDecimal(comCiro.ExecuteScalar());
                }
                catch
                {
                    try
                    {
                        var comCiro2 = new SqlCommand(@"
                        SELECT ISNULL(SUM(ToplamTutar), 0) FROM Siparis 
                        WHERE OdemeDurumu IN (N'Tamamlandı', N'Ödendi', N'Odendi')
                    ", b.Baglanti);
                        ciro = Convert.ToDecimal(comCiro2.ExecuteScalar());
                    }
                    catch { }
                }
                ltrToplamCiro.Text = ciro.ToString("N2") + " ₺";

                var musteriSayisi = 0;
                try
                {
                    var comMusteri = new SqlCommand("SELECT COUNT(*) FROM Musteri", b.Baglanti);
                    musteriSayisi = Convert.ToInt32(comMusteri.ExecuteScalar());
                }
                catch { }
                ltrMusteriSayisi.Text = musteriSayisi.ToString("N0");

                var urunSayisi = 0;
                try
                {
                    var comUrun = new SqlCommand("SELECT COUNT(*) FROM Urun WHERE Aktif=1", b.Baglanti);
                    urunSayisi = Convert.ToInt32(comUrun.ExecuteScalar());
                }
                catch { }
                ltrUrunSayisi.Text = urunSayisi.ToString("N0");
            }
            catch (Exception)
            {
                ltrToplamSiparis.Text = "0";
                ltrToplamCiro.Text = "0 ₺";
                ltrMusteriSayisi.Text = "0";
                ltrUrunSayisi.Text = "0";
            }
            finally { b.Baglanti.Close(); }
        }

        private void KritikStokGetir()
        {
            try
            {
                var b = new BaglantiBilgileri();
                var com = new SqlCommand(@"
                    SELECT TOP 10 ID, UrunAdi, ISNULL(StokAdedi, 0) AS StokAdedi, ISNULL(KritikStokSeviyesi, 5) AS KritikStokSeviyesi
                    FROM Urun 
                    WHERE Aktif=1 AND ISNULL(StokAdedi, 0) <= ISNULL(KritikStokSeviyesi, 5) AND ISNULL(StokAdedi, 0) >= 0
                    ORDER BY StokAdedi ASC
                ", b.Baglanti);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                try
                {
                    var dt = new DataTable();
                    dt.Load(com.ExecuteReader());
                    rptKritikStok.DataSource = dt;
                    rptKritikStok.DataBind();
                    pnlKritikStokTable.Visible = dt.Rows.Count > 0;
                    pnlKritikStokYok.Visible = dt.Rows.Count == 0;
                }
                finally { com.Connection.Close(); }
            }
            catch
            {
                pnlKritikStokTable.Visible = false;
                pnlKritikStokYok.Visible = true;
            }
        }

        protected string TutarPBAna(object tutar, object paraBirimi)
        {
            if (tutar == null || tutar == DBNull.Value) return "-";
            return ParaBirimiHelper.Format(Convert.ToDecimal(tutar), paraBirimi != null && paraBirimi != DBNull.Value ? paraBirimi.ToString() : ParaBirimiHelper.Try);
        }

        private void SonSiparisleriGetir()
        {
            try
            {
                var b = new BaglantiBilgileri();
                var com = new SqlCommand(@"
                    SELECT TOP 10 ID, SiparisNo, SiparisTarihi, ToplamTutar, SiparisDurumu, ISNULL(ParaBirimi, N'TRY') AS ParaBirimi
                    FROM Siparis 
                    ORDER BY SiparisTarihi DESC
                ", b.Baglanti);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                try
                {
                    var dt = new DataTable();
                    try
                    {
                        dt.Load(com.ExecuteReader());
                    }
                    catch
                    {
                        com.Connection.Close();
                        com = new SqlCommand(@"
                    SELECT TOP 10 ID, SiparisNo, SiparisTarihi, ToplamTutar, SiparisDurumu
                    FROM Siparis 
                    ORDER BY SiparisTarihi DESC
                ", b.Baglanti);
                        if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                        dt.Load(com.ExecuteReader());
                        if (!dt.Columns.Contains("ParaBirimi")) dt.Columns.Add("ParaBirimi", typeof(string));
                        foreach (System.Data.DataRow row in dt.Rows) row["ParaBirimi"] = ParaBirimiHelper.Try;
                    }
                    rptSonSiparisler.DataSource = dt;
                    rptSonSiparisler.DataBind();
                    pnlSonSiparisTable.Visible = dt.Rows.Count > 0;
                    pnlSonSiparisYok.Visible = dt.Rows.Count == 0;
                }
                finally { if (com.Connection.State == ConnectionState.Open) com.Connection.Close(); }
            }
            catch
            {
                pnlSonSiparisTable.Visible = false;
                pnlSonSiparisYok.Visible = true;
            }
        }

        private void GrafikVerileriniHazirla()
        {
            var ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;

            var aylikSatis = new List<object>();
            try
            {
                var b = new BaglantiBilgileri();
                var com = new SqlCommand(@"
                    SELECT YEAR(SiparisTarihi) AS Yil, MONTH(SiparisTarihi) AS Ay, ISNULL(SUM(ToplamTutar), 0) AS Tutar
                    FROM Siparis 
                    WHERE SiparisTarihi >= DATEADD(MONTH, -12, GETDATE())
                    AND OdemeDurumu IN (N'Tamamlandı', N'Ödendi', N'Odendi', N'Beklemede')
                    GROUP BY YEAR(SiparisTarihi), MONTH(SiparisTarihi)
                    ORDER BY Yil, Ay
                ", b.Baglanti);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                try
                {
                    using (var dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var yil = Convert.ToInt32(dr["Yil"]);
                            var ay = Convert.ToInt32(dr["Ay"]);
                            var tutar = Convert.ToDecimal(dr["Tutar"]);
                            aylikSatis.Add(new { date = $"{yil}-{ay:D2}-01", value = (double)tutar });
                        }
                    }
                }
                finally { com.Connection.Close(); }
            }
            catch { }

            if (aylikSatis.Count == 0)
            {
                var now = DateTime.Now;
                for (var i = 11; i >= 0; i--)
                {
                    var d = now.AddMonths(-i);
                    aylikSatis.Add(new { date = $"{d.Year}-{d.Month:D2}-01", value = 0 });
                }
            }
            hfAylikSatisData.Value = ser.Serialize(aylikSatis);

            var siparisDurum = new List<object>();
            try
            {
                var b = new BaglantiBilgileri();
                var com = new SqlCommand("SELECT ISNULL(SiparisDurumu, N'Belirsiz') AS Durum, COUNT(*) AS Adet FROM Siparis GROUP BY SiparisDurumu", b.Baglanti);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                try
                {
                    using (var dr = com.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            siparisDurum.Add(new
                            {
                                label = EncodingHelper.DuzeltTurkce(dr["Durum"]?.ToString() ?? ""),
                                value = Convert.ToInt32(dr["Adet"])
                            });
                        }
                    }
                }
                finally { com.Connection.Close(); }
            }
            catch { }

            if (siparisDurum.Count == 0)
                siparisDurum.Add(new { label = "Veri yok", value = 1 });
            hfSiparisDurumData.Value = ser.Serialize(siparisDurum);
        }
    }
}
