using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebApplication1.Class
{
    public class clsProducto
    {
        // Propiedades
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Costo { get; set; }
        public clsReferencia IdCategoria { get; set; }
        public clsReferencia IdUnidad { get; set; }
        public clsReferencia IdProveedor { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public bool Descontinuado { get; set; }

        // Agregar nuevo producto
        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Producto 
            (Codigo, Nombre, Descripcion, FechaVencimiento, Costo, IdCategoria, 
            IdUnidad, IdProveedor, StockMinimo, StockMaximo, Descontinuado)
            VALUES 
            (@Codigo, @Nombre, @Descripcion, @FechaVencimiento, @Costo, @IdCategoria, @IdUnidad, @IdProveedor, 
            @StockMinimo, @StockMaximo, @Descontinuado)";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Codigo", Codigo);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
                cmd.Parameters.AddWithValue("@FechaVencimiento", FechaVencimiento);
                cmd.Parameters.AddWithValue("@Costo", Costo);
                cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria.Id);
                cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad.Id);
                cmd.Parameters.AddWithValue("@IdProveedor", IdProveedor.Id);
                cmd.Parameters.AddWithValue("@StockMinimo", StockMinimo);
                cmd.Parameters.AddWithValue("@StockMaximo", StockMaximo);
                cmd.Parameters.AddWithValue("@Descontinuado", Descontinuado);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Modificar producto existente
        public void Modificar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Producto SET 
            Codigo = @Codigo,
            Nombre = @Nombre,
            Descripcion = @Descripcion,
            FechaVencimiento = @FechaVencimiento,
            Costo = @Costo,
            IdCategoria = @IdCategoria,
            IdUnidad = @IdUnidad,
            IdProveedor = @IdProveedor,
            StockMinimo = @StockMinimo,
            StockMaximo = @StockMaximo,
            Descontinuado = @Descontinuado
            WHERE IdProducto = @IdProducto";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdProducto", IdProducto);
                cmd.Parameters.AddWithValue("@Codigo", Codigo);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Descripcion);
                cmd.Parameters.AddWithValue("@FechaVencimiento", FechaVencimiento);
                cmd.Parameters.AddWithValue("@Costo", Costo);
                cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria.Id);
                cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad.Id);
                cmd.Parameters.AddWithValue("@IdProveedor", IdProveedor.Id);
                cmd.Parameters.AddWithValue("@StockMinimo", StockMinimo);
                cmd.Parameters.AddWithValue("@StockMaximo", StockMaximo);
                cmd.Parameters.AddWithValue("@Descontinuado", Descontinuado);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar producto por ID
        public void Eliminar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM Producto WHERE IdProducto = @IdProducto";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdProducto", IdProducto);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Buscar producto por ID
        public bool Buscar(string connectionString, int id)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Producto WHERE IdProducto = @IdProducto";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdProducto", id);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdProducto = Convert.ToInt32(reader["IdProducto"]);
                    Codigo = reader["Codigo"].ToString();
                    Nombre = reader["Nombre"].ToString();
                    Descripcion = reader["Descripcion"].ToString();
                    FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"]);
                    Costo = Convert.ToDecimal(reader["Costo"]);
                    IdCategoria = new clsReferencia { Id = Convert.ToInt32(reader["IdCategoria"]) };
                    IdUnidad = new clsReferencia { Id = Convert.ToInt32(reader["IdUnidad"]) };
                    IdProveedor = new clsReferencia { Id = Convert.ToInt32(reader["IdProveedor"]) };
                    StockMinimo = Convert.ToInt32(reader["StockMinimo"]);
                    StockMaximo = Convert.ToInt32(reader["StockMaximo"]);
                    Descontinuado = Convert.ToBoolean(reader["Descontinuado"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Listar todos los productos
        public List<clsProducto> ListarProductos(string connectionString)
        {
            List<clsProducto> lista = new List<clsProducto>();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                p.IdProducto,
                p.Codigo,
                p.Nombre,
                p.Descripcion,
                p.FechaVencimiento,
                p.Costo,
                p.IdCategoria,
                c.Nombre AS Categoria,
                p.IdUnidad,
                un.Nombre AS Unidad,
                p.IdProveedor,
                pr.RazonSocial AS Proveedor,
                p.StockMinimo,
                p.StockMaximo,
                p.Descontinuado
            FROM Producto p
            INNER JOIN Categoria c ON p.IdCategoria = c.IdCategoria
            INNER JOIN UnidadMedida un ON p.IdUnidad = un.IdUnidad
            INNER JOIN Proveedor pr ON p.IdProveedor = pr.IdProveedor";

                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new clsProducto
                    {
                        IdProducto = Convert.ToInt32(reader["IdProducto"]),
                        Codigo = reader["Codigo"].ToString(),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        FechaVencimiento = DateTime.Parse(reader["FechaVencimiento"].ToString()),
                        Costo = decimal.Parse(reader["Costo"].ToString()),
                        IdCategoria = new clsReferencia
                        {
                            Id = Convert.ToInt32(reader["IdCategoria"]),
                            Nombre = reader["Categoria"].ToString()
                        },
                        IdUnidad = new clsReferencia
                        {
                            Id = Convert.ToInt32(reader["IdUnidad"]),
                            Nombre = reader["Unidad"].ToString()
                        },
                        IdProveedor = new clsReferencia
                        {
                            Id = Convert.ToInt32(reader["IdProveedor"]),
                            Nombre = reader["Proveedor"].ToString()
                        },
                        StockMinimo = Convert.ToInt32(reader["StockMinimo"]),
                        StockMaximo = Convert.ToInt32(reader["StockMaximo"]),
                        Descontinuado = Convert.ToBoolean(reader["Descontinuado"])
                    });
                }
            }
            return lista;
        }
    }
}