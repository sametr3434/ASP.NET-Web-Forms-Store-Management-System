<%@ Page Title="M&#252;&#351;teri Detay" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="MusteriDetay.aspx.cs" Inherits="Saat.Saat0604.MusteriDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header page-header-light">
        <div class="page-header-content header-elements-md-inline">
            <div class="page-title d-flex">
                <h4><i class="icon-user mr-2"></i> <span class="font-weight-semibold">Panel</span> - M&#252;&#351;teri Detay</h4>
            </div>
        </div>
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="/Saat0604/Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i> Panel</a>
                    <a href="/Saat0604/MusteriListe.aspx" class="breadcrumb-item">M&#252;&#351;teri Listesi</a>
                    <span class="breadcrumb-item active">M&#252;&#351;teri Detay</span>
                </div>
            </div>
            <div class="header-elements">
                <a href="MusteriListe.aspx" class="btn btn-light btn-icon" title="Listeye D&#246;n"><i class="icon-arrow-left8"></i></a>
                <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yard&#305;m"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">A&#231;&#305;klamalar</span></a>
            </div>
        </div>
    </div>

    <div class="content">
        <asp:Panel ID="pnlMusteriYok" runat="server" Visible="false" CssClass="alert alert-warning">
            M&#252;&#351;teri bulunamad&#305; veya ge&#231;ersiz ID.
        </asp:Panel>

        <asp:Panel ID="pnlIcerik" runat="server">
            <div class="card">
                <div class="card-header header-elements-inline">
                    <h5 class="card-title"><asp:Literal ID="ltrMusteriAdi" runat="server"></asp:Literal></h5>
                    <div class="header-elements">
                        <asp:Label ID="badgeAktif" runat="server" CssClass="badge badge-success" Text="Aktif"></asp:Label>
                        <asp:Label ID="badgePasif" runat="server" CssClass="badge badge-danger" Text="Pasif" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="card-body">
                    <ul class="nav nav-tabs nav-tabs-highlight">
                        <li class="nav-item"><a href="#genel" class="nav-link active" data-toggle="tab"><i class="icon-user mr-2"></i>Genel Bilgiler</a></li>
                        <li class="nav-item"><a href="#siparisler" class="nav-link" data-toggle="tab"><i class="icon-basket mr-2"></i>Sipari&#351;ler</a></li>
                        <li class="nav-item"><a href="#sepet" class="nav-link" data-toggle="tab"><i class="icon-cart5 mr-2"></i>Sepet</a></li>
                        <li class="nav-item"><a href="#odeme" class="nav-link" data-toggle="tab"><i class="icon-credit-card mr-2"></i>&#214;deme Ge&#231;mi&#351;i</a></li>
                    </ul>
                    <div class="tab-content">
                        <!-- Genel Bilgiler -->
                        <div class="tab-pane fade show active" id="genel">
                            <div class="mt-4">
                                <div class="form-group row">
                                    <label class="col-sm-3 font-weight-semibold">Ad Soyad</label>
                                    <div class="col-sm-9"><asp:Literal ID="ltrAdSoyad" runat="server"></asp:Literal></div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-3 font-weight-semibold">E-Posta</label>
                                    <div class="col-sm-9"><asp:Literal ID="ltrEPosta" runat="server"></asp:Literal></div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-3 font-weight-semibold">Telefon</label>
                                    <div class="col-sm-9"><asp:Literal ID="ltrTelefon" runat="server"></asp:Literal></div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-3 font-weight-semibold">Adres</label>
                                    <div class="col-sm-9"><asp:Literal ID="ltrAdres" runat="server"></asp:Literal></div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-3 font-weight-semibold">&#304;l / &#304;l&#231;e / Posta Kodu</label>
                                    <div class="col-sm-9"><asp:Literal ID="ltrIlIlce" runat="server"></asp:Literal></div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-3 font-weight-semibold">Kay&#305;t Tarihi</label>
                                    <div class="col-sm-9"><asp:Literal ID="ltrKayitTarihi" runat="server"></asp:Literal></div>
                                </div>
                                <div class="form-group row">
                                    <label class="col-sm-3 font-weight-semibold">Son Giri&#351; Tarihi</label>
                                    <div class="col-sm-9"><asp:Literal ID="ltrSonGiris" runat="server"></asp:Literal></div>
                                </div>
                            </div>
                        </div>

                        <!-- Siparişler -->
                        <div class="tab-pane fade" id="siparisler">
                            <div class="mt-4">
                                <div class="table-responsive">
                                    <table id="siparisTable" class="table table-hover table-striped w-100" data-order='[[ 0, "desc" ]]'>
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>Sipari&#351; No</th>
                                                <th>Tarih</th>
                                                <th class="text-right">Toplam</th>
                                                <th>&#214;deme</th>
                                                <th>Durum</th>
                                                <th class="text-center">&#304;&#351;lem</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptSiparisler" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Eval("ID") %></td>
                                                        <td><%# Duzelt(Eval("SiparisNo")) %></td>
                                                        <td><%# Eval("SiparisTarihi") != DBNull.Value ? Convert.ToDateTime(Eval("SiparisTarihi")).ToString("dd.MM.yyyy HH:mm") : "-" %></td>
                                                        <td class="text-right"><%# SiparisToplamGoster(Eval("ToplamTutar"), Eval("ParaBirimi")) %></td>
                                                        <td><%# Duzelt(Eval("OdemeDurumu")) %></td>
                                                        <td><%# Duzelt(Eval("SiparisDurumu")) %></td>
                                                        <td class="text-center">
                                                            <a href='SiparisDetay.aspx?id=<%# Eval("ID") %>' class='btn btn-sm btn-primary'><i class='icon-eye'></i></a>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                                <asp:Panel ID="pnlSiparisBos" runat="server" Visible="false" CssClass="text-center text-muted py-4">
                                    Hen&#252;z sipari&#351; bulunmuyor.
                                </asp:Panel>
                            </div>
                        </div>

                        <!-- Sepet -->
                        <div class="tab-pane fade" id="sepet">
                            <div class="mt-4">
                                <div class="table-responsive">
                                    <table id="sepetTable" class="table table-hover table-striped w-100">
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>&#220;r&#252;n</th>
                                                <th class="text-right">Adet</th>
                                                <th class="text-right">Birim</th>
                                                <th class="text-right">Sat&#305;r</th>
                                                <th>Eklenme Tarihi</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptSepet" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Eval("ID") %></td>
                                                        <td><%# Duzelt(Eval("UrunAdi")) %></td>
                                                        <td class="text-right"><%# Eval("Adet") %></td>
                                                        <td class="text-right"><%# SepetBirimGoster(Eval("BirimFiyat"), Eval("ParaBirimi")) %></td>
                                                        <td class="text-right"><%# SepetSatirTutar(Eval("Adet"), Eval("BirimFiyat"), Eval("ParaBirimi")) %></td>
                                                        <td><%# Eval("EklenmeTarihi") != DBNull.Value ? Convert.ToDateTime(Eval("EklenmeTarihi")).ToString("dd.MM.yyyy HH:mm") : "-" %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                                <asp:Panel ID="pnlSepetBos" runat="server" Visible="false" CssClass="text-center text-muted py-4">
                                    Sepet bo&#351; veya misafir al&#305;&#351;veri&#351;i.
                                </asp:Panel>
                            </div>
                        </div>

                        <!-- Ödeme Geçmişi -->
                        <div class="tab-pane fade" id="odeme">
                            <div class="mt-4">
                                <div class="table-responsive">
                                    <table id="odemeTable" class="table table-hover table-striped w-100" data-order='[[ 2, "desc" ]]'>
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>Sipari&#351; No</th>
                                                <th>Tarih</th>
                                                <th class="text-right">Tutar</th>
                                                <th>Durum</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptOdeme" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Eval("ID") %></td>
                                                        <td><%# Duzelt(Eval("SiparisNo")) %></td>
                                                        <td><%# Eval("OdemeTarihi") != DBNull.Value ? Convert.ToDateTime(Eval("OdemeTarihi")).ToString("dd.MM.yyyy HH:mm") : (Eval("KayitTarihi") != DBNull.Value ? Convert.ToDateTime(Eval("KayitTarihi")).ToString("dd.MM.yyyy HH:mm") : "-") %></td>
                                                        <td class="text-right"><%# OdemeTutarGoster(Eval("TutarOrijinalPB"), Eval("ParaBirimi"), Eval("TutarTRY"), Eval("Tutar")) %></td>
                                                        <td><%# Duzelt(Eval("OdemeDurumu")) %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                                <asp:Panel ID="pnlOdemeBos" runat="server" Visible="false" CssClass="text-center text-muted py-4">
                                    &#214;deme kayd&#305; bulunmuyor.
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">M&#252;&#351;teri Detay</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <p><strong>Genel Bilgiler:</strong> M&#252;&#351;teri ileti&#351;im ve adres bilgileri.</p>
                    <p><strong>Sipari&#351;ler:</strong> M&#252;&#351;terinin verdi&#287;i sipari&#351;ler. Detay i&#231;in g&#246;z ikonuna t&#305;klay&#305;n.</p>
                    <p><strong>Sepet:</strong> M&#252;&#351;terinin giri&#351; yapm&#305;&#351;ken sepete ekledi&#287;i &#252;r&#252;nler.</p>
                    <p><strong>&#214;deme Ge&#231;mi&#351;i:</strong> PayTR &#252;zerinden yap&#305;lan &#246;demelerin kay&#305;tlar&#305;.</p>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            if ($('#siparisTable tbody tr').length > 0) {
                $('#siparisTable').DataTable({
                    language: { "sSearch": "Ara:", "sLengthMenu": "Sayfada _MENU_ kay&#305;t", "sInfo": "_TOTAL_ kay&#305;ttan _START_-_END_ aras&#305;", "sZeroRecords": "E&#351;le&#351;en kay&#305;t yok", "oPaginate": { "sFirst": "&#304;lk", "sLast": "Son", "sNext": "Sonraki", "sPrevious": "&#214;nceki" } }
                });
            }
            if ($('#sepetTable tbody tr').length > 0) {
                $('#sepetTable').DataTable({
                    language: { "sSearch": "Ara:", "sLengthMenu": "Sayfada _MENU_ kay&#305;t", "sInfo": "_TOTAL_ kay&#305;ttan _START_-_END_ aras&#305;", "sZeroRecords": "E&#351;le&#351;en kay&#305;t yok", "oPaginate": { "sFirst": "&#304;lk", "sLast": "Son", "sNext": "Sonraki", "sPrevious": "&#214;nceki" } }
                });
            }
            if ($('#odemeTable tbody tr').length > 0) {
                $('#odemeTable').DataTable({
                    language: { "sSearch": "Ara:", "sLengthMenu": "Sayfada _MENU_ kay&#305;t", "sInfo": "_TOTAL_ kay&#305;ttan _START_-_END_ aras&#305;", "sZeroRecords": "E&#351;le&#351;en kay&#305;t yok", "oPaginate": { "sFirst": "&#304;lk", "sLast": "Son", "sNext": "Sonraki", "sPrevious": "&#214;nceki" } }
                });
            }
        });
    </script>
</asp:Content>
