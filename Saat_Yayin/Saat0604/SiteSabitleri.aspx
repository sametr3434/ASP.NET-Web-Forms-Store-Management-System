<%@ Page Title="" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="SiteSabitleri.aspx.cs" Inherits="Saat.Saat0604.SiteSabitleri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblKullaniciID" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblKullaniciAdi" runat="server" Visible="False"></asp:Label>
    <div class="page-header page-header-light">
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a class="breadcrumb-item" href="Anasayfa.aspx"><i class="icon-home2 mr-2"></i>Anasayfa</a>
                    <a class="breadcrumb-item" href="#">Site Sabitleri</a>
                </div>
            </div>
            <div class="header-elements">
                <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yardım"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">A&#231;&#305;klamalar</span></a>
            </div>
        </div>
    </div>
    <div class="content">
        <div class="card">
            <div class="card-body">
                <asp:Panel ID="Panel1" runat="server" Visible="true">
                    <fieldset class="mb-3">
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Site Başlık</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSiteBaslik" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Logo Seçiniz</label>
                            <div class="col-sm-4">
                                <asp:FileUpload ID="fuLogo" runat="server" class="form-control" />
                                <asp:TextBox ID="txtLogo" runat="server" Visible="False"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Google Anahtar</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtGoogleAnahtar" runat="server" class="form-control" TextMode="MultiLine" Height="75"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Google Açıklama</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtGoogleAciklama" runat="server" class="form-control" TextMode="MultiLine" Height="75"></asp:TextBox>
                            </div>
                        </div>
                        <hr class="my-4" />
                        <h6 class="font-weight-semibold text-uppercase mb-3"><i class="icon-credit-card2 mr-2"></i>PayTR Ödeme Ayarları</h6>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Mağaza No</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtPayTRMagazaNo" runat="server" class="form-control" placeholder="PayTR mağaza numarası"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Mağaza Parola</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtPayTRMagazaParola" runat="server" class="form-control" TextMode="Password" placeholder="PayTR mağaza parolası"></asp:TextBox>
                                <small class="form-text text-muted">Güvenlik için parola gizlenir. Değiştirmek istemiyorsanız boş bırakın.</small>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Gizli Anahtar</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtPayTRGizliAnahtar" runat="server" class="form-control" TextMode="Password" placeholder="PayTR gizli anahtar"></asp:TextBox>
                                <small class="form-text text-muted">Güvenlik için gizli anahtar gizlenir. Değiştirmek istemiyorsanız boş bırakın.</small>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Test Modu</label>
                            <div class="col-sm-4">
                                <asp:CheckBox ID="chkPayTRTestModu" runat="server" Text="Ödeme işlemlerinde test modunu kullan" />
                            </div>
                        </div>
                        <hr class="my-3" />
                        <h6 class="font-weight-semibold text-uppercase mb-2"><i class="icon-coin-dollar mr-2"></i>D&#246;viz (PayTR i&#231;in TL d&#246;n&#252;&#351;&#252;m&#252;)</h6>
                        <p class="text-muted small">1 USD ve 1 EUR i&#231;in ka&#231; TL kullan&#305;laca&#287;&#305;n&#305; girin (&#246;n y&#252;z / &#246;deme hesab&#305;). PayTR genelde TL tahsilat yapar; yurt d&#305;&#351;&#305; m&#252;&#351;teri USD/EUR sepetinde tutar bu kurla TL&#39;ye &#231;evrilir.</p>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Kur (1 USD = ? TL)</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtKurUSD" runat="server" class="form-control" placeholder="&#214;r. 34,50"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Kur (1 EUR = ? TL)</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtKurEUR" runat="server" class="form-control" placeholder="&#214;r. 37,20"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Yurt d&#305;&#351;&#305; &#246;deme profili</label>
                            <div class="col-sm-8">
                                <asp:CheckBox ID="chkPayTRYurtdisi" runat="server" Text="Yurt d&#305;&#351;&#305; m&#252;&#351;teri / &#231;oklu para birimi ak&#305;&#351;&#305; aktif (PayTR ma&#287;aza ayarlar&#305;n&#305;zla uyumlu kullan&#305;n)" />
                            </div>
                        </div>
                        <hr class="my-3" />
                        <h6 class="font-weight-semibold text-uppercase mb-2"><i class="icon-truck mr-2"></i>Kargo Ayarlar&#305;</h6>
                        <p class="text-muted small">M&#252;&#351;terilerin sipari&#351;lerinde uygulanacak kargo &#252;creti ve bedava kargo limitini (TL cinsinden) belirleyin.</p>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Kargo &#220;creti (TL)</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtKargoUcreti" runat="server" class="form-control" placeholder="&#214;r. 50,00"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label" style="margin-top: 8px;">Bedava Kargo Limiti (TL)</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBedavaKargoLimiti" runat="server" class="form-control" placeholder="&#214;r. 1000,00"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                            </div>
                        </div>
                    </fieldset>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Site Sabitleri</h5>
                    <button class="close" data-dismiss="modal" type="button">&times;</button>
                </div>
                <div class="modal-body">
                    <p>Bu sayfa, site ba&#351;l&#305;&#287;&#305;, logo, Google anahtar/a&#231;&#305;klama ve PayTR &#246;deme ayarlar&#305;n&#305;n tan&#305;mlanmas&#305; i&#231;in kullan&#305;lmaktad&#305;r.</p>
                    <p><strong>PayTR:</strong> Ma&#287;aza No, Parola ve Gizli Anahtar bilgilerini PayTR panelinden al&#305;n&#305;z. Test modu a&#231;&#305;kken ger&#231;ek &#246;deme al&#305;nmaz.</p>
                </div>
                <div class="modal-footer">
                    <button class="btn bg-green-400 btn-ladda btn-ladda-progress ladda-button legitRipple" data-dismiss="modal" type="button">Kapat</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CustomScript" runat="server">
</asp:Content>
