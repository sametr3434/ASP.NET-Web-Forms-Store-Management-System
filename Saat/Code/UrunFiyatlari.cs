using System;
using System.Globalization;

namespace Saat.App_Code
{
    /// <summary>
    /// \u00dcr\u00fcn + varyant i\u00e7in se\u00e7ilen para biriminde efektif birim fiyat (liste / indirimli + fark).
    /// </summary>
    public static class UrunFiyatlari
    {
        public static decimal OkuDecimal(object o)
        {
            if (o == null || o == DBNull.Value) return 0m;
            return Convert.ToDecimal(o, CultureInfo.InvariantCulture);
        }

        public static decimal? OkuDecimalNull(object o)
        {
            if (o == null || o == DBNull.Value) return null;
            return Convert.ToDecimal(o, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Ana \u00fcr\u00fcn sat\u0131\u015f birim fiyat\u0131 (indirimli varsa ve ge\u00e7erliyse o; yabanc\u0131 para yoksa TRY + kur ile t\u00fcretir).
        /// </summary>
        public static decimal AnaBirimFiyat(string paraBirimi,
            decimal fiyatTry, object indirimliTry,
            object fiyatUsd, object indirimliUsd,
            object fiyatEur, object indirimliEur,
            decimal? kurUsd, decimal? kurEur)
        {
            var pb = ParaBirimiHelper.Normalize(paraBirimi);
            if (pb == ParaBirimiHelper.Usd)
            {
                var liste = OkuDecimal(fiyatUsd);
                if (liste > 0m)
                    return IndirimliUygula(liste, indirimliUsd);
                var tryEf = AnaBirimFiyat(ParaBirimiHelper.Try, fiyatTry, indirimliTry, null, null, null, null, kurUsd, kurEur);
                if (kurUsd.HasValue && kurUsd.Value > 0m)
                    return Math.Round(tryEf / kurUsd.Value, 2, MidpointRounding.AwayFromZero);
                return 0m;
            }
            if (pb == ParaBirimiHelper.Eur)
            {
                var liste = OkuDecimal(fiyatEur);
                if (liste > 0m)
                    return IndirimliUygula(liste, indirimliEur);
                var tryEf = AnaBirimFiyat(ParaBirimiHelper.Try, fiyatTry, indirimliTry, null, null, null, null, kurUsd, kurEur);
                if (kurEur.HasValue && kurEur.Value > 0m)
                    return Math.Round(tryEf / kurEur.Value, 2, MidpointRounding.AwayFromZero);
                return 0m;
            }
            var tlListe = fiyatTry;
            return IndirimliUygula(tlListe, indirimliTry);
        }

        static decimal IndirimliUygula(decimal liste, object indirimliObj)
        {
            var ind = OkuDecimalNull(indirimliObj);
            if (ind.HasValue && ind.Value > 0m && ind.Value < liste)
                return ind.Value;
            return liste;
        }

        public static decimal VaryantFarki(string paraBirimi, object farkTry, object farkUsd, object farkEur, decimal? kurUsd, decimal? kurEur)
        {
            var pb = ParaBirimiHelper.Normalize(paraBirimi);
            if (pb == ParaBirimiHelper.Usd)
            {
                var f = OkuDecimalNull(farkUsd);
                if (f.HasValue) return f.Value;
                var t = OkuDecimal(farkTry);
                if (t != 0m && kurUsd.HasValue && kurUsd.Value > 0m)
                    return Math.Round(t / kurUsd.Value, 2, MidpointRounding.AwayFromZero);
                return OkuDecimal(farkTry);
            }
            if (pb == ParaBirimiHelper.Eur)
            {
                var f = OkuDecimalNull(farkEur);
                if (f.HasValue) return f.Value;
                var t = OkuDecimal(farkTry);
                if (t != 0m && kurEur.HasValue && kurEur.Value > 0m)
                    return Math.Round(t / kurEur.Value, 2, MidpointRounding.AwayFromZero);
                return OkuDecimal(farkTry);
            }
            return OkuDecimal(farkTry);
        }

        public static decimal ToplamBirim(string paraBirimi,
            decimal fiyatTry, object indTry, object fUsd, object indUsd, object fEur, object indEur,
            decimal? kurUsd, decimal? kurEur,
            object farkTry, object farkUsd, object farkEur)
        {
            var ana = AnaBirimFiyat(paraBirimi, fiyatTry, indTry, fUsd, indUsd, fEur, indEur, kurUsd, kurEur);
            var fark = VaryantFarki(paraBirimi, farkTry, farkUsd, farkEur, kurUsd, kurEur);
            return ana + fark;
        }
    }
}
