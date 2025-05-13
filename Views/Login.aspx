<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RHControl.Views.Login" %>

<!DOCTYPE html>
<html lang="pt">
<head runat="server">
    <meta charset="utf-8" />
    <title>Login - RHControl</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fa;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container vh-100 d-flex justify-content-center align-items-center">
            <div class="row w-100 shadow rounded bg-white overflow-hidden" style="max-width: 900px;">

                <div class="col-md-6 d-none d-md-flex align-items-center justify-content-center p-4">
                    <img src="<%= ResolveUrl("~/Content/Img/LoginImg.png") %>"" alt="Login RH" class="img-fluid" />
                </div>

                <div class="col-md-6 p-5 d-flex flex-column justify-content-center">
                    <h2 class="mb-4 text-center">Bem-vindo ao RHControl</h2>

                    <div class="form-group mb-3">
                        <asp:Label ID="lblUsuario" runat="server" CssClass="form-label" Text="Usuário"></asp:Label>
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Digite seu usuário"></asp:TextBox>
                    </div>

                    <div class="form-group mb-4">
                        <asp:Label ID="lblSenha" runat="server" CssClass="form-label" Text="Senha"></asp:Label>
                        <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="form-control" placeholder="Digite sua senha"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnLogin" OnClick="btnLogin_Click" runat="server" Text="Entrar" CssClass="btn btn-primary w-100" />
                    <asp:Label ID="lblMensagemErro" runat="server" CssClass="text-danger mt-3 d-block text-center" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </form>

    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
