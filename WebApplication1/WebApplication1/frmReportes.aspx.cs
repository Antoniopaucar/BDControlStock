using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.IO;

namespace WebApplication1
{
    public partial class frmReportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicializar el ReportViewer
                ReportViewerPerdidas.ProcessingMode = ProcessingMode.Local;
                ReportViewerPerdidas.ShowParameterPrompts = false;
                ReportViewerPerdidas.ShowToolBar = true;
                ReportViewerPerdidas.Width = Unit.Percentage(100);
                ReportViewerPerdidas.Height = Unit.Pixel(600);
            }
        }

        protected void btn_Rep_Inv_Actual_Click(object sender, EventArgs e)
        {
            try
            {
                pnlGrillas.Visible = false;
                pnlReportes.Visible = true;
                ViewState["ReporteActivo"] = "Inventario";
                // Ocultar otros reportes y mostrar solo este
                ReportViewerPerdidas.Visible = false;
                ReportViewerInventario.Visible = true;
                
                // Configurar la ruta del reporte
                string reportPath = Server.MapPath("~/ReporteInventario.rdlc");
                if (!System.IO.File.Exists(reportPath))
                {
                    throw new FileNotFoundException($"No se encontró el archivo de reporte en: {reportPath}");
                }
                
                // Obtener los datos usando el procedimiento almacenado
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAlmacenConnection"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("MostrarProductosEnStock", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
                
                // Verificar si hay datos
                if (dt.Rows.Count == 0)
                {
                    dt.Columns.Add("Mensaje");
                    dt.Rows.Add("No se encontraron productos en el inventario");
                }
                
                // Configurar el ReportViewer
                ReportViewerInventario.ProcessingMode = ProcessingMode.Local;
                ReportViewerInventario.LocalReport.ReportPath = reportPath;
                
                // Crear y configurar el origen de datos
                ReportDataSource rds = new ReportDataSource("DataSetInventario", dt);
                
                // Limpiar fuentes de datos existentes y agregar la nueva
                ReportViewerInventario.LocalReport.DataSources.Clear();
                ReportViewerInventario.LocalReport.DataSources.Add(rds);
                
                // Actualizar el reporte
                ReportViewerInventario.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error al cargar el reporte de inventario: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\nDetalles: {ex.InnerException.Message}";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", 
                    $"alert('{errorMsg.Replace("'", "\\'")}');", true);
            }
        }

        protected void btn_Rep_Perdidas_Click(object sender, EventArgs e)
        {
            try
            {
                pnlGrillas.Visible = false;
                pnlReportes.Visible = true;

                ReportViewerPerdidas.Visible = true;
                ReportViewerInventario.Visible = false;

                ViewState["ReporteActivo"] = "Perdidas";
                // Mostrar el ReportViewer
                ReportViewerPerdidas.Visible = true;
                
                // Configurar la ruta del reporte
                string reportPath = Server.MapPath("~/ReportePerdidas.rdlc");
                if (!System.IO.File.Exists(reportPath))
                {
                    throw new FileNotFoundException($"No se encontró el archivo de reporte en: {reportPath}");
                }
                
                // Obtener los datos usando el procedimiento almacenado
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAlmacenConnection"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("AuditoriaAjusteNegativoOMerma", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
                
                // Verificar si hay datos
                if (dt.Rows.Count == 0)
                {
                    // Agregar una fila vacía si no hay datos para evitar errores
                    dt.Columns.Add("Mensaje");
                    dt.Rows.Add("No se encontraron registros de pérdidas");
                }
                
                // Configurar el ReportViewer
                ReportViewerPerdidas.ProcessingMode = ProcessingMode.Local;
                ReportViewerPerdidas.LocalReport.ReportPath = reportPath;
                
                // Crear y configurar el origen de datos
                ReportDataSource rds = new ReportDataSource("DataSetPerdidas", dt);
                
                // Limpiar fuentes de datos existentes y agregar la nueva
                ReportViewerPerdidas.LocalReport.DataSources.Clear();
                ReportViewerPerdidas.LocalReport.DataSources.Add(rds);
                
                // Actualizar el reporte
                ReportViewerPerdidas.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                // Mostrar el error real con más detalles
                string errorMsg = $"Error al cargar el reporte: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\nDetalles: {ex.InnerException.Message}";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", 
                    $"alert('{errorMsg.Replace("'", "\\'")}');", true);
            }
        }

        private DataTable ObtenerMovimientos(DateTime? fIni, DateTime? fFin)
        {
            var dt = new DataTable();
            string cs = ConfigurationManager.ConnectionStrings["DBAlmacenConnection"].ConnectionString;

            using (var cn = new SqlConnection(cs))
            using (var cmd = new SqlCommand("usp_Movimientos_Listar", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                //  Parametriza: si el valor es null → DBNull.Value
                cmd.Parameters.Add("@FechaInicio", SqlDbType.Date)
                               .Value = (object)fIni ?? DBNull.Value;
                cmd.Parameters.Add("@FechaFin", SqlDbType.Date)
                               .Value = (object)fFin ?? DBNull.Value;

                new SqlDataAdapter(cmd).Fill(dt);
            }
            return dt;
        }

        private void CargarMovimientos(DateTime? fIni, DateTime? fFin)
        {
            gvMovimientos.DataSource = ObtenerMovimientos(fIni, fFin);
            gvMovimientos.DataBind();
        }

        /* ——— 1.º botón: “REPORTE DE MOVIMIENTOS…” ——— */
        protected void btn_Rep_Mov_Fechas_Click(object sender, EventArgs e)
        {
            pnlReportes.Visible = false;
            pnlGrillas.Visible = true;

            ViewState["ReporteActivo"] = "Movimientos";
            ViewState["FIni"] = ViewState["FFin"] = null;

            CargarMovimientos(null, null);
            ViewState["ReporteActivo"] = "Movimientos";
            // 1) Limpia filtros y mensajes
            txtFechaInicio.Text = txtFechaFin.Text = "";
            lblMensaje.Text = "";

            // 2) Vuelve a cero el ViewState (sin rango)
            ViewState["FIni"] = null;
            ViewState["FFin"] = null;

            // 3) Oculta otras secciones si las tienes
            gvProductosMasSalidas.Visible = false;

            // 4) Carga los movimientos
            CargarMovimientos(null, null);

            // 5) MUESTRA la grilla de movimientos
            gvMovimientos.Visible = true;
        }

        /* ——— 2.º botón: FILTRAR ——— */
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(txtFechaInicio.Text, out DateTime fIni) ||!DateTime.TryParse(txtFechaFin.Text, out DateTime fFin))
            {
                lblMensaje.Text = "Fechas no válidas.";
                return;
            }

            // 1) Averigua qué reporte está visible
            string activo = ViewState["ReporteActivo"] as string ?? "";

            switch (activo)
            {
                case "Movimientos":
                    ViewState["FIni"] = fIni;
                    ViewState["FFin"] = fFin;
                    CargarMovimientos(fIni, fFin);
                    gvMovimientos.Visible = true;
                    break;

                case "ProdSalidas":
                    ViewState["FIniProd"] = fIni;
                    ViewState["FFinProd"] = fFin;
                    gvProductosMasSalidas.DataSource =
                        ObtenerProductosMasSalidas(fIni, fFin);
                    gvProductosMasSalidas.DataBind();
                    gvProductosMasSalidas.Visible = true;
                    break;

                /* Agrega aquí otros reportes (Stock, Pérdidas, etc.) */
                default:
                    lblMensaje.Text = "Selecciona un reporte antes de filtrar.";
                    break;
            }
        }

        /* ——— Paginación ——— */
        protected void gvMovimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMovimientos.PageIndex = e.NewPageIndex;
            CargarMovimientos((DateTime?)ViewState["FIni"],
                              (DateTime?)ViewState["FFin"]);
        }
        protected void gvMovimientos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btn_Rep_Prod_Sal_Click(object sender, EventArgs e)
        {
            pnlReportes.Visible = false;
            pnlGrillas.Visible = true;


            ViewState["ReporteActivo"] = "ProdSalidas";
            /* 1) Limpia otros mensajes o filtros si lo deseas.
              (Dejaremos las fechas tal cual, porque este reporte
               puede aprovechar el rango ya escrito por el usuario) */
            lblMensaje.Text = "";

            /* 2) Intenta leer las fechas, pero sin obligarlas.
                  Si el usuario no puso nada -> null → Top global   */
            DateTime? fIni = null, fFin = null;

            if (DateTime.TryParse(txtFechaInicio.Text, out DateTime fi)) fIni = fi;
            if (DateTime.TryParse(txtFechaFin.Text, out DateTime ff)) fFin = ff;

            /* 3) Guarda en ViewState para paginación del GridView */
            ViewState["FIniProd"] = fIni;
            ViewState["FFinProd"] = fFin;

            /* 4) Oculta grillas que no corresponden a este reporte */
            gvMovimientos.Visible = false;

            /* 5) Carga el TOP-10 (o el N que prefieras) de productos */
            gvProductosMasSalidas.DataSource = ObtenerProductosMasSalidas(fIni, fFin);
            gvProductosMasSalidas.DataBind();

            /* 6) Muestra la grilla de productos más salidas */
            gvProductosMasSalidas.Visible = true;
        }

        private DataTable ObtenerProductosMasSalidas(DateTime? fIni, DateTime? fFin)
        {
            var dt = new DataTable();
            string cs = ConfigurationManager.ConnectionStrings["DBAlmacenConnection"].ConnectionString;

            using (SqlDataAdapter da = new SqlDataAdapter("usp_Productos_MasSalidas", cs))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = (object)fIni ?? DBNull.Value;
                da.SelectCommand.Parameters.Add("@FechaFin", SqlDbType.Date).Value = (object)fFin ?? DBNull.Value;

                da.Fill(dt);
            }
            return dt;
        }
        protected void gvProdMasSalidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductosMasSalidas.PageIndex = e.NewPageIndex;

            // Recupera rango de fechas almacenado cuando pulsaste el botón
            DateTime? fIni = (DateTime?)ViewState["FIniProd"];
            DateTime? fFin = (DateTime?)ViewState["FFinProd"];

            gvProductosMasSalidas.DataSource = ObtenerProductosMasSalidas(fIni, fFin);
            gvProductosMasSalidas.DataBind();
        }
        protected void gvProductosMasSalidas_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            int idProd = Convert.ToInt32(gv.SelectedDataKey.Value);

            // Ejemplo: mostrar detalle o mensaje
            lblMensaje.Text = $"Seleccionaste el producto #{idProd}";
            // o bien: CargarDetalleProducto(idProd);
        }
    }
}