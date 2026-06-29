<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Sepet.aspx.cs" Inherits="Saat.Sepet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="cart-area pt--40 pb--80">
            <div class="container">
                <h2 class="mb--30">Sepetim</h2>
                <asp:Panel ID="pnlSepet" runat="server">
                    <div class="table-responsive">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>Ürün</th>
                                    <th>Fiyat</th>
                                    <th>Adet</th>
                                    <th>Toplam</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSepet" runat="server" OnItemCommand="rptSepet_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <img src='Upload/<%# Eval("AnaResim") %>' alt="" width="60" class="me-2" />
                                                <%# Duzelt(Eval("UrunAdi")) %>
                                            </td>
                                            <td><%# Convert.ToDecimal(Eval("BirimFiyat")).ToString("N2") %> TL</td>
                                            <td>
                                                <asp:TextBox ID="txtAdet" runat="server" Text='<%# Eval("Adet") %>' Width="50" CssClass="form-control form-control-sm" />
                                                <asp:LinkButton ID="lnkGuncelle" runat="server" CommandName="Guncelle" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-outline-secondary mt-1">Güncelle</asp:LinkButton>
                                            </td>
                                            <td><%# (Convert.ToDecimal(Eval("BirimFiyat")) * Convert.ToInt32(Eval("Adet"))).ToString("N2") %> TL</td>
                                            <td>
                                                <asp:LinkButton ID="lnkSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-danger" Text="Sil" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <div class="text-end mb--30">
                        <strong>Genel Toplam: </strong><asp:Literal ID="ltrToplam" runat="server"></asp:Literal>
                    </div>
                    <a href="/urunler" class="btn btn-style-3">Alışverişe Devam</a>
                    <asp:Button ID="btnOdeme" runat="server" CssClass="btn btn-style-1 ms-2" Text="Ödemeye Geç" OnClick="btnOdeme_Click" />
                </asp:Panel>
                <asp:Panel ID="pnlBos" runat="server" Visible="false">
                    <p>Sepetiniz boş. <a href="/urunler">Ürünlere göz atın</a></p>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
