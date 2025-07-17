<%@ Page Title="" Language="C#" MasterPageFile="~/TemplatePrincipal.Master" AutoEventWireup="true" CodeBehind="frmProductos.aspx.cs" Inherits="WebApplication1.frmProductos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="tablaProductos" class="tablaForm" >
            <tr class="titulo">
                <td colspan="4">
                    <asp:Label ID="Label2" runat="server" Text="Productos"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="ID:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_Id" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td></td>
                <td>
                    <asp:Button ID="btn_Buscar" runat="server" OnClick="btn_Buscar_Click" Text="BUSCAR" class="btn btn-info" visible="false"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Codigo:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_Codigo" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label11" runat="server" Text="Nombre:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txt_Nombre" runat="server" CssClass="full-width-textbox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Text="Descripcion:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txt_Descripcion" runat="server" CssClass="full-width-textbox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label15" runat="server" Text="Fecha de Vencimiento:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_FechaVencimiento" runat="server" CssClass="full-width-textbox" TextMode="Date"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Costo:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_Costo" runat="server" onkeypress="return soloDecimales(event, this);"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Categoria:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="Ddl_Categoria" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Unidad:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="Ddl_Unidad" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label13" runat="server" Text="Proveedor:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="Ddl_Proveedor" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Stock Minimo:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_StockMinimo" runat="server" TextMode="Number"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Stock Maximo:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_StockMaximo" runat="server" TextMode="Number"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Checkbox ID="chb_Descontinuado" runat="server" Text="Descontinuado"></asp:Checkbox>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btn_Agregar" runat="server" Text="AGREGAR" 
                        OnClientClick="return confirm('¿Deseas agregar esta categoría?') && validarCamposTabla('tablaProductos','txt_Id,txt_Descripcion');" 
                        OnClick="btn_Agregar_Click" class="btn btn-exito" />
                </td>
                <td>
                    <asp:Button ID="btn_Modificar" runat="server" Text="MODIFICAR" 
                        OnClientClick="return confirm('¿Deseas modificar esta categoría?') && validarCamposTabla('tablaProductos','txt_Id,txt_Descripcion');" 
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
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="full-width-textbox" placeholder="Buscar por nombre de producto..." />
        <asp:Button ID="btnFiltrar" runat="server" Text="FILTRAR" CssClass="btn btn-info" OnClick="btnFiltrar_Click" />
    </div>
    <div style="margin-top: 10px;">
        <asp:Label ID="lblMensaje" runat="server" ForeColor="White" Font-Bold="true" />
    </div>
</div>
<div style="max-height: 300px; overflow: auto;">
    <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="3"
        OnPageIndexChanging="gvProductos_PageIndexChanging"
        OnRowCommand="gvProductos_RowCommand"
        OnRowDataBound="gvProductos_RowDataBound"
        CssClass="gridview-style">
        <Columns>
            <asp:BoundField DataField="IdProducto" HeaderText="ID" />
            <asp:BoundField DataField="Codigo" HeaderText="Código" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
            <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vencimiento" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:C}" />
        
            <asp:BoundField DataField="IdCategoria.Nombre" HeaderText="Categoría" />
            <asp:BoundField DataField="IdUnidad.Nombre" HeaderText="Unidad de Medida" />
            <asp:BoundField DataField="IdProveedor.Nombre" HeaderText="Proveedor" />

            <asp:BoundField DataField="StockMinimo" HeaderText="Stock Mínimo" />
            <asp:BoundField DataField="StockMaximo" HeaderText="Stock Máximo" />
            <asp:BoundField DataField="Descontinuado" HeaderText="Descontinuado" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CommandName="Consultar"
                        CommandArgument='<%# Eval("IdProducto") %>' CssClass="btn btn-info btn-sm" />
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar"
                        CommandArgument='<%# Eval("IdProducto") %>' CssClass="btn btn-peligro btn-sm"
                        OnClientClick="return confirm('¿Deseas eliminar este producto?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>

</asp:Content>
