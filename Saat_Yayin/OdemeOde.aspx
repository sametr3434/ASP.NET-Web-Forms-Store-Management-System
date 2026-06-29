<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="OdemeOde.aspx.cs" Inherits="Saat.OdemeOde" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="checkout-area pt--40 pb--80">
            <div class="container">
                <h2 class="mb--30">Son Ödeme</h2>
                <div class="row">
                    <div class="col-md-7">
                        <p class="mb-3"><strong>Ödenecek tutar: </strong><asp:Literal ID="ltrToplam" runat="server"></asp:Literal></p>
                        <fieldset class="mb-4">
                            <legend>Ödeme Yöntemi</legend>
                            <asp:RadioButtonList ID="rblOdeme" runat="server" CssClass="list-unstyled" AutoPostBack="true" OnSelectedIndexChanged="rblOdeme_SelectedIndexChanged">
                                <asp:ListItem Value="kapida" Selected="True">Kapıda Ödeme</asp:ListItem>
                                <asp:ListItem Value="eft">EFT / Havale</asp:ListItem>
                                <asp:ListItem Value="kart">Kredi Kartı</asp:ListItem>
                            </asp:RadioButtonList>
                        </fieldset>
                        <asp:Panel ID="pnlKapida" runat="server" CssClass="alert alert-info">
                            Teslimat sırasında nakit veya kart ile ödeme yapabilirsiniz. (Demo)
                        </asp:Panel>
                        <asp:Panel ID="pnlEft" runat="server" Visible="false" CssClass="alert alert-secondary">
                            <p><strong>Demo IBAN:</strong> TR00 0000 0000 0000 0000 0000 00</p>
                            <p>Açıklama kısmına sipariş numaranızı yazın. (Demo — ödeme otomatik onaylanır)</p>
                        </asp:Panel>
                        <asp:Panel ID="pnlKart" runat="server" Visible="false" CssClass="border p-3 mb-3">
                            <div class="form-group mb-2">
                                <label>Kart Numarası</label>
                                <asp:TextBox ID="txtKartNo" runat="server" CssClass="form-control" placeholder="0000 0000 0000 0000" />
                            </div>
                            <div class="row">
                                <div class="col-6 form-group mb-2">
                                    <label>Son Kullanma</label>
                                    <asp:TextBox ID="txtSkT" runat="server" CssClass="form-control" placeholder="AA/YY (ör. 12/28)" />
                                </div>
                                <div class="col-6 form-group mb-2">
                                    <label>Güvenlik Kodu (CVV)</label>
                                    <asp:TextBox ID="txtCvv" runat="server" CssClass="form-control" placeholder="123" />
                                </div>
                            </div>
                            <small class="text-muted">Demo — kart bilgisi doğrulanmaz.</small>
                        </asp:Panel>
                        <asp:Button ID="btnOnayla" runat="server" CssClass="btn btn-style-1 btn-lg" Text="Siparişi Onayla" OnClick="btnOnayla_Click" />
                        <asp:Label ID="lblHata" Visible="false" runat="server" CssClass="text-danger d-block mt-2"></asp:Label>
  <asp:Label ID="lblOdendi" runat="server" CssClass="payment-badge"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
