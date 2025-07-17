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
    public partial class frmUnidadMedida : System.Web.UI.Page
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
                // Obtener la desde los controles
                clsUnidadMedida um = new clsUnidadMedida
                {
                    Nombre = this.txt_Nombre.Text,
                };

                // Agregar a la base de datos
                um.Agregar(connectionString);

                // Limpiar controles
                this.Limpiar();
                CargarGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Unidad de Medida agregada correctamente.');", true);

            }
            catch (Exception ex)
            {

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al agregar la Unidad de Medida');", true);

            }
        }

        protected void btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el ID no esté vacío
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe seleccionar un id para modificar.');", true);

                    return;
                }

                // Crear la instancia con los datos del formulario
                clsUnidadMedida um = new clsUnidadMedida
                {
                    IdUnidad = int.Parse(txt_Id.Text),
                    Nombre = txt_Nombre.Text,
                };

                // Llamar al método Modificar
                um.Modificar(connectionString);

                // Limpiar formulario
                this.Limpiar();
                CargarGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Unidad de Medida modificada correctamente.');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al modificar la Unidad de Medida');", true);

            }
        }

        protected void btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el ID esté presente
                if (string.IsNullOrWhiteSpace(txt_Id.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe seleccionar un id para eliminar.');", true);

                    return;
                }

                // Crear instancia con solo el ID
                clsUnidadMedida um = new clsUnidadMedida
                {
                    IdUnidad = int.Parse(txt_Id.Text)
                };

                // Llamar al método Eliminar
                um.Eliminar(connectionString);

                // Limpiar formulario
                this.Limpiar();
                CargarGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Unidad de Medida eliminada correctamente.');", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al eliminar la unidad de medida');", true);

            }
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
                clsUnidadMedida um = new clsUnidadMedida();

                // Buscar la categoría
                bool encontrada = um.Buscar(connectionString, id);

                if (encontrada)
                {
                    // Mostrar los datos en los controles
                    txt_Nombre.Text = um.Nombre;

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Unidad de medida encontrada.');", true);

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Unidad de medida no encontrada.');", true);

                    this.Limpiar();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al buscar la unidad de medida.');", true);

            }
        }
        protected void btn_Limpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void CargarGrid(string filtro = "")
        {
            clsUnidadMedida unidad = new clsUnidadMedida();
            List<clsUnidadMedida> lista = unidad.ListarUnidades(connectionString);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                lista = lista.Where(c => c.Nombre.ToLower().Contains(filtro.ToLower())).ToList();
            }

            gvUnidades.DataSource = lista;
            gvUnidades.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            clsUnidadMedida u = new clsUnidadMedida();
            var lista = u.ListarUnidades(connectionString);

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

            gvUnidades.DataSource = lista;
            gvUnidades.DataBind();
        }

        protected void gvUnidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            clsUnidadMedida unidad = new clsUnidadMedida();

            if (e.CommandName == "Consultar")
            {
                if (unidad.Buscar(connectionString, id))
                {
                    txt_Id.Text = unidad.IdUnidad.ToString();
                    txt_Nombre.Text = unidad.Nombre;

                    btn_Agregar.Enabled = false;
                    btn_Modificar.Enabled = true;
                    btn_Eliminar.Enabled = true;

                    //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Categoría consultada.');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Unidad de medida no encontrada.');", true);
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                unidad.IdUnidad = id;
                unidad.Eliminar(connectionString);
                CargarGrid(); // recargar la grilla

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Unidad de Medida eliminada correctamente.');", true);
            }
        }
        protected void gvUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUnidades.PageIndex = e.NewPageIndex;

            // Si quieres mantener el filtro al cambiar de página
            string filtro = txtBuscar.Text.Trim();
            CargarGrid(filtro);
        }

        protected void gvUnidadMedida_RowDataBound(object sender, GridViewRowEventArgs e)
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
            this.txt_Nombre.Text = string.Empty;
        }

    }
}