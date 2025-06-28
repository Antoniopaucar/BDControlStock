using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    public class clsDetTipoMovimiento
    {
        // Propiedades
        public int Id { get; set; }
        public int IdTipoMovimiento { get; set; }
        public string Nombre { get; set; }

        // Agregar nuevo rol
        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO DetalleTipoMovimiento (IdTipoMovimiento, Nombre) VALUES (@IdTipoMovimiento ,@Nombre)";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdTipoMovimiento", IdTipoMovimiento);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Modificar rol existente
        public void Modificar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE DetalleTipoMovimiento SET IdTipoMovimiento = @IdTipoMovimiento, Nombre = @Nombre WHERE IdDetalleTipoMovimiento = @Id";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@IdTipoMovimiento", IdTipoMovimiento);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar rol
        public void Eliminar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM DetalleTipoMovimiento WHERE IdDetalleTipoMovimiento = @Id";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Id", Id);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Buscar por id
        public bool Buscar(string connectionString, int id)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM DetalleTipoMovimiento WHERE IdDetalleTipoMovimiento = @Id";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Id", id);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Id = Convert.ToInt32(reader["IdDetalleTipoMovimiento"]);
                    IdTipoMovimiento = Convert.ToInt32(reader["IdTipoMovimiento"]);
                    Nombre = reader["Nombre"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Listar todos 
        public List<clsDetTipoMovimiento> ListarDetTiposMov(string connectionString,int IdTipoMov)
        {
            List<clsDetTipoMovimiento> lista = new List<clsDetTipoMovimiento>();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM DetalleTipoMovimiento WHERE IdTipoMovimiento = @IdTipoMov ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdTipoMov", IdTipoMov);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new clsDetTipoMovimiento
                    {
                        Id = Convert.ToInt32(reader["IdDetalleTipoMovimiento"]),
                        IdTipoMovimiento = Convert.ToInt32(reader["IdTipoMovimiento"]),
                        Nombre = reader["Nombre"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}