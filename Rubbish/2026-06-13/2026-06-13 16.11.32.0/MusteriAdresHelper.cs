using System;
using System.Data;
using System.Data.SqlClient;

namespace Saat.App_Code
{
    /// <summary>
    /// Üyenin birden fazla teslimat adresi yönetimi.
    /// </summary>
    public static class MusteriAdresHelper
    {
        public static DataTable Listele(int musteriId)
        {
            if (musteriId <= 0) return new DataTable();
            AnaAdrestenOlustur(musteriId);

            var b = new BaglantiBilgileri();
            var dt = new DataTable();
            using (var cmd = new SqlCommand(@"
SELECT ID, Baslik, Ad, Soyad, Telefon, Adres, Il, Ilce, PostaKodu, Varsayilan
FROM MusteriAdres
WHERE MusteriId=@mid
ORDER BY Varsayilan DESC, ID DESC", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@mid", musteriId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                new SqlDataAdapter(cmd).Fill(dt);
            }
            return dt;
        }

        /// <summary>Musteri tablosundaki tek adresi ilk kayıt olarak taşır (bir kez).</summary>
        public static void AnaAdrestenOlustur(int musteriId)
        {
            var b = new BaglantiBilgileri();
            int sayi;
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM MusteriAdres WHERE MusteriId=@mid", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@mid", musteriId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                sayi = (int)cmd.ExecuteScalar();
            }
            if (sayi > 0) return;

            using (var cmd = new SqlCommand(@"
SELECT Ad, Soyad, Telefon, Adres, Il, Ilce, PostaKodu
FROM Musteri WHERE ID=@id", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", musteriId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return;
                    string adres = rdr["Adres"]?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(adres)) return;
                    string ad = rdr["Ad"]?.ToString();
                    string soyad = rdr["Soyad"]?.ToString();
                    string tel = rdr["Telefon"]?.ToString();
                    string il = rdr["Il"]?.ToString();
                    string ilce = rdr["Ilce"]?.ToString();
                    string pk = rdr["PostaKodu"]?.ToString();
                    Ekle(musteriId, "Ev", ad, soyad, tel, adres, il, ilce, pk, true);
                }
            }
        }

        public static DemoCheckoutAdres CheckoutAdresiAl(int musteriId, int? adresId = null)
        {
            if (musteriId <= 0) return null;

            string email = "";
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand("SELECT EPosta FROM Musteri WHERE ID=@id", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", musteriId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                email = cmd.ExecuteScalar()?.ToString() ?? "";
            }

            AnaAdrestenOlustur(musteriId);

            string sql = adresId.HasValue && adresId.Value > 0
                ? @"SELECT Ad, Soyad, Telefon, Adres, Il, Ilce, PostaKodu FROM MusteriAdres
                    WHERE ID=@aid AND MusteriId=@mid"
                : @"SELECT TOP 1 Ad, Soyad, Telefon, Adres, Il, Ilce, PostaKodu FROM MusteriAdres
                    WHERE MusteriId=@mid ORDER BY Varsayilan DESC, ID DESC";

            using (var cmd = new SqlCommand(sql, b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@mid", musteriId);
                if (adresId.HasValue && adresId.Value > 0)
                    cmd.Parameters.AddWithValue("@aid", adresId.Value);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read())
                        return MusteriAuth.TeslimatBilgisiAl(musteriId);
                    return new DemoCheckoutAdres
                    {
                        Ad = rdr["Ad"]?.ToString() ?? "",
                        Soyad = rdr["Soyad"]?.ToString() ?? "",
                        Telefon = rdr["Telefon"]?.ToString() ?? "",
                        Email = email,
                        Adres = rdr["Adres"]?.ToString() ?? "",
                        Il = rdr["Il"]?.ToString() ?? "",
                        Ilce = rdr["Ilce"]?.ToString() ?? "",
                        PostaKodu = rdr["PostaKodu"]?.ToString() ?? ""
                    };
                }
            }
        }

        public static int Ekle(int musteriId, string baslik, string ad, string soyad, string telefon,
            string adres, string il, string ilce, string postaKodu, bool varsayilan)
        {
            if (musteriId <= 0 || string.IsNullOrWhiteSpace(adres)) return 0;

            var b = new BaglantiBilgileri();
            if (b.Baglanti.State == ConnectionState.Closed) b.Baglanti.Open();

            if (varsayilan)
            {
                using (var cmd = new SqlCommand(
                    "UPDATE MusteriAdres SET Varsayilan=0 WHERE MusteriId=@mid", b.Baglanti))
                {
                    cmd.Parameters.AddWithValue("@mid", musteriId);
                    cmd.ExecuteNonQuery();
                }
            }

            using (var cmd = new SqlCommand(@"
INSERT INTO MusteriAdres (MusteriId, Baslik, Ad, Soyad, Telefon, Adres, Il, Ilce, PostaKodu, Varsayilan, KayitTarihi)
VALUES (@mid, @baslik, @ad, @soyad, @tel, @adres, @il, @ilce, @pk, @var, GETDATE());
SELECT CAST(SCOPE_IDENTITY() AS INT);", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@mid", musteriId);
                cmd.Parameters.AddWithValue("@baslik", string.IsNullOrWhiteSpace(baslik) ? "Adres" : baslik.Trim());
                cmd.Parameters.AddWithValue("@ad", string.IsNullOrWhiteSpace(ad) ? (object)DBNull.Value : ad.Trim());
                cmd.Parameters.AddWithValue("@soyad", string.IsNullOrWhiteSpace(soyad) ? (object)DBNull.Value : soyad.Trim());
                cmd.Parameters.AddWithValue("@tel", string.IsNullOrWhiteSpace(telefon) ? (object)DBNull.Value : telefon.Trim());
                cmd.Parameters.AddWithValue("@adres", adres.Trim());
                cmd.Parameters.AddWithValue("@il", string.IsNullOrWhiteSpace(il) ? (object)DBNull.Value : il.Trim());
                cmd.Parameters.AddWithValue("@ilce", string.IsNullOrWhiteSpace(ilce) ? (object)DBNull.Value : ilce.Trim());
                cmd.Parameters.AddWithValue("@pk", string.IsNullOrWhiteSpace(postaKodu) ? (object)DBNull.Value : postaKodu.Trim());
                cmd.Parameters.AddWithValue("@var", varsayilan);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static bool Sil(int musteriId, int adresId)
        {
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                "DELETE FROM MusteriAdres WHERE ID=@aid AND MusteriId=@mid", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@aid", adresId);
                cmd.Parameters.AddWithValue("@mid", musteriId);
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool VarsayilanYap(int musteriId, int adresId)
        {
            var b = new BaglantiBilgileri();
            if (b.Baglanti.State == ConnectionState.Closed) b.Baglanti.Open();
            using (var tr = b.Baglanti.BeginTransaction())
            {
                try
                {
                    using (var cmd = new SqlCommand(
                        "UPDATE MusteriAdres SET Varsayilan=0 WHERE MusteriId=@mid", b.Baglanti, tr))
                    {
                        cmd.Parameters.AddWithValue("@mid", musteriId);
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = new SqlCommand(
                        "UPDATE MusteriAdres SET Varsayilan=1 WHERE ID=@aid AND MusteriId=@mid", b.Baglanti, tr))
                    {
                        cmd.Parameters.AddWithValue("@aid", adresId);
                        cmd.Parameters.AddWithValue("@mid", musteriId);
                        cmd.ExecuteNonQuery();
                    }
                    tr.Commit();
                    return true;
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }

        public static string DropdownMetin(DataRow row)
        {
            if (row == null) return "";
            string baslik = EncodingHelper.DuzeltTurkce(row["Baslik"]?.ToString() ?? "Adres");
            string ilce = EncodingHelper.DuzeltTurkce(row["Ilce"]?.ToString() ?? "");
            string il = EncodingHelper.DuzeltTurkce(row["Il"]?.ToString() ?? "");
            string yer = ilce;
            if (!string.IsNullOrWhiteSpace(il))
                yer += (yer.Length > 0 ? ", " : "") + il;
            return string.IsNullOrWhiteSpace(yer) ? baslik : baslik + " — " + yer;
        }
    }
}
