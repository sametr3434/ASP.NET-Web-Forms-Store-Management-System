<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="OdemeBasarili.aspx.cs" Inherits="Saat.OdemeBasarili" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="pt--60 pb--80">
            <div class="container text-center">
                <div class="border p-5 mx-auto" style="max-width:600px;">
                    <i class="fa fa-check-circle text-success" style="font-size:64px;"></i>
                    <h2 class="mt-3">Siparişiniz Tamamlanmıştır</h2>
                    <p>Sipariş numaranız: <strong><asp:Literal ID="ltrSiparisNo" runat="server"></asp:Literal></strong></p>
                    <p>Ödeme yöntemi: <strong><asp:Literal ID="ltrYontem" runat="server"></asp:Literal></strong></p>
                    <asp:Literal ID="ltrMesaj" runat="server"></asp:Literal>
                    <div class="mt-4">
                        <a href="/anasayfa" class="btn btn-style-1">Ana Sayfa</a>
                        <a href="/urunler" class="btn btn-style-3 ms-2">Alışverişe Devam</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
