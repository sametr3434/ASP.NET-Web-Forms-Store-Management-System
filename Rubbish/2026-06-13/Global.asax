<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>

<script RunAt="server">


    void Application_Start(object sender, EventArgs e)
    {
        RegisterRoutes(RouteTable.Routes);
        var bundles = BundleTable.Bundles;

        //var scrLoginJs = new ScriptBundle("~/AdminTuzla/assets/js/jsLogin");
        //scrLoginJs.Include("~/AdminTuzla/assets/js/main.js");
        //bundles.Add(scrLoginJs);

        //var scrLoginCss = new StyleBundle("~/AdminTuzla/assets/css/cssLogin");
        //scrLoginCss.Include("~/AdminTuzla/assets/css/main.css");
        //bundles.Add(scrLoginCss);

        BundleTable.EnableOptimizations = true;
    }

    void Application_End(object sender, EventArgs e)
    {

    }

    void Application_Error(object sender, EventArgs e)
    {

    }

    void Session_Start(object sender, EventArgs e)
    {

    }

    void Session_End(object sender, EventArgs e)
    {

    }
    void RegisterRoutes(RouteCollection routes)
    {

        routes.Ignore("{parent}/{resource}.axd");
        
                
        routes.MapPageRoute("anasayfa",
                            "anasayfa",
                            "~/Default.aspx");

        routes.MapPageRoute("urunler",
                            "urunler",
                            "~/Urunler.aspx");

        routes.MapPageRoute("urundetay",
                            "urundetay/{ID}/{Baslik}",
                            "~/UrunDetay.aspx");

        routes.MapPageRoute("blog",
                            "blog",
                            "~/Blog.aspx");

        routes.MapPageRoute("blogdetay",
                            "blogdetay/{ID}/{Baslik}",
                            "~/BlogDetay.aspx");

        routes.MapPageRoute("hakkimizda",
                            "hakkimizda",
                            "~/Hakkimizda.aspx");

        routes.MapPageRoute("iletisim",
                            "iletisim",
                            "~/Iletisim.aspx");

        routes.MapPageRoute("teslimatbilgileri",
                            "teslimatbilgileri",
                            "~/TeslimatBilgileri.aspx");

        routes.MapPageRoute("gizlilikpolitikasi",
                            "gizlilikpolitikasi",
                            "~/GizlilikPolitikasi.aspx");

        routes.MapPageRoute("misyon",
                            "misyon",
                            "~/Misyon.aspx");

        routes.MapPageRoute("vizyon",
                            "vizyon",
                            "~/Vizyon.aspx");

        routes.MapPageRoute("kampanyalar",
                            "kampanyalar",
                            "~/Kampanyalar.aspx");

        routes.MapPageRoute("kullanimkosullari",
                            "kullanimkosullari",
                            "~/KullanimKosullari.aspx");

        routes.MapPageRoute("siparistakip",
                            "siparistakip",
                            "~/SiparisTakip.aspx");        
    }
       
</script>
