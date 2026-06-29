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
                                <div class="comment">
                                    <div class="comment-respond">
                                        <h3 class="comment-reply-title">LEAVE A REPLY</h3>
                                        <ul class="comment-list">
                                            <li>
                                                <div class="single-comment">
                                                    <div class="comment-avatar">
                                                        <img src="assets/img/others/comment-1.jpg" alt="comment">
                                                    </div>
                                                    <div class="comment-info">
                                                        <div class="comment-meta">
                                                            <h5 class="comment-author"><a href="#">Julia Rebeca</a></h5>
                                                            <span class="comment-date">30 Janurary, 2018</span>
                                                            <a href="#" class="reply">Reply</a>
                                                        </div>
                                                        <div class="comment-content">
                                                            <p>enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia cntur magn lores eos qui ratione voluptatem sequi nesciunt. Neque porro</p>
                                                        </div>
                                                    </div>
                                                </div>
                                                <ul class="children">
                                                    <li>
                                                        <div class="single-comment">
                                                            <div class="comment-avatar">
                                                                <img src="assets/img/others/comment-2.jpg" alt="comment">
                                                            </div>
                                                            <div class="comment-info">
                                                                <div class="comment-meta">
                                                                    <h5 class="comment-author"><a href="#">Admin</a></h5>
                                                                    <span class="comment-date">30 Janurary, 2018</span>
                                                                    <a href="#" class="reply">Reply</a>
                                                                </div>
                                                                <div class="comment-content">
                                                                    <p>enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia cntur magn lores eos qui ratione voluptatem sequi nesciunt. Neque porro</p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                </ul>
                                            </li>
                                            <li>
                                                <div class="single-comment">
                                                    <div class="comment-avatar">
                                                        <img src="assets/img/others/comment-3.jpg" alt="comment">
                                                    </div>
                                                    <div class="comment-info">
                                                        <div class="comment-meta">
                                                            <h5 class="comment-author"><a href="#">Julia Rebeca</a></h5>
                                                            <span class="comment-date">30 Janurary, 2018</span>
                                                            <a href="#" class="reply">Reply</a>
                                                        </div>
                                                        <div class="comment-content">
                                                            <p>enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia cntur magn lores eos qui ratione voluptatem sequi nesciunt. Neque porro</p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                        <div class="form comment-form">
                                            <p class="comment-notes"><span id="email-notes">Your email address will not be published.</span> Required fields are marked <span class="required">*</span></p>
                                            <div class="form-row mb--20">
                                                <div class="col-12">
                                                    <div class="form__group">
                                                        <label class="form__label" for="comment">Comment *</label>
                                                        <textarea name="comment" id="comment" class="form__input form__input--3 form__input--textarea"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-row mb--20">
                                                <div class="col-md-4 mb-sm--20">
                                                    <div class="form__group">
                                                        <label class="form__label" for="comment_name">Name *</label>
                                                        <input type="text" id="comment_name" name="comment_name" class="form__input form__input--3">
                                                    </div>
                                                </div>
                                                <div class="col-md-4 mb-sm--20">
                                                    <div class="form__group">
                                                        <label class="form__label" for="comment_email">Email *</label>
                                                        <input type="email" id="comment_email" name="comment_email" class="form__input form__input--3">
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form__group">
                                                        <label class="form__label" for="comment_url">Website *</label>
                                                        <input type="url" id="comment_url" name="comment_url" class="form__input form__input--3">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-row">
                                                <div class="col-12">
                                                    <button type="submit" class="form__submit">Post Comment</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 order-lg-1 order-2">
                            <aside class="blog-sidebar">
                                <!-- Search Widget Start -->
                                <div class="sidebar-widget search-widget">
                                    <h3 class="widget-title">Search</h3>
                                    <div class="widget_conent">
                                        <form action="#" class="searchform">
                                            <input type="text" class="searchform__input" name="search" id="blog_search" placeholder="Search...">
                                            <button class="searchform__submit"><i class="fa fa-search"></i></button>
                                        </form>
                                    </div>
                                </div>
                                <!-- Search Widget End -->

                                <!-- Archive Widget Start -->
                                <div class="sidebar-widget archive-widget">
                                    <h3 class="widget-title">Blog Archives</h3>
                                    <div class="widget_conent">
                                        <ul>
                                            <li><a href="single-blog.html">March 2015</a> <span>(1)</span></li>
                                            <li><a href="single-blog.html">April 2015</a> <span>(5)</span></li>
                                            <li><a href="single-blog.html">May 2015</a> <span>(7)</span></li>
                                            <li><a href="single-blog.html">June 2015</a> <span>(10)</span></li>
                                            <li><a href="single-blog.html">July 2015</a> <span>(15)</span></li>
                                            <li><a href="single-blog.html">August 2015</a> <span>(10)</span></li>
                                            <li><a href="single-blog.html">September 2015</a> <span>(8)</span></li>
                                        </ul>
                                    </div>
                                </div>
                                <!-- Archive Widget Start -->

                                <!-- Recent Post Widget Start -->
                                <div class="sidebar-widget recent-post-widget">
                                    <h3 class="widget-title">Recent Posts</h3>
                                    <div class="widget_conent">
                                        <div class="recent-post-single">
                                            <div class="recent-post-media">
                                                <div class="image">
                                                    <img src="assets/img/blog/post4-370x230.jpg" alt="Blog">
                                                </div>
                                            </div>
                                            <div class="recent-post-content">
                                                <h4><a href="single-blog.html">Gravida luctus lorem accumsan est massa mauris.</a></h4>
                                                <p><a href="single-blog.html">26-10-18</a></p>
                                            </div>
                                        </div>
                                        <div class="recent-post-single">
                                            <div class="recent-post-media">
                                                <div class="image">
                                                    <img src="assets/img/blog/post3-370x230.jpg" alt="Blog">
                                                </div>
                                            </div>
                                            <div class="recent-post-content">
                                                <h4><a href="single-blog.html">Gravida luctus lorem accumsan est massa mauris.</a></h4>
                                                <p><a href="single-blog.html">27-10-18</a></p>
                                            </div>
                                        </div>
                                        <div class="recent-post-single">
                                            <div class="recent-post-media">
                                                <div class="image">
                                                    <img src="assets/img/blog/post2-370x230.jpg" alt="Blog">
                                                </div>
                                            </div>
                                            <div class="recent-post-content">
                                                <h4><a href="single-blog.html">Gravida luctus lorem accumsan est massa mauris.</a></h4>
                                                <p><a href="single-blog.html">28-10-18</a></p>
                                            </div>
                                        </div>
                                        <div class="recent-post-single">
                                            <div class="recent-post-media">
                                                <div class="image">
                                                    <img src="assets/img/blog/post1-370x230.jpg" alt="Blog">
                                                </div>
                                            </div>
                                            <div class="recent-post-content">
                                                <h4><a href="single-blog.html">Gravida luctus lorem accumsan est massa mauris.</a></h4>
                                                <p><a href="single-blog.html">26-10-18</a></p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- Recent Post Widget End -->

                                <!-- Facebook Like Box Widget Start -->
                                <div class="sidebar-widget widget_fb_like_box">
                                    <h3 class="widget-title">Facebook Like Box</h3>
                                    <div class="fb-page">
                                        <iframe src="https://www.facebook.com/plugins/page.php?href=https%3A%2F%2Fwww.facebook.com%2Fhastechitlimited%2F&amp;tabs=timeline&amp;width=340&amp;height=500&amp;small_header=false&amp;adapt_container_width=false&amp;hide_cover=false&amp;show_facepile=true&amp;appId" height="280" style="border:none;overflow:hidden"></iframe>
                                    </div>
                                </div>
                                <!-- Facebook Like Box Widget End -->

                                <!-- Twitter Feed Widget Start -->
                                <div class="sidebar-widget twitter-feed-widget">
                                    <h3 class="widget-title">Latest Twitter Feeds</h3>
                                    <ul class="twitter-feed">
                                        <li>
                                            <div class="twitter-feed__avatar">
                                                <img src="assets/img/others/comment-icon.png" alt="avatar">
                                            </div>
                                            <div class="twitter-feed__info">
                                                <div class="twitter-feed__header">
                                                    <a href="#"><strong>Keving Sobo</strong></a>
                                                    <a href="#">@hastech</a>
                                                </div>
                                                <div class="twitter-feed__content">
                                                    <p>Our best WordPress theme for your online store is here https://t.co/BYA8Bn8A6f https://t.co/qtVhWOH5PU </p>
                                                </div>
                                                <div class="twitter-feed__footer">
                                                    <a href="#">Sep 23</a>
                                                    <a href="#">reply</a>
                                                    <a href="#">retweet</a>
                                                    <a href="#">favorite</a>
                                                    <a href="#">2 years ago</a>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="twitter-feed__avatar">
                                                <img src="assets/img/others/comment-icon.png" alt="avatar">
                                            </div>
                                            <div class="twitter-feed__info">
                                                <div class="twitter-feed__header">
                                                    <a href="#"><strong>Keving Sobo</strong></a>
                                                    <a href="#">@hastech</a>
                                                </div>
                                                <div class="twitter-feed__content">
                                                    <p>Our best WordPress theme for your online store is here https://t.co/BYA8Bn8A6f https://t.co/qtVhWOH5PU </p>
                                                </div>
                                                <div class="twitter-feed__footer">
                                                    <a href="#">Sep 23</a>
                                                    <a href="#">reply</a>
                                                    <a href="#">retweet</a>
                                                    <a href="#">favorite</a>
                                                    <a href="#">2 years ago</a>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="twitter-feed__avatar">
                                                <img src="assets/img/others/comment-icon.png" alt="avatar">
                                            </div>
                                            <div class="twitter-feed__info">
                                                <div class="twitter-feed__header">
                                                    <a href="#"><strong>Keving Sobo</strong></a>
                                                    <a href="#">@hastech</a>
                                                </div>
                                                <div class="twitter-feed__content">
                                                    <p>Our best WordPress theme for your online store is here https://t.co/BYA8Bn8A6f https://t.co/qtVhWOH5PU </p>
                                                </div>
                                                <div class="twitter-feed__footer">
                                                    <a href="#">Sep 23</a>
                                                    <a href="#">reply</a>
                                                    <a href="#">retweet</a>
                                                    <a href="#">favorite</a>
                                                    <a href="#">2 years ago</a>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <!-- Twitter Feed Widget End -->

                                <!-- Tags Widget Start -->
                                <div class="sidebar-widget tags-widget">
                                    <h3 class="widget-title">Tags</h3>
                                    <div class="widget_conent">
                                        <div class="tagcloud">
                                            <a href="blog.html">chilled</a>
                                            <a href="blog.html">dark</a>
                                            <a href="blog.html">euro</a>
                                            <a href="blog.html">fashion</a>
                                            <a href="blog.html">food</a>
                                            <a href="blog.html">hardware</a>
                                            <a href="blog.html">hat</a>
                                            <a href="blog.html">hipster</a>
                                            <a href="blog.html">holidays</a>
                                            <a href="blog.html">light</a>
                                        </div>
                                    </div>
                                </div>
                                <!-- Tags Widget End -->
                            </aside>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Main Wrapper End -->
</asp:Content>
