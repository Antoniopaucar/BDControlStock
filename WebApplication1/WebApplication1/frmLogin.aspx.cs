using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using WebApplication1.Class;
using System.Configuration;

namespace WebApplication1
{
    public partial class frmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBAlmacenConnection"].ConnectionString;
          
            clsUsuario user = new clsUsuario
            {
                NombreUsuario = txtUsuario.Text.Trim(),
                Contrasenia = txtClave.Text.Trim()
            };

            if (user.IniciarSesion(connectionString))
            {
                Session["IdUser"] = user.IdUsuario;
                Session["Usuario"] = user.Nombres + " " + user.Apellidos;
                Session["IdRol"] = user.IdRol.Id;
                Response.Redirect("frmInicio.aspx");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error en el usuario o clave.');", true);
            }

        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            string script = "window.location.href = 'https://www.google.com';";
            ClientScript.RegisterStartupScript(this.GetType(), "IrAGoogle", script, true);
        }

    }
}