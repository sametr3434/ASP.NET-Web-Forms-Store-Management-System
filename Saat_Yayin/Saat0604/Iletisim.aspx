<%@ Page Title="" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="Iletisim.aspx.cs" Inherits="Saat.Saat0604.Iletisim" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

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
                    <a class="breadcrumb-item" href="#">İletisim</a>
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
                <asp:Panel ID="Panel1" runat="server">
                    <fieldset class="mb-3">
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Telefon 1</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtTelefon1" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Telefon 2</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtTelefon2" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Mail Adresi</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtMailAdresi" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Adres</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtAdres" runat="server" class="form-control" TextMode="MultiLine" Height="75"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Facebook</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtFacebook" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Twitter</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtTwitter" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Linkedin</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtLinkedin" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">İnstagram</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtInstagram" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Whatsapp</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtWhatsapp" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton><asp:Label ID="lblDegistirID" runat="server" Visible="false"></asp:Label><asp:Label ID="lblSilinecekID" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </fieldset>
                    <div class="text-right">
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">&#304;leti&#351;im</h5>
                    <button class="close" data-dismiss="modal" type="button">&times;</button>
                </div>
                <div class="modal-body">
                    <p>Bu sayfa &#252;zerinde yapacak oldu&#287;unuz tan&#305;mlamalar; sitede bulunan &#304;leti&#351;im b&#246;l&#252;m&#252;n&#252;n i&#231;eri&#287;ine kaydedilecektir.</p>
                    <p>Telefon, mail, adres ve sosyal medya linkleri buradan y&#246;netilir.</p>
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
