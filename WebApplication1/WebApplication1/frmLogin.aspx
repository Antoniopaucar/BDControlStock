<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="WebApplication1.frmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        /* Fondo general */
        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', sans-serif;
            background-image: url('img/fondo2.jpg'); /* Cambia la ruta según tu imagen */
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        /* Contenedor del formulario */
        .login-container {
            padding: 40px 30px;
            border-radius: 10px;
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
            width: 100%;
            max-width: 400px;
            text-align:center;
        }

        /* Título */
        .login-container h1 {
            text-align: center;
            margin-bottom: 25px;
            color: #333;
        }

        /* Campos de entrada */
        .login-container input[type="text"],
        .login-container input[type="password"] {
            width: 100%;
            padding: 12px;
            margin: 10px 0 20px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 16px;
            background-color:lightgrey;
        }

        /* Estilo base */
        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            font-family: 'Segoe UI', sans-serif;
            cursor: pointer;
            transition: background-color 0.3s ease, transform 0.2s ease;
            color: white;
        }

        /* Botón primario (azul) */
        .btn-primario {
            background-color: #007BFF;
        }
        .btn-primario:hover {
            background-color: #0056b3;
        }

        .auto-style1 {
            width: 201px;
            height: 166px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server" class="login-container">
        <img src="img/usuario2.png" class="auto-style1" />
            <table style="margin: auto; text-align: center;">
                <tr style="font-weight:bold;font-size:24px">
                    <td colspan="2">Usuario</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr style="font-weight:bold;font-size:24px">
                    <td colspan="2">Contraseña</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtClave" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAceptar" runat="server" OnClick="btnAceptar_Click" Text="Aceptar" CssClass="btn btn-primario"/>
                    </td>
                    <td>
                        <asp:Button ID="btnSalir" runat="server" Text="Salir" CssClass="btn btn-primario" OnClick="btnSalir_Click"/>
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
