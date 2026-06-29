<%@ Page Title="M&#252;&#351;teri Listesi" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="MusteriListe.aspx.cs" Inherits="Saat.Saat0604.MusteriListe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-arrow-left52 mr-2"></i> <span class="font-weight-semibold">Panel</span> - M&#252;&#351;teri Listesi</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <span class="breadcrumb-item active">M&#252;&#351;teri Listesi</span>
                </div>
            </div>
            <div class="header-elements">
                <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yard&#305;m"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">A&#231;&#305;klamalar</span></a>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">M&#252;&#351;teri Listesi</h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="musteriTable" class="table datatable-responsive w-100" data-order='[[ 0, "desc" ]]'>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Ad Soyad</th>
                                <th>E-Posta</th>
                                <th>Telefon</th>
                                <th>&#304;l / &#304;l&#231;e</th>
                                <th>Kay&#305;t Tarihi</th>
                                <th>Durum</th>
                                <th class="text-center">&#304;&#351;lem</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptMusteriler" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Duzelt(Eval("Ad")) %> <%# Duzelt(Eval("Soyad")) %></td>
                                        <td><a href='mailto:<%# Eval("EPosta") %>'><%# Duzelt(Eval("EPosta")) %></a></td>
                                        <td><%# GetTelefonHtml(Eval("Telefon")) %></td>
                                        <td><%# Duzelt(Eval("Il")) %> / <%# Duzelt(Eval("Ilce")) %></td>
                                        <td><%# Eval("KayitTarihi") != DBNull.Value ? Convert.ToDateTime(Eval("KayitTarihi")).ToString("dd.MM.yyyy") : "-" %></td>
                                        <td><%# (Eval("Aktif") != DBNull.Value && Convert.ToBoolean(Eval("Aktif"))) ? "<span class='badge badge-success'>Aktif</span>" : "<span class='badge badge-danger'>Pasif</span>" %></td>
                                        <td class="text-center">
                                            <a href='MusteriDetay.aspx?id=<%# Eval("ID") %>' class='btn btn-primary btn-sm'><i class='icon-eye'></i> Detay</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <asp:Panel ID="pnlBos" runat="server" Visible="false" CssClass="text-center text-muted py-4">
                    Hen&#252;z m&#252;&#351;teri kayd&#305; bulunmuyor.
                </asp:Panel>
            </div>
        </div>
    </div>

    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">M&#252;&#351;teri Listesi</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <p>Bu sayfa, siteye kay&#305;t olmu&#351; m&#252;&#351;terileri listeler. Ad, e-posta, telefon, il/il&#231;e ve kay&#305;t tarihi g&#246;r&#252;nt&#252;lenir.</p>
                    <p><strong>Detay:</strong> M&#252;&#351;teri bilgileri ve sipari&#351; ge&#231;mi&#351;i i&#231;in Detay sayfas&#305;na gidin.</p>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            if ($('#musteriTable tbody tr').length > 0) {
                $('#musteriTable').DataTable({
                    language: {
                        "sDecimal": ",",
                        "sEmptyTable": "Tabloda herhangi bir veri mevcut de&#287;il",
                        "sInfo": "_TOTAL_ kay&#305;ttan _START_ - _END_ aras&#305;ndaki kay&#305;tlar g&#246;steriliyor",
                        "sInfoEmpty": "Kay&#305;t yok",
                        "sInfoFiltered": "(_MAX_ kay&#305;t i&#231;erisinden bulunan)",
                        "sInfoPostFix": "",
                        "sInfoThousands": ".",
                        "sLengthMenu": "Sayfada _MENU_ kay&#305;t g&#246;ster",
                        "sLoadingRecords": "Y&#252;kleniyor...",
                        "sProcessing": "&#304;&#351;leniyor...",
                        "sSearch": "Ara:",
                        "sZeroRecords": "E&#351;le&#351;en kay&#305;t bulunamad&#305;",
                        "oPaginate": {
                            "sFirst": "&#304;lk",
                            "sLast": "Son",
                            "sNext": "Sonraki",
                            "sPrevious": "&#214;nceki"
                        }
                    }
                });
            }
        });
    </script>
</asp:Content>
