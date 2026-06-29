<%@ Page Title="Ürün Listesi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="UrunListe.aspx.cs" Inherits="Saat.Saat0604.UrunListe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class="font-weight-semibold">Panel</span> - Ürün Listesi</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <span class="breadcrumb-item active">Ürün Listesi</span>
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
                <h5 class="card-title">Ürün Listesi</h5>
                <div class="header-elements">
                    <a href="UrunEkle.aspx" class="btn btn-primary btn-lg"><i class="icon-plus22 mr-2"></i>Yeni Ürün Ekle</a>
                </div>
            </div>
            <div class="card-body">
                <div class="form-row align-items-end mb-3">
                    <div class="col-md-3">
                        <label>Kategori</label>
                        <asp:DropDownList ID="ddlKategori" runat="server" CssClass="form-control select-search" data-fouc="" AppendDataBoundItems="true">
                            <asp:ListItem Value="">Tümü</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label>Marka</label>
                        <asp:DropDownList ID="ddlMarka" runat="server" CssClass="form-control select-search" data-fouc="" AppendDataBoundItems="true">
                            <asp:ListItem Value="">Tümü</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:LinkButton ID="btnFiltrele" runat="server" CssClass="btn btn-info mt-4" OnClick="btnFiltrele_Click"><i class="icon-filter3 mr-2"></i>Filtrele</asp:LinkButton>
                        <asp:LinkButton ID="btnTemizle" runat="server" CssClass="btn btn-light mt-4 ml-2" OnClick="btnTemizle_Click"><i class="icon-reset mr-2"></i>Temizle</asp:LinkButton>
                    </div>
                </div>
                <div class="table-responsive">
                    <table id="urunTable" class="table datatable-responsive w-100 text-nowrap" data-order='[[ 0, "desc" ]]'>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Ürün Adı</th>
                                <th>Kodu</th>
                                <th>Marka</th>
                                <th>Fiyat TL</th>
                                <th>USD (liste / ind.)</th>
                                <th>EUR (liste / ind.)</th>
                                <th>Stok</th>
                                <th>Durum</th>
                                <th class="text-center">İşlemler</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptUrunler" runat="server" OnItemCommand="rptUrunler_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Duzelt(Eval("UrunAdi")) %></td>
                                        <td><%# Duzelt(Eval("UrunKodu")) %></td>
                                        <td><%# Duzelt(Eval("MarkaAdi")) %></td>
                                        <td class="text-right"><%# FiyatListeTL(Eval("Fiyat"), Eval("IndirimliFiyat")) %></td>
                                        <td class="text-right"><%# FiyatListeUsd(Eval("FiyatUSD"), Eval("IndirimliFiyatUSD")) %></td>
                                        <td class="text-right"><%# FiyatListeEur(Eval("FiyatEUR"), Eval("IndirimliFiyatEUR")) %></td>
                                        <td class="text-right"><%# Eval("StokAdedi") %></td>
                                        <td><%# Convert.ToBoolean(Eval("Aktif")) ? "<span class='badge badge-success'>Aktif</span>" : "<span class='badge badge-danger'>Pasif</span>" %></td>
                                        <td class="text-center">
                                            <div class="d-inline-flex align-items-center">
                                                <a href='UrunDuzenle.aspx?id=<%# Eval("ID") %>' class='btn btn-primary btn-sm mr-2'><i class='icon-gear mr-2'></i>Düzenle</a>
                                                <a href='UrunResim.aspx?id=<%# Eval("ID") %>' class='btn btn-warning btn-sm mr-2'><i class='icon-image2 mr-2'></i>Resimler</a>
                                                <asp:LinkButton ID="lbSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Bu ürünü silmek istediğinizden emin misiniz?');" CssClass="btn btn-danger btn-sm"><i class="icon-trash mr-2"></i>Sil</asp:LinkButton>
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
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title">A&#231;&#305;klamalar</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <h6 class="font-weight-semibold">Ürün Listeleme Sayfas&#305;</h6>
                    <p>Bu sayfada kategori ve marka filtresi ile &#252;r&#252;nleri listeleyebilir, d&#252;zenleme ve resim y&#246;netimi sayfalar&#305;na h&#305;zl&#305; eri&#351;im sa&#287;layabilirsiniz.</p>
                    <p><b>Varyant Y&#246;netimi:</b> Renk ve Numara se&#231;enekleri &#252;r&#252;n d&#252;zenleme/de&#287;i&#351;tirme ak&#305;&#351;&#305;ndan tan&#305;mlanacakt&#305;r. Varyant ekleme/d&#252;zenleme i&#351;lemleri liste sayfas&#305;ndan de&#287;il, ilgili &#252;r&#252;n detayı &#252;zerinden ger&#231;ekle&#351;ir.</p>
                    <p><b>&#304;pucu:</b> Filtreleri temizlemek i&#231;in <i>Temizle</i> butonunu kullanabilirsiniz.</p>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            var dt = $('#urunTable').DataTable({
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
                    "oPaginate": { "sFirst": "İlk","sLast": "Son","sNext": "Sonraki","sPrevious": "Önceki" },
                    "oAria": { "sSortAscending": ": artan sütun sıralamasını aktifleştir", "sSortDescending": ": azalan sütun sıralamasını aktifleştir" }
                },
                columns: [
                    { width: '60px' },
                    { width: null },
                    { width: '120px' },
                    { width: '160px' },
                    { width: '110px', className: 'text-right' },
                    { width: '90px', className: 'text-right' },
                    { width: '110px' },
                    { width: '260px' }
                ]
            });
        });
    </script>
</asp:Content>
