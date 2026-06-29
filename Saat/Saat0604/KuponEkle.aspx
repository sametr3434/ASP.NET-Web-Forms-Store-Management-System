<%@ Page Title="Kupon Yönetimi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="KuponEkle.aspx.cs" Inherits="Saat.Saat0604.KuponEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-ticket mr-2"></i> <span class="font-weight-semibold">Panel</span> - Kupon Yönetimi</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/KuponListe.aspx" class="breadcrumb-item">Kupon Listesi</a>
                    <span class="breadcrumb-item active">Kupon Yönetimi</span>
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
                <h5 class="card-title">Kupon Bilgileri</h5>
                <div class="header-elements">
                    <a href="KuponListe.aspx" class="btn btn-secondary btn-lg ml-2"><i class="icon-list2 mr-2"></i>Listeye Dön</a>
                </div>
            </div>

            <div class="card-body">
                <asp:Panel ID="pnlForm" runat="server">
                    <fieldset class="mb-3">
                        <legend class="text-uppercase font-size-sm font-weight-bold">Temel Bilgiler</legend>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Kupon Kodu</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtKuponKodu" runat="server" class="form-control" placeholder="Örn: INDIRIM20"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">İndirim Tipi</label>
                            <div class="col-sm-6">
                                <asp:DropDownList ID="ddlIndirimTipi" runat="server" class="form-control select-search" data-fouc="">
                                    <asp:ListItem Value="Yuzde">% Yüzde</asp:ListItem>
                                    <asp:ListItem Value="Tutar">Tutar</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">İndirim Değeri</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtIndirimDegeri" runat="server" class="form-control" placeholder="% veya TL"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Min. Sipariş Tutarı</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtMinTutar" runat="server" class="form-control" placeholder="Opsiyonel"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Başlangıç</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtBaslangic" runat="server" class="form-control" placeholder="gg.aa.yyyy"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label text-right" style="margin-top: 8px;">Bitiş</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtBitis" runat="server" class="form-control" placeholder="gg.aa.yyyy"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Kullanım Limiti</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtLimit" runat="server" class="form-control" placeholder="Toplam kullanım limiti (opsiyonel)"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Durum</label>
                            <div class="col-sm-6">
                                <label class="form-check-label d-inline-flex align-items-center">
                                    <asp:CheckBox ID="chkAktif" runat="server" class="form-check-input mr-1" Checked="true" /> Aktif
                                </label>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkKaydet" runat="server" class="btn btn-success btn-lg" Visible="true" OnClick="lnkKaydet_Click"><i class="icon-checkmark3 mr-2"></i>Kaydet</asp:LinkButton>
                                <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" Visible="false" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                                <asp:LinkButton ID="lnkIptal" runat="server" class="btn btn-secondary btn-lg ml-2" OnClick="lnkIptal_Click"><i class="icon-cross2 mr-2"></i>İptal</asp:LinkButton>
                                <asp:Label ID="lblKuponID" runat="server" Visible="false"></asp:Label>
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
                    <p><b>Kupon Kodu:</b> Kullanıcıların ödeme sayfasında gireceği kod.</p>
                    <p><b>İndirim Tipi:</b> Yüzde veya Tutar bazlı indirim.</p>
                    <p><b>İndirim Değeri:</b> Tipine göre % veya TL.</p>
                    <p><b>Min. Sipariş Tutarı:</b> Bu tutarın üzerindeki siparişlerde geçerli.</p>
                    <p><b>Tarih Aralığı:</b> Başlangıç–Bitiş arasında geçerlidir.</p>
                    <p><b>Kullanım Limiti:</b> Toplamda en fazla kaç kez kullanılabilir.</p>
                    <p><b>Aktif:</b> İşaretli değilse kupon çalışmaz.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
