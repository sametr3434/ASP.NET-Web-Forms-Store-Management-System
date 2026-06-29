using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

namespace Saat.App_Code
{
    public static class CacheHelper
    {
        // 1 saatlik cache suresi
        private static readonly int CacheDurationMinutes = 60;

        public static DataTable KategorileriGetir(BaglantiBilgileri b)
        {
            string cacheKey = "Kategoriler";
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (DataTable)HttpContext.Current.Cache[cacheKey];
            }

            DataTable dt = new DataTable();
            using (SqlCommand com = new SqlCommand("SELECT ID, KategoriAdi, UstId FROM Kategori WHERE Aktif=1 ORDER BY Siralama", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
            }

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Cache.Insert(cacheKey, dt, null, DateTime.Now.AddMinutes(CacheDurationMinutes), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable MarkalariGetir(BaglantiBilgileri b)
        {
            string cacheKey = "Markalar";
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (DataTable)HttpContext.Current.Cache[cacheKey];
            }

            DataTable dt = new DataTable();
            using (SqlCommand com = new SqlCommand("SELECT ID, MarkaAdi FROM Marka WHERE Aktif=1 ORDER BY Siralama", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
            }

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Cache.Insert(cacheKey, dt, null, DateTime.Now.AddMinutes(CacheDurationMinutes), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable SiteSabitleriGetir(BaglantiBilgileri b)
        {
            string cacheKey = "SiteSabitleri";
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (DataTable)HttpContext.Current.Cache[cacheKey];
            }

            DataTable dt = new DataTable();
            using (SqlCommand com = new SqlCommand("SELECT TOP 1 * FROM sitesabitleri", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();
            }

            if (dt.Rows.Count > 0)
            {
                HttpContext.Current.Cache.Insert(cacheKey, dt, null, DateTime.Now.AddMinutes(CacheDurationMinutes), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void CacheTemizle()
        {
            HttpContext.Current.Cache.Remove("Kategoriler");
            HttpContext.Current.Cache.Remove("Markalar");
            HttpContext.Current.Cache.Remove("SiteSabitleri");
        }
    }
}
