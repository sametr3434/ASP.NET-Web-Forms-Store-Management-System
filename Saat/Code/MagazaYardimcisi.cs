using System;
using System.Web;

namespace Saat.App_Code
{
    /// <summary>
    /// \u00d6n y\u00fcz oturum (misafir sepeti) ve para birimi \u00e7erez.
    /// </summary>
    public static class MagazaYardimcisi
    {
        public const string SessionOturumAnahtari = "MagazaOturumId";
        public const string CookieOturumId = "MagazaOturumId";
        public const string CookieParaBirimi = "MagazaParaBirimi";

        /// <summary>
        /// Misafir sepet kimligini COOKIE'den okur (yoksa olusturur ve yazar).
        /// Session degil cookie kullanilir: session sona erdiginde sepet kaybolmasin.
        /// </summary>
        public static string OturumIdAl(HttpContext ctx)
        {
            if (ctx == null) return Guid.NewGuid().ToString("N");

            // Oncelik: cookie
            var gelen = ctx.Request?.Cookies[CookieOturumId];
            string id = gelen?.Value;

            // Gecerli GUID degilse yeni olustur
            if (string.IsNullOrWhiteSpace(id) || id.Length < 16)
            {
                id = Guid.NewGuid().ToString("N");
                var ck = new HttpCookie(CookieOturumId, id)
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(90),
                    Path = "/"
                };
                ctx.Response?.Cookies.Set(ck);
            }

            // Session'a da kopyala (SepetHelper.MisafirSepetiMerge SessionOturumAnahtari okur)
            if (ctx.Session != null)
                ctx.Session[SessionOturumAnahtari] = id;

            return id;
        }

        public static string ParaBirimiOku(HttpRequest req)
        {
            if (req == null) return ParaBirimiHelper.Try;
            var c = req.Cookies[CookieParaBirimi];
            if (c != null && !string.IsNullOrWhiteSpace(c.Value))
                return ParaBirimiHelper.Normalize(c.Value);
            return ParaBirimiHelper.Try;
        }

        public static void ParaBirimiYaz(HttpResponse resp, string kod)
        {
            if (resp == null) return;
            var v = ParaBirimiHelper.Normalize(kod);
            var ck = new HttpCookie(CookieParaBirimi, v)
            {
                HttpOnly = false,
                Expires = DateTime.UtcNow.AddDays(90),
                Path = "/"
            };
            resp.Cookies.Set(ck);
        }
    }
}
