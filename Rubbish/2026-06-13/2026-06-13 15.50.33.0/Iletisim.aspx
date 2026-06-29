<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Iletisim.aspx.cs" Inherits="Saat.Iletisim" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <!-- Breadcumb area Start -->
        <div class="breadcrumb-area">
            <div class="container">
                <div class="row">
                    <div class="col-12 text-center">
                        <h1 class="page-title">Bize Ulaşın</h1>
                        <ul class="breadcrumb justify-content-center">
                            <li><a href="/anasayfa">Anasayfa</a></li>
                            <li class="current"><a href="/iletisim">İletişim</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!-- Breadcumb area End -->

        <!-- Main Wrapper Start -->
        <div class="main-content-wrapper">
            <!-- Google Map Start -->
<!-- Google Map Start -->
<div class="google-map-wrapper pt--40 pt-md--30">
    <iframe
        src="https://www.google.com/maps/embed?pb=!1m10!1m8!1m3!1d3574.3928229339595!2d29.116821926422872!3d40.952588172851556!3m2!1i1024!2i768!4f13.1!5e1!3m2!1str!2str!4v1781285242616!5m2!1str!2str"
        width="100%"
        height="450"
        style="border:0;"
        allowfullscreen
        loading="lazy">
    </iframe>
</div>
<!-- Google Map End -->
            <!-- Google Map End -->

            <!-- Contact Area Start -->
            <div class="contact-area ptb--80 ptb-md--60">
                <div class="container">
                    <div class="row">
                        <div class="col-12">
                            <h2 class="heading-secondary border-bottom mb--30">Bize Mesaj Gönderin</h2>
                            <div class="form form--contact" id="iletisim-form">
                                <div class="form-row mb--20">
 
                                    </div>
<div class="col-md-10">
    <asp:TextBox ID="txtAdi" placeholder="Adınız" runat="server"
        style="width:100%; background:transparent; border:none; border-bottom:1.5px solid rgba(255,255,255,0.3); outline:none; padding:12px 4px; font-size:15px; color:#ffffff; margin-bottom:24px; display:block; box-sizing:border-box; border-radius:0;">
    </asp:TextBox>
</div>
<div class="col-md-10">
    <asp:TextBox ID="txtEmail" placeholder="E-posta adresiniz" runat="server"
        style="width:100%; background:transparent; border:none; border-bottom:1.5px solid rgba(255,255,255,0.3); outline:none; padding:12px 4px; font-size:15px; color:#ffffff; margin-bottom:24px; display:block; box-sizing:border-box; border-radius:0;">
    </asp:TextBox>
</div>
<div class="col-md-10">
    <asp:TextBox ID="txtTelefon" placeholder="Telefon numaranız" runat="server"
        style="width:100%; background:transparent; border:none; border-bottom:1.5px solid rgba(255,255,255,0.3); outline:none; padding:12px 4px; font-size:15px; color:#ffffff; margin-bottom:24px; display:block; box-sizing:border-box; border-radius:0;">
    </asp:TextBox>
</div>
<div class="col-md-10">
    <asp:TextBox ID="txtMesaj" TextMode="MultiLine" placeholder="Mesajınız" runat="server"
        style="width:100%; height:150px; background:transparent; border:none; border-bottom:1.5px solid rgba(255,255,255,0.3); outline:none; padding:12px 4px; font-size:15px; color:#ffffff; margin-bottom:24px; display:block; box-sizing:border-box; border-radius:0;">
    </asp:TextBox>
</div>
<asp:Button ID="btnMesajGonder" OnClick="btnMesajGonder_Click" runat="server" Text="Mesaj Gönder"
    style="background:transparent; color:#e8c97a; border:2px solid #e8c97a; padding:13px 36px; font-size:14px; font-weight:600; letter-spacing:1px; cursor:pointer; border-radius:0;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Contact Area End -->
        </div>
        <!-- Main Wrapper End -->

        <!-- Google Map -->
    <script src="https://www.google.com/maps/embed?pb=!1m10!1m8!1m3!1d3574.3928229339595!2d29.116821926422872!3d40.952588172851556!3m2!1i1024!2i768!4f13.1!5e1!3m2!1str!2str!4v1781285242616!5m2!1str!2str"></script>






</asp:Content>
