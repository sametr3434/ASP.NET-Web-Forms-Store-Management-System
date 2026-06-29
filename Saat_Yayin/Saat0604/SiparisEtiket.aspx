<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiparisEtiket.aspx.cs" Inherits="Saat.Saat0604.SiparisEtiket" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Kargo Etiketi - <%: SiparisNo %></title>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { font-family: Arial, sans-serif; font-size: 14px; padding: 20px; background: #f5f5f5; }
        .no-print { margin-bottom: 15px; }
        .no-print button { padding: 10px 24px; font-size: 16px; cursor: pointer; background: #2196F3; color: white; border: none; border-radius: 4px; }
        .no-print button:hover { background: #1976D2; }
        .etiket { width: 100mm; min-height: 150mm; padding: 8mm; background: white; border: 1px solid #ddd; margin: 0 auto; }
        .etiket h2 { font-size: 16px; margin-bottom: 12px; border-bottom: 2px solid #333; padding-bottom: 4px; }
        .etiket .bolum { margin-bottom: 14px; line-height: 1.5; }
        .etiket .bolum strong { display: block; font-size: 10px; color: #666; margin-bottom: 2px; text-transform: uppercase; }
        .etiket .siparis-no { font-size: 18px; font-weight: bold; margin: 10px 0; letter-spacing: 1px; }
        .etiket .kargo-no { font-size: 14px; margin-bottom: 8px; }
        .etiket .siparis-icerik { font-size: 11px; line-height: 1.4; }
        @media print {
            body { background: white; padding: 0; }
            .no-print { display: none !important; }
            .etiket { width: 100mm; min-height: 150mm; border: none; box-shadow: none; margin: 0; page-break-after: always; }
            @page { size: 100mm 150mm; margin: 5mm; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="no-print">
            <button type="button" onclick="window.print();">🖨️ Yazdır</button>
            <button type="button" onclick="window.close();">Kapat</button>
        </div>
        <div class="etiket" id="etiket">
            <div class="bolum">
                <strong>Gönderen</strong>
                <asp:Literal ID="ltrGonderen" runat="server"></asp:Literal>
            </div>
            <hr style="border: none; border-top: 1px dashed #999; margin: 12px 0;" />
            <div class="bolum">
                <strong>Alıcı</strong>
                <asp:Literal ID="ltrAlici" runat="server"></asp:Literal>
            </div>
            <div class="bolum">
                <strong>Teslimat Adresi</strong>
                <asp:Literal ID="ltrAdres" runat="server"></asp:Literal>
            </div>
            <div class="bolum">
                <strong>Telefon</strong>
                <asp:Literal ID="ltrTelefon" runat="server"></asp:Literal>
            </div>
            <hr style="border: none; border-top: 1px dashed #999; margin: 12px 0;" />
            <div class="bolum siparis-icerik">
                <strong>Sipariş İçeriği</strong>
                <asp:Literal ID="ltrSiparisIcerik" runat="server"></asp:Literal>
            </div>
            <hr style="border: none; border-top: 1px dashed #999; margin: 12px 0;" />
            <div class="siparis-no">
                Sipariş: <asp:Literal ID="ltrSiparisNo" runat="server"></asp:Literal>
            </div>
            <div class="kargo-no">
                Kargo Takip: <asp:Literal ID="ltrKargoTakip" runat="server"></asp:Literal>
            </div>
        </div>
    </form>
</body>
</html>
