-- Üniversite demo: bir kez çalıştırın (admin_saat veritabanında)

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'Urun') AND name = N'IncelemeSayisi')
    ALTER TABLE Urun ADD IncelemeSayisi INT NOT NULL CONSTRAINT DF_Urun_IncelemeSayisi DEFAULT 0;
GO

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
END
GO
