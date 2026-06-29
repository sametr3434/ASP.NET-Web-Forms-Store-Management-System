<%@ Page Title="" Language="C#" MasterPageFile="~/Saat0604/MasterPage.Master" AutoEventWireup="true" CodeBehind="Hakkimizda.aspx.cs" Inherits="Saat.Saat0604.Hakkimizda" %>
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
                    <a class="breadcrumb-item" href="#">Hakkımızda</a>
                </div>
            </div>
            <div class="header-elements">
                <a href="#" class="btn btn-light btn-icon" data-toggle="modal" data-target="#aciklamalar" title="Yardım"><i class="icon-question3"></i> <span class="d-none d-md-inline ml-1">A&#231;&#305;klamalar</span></a>
            </div>
        </div>
    </div>
    <div class="content">
        <div class="card">
            <div class="card-body">
                <asp:Panel ID="Panel1" runat="server">
                    <fieldset class="mb-3">
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Başlık</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtBaslik" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Kısa Açıklama</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtKisaAciklama" runat="server" class="form-control" TextMode="MultiLine" Height="75px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Resim Seçiniz</label>
                            <div class="col-sm-3">
                                <asp:FileUpload ID="FileUpload1" runat="server" class="form-control" onchange="PreviewImage();" />
                            </div>
                            <label class="col-sm-7 control-label" for="input01" style="margin-top: 8px;">650 x 980 Ebat Uygulanacaktır</label>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">&nbsp;</label>
                            <div class="col-sm-8">
                                <asp:Image ID="Image1" runat="server" style="max-width: 100%;" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">İçerik</label>
                            <div class="col-sm-10">
                                <FCKeditorV2:FCKeditor ID="fckIcerik" runat="server" Height="500px">
                                </FCKeditorV2:FCKeditor>
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
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Rakam Başlık 1</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRakamBaslik1" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Rakam 1</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRakam1" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Rakam Başlık 2</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRakamBaslik2" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Rakam 2</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRakam2" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Rakam Başlık 3</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRakamBaslik3" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Rakam 3</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRakam3" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Rakam Başlık 4</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRakamBaslik4" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <label class="col-sm-2 control-label" for="input01" style="margin-top: 8px;">Rakam 4</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRakam4" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-form-label col-lg-2"></label>
                            <div class="col-lg-10">
                                <asp:LinkButton ID="lnkGuncelle" runat="server" class="btn btn-info btn-lg" OnClick="lnkGuncelle_Click"><i class="icon-checkmark3 mr-2"></i>Güncelle</asp:LinkButton><asp:Label ID="lblDegistirID" runat="server" Visible="false"></asp:Label><asp:Label ID="lblSilinecekID" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </fieldset>
                    <div class="text-right">
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div id="aciklamalar" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Hakk&#305;m&#305;zda</h5>
                    <button class="close" data-dismiss="modal" type="button">&times;</button>
                </div>
                <div class="modal-body">
                    <p>Bu sayfa &#252;zerinde yapacak oldu&#287;unuz tan&#305;mlamalar; sitede bulunan Hakk&#305;m&#305;zda b&#246;l&#252;m&#252;n&#252;n i&#231;eri&#287;ine kaydedilecektir.</p>
                    <p><strong>Rakam alanlar&#305;:</strong> &#214;rn. "Mutlu M&#252;&#351;teri" - 15000 gibi istatistik g&#246;sterimleri i&#231;in kullan&#305;l&#305;r.</p>
                    <p><strong>Resim:</strong> 650 x 980 piksel ebat &#246;nerilir.</p>
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
            if (width >= 650 && height >= 980) {//minmum genişlik ve yükseklik kontrolü
                setTimeout(crop, 200); //0.2sn bekle ve crop'u çağır(fonksiyon çakışmalarını önlmek için)
            } else {//boyut uymuyorsa hata mesajı ve resmi resetle
                alert('Minimum ebat 650X980 olmalı');//Resim boyutu uyarısı
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
                    minSize: [650, 980],//minimum dosya boyutu genişlik,yükselik
                    setSelect: [0, 0, 650, 980],//seçimalanını göster
                    aspectRatio: 161 / 244,//genişlik bölü yükseklik oranı (Buradaki değer olması gereken resmin genişliğin yüksekliğe oranı tamsayı olarak yerleşecek ör: 0.5 yerine 1/2 yazılacak)
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
