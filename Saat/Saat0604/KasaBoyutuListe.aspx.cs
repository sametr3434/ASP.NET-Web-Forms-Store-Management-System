using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class KasaBoyutuListe : Page
    {
        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindKasaBoyutlari();
            }
        }

        private void BindKasaBoyutlari()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT ID, kasano, Siralama, Aktif FROM kasaboyutu ORDER BY Siralama ASC, kasano ASC", b.Baglanti))
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
                rptKasaBoyutlari.DataSource = dt;
                rptKasaBoyutlari.DataBind();
            }
        }

        protected void rptKasaBoyutlari_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("KasaBoyutuSil.aspx?id=" + id);
            }
        }
    }
}
