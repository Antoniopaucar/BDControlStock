using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class MenuPrincipañ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblusuario.Text = Session["Usuario"].ToString();
        }



        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var datos = new HttpCookie("Usuario", Session["Usuario"].ToString());
            datos.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(datos);
        }
    }
}