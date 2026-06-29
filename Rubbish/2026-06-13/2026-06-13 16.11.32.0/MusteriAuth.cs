using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.SessionState;

namespace Saat.App_Code
{
    /// <summary>
    /// M\u00fc\u015fteri kimlik do\u011frulama: PBKDF2 hash + oturum y\u00f6netimi.
    /// </summary>
    public static class MusteriAuth
    {
        private const string SessionKimlik = "SaatMusteriId";
        private const string SessionAdSoyad = "SaatMusteriAdSoyad";
        private const int Iterations = 10000;

        // ── Hash / Dogrulama ──────────────────────────────────────────────

        /// <summary>PBKDF2-SHA1 (RFC2898): version(1) + salt(16) + hash(20) = 37 byte → Base64.</summary>
        public static string HashOlustur(string sifre)
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);
            byte[] hash = new Rfc2898DeriveBytes(sifre, salt, Iterations).GetBytes(20);
            byte[] combined = new byte[37];
            combined[0] = 1;
            Buffer.BlockCopy(salt, 0, combined, 1, 16);
            Buffer.BlockCopy(hash, 0, combined, 17, 20);
            return Convert.ToBase64String(combined);
        }

        public static bool HashDogrula(string sifre, string hashStr)
        {
            if (string.IsNullOrEmpty(sifre) || string.IsNullOrEmpty(hashStr)) return false;
            try
            {
                byte[] combined = Convert.FromBase64String(hashStr);
                if (combined.Length != 37 || combined[0] != 1) return false;
                byte[] salt = new byte[16];
                Buffer.BlockCopy(combined, 1, salt, 0, 16);
                byte[] expected = new byte[20];
                Buffer.BlockCopy(combined, 17, expected, 0, 20);
                byte[] actual = new Rfc2898DeriveBytes(sifre, salt, Iterations).GetBytes(20);
                uint diff = 0;
                for (int i = 0; i < 20; i++) diff |= (uint)(expected[i] ^ actual[i]);
                return diff == 0;
            }
            catch { return false; }
        }

        // ── Oturum ───────────────────────────────────────────────────────

        public static void OturumAc(HttpSessionState session, int id, string adSoyad)
        {
            session[SessionKimlik] = id;
            session[SessionAdSoyad] = adSoyad;
        }

        public static void OturumKapat(HttpSessionState session)
        {
            session.Remove(SessionKimlik);
            session.Remove(SessionAdSoyad);
        }

        public static bool GirisMi(HttpSessionState session)
        {
            if (session == null) return false;

            object v = session[SessionKimlik];
            if (v == null) return false;

            int id;
            if (int.TryParse(v.ToString(), out id))
                return id > 0;

            return false;
        }

        public static int MusteriId(HttpSessionState session)
        {
            if (session == null) return 0;

            object v = session[SessionKimlik];
            if (v == null) return 0;

            int id;
            return int.TryParse(v.ToString(), out id) ? id : 0;
        }

        public static string MusteriAdSoyad(HttpSessionState session)
        {
            if (session == null) return "";
            return session[SessionAdSoyad]?.ToString() ?? "";
        }

        // ── Veritaban\u0131 ─────────────────────────────────────────────────────

        /// <summary>
        /// Giri\u015f denemesi. D\u00f6n\u00fc\u015f: >0 = MusteriId (ba\u015far\u0131l\u0131), -1 = pasif hesap, 0 = hatal\u0131 bilgi.
        /// </summary>
        public static int GirisYap(string kullaniciVeyaEposta, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciVeyaEposta)) return 0;
            string giris = kullaniciVeyaEposta.Trim().ToLowerInvariant();
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                @"SELECT ID, Sifre, Aktif FROM Musteri
                  WHERE LOWER(LTRIM(RTRIM(EPosta)))=@giris
                     OR LOWER(LTRIM(RTRIM(ISNULL(KullaniciAdi, N''))))=@giris", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@giris", giris);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return 0;
                    int id = (int)rdr["ID"];
                    bool aktif = rdr["Aktif"] != DBNull.Value && (bool)rdr["Aktif"];
                    string hash = rdr["Sifre"] == DBNull.Value ? "" : rdr["Sifre"].ToString();
                    rdr.Close();
                    if (!aktif) return -1;
                    if (!HashDogrula(sifre, hash)) return 0;
                    using (var u = new SqlCommand(
                        "UPDATE Musteri SET SonGirisTarihi=@t WHERE ID=@id", cmd.Connection))
                    {
                        u.Parameters.AddWithValue("@t", DateTime.Now);
                        u.Parameters.AddWithValue("@id", id);
                        u.ExecuteNonQuery();
                    }
                    return id;
                }
            }
        }

        public static string AdSoyadAl(int musteriId)
        {
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                "SELECT Ad, Soyad FROM Musteri WHERE ID=@id", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", musteriId);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return "";
                    string ad = EncodingHelper.DuzeltTurkce(rdr["Ad"].ToString().Trim());
                    string soyad = EncodingHelper.DuzeltTurkce(rdr["Soyad"].ToString().Trim());
                    return (ad + " " + soyad).Trim();
                }
            }
        }

        public static bool EPostaKayitli(string eposta)
        {
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Musteri WHERE EPosta=@ep", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@ep", eposta.Trim().ToLowerInvariant());
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public static bool KullaniciAdiKayitli(string kullaniciAdi)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi)) return false;
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Musteri WHERE LOWER(LTRIM(RTRIM(KullaniciAdi)))=@ka", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@ka", kullaniciAdi.Trim().ToLowerInvariant());
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        /// <summary>Yeni m\u00fc\u015fteri kaydeder. D\u00f6n\u00fc\u015f: yeni MusteriId (>0) veya 0.</summary>
        public static int Kayit(string ad, string soyad, string eposta, string telefon, string sifre, string kullaniciAdi = null)
        {
            string hash = HashOlustur(sifre);
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                @"INSERT INTO Musteri(Ad,Soyad,EPosta,Telefon,Sifre,KullaniciAdi,KayitTarihi,Aktif)
                  VALUES(@ad,@soyad,@ep,@tel,@sifre,@ka,@kt,1);
                  SELECT SCOPE_IDENTITY();", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@ad", ad.Trim());
                cmd.Parameters.AddWithValue("@soyad", soyad.Trim());
                cmd.Parameters.AddWithValue("@ep", eposta.Trim().ToLowerInvariant());
                cmd.Parameters.AddWithValue("@tel",
                    string.IsNullOrWhiteSpace(telefon) ? (object)DBNull.Value : telefon.Trim());
                cmd.Parameters.AddWithValue("@sifre", hash);
                cmd.Parameters.AddWithValue("@ka",
                    string.IsNullOrWhiteSpace(kullaniciAdi) ? (object)DBNull.Value : kullaniciAdi.Trim());
                cmd.Parameters.AddWithValue("@kt", DateTime.Now);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                var r = cmd.ExecuteScalar();
                return r == null || r == DBNull.Value ? 0 : Convert.ToInt32(r);
            }
        }

        /// <summary>M\u00fc\u015fteri bilgilerini g\u00fcnceller (admin eri\u015fimi olmadan, sifre bos birakilirsa degismez).</summary>
        public static bool BilgileriGuncelle(int musteriId, string ad, string soyad, string eposta,
            string telefon, string adres, string il, string ilce, string postaKodu,
            string yeniSifre = null)
        {
            var b = new BaglantiBilgileri();
            string sql = @"UPDATE Musteri SET Ad=@ad, Soyad=@soyad, EPosta=@ep, Telefon=@tel,
                           Adres=@adres, Il=@il, Ilce=@ilce, PostaKodu=@pk" +
                (string.IsNullOrWhiteSpace(yeniSifre) ? "" : ", Sifre=@sifre") +
                " WHERE ID=@id";
            using (var cmd = new SqlCommand(sql, b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@ad", ad.Trim());
                cmd.Parameters.AddWithValue("@soyad", soyad.Trim());
                cmd.Parameters.AddWithValue("@ep", eposta.Trim().ToLowerInvariant());
                cmd.Parameters.AddWithValue("@tel",
                    string.IsNullOrWhiteSpace(telefon) ? (object)DBNull.Value : telefon.Trim());
                cmd.Parameters.AddWithValue("@adres",
                    string.IsNullOrWhiteSpace(adres) ? (object)DBNull.Value : adres.Trim());
                cmd.Parameters.AddWithValue("@il",
                    string.IsNullOrWhiteSpace(il) ? (object)DBNull.Value : il.Trim());
                cmd.Parameters.AddWithValue("@ilce",
                    string.IsNullOrWhiteSpace(ilce) ? (object)DBNull.Value : ilce.Trim());
                cmd.Parameters.AddWithValue("@pk",
                    string.IsNullOrWhiteSpace(postaKodu) ? (object)DBNull.Value : postaKodu.Trim());
                if (!string.IsNullOrWhiteSpace(yeniSifre))
                    cmd.Parameters.AddWithValue("@sifre", HashOlustur(yeniSifre));
                cmd.Parameters.AddWithValue("@id", musteriId);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ── Beni Hatırla (persistent cookie) ─────────────────────────────────

        public const string CookieHatirla = "AuramixHatirla";
        private const int HatirlaSuruGun = 30;

        /// <summary>
        /// "Beni hat\u0131rla" i\u015faretlendi\u011finde \u00e7a\u011fr\u0131l\u0131r.
        /// G\u00fcvenli rastgele token \u00fcretir, DB'ye yazar, 30 g\u00fcnl\u00fck cookie set eder.
        /// </summary>
        public static void HatirlaAyarla(HttpResponse response, int musteriId)
        {
            var tokenBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(tokenBytes);
            string token = Convert.ToBase64String(tokenBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');

            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                "UPDATE Musteri SET HatirlaToken=@t, HatirlaTokenSona=@s WHERE ID=@id",
                b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@t", token);
                cmd.Parameters.AddWithValue("@s", DateTime.UtcNow.AddDays(HatirlaSuruGun));
                cmd.Parameters.AddWithValue("@id", musteriId);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed) cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }

            var cookie = new HttpCookie(CookieHatirla, musteriId + "|" + token)
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(HatirlaSuruGun),
                Path = "/"
            };
            response.Cookies.Set(cookie);
        }

        /// <summary>
        /// Cookie varsa do\u011frular, ge\u00e7erliyse MusteriId d\u00f6nd\u00fcr\u00fcr; de\u011filse 0.
        /// </summary>
        public static int HatirlaDogrula(HttpRequest request)
        {
            var cookie = request?.Cookies[CookieHatirla];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value)) return 0;

            var parts = cookie.Value.Split('|');
            if (parts.Length != 2) return 0;
            int mid;
            if (!int.TryParse(parts[0], out mid) || mid <= 0)
                return 0;
            string token = parts[1];

            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                "SELECT ID FROM Musteri WHERE ID=@id AND HatirlaToken=@t AND HatirlaTokenSona>GETUTCDATE() AND Aktif=1",
                b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", mid);
                cmd.Parameters.AddWithValue("@t", token);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed) cmd.Connection.Open();
                var r = cmd.ExecuteScalar();
                return r != null && r != DBNull.Value ? Convert.ToInt32(r) : 0;
            }
        }

        /// <summary>
        /// \u00c7\u0131k\u0131\u015fta DB token'\u0131 ve cookie'yi temizler.
        /// </summary>
        public static DemoCheckoutAdres TeslimatBilgisiAl(int musteriId)
        {
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                "SELECT Ad, Soyad, Telefon, EPosta, Adres, Il, Ilce FROM Musteri WHERE ID=@id", b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", musteriId);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return null;
                    return new DemoCheckoutAdres
                    {
                        Ad = rdr["Ad"]?.ToString() ?? "",
                        Soyad = rdr["Soyad"]?.ToString() ?? "",
                        Telefon = rdr["Telefon"]?.ToString() ?? "",
                        Email = rdr["EPosta"]?.ToString() ?? "",
                        Adres = rdr["Adres"]?.ToString() ?? "",
                        Il = rdr["Il"]?.ToString() ?? "",
                        Ilce = rdr["Ilce"]?.ToString() ?? ""
                    };
                }
            }
        }

        /// <summary>Giri\u015f zorunlu sayfalar i\u00e7in y\u00f6nlendirme.</summary>
        public static void GirisGerekli(HttpContext ctx, string returnUrl = null)
        {
            if (ctx == null) return;
            if (GirisMi(ctx.Session)) return;
            string hedef = "/uye-giris";
            if (!string.IsNullOrWhiteSpace(returnUrl))
                hedef += "?returnUrl=" + HttpUtility.UrlEncode(returnUrl);
            ctx.Response.Redirect(hedef, false);
            ctx.ApplicationInstance.CompleteRequest();
        }

        public static void HatirlaSil(HttpRequest request, HttpResponse response, int musteriId)
        {
            // Cookie temizle
            var cookie = new HttpCookie(CookieHatirla, "")
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(-1),
                Path = "/"
            };
            response.Cookies.Set(cookie);

            // DB temizle
            if (musteriId <= 0) return;
            var b = new BaglantiBilgileri();
            using (var cmd = new SqlCommand(
                "UPDATE Musteri SET HatirlaToken=NULL, HatirlaTokenSona=NULL WHERE ID=@id",
                b.Baglanti))
            {
                cmd.Parameters.AddWithValue("@id", musteriId);
                if (cmd.Connection.State == System.Data.ConnectionState.Closed) cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
