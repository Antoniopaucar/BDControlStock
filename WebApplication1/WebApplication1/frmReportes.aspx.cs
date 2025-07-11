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

        protected void btn_Rep_Perdidas_Click(object sender, EventArgs e)
        {
            try
            {
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
    }
}