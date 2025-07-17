using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Class;

namespace WebApplication1
{
    public partial class frmProductos : System.Web.UI.Page
    {
        //string connectionString = "Data Source=.;Initial Catalog=Pachacamac;User ID=sa;Password=123456;";
        string connectionString = ConfigurationManager.ConnectionStrings["DBAlmacenConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdRol"] == null || ((int)Session["IdRol"] != 1 && (int)Session["IdRol"] != 2))
            {
                Response.Redirect("NoAutorizado.aspx");
                return;
            }
            if (!IsPostBack)
            {
                CargarGrid();
                this.btn_Modificar.Enabled = false;
                this.btn_Eliminar.Enabled = false;

                clsCategoria categoria = new clsCategoria();
                var lista = categoria.ListarCategorias(connectionString);

                Ddl_Categoria.DataSource = lista;
                Ddl_Categoria.DataTextField = "Nombre";
                Ddl_Categoria.DataValueField = "IdCategoria";
                Ddl_Categoria.DataBind();
                Ddl_Categoria.Items.Insert(0, new ListItem("-- Seleccione una Categoria --", ""));

                clsUnidadMedida unidad = new clsUnidadMedida();
                var lista2 = unidad.ListarUnidades(connectionString);

                Ddl_Unidad.DataSource = lista2;
                Ddl_Unidad.DataTextField = "Nombre";
                Ddl_Unidad.DataValueField = "IdUnidad";
                Ddl_Unidad.DataBind();
                Ddl_Unidad.Items.Insert(0, new ListItem("-- Seleccione una Unidad de Medida --", ""));

                clsProveedor proveedor = new clsProveedor();
                var lista3 = proveedor.ListarProveedores(connectionString);

                this.Ddl_Proveedor.DataSource = lista3;
                this.Ddl_Proveedor.DataTextField = "RazonSocial";
                this.Ddl_Proveedor.DataValueField = "IdProveedor";
                this.Ddl_Proveedor.DataBind();
                Ddl_Proveedor.Items.Insert(0, new ListItem("-- Seleccione un Proveedor --", ""));

                if (Session["IdRol"] != null && (int)Session["IdRol"] == 2)
                {
                    btn_Agregar.Enabled = false;
                    btn_Eliminar.Enabled = false;
                    btn_Modificar.Enabled = false;
                    btn_Limpiar.Enabled = false;
                }
            }
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear el producto con referencias
                clsProducto p = new clsProducto
                {
                    Codigo = txt_Codigo.Text.Trim(),
                    Nombre = txt_Nombre.Text.Trim(),
                    Descripcion = txt_Descripcion.Text.Trim(),
                    FechaVencimiento = DateTime.Parse(txt_FechaVencimiento.Text),
                    Costo = decimal.Parse(txt_Costo.Text),

                    IdCategoria = new clsReferencia
                    {
                        Id = int.Parse(Ddl_Categoria.SelectedValue),
                        Nombre = Ddl_Categoria.SelectedItem.Text
                    },
                    IdUnidad = new clsReferencia
                    {
                        Id = int.Parse(Ddl_Unidad.SelectedValue),
                        Nombre = Ddl_Unidad.SelectedItem.Text
                    },
                    IdProveedor = new clsReferencia
                    {
                        Id = int.Parse(Ddl_Proveedor.SelectedValue),
                        Nombre = Ddl_Proveedor.SelectedItem.Text
                    },

                    StockMinimo = int.Parse(txt_StockMinimo.Text),
                    StockMaximo = int.Parse(txt_StockMaximo.Text),
                    Descontinuado = chb_Descontinuado.Checked
                };

                // Agregar a la base de datos
                p.Agregar(connectionString);

                // Limpiar controles y actualizar vista
                Limpiar();
                CargarGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Producto agregado correctamente.');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al agregar un producto.');", true);
                // Puedes registrar el error si lo deseas: lblMensaje.Text = "Error: " + ex.Message;
            }
        }


        protected void btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el campo 
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    //lblMensaje.Text = "Debe ingresar el id para modificar.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar el id para modificar.');", true);
                    return;
                }

                // Crear instancia del usuario con los datos del formulario
                clsProducto p = new clsProducto
                {
                    IdProducto = int.Parse(txt_Id.Text),
                    Codigo = txt_Codigo.Text.Trim(),
                    Nombre = txt_Nombre.Text.Trim(),
                    Descripcion = txt_Descripcion.Text.Trim(),
                    FechaVencimiento = DateTime.Parse(txt_FechaVencimiento.Text),
                    Costo = decimal.Parse(txt_Costo.Text),

                    IdCategoria = new clsReferencia
                    {
                        Id = int.Parse(Ddl_Categoria.SelectedValue),
                        Nombre = Ddl_Categoria.SelectedItem.Text
                    },
                    IdUnidad = new clsReferencia
                    {
                        Id = int.Parse(Ddl_Unidad.SelectedValue),
                        Nombre = Ddl_Unidad.SelectedItem.Text
                    },
                    IdProveedor = new clsReferencia
                    {
                        Id = int.Parse(Ddl_Proveedor.SelectedValue),
                        Nombre = Ddl_Proveedor.SelectedItem.Text
                    },

                    StockMinimo = int.Parse(txt_StockMinimo.Text),
                    StockMaximo = int.Parse(txt_StockMaximo.Text),
                    Descontinuado = chb_Descontinuado.Checked
                };

                // Llamar al método Modificar de la clase
                p.Modificar(connectionString);

                // Limpiar formulario
                Limpiar();
                CargarGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Producto modificado correctamente.');", true);
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al modificar un producto:');", true);
                //lblMensaje.Text = "Error al modificar un producto: " + ex.Message;
            }
        }

        protected void btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el ID esté presente
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    //lblMensaje.Text = "Debe seleccionar un producto para eliminar.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe seleccionar un producto para eliminar.');", true);
                    return;
                }

                // Crear instancia con solo el ID
                clsProducto p = new clsProducto
                {
                    IdProducto = int.Parse(txt_Id.Text)
                };

                // Llamar al método Eliminar
                p.Eliminar(connectionString);

                // Limpiar formulario
                this.Limpiar();
                CargarGrid();
                //lblMensaje.Text = "Producto eliminado correctamente.";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Producto eliminado correctamente.');", true);
            }
            catch (Exception ex)
            {

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al eliminar un Producto:');", true);
                //lblMensaje.Text = "Error al eliminar un Producto: " + ex.Message;
            }
        }

        protected void btn_Limpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            btn_Eliminar.Enabled = true;
            btn_Modificar.Enabled = true;
            btn_Agregar.Enabled = false;

            try
            {
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ingrese el ID del producto para buscar.');", true);
                    return;
                }

                int id = int.Parse(txt_Id.Text);
                clsProducto p = new clsProducto();
                bool encontrado = p.Buscar(connectionString, id);

                if (encontrado)
                {
                    txt_Codigo.Text = p.Codigo;
                    txt_Nombre.Text = p.Nombre;
                    txt_Descripcion.Text = p.Descripcion;

                    txt_FechaVencimiento.Text = p.FechaVencimiento != DateTime.MinValue
                        ? p.FechaVencimiento.ToString("yyyy-MM-dd")
                        : string.Empty;

                    txt_Costo.Text = p.Costo.ToString();

                    // Asignar valores a los DropDownList
                    Ddl_Categoria.SelectedValue = p.IdCategoria.Id.ToString();
                    Ddl_Unidad.SelectedValue = p.IdUnidad.Id.ToString();
                    Ddl_Proveedor.SelectedValue = p.IdProveedor.Id.ToString();

                    txt_StockMinimo.Text = p.StockMinimo.ToString();
                    txt_StockMaximo.Text = p.StockMaximo.ToString();
                    chb_Descontinuado.Checked = p.Descontinuado;

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Producto encontrado.');", true);
                }
                else
                {
                    Limpiar();
                    CargarGrid();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Producto no encontrado.');", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error al buscar un producto: {ex.Message}');", true);
            }
        }

        private void CargarGrid(string filtro = "")
        {
            clsProducto producto = new clsProducto();
            List<clsProducto> lista = producto.ListarProductos(connectionString);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                lista = lista.Where(c => c.Nombre.ToLower().Contains(filtro.ToLower())).ToList();
            }

            gvProductos.DataSource = lista;
            gvProductos.DataBind();
        }


        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            clsProducto p = new clsProducto();
            var lista = p.ListarProductos(connectionString);

            if (!string.IsNullOrEmpty(filtro))
            {
                lista = lista.Where(x => x.Nombre.ToLower().Contains(filtro)).ToList();
            }

            lblMensaje.Text = lista.Count == 0 ? "No se encontraron resultados para el filtro ingresado.": "";

            gvProductos.DataSource = lista;
            gvProductos.DataBind();
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            clsProducto producto = new clsProducto();

            if (e.CommandName == "Consultar")
            {
                if (producto.Buscar(connectionString, id))
                {
                    txt_Id.Text = producto.IdProducto.ToString();
                    txt_Codigo.Text = producto.Codigo;
                    txt_Nombre.Text = producto.Nombre;
                    txt_Descripcion.Text = producto.Descripcion;
                    txt_FechaVencimiento.Text = producto.FechaVencimiento.ToString("yyyy-MM-dd");
                    txt_Costo.Text = producto.Costo.ToString();

                    Ddl_Categoria.SelectedValue = producto.IdCategoria.Id.ToString();
                    Ddl_Unidad.SelectedValue = producto.IdUnidad.Id.ToString();
                    Ddl_Proveedor.SelectedValue = producto.IdProveedor.Id.ToString();

                    txt_StockMinimo.Text = producto.StockMinimo.ToString();
                    txt_StockMaximo.Text = producto.StockMaximo.ToString();
                    chb_Descontinuado.Checked = producto.Descontinuado;

                    btn_Agregar.Enabled = false;
                    btn_Modificar.Enabled = true;
                    btn_Eliminar.Enabled = true;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Producto no encontrado.');", true);
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                producto.IdProducto = id;
                producto.Eliminar(connectionString);
                CargarGrid();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Producto eliminado correctamente.');", true);
            }
        }

        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            string filtro = txtBuscar.Text.Trim();
            CargarGrid(filtro);
        }

        protected void gvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["IdRol"] != null && (int)Session["IdRol"] == 2)
                {
                    Button btnEliminar = (Button)e.Row.FindControl("btnEliminar");
                    if (btnEliminar != null)
                        btnEliminar.Enabled = false;

                    Button btnConsultar = (Button)e.Row.FindControl("btnConsultar");
                    if (btnConsultar != null)
                        btnConsultar.Enabled = false;
                }
            }
        }

        private void Limpiar()
        {
            this.btn_Agregar.Enabled = true;
            this.btn_Modificar.Enabled = false;
            this.btn_Eliminar.Enabled = false;

            this.txt_Id.Text = string.Empty;
            this.txt_Codigo.Text = string.Empty ;
            this.txt_Nombre.Text = string .Empty ;
            this.txt_Descripcion.Text = string.Empty;
            this.txt_FechaVencimiento.Text = DateTime.Now.ToString();
            this.txt_Costo.Text = string.Empty;
            this.Ddl_Categoria.SelectedIndex = 0;
            this.Ddl_Unidad.SelectedIndex = 0;
            this.Ddl_Proveedor.SelectedIndex = 0;
            this.txt_StockMinimo.Text = string.Empty;
            this.txt_StockMaximo.Text = string.Empty;
            this.chb_Descontinuado.Checked = false;
        }
    }
}