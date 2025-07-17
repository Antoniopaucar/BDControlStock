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
    public partial class frmProveedores : System.Web.UI.Page
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
            }
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener el usuario desde los controles
                clsProveedor p = new clsProveedor
                {
                    RazonSocial = txt_RazonSocial.Text.Trim(),
                    Representante = txt_Representante.Text.Trim(),
                    RUC= txt_Ruc.Text.Trim(),
                    Direccion = txt_Direccion.Text.Trim(),
                    Telefono = txt_Telefono.Text.Trim(),
                    Email = txt_Email.Text.Trim(),
                };

                // Agregar a la base de datos
                p.Agregar(connectionString);

                // Limpiar controles
                Limpiar();
                CargarGrid();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Proveedor agregado correctamente.');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al agregar un proveedor');", true);

            }
        }

        protected void btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el campo 
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar el id para modificar.');", true);
                    return;
                }

                // Crear instancia del usuario con los datos del formulario
                clsProveedor p = new clsProveedor
                {
                    IdProveedor = int.Parse(txt_Id.Text),
                    RazonSocial = txt_RazonSocial.Text.Trim(),
                    Representante = txt_Representante.Text.Trim(),
                    RUC = txt_Ruc.Text.Trim(),
                    Direccion = txt_Direccion.Text.Trim(),
                    Telefono = txt_Telefono.Text.Trim(),
                    Email = txt_Email.Text.Trim(),
                };

                // Llamar al método Modificar de la clase
                p.Modificar(connectionString);

                // Limpiar formulario
                Limpiar();
                CargarGrid();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Proveedor modificado correctamente.');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al modificar un proveedor');", true);

            }
        }

        protected void btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el ID esté presente
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe seleccionar un proveedor para eliminar.');", true);

                    return;
                }

                // Crear instancia con solo el ID
                clsProveedor p = new clsProveedor
                {
                    IdProveedor = int.Parse(txt_Id.Text)
                };

                // Llamar al método Eliminar
                p.Eliminar(connectionString);

                // Limpiar formulario
                this.Limpiar();
                CargarGrid();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Proveedor eliminado correctamente.');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al eliminar un Proveedor');", true);

            }
        }

        protected void btn_Limpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            this.btn_Eliminar.Enabled = true;
            this.btn_Modificar.Enabled = true;
            this.btn_Agregar.Enabled = false;

            try
            {
                // Validar que el campo
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ingrese el id del proveedor para buscar.');", true);

                    return;
                }

                int id = int.Parse(txt_Id.Text);
                clsProveedor p = new clsProveedor();
                bool encontrado = p.Buscar(connectionString, id);

                if (encontrado)
                {
                    this.txt_RazonSocial.Text = p.RazonSocial;
                    this.txt_Representante.Text = p.Representante;
                    this.txt_Ruc.Text = p.RUC;
                    this.txt_Direccion.Text = p.Direccion;
                    this.txt_Telefono.Text = p.Telefono;
                    this.txt_Email.Text = p.Email;

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Proveedor encontrado.');", true);

                }
                else
                {
                    Limpiar();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Proveedor no encontrado.');", true);

                }
            }
            catch (Exception ex)
            {

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al buscar un proveedor');", true);

            }
        }

        private void CargarGrid(string filtro = "")
        {
            clsProveedor proveedor = new clsProveedor();
            List<clsProveedor> lista = proveedor.ListarProveedores(connectionString);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                lista = lista.Where(c => c.RazonSocial.ToLower().Contains(filtro.ToLower())).ToList();
            }

            gvProveedores.DataSource = lista;
            gvProveedores.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            clsProveedor p = new clsProveedor();
            var lista = p.ListarProveedores(connectionString);

            if (!string.IsNullOrEmpty(filtro))
            {
                lista = lista.Where(x => x.RazonSocial.ToLower().Contains(filtro)).ToList();
            }

            if (lista.Count == 0)
            {
                lblMensaje.Text = "No se encontraron resultados para el filtro ingresado.";
            }
            else
            {
                lblMensaje.Text = "";
            }

            gvProveedores.DataSource = lista;
            gvProveedores.DataBind();
        }



        protected void gvProveedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            clsProveedor proveedor = new clsProveedor();

            if (e.CommandName == "Consultar")
            {
                if (proveedor.Buscar(connectionString, id))
                {
                    txt_Id.Text = proveedor.IdProveedor.ToString();
                    txt_RazonSocial.Text = proveedor.RazonSocial;
                    txt_Representante.Text = proveedor.Representante;
                    txt_Ruc.Text = proveedor.RUC;
                    txt_Direccion.Text = proveedor.Direccion;
                    txt_Telefono.Text = proveedor.Telefono;
                    txt_Email.Text = proveedor.Email;

                    btn_Agregar.Enabled = false;
                    btn_Modificar.Enabled = true;
                    btn_Eliminar.Enabled = true;

                    //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría consultada.');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría no encontrada.');", true);
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                proveedor.IdProveedor = id;
                proveedor.Eliminar(connectionString);
                CargarGrid(); // recargar la grilla

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría eliminada correctamente.');", true);
            }
        }
        protected void gvProveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProveedores.PageIndex = e.NewPageIndex;

            // Si quieres mantener el filtro al cambiar de página
            string filtro = txtBuscar.Text.Trim();
            CargarGrid(filtro);
        }

        private void Limpiar()
        {
            this.btn_Agregar.Enabled = true;
            this.btn_Modificar.Enabled = false;
            this.btn_Eliminar.Enabled = false;

            this.txt_Id.Text =string.Empty;
            this.txt_RazonSocial.Text = string.Empty;
            this.txt_Representante.Text = string.Empty;
            this.txt_Ruc.Text = string.Empty;
            this.txt_Direccion.Text = string.Empty;
            this.txt_Telefono.Text = string.Empty ;
            this.txt_Email.Text = string.Empty ;

        }
    }
}