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
                    }

                    _hazir = true;
                }
                catch
                {
                    /* bağlantı yoksa demo sayfaları yine çalışsın */
                }
            }
        }
    }
}
