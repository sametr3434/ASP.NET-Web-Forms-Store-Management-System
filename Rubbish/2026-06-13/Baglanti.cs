using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

namespace Saat.App_Code
{
    public class BaglantiBilgileri //public ifadesi projenin her yerinden erişim sağlar
    {
        private SqlConnection _Baglanti; //bu forma özel private bir bağlantı tanımlaması yap

        public SqlConnection Baglanti
        {
            get { return _Baglanti; } //hazırlanmış bilgi sql connection özelliğiyle fırlatılıyor
        }

        public BaglantiBilgileri()
        {
            this._Baglanti = new SqlConnection("server=mssql.tuzlatasarim.com.tr;Initial Catalog=admin_saat;User ID=saat;Password=49#u4wb8Ab9Tb7!1x2"); //veritabanı bağlantı bilgilerinin tanımlaması
        }
    }
}