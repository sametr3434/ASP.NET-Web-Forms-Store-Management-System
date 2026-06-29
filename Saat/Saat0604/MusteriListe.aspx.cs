
using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class MusteriListe : Page
    {
        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        public string GetTelefonHtml(object o)
        {
            if (o == null || o == DBNull.Value) return "-";
            var s = EncodingHelper.DuzeltTurkce(o.ToString());
            if (string.IsNullOrWhiteSpace(s)) return "-";
            return string.Format("<a href='tel:{0}'>{1}</a>", o.ToString(), s);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMusteriler();
            }
        }

        private void BindMusteriler()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(
                "SELECT ID, Ad, Soyad, EPosta, Telefon, Il, Ilce, KayitTarihi, Aktif FROM Musteri ORDER BY KayitTarihi DESC, ID DESC", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                var dt = new DataTable();
                dt.Load(com.ExecuteReader());
                com.Connection.Close();

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (row[col] != DBNull.Value && row[col] != null && col.DataType == typeof(string))
                            row[col] = EncodingHelper.DuzeltTurkce(row[col].ToString());
                    }
                }

                rptMusteriler.DataSource = dt;
                rptMusteriler.DataBind();

                pnlBos.Visible = dt.Rows.Count == 0;
            }
        }
    }
}
