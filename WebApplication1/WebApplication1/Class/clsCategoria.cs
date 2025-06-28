using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    public class clsCategoria
    {
        // Propiedades
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        // Agregar nueva categoría
        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Categoria (Nombre, Descripcion)
                             VALUES (@Nombre, @Descripcion)";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Descripcion);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Modificar categoría existente
        public void Modificar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Categoria 
                             SET Nombre = @Nombre, Descripcion = @Descripcion
                             WHERE IdCategoria = @IdCategoria";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Descripcion);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar categoría por ID
        public void Eliminar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM Categoria WHERE IdCategoria = @IdCategoria";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Buscar categoría por ID
        public bool Buscar(string connectionString, int id)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Categoria WHERE IdCategoria = @IdCategoria";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdCategoria", id);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdCategoria = Convert.ToInt32(reader["IdCategoria"]);
                    Nombre = reader["Nombre"].ToString();
                    Descripcion = reader["Descripcion"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<clsCategoria> ListarCategorias(string connectionString)
        {
            List<clsCategoria> lista = new List<clsCategoria>();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "SELECT IdCategoria, Nombre, Descripcion FROM Categoria ORDER BY Nombre";

                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new clsCategoria
                    {
                        IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString()
                    });
                }
            }
            return lista;
        }
    }
}