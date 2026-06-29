using System.Web.Routing;

namespace Saat.Routing
{
    /// <summary>
    /// \u00d6n y\u00fcz URL yollar\u0131. Kaynaklar AuramixCode/ alt\u0131nda (App_Code klas\u00f6r ad\u0131 CS0433 \u00e7ift derleme yarat\u0131r).
    /// </summary>
    public static class RouteConfig
    {
        public const string RouteAnasayfa = "anasayfa";
        public const string RouteUrunler = "urunler";
        public const string RouteUrun = "urun";
        /// <summary>SEO: urun/{id}/{slug} — once kayit (daha uzun URL once eslesir).</summary>
        public const string RouteUrunSlug = "urun_slug";
        public const string RouteSepet = "sepet";
        public const string RouteOdeme = "odeme";
        public const string RouteOdemeOde = "odeme_ode";
        public const string RouteOdemeBasarili = "odeme_basarili";
        public const string RouteOdemeBasarisiz = "odeme_basarisiz";
        public const string RouteParaBirimi = "para_birimi";
        public const string RouteHakkimizda = "hakkimizda";
        public const string RouteIletisim = "iletisim";
        public const string RoutePayTrCallback = "paytr_callback";
        /// <summary>PayTR_Callback.aspx i\u00e7in SEO dostu yol (Bildirim URL olarak kullan\u0131labilir).</summary>
        public const string RoutePayTrCallbackAlt = "paytr_callback_alt";
        public const string RouteUrunDetayLegacy = "urun_detay_legacy";
        public const string RouteUyeGiris = "uye_giris";
        public const string RouteUyeKayit = "uye_kayit";
        public const string RouteUyeCikis = "uye_cikis";
        public const string RouteHesabim = "hesabim";
        public const string RouteSiparisDetayim = "siparis_detayim";

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute(RouteAnasayfa, "anasayfa", "~/Default.aspx");
            routes.MapPageRoute(RouteAnasayfa + "_slash", "anasayfa/", "~/Default.aspx");

            routes.MapPageRoute(RouteUrunler, "urunler", "~/Urunler.aspx");
            routes.MapPageRoute(RouteUrunler + "_slash", "urunler/", "~/Urunler.aspx");

            routes.MapPageRoute(RouteUrunSlug, "urun/{id}/{*slug}", "~/UrunDetay.aspx");
            routes.MapPageRoute(RouteUrun, "urun/{id}", "~/UrunDetay.aspx");

            routes.MapPageRoute(RouteSepet, "sepet", "~/Sepet.aspx");
            routes.MapPageRoute(RouteSepet + "_slash", "sepet/", "~/Sepet.aspx");

            routes.MapPageRoute(RouteOdeme, "odeme", "~/Odeme.aspx");
            routes.MapPageRoute(RouteOdeme + "_slash", "odeme/", "~/Odeme.aspx");

            routes.MapPageRoute(RouteOdemeOde, "odeme-ode", "~/OdemeOde.aspx");
            routes.MapPageRoute(RouteOdemeOde + "_slash", "odeme-ode/", "~/OdemeOde.aspx");

            routes.MapPageRoute(RouteOdemeBasarili, "odeme-basarili", "~/OdemeBasarili.aspx");
            routes.MapPageRoute(RouteOdemeBasarili + "_slash", "odeme-basarili/", "~/OdemeBasarili.aspx");

            routes.MapPageRoute(RouteOdemeBasarisiz, "odeme-basarisiz", "~/OdemeBasarisiz.aspx");
            routes.MapPageRoute(RouteOdemeBasarisiz + "_slash", "odeme-basarisiz/", "~/OdemeBasarisiz.aspx");

            routes.MapPageRoute(RouteParaBirimi, "para-birimi", "~/ParaBirimi.aspx");
            routes.MapPageRoute(RouteParaBirimi + "_slash", "para-birimi/", "~/ParaBirimi.aspx");

            routes.MapPageRoute(RouteHakkimizda, "hakkimizda", "~/MagazaHakkimizda.aspx");
            routes.MapPageRoute(RouteHakkimizda + "_slash", "hakkimizda/", "~/MagazaHakkimizda.aspx");

            routes.MapPageRoute(RouteIletisim, "iletisim", "~/MagazaIletisim.aspx");
            routes.MapPageRoute(RouteIletisim + "_slash", "iletisim/", "~/MagazaIletisim.aspx");

            routes.MapPageRoute(RoutePayTrCallback, "paytr-bildirim", "~/PayTRCallback.aspx");
            routes.MapPageRoute(RoutePayTrCallback + "_slash", "paytr-bildirim/", "~/PayTRCallback.aspx");

            routes.MapPageRoute(RoutePayTrCallbackAlt, "paytr-callback", "~/PayTR_Callback.aspx");
            routes.MapPageRoute(RoutePayTrCallbackAlt + "_slash", "paytr-callback/", "~/PayTR_Callback.aspx");

            routes.MapPageRoute(RouteUrunDetayLegacy, "urundetay/{id}/{*slug}", "~/UrunDetay.aspx");

            routes.MapPageRoute(RouteUyeGiris, "uye-giris", "~/UyeGiris.aspx");
            routes.MapPageRoute(RouteUyeGiris + "_slash", "uye-giris/", "~/UyeGiris.aspx");

            routes.MapPageRoute(RouteUyeKayit, "uye-kayit", "~/UyeKayit.aspx");
            routes.MapPageRoute(RouteUyeKayit + "_slash", "uye-kayit/", "~/UyeKayit.aspx");

            routes.MapPageRoute(RouteUyeCikis, "uye-cikis", "~/UyeCikis.aspx");
            routes.MapPageRoute(RouteUyeCikis + "_slash", "uye-cikis/", "~/UyeCikis.aspx");

            routes.MapPageRoute(RouteHesabim, "hesabim", "~/Hesabim.aspx");
            routes.MapPageRoute(RouteHesabim + "_slash", "hesabim/", "~/Hesabim.aspx");

            routes.MapPageRoute(RouteSiparisDetayim, "siparisim/{id}", "~/SiparisDetayim.aspx");
        }
    }
}
