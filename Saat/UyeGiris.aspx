<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UyeGiris.aspx.cs" Inherits="Saat.UyeGiris" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="login-register-area pt--40 pb--80">
            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-md-6 col-lg-5">
                        <h2 class="mb--30">Üye Girişi</h2>
                        <asp:Panel ID="pnlHata" runat="server" Visible="false" CssClass="alert alert-danger">
                            <asp:Literal ID="ltrHata" runat="server"></asp:Literal>
                        </asp:Panel>
                        <div class="form-group mb-3">
                            <label>Kullanıcı adı veya e-posta</label>
                            <asp:TextBox ID="txtKullanici" runat="server" CssClass="form-control" placeholder="demo" />
                        </div>
                        <div class="form-group mb-3">
                            <label>Şifre</label>
                            <asp:TextBox ID="txtSifre" runat="server" CssClass="form-control" TextMode="Password" placeholder="demo" />
                        </div>
                        <div class="form-group mb-3">
                            <asp:CheckBox ID="chkHatirla" runat="server" Text=" Beni hatırla" />
                        </div>
                        <asp:Button ID="btnGiris" runat="server" CssClass="btn btn-style-1" Text="Giriş Yap" OnClick="btnGiris_Click" />
                        <p class="mt-3 mb-0">
                            Hesabınız yok mu? <a href="/uye-kayit">Kayıt olun</a>
                        </p>
                        <p class="text-muted small mt-2 mb-0">Demo: kullanıcı adı <strong>demo</strong>, şifre <strong>demo</strong></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
