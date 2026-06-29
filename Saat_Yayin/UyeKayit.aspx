<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UyeKayit.aspx.cs" Inherits="Saat.UyeKayit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="login-register-area pt--40 pb--80">
            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-md-8 col-lg-6">
                        <h2 class="mb--30">Üye Kayıt</h2>
                        <asp:Panel ID="pnlHata" runat="server" Visible="false" CssClass="alert alert-danger">
                            <asp:Literal ID="ltrHata" runat="server"></asp:Literal>
                        </asp:Panel>
                        <div class="row">
                            <div class="col-md-6 form-group mb-3">
                                <label>Ad</label>
                                <asp:TextBox ID="txtAd" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-6 form-group mb-3">
                                <label>Soyad</label>
                                <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="form-group mb-3">
                            <label>E-posta</label>
                            <asp:TextBox ID="txtEposta" runat="server" CssClass="form-control" TextMode="Email" />
                        </div>
                        <div class="form-group mb-3">
                            <label>Kullanıcı adı (isteğe bağlı)</label>
                            <asp:TextBox ID="txtKullaniciAdi" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <label>Telefon</label>
                            <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" />
                        </div>
                        <div class="row">
                            <div class="col-md-6 form-group mb-3">
                                <label>Şifre</label>
                                <asp:TextBox ID="txtSifre" runat="server" CssClass="form-control" TextMode="Password" />
                            </div>
                            <div class="col-md-6 form-group mb-3">
                                <label>Şifre (tekrar)</label>
                                <asp:TextBox ID="txtSifreTekrar" runat="server" CssClass="form-control" TextMode="Password" />
                            </div>
                        </div>
                        <asp:Button ID="btnKayit" runat="server" CssClass="btn btn-style-1" Text="Kayıt Ol" OnClick="btnKayit_Click" />
                        <p class="mt-3 mb-0">
                            Zaten üye misiniz? <a href="/uye-giris">Giriş yapın</a>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
