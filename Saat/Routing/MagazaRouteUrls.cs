using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Saat.App_Code;

namespace Saat.Routing
{
    /// <summary>
    /// \u00d6n y\u00fczde RouteTable URL \u00fcretimi; rota yoksa .aspx yede\u011fi.
    /// </summary>
    public static class MagazaRouteUrls
    {
        public static string Href(Page page, string routeName, string fallbackVirtualPath)
        {
            if (page == null)
                return fallbackVirtualPath ?? "";
            try
            {
                var vp = page.GetRouteUrl(routeName, (object)null);
                if (!string.IsNullOrEmpty(vp))
                    return page.ResolveUrl(vp);
            }
            catch
            {
                /* yedek */
            }
            return page.ResolveUrl(fallbackVirtualPath ?? "~/");
        }

        /// <summary>Urun adi verilirse SEO URL: /urun/{id}/{slug}; aksi kisa /urun/{id}.</summary>
        public static string UrunDetay(Page page, object urunIdObj, object urunAdiObj = null)
        {
            if (page == null)
                return "~/Urunler.aspx";
            int id;
            if (urunIdObj == null || urunIdObj == DBNull.Value ||
                !int.TryParse(urunIdObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out id) || id <= 0)
                return page.ResolveUrl("~/Urunler.aspx");

            string slug = null;
            if (urunAdiObj != null && urunAdiObj != DBNull.Value)
            {
                var raw = urunAdiObj.ToString();
                if (!string.IsNullOrWhiteSpace(raw))
                    slug = UrunSeoSlug.Olustur(raw);
            }

            if (!string.IsNullOrEmpty(slug))
            {
                try
                {
                    var vp = page.GetRouteUrl(RouteConfig.RouteUrunSlug, new { id, slug });
                    if (!string.IsNullOrEmpty(vp))
                        return page.ResolveUrl(vp);
                }
                catch
                {
                    /* kisa rota */
                }
            }

            try
            {
                var vp2 = page.GetRouteUrl(RouteConfig.RouteUrun, new { id });
                if (!string.IsNullOrEmpty(vp2))
                    return page.ResolveUrl(vp2);
            }
            catch
            {
                /* yedek */
            }
            return page.ResolveUrl("~/UrunDetay.aspx?id=" + id.ToString(CultureInfo.InvariantCulture));
        }

        public static string ParaBirimi(Page page, string pb, string returnRawUrl)
        {
            var baseUrl = Href(page, RouteConfig.RouteParaBirimi, "~/ParaBirimi.aspx");
            var q = "?pb=" + HttpUtility.UrlEncode(pb ?? "TRY");
            if (!string.IsNullOrEmpty(returnRawUrl))
                q += "&return=" + HttpUtility.UrlEncode(returnRawUrl);
            return baseUrl + q;
        }

        public static string ParaBirimi(Page page, string pb)
        {
            return ParaBirimi(page, pb, null);
        }
    }
}
