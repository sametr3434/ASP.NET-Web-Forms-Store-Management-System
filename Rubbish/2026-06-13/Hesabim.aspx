<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Hesabim.aspx.cs" Inherits="Saat.Hesabim" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-content-wrapper">
        <div class="my-account-area pt--40 pb--80">
            <div class="container">
                <div class="d-flex justify-content-between align-items-center flex-wrap mb--30">
                    <h2 class="mb-0">Hesabım</h2>
                    <a href="/uye-cikis" class="btn btn-style-1 btn-sm">Çıkış Yap</a>
                </div>

                <div class="row mb-4">
                    <div class="col-md-6">
                        <div class="border p-3 h-100">
                            <h4>Üye Bilgileri</h4>
                            <p class="mb-1"><strong>Ad Soyad:</strong> <asp:Literal ID="ltrAdSoyad" runat="server"></asp:Literal></p>
                            <p class="mb-1"><strong>E-posta:</strong> <asp:Literal ID="ltrEposta" runat="server"></asp:Literal></p>
                            <p class="mb-1"><strong>Telefon:</strong> <asp:Literal ID="ltrTelefon" runat="server"></asp:Literal></p>
                            <p class="mb-0"><strong>Adres:</strong> <asp:Literal ID="ltrAdres" runat="server"></asp:Literal></p>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="border p-3 h-100">
                            <h4>Hızlı İşlemler</h4>
                            <p class="mb-2"><a href="/urunler">Alışverişe devam et</a></p>
                            <p class="mb-2"><a href="/sepet">Sepetim</a></p>
                            <p class="mb-0"><a href="#" data-bs-toggle="modal" data-bs-target="#aciklamalar">Sayfa hakkında bilgi</a></p>
                        </div>
                    </div>
                </div>

                <h3 id="siparisler" class="mb--20">Siparişlerim</h3>
                <asp:Panel ID="pnlSiparisYok" runat="server" Visible="false" CssClass="alert alert-info">
                    Henüz siparişiniz bulunmuyor. <a href="/urunler">Ürünleri inceleyin</a>.
                </asp:Panel>
                <asp:Panel ID="pnlSiparisler" runat="server">
                    <div class="table-responsive">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Sipariş No</th>
                                    <th>Tarih</th>
                                    <th>Ürünler</th>
                                    <th>Toplam</th>
                                    <th>Ödeme</th>
                                    <th>Durum</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSiparisler" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Duzelt(Eval("SiparisNo")) %></td>
                                            <td><%# Convert.ToDateTime(Eval("SiparisTarihi")).ToString("dd.MM.yyyy HH:mm") %></td>
                                            <td><%# Duzelt(Eval("UrunOzeti")) %></td>
                                            <td><%# Convert.ToDecimal(Eval("ToplamTutar")).ToString("N2") %> TL</td>
                                            <td><%# Duzelt(Eval("OdemeDurumu")) %></td>
                                            <td><%# Duzelt(Eval("SiparisDurumu")) %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <div class="modal fade" id="aciklamalar" tabindex="-1" aria-labelledby="aciklamalarLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="aciklamalarLabel">Hesabım</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Kapat"></button>
                </div>
                <div class="modal-body">
                    <p>Bu sayfada üyelik bilgilerinizi ve geçmiş siparişlerinizi görüntüleyebilirsiniz.</p>
                    <p>Demo hesabı: kullanıcı adı <strong>demo</strong>, şifre <strong>demo</strong>.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
