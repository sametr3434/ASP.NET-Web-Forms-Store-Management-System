<%@ Page Title="Kategori Yönetimi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="KategoriEkle.aspx.cs" Inherits="Saat.Saat0604.KategoriEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page header -->
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class="font-weight-semibold">Panel</span> - Kategori Yönetimi</h4>
                <a href="#" class="header-elements-toggle text-default d-md-none"><i class="icon-more"></i></a>
            </div>
        </div>

        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/KategoriListe.aspx" class="breadcrumb-item">Kategori Listesi</a>
                    <span class="breadcrumb-item active">Kategori Yönetimi</span>
                </div>

                <a href="#" class="header-elements-toggle text-default d-md-none"><i class="icon-more"></i></a>
            </div>

            <div class="header-elements">
                <div class="d-flex justify-content-center">
                    <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yardim"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">A&#231;&#305;klamalar</span></a>
                </div>
            </div>
        </div>
    </div>
    <!-- /page header -->

    <!-- Content area -->
    <div class="content">

        <!-- Basic card -->
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Kategori Yönetimi</h5>
                <div class="header-elements">
                    <asp:LinkButton ID="lnkYeni" runat="server" class="btn btn-primary btn-lg" OnClick="lnkYeni_Click"><i class="icon-plus22 mr-2"></i>Yeni Kategori</asp:LinkButton>
                    <a href="KategoriListe.aspx" class="btn btn-secondary btn-lg ml-2"><i class="icon-list2 mr-2"></i>Listeye Dön</a>
                </div>
            </div>

            <div class="card-body">
                <!-- Tabs -->
                <ul class="nav nav-tabs nav-tabs-highlight">
                    <li class="nav-item">
                        <a href="#kategori-bilgileri" class="nav-link active" data-toggle="tab">
                            <i class="icon-info3 mr-2"></i>Kategori Bilgileri
                        </a>
                    </li>
                    <li class="nav-item">
                        <a href="#kategori-urunleri" class="nav-link" data-toggle="tab">
                            <i class="icon-box mr-2"></i>Kategori Ürünleri
                        </a>
                    </li>
                </ul>

                <div class="tab-content">
                    <!-- Kategori Bilgileri Tab -->
                    <div class="tab-pane fade show active" id="kategori-bilgileri">
                        <asp:Panel ID="pnlForm" runat="server">
                            <fieldset class="mb-3">
                                <legend class="text-uppercase font-size-sm font-weight-bold">Kategori Bilgileri</legend>
                                
                                <div class="form-group row">
                                    <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Kategori Adı</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtKategoriAdi" runat="server" class="form-control" placeholder="Kategori adını giriniz..."></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Açıklama</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtAciklama" runat="server" class="form-control" TextMode="MultiLine" Rows="4" placeholder="Kategori açıklaması..."></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Sıralama</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtSiralama" runat="server" class="form-control" placeholder="Sıralama numarası..."></asp:TextBox>
                                        <span class="form-text text-muted">Düşük değerler önce gelir (0, 1, 2...)</span>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Üst Kategori</label>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddlUstKategori" runat="server" class="form-control select-search" data-fouc="" AppendDataBoundItems="true">
                                            <asp:ListItem Value="">-- Ana Kategori --</asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="form-text text-muted">Boş bırakırsanız ana kategori olur</span>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Durum</label>
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
                                    <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Kategori Resmi</label>
                                    <div class="col-sm-6">
                                        <asp:FileUpload ID="fuResim" runat="server" class="form-control" />
                                        <span class="form-text text-muted">JPG, PNG formatında, maksimum 2MB</span>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label class="col-form-label col-lg-2"></label>
                                    <div class="col-lg-10">
                                        <asp:LinkButton ID="lnkKaydet" runat="server" class="btn btn-success btn-lg" Visible="true" OnClick="lnkKaydet_Click"><i class="icon-checkmark3 mr-2"></i>Kaydet</asp:LinkButton>
                                        <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" Visible="false" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                                        <asp:LinkButton ID="lnkIptal" runat="server" class="btn btn-secondary btn-lg ml-2" OnClick="lnkIptal_Click"><i class="icon-cross2 mr-2"></i>İptal</asp:LinkButton>
                                        <asp:Label ID="lblKategoriID" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>
                            </fieldset>
                        </asp:Panel>

                        <asp:Panel ID="pnlBilgi" runat="server" Visible="false" CssClass="alert alert-info">
                            <h6 class="alert-heading"><i class="icon-info3 mr-2"></i>Bilgi</h6>
                            <p class="mb-0">Yeni kategori eklemek veya mevcut kategoriyi düzenlemek için "Yeni Kategori" butonuna tıklayınız.</p>
                        </asp:Panel>
                    </div>

                    <!-- Kategori Ürünleri Tab -->
                    <div class="tab-pane fade" id="kategori-urunleri">
                        <asp:Panel ID="pnlUrunler" runat="server">
                            <div class="d-flex justify-content-end mb-2">
                                <a href="#" class="btn btn-primary" data-toggle="modal" data-target="#urunEkleModal"><i class="icon-plus22 mr-2"></i>Ürün Ekle</a>
                            </div>
                            <div class="table-responsive">
                                <table id="urunlerTable" class="table datatable-responsive table-hover table-striped w-100 text-nowrap" style="width:100%" data-order='[[ 0, "desc" ]]'>
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>Ürün Adı</th>
                                            <th>Ürün Kodu</th>
                                            <th>Marka</th>
                                            <th>Fiyat</th>
                                            <th>Stok</th>
                                            <th>Durum</th>
                                            <th>İşlemler</th>
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
                                                    <td><%# Eval("Fiyat", "{0:C2}") %></td>
                                                    <td><%# Eval("StokAdedi") %></td>
                                                    <td>
                                                        <%# Convert.ToBoolean(Eval("Aktif")) ? "<span class='badge badge-success'>Aktif</span>" : "<span class='badge badge-danger'>Pasif</span>" %>
                                                    </td>
                                                    <td>
                                                        <div class="d-inline-flex align-items-center">
                                                            <a href='UrunDuzenle.aspx?id=<%# Eval("ID") %>' class="btn btn-sm btn-outline-primary mr-2"><i class="icon-pencil7 mr-1"></i>Düzenle</a>
                                                            <a href='UrunResim.aspx?id=<%# Eval("ID") %>' class="btn btn-sm btn-outline-warning mr-2"><i class="icon-image2 mr-1"></i>Resimler</a>
                                                            <asp:LinkButton ID="lbKaldir" runat="server" CommandName="Kaldir" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Bu ürünü bu kategoriden kaldırmak istediğinize emin misiniz?');"><i class="icon-minus2 mr-1"></i>Kaldır</asp:LinkButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlUrunYok" runat="server" Visible="false" CssClass="alert alert-info">
                            <h6 class="alert-heading"><i class="icon-info3 mr-2"></i>Bilgi</h6>
                            <p class="mb-0">Bu kategoriye ait ürün bulunmamaktadır.</p>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
        <!-- /basic card -->

    </div>
    <!-- /content area -->

    <!-- Ürün Ekle Modal -->
    <div id="urunEkleModal" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title">Kategoriye Ürün Ekle</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Ürün Seçiniz</label>
                        <asp:DropDownList ID="ddlUrunSec" runat="server" CssClass="form-control select-search" data-fouc=""></asp:DropDownList>
                        <small class="form-text text-muted">Listede, bu kategoriye henüz bağlı olmayan aktif ürünler gösterilir.</small>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lnkKategoriyeEkle" runat="server" CssClass="btn btn-success" OnClick="lnkKategoriyeEkle_Click"><i class="icon-checkmark3 mr-2"></i>Ekle</asp:LinkButton>
                    <button type="button" class="btn btn-light" data-dismiss="modal"><i class="icon-cross2 mr-2"></i>Kapat</button>
                </div>
            </div>
        </div>
    </div>
    <!-- /Ürün Ekle Modal -->

    <!-- Açıklamalar Modal -->
    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title">A&#231;&#305;klamalar</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <h6 class="font-weight-semibold">Kategori Yönetim Sayfas&#305;</h6>
                    <p>Bu sayfa, kategori ekleme, düzenleme ve yönetimi i&#231;in kullan&#305;l&#305;r. &#220;&#231; sekmeli yapı sayesinde t&#252;m i&#351;lemleri tek sayfada yapabilirsiniz.</p>
                    <p><b>Kategori Bilgileri:</b> Yeni kategori ekleme veya mevcut kategoriyi d&#252;zenleme sekmesi.</p>
                    <p><b>Mevcut Kategoriler:</b> T&#252;m kategorilerin listelendiği, d&#252;zenlenebildiği ve silinebildiği sekme.</p>
                    <p><b>İstatistikler:</b> Kategori say&#305;s&#305;, durum da&#287;&#305;l&#305;m&#305; ve hiyerarşi yap&#305;s&#305;n&#305; g&#246;steren sekme.</p>
                    <p><b>Üst Kategori:</b> Alt kategori olu&#351;turmak i&#231;in bir ana kategori se&#231;ebilirsiniz.</p>
                    <p><b>Sıralama:</b> Kategorilerin sitede g&#246;r&#252;nme s&#305;ras&#305;n&#305; belirler.</p>
                </div>
            </div>
        </div>
    </div>
    <!-- /Açıklamalar Modal -->

    <!-- DataTables Script -->
    <script>
        $(document).ready(function () {
            var urunDT = $('#urunlerTable').DataTable({
                autoWidth: false,
                scrollX: true,
                dom: "<'row align-items-center mb-2'<'col-sm-6'l><'col-sm-6 text-right'f>>" +
                     "t" +
                     "<'row mt-2'<'col-sm-6'i><'col-sm-6'p>>",
                columns: [
                    { width: '60px' },
                    { width: null },
                    { width: '140px' },
                    { width: '160px' },
                    { width: '110px', className: 'text-right' },
                    { width: '90px', className: 'text-right' },
                    { width: '110px' },
                    { width: '190px' }
                ],
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
            $('a[data-toggle=\"tab\"][href=\"#kategori-urunleri\"]').on('shown.bs.tab', function () {
                urunDT.columns.adjust().draw(false);
            });
        });
    </script>
</asp:Content>
