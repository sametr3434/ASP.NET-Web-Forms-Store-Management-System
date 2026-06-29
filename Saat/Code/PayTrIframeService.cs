using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace Saat.App_Code
{
    /// <summary>
    /// PayTR iFrame 1. ad\u0131m: get-token. Ma\u011faza: sitesabitleri PayTR_MagazaNo / Parola (salt) / GizliAnahtar (key).
    /// </summary>
    public static class PayTrIframeService
    {
        const string GetTokenUrl = "https://www.paytr.com/odeme/api/get-token";

        public class TokenSonuc
        {
            public bool Basarili { get; set; }
            public string Token { get; set; }
            public string Mesaj { get; set; }
        }

        /// <summary>
        /// \u00d6deme tutar\u0131 TL cinsinden (PayTR payment_amount i\u00e7in kuru\u015fa \u00e7evrilir). Sepet her zaman TL sat\u0131rlar\u0131yla g\u00f6nderilir.
        /// </summary>
        public static TokenSonuc TokenAl(
            string merchantId, string merchantKey, string merchantSalt,
            string userIp, string merchantOid, string email,
            decimal tutarTL, int noInstallment, int maxInstallment,
            string currency, int testMode,
            string userName, string userAddress, string userPhone,
            string merchantOkUrl, string merchantFailUrl,
            IList<Tuple<string, decimal, int>> sepetSatirlariTL,
            int debugOn = 1, int timeoutLimit = 30, string lang = "tr")
        {
            var sonuc = new TokenSonuc();
            if (string.IsNullOrWhiteSpace(merchantId) || string.IsNullOrWhiteSpace(merchantKey) || string.IsNullOrWhiteSpace(merchantSalt))
            {
                sonuc.Mesaj = "PayTR ma\u011faza bilgileri eksik.";
                return sonuc;
            }

            int paymentAmount = (int)Math.Round(tutarTL * 100m, 0, MidpointRounding.AwayFromZero);
            if (paymentAmount <= 0)
            {
                sonuc.Mesaj = "Ge\u00e7ersiz \u00f6deme tutar\u0131.";
                return sonuc;
            }

            var basketArr = sepetSatirlariTL.Select(x => new object[] { x.Item1, x.Item2.ToString("F2", CultureInfo.InvariantCulture), x.Item3 }).ToList();
            var userBasket = Convert.ToBase64String(Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(basketArr)));

            string cur = string.IsNullOrWhiteSpace(currency) ? "TL" : currency.Trim().ToUpperInvariant();
            if (cur == "TRY") cur = "TL";

            string hashStr = string.Concat(merchantId, userIp, merchantOid, email, paymentAmount.ToString(CultureInfo.InvariantCulture), userBasket,
                noInstallment.ToString(CultureInfo.InvariantCulture), maxInstallment.ToString(CultureInfo.InvariantCulture), cur, testMode.ToString(CultureInfo.InvariantCulture));
            string paytrToken;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchantKey)))
            {
                var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(hashStr + merchantSalt));
                paytrToken = Convert.ToBase64String(bytes);
            }

            var post = new Dictionary<string, string>
            {
                { "merchant_id", merchantId },
                { "user_ip", userIp ?? "127.0.0.1" },
                { "merchant_oid", merchantOid },
                { "email", email ?? "musteri@ornek.com" },
                { "payment_amount", paymentAmount.ToString(CultureInfo.InvariantCulture) },
                { "paytr_token", paytrToken },
                { "user_basket", userBasket },
                { "debug_on", debugOn.ToString(CultureInfo.InvariantCulture) },
                { "no_installment", noInstallment.ToString(CultureInfo.InvariantCulture) },
                { "max_installment", maxInstallment.ToString(CultureInfo.InvariantCulture) },
                { "user_name", userName ?? "" },
                { "user_address", userAddress ?? "" },
                { "user_phone", userPhone ?? "" },
                { "merchant_ok_url", merchantOkUrl ?? "" },
                { "merchant_fail_url", merchantFailUrl ?? "" },
                { "timeout_limit", timeoutLimit.ToString(CultureInfo.InvariantCulture) },
                { "currency", cur },
                { "test_mode", testMode.ToString(CultureInfo.InvariantCulture) },
                { "lang", lang }
            };

            try
            {
                var req = (HttpWebRequest)WebRequest.Create(GetTokenUrl);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 25000;
                var body = string.Join("&", post.Select(kv => Uri.EscapeDataString(kv.Key) + "=" + Uri.EscapeDataString(kv.Value ?? "")));
                var buf = Encoding.UTF8.GetBytes(body);
                req.ContentLength = buf.Length;
                using (var st = req.GetRequestStream())
                    st.Write(buf, 0, buf.Length);
                string json;
                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    json = sr.ReadToEnd();

                var ser = new JavaScriptSerializer();
                var dict = ser.Deserialize<Dictionary<string, object>>(json);
                if (dict != null && dict.ContainsKey("status") && dict["status"] != null && dict["status"].ToString().Equals("success", StringComparison.OrdinalIgnoreCase)
                    && dict.ContainsKey("token") && dict["token"] != null)
                {
                    sonuc.Basarili = true;
                    sonuc.Token = dict["token"].ToString();
                    return sonuc;
                }
                sonuc.Mesaj = dict != null && dict.ContainsKey("reason") && dict["reason"] != null ? dict["reason"].ToString() : json;
            }
            catch (Exception ex)
            {
                sonuc.Mesaj = ex.Message;
            }
            return sonuc;
        }

        public static bool CallbackHashDogru(string merchantKey, string merchantSalt, string merchantOid, string status, string totalAmount, string gelenHash)
        {
            if (string.IsNullOrEmpty(gelenHash) || string.IsNullOrEmpty(merchantKey)) return false;
            string birlestir = string.Concat(merchantOid ?? "", merchantSalt ?? "", status ?? "", totalAmount ?? "");
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchantKey)))
            {
                var b = hmac.ComputeHash(Encoding.UTF8.GetBytes(birlestir));
                var token = Convert.ToBase64String(b);
                return string.Equals(token, gelenHash, StringComparison.Ordinal);
            }
        }
    }
}
