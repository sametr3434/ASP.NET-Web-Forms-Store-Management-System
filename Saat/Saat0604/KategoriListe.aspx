<%@ Page Title="Kategori Listesi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="KategoriListe.aspx.cs" Inherits="Saat.Saat0604.KategoriListe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page header -->
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class="font-weight-semibold">Panel</span> - Kategori Listesi</h4>
                <a href="#" class="header-elements-toggle text-default d-md-none"><i class="icon-more"></i></a>
            </div>
        </div>

        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <span class="breadcrumb-item active">Kategori Listesi</span>
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
                <h5 class="card-title">Kategori Listesi</h5>
                <div class="header-elements">
                    <asp:LinkButton ID="lnkYeniKayit" runat="server" class="btn btn-primary btn-lg" OnClick="lnkYeniKayit_Click"><i class="icon-plus22 mr-2"></i>Yeni Kategori Ekle</asp:LinkButton>
                </div>
            </div>

            <div class="card-body">
                <asp:Panel ID="PanelForm" runat="server" Visible="false">
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
                                <asp:TextBox ID="txtAciklama" runat="server" class="form-control" TextMode="MultiLine" Rows="3" placeholder="Kategori açıklaması..."></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Sıralama</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtSiralama" runat="server" class="form-control" placeholder="Sıralama numarası..."></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Durum</label>
                            <div class="col-sm-6">
                                <div class="form-check">
                                    <label class="form-check-label">
                                        <asp:CheckBox ID="chkAktif" runat="server" class="form-check-input" Checked="true" />
                                        Aktif
                                    </label>
                                </div>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkKaydet" runat="server" class="btn btn-success btn-lg" Visible="true" OnClick="lnkKaydet_Click"><i class="icon-checkmark3 mr-2"></i>Kaydet</asp:LinkButton>
                                <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" Visible="false" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                                <asp:Label ID="lblDegistirID" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </fieldset>
                </asp:Panel>

                <div class="table-responsive">
                    <table id="kategoriTable" class="table datatable-responsive" data-order='[[ 0, "desc" ]]'>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Kategori Adı</th>
                                <th>Açıklama</th>
                                <th>Sıralama</th>
                                <th>Durum</th>
                                <th class="text-center">İşlemler</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptKategoriler" runat="server" OnItemCommand="rptKategoriler_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Duzelt(Eval("KategoriAdi")) %></td>
                                        <td><%# Duzelt(Eval("Aciklama")) %></td>
                                        <td><%# Eval("Siralama") %></td>
                                        <td>
                                            <%# Convert.ToBoolean(Eval("Aktif")) ? "<span class='badge badge-success'>Aktif</span>" : "<span class='badge badge-danger'>Pasif</span>" %>
                                        </td>
                                        <td class="text-center">
                                            <div class="d-inline-flex align-items-center">
                                                <a href='KategoriEkle.aspx?id=<%# Eval("ID") %>' class='btn btn-primary btn-lg mr-2'><i class='icon-gear mr-2'></i>Düzenle</a>
                                                <asp:LinkButton ID="lbSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Bu kategoriyi silmek istediğinizden emin misiniz?');" CssClass="btn btn-danger btn-lg"><i class="icon-trash mr-2"></i>Sil</asp:LinkButton>
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
        <!-- /basic card -->

    </div>
    <!-- /content area -->

    <!-- Açıklamalar Modal -->
    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title">A&#231;&#305;klamalar</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <h6 class="font-weight-semibold">Kategori Listeleme Sayfas&#305;</h6>
                    <p>Bu sayfa, sistemdeki t&#252;m &#252;r&#252;n kategorilerini listelemek ve y&#246;netmek i&#231;in kullan&#305;l&#305;r. Kategorileri d&#252;zenleyebilir, silebilir veya yeni kategoriler ekleyebilirsiniz.</p>
                    <p><b>Yeni Kategori Ekle:</b> "Yeni Kategori Ekle" butonu ile formu a&#231;&#305;p yeni kategori kaydedebilirsiniz.</p>
                    <p><b>D&#252;zenle:</b> Se&#231;ilen kategorinin detaylar&#305;n&#305; d&#252;zenlemek i&#231;in kullan&#305;l&#305;r.</p>
                    <p><b>Sil:</b> Se&#231;ilen kategoriyi sistemden kald&#305;r&#305;r. Bu i&#351;lem geri al&#305;namaz.</p>
                    <p><b>Durum:</b> Kategorinin site g&#246;r&#252;n&#252;p g&#246;r&#252;nmediğini belirler. Sadece aktif kategoriler sitede g&#246;sterilir.</p>
                </div>
            </div>
        </div>
    </div>
    <!-- /Açıklamalar Modal -->

    <!-- Datatables Init Script -->
    <script>
        $(document).ready(function () {
            $('#kategoriTable').DataTable({
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
