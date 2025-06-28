<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="frmProveedores.aspx.cs" Inherits="WebApplication1.frmProveedores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="tablaProveedores" class="tablaForm" >
    <tr class="titulo">
        <td colspan="4">
            <asp:Label ID="Label2" runat="server" Text="Proveedores"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="ID:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txt_Id" runat="server" CssClass="full-width-textbox" Enabled="False"></asp:TextBox>
        </td>
        <td></td>
        <td>
            <asp:Button ID="btn_Buscar" runat="server" OnClick="btn_Buscar_Click" Text="BUSCAR" class="btn btn-info" visible="false"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Razon Social:"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txt_RazonSocial" runat="server" CssClass="full-width-textbox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Text="Representante:"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txt_Representante" runat="server" CssClass="full-width-textbox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="RUC:"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txt_Ruc" runat="server" CssClass="full-width-textbox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label6" runat="server" Text="Direccion:"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txt_Direccion" runat="server" CssClass="full-width-textbox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label11" runat="server" Text="Telefono:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txt_Telefono" runat="server" CssClass="full-width-textbox"></asp:TextBox>
        </td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label12" runat="server" Text="Email:"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txt_Email" runat="server" CssClass="full-width-textbox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btn_Agregar" runat="server" Text="AGREGAR" 
                OnClientClick="return confirm('¿Deseas agregar esta categoría?') && validarCamposTabla('tablaProveedores','txt_Id,txt_Representante');" 
                OnClick="btn_Agregar_Click" class="btn btn-exito" />
        </td>
        <td>
            <asp:Button ID="btn_Modificar" runat="server" Text="MODIFICAR" 
                OnClientClick="return confirm('¿Deseas modificar esta categoría?') && validarCamposTabla('tablaProveedores','txt_Id,txt_Representante');" 
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
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="full-width-textbox" placeholder="Buscar por razon social..." />
        <asp:Button ID="btnFiltrar" runat="server" Text="FILTRAR" CssClass="btn btn-info" OnClick="btnFiltrar_Click" />
    </div>
    <div style="margin-top: 10px;">
        <asp:Label ID="lblMensaje" runat="server" ForeColor="White" Font-Bold="true" />
    </div>
</div>

<asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="7"
    OnPageIndexChanging="gvProveedores_PageIndexChanging"
    OnRowCommand="gvProveedores_RowCommand"
    CssClass="gridview-style">
    <Columns>
        <asp:BoundField DataField="IdProveedor" HeaderText="ID" />
        <asp:BoundField DataField="RazonSocial" HeaderText="Razon Social" />
        <asp:BoundField DataField="Representante" HeaderText="Representante" />
        <asp:BoundField DataField="RUC" HeaderText="RUC" />
        <asp:BoundField DataField="Direccion" HeaderText="Direccion" />
        <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CommandName="Consultar"
                    CommandArgument='<%# Eval("IdProveedor") %>' CssClass="btn btn-info btn-sm" />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar"
                    CommandArgument='<%# Eval("IdProveedor") %>' CssClass="btn btn-peligro btn-sm"
                    OnClientClick="return confirm('¿Deseas eliminar este proveedor?');" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Content>
