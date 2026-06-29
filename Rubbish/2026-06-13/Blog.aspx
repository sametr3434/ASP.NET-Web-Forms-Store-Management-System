<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Blog.aspx.cs" Inherits="Saat.Blog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <!-- Main Wrapper Start -->
<div class="main-content-wrapper">
    <div class="blog-area pt--40 pb--80 pt-md--30 pb-md--60">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">

                    <div class="row">
                        <asp:Repeater ID="rptBlogGetir" runat="server">
                            <ItemTemplate>                      
                                <div class="col-xl-3 col-lg-4 col-md-6 mb--30">  
                                    <article class="post sticky single-post format-image">
                                        <div class="post-media">
                                            <div class="image">
                                                <a href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("baslik").ToString())) %>'>
                                                    <img src="Upload/<%# Eval("Resim") %>" alt="<%# Eval("Baslik") %>">
                                                </a>
                                            </div>
                                        </div>
                                        <div class="post-info">
                                            <header class="entry-header">
                                                <div class="entry-meta">
                                                    <span class="post-date"><%# Eval("tarih") %></span>
                                                </div>
                                                <h2 class="post-title">
                                                    <a href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("baslik").ToString())) %>'>
                                                        <%# Eval("baslik") %>
                                                    </a>
                                                </h2>
                                            </header>
                                            <div class="post-content">
                                                <p><%# Eval("KisaAciklama") %></p>
                                            </div>
                                            <a href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("baslik").ToString())) %>'
                                               class="btn btn-read-more btn-style-2">Daha Fazla</a>
                                        </div>
                                    </article>
                                </div>                          
                            </ItemTemplate>                       
                        </asp:Repeater>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <div class="pagination-wrap">
                                <p class="page-ammount">Showing 1 to 8 of 15 (2 Pages)</p>
                                <ul class="pagination">
                                    <li><a href="#" class="first">|&lt;</a></li>
                                    <li><a href="#" class="prev">&lt;</a></li>
                                    <li><a href="#" class="current">1</a></li>
                                    <li><a href="#">2</a></li>
                                    <li><a href="#" class="next">&gt;</a></li>
                                    <li><a href="#" class="next">&gt;|</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</div>
        <!-- Main Wrapper End -->
</asp:Content>
