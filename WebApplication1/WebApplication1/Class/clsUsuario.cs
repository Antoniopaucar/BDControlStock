using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    public class clsUsuario
    {
        // Propiedades
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Contrasenia { get; set; }
        public clsReferencia IdRol { get; set; }
        public bool Estado { get; set; }

        // Agregar nuevo usuario
        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Usuario 
                            (NombreUsuario, Nombres, Apellidos, Contrasenia, IdRol, Estado)
                             VALUES 
                            (@NombreUsuario, @Nombres, @Apellidos, @Contrasenia, @IdRol, @Estado)";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                cmd.Parameters.AddWithValue("@Nombres", Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", Apellidos);
                cmd.Parameters.AddWithValue("@Contrasenia", Contrasenia);
                cmd.Parameters.AddWithValue("@IdRol", IdRol.Id);
                cmd.Parameters.AddWithValue("@Estado", Estado);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Modificar usuario existente
        public void Modificar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query;
                if (!string.IsNullOrEmpty(Contrasenia))
                {
                    query = @"UPDATE Usuario SET 
                            NombreUsuario = @NombreUsuario,
                            Nombres = @Nombres,
                            Apellidos = @Apellidos,
                            Contrasenia = @Contrasenia,
                            IdRol = @IdRol,
                            Estado = @Estado
                         WHERE IdUsuario = @IdUsuario";
                }
                else
                {
                    query = @"UPDATE Usuario SET 
                            NombreUsuario = @NombreUsuario,
                            Nombres = @Nombres,
                            Apellidos = @Apellidos,
                            IdRol = @IdRol,
                            Estado = @Estado
                         WHERE IdUsuario = @IdUsuario";
                }

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                cmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                cmd.Parameters.AddWithValue("@Nombres", Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", Apellidos);
                cmd.Parameters.AddWithValue("@IdRol", IdRol.Id);
                cmd.Parameters.AddWithValue("@Estado", Estado);
                if (!string.IsNullOrEmpty(Contrasenia))
                {
                    cmd.Parameters.AddWithValue("@Contrasenia", Contrasenia);
                }

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        // Eliminar usuario por nombre de usuario
        public void Eliminar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM Usuario WHERE IdUsuario = @IdUsuario";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        // Buscar usuario por nombre de usuario
        public bool Buscar(string connectionString, int ID)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdUsuario", ID);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                    NombreUsuario = reader["NombreUsuario"].ToString();
                    Nombres = reader["Nombres"].ToString();
                    Apellidos = reader["Apellidos"].ToString();
                    Contrasenia = reader["Contrasenia"].ToString();
                    IdRol = new clsReferencia { Id = Convert.ToInt32(reader["IdRol"]) };
                    Estado = Convert.ToBoolean(reader["Estado"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Listar todos los usuarios
        public List<clsUsuario> ListarUsuarios(string connectionString)
        {
            List<clsUsuario> lista = new List<clsUsuario>();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                us.IdUsuario,
                us.NombreUsuario,
                us.Nombres,
                us.Apellidos,
                us.Contrasenia,
                us.IdRol,
                r.NombreRol AS Rol,
                us.Estado
            FROM Usuario us
            INNER JOIN Rol r ON us.IdRol = r.IdRol";

                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new clsUsuario
                    {
                        IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Nombres = reader["Nombres"].ToString(),
                        Apellidos = reader["Apellidos"].ToString(),
                        Contrasenia = "**********************",
                        IdRol= new clsReferencia
                        {
                            Id = Convert.ToInt32(reader["IdRol"]),
                            Nombre = reader["Rol"].ToString()
                        },
                        Estado = Convert.ToBoolean(reader["Estado"])
                    });
                }
            }
            return lista;
        }

        public bool IniciarSesion(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Usuario 
                             WHERE NombreUsuario = @NombreUsuario AND Contrasenia = @Contrasenia AND Estado = 1";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                cmd.Parameters.AddWithValue("@Contrasenia", Contrasenia); // Considera encriptar en producción

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                    Nombres = reader["Nombres"].ToString();
                    Apellidos = reader["Apellidos"].ToString();
                    IdRol = new clsReferencia { Id = Convert.ToInt32(reader["IdRol"]) };
                    Estado = Convert.ToBoolean(reader["Estado"]);
                    return true;
                }

                return false;
            }
        }
    }
}