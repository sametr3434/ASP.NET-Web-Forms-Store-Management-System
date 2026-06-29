<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Saat.Saat0604.Default" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css" />
    <link href="/Saat0604/global_assets/css/icons/icomoon/styles.min.css" type="text/css" rel="stylesheet" />
    <link href="/Saat0604/global_assets/css/icons/fontawesome/styles.min.css" type="text/css" rel="stylesheet" />
    <link href="/Saat0604/global_assets/css/icons/material/styles.min.css" type="text/css" rel="stylesheet" />
    <link href="/Saat0604/assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/Saat0604/assets/css/bootstrap_limitless.min.css" rel="stylesheet" type="text/css" />
    <link href="/Saat0604/assets/css/layout.min.css" rel="stylesheet" type="text/css" />
    <link href="/Saat0604/assets/css/components.min.css" rel="stylesheet" type="text/css" />
    <link href="/Saat0604/assets/css/colors.min.css" rel="stylesheet" type="text/css" />
    <script src="/Saat0604/global_assets/js/main/jquery.min.js"></script>
    <script src="/Saat0604/global_assets/js/main/bootstrap.bundle.min.js"></script>
    <script src="/Saat0604/global_assets/js/plugins/loaders/blockui.min.js"></script>
    <script src="/Saat0604/global_assets/js/plugins/extensions/jquery_ui/interactions.min.js"></script>
    <script src="/Saat0604/global_assets/js/plugins/forms/selects/select2.min.js"></script>
    <script src="/Saat0604/assets/js/app.js"></script>
    <script src="/Saat0604/global_assets/js/demo_pages/form_select2.js"></script>

    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
</head>
<body>
    <div class="page-content">
        <div class="content-wrapper">
            <div class="content d-flex justify-content-center align-items-center">
                <div class="login-form">
                    <div class="card mb-0">
                        <form id="form1" runat="server">
                            <asp:Label ID="lblKullaniciID" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblKullaniciAdi" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblYetki" runat="server" Visible="false"></asp:Label>
                            <div class="card-body">
                                <div class="text-center mb-3">
                                    <asp:Literal ID="ltrLogo" runat="server"></asp:Literal>
                                    <hr />
                                </div>
                                <div class="form-group form-group-feedback form-group-feedback-left">
                                    <asp:TextBox ID="txtKullaniciAdi" runat="server" class="form-control" placeholder="Kullanıcı Adı"></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtKullaniciAdi" Display="Dynamic" ErrorMessage="Sadece metin, rakam ve max 250 karakter" Style="color: red;" ValidationExpression="^['a-zA-Z''0-9''ıİğĞüÜşŞçÇöÖ''\-._,!'\s]{1,250}$"></asp:RegularExpressionValidator>
                                    <div class="form-control-feedback">
                                        <i class="icon-user text-muted"></i>
                                    </div>
                                </div>
                                <div class="form-group form-group-feedback form-group-feedback-left">
                                    <div class="position-relative">
                                        <asp:TextBox ID="txtSifre" runat="server" TextMode="Password" class="form-control" placeholder="&#350;ifre" style="padding-right: 2.5rem;"></asp:TextBox>
                                        <span id="btnSifreGoster" class="position-absolute" style="right: 0.75rem; top: 50%; transform: translateY(-50%); cursor: pointer; z-index: 5;" title="&#350;ifreyi g&#246;ster">
                                            <i class="icon-eye text-muted" id="icoSifre"></i>
                                        </span>
                                    </div>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtSifre" Display="Dynamic" ErrorMessage="Sadece metin, rakam ve max 250 karakter" Style="color: red;" ValidationExpression="^['a-zA-Z''0-9''ıİğĞüÜşŞçÇöÖ''\-._,!'\s]{1,250}$"></asp:RegularExpressionValidator>
                                    <div class="form-control-feedback">
                                        <i class="icon-lock2 text-muted"></i>
                                    </div>
                                </div>
                                <%-- TUZLA TEST --%>
                                  <div class="g-recaptcha" data-sitekey="6LdIIZAsAAAAAMsQAFPIv0hnkmO78mKd5lXmmcUR" style="margin-left: -13px; margin-bottom: 15px;"></div>
                                
                                <%--LOCAL--%>
                                <%--<div class="g-recaptcha" data-sitekey="6LcvVRQaAAAAALZT1Zl4-P4eP9kQPQ_23DggSY91" style="margin-left: -13px; margin-bottom: 15px;"></div>--%>
                                <div class="row">
                                    <div class="col-md-12 text-center">
                                        <asp:LinkButton ID="lnkGiris" runat="server" class="btn btn-outline bg-indigo-400 text-indigo-400 border-indigo-400 col-md-8 text-center" Font-Bold="true" OnClick="lnkGiris_Click">Giriş <i class="icon-key ml-2"></i></asp:LinkButton>
                                    </div>
                                </div>
                                <hr />
                            </div>
                        </form>
                    </div>
                </div>
            </div>
            <div class="navbar navbar-expand-lg navbar-light" style="position: fixed; bottom: 0; left: 0; right: 0; margin: 0; z-index: 100;">
                <div class="text-center d-lg-none w-100">
                    <button type="button" class="navbar-toggler dropdown-toggle" data-toggle="collapse" data-target="#navbar-footer">
                        <i class="icon-unfold mr-2"></i>
                        Footer
                    </button>
                </div>
                <div class="navbar-collapse collapse" id="navbar-footer">
                    <span class="navbar-text">
                        <asp:Literal ID="ltrFooterBaslik" runat="server"></asp:Literal>
                        © 2024 &nbsp;&nbsp;<a href="http://www.tuzlatasarim.com" target="_blank">
                            <img src="/Upload/tuzlatasarim.png" style="height: 30px;" />
                        </a>&nbsp;&nbsp;|&nbsp;&nbsp; <a href="tel:05383122580">0 538 312 25 80</a>
                    </span>
                    <%--<ul class="navbar-nav ml-lg-auto">
                    <li class="nav-item"><a href="mailto:info@tuzlatasarim.com" class="navbar-nav-link legitRipple" target="_blank"><i class="icon-lifebuoy mr-2"></i>Destek</a></li>
                </ul>--%>
                </div>
            </div>
        </div>
    </div>
    <script>
        if (window.history.replaceState) {
            window.history.replaceState(null, null, window.location.href);
        }
        (function () {
            var btn = document.getElementById('btnSifreGoster');
            var ico = document.getElementById('icoSifre');
            var sifre = document.getElementById('<%= txtSifre.ClientID %>');
            if (btn && ico && sifre) {
                btn.onclick = function () {
                    if (sifre.type === 'password') {
                        sifre.type = 'text';
                        ico.className = 'icon-eye-blocked text-muted';
                        btn.title = '&#350;ifreyi gizle';
                    } else {
                        sifre.type = 'password';
                        ico.className = 'icon-eye text-muted';
                        btn.title = '&#350;ifreyi g&#246;ster';
                    }
                };
            }
        })();
    </script>
</body>
</html>
