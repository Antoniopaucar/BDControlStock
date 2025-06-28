using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    public class clsUnidadMedida
    {
        // Propiedades
        public int IdUnidad { get; set; }
        public string Nombre { get; set; }

        // Agregar nuevo rol
        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO UnidadMedida (Nombre) VALUES (@Nombre)";
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
                string query = @"UPDATE UnidadMedida SET Nombre = @Nombre WHERE IdUnidad = @IdUnidad";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad);
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
                string query = @"DELETE FROM UnidadMedida WHERE IdUnidad = @IdUnidad";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Buscar  por Id
        public bool Buscar(string connectionString, int id)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM UnidadMedida WHERE IdUnidad = @IdUnidad";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdUnidad", id);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdUnidad = Convert.ToInt32(reader["IdUnidad"]);
                    Nombre = reader["Nombre"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Listar todos los roles
        public List<clsUnidadMedida> ListarUnidades(string connectionString)
        {
            List<clsUnidadMedida> lista = new List<clsUnidadMedida>();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "SELECT IdUnidad, Nombre FROM UnidadMedida ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new clsUnidadMedida
                    {
                        IdUnidad = Convert.ToInt32(reader["IdUnidad"]),
                        Nombre = reader["Nombre"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}