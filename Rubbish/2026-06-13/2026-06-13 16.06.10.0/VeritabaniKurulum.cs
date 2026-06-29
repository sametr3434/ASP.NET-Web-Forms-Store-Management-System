using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Saat.App_Code
{
    /// <summary>
    /// Demo için gerekli tablo/kolon — uygulama açılışında otomatik (manuel script gerekmez).
    /// </summary>
    public static class VeritabaniKurulum
    {
        static readonly object Kilit = new object();
        static bool _hazir;

        public static void Hazirla()
        {
            if (_hazir) return;
            lock (Kilit)
            {
                if (_hazir) return;
                try
                {
                    var cs = ConfigurationManager.ConnectionStrings["TuzlaTasarim"]?.ConnectionString;
                    if (string.IsNullOrWhiteSpace(cs)) return;

                    using (var conn = new SqlConnection(cs))
                    {
                        conn.Open();

                        using (var cmd = new SqlCommand(@"
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'Urun') AND name = N'IncelemeSayisi')
    ALTER TABLE Urun ADD IncelemeSayisi INT NOT NULL CONSTRAINT DF_Urun_IncelemeSayisi DEFAULT 0;", conn))
                            cmd.ExecuteNonQuery();

                        using (var cmd2 = new SqlCommand(@"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'UrunIlgiLog')
BEGIN
    CREATE TABLE UrunIlgiLog (
        ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        MusteriId INT NULL,
        OturumId NVARCHAR(64) NULL,
        UrunId INT NOT NULL,
        OlayTipi NVARCHAR(32) NOT NULL,
        Puan INT NOT NULL DEFAULT 1,
        Tarih DATETIME NOT NULL DEFAULT GETDATE()
    );
    CREATE INDEX IX_UrunIlgiLog_Oturum ON UrunIlgiLog(OturumId, Tarih);
    CREATE INDEX IX_UrunIlgiLog_Musteri ON UrunIlgiLog(MusteriId, Tarih);
END", conn))
                            cmd2.ExecuteNonQuery();

                        using (var cmd3 = new SqlCommand(@"
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'Musteri') AND name = N'KullaniciAdi')
    ALTER TABLE Musteri ADD KullaniciAdi NVARCHAR(64) NULL;", conn))
                            cmd3.ExecuteNonQuery();

                        DemoVerileriniHazirla(conn);
                    }

                    _hazir = true;
                }
                catch
                {
                    /* bağlantı yoksa demo sayfaları yine çalışsın */
                }
            }
        }

        static void DemoVerileriniHazirla(SqlConnection conn)
        {
            int demoId = 0;
            using (var cmd = new SqlCommand(
                "SELECT ID FROM Musteri WHERE LOWER(LTRIM(RTRIM(KullaniciAdi)))=N'demo'", conn))
            {
                var r = cmd.ExecuteScalar();
                if (r != null && r != DBNull.Value)
                    demoId = Convert.ToInt32(r);
            }

            if (demoId <= 0)
            {
                string hash = MusteriAuth.HashOlustur("demo");
                using (var cmd = new SqlCommand(@"
INSERT INTO Musteri (Ad, Soyad, EPosta, Telefon, Sifre, KullaniciAdi, Adres, Il, Ilce, PostaKodu, KayitTarihi, Aktif)
VALUES (N'Demo', N'Kullanıcı', N'demo@saat.local', N'0555 123 45 67', @sifre, N'demo',
        N'Atatürk Cad. No:12 D:4', N'İstanbul', N'Kadıköy', N'34710', DATEADD(MONTH, -3, GETDATE()), 1);
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
                {
                    cmd.Parameters.AddWithValue("@sifre", hash);
                    demoId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            else
            {
                using (var cmd = new SqlCommand(@"
UPDATE Musteri SET KullaniciAdi=N'demo', Aktif=1
WHERE ID=@id AND (KullaniciAdi IS NULL OR LTRIM(RTRIM(KullaniciAdi))=N'')", conn))
                {
                    cmd.Parameters.AddWithValue("@id", demoId);
                    cmd.ExecuteNonQuery();
                }
            }

            if (demoId <= 0) return;

            int siparisSayisi;
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Siparis WHERE MusteriId=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", demoId);
                siparisSayisi = (int)cmd.ExecuteScalar();
            }

            if (siparisSayisi > 0) return;

            string urun1Ad = N"Demo Saat Model A";
            string urun2Ad = N"Demo Saat Model B";
            decimal urun1Fiyat = 2499.00m;
            decimal urun2Fiyat = 1899.00m;

            using (var cmd = new SqlCommand(@"
SELECT TOP 1 ID, UrunAdi, ISNULL(NULLIF(IndirimliFiyat,0), Fiyat) AS Fiyat
FROM Urun WHERE Aktif=1 ORDER BY ID", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    urun1Ad = rdr["UrunAdi"].ToString();
                    urun1Fiyat = Convert.ToDecimal(rdr["Fiyat"]);
                }
            }

            using (var cmd = new SqlCommand(@"
SELECT TOP 1 ID, UrunAdi, ISNULL(NULLIF(IndirimliFiyat,0), Fiyat) AS Fiyat
FROM Urun WHERE Aktif=1 AND UrunAdi<>@ad ORDER BY ID DESC", conn))
            {
                cmd.Parameters.AddWithValue("@ad", urun1Ad);
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        urun2Ad = rdr["UrunAdi"].ToString();
                        urun2Fiyat = Convert.ToDecimal(rdr["Fiyat"]);
                    }
                }
            }

            DemoSiparisEkle(conn, demoId, "SP202601150001", DATEADD_MONTHS(-2),
                urun1Ad, urun1Fiyat, 1, N"Teslim Edildi", N"Ödendi - Kapıda Ödeme");
            DemoSiparisEkle(conn, demoId, "SP202605280002", DATEADD_MONTHS(0).AddDays(-14),
                urun2Ad, urun2Fiyat, 2, N"Kargoda", N"Ödendi - EFT/Havale");
        }

        static DateTime DATEADD_MONTHS(int ay) => DateTime.Now.AddMonths(ay);

        static void DemoSiparisEkle(SqlConnection conn, int musteriId, string siparisNo, DateTime tarih,
            string urunAdi, decimal birimFiyat, int adet, string siparisDurumu, string odemeDurumu)
        {
            decimal ara = birimFiyat * adet;
            decimal kargo = 0m;
            decimal toplam = ara + kargo;
            int siparisId;

            using (var cmd = new SqlCommand(@"
INSERT INTO Siparis (
    SiparisNo, MusteriId, MisafirAd, MisafirSoyad, MisafirTelefon,
    TeslimatAdres, TeslimatIl, TeslimatIlce,
    AraToplam, IndirimTutari, KargoTutari, ToplamTutar, ParaBirimi,
    OdemeDurumu, SiparisDurumu, SiparisTarihi, OdemeTutarTRY)
VALUES (
    @no, @mid, N'Demo', N'Kullanıcı', N'0555 123 45 67',
    N'Atatürk Cad. No:12 D:4', N'İstanbul', N'Kadıköy',
    @ara, 0, @kargo, @toplam, N'TRY',
    @odeme, @durum, @tarih, @toplam);
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn))
            {
                cmd.Parameters.AddWithValue("@no", siparisNo);
                cmd.Parameters.AddWithValue("@mid", musteriId);
                cmd.Parameters.AddWithValue("@ara", ara);
                cmd.Parameters.AddWithValue("@kargo", kargo);
                cmd.Parameters.AddWithValue("@toplam", toplam);
                cmd.Parameters.AddWithValue("@odeme", odemeDurumu);
                cmd.Parameters.AddWithValue("@durum", siparisDurumu);
                cmd.Parameters.AddWithValue("@tarih", tarih);
                siparisId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = new SqlCommand(@"
INSERT INTO SiparisDetay (SiparisId, UrunAdi, Beden, Renk, Adet, BirimFiyat, ToplamFiyat, ParaBirimi)
VALUES (@sid, @urun, NULL, NULL, @adet, @birim, @toplam, N'TRY')", conn))
            {
                cmd.Parameters.AddWithValue("@sid", siparisId);
                cmd.Parameters.AddWithValue("@urun", urunAdi);
                cmd.Parameters.AddWithValue("@adet", adet);
                cmd.Parameters.AddWithValue("@birim", birimFiyat);
                cmd.Parameters.AddWithValue("@toplam", birimFiyat * adet);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
