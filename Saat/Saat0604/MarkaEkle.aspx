<%@ Page Title="Marka Yönetimi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="MarkaEkle.aspx.cs" Inherits="Saat.Saat0604.MarkaEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class="font-weight-semibold">Panel</span> - Marka Yönetimi</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/MarkaListe.aspx" class="breadcrumb-item">Marka Listesi</a>
                    <span class="breadcrumb-item active">Marka Yönetimi</span>
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
                <h5 class="card-title">Marka Yönetimi</h5>
                <div class="header-elements">
                    <a href="MarkaListe.aspx" class="btn btn-secondary btn-lg ml-2"><i class="icon-list2 mr-2"></i>Listeye Dön</a>
                </div>
            </div>

            <div class="card-body">
                <asp:Panel ID="pnlForm" runat="server">
                    <fieldset class="mb-3">
                        <legend class="text-uppercase font-size-sm font-weight-bold">Marka Bilgileri</legend>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Marka Adı</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtMarkaAdi" runat="server" class="form-control" placeholder="Marka adını giriniz..."></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Açıklama</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtAciklama" runat="server" class="form-control" TextMode="MultiLine" Rows="4" placeholder="Marka açıklaması..."></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Sıralama</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtSiralama" runat="server" class="form-control" placeholder="Sıralama numarası..."></asp:TextBox>
                                <span class="form-text text-muted">Düşük değerler önce gelir (0, 1, 2...)</span>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Durum</label>
                            <div class="col-sm-6">
                                <div class="form-check">
                                    <label class="form-check-label">
                                        <asp:CheckBox ID="chkAktif" runat="server" class="form-check-input" Checked="true" />
                                        Aktif (Sitede görünsün)
                                    </label>
                                </div>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Logo</label>
                            <div class="col-sm-6">
                                <asp:FileUpload ID="fuLogo" runat="server" class="form-control" />
                                <span class="form-text text-muted">JPG, PNG formatında, maksimum 2MB</span>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkKaydet" runat="server" class="btn btn-success btn-lg" Visible="true" OnClick="lnkKaydet_Click"><i class="icon-checkmark3 mr-2"></i>Kaydet</asp:LinkButton>
                                <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" Visible="false" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                                <asp:LinkButton ID="lnkIptal" runat="server" class="btn btn-secondary btn-lg ml-2" OnClick="lnkIptal_Click"><i class="icon-cross2 mr-2"></i>İptal</asp:LinkButton>
                                <asp:Label ID="lblMarkaID" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </fieldset>
                </asp:Panel>
            </div>
        </div>
    </div>

    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title">A&#231;&#305;klamalar</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <h6 class="font-weight-semibold">Marka Yönetim Sayfas&#305;</h6>
                    <p>Bu sayfa, marka ekleme, d&#252;zenleme ve y&#246;netimi i&#231;in kullan&#305;l&#305;r.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
