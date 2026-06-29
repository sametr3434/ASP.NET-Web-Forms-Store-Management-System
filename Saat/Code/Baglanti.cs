using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Saat.App_Code
{
    public class BaglantiBilgileri : IDisposable
    {
        private SqlConnection _baglanti;
        private bool _disposed;

        public SqlConnection Baglanti
        {
            get { return _baglanti; }
        }

        public BaglantiBilgileri()
        {
            var cs = ConfigurationManager.ConnectionStrings["TuzlaTasarim"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
                cs = "Data Source=YOUR_SQL_SERVER;Initial Catalog=YOUR_DATABASE;Persist Security Info=True;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD";
            _baglanti = new SqlConnection(cs);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            if (_baglanti == null) return;
            try
            {
                if (_baglanti.State != ConnectionState.Closed)
                    _baglanti.Close();
            }
            finally
            {
                _baglanti.Dispose();
                _baglanti = null;
            }
        }
    }
}
