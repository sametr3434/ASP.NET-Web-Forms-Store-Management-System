using System.Text.RegularExpressions;

public static class firsttemizle
{
    public static string CleanDataToDb(string veri)
    {
        veri = veri.Trim();
        veri = veri.Replace("0x", "&#sfr;x");
        veri = veri.Replace("0X", "&#sfr;X");
        veri = veri.Replace(" ", "&#32;");//space
        veri = veri.Replace("!", "&#33;");
        veri = veri.Replace("$", "&#36;");
        veri = veri.Replace("%", "&#37;");
        veri = veri.Replace("'", "&#39;");
        veri = veri.Replace("(", "&#40;");
        veri = veri.Replace(")", "&#41;");
        veri = veri.Replace("*", "&#42;");
        veri = veri.Replace("+", "&#43;");
        veri = veri.Replace(",", "&#44;");
        veri = veri.Replace("-", "&#45;");
        veri = veri.Replace("/", "&#47;");
        veri = veri.Replace(":", "&#58;");
        veri = veri.Replace("<", "&#60;");
        veri = veri.Replace("&lt;", "&#60;");
        veri = veri.Replace("=", "&#61;");
        veri = veri.Replace(">", "&#62;");
        veri = veri.Replace("&gt;", "&#62;");
        veri = veri.Replace("?", "&#63;");
        veri = veri.Replace("@", "&#64;");
        veri = veri.Replace("\\", "&#92;");
        veri = veri.Replace("[", "&#91;");
        veri = veri.Replace("]", "&#93;");
        veri = veri.Replace("^", "&#94;");
        veri = veri.Replace("_", "&#95;");
        veri = veri.Replace("`", "&#96;");
        veri = veri.Replace("{", "&#123;");
        veri = veri.Replace("|", "&#124;");
        veri = veri.Replace("}", "&#125;");
        veri = veri.Replace("~", "&#126;");
        veri = veri.Replace("\"", "&#34;");
        veri = veri.Replace("&quot;", "&#34;");
        veri = veri.Replace("	", "&#32;");
        return veri;
    }

    public static string CleanDataToDbMail(string veri)
    {
        veri = veri.Trim();
        veri = veri.Replace("0x", "&#sfr;x");
        veri = veri.Replace("0X", "&#sfr;X");
        veri = veri.Replace(" ", "&#32;");//space
        veri = veri.Replace("!", "&#33;");
        veri = veri.Replace("$", "&#36;");
        veri = veri.Replace("%", "&#37;");
        veri = veri.Replace("'", "&#39;");
        veri = veri.Replace("(", "&#40;");
        veri = veri.Replace(")", "&#41;");
        veri = veri.Replace("*", "&#42;");
        veri = veri.Replace("+", "&#43;");
        veri = veri.Replace(",", "&#44;");
        //veri = veri.Replace("-", "&#45;");
        veri = veri.Replace("/", "&#47;");
        veri = veri.Replace(":", "&#58;");
        veri = veri.Replace("<", "&#60;");
        veri = veri.Replace("=", "&#61;");
        veri = veri.Replace(">", "&#62;");
        veri = veri.Replace("?", "&#63;");
        //veri = veri.Replace("@", "&#64;");
        veri = veri.Replace("\\", "&#92;");
        veri = veri.Replace("[", "&#91;");
        veri = veri.Replace("]", "&#93;");
        veri = veri.Replace("^", "&#94;");
        //veri = veri.Replace("_", "&#95;");
        veri = veri.Replace("`", "&#96;");
        veri = veri.Replace("{", "&#123;");
        veri = veri.Replace("|", "&#124;");
        veri = veri.Replace("}", "&#125;");
        veri = veri.Replace("~", "&#126;");
        veri = veri.Replace("\"", "&#34;");
        veri = veri.Replace("&quot;", "&#34;");
        veri = veri.Replace("	", "&#09;");
        return veri;
    }

    public static string CleanDataToDbAdmin(string veri)
    {
        veri = veri.Trim();
        veri = veri.Replace("0x", "&#sfr;x");
        veri = veri.Replace("0X", "&#sfr;X");
        //enter
        veri = veri.Replace("<br>", "&#br;");
        veri = veri.Replace("<Br>", "&#br;");
        veri = veri.Replace("<BR>", "&#br;");
        veri = veri.Replace("<bR>", "&#br;");
        veri = veri.Replace("<br />", "&#br;");
        veri = veri.Replace("<Br />", "&#br;");
        //paragraf
        veri = veri.Replace("<p>", "&#par;");
        veri = veri.Replace("</p>", "&#parEnd;");
        veri = veri.Replace("<P>", "&#par;");
        veri = veri.Replace("</P>", "&#parEnd;");
        // h tagları
        veri = veri.Replace("<h1>", "&#head1;");
        veri = veri.Replace("<H1>", "&#head1;");
        veri = veri.Replace("</h1>", "&#1head;");
        veri = veri.Replace("</H1>", "&#1head;");
        veri = veri.Replace("<h2>", "&#head2;");
        veri = veri.Replace("<H2>", "&#head2;");
        veri = veri.Replace("</h2>", "&#2head;");
        veri = veri.Replace("</H2>", "&#2head;");
        veri = veri.Replace("<h3>", "&#head3;");
        veri = veri.Replace("<H3>", "&#head3;");
        veri = veri.Replace("</h3>", "&#3head;");
        veri = veri.Replace("</H3>", "&#3head;");
        veri = veri.Replace("<h4>", "&#head4;");
        veri = veri.Replace("<H4>", "&#head4;");
        veri = veri.Replace("</h4>", "&#4head;");
        veri = veri.Replace("</H4>", "&#4head;");
        veri = veri.Replace("<h5>", "&#head5;");
        veri = veri.Replace("<H5>", "&#head5;");
        veri = veri.Replace("</h5>", "&#5head;");
        veri = veri.Replace("</H5>", "&#5head;");
        veri = veri.Replace("<h6>", "&#head6;");
        veri = veri.Replace("<H6>", "&#head6;");
        veri = veri.Replace("</h6>", "&#6head;");
        veri = veri.Replace("</H6>", "&#6head;");
        //kalın
        veri = veri.Replace("<b>", "&#strg;");
        veri = veri.Replace("<B>", "&#strg;");
        veri = veri.Replace("<strong>", "&#strg;");
        veri = veri.Replace("<Strong>", "&#strg;");
        veri = veri.Replace("<STRONG>", "&#strg;");
        veri = veri.Replace("</b>", "&#strgEnd;");
        veri = veri.Replace("</B>", "&#strgEnd;");
        veri = veri.Replace("</strong>", "&#strgEnd;");
        veri = veri.Replace("</Strong>", "&#strgEnd;");
        veri = veri.Replace("</STRONG>", "&#strgEnd;");
        //küçük
        veri = veri.Replace("<small>", "&#small;");
        veri = veri.Replace("<Small>", "&#small;");
        veri = veri.Replace("<SMALL>", "&#small;");
        veri = veri.Replace("</small>", "&#smallEnd;");
        veri = veri.Replace("</Small>", "&#smallEnd;");
        veri = veri.Replace("</SMALL>", "&#smallEnd;");
        //italik
        veri = veri.Replace("<i>", "&#it;");
        veri = veri.Replace("<I>", "&#it;");
        veri = veri.Replace("<em>", "&#it;");
        veri = veri.Replace("<Em>", "&#it;");
        veri = veri.Replace("<EM>", "&#it;");
        veri = veri.Replace("</i>", "&#itEnd;");
        veri = veri.Replace("</I>", "&#itEnd;");
        veri = veri.Replace("</em>", "&#itEnd;");
        veri = veri.Replace("</Em>", "&#itEnd;");
        veri = veri.Replace("</EM>", "&#itEnd;");
        //alt çizgi
        veri = veri.Replace("<u>", "&#und;");
        veri = veri.Replace("<U>", "&#und;");
        veri = veri.Replace("</u>", "&#undEnd;");
        veri = veri.Replace("</U>", "&#undEnd;");
        //karakter temizle
        veri = veri.Replace(" ", "&#32;");//space
        veri = veri.Replace("!", "&#33;");
        veri = veri.Replace("$", "&#36;");
        veri = veri.Replace("%", "&#37;");
        veri = veri.Replace("'", "&#39;");
        veri = veri.Replace("(", "&#40;");
        veri = veri.Replace(")", "&#41;");
        veri = veri.Replace("*", "&#42;");
        veri = veri.Replace("+", "&#43;");
        veri = veri.Replace(",", "&#44;");
        veri = veri.Replace("-", "&#45;");
        veri = veri.Replace("/", "&#47;");
        veri = veri.Replace(":", "&#58;");
        veri = veri.Replace("<", "&#60;");
        veri = veri.Replace("=", "&#61;");
        veri = veri.Replace(">", "&#62;");
        veri = veri.Replace("?", "&#63;");
        veri = veri.Replace("@", "&#64;");
        veri = veri.Replace("\\", "&#92;");
        veri = veri.Replace("[", "&#91;");
        veri = veri.Replace("\\", "&#92");
        veri = veri.Replace("]", "&#93;");
        veri = veri.Replace("^", "&#94;");
        veri = veri.Replace("_", "&#95;");
        veri = veri.Replace("`", "&#96;");
        veri = veri.Replace("{", "&#123;");
        veri = veri.Replace("|", "&#124;");
        veri = veri.Replace("}", "&#125;");
        veri = veri.Replace("~", "&#126;");
        veri = veri.Replace("\"", "&#34;");
        veri = veri.Replace("&quot;", "&#34;");
        veri = veri.Replace("	", "&#09;");
        return veri;
    }

    public static string UrlCevir(string veri)
    {
        // Ürün, kategori, blog başlık vs..de sadece rakam harf ve boşluk olacak!
        // \\i kontrol et böyle yapınca çift olarak mı görüyor yoksa kaçış karakteri olarak mı
        // *" i kontrol et çift tırnak olarakmı alıyor yoksa kaçış karakterini görmüyor mu
        veri = veri.Trim().ToLower();
        veri = veri.Replace("&#sfr;x", "0x");
        veri = veri.Replace("&#sfr;X", "0x");
        veri = veri.Replace("-", "");
        veri = veri.Replace("&#32;", " ");
        veri = veri.Replace("ç", "c");
        veri = veri.Replace("Ç", "C");
        veri = veri.Replace("ğ", "g");
        veri = veri.Replace("Ğ", "G");
        veri = veri.Replace("ü", "u");
        veri = veri.Replace("Ü", "U");
        veri = veri.Replace("ş", "s");
        veri = veri.Replace("Ş", "S");
        veri = veri.Replace("ö", "o");
        veri = veri.Replace("Ö", "O");
        veri = veri.Replace("ı", "i");
        veri = veri.Replace("İ", "I");
        veri = veri.Replace("!", "");
        veri = veri.Replace("&#33;", ""); //!
        veri = veri.Replace("$", "");
        veri = veri.Replace("&#36;", ""); //$
        veri = veri.Replace("%", "");
        veri = veri.Replace("&#37;", ""); //%
        veri = veri.Replace("'", "");
        veri = veri.Replace("&#39;", ""); //'
        veri = veri.Replace("(", "");
        veri = veri.Replace("&#40;", ""); //(
        veri = veri.Replace(")", "");
        veri = veri.Replace("&#41;", ""); //)
        veri = veri.Replace("*", "");
        veri = veri.Replace("&#42;", ""); //*
        veri = veri.Replace("+", "");
        veri = veri.Replace("&#43;", ""); //+
        veri = veri.Replace(",", "");
        veri = veri.Replace("&#44;", ""); //,
        veri = veri.Replace("/", "");
        veri = veri.Replace("&#47;", ""); // /
        veri = veri.Replace(":", "");
        veri = veri.Replace("&#58;", ""); //:
        veri = veri.Replace("<", "");
        veri = veri.Replace("&lt;", "");
        veri = veri.Replace("&#60;", "");//<
        veri = veri.Replace("=", "");
        veri = veri.Replace("&#61;", ""); //-
        veri = veri.Replace(">", "");
        veri = veri.Replace("&gt;", "");
        veri = veri.Replace("&#62;", "");//>
        veri = veri.Replace("?", "");
        veri = veri.Replace("&#63;", "");//?
        veri = veri.Replace("@", "");
        veri = veri.Replace("&#64;", "");//@
        veri = veri.Replace("\\", "");
        veri = veri.Replace("&#92;", "");// \
        veri = veri.Replace("[", "");
        veri = veri.Replace("&#91;", ""); //[
        veri = veri.Replace("]", "");
        veri = veri.Replace("&#93;", ""); //]
        veri = veri.Replace("^", "");
        veri = veri.Replace("&#94;", ""); //^
        veri = veri.Replace("_", "-");
        veri = veri.Replace("&#95;", ""); //_
        veri = veri.Replace("`", "");
        veri = veri.Replace("&#96;", ""); //`
        veri = veri.Replace("{", "");
        veri = veri.Replace("&#123;", ""); //{
        veri = veri.Replace("|", "");
        veri = veri.Replace("&#124;", ""); //|
        veri = veri.Replace("}", "");
        veri = veri.Replace("&#125;", ""); //}
        veri = veri.Replace("~", "");
        veri = veri.Replace("&#126;", ""); //~
        veri = veri.Replace("\"", "");
        veri = veri.Replace("&quot;", "");
        veri = veri.Replace("&#34;", ""); //"
        veri = veri.Replace("	", " ");
        veri = veri.Replace("&#09;", " "); //tab
        veri = veri.Replace("&", ""); //olası encodeden kalan artıklar için
        veri = veri.Replace("#", ""); //olası encodeden kalan artıklar için
        veri = veri.Replace(";", ""); //olası encodeden kalan artıklar için
        veri = veri.Replace("          ", "-");//10space
        veri = veri.Replace("         ", "-");//9space
        veri = veri.Replace("        ", "-");//8space
        veri = veri.Replace("       ", "-");//7space
        veri = veri.Replace("      ", "-");//6space
        veri = veri.Replace("     ", "-");//5space
        veri = veri.Replace("    ", "-");//4space
        veri = veri.Replace("   ", "-");//3space
        veri = veri.Replace("  ", "-");//2space
        veri = veri.Replace(" ", "-");//1space
        return veri;
    }
}