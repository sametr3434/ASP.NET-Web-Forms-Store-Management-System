<%@ Page Title="" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="Anasayfa.aspx.cs" Inherits="Saat.Saat0604.Anasayfa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblKullaniciID" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblKullaniciAdi" runat="server" Visible="False"></asp:Label>

    <!-- Page header -->
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-home4 mr-2"></i> <span class="font-weight-semibold">Panel Anasayfa</span></h4>
                <a href="#" class="header-elements-toggle text-default d-md-none"><i class="icon-more"></i></a>
            </div>
            <div class="header-elements">
                <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#modalYardim" title="Yardım">
                    <i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">Yardım</span>
                </a>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Anasayfa</a>
                </div>
            </div>
        </div>
    </div>

    <div class="content pt-0">
        <!-- İstatistik kartları -->
        <div class="row">
            <div class="col-sm-6 col-xl-3">
                <div class="card card-body bg-primary-400 has-bg-image">
                    <div class="media">
                        <div class="mr-3 align-self-center">
                            <i class="icon-basket icon-3x opacity-75"></i>
                        </div>
                        <div class="media-body text-right">
                            <h3 class="font-weight-semibold mb-0"><asp:Literal ID="ltrToplamSiparis" runat="server" Text="0" /></h3>
                            <span class="text-uppercase font-size-xs opacity-75">Toplam Sipariş</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-xl-3">
                <div class="card card-body bg-success-400 has-bg-image">
                    <div class="media">
                        <div class="mr-3 align-self-center">
                            <i class="icon-coins icon-3x opacity-75"></i>
                        </div>
                        <div class="media-body text-right">
                            <h3 class="font-weight-semibold mb-0"><asp:Literal ID="ltrToplamCiro" runat="server" Text="0 ₺" /></h3>
                            <span class="text-uppercase font-size-xs opacity-75">Ciro (TL sipari&#351;leri)</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-xl-3">
                <div class="card card-body bg-info-400 has-bg-image">
                    <div class="media">
                        <div class="mr-3 align-self-center">
                            <i class="icon-users4 icon-3x opacity-75"></i>
                        </div>
                        <div class="media-body text-right">
                            <h3 class="font-weight-semibold mb-0"><asp:Literal ID="ltrMusteriSayisi" runat="server" Text="0" /></h3>
                            <span class="text-uppercase font-size-xs opacity-75">Müşteri</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-xl-3">
                <div class="card card-body bg-warning-400 has-bg-image">
                    <div class="media">
                        <div class="mr-3 align-self-center">
                            <i class="icon-cart5 icon-3x opacity-75"></i>
                        </div>
                        <div class="media-body text-right">
                            <h3 class="font-weight-semibold mb-0"><asp:Literal ID="ltrUrunSayisi" runat="server" Text="0" /></h3>
                            <span class="text-uppercase font-size-xs opacity-75">Aktif Ürün</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Grafikler -->
        <div class="row">
            <div class="col-xl-8">
                <div class="card">
                    <div class="card-header header-elements-inline">
                        <h5 class="card-title">Aylık Satış Grafiği</h5>
                    </div>
                    <div class="card-body">
                        <div id="chart_aylik_satis" style="height: 280px;"></div>
                    </div>
                </div>
            </div>
            <div class="col-xl-4">
                <div class="card">
                    <div class="card-header header-elements-inline">
                        <h5 class="card-title">Sipariş Durumları</h5>
                    </div>
                    <div class="card-body text-center">
                        <div id="chart_siparis_donut" style="height: 260px;"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Kritik stok ve son siparişler -->
        <div class="row">
            <div class="col-xl-6">
                <div class="card">
                    <div class="card-header header-elements-inline">
                        <h5 class="card-title">Kritik Stok Uyarıları</h5>
                        <div class="header-elements">
                            <a href="UrunListe.aspx" class="btn btn-sm btn-light">Tümünü Gör</a>
                        </div>
                    </div>
                    <div class="card-body p-0">
                        <asp:Panel ID="pnlKritikStokTable" runat="server">
                            <asp:Repeater ID="rptKritikStok" runat="server">
                                <HeaderTemplate>
                                    <div class="table-responsive">
                                        <table class="table table-hover table-striped mb-0">
                                            <thead><tr><th>Ürün</th><th>Stok</th><th>Kritik</th><th></th></tr></thead>
                                            <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Duzelt(Eval("UrunAdi")) %></td>
                                        <td><span class="badge badge-danger"><%# Eval("StokAdedi") %></span></td>
                                        <td><%# Eval("KritikStokSeviyesi") %></td>
                                        <td><a href='UrunListe.aspx' class="btn btn-sm btn-outline-primary">Liste</a></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                        <asp:Panel ID="pnlKritikStokYok" runat="server" CssClass="p-3 text-muted text-center" Visible="false">
                            Kritik stok seviyesinde ürün bulunmuyor.
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="col-xl-6">
                <div class="card">
                    <div class="card-header header-elements-inline">
                        <h5 class="card-title">Son Siparişler</h5>
                        <div class="header-elements">
                            <a href="SiparisListe.aspx" class="btn btn-sm btn-light">Tümünü Gör</a>
                        </div>
                    </div>
                    <div class="card-body p-0">
                        <asp:Panel ID="pnlSonSiparisTable" runat="server">
                            <asp:Repeater ID="rptSonSiparisler" runat="server">
                                <HeaderTemplate>
                                    <div class="table-responsive">
                                        <table class="table table-hover table-striped mb-0">
                                            <thead><tr><th>Sipariş No</th><th>Tarih</th><th>Tutar</th><th>Durum</th><th></th></tr></thead>
                                            <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Duzelt(Eval("SiparisNo")) %></td>
                                        <td><%# Eval("SiparisTarihi", "{0:dd.MM.yyyy}") %></td>
                                        <td><%# TutarPBAna(Eval("ToplamTutar"), Eval("ParaBirimi")) %></td>
                                        <td><span class="badge badge-secondary"><%# Duzelt(Eval("SiparisDurumu")) %></span></td>
                                        <td><a href='SiparisListe.aspx' class="btn btn-sm btn-outline-primary">Liste</a></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                        <asp:Panel ID="pnlSonSiparisYok" runat="server" CssClass="p-3 text-muted text-center" Visible="false">
                            Henüz sipariş bulunmuyor.
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Yardım Modal -->
    <div id="modalYardim" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title"><i class="icon-question3 mr-2"></i>SEO ve Panel A&#231;&#305;klamalar&#305;</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <p class="font-weight-semibold">Panel içindeki bilgilerin eksiksiz olması, SEO çalışmaları için oldukça önemlidir. Lütfen boş alan bırakmamaya özen gösteriniz.</p>
                    <p>Site üzerinde anahtar kelimeler ile ilgili yapılması gerekli bir kaç konu bulunmakta. Aşağıdaki açıklamaları dikkate alırsanız sitenizin Google, Yandex ve Bing gibi arama motorlarında görüntülenmesi daha hızlı ve sağlıklı olacaktır.</p>
                    <p><strong>Yönetim Panelindeki tüm sayfaları tek tek gezerek, Google Anahtar ve Google Açıklama alanlarının doldurulması gerekmektedir.</strong></p>
                    <p>Bu anahtarlar belirlenirken yapılması gereken işlemler şu şekilde olmalı:</p>
                    <ol>
                        <li>Sizden Google Anahtar istenen her alan için 7 farklı anahtar tanımlaması yapabilirsiniz.</li>
                        <li>Anahtar kelimelerin arasını virgülle ayırıyoruz. Örneğin: Tuzla Tasarım, Tuzla kullanımı iki farklı anahtar olarak hizmet vermektedir.</li>
                        <li>Anahtar kelimeleri düzenlerken ilk anahtar kelimeyi firma ismi olarak belirleyiniz. Tuzla Tasarım yada Tuzla Tasarım Yazılım gibi değişik varyasyonlarla kullanmanız avantajlı olacaktır.</li>
                        <li>İkinci anahtar kelime; size ulaşılmasını istediğiniz konumları belirlemek içindir. Yani size Tuzla'dan birilerinin ulaşmasını istiyorsanız, bunu hangi konuda size ulaşılmasını istiyorsanız o konunun ikinci anahtarı olarak belirleyiniz.</li>
                        <li>Giriş yapmış olduğunuz anahtar kelimeler 15-21 gün arasında Google tarafından tanınacaktır. Sitenin anahtar kelimelerini bu zamana göre periyodik olarak farklı tanımlamak hem Google botları tarafından indekslenmesini sağlayacaktır. Yanı sıra bu işlem Google tarafından artı değer olarak kabul edilecektir, hem de Google üzerinde çok değişik kelimelerle bulunabilme şansınızı arttıracaktır.</li>
                        <li>Diğer anahtar kelimeleri konuyla ilişkili olarak belirlemeniz gerekiyor. Örneğin Web Tasarımı için: Tuzla Tasarım, Tuzla, Web Tasarımı, İnternet Sitesi, Web Sitesi, Web Sayfası, İnternet Sayfası gibi.</li>
                        <li>Anahtar kelime belirlenirken en çok dikkat etmeniz gereken şey şudur: insanlar bu işlemi hangi kelimelerle arayabilir diye düşünerek genel aranabilecek kelimeleri tanımlamanız olacaktır.</li>
                        <li>Google Açıklama alanı ise; bir diğer en önemli konumuz. Google açıklama yazısı en fazla 160 karaktere kadar desteklenir ve sitenizin bulunması için önemlidir.</li>
                        <li>Girişini yaptığınız konu ile ilgili en net ve kısa ifadesi burada tanımlamanız gerekiyor.</li>
                        <li>Örneğin: Tuzla Tasarım Yazılım, Tuzla, Pendik, Gebze Bölgelerinde internet sitesi hizmetleri vermektedir. İhtiyaçlarınız için lütfen bize ulaşın 0 538 312 25 80 gibi. (Yazdığınız yazıların karakter sayısı için <a href="https://translate.google.com.tr/" target="_blank">Google Çeviri</a> sayfasında yazıları yazarak kaç karakter olduğunu görebilirsiniz.)</li>
                    </ol>
                    <p class="mb-0">Ekstra bilgi için lütfen bize ulaşınız. İyi çalışmalar. Hayırlı Olsun Dileklerimizle.</p>
                    <p class="text-primary font-weight-semibold mt-2">Tuzla Tasarım Ailesi</p>
                </div>
            </div>
        </div>
    </div>

    <!-- Chart data (server'dan gelen JSON) -->
    <asp:HiddenField ID="hfAylikSatisData" runat="server" />
    <asp:HiddenField ID="hfSiparisDurumData" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CustomScript" runat="server">
    <script src="/Saat0604/global_assets/js/plugins/visualization/d3/d3_tooltip.js"></script>
    <script>
        var _dashboardAylikId = '<%= hfAylikSatisData.ClientID %>';
        var _dashboardSiparisId = '<%= hfSiparisDurumData.ClientID %>';
    </script>
    <script src="/Saat0604/assets/js/anasayfa_dashboard.js"></script>
</asp:Content>
