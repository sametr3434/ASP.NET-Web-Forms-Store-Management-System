<%@ Page Title="Sipariş Listesi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="SiparisListe.aspx.cs" Inherits="Saat.Saat0604.SiparisListe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-basket mr-2"></i> <span class="font-weight-semibold">Panel</span> - Sipariş Listesi</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <span class="breadcrumb-item active">Sipariş Listesi</span>
                </div>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Siparişler</h5>
            </div>
            <div class="card-body">
                <div class="form-row align-items-end mb-3">
                    <div class="col-md-3">
                        <label>Durum</label>
                        <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-control select-search" data-fouc="" AppendDataBoundItems="true">
                            <asp:ListItem Value="">Tümü</asp:ListItem>
                            <asp:ListItem>Yeni</asp:ListItem>
                            <asp:ListItem>Hazırlanıyor</asp:ListItem>
                            <asp:ListItem>Kargolandı</asp:ListItem>
                            <asp:ListItem>Tamamlandı</asp:ListItem>
                            <asp:ListItem>İptal</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label>Başlangıç</label>
                        <asp:TextBox ID="txtBaslangic" runat="server" CssClass="form-control" placeholder="gg.aa.yyyy"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label>Bitiş</label>
                        <asp:TextBox ID="txtBitis" runat="server" CssClass="form-control" placeholder="gg.aa.yyyy"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:LinkButton ID="btnFiltre" runat="server" CssClass="btn btn-info mt-4" OnClick="btnFiltre_Click"><i class="icon-filter3 mr-2"></i>Filtrele</asp:LinkButton>
                        <asp:LinkButton ID="btnTemizle" runat="server" CssClass="btn btn-light mt-4 ml-2" OnClick="btnTemizle_Click"><i class="icon-reset mr-2"></i>Temizle</asp:LinkButton>
                    </div>
                </div>

                <div class="table-responsive">
                    <table id="siparisTable" class="table datatable-responsive w-100 text-nowrap" data-order='[[ 0, "desc" ]]'>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Sipariş No</th>
                                <th>Müşteri</th>
                                <th>Ara Toplam</th>
                                <th>İndirim</th>
                                <th>Kargo</th>
                                <th>Toplam</th>
                                <th>PB</th>
                                <th>&#214;deme</th>
                                <th>Sipariş Durumu</th>
                                <th>Tarih</th>
                                <th class="text-center">İşlemler</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSiparisler" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Duzelt(Eval("SiparisNo")) %></td>
                                        <td><%# Duzelt(Eval("MusteriAdi")) %></td>
                                        <td class="text-right"><%# TutarPB(Eval("AraToplam"), Eval("ParaBirimi")) %></td>
                                        <td class="text-right"><%# TutarPB(Eval("IndirimTutari"), Eval("ParaBirimi")) %></td>
                                        <td class="text-right"><%# TutarPB(Eval("KargoTutari"), Eval("ParaBirimi")) %></td>
                                        <td class="text-right"><%# TutarPB(Eval("ToplamTutar"), Eval("ParaBirimi")) %></td>
                                        <td><%# Duzelt(Eval("ParaBirimi")) %></td>
                                        <td><%# Duzelt(Eval("OdemeDurumu")) %></td>
                                        <td><%# Duzelt(Eval("SiparisDurumu")) %></td>
                                        <td><%# Eval("SiparisTarihi","{0:dd.MM.yyyy HH:mm}") %></td>
                                        <td class="text-center">
                                            <div class="d-inline-flex align-items-center">
                                                <a href='SiparisDetay.aspx?id=<%# Eval("ID") %>' class='btn btn-primary btn-sm mr-2'><i class='icon-eye mr-2'></i>Detay</a>
                                            </div>
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

    <script>
        $(document).ready(function () {
            $('#siparisTable').DataTable({
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
                    { width: '120px' },
                    { width: '180px' },
                    { width: '110px', className: 'text-right' },
                    { width: '100px', className: 'text-right' },
                    { width: '100px', className: 'text-right' },
                    { width: '110px', className: 'text-right' },
                    { width: '110px' },
                    { width: '130px' },
                    { width: '150px' },
                    { width: '150px' }
                ]
            });
        });
    </script>
</asp:Content>
