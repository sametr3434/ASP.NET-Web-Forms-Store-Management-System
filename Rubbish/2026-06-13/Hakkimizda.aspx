<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Hakkimizda.aspx.cs" Inherits="Saat.Hakkimizda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <!-- Main Wrapper Start -->
        <div class="main-content-wrapper">
            <div class="about-area bg--dark-3 mt--40 mt-sm--30">
                <div class="container-fluid p-0">
                    <div class="row no-gutters align-items-center">
                        <div class="col-xl-6">
                            <div class="img-box text-center">
                                <img src='/Upload/<%=resim %>' alt="Hakkımızda">
                            </div>
                        </div>
                        <div class="col-xl-6">
                            <div class="row">
                                <div class="col-10 offset-1">
                                    <div class="about-text text-center">
                                        <h2 class="heading-secondary mb--40 mb-sm--30">
                                            <asp:Literal ID="ltrHakkimizdaBaslik" runat="server"></asp:Literal>
                                        </h2>
                                        <p class="mb--40 mb-sm--30"> <asp:Literal ID="ltrHakkimizdaAciklama" runat="server"></asp:Literal></p>
                                        <div class="about-btn-group text-center">
                                            <a href="/iletisim" class="btn btn-style-3">Bizimle İletişime Geç</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="fact-area" id="fun-fact">
                <div class="container-fluid p-0">
                    <div class="row no-gutters">
                        <div class="col-lg-3 col-sm-6">
                            <div class="fact">
                                <div class="fact__icon">
                                    <img src="assets/img/icons/about-us-icon1.png" alt="about icon">
                                </div>
                                <div class="fact__content">
                                    <h3><span class="counter" data-count="<%=Rakam1 %>"">0</span></h3>
                                    <p>
                                        <asp:Literal ID="ltrRakam1Yazisi" runat="server"></asp:Literal></p>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <div class="fact">
                                <div class="fact__icon">
                                    <img src="assets/img/icons/about-us-icon2.png" alt="about icon">
                                </div>
                                <div class="fact__content">
                                    <h3><span class="counter" data-count="<%=Rakam2 %>"">0</span></h3>
                                    <p>
                                        <asp:Literal ID="ltrRakam2Yazisi" runat="server"></asp:Literal></p>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <div class="fact">
                                <div class="fact__icon">
                                    <img src="assets/img/icons/about-us-icon3.png" alt="about icon">
                                </div>
                                <div class="fact__content">
                                    <h3><span class="counter" data-count="<%=Rakam3 %>"">0</span></h3>
                                    <p>
                                        <asp:Literal ID="ltrRakam3Yazisi" runat="server"></asp:Literal></p>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <div class="fact">
                                <div class="fact__icon">
                                    <img src="assets/img/icons/about-us-icon4.png" alt="about icon">
                                </div>
                                <div class="fact__content">
                                    <h3><span class="counter" data-count="<%=Rakam4 %>"">0</span></h3>
                                    <p>
                                        <asp:Literal ID="ltrRakam4Yazisi" runat="server"></asp:Literal></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!-- Main Wrapper End -->
</asp:Content>
