<%@ Page Title="" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="Blog.aspx.cs" Inherits="Saat.Saat0604.Blog" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblKullaniciID" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="lblKullaniciAdi" runat="server" Visible="False"></asp:Label>
    <div class="page-header page-header-light">
        <div class="breadcrumb-line breadcrumb-line-light header-elements-md-inline">
            <div class="d-flex">
                <div class="breadcrumb">
                    <a class="breadcrumb-item" href="Anasayfa.aspx"><i class="icon-home2 mr-2"></i>Anasayfa</a>
                    <a class="breadcrumb-item" href="#">Blog</a>
                </div>
            </div>
            <div class="header-elements d-none">
                <div class="breadcrumb justify-content-center">
                    <button class="btn bg-purple-400 btn-ladda btn-ladda-progress ladda-button legitRipple" data-target="#aciklamalar" data-toggle="modal" type="button">Açıklamalar<i class="icon-question3 ml-2"></i></button>
                </div>
            </div>
        </div>
    </div>
    <div class="content">
        <div class="card">
            <div class="card-header header-elements-inline">
                <h5 class="card-title">Blog</h5>
                <div class="header-elements">
                    <asp:LinkButton ID="lnkYeniKayit" runat="server" class="btn btn-primary btn-lg" OnClick="lnkYeniKayit_Click"><i class="icon-checkmark3 mr-2"></i>Yeni Kayıt</asp:LinkButton>
                </div>
            </div>
            <div class="card-body">
                <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <fieldset class="mb-3">
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Başlık</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtBaslik" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Tarih</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtTarih" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Resim Seçiniz</label>
                            <div class="col-sm-3">
                                <asp:FileUpload ID="FileUpload1" runat="server" class="form-control" onchange="PreviewImage();" />
                            </div>
                            <label class="col-sm-7 control-label" for="input01" style="margin-top: 8px;">1200 x 800 Ebat Uygulanacaktır</label>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">&nbsp;</label>
                            <div class="col-sm-8">
                                <asp:Image ID="Image1" runat="server" Style="max-width: 100%;" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Kısa Açıklama</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtKisaAciklama" runat="server" class="form-control" TextMode="MultiLine" Height="75"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Açıklama</label>
                            <div class="col-sm-10">
                                <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" Height="500px">
                                </FCKeditorV2:FCKeditor>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Aktif</label>
                            <div class="col-sm-2">
                                <asp:RadioButtonList ID="rbAktif" runat="server" class="form-control" RepeatColumns="2">
                                    <asp:ListItem Value="1" Selected="True">Evet</asp:ListItem>
                                    <asp:ListItem Value="0">Hayır</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group row" >
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Sıralama</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtSiralama" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Google Anahtar</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtGoogleAnahtar" runat="server" class="form-control" TextMode="MultiLine" Height="75px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Google Açıklama</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtGoogleAciklama" runat="server" class="form-control" TextMode="MultiLine" Height="75px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkKaydet" runat="server" class="btn btn-success btn-lg" Visible="true" OnClick="lnkKaydet_Click"><i class="icon-checkmark3 mr-2"></i>Kaydet</asp:LinkButton>
                                <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" Visible="false" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton><asp:Label ID="lblDegistirID" runat="server" Visible="false"></asp:Label><asp:Label ID="lblSilinecekID" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </fieldset>
                    <div class="text-right">
                    </div>
                </asp:Panel>
            </div>
            <div class="table-responsive" style="padding-right: 10px;">
                <script type="text/javascript">
                    $(document).ready(function () {
                        $('#myTable').DataTable({
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
                <table id="myTable" class="table datatable-responsive" data-order='[[ 0, "desc" ]]'>
                    <thead>
                        <tr>
                            <th scope="col">No</th>
                            <th scope="col">Resim</th>
                            <th scope="col">Başlik</th>
                            <th scope="col">Aktif</th>
                            <th scope="col">Düzenle & Sil</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptListe" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <th scope="row"><%# Eval("ID") %></th>
                                    <td>
                                        <center>
                                            <div class="row">
                                                <div class="card">
                                                    <div class="card-img-actions m-1">
                                                        <img src='/upload/<%# Eval("resim") %>' style="max-width:150px;max-height:50px" class="card-img img-fluid" />
                                                        <div class="card-img-actions-overlay card-img">
                                                            <a href='/upload/<%# Eval("resim") %>' class="btn btn-outline bg-white text-white border-white border-2 btn-icon rounded-round" data-popup="lightbox" rel='group<%#Eval("ID") %>'>
                                                                <i class="icon-plus3"></i>
                                                            </a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </center>
                                    </td>
                                    <td><%# Eval("baslik") %></td>
                                    <td><%# string.Format("{0}", Eval("aktif").ToString()=="Evet"?"<span style='background-color:green; padding:10px; color: white; border-radius:5px;'>Evet</span>":"<span style='background-color:red; padding:10px; color: white; border-radius:5px;'>Hayır</span>" )%></td>
                                    <td>
                                        <asp:LinkButton ID="lnkDegistir" runat="server" class='btn btn-primary btn-lg' CommandArgument='<%# Eval("ID") %>' OnClick="lnkDegistir_Click"><i class='icon-gear mr-2'></i>Düzenle</asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lnkSil" runat="server" class='btn btn-danger btn-lg' OnClientClick="return confirm('Kayıt Silinecek Emin misiniz?');" CommandArgument='<%# Eval("ID") %>' OnClick="lnkSil_Click"><i class='icon-trash mr-2'></i>Sil</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Blog</h5>
                    <button class="close" data-dismiss="modal" type="button">&times;</button>
                </div>
                <div class="modal-body">
                    <hr />
                    <p>
                        Bu sayfa üzerinde yapacak olduğunuz tanımlamalar; Blog tanımlanması için kullanılmaktadır.
                    </p>
                </div>
                <div class="modal-footer">
                    <button class="btn bg-green-400 btn-ladda btn-ladda-progress ladda-button legitRipple" data-dismiss="modal" type="button">Kapat</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="X" runat="server" />
    <asp:HiddenField ID="Y" runat="server" />
    <asp:HiddenField ID="X2" runat="server" />
    <asp:HiddenField ID="Y2" runat="server" />
    <asp:HiddenField ID="eskiresim" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CustomScript" runat="server">
    <link href="/Saat0604/assets/js/jCrop/jquery.Jcrop.min.css" rel="stylesheet" />
    <script src="/Saat0604/assets/js/jCrop/jquery-migrate-1.2.1.js"></script>
    <script src="/Saat0604/assets/js/jCrop/jquery.Jcrop.min.js"></script>

    <script type="text/javascript">
        var jcrop_api;//jCrop apisi için boş değişken
        var count = 1; //jCrop api destroy kontrolü için
        var oldImg = $("#ContentPlaceHolder1_Image1").clone();//var olan resmi değişkene yükle
        function PreviewImage() {
            //seçilen dosyayı imgHolder'a yükle
            var oFReader = new FileReader();
            oFReader.readAsDataURL(document.getElementById("ContentPlaceHolder1_FileUpload1").files[0]);

            oFReader.onload = function (oFREvent) {
                document.getElementById("ContentPlaceHolder1_Image1").src = oFREvent.target.result;
            };//seçilen dosyayı imgHolder'a yükle SON

            try {//jcrop_api tanımsız, null veya boş ise hatayı yakala
                jcrop_api.destroy();
                if (count === 1) {
                    setTimeout(PreviewImage, 200);
                    count = 2;
                }
            } catch (e) {
                count = 1;
            }
            setTimeout(size, 200);//0.2sn bekle (resim imgHolder'a yüklensin diye) size fonk.u çağır
        };

        function size() {//resim boyut kontrolü
            var img = document.getElementById('ContentPlaceHolder1_Image1');
            var width = img.naturalWidth;
            var height = img.naturalHeight;
            if (width >= 1200 && height >= 800) {//minmum genişlik ve yükseklik kontrolü
                setTimeout(crop, 200); //0.2sn bekle ve crop'u çağır(fonksiyon çakışmalarını önlmek için)
            } else {//boyut uymuyorsa hata mesajı ve resmi resetle
                alert('Minimum ebat 1200X800 olmalı');//Resim boyutu uyarısı
                $("#ContentPlaceHolder1_Image1").replaceWith(oldImg); //eski resmi geri yükle
                oldImg = $("#ContentPlaceHolder1_Image1").clone();//eski resmi tekrar deşikene yükle
                $("#ContentPlaceHolder1_FileUpload1").val(""); //dosya yükleyiciyi boşalt
            }
        }

        function crop() {//Kırpma işlemi
            var img = document.getElementById('ContentPlaceHolder1_Image1');

            var width = img.naturalWidth;
            var height = img.naturalHeight;

            jQuery(document).ready(function () {
                jQuery('#ContentPlaceHolder1_Image1').Jcrop({
                    bgColor: 'black', //seçilmemiş alan rengi
                    bgOpacity: 0.4, //seçilmemiş alan transparanlığı
                    trueSize: [width, height],//orijnal dosya boyutu genişlil, yükseklik
                    minSize: [1200, 800],//minimum dosya boyutu genişlik,yükselik
                    setSelect: [0, 0, 1200, 800],//seçimalanını göster
                    aspectRatio: 120 / 80,//genişlik bölü yükseklik oranı (Buradaki değer olması gereken resmin genişliğin yüksekliğe oranı tamsayı olarak yerleşecek ör: 0.5 yerine 1/2 yazılacak)
                    onSelect: storeCoords
                }, function () { jcrop_api = this });//jCrop apisini oluştur
            });

            function storeCoords(c) {//seçim alanını forma işle
                jQuery('#ContentPlaceHolder1_X').val(c.x);
                jQuery('#ContentPlaceHolder1_Y').val(c.y);
                jQuery('#ContentPlaceHolder1_X2').val(c.w);
                jQuery('#ContentPlaceHolder1_Y2').val(c.h);
            };
        }
    </script>
</asp:Content>
