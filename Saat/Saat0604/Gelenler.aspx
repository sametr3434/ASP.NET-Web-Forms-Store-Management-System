<%@ Page Title="" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="Gelenler.aspx.cs" Inherits="Saat.Saat0604.Gelenler" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblKullaniciID" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblKullaniciAdi" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblInceleID" runat="server" Visible="False"></asp:Label>

    <div class="page-header page-header-light">
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a href="Anasayfa.aspx" class="breadcrumb-item"><i class="icon-home2 mr-2"></i>Anasayfa</a>
                    <a href="#" class="breadcrumb-item">Gelenler</a>
                </div>
            </div>
            <div class="header-elements">
                <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yardım"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">A&#231;&#305;klamalar</span></a>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Gelen Mesajlar</h5>
            </div>
            <asp:Panel ID="pnlIncele" runat="server" Visible="false" CssClass="card-body border-bottom">
                <fieldset>
                    <div class="form-group row">
                        <label class="col-form-label col-lg-2 font-weight-semibold">Tarih / Saat</label>
                        <div class="col-lg-2"><asp:Literal ID="ltrTarihSaat" runat="server"></asp:Literal></div>
                        <label class="col-form-label col-lg-1 font-weight-semibold">IP Adresi</label>
                        <div class="col-lg-2"><asp:Literal ID="ltrIPAdresi" runat="server"></asp:Literal></div>
                    </div>
                    <div class="form-group row">
                        <label class="col-form-label col-lg-2 font-weight-semibold">Adı Soyadı</label>
                        <div class="col-lg-2"><asp:Literal ID="ltrIsimSoyisim" runat="server"></asp:Literal></div>
                        <label class="col-form-label col-lg-1 font-weight-semibold">Telefon</label>
                        <div class="col-lg-2"><asp:Literal ID="ltrTelefon" runat="server"></asp:Literal></div>
                    </div>
                    <div class="form-group row">
                        <label class="col-form-label col-lg-2 font-weight-semibold">Mail Adresi</label>
                        <div class="col-lg-4"><asp:Literal ID="ltrMailAdresi" runat="server"></asp:Literal></div>
                    </div>
                    <div class="form-group row">
                        <label class="col-form-label col-lg-2 font-weight-semibold">Mesaj İçeriği</label>
                        <div class="col-lg-6"><asp:Literal ID="ltrMesaj" runat="server"></asp:Literal></div>
                    </div>
                    <div class="form-group row">
                        <div class="col-lg-8 text-right">
                            <asp:LinkButton ID="lnkOkunduYap" runat="server" CssClass="btn btn-success" OnClick="lnkOkunduYap_Click" Visible="false"><i class="icon-checkmark3 mr-2"></i>Okundu Olarak İşaretle</asp:LinkButton>
                            <asp:LinkButton ID="lnkKapat" runat="server" CssClass="btn btn-light" OnClick="lnkKapat_Click"><i class="icon-cross2 mr-2"></i>Kapat</asp:LinkButton>
                        </div>
                    </div>
                </fieldset>
            </asp:Panel>
            <div class="card-body">
                <div class="table-responsive">
                    <table id="tblGelenler" class="table table-hover table-striped">
                        <thead>
                            <tr>
                                <th>No</th>
                                <th>Tarih</th>
                                <th>Adı Soyadı</th>
                                <th>Telefon</th>
                                <th>Mail</th>
                                <th>Okundu</th>
                                <th>İşlem</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptGelenler" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ID") %></td>
                                        <td><%# Eval("TarihSaat", "{0:dd.MM.yyyy HH:mm}") %></td>
                                        <td><%# Duzelt(Eval("Isim")) %></td>
                                        <td><a href='tel:<%# Eval("Telefon") %>'><%# Duzelt(Eval("Telefon")) %></a></td>
                                        <td><a href='mailto:<%# Eval("MailAdresi") %>'><%# Duzelt(Eval("MailAdresi")) %></a></td>
                                        <td>
                                            <span class='badge <%# Duzelt(Eval("Okundu")) == "Evet" ? "badge-success" : "badge-danger" %>'><%# Duzelt(Eval("Okundu")) %></span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkIncele" runat="server" CssClass="btn btn-sm btn-primary" OnClick="lnkIncele_Click" CommandArgument='<%# Eval("ID") %>'><i class="icon-eye"></i> İncele</asp:LinkButton>
                                            <asp:LinkButton ID="lnkSil" runat="server" CssClass="btn btn-sm btn-danger" OnClick="lnkSil_Click" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Bu mesajı silmek istediğinize emin misiniz?');"><i class="icon-trash"></i> Sil</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <asp:Panel ID="pnlBos" runat="server" Visible="false" CssClass="text-center text-muted py-4">
                    Henüz gelen mesaj bulunmuyor.
                </asp:Panel>
            </div>
        </div>
    </div>

    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Gelen Mesajlar</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <p>Bu sayfa, ileti&#351;im formundan gelen mesajlar&#305; listeler. Tarih, ad soyad, telefon, mail ve mesaj i&#231;eri&#287;i g&#246;r&#252;nt&#252;lenir.</p>
                    <p><strong>Okundu:</strong> Mesaj&#305; inceleyip "Okundu Olarak &#304;&#351;aretle" ile g&#252;nceller. Silinen mesajlar geri al&#305;namaz.</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CustomScript" runat="server">
    <script>
        $(document).ready(function () {
            if ($('#tblGelenler tbody tr').length > 0) {
                $('#tblGelenler').DataTable({
                    order: [[0, 'desc']],
                    language: {
                        "sSearch": "Ara:",
                        "sLengthMenu": "Sayfada _MENU_ kayıt",
                        "sInfo": "_TOTAL_ kayıttan _START_-_END_ arası",
                        "sInfoEmpty": "Kayıt yok",
                        "sZeroRecords": "Eşleşen kayıt bulunamadı",
                        "oPaginate": { "sFirst": "İlk", "sLast": "Son", "sNext": "Sonraki", "sPrevious": "Önceki" }
                    }
                });
            }
        });
    </script>
</asp:Content>
