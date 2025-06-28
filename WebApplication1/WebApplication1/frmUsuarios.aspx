<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="frmUsuarios.aspx.cs" Inherits="WebApplication1.frmUsuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="tablaUsuarios" class="tablaForm" >
        <tr class="titulo">
            <td colspan="4">
                <asp:Label ID="Label2" runat="server" Text="Usuarios"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Id:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txt_IdUsuario" runat="server" Enabled="False"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btn_Buscar" runat="server" OnClick="btn_Buscar_Click" Text="BUSCAR" class="btn btn-info" Visible="false"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Usuario:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txt_NombreUsuario" runat="server" CssClass="full-width-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Nombres:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txt_Nombres" runat="server" CssClass="full-width-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Apellidos:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txt_Apellidos" runat="server" CssClass="full-width-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Contraseña:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txt_Contrasenia" runat="server" TextMode="Password" MaxLength="15"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Repetir Contraseña:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txt_Contrasenia2" runat="server" TextMode="Password" MaxLength="15"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Text="Rol:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:DropDownList ID="Ddl_Rol" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:CheckBox ID="chb_Estado" runat="server" Text="Habilitado"></asp:CheckBox>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_Agregar" runat="server" Text="AGREGAR" 
                    OnClientClick="return confirm('¿Deseas agregar esta categoría?') && validarCamposTabla('tablaUsuarios','txt_IdUsuario,txt_Nombres,txt_Apellidos');" 
                    OnClick="btn_Agregar_Click" class="btn btn-exito" />
            </td>
            <td>
                <%--<asp:Button ID="btn_Modificar" runat="server" Text="MODIFICAR" 
                    OnClientClick="return confirm('¿Deseas modificar esta categoría?') && validarCamposTabla('tablaUsuarios','txt_IdUsuario,txt_Nombres,txt_Apellidos');" 
                    OnClick="btn_Modificar_Click" class="btn btn-primario" />--%>
                    <asp:Button ID="btn_Modificar" runat="server" Text="MODIFICAR" 
                     OnClientClick ="return confirm('¿Deseas modificar esta categoría?') && validarCamposTabla('tablaUsuarios','txt_IdUsuario,txt_Nombres,txt_Apellidos,txt_Contrasenia,txt_Contrasenia2');" 
                     OnClick="btn_Modificar_Click" class="btn btn-primario" />
            </td>
            <td>
                <asp:Button ID="btn_Limpiar" runat="server" Text="LIMPIAR" OnClick="btn_Limpiar_Click" class="btn btn-advertencia" />
            </td>
            <td>
                <asp:Button ID="btn_Eliminar" runat="server" Text="ELIMINAR" 
                    OnClientClick="return confirm('¿Deseas eliminar esta categoría?');"
                    OnClick="btn_Eliminar_Click" class="btn btn-peligro" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

<div style="width: 40%; margin: 20px auto; text-align: center;">
    <div style="display: flex; justify-content: center; gap: 10px;">
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="full-width-textbox" placeholder="Buscar por nombre de usuario..." />
        <asp:Button ID="btnFiltrar" runat="server" Text="FILTRAR" CssClass="btn btn-info" OnClick="btnFiltrar_Click" />
    </div>
    <div style="margin-top: 10px;">
        <asp:Label ID="lblMensaje" runat="server" ForeColor="White" Font-Bold="true" />
    </div>
</div>
<style>
    .sticky-header th {
    position: sticky;
    top: 0;   
    z-index: 2;
}
</style>


    <div style="max-height: 300px; overflow: auto;">
    <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="5"
        OnPageIndexChanging="gvUsuarios_PageIndexChanging"
        OnRowCommand="gvUsuarios_RowCommand"
        CssClass="gridview-style sticky-header">    
        <Columns>
            <asp:BoundField DataField="IdUsuario" HeaderText="ID" />
            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
            <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
            <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
            <asp:BoundField DataField="Contrasenia" HeaderText="Contraseña" />
            <asp:BoundField DataField="IdRol.Nombre" HeaderText="Rol" />
            <asp:BoundField DataField="Estado" HeaderText="Estado" />
            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CommandName="Consultar"
                        CommandArgument='<%# Eval("IdUsuario") %>' CssClass="btn btn-info btn-sm" />
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar"
                        CommandArgument='<%# Eval("IdUsuario") %>' CssClass="btn btn-peligro btn-sm"
                        OnClientClick="return confirm('¿Deseas eliminar este usuario?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>

</asp:Content>
