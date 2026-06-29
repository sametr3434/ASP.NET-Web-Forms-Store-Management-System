<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Urunler.aspx.cs" Inherits="Saat.Urunler" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="shop-area pt--40 pb--80 pt-md--30 pb-md--60">
            <div class="container">
                <div class="row mb--30">
                    <div class="col-12">
                        <h2><asp:Literal ID="ltrBaslik" runat="server" Text="Ürünlerimiz"></asp:Literal></h2>
                        <asp:Panel ID="pnlAramaOzet" runat="server" Visible="false" CssClass="text-muted mb-2">
                            <asp:Literal ID="ltrAramaOzet" runat="server"></asp:Literal>
                        </asp:Panel>
<asp:DropDownList ID="ddlKategoriEkle" runat="server"
    CssClass="modern-select"
    AutoPostBack="true"
    OnSelectedIndexChanged="ddlKategoriEkle_SelectedIndexChanged">
</asp:DropDownList>
                        <asp:Label ID="lblKategoriID" Visible="false" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <asp:Repeater ID="rptUrunler" runat="server">
                        <ItemTemplate>
                            <div class="col-xl-3 col-lg-4 col-md-6 mb--30">
                                <div class="mirora-product">
                                    <div class="product-img">
                                        <a href='<%# UrunLink(Eval("ID"), Eval("UrunAdi")) %>'>
                                            <img src='Upload/<%# Eval("AnaResim") %>' alt='<%# Duzelt(Eval("UrunAdi")) %>' class="primary-image" />
                                        </a>
                                    </div>
                                    <div class="product-content text-center">
                                        <span><%# Duzelt(Eval("MarkaAdi")) %></span>
                                        <h4>
                                            <a href='<%# UrunLink(Eval("ID"), Eval("UrunAdi")) %>'><%# Duzelt(Eval("UrunAdi")) %></a>
                                        </h4>
                                        <div class="product-price-wrapper">
                                            <span class="money"><%# FiyatGoster(Eval("Fiyat"), Eval("IndirimliFiyat")) %></span>
                                        </div>
                                        <asp:Panel runat="server" Visible='<%# RenkVarMi(Eval("RenkOzet")) %>' CssClass="mt-2">
                                            <small class="text-muted d-block">Renk: <%# Duzelt(Eval("RenkOzet")) %></small>
                                            <asp:Literal runat="server" Text='<%# RenkSwatchHtml(Eval("RenkKodlari")) %>' />
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <asp:Panel ID="pnlBos" runat="server" Visible="false" CssClass="col-12">
                    <asp:Literal ID="ltrBosMesaj" runat="server" Text="Şu an listelenecek ürün bulunmuyor."></asp:Literal>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
