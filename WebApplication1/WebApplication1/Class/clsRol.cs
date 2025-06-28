using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    public class clsRol
    {
        // Propiedades
        public int IdRol { get; set; }
        public string NombreRol { get; set; }

        // Agregar nuevo rol
        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Rol (NombreRol) VALUES (@NombreRol)";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@NombreRol", NombreRol);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Modificar rol existente
        public void Modificar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Rol SET NombreRol = @NombreRol WHERE IdRol = @IdRol";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdRol", IdRol);
                cmd.Parameters.AddWithValue("@NombreRol", NombreRol);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar rol
        public void Eliminar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM Rol WHERE IdRol = @IdRol";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdRol", IdRol);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Buscar rol por Id
        public bool Buscar(string connectionString, int idRol)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Rol WHERE IdRol = @IdRol";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdRol", idRol);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdRol = Convert.ToInt32(reader["IdRol"]);
                    NombreRol = reader["NombreRol"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Listar todos los roles
        public List<clsRol> ListarRoles(string connectionString)
        {
            List<clsRol> lista = new List<clsRol>();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "SELECT IdRol, NombreRol FROM Rol ORDER BY NombreRol";

                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new clsRol
                    {
                        IdRol = Convert.ToInt32(reader["IdRol"]),
                        NombreRol = reader["NombreRol"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}