<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="frmCategorias.aspx.cs" Inherits="WebApplication1.frmCategorias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table id="tablaCategorias" class="tablaForm" >
            <tr class="titulo">
                <td colspan="4">Categorias</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblId" runat="server" Text="ID:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_Id" runat="server" CssClass="full-width-textbox" Enabled="False"></asp:TextBox>
                </td>
                <td></td>
                <td>
                    <asp:Button ID="btn_Buscar" runat="server" Text="BUSCAR" OnClick="btn_Buscar_Click" class="btn btn-info" Visible="false"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txt_Nombre" runat="server" CssClass="full-width-textbox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Descripción:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txt_Descripcion" runat="server" CssClass="full-width-textbox"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td>
                    <asp:Button ID="btn_Agregar" runat="server"
    Text="AGREGAR"
    OnClientClick="return confirm('¿Deseas agregar esta categoría?') && validarCamposTabla('tablaCategorias','txt_Id');"
    OnClick="btn_Agregar_Click"
    class="btn btn-exito" />
                </td>
                <td>
                    <asp:Button ID="btn_Modificar" runat="server"
    Text="MODIFICAR"
    OnClientClick="return confirm('¿Deseas modificar esta categoría?') && validarCamposTabla('tablaCategorias','txt_Id');"
    OnClick="btn_Modificar_Click"
    class="btn btn-primario" />
                </td>
                <td>
                    <asp:Button ID="btn_Limpiar" runat="server" Text="LIMPIAR" OnClick="btn_Limpiar_Click" class="btn btn-advertencia" />
                </td>
                <td>
                    <asp:Button ID="btn_Eliminar" runat="server"
    Text="ELIMINAR"
    OnClientClick="return confirm('¿Deseas eliminar esta categoría?');"
    OnClick="btn_Eliminar_Click"
    class="btn btn-peligro" />
                </td>
            </tr>
        </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

<div style="width: 40%; margin: 20px auto; text-align: center;">
    <div style="display: flex; justify-content: center; gap: 10px;">
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="full-width-textbox" placeholder="Buscar por nombre..." />
        <asp:Button ID="btnFiltrar" runat="server" Text="FILTRAR" CssClass="btn btn-info" OnClick="btnFiltrar_Click" />
    </div>
    <div style="margin-top: 10px;">
        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Font-Bold="true" />
    </div>
</div>

<asp:GridView ID="gvCategorias" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="7"
    OnPageIndexChanging="gvCategorias_PageIndexChanging"
    OnRowCommand="gvCategorias_RowCommand"
    CssClass="gridview-style">
    <Columns>
        <asp:BoundField DataField="IdCategoria" HeaderText="ID" />
        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CommandName="Consultar"
                    CommandArgument='<%# Eval("IdCategoria") %>' CssClass="btn btn-info btn-sm" />
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar"
                    CommandArgument='<%# Eval("IdCategoria") %>' CssClass="btn btn-peligro btn-sm"
                    OnClientClick="return confirm('¿Deseas eliminar esta categoría?');" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Content>
