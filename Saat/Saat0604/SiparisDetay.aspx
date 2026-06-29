<%@ Page Title="Sipariş Detay" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="SiparisDetay.aspx.cs" Inherits="Saat.Saat0604.SiparisDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-basket mr-2"></i> <span class="font-weight-semibold">Panel</span> - Sipariş Detay</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/SiparisListe.aspx" class="breadcrumb-item">Sipariş Listesi</a>
                    <span class="breadcrumb-item active">Sipariş Detay</span>
                </div>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Sipariş Bilgileri</h5>
                <div class="header-elements">
                    <a href="SiparisEtiket.aspx?id=<%= Request.QueryString["id"] %>" target="_blank" class="btn btn-primary btn-lg"><i class="icon-printer mr-2"></i>Etiket Yazdır</a>
                </div>
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <label class="col-sm-2 control-label" style="margin-top: 8px;">Sipariş No</label>
                    <div class="col-sm-4"><asp:Label ID="lblSiparisNo" runat="server"></asp:Label></div>
                    <label class="col-sm-2 control-label" style="margin-top: 8px;">Tarih</label>
                    <div class="col-sm-4"><asp:Label ID="lblTarih" runat="server"></asp:Label></div>
                </div>
                <div class="form-group row">
                    <label class="col-sm-2 control-label" style="margin-top: 8px;">M&#252;&#351;teri</label>
                    <div class="col-sm-4"><asp:Label ID="lblMusteri" runat="server"></asp:Label></div>
                    <label class="col-sm-2 control-label" style="margin-top: 8px;">&#214;deme</label>
                    <div class="col-sm-4"><asp:Label ID="lblOdeme" runat="server"></asp:Label></div>
                </div>
                <div class="form-group row">
                    <label class="col-sm-2 control-label" style="margin-top: 8px;">Para birimi / PayTR</label>
                    <div class="col-sm-10"><asp:Label ID="lblParaBirimi" runat="server" CssClass="text-muted"></asp:Label></div>
                </div>
                <div class="form-group row">
                    <label class="col-sm-2 control-label" style="margin-top: 8px;">Sipariş Durumu</label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlSiparisDurumu" runat="server" CssClass="form-control">
                            <asp:ListItem>Yeni</asp:ListItem>
                            <asp:ListItem>Hazırlanıyor</asp:ListItem>
                            <asp:ListItem>Kargolandı</asp:ListItem>
                            <asp:ListItem>Tamamlandı</asp:ListItem>
                            <asp:ListItem>İptal</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-6 text-right">
                        <asp:LinkButton ID="btnDurumGuncelle" runat="server" CssClass="btn btn-success" OnClick="btnDurumGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                    </div>
                </div>
                <div class="form-group row">
                    <label class="col-sm-2 control-label" style="margin-top: 8px;">Kargo Takip No</label>
                    <div class="col-sm-6"><asp:TextBox ID="txtKargo" runat="server" CssClass="form-control" /></div>
                    <div class="col-sm-4"><asp:LinkButton ID="btnKargoKaydet" runat="server" CssClass="btn btn-info" OnClick="btnKargoKaydet_Click"><i class="icon-truck mr-2"></i>Kaydet</asp:LinkButton></div>
                </div>
            </div>
        </div>

        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Kalemler</h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="kalemTable" class="table datatable-responsive w-100 text-nowrap" data-order='[[ 0, "asc" ]]'>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Ürün</th>
                                <th>Ürün Çapı</th>
                                <th>Renk</th>
                                <th>Adet</th>
                                <th>Birim</th>
                                <th>Toplam</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptKalemler" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Duzelt(Eval("UrunAdi")) %></td>
                                        <td><%# Duzelt(Eval("UrunCapi")) %></td>
                                        <td><%# Duzelt(Eval("Renk")) %></td>
                                        <td class="text-right"><%# Eval("Adet") %></td>
                                        <td class="text-right"><%# TutarKalem(Eval("BirimFiyat"), Eval("ParaBirimi")) %></td>
                                        <td class="text-right"><%# TutarKalem(Eval("ToplamFiyat"), Eval("ParaBirimi")) %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div class="text-right mt-3">
                    <asp:Label ID="lblOzet" runat="server" CssClass="font-weight-semibold"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            $('#kalemTable').DataTable({
                autoWidth: false,
                scrollX: true,
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
                    "oPaginate": { "sFirst": "İlk", "sLast": "Son", "sNext": "Sonraki", "sPrevious": "Önceki" },
                    "oAria": { "sSortAscending": ": artan sütun sıralamasını aktifleştir", "sSortDescending": ": azalan sütun sıralamasını aktifleştir" }
                },
                columns: [
                    { width: '60px' },
                    { width: null },
                    { width: '120px' },
                    { width: '140px' },
                    { width: '90px', className: 'text-right' },
                    { width: '110px', className: 'text-right' },
                    { width: '120px', className: 'text-right' }
                ]
            });
        });
    </script>
</asp:Content>
