<%@ Page Title="" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminTanim.aspx.cs" Inherits="Saat.Saat0604.AdminTanim" %>
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
                    <a class="breadcrumb-item" href="#">Kullanıcı Tanım</a>
                </div>
            </div>
            <div class="header-elements">
                <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yardım"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">A&#231;&#305;klamalar</span></a>
            </div>
        </div>
    </div>
    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Kullanıcı Tanım</h5>
                <div class="header-elements">
                    <asp:LinkButton ID="lnkYeniKayit" runat="server" class="btn btn-primary btn-lg" OnClick="lnkYeniKayit_Click"><i class="icon-checkmark3 mr-2"></i>Yeni Kayıt</asp:LinkButton>
                </div>
            </div>
            <div class="card-body">
                <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <fieldset class="mb-3">
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Kullanıcı Adı</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtKullaniciAdi" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Şifre</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtSifre" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Mail Adresi</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtMailAdresi" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkKaydet" runat="server" class="btn btn-success btn-lg" Visible="true" OnClick="lnkKaydet_Click"><i class="icon-checkmark3 mr-2"></i>Kaydet</asp:LinkButton>
                                <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" Visible="false" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton><asp:Label ID="lblDegistirID" runat="server" Visible="false"></asp:Label><asp:Label ID="lblSilinecekID" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </fieldset>
                    <div class="text-right">
                    </div>
                </asp:Panel>
            </div>
            <div class="table-responsive" style="padding-right: 10px;">
                <script type="text/javascript">
                    $(document).ready(function () {
                        $('#myTable').DataTable({
                            language: {
                                "sDecimal": ",",
                                "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
                                "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
                                "sInfoEmpty": "Kayıt yok",
                                "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
                                "sInfoPostFix": "",
                                "sInfoThousands": ".",
                                "sLengthMenu": "Sayfada  _MENU_  kayıt göster",
                                "sLoadingRecords": "Yükleniyor...",
                                "sProcessing": "İşleniyor...",
                                "sSearch": "Ara : ",
                                "sZeroRecords": "Eşleşen kayıt bulunamadı",
                                "oPaginate": {
                                    "sFirst": "İlk",
                                    "sLast": "Son",
                                    "sNext": "Sonraki",
                                    "sPrevious": "Önceki"
                                },
                                "oAria": {
                                    "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                                    "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
                                },
                                "select": {
                                    "rows": {
                                        "_": "%d kayıt seçildi",
                                        "0": "",
                                        "1": "1 kayıt seçildi"
                                    }
                                }
                            }
                        });
                    });
                </script>
                <table id="myTable" class="table datatable-responsive" data-order='[[ 0, "desc" ]]'>
                    <thead>
                        <tr>
                            <th scope="col">No</th>
                            <th scope="col">Kullanıcı Adı</th>
                            <th scope="col">Mail Adresi</th>
                            <th scope="col">Düzenle & Sil</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptListe" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <th scope="row"><%# Eval("ID") %></th>
                                    <td><%# Eval("kullaniciadi") %></td>
                                    <td><%# Eval("mailadresi") %></td>
                                    <td>
                                        <asp:LinkButton ID="lnkDegistir" runat="server" class='btn btn-primary btn-lg' CommandArgument='<%# Eval("ID") %>' OnClick="lnkDegistir_Click"><i class='icon-gear mr-2'></i>Düzenle</asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lnkSil" runat="server" class='btn btn-danger btn-lg' OnClientClick="return confirm('Kayıt Silinecek Emin misiniz?');" CommandArgument='<%# Eval("ID") %>' OnClick="lnkSil_Click"><i class='icon-trash mr-2'></i>Sil</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Kullan&#305;c&#305; Tan&#305;m</h5>
                    <button class="close" data-dismiss="modal" type="button">&times;</button>
                </div>
                <div class="modal-body">
                    <p>Bu sayfa &#252;zerinde yapacak oldu&#287;unuz tan&#305;mlamalar; y&#246;netim paneline giri&#351; i&#231;in yetkililerin tan&#305;mlanmas&#305; i&#231;indir.</p>
                    <p>Kullan&#305;c&#305; ad&#305;, &#351;ifre ve mail adresi ile panel eri&#351;im bilgileri y&#246;netilir.</p>
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
