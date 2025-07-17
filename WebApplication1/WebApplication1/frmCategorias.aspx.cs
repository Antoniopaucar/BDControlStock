using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WebApplication1.Class;

namespace WebApplication1
{
    public partial class frmCategorias : System.Web.UI.Page
    {
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
                // Obtener la categoría desde los controles
                clsCategoria c = new clsCategoria
                {
                    Nombre = this.txt_Nombre.Text,
                    Descripcion = this.txt_Descripcion.Text,
                };

                // Agregar a la base de datos
                c.Agregar(connectionString);

                // Limpiar controles
                this.Limpiar();
                this.CargarGrid();


                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría agregada correctamente.');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al agregar la categoría');", true);
            }
        }

        protected void btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el ID no esté vacío
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe seleccionar una categoría para modificar.');", true);
                    return;
                }

                // Crear la instancia con los datos del formulario
                clsCategoria c = new clsCategoria
                {
                    IdCategoria = int.Parse(txt_Id.Text),
                    Nombre = txt_Nombre.Text,
                    Descripcion = txt_Descripcion.Text,
                };

                // Llamar al método Modificar
                c.Modificar(connectionString);

                // Limpiar formulario
                this.Limpiar();
                this.CargarGrid();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría modificada correctamente.');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al modificar la categoría');", true);
            }
        }

        protected void btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el ID esté presente
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe seleccionar una categoría para eliminar.');", true);
                    return;
                }

                // Crear instancia con solo el ID
                clsCategoria c = new clsCategoria
                {
                    IdCategoria = int.Parse(txt_Id.Text)
                };

                // Llamar al método Eliminar
                c.Eliminar(connectionString);

                // Limpiar formulario
                this.Limpiar();
                this.CargarGrid();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría eliminada correctamente.');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al eliminar la categoría');", true);
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
                // Validar que el ID esté presente
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ingrese un ID para buscar.');", true);
                    return;
                }

                int id = int.Parse(txt_Id.Text);
                clsCategoria c = new clsCategoria();

                // Buscar la categoría
                bool encontrada = c.Buscar(connectionString, id);

                if (encontrada)
                {
                    // Mostrar los datos en los controles
                    txt_Nombre.Text = c.Nombre;
                    txt_Descripcion.Text = c.Descripcion;

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría encontrada.');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría no encontrada.');", true);
                    this.Limpiar();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al buscar la categoría.');", true);

            }
        }

        private void CargarGrid(string filtro = "")
        {
            clsCategoria categoria = new clsCategoria();
            List<clsCategoria> lista = categoria.ListarCategorias(connectionString);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                lista = lista.Where(c => c.Nombre.ToLower().Contains(filtro.ToLower())).ToList();
            }

            gvCategorias.DataSource = lista;
            gvCategorias.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            clsCategoria c = new clsCategoria();
            var lista = c.ListarCategorias(connectionString);

            if (!string.IsNullOrEmpty(filtro))
            {
                lista = lista.Where(x => x.Nombre.ToLower().Contains(filtro)).ToList();
            }

            if (lista.Count == 0)
            {
                lblMensaje.Text = "No se encontraron resultados para el filtro ingresado.";
            }
            else
            {
                lblMensaje.Text = "";
            }

            gvCategorias.DataSource = lista;
            gvCategorias.DataBind();
        }

        protected void gvCategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            clsCategoria categoria = new clsCategoria();

            if (e.CommandName == "Consultar")
            {
                if (categoria.Buscar(connectionString, id))
                {
                    txt_Id.Text = categoria.IdCategoria.ToString();
                    txt_Nombre.Text = categoria.Nombre;
                    txt_Descripcion.Text = categoria.Descripcion;

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
                categoria.IdCategoria = id;
                categoria.Eliminar(connectionString);
                CargarGrid(); // recargar la grilla

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría eliminada correctamente.');", true);
            }
        }
        protected void gvCategorias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategorias.PageIndex = e.NewPageIndex;

            // Si quieres mantener el filtro al cambiar de página
            string filtro = txtBuscar.Text.Trim();
            CargarGrid(filtro);
        }

        protected void gvCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
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
            this.btn_Agregar.Enabled=true;
            this.btn_Modificar.Enabled=false;
            this.btn_Eliminar.Enabled=false;

            this.txt_Id.Text = string.Empty;
            this.txt_Nombre.Text = string.Empty;
            this.txt_Descripcion.Text = string.Empty;
        }
    }
}