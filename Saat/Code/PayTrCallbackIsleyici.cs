using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;

namespace Saat.App_Code
{
    /// <summary>
    /// PayTR 2. ad\u0131m bildirim URL i\u015f mant\u0131\u011f\u0131 (BeyazOrkide testsonuc / PaytrBilgi ile ayn\u0131 hash do\u011frulamas\u0131: PayTrIframeService.CallbackHashDogru).
    /// </summary>
    public static class PayTrCallbackIsleyici
    {
        public static void Calistir(HttpContext ctx)
        {
            if (ctx == null) return;

            var Response = ctx.Response;
            var Request = ctx.Request;

            Response.Clear();
            Response.ContentType = "text/plain";
            if (!Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                Response.StatusCode = 405;
                Response.End();
                return;
            }

            string merchantOid = Request.Form["merchant_oid"];
            string status = Request.Form["status"];
            string totalAmount = Request.Form["total_amount"];
            string hash = Request.Form["hash"];
            if (string.IsNullOrEmpty(merchantOid) || string.IsNullOrEmpty(status) || string.IsNullOrEmpty(totalAmount) || string.IsNullOrEmpty(hash))
            {
                Response.Write("OK");
                Response.End();
                return;
            }

            var b = new BaglantiBilgileri();
            string mKey = "", mSalt = "";
            using (var com = new SqlCommand("SELECT PayTR_MagazaParola, PayTR_MagazaGizliAnahtar FROM sitesabitleri WHERE ID=1", b.Baglanti))
            {
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                using (var dr = com.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        mSalt = dr["PayTR_MagazaParola"] != DBNull.Value ? dr["PayTR_MagazaParola"].ToString() : "";
                        mKey = dr["PayTR_MagazaGizliAnahtar"] != DBNull.Value ? dr["PayTR_MagazaGizliAnahtar"].ToString() : "";
                    }
                }
                com.Connection.Close();
            }

            if (!PayTrIframeService.CallbackHashDogru(mKey, mSalt, merchantOid, status, totalAmount, hash.Trim()))
            {
                Response.StatusCode = 400;
                Response.Write("hash");
                Response.End();
                return;
            }

            int siparisId = 0;
            decimal odemeTry = 0;
            string odemeDurumu = "";
            using (var com = new SqlCommand("SELECT ID, OdemeTutarTRY, OdemeDurumu FROM Siparis WHERE SiparisNo=@No", b.Baglanti))
            {
                com.Parameters.AddWithValue("@No", merchantOid);
                if (com.Connection.State == ConnectionState.Closed) com.Connection.Open();
                using (var dr = com.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        Response.Write("OK");
                        Response.End();
                        return;
                    }
                    siparisId = Convert.ToInt32(dr["ID"]);
                    odemeTry = dr["OdemeTutarTRY"] != DBNull.Value ? Convert.ToDecimal(dr["OdemeTutarTRY"]) : 0m;
                    odemeDurumu = dr["OdemeDurumu"] != DBNull.Value ? dr["OdemeDurumu"].ToString() : "";
                }
                com.Connection.Close();
            }

            if (odemeDurumu.IndexOf("\u00d6dendi", StringComparison.OrdinalIgnoreCase) >= 0
                || odemeDurumu.IndexOf("Odendi", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Response.Write("OK");
                Response.End();
                return;
            }

            int beklenenKurus = (int)Math.Round(odemeTry * 100m, 0, MidpointRounding.AwayFromZero);
            int gelenKurus;
            if (!int.TryParse(totalAmount, NumberStyles.Integer, CultureInfo.InvariantCulture, out gelenKurus))
                gelenKurus = 0;

            if (string.Equals(status, "success", StringComparison.OrdinalIgnoreCase))
            {
                if (beklenenKurus > 0 && gelenKurus < beklenenKurus)
                {
                    Response.Write("OK");
                    Response.End();
                    return;
                }
                using (var conn = b.Baglanti)
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (var tr = conn.BeginTransaction())
                    {
                        try
                        {
                            using (var upS = new SqlCommand("UPDATE Siparis SET OdemeDurumu=@O WHERE ID=@ID", conn, tr))
                            {
                                upS.Parameters.AddWithValue("@O", "\u00d6dendi");
                                upS.Parameters.AddWithValue("@ID", siparisId);
                                upS.ExecuteNonQuery();
                            }
                            using (var upO = new SqlCommand("UPDATE OdemeKaydi SET OdemeDurumu=@O, OdemeTarihi=GETDATE() WHERE SiparisId=@ID", conn, tr))
                            {
                                upO.Parameters.AddWithValue("@O", "\u00d6dendi");
                                upO.Parameters.AddWithValue("@ID", siparisId);
                                upO.ExecuteNonQuery();
                            }
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                string failMsg = Request.Form["failed_reason_msg"] ?? "";
                if (failMsg.Length > 500) failMsg = failMsg.Substring(0, 500);
                using (var upO = new SqlCommand("UPDATE OdemeKaydi SET OdemeDurumu=@O, HataMesaji=@H WHERE SiparisId=@ID", b.Baglanti))
                {
                    upO.Parameters.AddWithValue("@O", "Basarisiz");
                    upO.Parameters.AddWithValue("@H", failMsg);
                    upO.Parameters.AddWithValue("@ID", siparisId);
                    if (upO.Connection.State == ConnectionState.Closed) upO.Connection.Open();
                    upO.ExecuteNonQuery();
                    upO.Connection.Close();
                }
            }

            Response.Write("OK");
            Response.End();
        }
    }
}
