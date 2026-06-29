<%@ Page Title="Ürün Resimleri" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="UrunResim.aspx.cs" Inherits="Saat.Saat0604.UrunResim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class="font-weight-semibold">Panel</span> - Ürün Resimleri</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/UrunListe.aspx" class="breadcrumb-item">Ürün Listesi</a>
                    <span class="breadcrumb-item active">Ürün Resimleri</span>
                </div>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Resim Yükleme</h5>
                <div class="header-elements">
                    <a href="UrunDuzenle.aspx?id=<%= Request.QueryString["id"] %>" class="btn btn-secondary btn-lg ml-2"><i class="icon-pencil7 mr-2"></i>Ürünü Düzenle</a>
                </div>
            </div>
            <div class="card-body">
                <asp:Panel ID="pnlYukle" runat="server">
                    <div class="form-group row">
                        <label class="col-sm-2 control-label" style="margin-top: 8px;">Resim</label>
                        <div class="col-sm-6">
                            <asp:FileUpload ID="fuResim" runat="server" class="form-control" />
                            <small class="form-text text-muted">JPG/PNG, max 2MB</small>
                        </div>
                        <div class="col-sm-2 text-right">
                            <asp:LinkButton ID="lnkYukle" runat="server" CssClass="btn btn-primary" OnClick="lnkYukle_Click"><i class="icon-plus22 mr-2"></i>Yükle</asp:LinkButton>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-2 control-label" style="margin-top: 8px;">Başlık</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtBaslik" runat="server" CssClass="form-control" placeholder="Resim başlığı (opsiyonel)"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-2 control-label" style="margin-top: 8px;">Sıralama</label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtSira" runat="server" CssClass="form-control" placeholder="Sayfadaki gösterim sırası (örn. 1, 2, 3)"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>
                <div class="table-responsive">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Resim</th>
                                <th style="width: 35%;">Başlık</th>
                                <th style="width: 120px;">Sıralama</th>
                                <th class="text-right">İşlemler</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptResimler" runat="server" OnItemCommand="rptResimler_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><img src='/Upload/<%# Eval("Resim") %>' style="max-height:60px" /></td>
                                        <td>
                                            <asp:TextBox ID="txtBaslikRow" runat="server" CssClass="form-control form-control-sm" Text='<%# Duzelt(Eval("Baslik")) %>'></asp:TextBox>
                                        </td>
                                        <td style="max-width: 120px;">
                                            <asp:TextBox ID="txtSiraRow" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Siralama") %>'></asp:TextBox>
                                        </td>
                                        <td class="text-right">
                                            <asp:LinkButton ID="lbGuncelle" runat="server" CommandName="Guncelle" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-success btn-sm mr-2"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                                            <asp:LinkButton ID="lbSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Bu resmi silmek istediğinize emin misiniz?');"><i class="icon-trash mr-2"></i>Sil</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
