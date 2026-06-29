<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Odeme.aspx.cs" Inherits="Saat.Odeme" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="checkout-area pt--40 pb--80">
            <div class="container">
                <h2 class="mb--30">Teslimat Bilgileri</h2>
                <div class="row">
                    <div class="col-md-7">
                        <div class="form-group mb-3">
                            <label>Ad</label>
                            <asp:TextBox ID="txtAd" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <label>Soyad</label>
                            <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <label>Telefon</label>
                            <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <label>E-posta</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <label>Adres</label>
                            <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>
                        <div class="row">
                            <div class="col-md-6 form-group mb-3">
                                <label>İl</label>
                                <asp:TextBox ID="txtIl" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-6 form-group mb-3">
                                <label>İlçe</label>
                                <asp:TextBox ID="txtIlce" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <asp:Button ID="btnDevam" runat="server" CssClass="btn btn-style-1" Text="Ödemeye Devam Et" OnClick="btnDevam_Click" />
                    </div>
                    <div class="col-md-5">
                        <div class="border p-3">
                            <h4>Sipariş Özeti</h4>
                            <asp:Literal ID="ltrOzet" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
