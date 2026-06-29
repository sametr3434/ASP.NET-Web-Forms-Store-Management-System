<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UrunDetay.aspx.cs" Inherits="Saat.UrunDetay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="product-details-area pt--40 pb--80">
            <div class="container">
                <asp:Panel ID="pnlUrun" runat="server">
                    <div class="row">
                        <div class="col-md-6 mb--30">
                            <asp:Literal ID="ltrResim" runat="server"></asp:Literal>
                        </div>
                        <div class="col-md-6">
                            <p><asp:Literal ID="ltrMarka" runat="server"></asp:Literal></p>
                            <h2><asp:Literal ID="ltrUrunAdi" runat="server"></asp:Literal></h2>
                            <p class="h4 text-danger"><asp:Literal ID="ltrFiyat" runat="server"></asp:Literal></p>
                            <p><asp:Literal ID="ltrAciklama" runat="server"></asp:Literal></p>
                            <asp:Panel ID="pnlVaryant" runat="server" Visible="false">
                                <asp:Panel ID="pnlKasaGrup" runat="server" CssClass="form-group">
                                    <label>Kasa çapı</label>
                                    <asp:DropDownList ID="ddlKasa" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="VaryantSecimiDegisti" />
                                </asp:Panel>
                                <asp:Panel ID="pnlRenkGrup" runat="server" CssClass="form-group">
                                    <label>Renk</label>
                                    <asp:DropDownList ID="ddlRenk" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="VaryantSecimiDegisti" />
                                </asp:Panel>
                                <asp:Literal ID="ltrRenkSwatch" runat="server"></asp:Literal>
                                <asp:Label ID="lblStok" runat="server" CssClass="text-muted d-block mb-2"></asp:Label>
                            </asp:Panel>
                            <div class="form-group">
                                <label>Adet</label>
                                <asp:TextBox ID="txtAdet" runat="server" CssClass="form-control" Text="1" Width="80" />
                            </div>
                            <asp:Button ID="btnSepeteEkle" runat="server" CssClass="btn btn-style-1" Text="Sepete Ekle" OnClick="btnSepeteEkle_Click" />
                            <asp:Label ID="lblMesaj" runat="server" CssClass="text-success d-block mt-2"></asp:Label>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlYok" runat="server" Visible="false">
                    <p>Ürün bulunamadı. <a href="/urunler">Ürünlere dön</a></p>
                </asp:Panel>
                <asp:Panel ID="pnlOneriler" runat="server" CssClass="mt-5">
                    <h3>Size Öneriyoruz</h3>
                    <p class="text-muted">Son incelediğiniz saatlere benzer modeller</p>
                    <div class="row">
                        <asp:Repeater ID="rptOneriler" runat="server">
                            <ItemTemplate>
                                <div class="col-md-3 col-sm-6 mb-3">
                                    <a href='<%# UrunLink(Eval("ID"), Eval("UrunAdi")) %>'>
                                        <img src='/Upload/<%# Eval("AnaResim") %>' class="img-fluid mb-2" alt="" />
                                    </a>
                                    <p class="mb-0"><strong><%# Eval("UrunAdi") %></strong></p>
                                    <span><%# FiyatGoster(Eval("Fiyat"), Eval("IndirimliFiyat")) %></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
