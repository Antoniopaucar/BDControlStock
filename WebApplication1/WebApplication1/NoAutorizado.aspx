<%@ Page Language="C#" AutoEventWireup="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Acceso Denegado</title>
    <style>
        body {
            background-color: #f8d7da;
            color: #721c24;
            font-family: 'Segoe UI', sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }
        .denegado-container {
            background: #fff;
            border: 1px solid #f5c6cb;
            border-radius: 8px;
            padding: 40px 30px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.1);
            text-align: center;
        }
        .denegado-container h1 {
            color: #721c24;
            margin-bottom: 20px;
        }
        .denegado-container a {
            color: #007BFF;
            text-decoration: none;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <div class="denegado-container">
        <h1>Acceso Denegado</h1>
        <p>No tienes permisos para acceder a esta página.</p>
        <a href="frmInicio.aspx">Volver al inicio</a>
    </div>
</body>
</html> 