using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;

namespace Saat.App_Code
{
    [Serializable]
    public class DemoCheckoutAdres
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string PostaKodu { get; set; }
    }

    public static class SiparisHelper
    {
        public const string SessionAdresKey = "DemoCheckoutAdres";

        public static string SahteOdemeTamamla(HttpContext ctx, DemoCheckoutAdres adres, string odemeYontemi)
        {
            if (ctx == null || adres == null) return null;
            VeritabaniKurulum.SiparisSemasiniGuncelle();
            MagazaYardimcisi.OturumIdAl(ctx);

            var sepet = SepetHelper.Listele(ctx);
            if (sepet.Rows.Count == 0) return null;

            decimal araToplam = SepetHelper.ToplamTutar(ctx);
            decimal kargo = 0m;
            decimal toplam = araToplam + kargo;
            string siparisNo = "SP" + DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            string odemeMetni = OdemeYontemiMetin(odemeYontemi);
            int? musteriId = MusteriAuth.GirisMi(ctx.Session)
                ? (int?)MusteriAuth.MusteriId(ctx.Session) : null;

            using (var b = new BaglantiBilgileri())
            {
            if (b.Baglanti.State == ConnectionState.Closed) b.Baglanti.Open();

            using (var tr = b.Baglanti.BeginTransaction())
            {
                try
                {
                    int siparisId;
                    using (var cmd = new SqlCommand(@"
                        INSERT INTO Siparis (
                            SiparisNo, MusteriId, MisafirAd, MisafirSoyad, MisafirTelefon,
                            TeslimatAdres, TeslimatIl, TeslimatIlce, TeslimatPostaKodu,
                            AraToplam, IndirimTutari, KargoTutari, ToplamTutar,
                            OdemeDurumu, SiparisDurumu, SiparisTarihi, OdemeTutarTRY)
                        VALUES (
                            @SiparisNo, @MusteriId, @MisafirAd, @MisafirSoyad, @MisafirTelefon,
                            @TeslimatAdres, @TeslimatIl, @TeslimatIlce, @PostaKodu,
                            @AraToplam, 0, @Kargo, @Toplam,
                            @OdemeDurumu, N'Yeni', GETDATE(), @OdemeTry);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);", b.Baglanti, tr))
                    {
                        cmd.Parameters.AddWithValue("@SiparisNo", siparisNo);
                        cmd.Parameters.AddWithValue("@MusteriId", musteriId.HasValue ? (object)musteriId.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@MisafirAd", adres.Ad ?? "");
                        cmd.Parameters.AddWithValue("@MisafirSoyad", adres.Soyad ?? "");
                        cmd.Parameters.AddWithValue("@MisafirTelefon", adres.Telefon ?? "");
                        cmd.Parameters.AddWithValue("@TeslimatAdres", adres.Adres ?? "");
                        cmd.Parameters.AddWithValue("@TeslimatIl", string.IsNullOrWhiteSpace(adres.Il) ? (object)DBNull.Value : adres.Il);
                        cmd.Parameters.AddWithValue("@TeslimatIlce", string.IsNullOrWhiteSpace(adres.Ilce) ? (object)DBNull.Value : adres.Ilce);
                        cmd.Parameters.AddWithValue("@PostaKodu", string.IsNullOrWhiteSpace(adres.PostaKodu) ? (object)DBNull.Value : adres.PostaKodu);
                        cmd.Parameters.AddWithValue("@AraToplam", araToplam);
                        cmd.Parameters.AddWithValue("@Kargo", kargo);
                        cmd.Parameters.AddWithValue("@Toplam", toplam);
                        cmd.Parameters.AddWithValue("@OdemeDurumu", odemeMetni);
                        cmd.Parameters.AddWithValue("@OdemeTry", toplam);
                        siparisId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    foreach (DataRow row in sepet.Rows)
                    {
                        int adet = Convert.ToInt32(row["Adet"]);
                        decimal birim = Convert.ToDecimal(row["BirimFiyat"]);
                        string urunAdi = row["UrunAdi"]?.ToString() ?? "";
                        int urunId = Convert.ToInt32(row["UrunId"]);
                        string kasa = row.Table.Columns.Contains("KasaBoyutu") && row["KasaBoyutu"] != DBNull.Value
                            ? EncodingHelper.DuzeltTurkce(row["KasaBoyutu"].ToString()) : null;
                        string renk = row.Table.Columns.Contains("RenkAdi") && row["RenkAdi"] != DBNull.Value
                            ? EncodingHelper.DuzeltTurkce(row["RenkAdi"].ToString()) : null;
                        IlgiTakipHelper.SatinAlmaKaydet(ctx, urunId);
                        using (var cmd = new SqlCommand(@"
                            INSERT INTO SiparisDetay (SiparisId, UrunId, UrunAdi, UrunCapi, Renk, Adet, BirimFiyat, ToplamFiyat)
                            VALUES (@Sid, @Uid, @UrunAdi, @UrunCapi, @Renk, @Adet, @Birim, @Toplam)", b.Baglanti, tr))
                        {
                            cmd.Parameters.AddWithValue("@Sid", siparisId);
                            cmd.Parameters.AddWithValue("@Uid", urunId);
                            cmd.Parameters.AddWithValue("@UrunAdi", urunAdi);
                            cmd.Parameters.AddWithValue("@UrunCapi", string.IsNullOrEmpty(kasa) ? (object)DBNull.Value : kasa);
                            cmd.Parameters.AddWithValue("@Renk", string.IsNullOrEmpty(renk) ? (object)DBNull.Value : renk);
                            cmd.Parameters.AddWithValue("@Adet", adet);
                            cmd.Parameters.AddWithValue("@Birim", birim);
                            cmd.Parameters.AddWithValue("@Toplam", birim * adet);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            SepetHelper.Temizle(ctx);
            ctx.Session.Remove(SessionAdresKey);
            return siparisNo;
            }
        }

        public static string OdemeYontemiMetin(string kod)
        {
            if (string.IsNullOrWhiteSpace(kod)) return "Ödendi";
            switch (kod.Trim().ToLowerInvariant())
            {
                case "kapida": return "Ödendi - Kapıda Ödeme";
                case "eft": return "Ödendi - EFT/Havale";
                case "kart": return "Ödendi - Kredi Kartı";
                default: return "Ödendi - " + kod;
            }
        }

        public static string OdemeYontemiBaslik(string kod)
        {
            if (string.IsNullOrWhiteSpace(kod)) return "Ödeme";
            switch (kod.Trim().ToLowerInvariant())
            {
                case "kapida": return "Kapıda Ödeme";
                case "eft": return "EFT / Havale";
                case "kart": return "Kredi Kartı";
                default: return kod;
            }
        }
    }
}
