using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class MarkaListe : Page
    {
        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMarkalar();
            }
        }

        private void BindMarkalar()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand("SELECT ID, MarkaAdi, Aciklama, Siralama, Aktif FROM Marka ORDER BY Siralama ASC, MarkaAdi ASC", b.Baglanti))
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
                rptMarkalar.DataSource = dt;
                rptMarkalar.DataBind();
            }
        }

        protected void rptMarkalar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("MarkaSil.aspx?id=" + id);
            }
        }
    }
}
