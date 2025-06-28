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
    public partial class frmUsuarios : System.Web.UI.Page
    {
        //string connectionString = "Data Source=.;Initial Catalog=Pachacamac;User ID=sa;Password=123456;";
        string connectionString = ConfigurationManager.ConnectionStrings["DBAlmacenConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrid();
                this.btn_Modificar.Enabled = false;
                this.btn_Eliminar.Enabled = false;

                clsRol rol = new clsRol();
                var lista = rol.ListarRoles(connectionString);

                Ddl_Rol.DataSource = lista;
                Ddl_Rol.DataTextField = "NombreRol";
                Ddl_Rol.DataValueField = "IdRol";
                Ddl_Rol.DataBind();
                Ddl_Rol.Items.Insert(0, new ListItem("-- Seleccione un Rol --", ""));
            }
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.Equals(txt_Contrasenia.Text.Trim(), txt_Contrasenia2.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Las contraseñas deben ser iguales.');", true);
                    return;
                }

                // Obtener el usuario desde los controles
                clsUsuario u = new clsUsuario
                {
                    //IdUsuario = int.Parse( txt_IdUsuario.Text),
                    NombreUsuario = txt_NombreUsuario.Text.Trim(),
                    Nombres = txt_Nombres.Text.Trim(),
                    Apellidos = txt_Apellidos.Text.Trim(),
                    Contrasenia= txt_Contrasenia.Text.Trim(),

                    IdRol = new clsReferencia
                    {
                        Id = int.Parse(Ddl_Rol.SelectedValue),
                        Nombre = Ddl_Rol.SelectedItem.Text
                    },
                    Estado = chb_Estado.Checked
                };

                // Agregar a la base de datos
                u.Agregar(connectionString);

                // Limpiar controles
                Limpiar();
                CargarGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Usuario agregado correctamente. ');", true);
            }
            catch (Exception ex)
            {
                //lblMensaje.Text = "Error al agregar el usuario: " + ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al agregar el usuario.');", true);
            }
        }

        protected void btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.Equals(txt_Contrasenia.Text.Trim(), txt_Contrasenia2.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Las contraseñas deben ser iguales.');", true);
                    return;
                }
                // Crear instancia del usuario con los datos del formulario
                clsUsuario u = new clsUsuario
                {
                    IdUsuario =int.Parse(txt_IdUsuario.Text),
                    NombreUsuario = txt_NombreUsuario.Text.Trim(),
                    Nombres = txt_Nombres.Text.Trim(),
                    Apellidos = txt_Apellidos.Text.Trim(),
                    // Contrasenia = txt_Contrasenia.Text.Trim(),
                    Contrasenia = string.IsNullOrWhiteSpace(txt_Contrasenia.Text) ? null : txt_Contrasenia.Text.Trim(),
                    IdRol = new clsReferencia
                    {
                        Id = int.Parse(Ddl_Rol.SelectedValue),
                        Nombre = Ddl_Rol.SelectedItem.Text
                    },
                    Estado = chb_Estado.Checked
                };

                // Llamar al método Modificar de la clase
                u.Modificar(connectionString);

                // Limpiar formulario
                Limpiar();
                CargarGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Usuario modificado correctamente.');", true);
            }
            catch (Exception ex)
            {
                // ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al modificar el usuario');", true);
                   ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al modificar el usuario: " + ex.Message.Replace("'", "") + "');", true);
            }
        }

        protected void btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que el campo Usuario no esté vacío
                if (string.IsNullOrWhiteSpace(txt_IdUsuario.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Debe ingresar el nombre de usuario para eliminar.');", true);

                    return;
                }

                // Crear instancia del usuario con solo el nombre de usuario
                clsUsuario u = new clsUsuario
                {
                    IdUsuario =int.Parse(txt_IdUsuario.Text)
                };

                // Llamar al método Eliminar
                u.Eliminar(connectionString);

                // Limpiar formulario
                Limpiar();
                CargarGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Usuario eliminado correctamente.');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al eliminar el usuario');", true);
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
                // Validar que el campo Usuario no esté vacío
                if (string.IsNullOrWhiteSpace(txt_IdUsuario.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ingrese el nombre de usuario para buscar.');", true);
                    return;
                }

                int id = int.Parse(txt_IdUsuario.Text);
                clsUsuario u = new clsUsuario();
                bool encontrado = u.Buscar(connectionString, id);

                if (encontrado)
                {
                    txt_NombreUsuario.Text = u.NombreUsuario;
                    txt_Nombres.Text = u.Nombres;
                    txt_Apellidos.Text = u.Apellidos;
                    txt_Contrasenia.Text = u.Contrasenia;
                    Ddl_Rol.SelectedValue = u.IdRol.Id.ToString();
                    chb_Estado.Checked = u.Estado;

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Usuario encontrado.');", true);
                }
                else
                {
                    Limpiar();
                    CargarGrid();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Usuario no encontrado.');", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al buscar el usuario');", true);
            }
        }

        private void CargarGrid(string filtro = "")
        {
            clsUsuario user = new clsUsuario();
            List<clsUsuario> lista = user.ListarUsuarios(connectionString);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                lista = lista.Where(c => c.NombreUsuario.ToLower().Contains(filtro.ToLower())).ToList();
            }

            gvUsuarios.DataSource = lista;
            gvUsuarios.DataBind();
        }


        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            clsUsuario us = new clsUsuario();
            var lista = us.ListarUsuarios(connectionString);

            if (!string.IsNullOrEmpty(filtro))
            {
                lista = lista.Where(x => x.NombreUsuario.ToLower().Contains(filtro)).ToList();
            }

            lblMensaje.Text = lista.Count == 0 ? "No se encontraron resultados para el filtro ingresado." : "";

            gvUsuarios.DataSource = lista;
            gvUsuarios.DataBind();
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            clsUsuario user = new clsUsuario();

            if (e.CommandName == "Consultar")
            {
                if (user.Buscar(connectionString, id))
                {
                    txt_IdUsuario.Text = user.IdUsuario.ToString();
                    txt_NombreUsuario.Text = user.NombreUsuario;
                    txt_Nombres.Text = user.Nombres;
                    txt_Apellidos.Text = user.Apellidos;
                    txt_Contrasenia.Text = user.Contrasenia;

                    Ddl_Rol.SelectedValue = user.IdRol.Id.ToString();

                    chb_Estado.Checked = user.Estado;

                    btn_Agregar.Enabled = false;
                    btn_Modificar.Enabled = true;
                    btn_Eliminar.Enabled = true;
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Usuario no encontrado.');", true);
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                user.IdUsuario = id;
                user.Eliminar(connectionString);
                CargarGrid();

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Producto eliminado correctamente.');", true);
            }
        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;
            string filtro = txtBuscar.Text.Trim();
            CargarGrid(filtro);
        }

        private void Limpiar()
        {
            this.btn_Agregar.Enabled = true;
            this.btn_Modificar.Enabled = false;
            this.btn_Eliminar.Enabled = false;

            txt_IdUsuario.Text = string.Empty;
            txt_NombreUsuario.Text = string.Empty;
            txt_Nombres.Text = string.Empty;
            txt_Apellidos.Text = string.Empty;
            txt_Contrasenia.Text = string.Empty;
            Ddl_Rol.SelectedIndex = 0;
            chb_Estado.Checked = false;
        }   
    }
}