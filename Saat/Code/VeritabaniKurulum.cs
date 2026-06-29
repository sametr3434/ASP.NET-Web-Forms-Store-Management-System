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

        /// <summary>Sipariş kaydı öncesi şema uyumu — app pool yenilenmeden de çalışır.</summary>
        public static void SiparisSemasiniGuncelle()
        {
            try
            {
                var cs = ConfigurationManager.ConnectionStrings["TuzlaTasarim"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(cs)) return;
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    SiparisTablolariHazirla(conn);
                }
            }
            catch
            {
                /* bağlantı yoksa checkout hata mesajı gösterir */
            }
        }

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

                        using (var cmdKasa = new SqlCommand(@"
IF OBJECT_ID(N'UrunVaryant', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'UrunVaryant', N'KasaId') IS NULL
    BEGIN
        IF COL_LENGTH(N'UrunVaryant', N'BedenId') IS NOT NULL
            EXEC sp_rename N'UrunVaryant.BedenId', N'KasaId', N'COLUMN';
        ELSE
            ALTER TABLE UrunVaryant ADD KasaId INT NULL;
    END
END", conn))
                            cmdKasa.ExecuteNonQuery();

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

                        using (var cmd4 = new SqlCommand(@"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'MusteriAdres')
BEGIN
    CREATE TABLE MusteriAdres (
        ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        MusteriId INT NOT NULL,
        Baslik NVARCHAR(64) NOT NULL,
        Ad NVARCHAR(64) NULL,
        Soyad NVARCHAR(64) NULL,
        Telefon NVARCHAR(32) NULL,
        Adres NVARCHAR(500) NOT NULL,
        Il NVARCHAR(64) NULL,
        Ilce NVARCHAR(64) NULL,
        PostaKodu NVARCHAR(16) NULL,
        Varsayilan BIT NOT NULL CONSTRAINT DF_MusteriAdres_Varsayilan DEFAULT 0,
        KayitTarihi DATETIME NOT NULL CONSTRAINT DF_MusteriAdres_Kayit DEFAULT GETDATE()
    );
    CREATE INDEX IX_MusteriAdres_Musteri ON MusteriAdres(MusteriId);
END", conn))
                            cmd4.ExecuteNonQuery();

                        DemoRenkKasaVeVaryantHazirla(conn);
                        SiparisTablolariHazirla(conn);
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

        /// <summary>Sipariş tabloları — canlı şema ile SiparisHelper uyumu (UrunCapi, OdemeTutarTRY vb.).</summary>
        static void SiparisTablolariHazirla(SqlConnection conn)
        {
            if (conn == null) return;

            using (var cmd = new SqlCommand(@"
IF OBJECT_ID(N'Siparis', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'Siparis', N'TeslimatIl') IS NULL
        ALTER TABLE Siparis ADD TeslimatIl NVARCHAR(64) NULL;
    IF COL_LENGTH(N'Siparis', N'TeslimatIlce') IS NULL
        ALTER TABLE Siparis ADD TeslimatIlce NVARCHAR(64) NULL;
    IF COL_LENGTH(N'Siparis', N'TeslimatPostaKodu') IS NULL
        ALTER TABLE Siparis ADD TeslimatPostaKodu NVARCHAR(16) NULL;
    IF COL_LENGTH(N'Siparis', N'OdemeTutarTRY') IS NULL
        ALTER TABLE Siparis ADD OdemeTutarTRY DECIMAL(18,2) NULL;
    IF COL_LENGTH(N'Siparis', N'AraToplam') IS NULL
        ALTER TABLE Siparis ADD AraToplam DECIMAL(18,2) NULL;
    IF COL_LENGTH(N'Siparis', N'IndirimTutari') IS NULL
        ALTER TABLE Siparis ADD IndirimTutari DECIMAL(18,2) NULL;
    IF COL_LENGTH(N'Siparis', N'KargoTutari') IS NULL
        ALTER TABLE Siparis ADD KargoTutari DECIMAL(18,2) NULL;
    IF COL_LENGTH(N'Siparis', N'ToplamTutar') IS NULL
        ALTER TABLE Siparis ADD ToplamTutar DECIMAL(18,2) NULL;
END

IF OBJECT_ID(N'SiparisDetay', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'SiparisDetay', N'UrunId') IS NULL
        ALTER TABLE SiparisDetay ADD UrunId INT NULL;

    IF COL_LENGTH(N'SiparisDetay', N'UrunCapi') IS NULL
    BEGIN
        IF COL_LENGTH(N'SiparisDetay', N'Beden') IS NOT NULL
            EXEC sp_rename N'SiparisDetay.Beden', N'UrunCapi', N'COLUMN';
        ELSE
            ALTER TABLE SiparisDetay ADD UrunCapi NVARCHAR(128) NULL;
    END

    IF COL_LENGTH(N'SiparisDetay', N'Renk') IS NULL
        ALTER TABLE SiparisDetay ADD Renk NVARCHAR(64) NULL;
    IF COL_LENGTH(N'SiparisDetay', N'BirimFiyat') IS NULL
        ALTER TABLE SiparisDetay ADD BirimFiyat DECIMAL(18,2) NULL;
    IF COL_LENGTH(N'SiparisDetay', N'ToplamFiyat') IS NULL
        ALTER TABLE SiparisDetay ADD ToplamFiyat DECIMAL(18,2) NULL;
END", conn))
                cmd.ExecuteNonQuery();
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

            DemoAdresleriHazirla(conn, demoId);

            int siparisSayisi;
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Siparis WHERE MusteriId=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", demoId);
                siparisSayisi = (int)cmd.ExecuteScalar();
            }

            if (siparisSayisi > 0) return;

            int urun1Id = 1;
            int urun2Id = 2;
            string urun1Ad = "Demo Saat Model A";
            string urun2Ad = "Demo Saat Model B";
            decimal urun1Fiyat = 2499.00m;
            decimal urun2Fiyat = 1899.00m;

            using (var cmd = new SqlCommand(@"
SELECT TOP 1 ID, UrunAdi, ISNULL(NULLIF(IndirimliFiyat,0), Fiyat) AS Fiyat
FROM Urun WHERE Aktif=1 ORDER BY ID", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    urun1Id = Convert.ToInt32(rdr["ID"]);
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
                        urun2Id = Convert.ToInt32(rdr["ID"]);
                        urun2Ad = rdr["UrunAdi"].ToString();
                        urun2Fiyat = Convert.ToDecimal(rdr["Fiyat"]);
                    }
                }
            }

            DemoSiparisEkle(conn, demoId, "SP202601150001", DATEADD_MONTHS(-2),
                urun1Id, urun1Ad, urun1Fiyat, 1, "Teslim Edildi", "Ödendi - Kapıda Ödeme");
            DemoSiparisEkle(conn, demoId, "SP202605280002", DATEADD_MONTHS(0).AddDays(-14),
                urun2Id, urun2Ad, urun2Fiyat, 2, "Kargoda", "Ödendi - EFT/Havale");
        }

        static void DemoAdresleriHazirla(SqlConnection conn, int demoId)
        {
            int evSayisi;
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM MusteriAdres WHERE MusteriId=@id AND Baslik=N'Ev'", conn))
            {
                cmd.Parameters.AddWithValue("@id", demoId);
                evSayisi = (int)cmd.ExecuteScalar();
            }
            if (evSayisi == 0)
            {
                using (var cmd = new SqlCommand(@"
INSERT INTO MusteriAdres (MusteriId, Baslik, Ad, Soyad, Telefon, Adres, Il, Ilce, PostaKodu, Varsayilan)
VALUES (@mid, N'Ev', N'Demo', N'Kullanıcı', N'0555 123 45 67', N'Atatürk Cad. No:12 D:4', N'İstanbul', N'Kadıköy', N'34710', 1);", conn))
                {
                    cmd.Parameters.AddWithValue("@mid", demoId);
                    cmd.ExecuteNonQuery();
                }
            }

            int isSayisi;
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM MusteriAdres WHERE MusteriId=@id AND Baslik=N'İş'", conn))
            {
                cmd.Parameters.AddWithValue("@id", demoId);
                isSayisi = (int)cmd.ExecuteScalar();
            }
            if (isSayisi == 0)
            {
                using (var cmd = new SqlCommand(@"
INSERT INTO MusteriAdres (MusteriId, Baslik, Ad, Soyad, Telefon, Adres, Il, Ilce, PostaKodu, Varsayilan)
VALUES (@mid, N'İş', N'Demo', N'Kullanıcı', N'0555 123 45 67', N'Levent Plaza Kat:8', N'İstanbul', N'Beşiktaş', N'34330', 0);", conn))
                {
                    cmd.Parameters.AddWithValue("@mid", demoId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static DateTime DATEADD_MONTHS(int ay) => DateTime.Now.AddMonths(ay);

        static void DemoSiparisEkle(SqlConnection conn, int musteriId, string siparisNo, DateTime tarih,
            int urunId, string urunAdi, decimal birimFiyat, int adet, string siparisDurumu, string odemeDurumu)
        {
            decimal ara = birimFiyat * adet;
            decimal kargo = 0m;
            decimal toplam = ara + kargo;
            int siparisId;

            using (var cmd = new SqlCommand(@"
INSERT INTO Siparis (
    SiparisNo, MusteriId, MisafirAd, MisafirSoyad, MisafirTelefon,
    TeslimatAdres, TeslimatIl, TeslimatIlce,
    AraToplam, IndirimTutari, KargoTutari, ToplamTutar,
    OdemeDurumu, SiparisDurumu, SiparisTarihi, OdemeTutarTRY)
VALUES (
    @no, @mid, N'Demo', N'Kullanıcı', N'0555 123 45 67',
    N'Atatürk Cad. No:12 D:4', N'İstanbul', N'Kadıköy',
    @ara, 0, @kargo, @toplam,
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
INSERT INTO SiparisDetay (SiparisId, UrunId, UrunAdi, UrunCapi, Renk, Adet, BirimFiyat, ToplamFiyat)
VALUES (@sid, @uid, @urun, NULL, NULL, @adet, @birim, @toplam)", conn))
            {
                cmd.Parameters.AddWithValue("@sid", siparisId);
                cmd.Parameters.AddWithValue("@uid", urunId);
                cmd.Parameters.AddWithValue("@urun", urunAdi);
                cmd.Parameters.AddWithValue("@adet", adet);
                cmd.Parameters.AddWithValue("@birim", birimFiyat);
                cmd.Parameters.AddWithValue("@toplam", birimFiyat * adet);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Renk, kasa boyutu ve örnek varyant — ürün detayda renk/kasa seçimi için.</summary>
        static void DemoRenkKasaVeVaryantHazirla(SqlConnection conn)
        {
            if (conn == null) return;

            int renkSayisi;
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM Renk", conn))
                renkSayisi = Convert.ToInt32(cmd.ExecuteScalar());

            if (renkSayisi == 0)
            {
                using (var cmd = new SqlCommand(@"
INSERT INTO Renk (RenkAdi, RenkKodu, Siralama, Aktif) VALUES
(N'Siyah', N'#000000', 1, 1),
(N'Gümüş', N'#C0C0C0', 2, 1),
(N'Altın', N'#D4AF37', 3, 1),
(N'Lacivert', N'#1a237e', 4, 1);", conn))
                    cmd.ExecuteNonQuery();
            }

            int kasaSayisi;
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM kasaboyutu", conn))
                kasaSayisi = Convert.ToInt32(cmd.ExecuteScalar());

            if (kasaSayisi == 0)
            {
                using (var cmd = new SqlCommand(@"
INSERT INTO kasaboyutu (kasano, Siralama, Aktif) VALUES
(N'40 mm', 1, 1),
(N'42 mm', 2, 1),
(N'44 mm', 3, 1);", conn))
                    cmd.ExecuteNonQuery();
            }

            int kasa42Id = 0, renkSiyahId = 0, renkGumusId = 0;
            using (var cmd = new SqlCommand("SELECT TOP 1 ID FROM kasaboyutu WHERE kasano LIKE N'42%' ORDER BY Siralama, ID", conn))
            {
                var r = cmd.ExecuteScalar();
                if (r != null && r != DBNull.Value) kasa42Id = Convert.ToInt32(r);
            }
            if (kasa42Id <= 0)
            {
                using (var cmd = new SqlCommand("SELECT TOP 1 ID FROM kasaboyutu ORDER BY Siralama, ID", conn))
                {
                    var r = cmd.ExecuteScalar();
                    if (r != null && r != DBNull.Value) kasa42Id = Convert.ToInt32(r);
                }
            }
            using (var cmd = new SqlCommand("SELECT TOP 1 ID FROM Renk WHERE RenkAdi=N'Siyah' ORDER BY ID", conn))
            {
                var r = cmd.ExecuteScalar();
                if (r != null && r != DBNull.Value) renkSiyahId = Convert.ToInt32(r);
            }
            using (var cmd = new SqlCommand("SELECT TOP 1 ID FROM Renk WHERE RenkAdi=N'Gümüş' ORDER BY ID", conn))
            {
                var r = cmd.ExecuteScalar();
                if (r != null && r != DBNull.Value) renkGumusId = Convert.ToInt32(r);
            }
            if (renkSiyahId <= 0 || kasa42Id <= 0) return;

            using (var cmd = new SqlCommand(@"
SELECT u.ID FROM Urun u
WHERE u.Aktif = 1
  AND NOT EXISTS (SELECT 1 FROM UrunVaryant v WHERE v.UrunId = u.ID)", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                var urunIds = new System.Collections.Generic.List<int>();
                while (rdr.Read())
                    urunIds.Add(Convert.ToInt32(rdr["ID"]));
                rdr.Close();

                foreach (int uid in urunIds)
                {
                    VaryantEkle(conn, uid, kasa42Id, renkSiyahId, 5);
                    if (renkGumusId > 0)
                        VaryantEkle(conn, uid, kasa42Id, renkGumusId, 3);
                }
            }
        }

        static void VaryantEkle(SqlConnection conn, int urunId, int kasaId, int renkId, int stok)
        {
            using (var cmd = new SqlCommand(@"
IF NOT EXISTS (SELECT 1 FROM UrunVaryant WHERE UrunId=@uid AND KasaId=@kid AND RenkId=@rid)
INSERT INTO UrunVaryant (UrunId, KasaId, RenkId, StokAdedi) VALUES (@uid, @kid, @rid, @stok)", conn))
            {
                cmd.Parameters.AddWithValue("@uid", urunId);
                cmd.Parameters.AddWithValue("@kid", kasaId);
                cmd.Parameters.AddWithValue("@rid", renkId);
                cmd.Parameters.AddWithValue("@stok", stok);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
