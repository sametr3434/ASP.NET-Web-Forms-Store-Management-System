<%@ Page Title="Kupon Listesi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="KuponListe.aspx.cs" Inherits="Saat.Saat0604.KuponListe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-ticket mr-2"></i> <span class="font-weight-semibold">Panel</span> - Kupon Listesi</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <span class="breadcrumb-item active">Kupon Listesi</span>
                </div>
            </div>
            <div class="header-elements">
                <div class="d-flex justify-content-center">
                    <a href="KuponEkle.aspx" class="btn btn-primary btn-lg"><i class="icon-plus22 mr-2"></i>Yeni Kupon Ekle</a>
                </div>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Kuponlar</h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="kuponTable" class="table datatable-responsive w-100 text-nowrap" data-order='[[ 0, "desc" ]]'>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Kupon Kodu</th>
                                <th>İndirim Tipi</th>
                                <th>Değer</th>
                                <th>Min. Tutar</th>
                                <th>Başlangıç</th>
                                <th>Bitiş</th>
                                <th>Kull.Limiti</th>
                                <th>Kullanılan</th>
                                <th>Durum</th>
                                <th class="text-center">İşlemler</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptKuponlar" runat="server" OnItemCommand="rptKuponlar_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Duzelt(Eval("KuponKodu")) %></td>
                                        <td><%# Duzelt(Eval("IndirimTipi")) %></td>
                                        <td class="text-right"><%# Eval("IndirimDegeri","{0:N2}") %></td>
                                        <td class="text-right"><%# Eval("MinSiparisTutari","{0:N2}") %></td>
                                        <td><%# Eval("BaslangicTarihi","{0:dd.MM.yyyy}") %></td>
                                        <td><%# Eval("BitisTarihi","{0:dd.MM.yyyy}") %></td>
                                        <td class="text-right"><%# Eval("KullanimLimiti") %></td>
                                        <td class="text-right"><%# Eval("KullanilanAdet") %></td>
                                        <td><%# Convert.ToBoolean(Eval("Aktif")) ? "<span class='badge badge-success'>Aktif</span>" : "<span class='badge badge-danger'>Pasif</span>" %></td>
                                        <td class="text-center">
                                            <div class="d-inline-flex align-items-center">
                                                <a href='KuponEkle.aspx?id=<%# Eval("ID") %>' class='btn btn-primary btn-sm mr-2'><i class='icon-gear mr-2'></i>Düzenle</a>
                                                <asp:LinkButton ID="lbSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Bu kuponu silmek istediğinizden emin misiniz?');"><i class="icon-trash mr-2"></i>Sil</asp:LinkButton>
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
            $('#kuponTable').DataTable({
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
                    { width: '140px' },
                    { width: '120px' },
                    { width: '100px', className: 'text-right' },
                    { width: '110px', className: 'text-right' },
                    { width: '110px' },
                    { width: '110px' },
                    { width: '90px', className: 'text-right' },
                    { width: '90px', className: 'text-right' },
                    { width: '100px' },
                    { width: '210px' }
                ]
            });
        });
    </script>
</asp:Content>
