using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    public class clsTipoMovimiento
    {
        // Propiedades
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Agregar nuevo rol
        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO TipoMovimiento (Nombre) VALUES (@Nombre)";
                SqlCommand cmd = new SqlCommand(query, cn);
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
                string query = @"UPDATE TipoMovimiento SET Nombre = @Nombre WHERE IdTipoMovimiento = @Id";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Id", Id);
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
                string query = @"DELETE FROM TipoMovimiento WHERE IdTipoMovimiento = @Id";
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
                string query = @"SELECT * FROM TipoMovimiento WHERE IdTipoMovimiento = @Id";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Id", id);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Id = Convert.ToInt32(reader["IdTipoMovimiento"]);
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
        public List<clsTipoMovimiento> ListarTiposMov(string connectionString)
        {
            List<clsTipoMovimiento> lista = new List<clsTipoMovimiento>();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM TipoMovimiento ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new clsTipoMovimiento
                    {
                        Id = Convert.ToInt32(reader["IdTipoMovimiento"]),
                        Nombre = reader["Nombre"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}