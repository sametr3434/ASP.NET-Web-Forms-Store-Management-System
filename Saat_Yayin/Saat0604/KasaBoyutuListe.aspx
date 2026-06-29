<%@ Page Title="Kasa Boyutu Listesi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="KasaBoyutuListe.aspx.cs" Inherits="Saat.Saat0604.KasaBoyutuListe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class="font-weight-semibold">Panel</span> - Kasa Boyutu Listesi</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <span class="breadcrumb-item active">Kasa Boyutu Listesi</span>
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
                <h5 class="card-title">Kasa Boyutu Listesi</h5>
                <div class="header-elements">
                    <a href="KasaBoyutuEkleme.aspx" class="btn btn-primary btn-lg"><i class="icon-plus22 mr-2"></i>Yeni Kasa Boyutu Ekle</a>
                </div>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="bedenTable" class="table datatable-responsive w-100" data-order='[[ 2, "asc" ]]'>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Kasa Boyutu</th>
                                <th>Sıralama</th>
                                <th>Durum</th>
                                <th class="text-center">İşlemler</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptKasaBoyutlari" runat="server" OnItemCommand="rptKasaBoyutlari_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Duzelt(Eval("kasano")) %></td>
                                        <td><%# Eval("Siralama") %></td>
                                        <td><%# Convert.ToBoolean(Eval("Aktif")) ? "<span class='badge badge-success'>Aktif</span>" : "<span class='badge badge-danger'>Pasif</span>" %></td>
                                        <td class="text-center">
                                            <div class="d-inline-flex align-items-center">
                                                <a href='KasaBoyutuEkleme.aspx?id=<%# Eval("ID") %>' class='btn btn-primary btn-lg mr-2'><i class='icon-gear mr-2'></i>Düzenle</a>
                                                <asp:LinkButton ID="lbSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Bu bedeni silmek istediğinizden emin misiniz?');" CssClass="btn btn-danger btn-lg"><i class="icon-trash mr-2"></i>Sil</asp:LinkButton>
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

    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title">A&#231;&#305;klamalar</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <h6 class="font-weight-semibold">Kasa Boyutu Listeleme Sayfas&#305;</h6>
                    <p>Ayakkab&#305; Kasa Boyutular&#305;n&#305; (36, 37, 38...) listeleyebilir, d&#252;zenleyebilir ve silebilirsiniz. Her &#252;r&#252;n birden fazla Kasa Boyutuyla e&#351;lenebilir (UrunVaryant).</p>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            $('#bedenTable').DataTable({
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
</asp:Content>
