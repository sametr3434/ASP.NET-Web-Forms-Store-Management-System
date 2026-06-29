<%@ Page Title="Ürün Varyantları" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="UrunVaryant.aspx.cs" Inherits="Saat.Saat0604.UrunVaryant" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-grid6 mr-2"></i> <span class="font-weight-semibold">Panel</span> - Ürün Varyantları</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/UrunListe.aspx" class="breadcrumb-item">Ürün Listesi</a>
                    <span class="breadcrumb-item active">Ürün Varyantları</span>
                </div>
            </div>
            <div class="header-elements">
                <div class="d-flex justify-content-center">
                    <a href="UrunDuzenle.aspx?id=<%= Request.QueryString["id"] %>" class="btn btn-secondary btn-lg mr-2"><i class="icon-pencil7 mr-2"></i>Ürünü Düzenle</a>
                    <a href="UrunResim.aspx?id=<%= Request.QueryString["id"] %>" class="btn btn-warning btn-lg mr-2"><i class="icon-image2 mr-2"></i>Resimler</a>
                    <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yardım"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">Açıklamalar</span></a>
                </div>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Varyant Ekle</h5>
            </div>
            <div class="card-body">
                <asp:UpdatePanel ID="upEkle" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-row align-items-end">
                            <div class="col-md-3">
                                <label>Kasa çapı</label>
                                <asp:DropDownList ID="ddlBeden" runat="server" CssClass="form-control select-search" data-fouc=""></asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label>Renk</label>
                                <asp:DropDownList ID="ddlRenk" runat="server" CssClass="form-control select-search" data-fouc=""></asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label>Stok</label>
                                <asp:TextBox ID="txtStok" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label>Barkod</label>
                                <asp:TextBox ID="txtBarkod" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label>Fark TL</label>
                                <asp:TextBox ID="txtFiyatFarki" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label>Fark USD</label>
                                <asp:TextBox ID="txtFiyatFarkiUSD" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label>Fark EUR</label>
                                <asp:TextBox ID="txtFiyatFarkiEUR" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="mt-3">
                            <asp:LinkButton ID="btnEkle" runat="server" CssClass="btn btn-primary" OnClick="btnEkle_Click"><i class="icon-plus22 mr-2"></i>Varyant Ekle</asp:LinkButton>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnEkle" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Varyantlar</h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="varyantTable" class="table datatable-responsive w-100 text-nowrap" data-order='[[ 0, "desc" ]]'>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Kasa çapı</th>
                                <th>Renk</th>
                                <th>Stok</th>
                                <th>Barkod</th>
                                <th>Fark TL</th>
                                <th>Fark USD</th>
                                <th>Fark EUR</th>
                                <th class="text-right">&#304;&#351;lemler</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptVaryantlar" runat="server" OnItemCommand="rptVaryantlar_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Duzelt(Eval("kasano")) %></td>
                                        <td><%# Duzelt(Eval("RenkAdi")) %></td>
                                        <td style="max-width:120px"><asp:TextBox ID="txtStokRow" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("StokAdedi") %>' /></td>
                                        <td style="max-width:180px"><asp:TextBox ID="txtBarkodRow" runat="server" CssClass="form-control form-control-sm" Text='<%# Duzelt(Eval("Barkod")) %>' /></td>
                                        <td style="max-width:100px"><asp:TextBox ID="txtFarkRow" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("FiyatFarki") == DBNull.Value ? "" : Eval("FiyatFarki") %>' /></td>
                                        <td style="max-width:100px"><asp:TextBox ID="txtFarkRowUSD" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("FiyatFarkiUSD") == DBNull.Value ? "" : Eval("FiyatFarkiUSD") %>' /></td>
                                        <td style="max-width:100px"><asp:TextBox ID="txtFarkRowEUR" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("FiyatFarkiEUR") == DBNull.Value ? "" : Eval("FiyatFarkiEUR") %>' /></td>
                                        <td class="text-right">
                                            <asp:LinkButton ID="lbGuncelle" runat="server" CommandName="Guncelle" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-success btn-sm mr-2"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton>
                                            <asp:LinkButton ID="lbSil" runat="server" CommandName="Sil" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Bu varyantı silmek istediğinize emin misiniz?');"><i class="icon-trash mr-2"></i>Sil</asp:LinkButton>
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
                    <h5 class="modal-title">Açıklamalar</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <p><b>Kasa çapı / Renk:</b> Varyant; ürünün kasa çapı ve renk kombinasyonudur.</p>
                    <p><b>Stok:</b> Bu varyanta ait stok adedi.</p>
                    <p><b>Barkod:</b> Varyanta özel barkod (opsiyonel).</p>
                    <p><b>Fiyat Farkı:</b> Ana fiyata eklenecek (+) veya düşülecek (−) fark. Boş/0 ise ana fiyat kullanılır.</p>
                    <p>Satır içindeki alanları düzenleyip <i>Güncelle</i> ile kaydedebilirsiniz. Sil ile varyant kaldırılır.</p>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            $('#varyantTable').DataTable({
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
                    { width: '140px' },
                    { width: '120px' },
                    { width: '180px' },
                    { width: '140px' },
                    { width: '210px' }
                ]
            });
        });
    </script>
</asp:Content>
