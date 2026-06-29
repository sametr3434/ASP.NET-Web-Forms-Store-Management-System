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
                            <p class="mb-0"><strong>Varsayılan adres:</strong> <asp:Literal ID="ltrAdres" runat="server"></asp:Literal></p>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="border p-3 h-100">
                            <h4>Hızlı İşlemler</h4>
                            <p class="mb-2"><a href="/urunler">Alışverişe devam et</a></p>
                            <p class="mb-2"><a href="/sepet">Sepetim</a></p>
                            <p class="mb-0"><a href="#adresler">Adreslerimi yönet</a></p>
                        </div>
                    </div>
                </div>

                <h3 id="adresler" class="mb--20">Adreslerim</h3>
                <asp:Panel ID="pnlAdresYok" runat="server" Visible="false" CssClass="alert alert-info mb-3">
                    Kayıtlı adresiniz yok. Aşağıdan yeni adres ekleyebilirsiniz.
                </asp:Panel>
                <asp:Panel ID="pnlAdresler" runat="server">
                    <div class="table-responsive mb-4">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Başlık</th>
                                    <th>Adres</th>
                                    <th>Telefon</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptAdresler" runat="server" OnItemCommand="rptAdresler_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Duzelt(Eval("Baslik")) %>
                                                <%# Convert.ToBoolean(Eval("Varsayilan")) ? " <span class=\"badge bg-primary\">Varsayılan</span>" : "" %>
                                            </td>
                                            <td>
                                                <%# Duzelt(Eval("Adres")) %><br />
                                                <%# Duzelt(Eval("Ilce")) %><%# Eval("Il") != null && Eval("Il").ToString() != "" ? ", " + Duzelt(Eval("Il")) : "" %>
                                            </td>
                                            <td><%# Duzelt(Eval("Telefon")) %></td>
                                            <td class="text-nowrap">
                                                <asp:LinkButton ID="lnkVarsayilan" runat="server" CssClass="btn btn-sm btn-outline-secondary me-1"
                                                    CommandName="Varsayilan" CommandArgument='<%# Eval("ID") %>'
                                                    Visible='<%# !Convert.ToBoolean(Eval("Varsayilan")) %>' Text="Varsayılan yap" />
                                                <asp:LinkButton ID="lnkSil" runat="server" CssClass="btn btn-sm btn-outline-danger"
                                                    CommandName="Sil" CommandArgument='<%# Eval("ID") %>'
                                                    OnClientClick="return confirm('Bu adres silinsin mi?');" Text="Sil" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </asp:Panel>

                <div class="border p-3 mb-4">
                    <h4 class="mb-3">Yeni Adres Ekle</h4>
                    <div class="row">
                        <div class="col-md-4 form-group mb-3">
                            <label>Başlık</label>
                            <asp:TextBox ID="txtAdresBaslik" runat="server" CssClass="form-control" placeholder="Ev, İş..." />
                        </div>
                        <div class="col-md-4 form-group mb-3">
                            <label>Telefon</label>
                            <asp:TextBox ID="txtYeniTelefon" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-4 form-group mb-3 d-flex align-items-end">
                            <asp:CheckBox ID="chkVarsayilan" runat="server" CssClass="mb-2" Text=" Varsayılan adres olsun" />
                        </div>
                    </div>
                    <div class="form-group mb-3">
                        <label>Adres</label>
                        <asp:TextBox ID="txtYeniAdres" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                    </div>
                    <div class="row">
                        <div class="col-md-4 form-group mb-3">
                            <label>İl</label>
                            <asp:TextBox ID="txtYeniIl" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-4 form-group mb-3">
                            <label>İlçe</label>
                            <asp:TextBox ID="txtYeniIlce" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-4 form-group mb-3">
                            <label>Posta kodu</label>
                            <asp:TextBox ID="txtYeniPostaKodu" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <asp:Button ID="btnAdresEkle" runat="server" CssClass="btn btn-style-1" Text="Adres Ekle" OnClick="btnAdresEkle_Click" />
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
                    <p>Bu sayfada üyelik bilgilerinizi, kayıtlı adreslerinizi ve geçmiş siparişlerinizi görüntüleyebilirsiniz.</p>
                    <p>Demo hesabı: kullanıcı adı <strong>demo</strong>, şifre <strong>demo</strong>.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
