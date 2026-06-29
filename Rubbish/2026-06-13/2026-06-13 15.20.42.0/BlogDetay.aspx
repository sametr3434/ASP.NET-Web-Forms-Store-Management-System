<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BlogDetay.aspx.cs" Inherits="Saat.BlogDetay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <!-- Main Wrapper Start -->
        <div class="main-content-wrapper">
            <div class="single-post-area pt--40 pb--80 pt-md--30 pb-md--60">
                <div class="container">
                    <div class="row">
                        <div class="col-lg-12 order-1 mb-md--30">
                            <div class="single-post-wrapper">
                                <article class="post post-details mb--30">
                                    <div class="post-media">
                                        <div class="image">
  <asp:Literal ID="ltrResim" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="post-info">
                                        <header class="entry-header">
                                            <div class="entry-meta">

                                                
                                                <span class="post-date">
                <asp:Literal ID="ltrTarih" runat="server"></asp:Literal></span>
                                            </div>
                                            <h2 class="post-title">
                    <asp:Literal ID="ltrBlogBaslik" runat="server"></asp:Literal></h2>
                                        </header>
                                        <div class="post-content">
                                            <p>
                        <asp:Literal ID="ltrKisaAciklama" runat="server"></asp:Literal></p> 

<blockquote style="color:darkred !important">
    <asp:Literal ID="ltrAciklama" runat="server"></asp:Literal>
</blockquote>

                                          
                                        </div>
                                        <div class="footer-meta">
                                            <a class="comment-count" href="#">Yorum Sayısı : <asp:Literal ID="ltrYorumAdeti" runat="server"></asp:Literal></a>
                                            <span></span>

                                        </div>
                                        
                                    </div>
                                    <div class="related-posts-wrap">
                                        <h3>İLGİLİ YAZILAR</h3>

                                        <div class="row">
                                                                                    <asp:Repeater ID="rptBlogGetir" runat="server">
                                <ItemTemplate>
                                            <div class="col-lg-4 mb-md--30">
                                                <div class="related-post">
                                                    <a href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("baslik").ToString())) %>' class="related-post__thumb">
                                                        <img src="/Upload/<%#Eval("resim") %>" alt="<%#Eval("baslik") %>">
                                                    </a>
                                                    <h4><a href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("baslik").ToString())) %>'><%#Eval("baslik") %></a></h4>
                                                    <span><%#Eval("tarih") %></span>
                                                </div>  
                                            </div>
                             </ItemTemplate>
                            </asp:Repeater>
                                   
                                        </div>
            
                                    </div>
                                </article>
                    </div>
                </div>
            </div>
        </div>
        <!-- Main Wrapper End -->
</asp:Content>
