<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Urunler.aspx.cs" Inherits="Saat.Urunler" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="shop-area pt--40 pb--80 pt-md--30 pb-md--60">
            <div class="container">
                <div class="row mb--30">
                    <div class="col-12">
                        <h2>Ürünlerimiz</h2>
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
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <asp:Panel ID="pnlBos" runat="server" Visible="false" CssClass="col-12">
                    <p>Şu an listelenecek ürün bulunmuyor.</p>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
