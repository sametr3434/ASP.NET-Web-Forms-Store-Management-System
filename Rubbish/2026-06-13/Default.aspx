<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Saat.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <!-- Main Content Wrapper Start -->
        <div class="main-content-wrapper">
            <!-- Slider area Start -->

            <div class="slider-area">
                <div class="homepage-slider">
<asp:Repeater ID="rptSliderGetir" runat="server">
    <ItemTemplate>

        <div class="single-slider content-v-center"
             style='<%# "background-image:url(/Upload/" + Eval("resim") + ");" %>'>

            <div class="container">
                <div class="row">
                    <div class="col-12">
                        <div class="slider-content">

                            <h5><%# Eval("yazi1") %></h5>
                            <h1><%# Eval("yazi2") %></h1>
                            <p class="mb--30 mb-sm--20"><%# Eval("yazi3") %></p>

                            <div class="slide-btn-group">
                                <a href="/urunler" class="btn btn-bordered btn-style-1">
                                    Alışveriş Yap
                                </a>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>

    </ItemTemplate>
</asp:Repeater>


                </div>
            </div>

            <!-- Slider area End -->

            <!-- Promo Box area Start -->
<div class="promo-box-area border-bottom ptb--80 ptb-md--60">
    <div class="container">
        <div class="row">

            <asp:Repeater ID="rptVitrinGetir" runat="server">
                <ItemTemplate>
                    <div class="col-md-4 mb-sm--30">
                        <div class="promo promo-1">
                            <a href='<%# string.Format("/urundetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("UrunAdi").ToString())) %>' class="promo__box">
                                <img src="/Upload/<%# Eval("AnaResim") %>" alt="<%# Eval("UrunAdi") %>">

                                <span class="promo__content">
                                    <span class="promo__label"><%# Eval("MarkaAdi") %></span>
                                    <span class="promo__name"><%# Eval("UrunAdi") %></span>
                                    <span class="promo__price">
                                        ₺<%# Eval("Fiyat","{0:N2}") %>
                                    </span>
                                </span>
                            </a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
    </div>
</div>
      
            <!-- Promo Box area End -->

            <!-- Most Viewed Product area Start -->

<section class="mostviewed-product-area border-bottom pt--80 pb--60 pt-md--60 pb-md--50">
    <div class="container">

        <div class="row">
            <div class="col-12">
                <div class="section-title mb--15">
                    <h2 class="color--white">En Çok Görüntülenen Ürünler</h2>
                </div>
            </div>
        </div>

        <div class="row no-gutters">
            <div class="col-12">
                <div class="product-carousel nav-top js-product-carousel-2">

                    <asp:Repeater ID="rptEnCokGoruntulenen" runat="server">
                        <ItemTemplate>

                            <div class="mirora-product">
                                <div class="product-img">
                                    <img src='Upload/<%# Eval("AnaResim") %>' 
                                         alt='<%# Eval("UrunAdi") %>' 
                                         class="primary-image" />

                                    <div class="product-img-overlay">
                                        <a data-bs-toggle="modal" 
                                           data-bs-target="#productModal" 
                                           class="btn btn-transparent btn-fullwidth btn-medium btn-style-1">
                                            Hızlı Görüntüle
                                        </a>
                                    </div>
                                </div>

                                <div class="product-content text-center">
                                    <span><%# Eval("MarkaAdi") %></span>

                                    <h4>
                                        <a href='<%# string.Format("/urundetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("UrunAdi").ToString())) %>'>
                                            <%# Eval("UrunAdi") %>
                                        </a>
                                    </h4>

                                    <div class="product-price-wrapper">
                                        <span class="money"><%# Eval("Fiyat") %> TL</span>
                                    </div>
                                </div>

                                <div class="mirora_product_action text-center position-absolute">
                                    <p><%# Eval("KisaAciklama") %></p>

                                    <div class="product-action">
                                        <a class="same-action" href="wishlist.html" title="Favoriler">
                                            <i class="fa fa-heart-o"></i>
                                        </a>

                                        <a class="add_cart cart-item action-cart" href="cart.html" title="Sepete Ekle">
                                            <span>Sepete Ekle</span>
                                        </a>

                                        <a class="same-action compare-mrg" 
                                           data-bs-toggle="modal" 
                                           data-bs-target="#productModal" 
                                           href="compare.html">
                                            <i class="fa fa-sliders fa-rotate-90"></i>
                                        </a>
                                    </div>
                                </div>
                            </div>

                        </ItemTemplate>
                    </asp:Repeater>

                </div>
            </div>
        </div>

    </div>
</section> 

            <!-- Most Viewed Product area End -->

            <!-- Blog area Start -->

            <section class="blog-area pt--80 pb--40 pt-md--60 pb-md--30">
                <div class="container">
                    <div class="row">
                        <div class="col-12">
                            <div class="section-title mb--30">
                                <h2>Bloglarımız</h2>
                            </div>
                        </div>
                    </div>  
                    <div class="row">
                        <div class="col-12">
                            <div class="blog-carousel nav-top slick-item-gutter">
                                <asp:Repeater ID="rptBloglarimiz" runat="server">
        <ItemTemplate>
                                <article class="blog">
                                    <a href="'<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("Baslik").ToString())) %>'" class="blog__thumb">
                                        <img src="Upload/<%#Eval("resim") %>" alt="Blog">
                                    </a>
                                    <div class="blog__content">
                                        <div class="blog__meta">
                                            <p class="blog__author">Post By: <a href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("Baslik").ToString())) %>'><%#Eval("Baslik") %>" </a></p>
                                            <p class="blog__date"><a href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("Baslik").ToString())) %>'><%#Eval("Tarih") %>"</a></p>
                                        </div>
                                        
                                        <h3 class="blog__title"><a href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("Baslik").ToString())) %>'><%#Eval("KisaAciklama") %>".</a></h3>
                                        <div class="blog__text">
                                            
                                            <a class="read-more" href='<%# string.Format("/blogdetay/{0}/{1}", Eval("ID"), Temizle.UrlCevir(Eval("Baslik").ToString())) %>'>Daha Fazlası</a>
                                        </div>
                                        
                                    </div>
                                </article>
        </ItemTemplate>
    </asp:Repeater>


                            </div>
                        </div>
                    </div>

                </div>
            </section>

            <!-- Blog area End -->

            <!-- Newsletter area End -->
            
            <div class="newsletter-area pt--40 pb--80 pt-md--30 pb-md--60">
                <div class="container">
                    <div class="row justify-content-center">
                        <div class="col-xl-9 col-lg-10">
                            <div class="newsletter text-center">
                                <h3 class="color--white">İndirimlerden ve önemli haberlerden haberdar olmak için e-posta adresinizi girin</h3>
                                <p>Yeni kampanyalar, özel indirimler ve saat dünyasındaki önemli gelişmelerden ilk siz haberdar olun</p>

                                <div class="newsletter-form validate mt--40" action="https://devitems.us11.list-manage.com/subscribe/post?u=6bbb9b6f5827bd842d9640c82&amp;id=05d85f18ef" method="post" id="mc-embedded-newsletter-form" name="mc-embedded-newsletter-form" target="_blank" novalidate="">
<asp:TextBox ID="txtKisiMailAdresi" PlaceHolder="E-Posta Adresiniz" runat="server"></asp:TextBox>
        <asp:Button ID="btnMailAdresiKayiEt" OnClick="btnMailAdresiKayiEt_Click" runat="server" Text="Mail Kaydet" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Newsletter area End -->


        </div>
        <!-- Main Content Wrapper Start -->
</asp:Content>
