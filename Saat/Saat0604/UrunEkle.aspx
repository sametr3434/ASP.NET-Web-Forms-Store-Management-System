<%@ Page Title="Ürün Ekle" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="UrunEkle.aspx.cs" Inherits="Saat.Saat0604.UrunEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class class="font-weight-semibold">Panel</span> - Ürün Ekle</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/UrunListe.aspx" class="breadcrumb-item">Ürün Listesi</a>
                    <span class="breadcrumb-item active">Ürün Ekle</span>
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
                <h5 class="card-title">Ürün Bilgileri</h5>
                <div class="header-elements">
                    <a href="UrunListe.aspx" class="btn btn-secondary btn-lg ml-2"><i class="icon-list2 mr-2"></i>Listeye Dön</a>
                </div>
            </div>
            <div class="card-body">
                <asp:Panel ID="pnlForm" runat="server">
                    <fieldset class="mb-3">
                        <legend class="text-uppercase font-size-sm font-weight-bold">Temel Bilgiler</legend>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Ürün Adı</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtUrunAdi" runat="server" class="form-control" placeholder="Ürün adını giriniz..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Ürün Kodu</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtUrunKodu" runat="server" class="form-control" placeholder="Ürün kodu..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Barkod</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtBarkod" runat="server" class="form-control" placeholder="Barkod..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Marka</label>
                            <div class="col-sm-6">
                                <asp:DropDownList ID="ddlMarka" runat="server" class="form-control select-search" data-fouc="" AppendDataBoundItems="true">
                                    <asp:ListItem Value="">Seçiniz</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Fiyat (TL)</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtFiyat" runat="server" class="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label text-right" style="margin-top: 8px;">&#304;ndirimli (TL)</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtIndirimli" runat="server" class="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Fiyat (USD)</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtFiyatUSD" runat="server" class="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label text-right" style="margin-top: 8px;">&#304;ndirimli (USD)</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtIndirimliUSD" runat="server" class="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Fiyat (EUR)</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtFiyatEUR" runat="server" class="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label text-right" style="margin-top: 8px;">&#304;ndirimli (EUR)</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtIndirimliEUR" runat="server" class="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Stok</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtStok" runat="server" class="form-control" placeholder="Stok adedi..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Durum</label>
                            <div class="col-sm-6">
                                <div class="row no-gutters">
                                    <div class="col-md-3 pr-3 mb-2">
                                        <label class="form-check-label d-inline-flex align-items-center"><asp:CheckBox ID="chkAktif" runat="server" class="form-check-input mr-1" Checked="true" /> Aktif</label>
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
                            <div class="col-sm-6">
                                <asp:FileUpload ID="fuAnaResim" runat="server" class="form-control" />
                                <span class="form-text text-muted">JPG/PNG, max 2MB</span>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkKaydet" runat="server" class="btn btn-success btn-lg" OnClick="lnkKaydet_Click"><i class="icon-checkmark3 mr-2"></i>Kaydet ve Düzenlemeye Geç</asp:LinkButton>
                            </div>
                        </div>
                    </fieldset>
                </asp:Panel>
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
                    <p class="mb-2"><b>Kategoriler:</b> Kaydettikten sonra &#220;r&#252;n D&#252;zenle ekran&#305;ndaki <i>Kategoriler</i> sekmesinden &#231;oklu kategori ba&#287;lant&#305;s&#305; yapabilirsiniz.</p>
                    <p class="mb-2"><b>Numara/Renk (Varyant):</b> Kaydettikten sonra <i>Numara/Renk</i> sekmesinde varyant ekleyebilir; stok/barkod/fiyat fark&#305; tan&#305;mlayabilirsiniz.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
