using Saat.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saat.Saat0604
{
    public partial class KuponListe : Page
    {
        public string Duzelt(object o)
        {
            return o == null || o == DBNull.Value ? "" : EncodingHelper.DuzeltTurkce(o.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindKuponlar();
            }
        }

        private void BindKuponlar()
        {
            BaglantiBilgileri b = new BaglantiBilgileri();
            using (SqlCommand com = new SqlCommand(@"SELECT ID, KuponKodu, IndirimTipi, IndirimDegeri, MinSiparisTutari, BaslangicTarihi, BitisTarihi, KullanimLimiti, KullanilanAdet, Aktif FROM Kupon ORDER BY ID DESC", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                DataTable dt = new DataTable();
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
                rptKuponlar.DataSource = dt;
                rptKuponlar.DataBind();
            }
        }

        protected void rptKuponlar_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Sil")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                BaglantiBilgileri b = new BaglantiBilgileri();
                using (SqlCommand com = new SqlCommand("DELETE FROM Kupon WHERE ID=@ID", b.Baglanti))
                {
                    com.Parameters.AddWithValue("@ID", id);
                    if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                    com.ExecuteNonQuery();
                    com.Connection.Close();
                }
                BindKuponlar();
            }
        }
    }
}
