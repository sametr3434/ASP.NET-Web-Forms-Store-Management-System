<%@ Page Title="Ürün Düzenle" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="UrunDuzenle.aspx.cs" Inherits="Saat.Saat0604.UrunDuzenle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class="font-weight-semibold">Panel</span> - Ürün Düzenle</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/UrunListe.aspx" class="breadcrumb-item">Ürün Listesi</a>
                    <span class="breadcrumb-item active">Ürün Düzenle</span>
                </div>
            </div>
            <div class="header-elements">
                <div class="d-flex justify-content-center">
                    <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yardim"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">A&#231;&#305;klamalar</span></a>
                </div>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Ürün Düzenle</h5>
                <div class="header-elements">
                    <a href="UrunListe.aspx" class="btn btn-secondary btn-lg ml-2"><i class="icon-list2 mr-2"></i>Listeye Dön</a>
                </div>
            </div>
            <div class="card-body">
                <ul class="nav nav-tabs nav-tabs-highlight">
                    <li class="nav-item"><a href="#urun-bilgileri" class="nav-link active" data-toggle="tab"><i class="icon-info3 mr-2"></i>Ürün Bilgileri</a></li>
                    <li class="nav-item"><a href="#kategoriler" class="nav-link" data-toggle="tab"><i class="icon-tree7 mr-2"></i>Kategoriler</a></li>
                    <li class="nav-item"><a href="#numara-renk" class="nav-link" data-toggle="tab"><i class="icon-grid6 mr-2"></i>Numara/Renk</a></li>
                    <li class="nav-item"><a href="#satis-bilgileri" class="nav-link" data-toggle="tab"><i class="icon-stats-bars mr-2"></i>Satış Bilgileri</a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane fade show active" id="urun-bilgileri">
                        <asp:Panel ID="pnlUrun" runat="server" CssClass="mt-3">
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Ürün Adı</label>
                                <div class="col-sm-6"><asp:TextBox ID="txtUrunAdi" runat="server" class="form-control"></asp:TextBox></div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Ürün Kodu</label>
                                <div class="col-sm-3"><asp:TextBox ID="txtUrunKodu" runat="server" class="form-control"></asp:TextBox></div>
                                <label class="col-sm-2 control-label text-right" style="margin-top: 8px;">Barkod</label>
                                <div class="col-sm-3"><asp:TextBox ID="txtBarkod" runat="server" class="form-control"></asp:TextBox></div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Marka</label>
                                <div class="col-sm-6"><asp:DropDownList ID="ddlMarka" runat="server" class="form-control select-search" data-fouc=""></asp:DropDownList></div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Fiyat (TL)</label>
                                <div class="col-sm-3"><asp:TextBox ID="txtFiyat" runat="server" class="form-control"></asp:TextBox></div>
                                <label class="col-sm-2 control-label text-right" style="margin-top: 8px;">&#304;ndirimli (TL)</label>
                                <div class="col-sm-3"><asp:TextBox ID="txtIndirimli" runat="server" class="form-control"></asp:TextBox></div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Fiyat (USD)</label>
                                <div class="col-sm-3"><asp:TextBox ID="txtFiyatUSD" runat="server" class="form-control"></asp:TextBox></div>
                                <label class="col-sm-2 control-label text-right" style="margin-top: 8px;">&#304;ndirimli (USD)</label>
                                <div class="col-sm-3"><asp:TextBox ID="txtIndirimliUSD" runat="server" class="form-control"></asp:TextBox></div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Fiyat (EUR)</label>
                                <div class="col-sm-3"><asp:TextBox ID="txtFiyatEUR" runat="server" class="form-control"></asp:TextBox></div>
                                <label class="col-sm-2 control-label text-right" style="margin-top: 8px;">&#304;ndirimli (EUR)</label>
                                <div class="col-sm-3"><asp:TextBox ID="txtIndirimliEUR" runat="server" class="form-control"></asp:TextBox></div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Stok</label>
                                <div class="col-sm-6"><asp:TextBox ID="txtStok" runat="server" class="form-control"></asp:TextBox></div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Durum</label>
                                <div class="col-sm-6">
                                    <div class="row no-gutters">
                                        <div class="col-md-3 pr-3 mb-2">
                                            <label class="form-check-label d-inline-flex align-items-center"><asp:CheckBox ID="chkAktif" runat="server" class="form-check-input mr-1" /> Aktif</label>
                                        </div>
                                        <div class="col-md-3 pr-3 mb-2">
                                            <label class="form-check-label d-inline-flex align-items-center"><asp:CheckBox ID="chkVitrin" runat="server" class="form-check-input mr-1" /> Vitrin</label>
                                        </div>
                                        <div class="col-md-3 pr-3 mb-2">
                                            <label class="form-check-label d-inline-flex align-items-center"><asp:CheckBox ID="chkOnerilen" runat="server" class="form-check-input mr-1" /> Önerilen</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 control-label" style="margin-top: 8px;">Ana Resim</label>
                                <div class="col-sm-6"><asp:FileUpload ID="fuAnaResim" runat="server" class="form-control" /></div>
                            </div>
                            <div class="form-group row">
                                <label class="col-form-label col-lg-2"></label>
                                <div class="col-lg-10">
                                    <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-success" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                                    <asp:Label ID="lblUrunID" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="tab-pane fade" id="kategoriler">
                        <asp:UpdatePanel ID="upKategoriler" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="mt-3">
                                    <div class="form-row align-items-end">
                                        <div class="col-md-6">
                                            <label>Kategori Ekle</label>
                                            <asp:DropDownList ID="ddlKategori" runat="server" class="form-control select-search" data-fouc=""></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:LinkButton ID="btnKategoriEkle" runat="server" class="btn btn-primary" OnClick="btnKategoriEkle_Click"><i class="icon-plus22 mr-2"></i>Ekle</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="table-responsive mt-3">
                                        <table class="table table-striped">
                                            <thead><tr><th>#</th><th>Kategori</th><th class="text-right">İşlem</th></tr></thead>
                                            <tbody>
                                                <asp:Repeater ID="rptKategoriler" runat="server" OnItemCommand="rptKategoriler_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("ID") %></td>
                                                            <td><%# Duzelt(Eval("KategoriAdi")) %></td>
                                                            <td class="text-right">
                                                                <asp:LinkButton ID="lbKatSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("UKID") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Bu kategoriyi bu üründen kaldırmak istediğinize emin misiniz?');"><i class="icon-trash mr-2"></i>Kaldır</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnKategoriEkle" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="rptKategoriler" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane fade" id="numara-renk">
                        <asp:UpdatePanel ID="upVaryant" runat="server" UpdateMode="Conditional" CssClass="js-up-varyant">
                            <ContentTemplate>
                                <div class="mt-3">
                                    <div class="form-row align-items-end">
                                        <div class="col-md-3">
                                            <label>Numara</label>
                                            <%-- select-search kullanilmaz: gizli sekme + UpdatePanel ile custom.js erken Select2 bozuyor; asagidaki script varyant-select2 baglar --%>
                                            <asp:DropDownList ID="ddlKasaBoyu" runat="server" CssClass="form-control varyant-select2"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Renk</label>
                                            <asp:DropDownList ID="ddlRenk" runat="server" CssClass="form-control varyant-select2"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <label>Stok</label>
                                            <asp:TextBox ID="txtVaryantStok" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-2">
                                            <label>Barkod</label>
                                            <asp:TextBox ID="txtVaryantBarkod" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-2">
                                            <label>Fark TL</label>
                                            <asp:TextBox ID="txtFiyatFarki" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-2">
                                            <label>Fark USD</label>
                                            <asp:TextBox ID="txtFiyatFarkiUSD" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-2">
                                            <label>Fark EUR</label>
                                            <asp:TextBox ID="txtFiyatFarkiEUR" runat="server" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="mt-2">
                                        <asp:LinkButton ID="btnVaryantEkle" runat="server" class="btn btn-primary" OnClick="btnVaryantEkle_Click"><i class="icon-plus22 mr-2"></i>Varyant Ekle</asp:LinkButton>
                                    </div>
                                    <div class="table-responsive mt-3">
                                        <table class="table table-striped">
                                            <thead><tr><th>#</th><th>Numara</th><th>Renk</th><th>Stok</th><th>Barkod</th><th>Fark TL</th><th>Fark USD</th><th>Fark EUR</th><th class="text-right">&#304;&#351;lem</th></tr></thead>
                                            <tbody>
                                                <asp:Repeater ID="rptVaryantlar" runat="server" OnItemCommand="rptVaryantlar_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("ID") %></td>
                                                            <td><%# Duzelt(Eval("kasano")) %></td>
                                                            <td><%# Duzelt(Eval("RenkAdi")) %></td>
                                                            <td><%# Eval("StokAdedi") %></td>
                                                            <td><%# Duzelt(Eval("Barkod")) %></td>
                                                            <td><%# Eval("FiyatFarki") != DBNull.Value ? Convert.ToDecimal(Eval("FiyatFarki")).ToString("N2") : "-" %></td>
                                                            <td><%# Eval("FiyatFarkiUSD") != DBNull.Value ? Convert.ToDecimal(Eval("FiyatFarkiUSD")).ToString("N2") : "-" %></td>
                                                            <td><%# Eval("FiyatFarkiEUR") != DBNull.Value ? Convert.ToDecimal(Eval("FiyatFarkiEUR")).ToString("N2") : "-" %></td>
                                                            <td class="text-right">
                                                                <asp:LinkButton ID="lbVarSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Bu varyantı silmek istediğinize emin misiniz?');"><i class="icon-trash mr-2"></i>Sil</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnVaryantEkle" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="rptVaryantlar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="tab-pane fade" id="satis-bilgileri">
                        <div class="mt-3">
                            <asp:LinkButton ID="btnSatisOzeti" runat="server" CssClass="btn btn-info" OnClick="btnSatisOzeti_Click"><i class="icon-stats-bars mr-2"></i>Satış Özetini Göster</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="satisModal" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title">Satış Özeti</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <asp:Repeater ID="rptSatisOzet" runat="server">
                        <HeaderTemplate>
                            <table class="table table-striped"><thead><tr><th>Tarih</th><th>Numara</th><th>Renk</th><th>Adet</th></tr></thead><tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Tarih","{0:dd.MM.yyyy}") %></td>
                                <td><%# Duzelt(Eval("kasano")) %></td>
                                <td><%# Duzelt(Eval("Renk")) %></td>
                                <td><%# Eval("Adet") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody></table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlSatisYok" runat="server" Visible="false" CssClass="text-muted">Satış bulunamadı.</asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title">A&#231;&#305;klamalar</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <p class="mb-2"><b>&#220;r&#252;n Bilgileri (alan a&#231;&#305;klamalar&#305;):</b></p>
                    <ul class="mb-3">
                        <li><b>&#220;r&#252;n Ad&#305;:</b> Sitede g&#246;r&#252;nen tam ad.</li>
                        <li><b>&#220;r&#252;n Kodu:</b> Dahili/raf kodu (opsiyonel). Arama ve sipari&#351; detaylar&#305;nda g&#246;r&#252;n&#252;r.</li>
                        <li><b>Barkod:</b> Tekil barkod (opsiyonel). Varyant i&#231;in ayr&#305; barkod da tan&#305;mlanabilir.</li>
                        <li><b>Marka:</b> &#220;r&#252;n&#252;n ait oldu&#287;u marka. Bo&#351; b&#305;rak&#305;labilir.</li>
                        <li><b>Fiyat:</b> Sat&#305;&#351; fiyat&#305; (TL). Ondal&#305;k i&#231;in virg&#252;l veya nokta girilebilir (&#246;r. 199,90 / 199.90).</li>
                        <li><b>&#304;ndirimli:</b> &#304;ndirime giren fiyat (TL). <i>Bo&#351; ise</i> indirim uygulanmaz; <i>de&#287;er girilirse</i> liste/detayda indirimli g&#246;r&#252;n&#252;r. &#214;neri: &#304;ndirimli &le; Fiyat.</li>
                        <li><b>Stok:</b> Ana stok adedi. Varyant stoklar&#305; ayr&#305; ekranda tan&#305;mlan&#305;r; burada genel g&#246;stergedir.</li>
                        <li><b>Durumlar:</b> <b>Aktif</b> (sitede yay&#305;nda), <b>Vitrin</b> (ana sayfa/&#246;ne &#231;&#305;kan), <b>&#214;nerilen</b> (ilgili bloklar).</li>
                        <li><b>Ana Resim:</b> Liste ve detay i&#231;in kapak g&#246;rseli. Ek g&#246;rseller i&#231;in <i>&#220;r&#252;n Resimleri</i>.</li>
                    </ul>
                    <p class="mb-2"><b>Kategoriler:</b> &#220;r&#252;n birden fazla kategoriye ba&#287;lanabilir. A&#351;a&#287;&#305;daki listeden eklenen kategorileri <i>Kald&#305;r</i> ile &#231;&#305;karabilirsiniz.</p>
                    <p class="mb-2"><b>Numara/Renk (Varyant):</b> Numara ve renk kombinasyonlar&#305; varyant olarak eklenir. <b>Stok</b>, <b>Barkod</b> (opsiyonel) ve <b>Fiyat Fark&#305;</b> (opsiyonel, +/− yaz&#305;labilir) girilebilir. Fiyat fark&#305; bo&#351;/0 ise ana fiyata eklenmez.</p>
                    <p><b>Sat&#305;&#351; Bilgileri:</b> Tarih–numara–renk baz&#305;nda adet toplamlar&#305; modalda g&#246;r&#252;nt&#252;lenir; h&#305;zl&#305; trend g&#246;r&#252;n&#252;m&#252; i&#231;in kullan&#305;n.</p>
                </div>
            </div>
        </div>
    </div>
    <style type="text/css">
        #numara-renk .varyant-select2 + .select2-container { width: 100% !important; min-width: 0; }
        #numara-renk .select2-container .select2-selection--single { min-height: 2.375rem; }
    </style>
    <script type="text/javascript">
        (function () {
            function varyantSelect2Opts() {
                return {
                    width: '100%',
                    dropdownParent: jQuery('body'),
                    minimumResultsForSearch: 0
                };
            }
            function reinitVaryantSelect2() {
                if (typeof jQuery === 'undefined' || !jQuery.fn.select2) return;
                var $root = jQuery('#numara-renk');
                if (!$root.length) return;
                $root.find('select.varyant-select2').each(function () {
                    var $el = jQuery(this);
                    try {
                        if ($el.data('select2')) $el.select2('destroy');
                    } catch (ex) { }
                });
                $root.find('select.varyant-select2').select2(varyantSelect2Opts());
            }
            function scheduleReinit() {
                window.setTimeout(reinitVaryantSelect2, 0);
            }
            if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    scheduleReinit();
                });
            }
            jQuery(document).on('shown.bs.tab', 'a[data-toggle="tab"][href="#numara-renk"]', function () {
                scheduleReinit();
            });
            jQuery(document).on('click', 'a[data-toggle="tab"][href="#numara-renk"]', function () {
                window.setTimeout(reinitVaryantSelect2, 150);
            });
            jQuery(function () {
                if (jQuery('#numara-renk').hasClass('active') && jQuery('#numara-renk').hasClass('show')) scheduleReinit();
            });
        })();
    </script>
</asp:Content>
