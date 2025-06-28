using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Class;

namespace WebApplication1
{
    public partial class frmMovimientoInventario : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBAlmacenConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.btn_Modificar.Enabled = false;
                this.btn_Eliminar.Enabled = false;

                clsUsuario user = new clsUsuario();
                var listaUser = user.ListarUsuarios(connectionString);

                Ddl_Usuario.DataSource = listaUser;
                Ddl_Usuario.DataTextField = "NombreUsuario";
                Ddl_Usuario.DataValueField = "IdUsuario";
                Ddl_Usuario.DataBind();
                Ddl_Usuario.SelectedValue = Session["IdUser"].ToString();

                clsTipoMovimiento tipoMov = new clsTipoMovimiento();
                var lista = tipoMov.ListarTiposMov(connectionString);

                Ddl_Movimiento.DataSource = lista;
                Ddl_Movimiento.DataTextField = "Nombre";
                Ddl_Movimiento.DataValueField = "Id";
                Ddl_Movimiento.DataBind();
                Ddl_Movimiento.Items.Insert(0, new ListItem("-- Seleccione un Tipo de Movimiento --", ""));

                // Inicializar subtipos vacíos
                Ddl_TipoMovimiento.Items.Clear();
                Ddl_TipoMovimiento.Items.Insert(0, new ListItem("-- Seleccione un Sub Tipo de Movimiento --", ""));

                clsProducto producto = new clsProducto();
                var lista3 = producto.ListarProductos(connectionString);

                this.ddlProducto.DataSource = lista3;
                this.ddlProducto.DataTextField = "Nombre";
                this.ddlProducto.DataValueField = "IdProducto";
                this.ddlProducto.DataBind();
                this.ddlProducto.Items.Insert(0, new ListItem("-- Seleccione un Producto --", ""));
            }
        }

        protected void Ddl_Movimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Ddl_Movimiento.SelectedValue))
            {
                int idTipoMov = Convert.ToInt32(Ddl_Movimiento.SelectedValue);

                clsDetTipoMovimiento detTipoMov = new clsDetTipoMovimiento();
                var lista2 = detTipoMov.ListarDetTiposMov(connectionString, idTipoMov);

                Ddl_TipoMovimiento.DataSource = lista2;
                Ddl_TipoMovimiento.DataTextField = "Nombre";
                Ddl_TipoMovimiento.DataValueField = "Id";
                Ddl_TipoMovimiento.DataBind();
                Ddl_TipoMovimiento.Items.Insert(0, new ListItem("-- Seleccione un Sub Tipo de Movimiento --", ""));
            }
            else
            {
                Ddl_TipoMovimiento.Items.Clear();
                Ddl_TipoMovimiento.Items.Insert(0, new ListItem("-- Seleccione un Sub Tipo de Movimiento --", ""));
            }
        }

        private List<clsDetalleMovimiento> Detalles
        {
            get
            {
                if (ViewState["Detalles"] == null)
                    ViewState["Detalles"] = new List<clsDetalleMovimiento>();
                return (List<clsDetalleMovimiento>)ViewState["Detalles"];
            }
            set
            {
                ViewState["Detalles"] = value;
            }
        }

        private void ActualizarGrilla()
        {
            var datos = Detalles.Select(x => new
            {
                IdProducto = x.IdProducto.Id,
                NombreProducto = x.IdProducto.Nombre,
                x.Cantidad,
                x.Costo
            }).ToList();

            gvDetalles.DataSource = datos;
            gvDetalles.DataBind();
        }

        protected void gvDetalles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarFila")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var lista = Detalles;
                lista.RemoveAt(index);
                Detalles = lista;
                ActualizarGrilla();
            }
        }

        protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProducto.SelectedIndex > 0)
            {
                int idProducto = int.Parse(ddlProducto.SelectedValue);
                clsProducto producto = new clsProducto();
                if (producto.Buscar(connectionString, idProducto))
                {
                    txtCosto.Text = producto.Costo.ToString("0.00");
                }
            }
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            if (Detalles.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "alert('Debe agregar al menos un producto en la grilla antes de guardar.');", true);
                return;
            }

            clsMovimiento mov = new clsMovimiento
            {
                Fecha = DateTime.Parse(txt_Fecha.Text),
                IdTipoMovimiento = new clsReferencia { Id = int.Parse(Ddl_Movimiento.SelectedValue) },
                IdDetalleTipoMovimiento = new clsReferencia { Id = int.Parse(Ddl_TipoMovimiento.SelectedValue) },
                Observaciones = this.txt_Observaciones.Text,
                IdUsuario = new clsReferencia { Id = int.Parse(Ddl_Usuario.SelectedValue) },
                Detalles = Detalles
            };

            mov.Agregar(connectionString);
            LimpiarFormulario();
        }

        protected void btn_Modificar_Click(object sender, EventArgs e)
        {
            if (Detalles.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "alert('Debe haber al menos un producto en la grilla para modificar el movimiento.');", true);
                return;
            }

            clsMovimiento mov = new clsMovimiento
            {
                IdMovimiento = int.Parse(txt_Id.Text),
                Fecha = DateTime.Parse(txt_Fecha.Text),
                IdTipoMovimiento = new clsReferencia { Id = int.Parse(Ddl_Movimiento.SelectedValue) },
                IdDetalleTipoMovimiento = new clsReferencia { Id = int.Parse(Ddl_TipoMovimiento.SelectedValue) },
                Observaciones = this.txt_Observaciones.Text,
                //IdUsuario = new clsReferencia { Id = int.Parse(Ddl_Usuario.SelectedValue) },
                IdUsuario = new clsReferencia {Id = int.Parse(Session["IdUser"].ToString()) },
                Detalles = Detalles
            };

            mov.Modificar(connectionString);
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txt_Id.Text = "";
            txt_Fecha.Text = "";
            Ddl_Movimiento.SelectedIndex = 0;
            Ddl_TipoMovimiento.Items.Clear();
            Ddl_TipoMovimiento.Items.Insert(0, new ListItem("-- Seleccione un Sub Tipo de Movimiento --", ""));
            txt_Observaciones.Text = "";
            Ddl_Usuario.SelectedValue = Session["IdUser"].ToString();
            ddlProducto.SelectedIndex = 0;
            txtCantidad.Text = "";
            txtCosto.Text = "";

            Detalles = new List<clsDetalleMovimiento>();
            ActualizarGrilla();

            btn_Agregar.Enabled = true;
            btn_Modificar.Enabled = false;
            btn_Eliminar.Enabled = false;
        }

        protected void btn_Limpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Buscar.Text))
            {
                int idBuscado;
                if (int.TryParse(txt_Buscar.Text, out idBuscado))
                {
                    clsMovimiento mov = new clsMovimiento();
                    if (mov.Buscar(connectionString, idBuscado))
                    {
                        // Rellenar campos
                        txt_Id.Text = mov.IdMovimiento.ToString();
                        txt_Fecha.Text = mov.Fecha.ToString("yyyy-MM-dd");
                        Ddl_Movimiento.SelectedValue = mov.IdTipoMovimiento.Id.ToString();

                        // Cargar subtipo
                        clsDetTipoMovimiento detTipoMov = new clsDetTipoMovimiento();
                        var lista2 = detTipoMov.ListarDetTiposMov(connectionString, mov.IdTipoMovimiento.Id);
                        Ddl_TipoMovimiento.DataSource = lista2;
                        Ddl_TipoMovimiento.DataTextField = "Nombre";
                        Ddl_TipoMovimiento.DataValueField = "Id";
                        Ddl_TipoMovimiento.DataBind();
                        Ddl_TipoMovimiento.SelectedValue = mov.IdDetalleTipoMovimiento.Id.ToString();

                        // Observaciones
                        txt_Observaciones.Text = mov.Observaciones;
                        Ddl_Usuario.SelectedValue = mov.IdUsuario.Id.ToString();

                        // Cargar detalles en ViewState y grilla
                        Detalles = mov.Detalles;
                        ActualizarGrilla();
                        txt_Buscar.Text = "";

                        // Ajustar botones
                        btn_Agregar.Enabled = false;
                        btn_Modificar.Enabled = true;
                        btn_Eliminar.Enabled = true;
                    }
                    else
                    {
                        // No se encontró
                        txt_Buscar.Text = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No se encontró el movimiento con ese Número.');", true);
                    }
                }
                else
                {
                    txt_Buscar.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Ingrese un número válido.');", true);
                }
            }
        }

        protected void btnAgregarFila_Click(object sender, EventArgs e)
        {
            if (ddlProducto.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtCantidad.Text) && !string.IsNullOrWhiteSpace(txtCosto.Text))
            {
                int idProducto = int.Parse(ddlProducto.SelectedValue);
                clsProducto producto = new clsProducto();
                producto.Buscar(connectionString, idProducto);

                clsDetalleMovimiento nuevo = new clsDetalleMovimiento
                {
                    IdProducto = new clsReferencia
                    {
                        Id = idProducto,
                        Nombre = producto.Nombre
                    },
                    Cantidad = int.Parse(txtCantidad.Text),
                    Costo = decimal.Parse(txtCosto.Text)
                };

                Detalles.Add(nuevo);
                ActualizarGrilla();

                ddlProducto.SelectedIndex = 0;
                txtCantidad.Text = "";
                txtCosto.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "alert('Ningun campo debe estar vacio.');", true);
                return;
            }
        }

        protected void btn_Eliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txt_Id.Text);
            clsMovimiento mov = new clsMovimiento { IdMovimiento = id };
            mov.Eliminar(connectionString);
            LimpiarFormulario();
        }
    }
}