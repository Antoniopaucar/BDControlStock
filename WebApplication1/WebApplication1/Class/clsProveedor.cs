using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    public class clsProveedor
    {
        // Propiedades
        public int IdProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string Representante { get; set; }
        public string RUC { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }


        // Agregar
        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Proveedor
                            (RazonSocial, Representante, RUC, Direccion, Telefono, Email)
                             VALUES 
                            (@RazonSocial, @Representante, @RUC, @Direccion, @Telefono, @Email)";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@RazonSocial", RazonSocial);
                cmd.Parameters.AddWithValue("@Representante", Representante);
                cmd.Parameters.AddWithValue("@RUC", RUC);
                cmd.Parameters.AddWithValue("@Direccion", Direccion);
                cmd.Parameters.AddWithValue("@Telefono", Telefono);
                cmd.Parameters.AddWithValue("@Email", Email);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Modificar 
        public void Modificar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Proveedor SET 
                            RazonSocial = @RazonSocial,
                            Representante = @Representante,
                            RUC = @RUC,
                            Direccion = @Direccion,
                            Telefono = @Telefono,
                            Email = @Email
                            WHERE IdProveedor = @IdProveedor";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdProveedor", IdProveedor);
                cmd.Parameters.AddWithValue("@RazonSocial", RazonSocial);
                cmd.Parameters.AddWithValue("@Representante", Representante);
                cmd.Parameters.AddWithValue("@RUC", RUC);
                cmd.Parameters.AddWithValue("@Direccion", Direccion);
                cmd.Parameters.AddWithValue("@Telefono", Telefono);
                cmd.Parameters.AddWithValue("@Email", Email);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar 
        public void Eliminar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM Proveedor WHERE IdProveedor = @IdProveedor";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdProveedor", IdProveedor);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Buscar 
        public bool Buscar(string connectionString, int id)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Proveedor WHERE IdProveedor = @IdProveedor";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdProveedor", id);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdProveedor = Convert.ToInt32(reader["IdProveedor"]);
                    RazonSocial = reader["RazonSocial"].ToString();
                    Representante = reader["Representante"].ToString();
                    RUC = reader["RUC"].ToString();
                    Direccion = reader["Direccion"].ToString();
                    Telefono = reader["Telefono"].ToString();
                    Email = reader["Email"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<clsProveedor> ListarProveedores(string connectionString)
        {
            List<clsProveedor> lista = new List<clsProveedor>();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Proveedor ORDER BY RazonSocial";

                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new clsProveedor
                    {
                        IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                        RazonSocial = reader["RazonSocial"].ToString(),
                        Representante = reader["Representante"].ToString(),
                        RUC = reader["RUC"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}