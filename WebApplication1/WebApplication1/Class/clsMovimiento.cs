using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    public class clsMovimiento
    {
        public int IdMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public clsReferencia IdTipoMovimiento { get; set; }
        public clsReferencia IdDetalleTipoMovimiento { get; set; }
        public string Observaciones { get; set; }
        public clsReferencia IdUsuario { get; set; }

        public List<clsDetalleMovimiento> Detalles { get; set; } = new List<clsDetalleMovimiento>();

        public void Agregar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                string queryEncabezado = @"INSERT INTO MovimientoInventario 
                (Fecha, IdTipoMovimiento, IdDetalleTipoMovimiento, Observaciones, IdUsuario)
                OUTPUT INSERTED.IdMovimiento
                VALUES (@Fecha, @IdTipoMovimiento, @IdDetalleTipoMovimiento, @Observaciones, @IdUsuario)";

                SqlCommand cmdEncabezado = new SqlCommand(queryEncabezado, cn);
                cmdEncabezado.Parameters.AddWithValue("@Fecha", Fecha);
                cmdEncabezado.Parameters.AddWithValue("@IdTipoMovimiento", IdTipoMovimiento.Id);
                cmdEncabezado.Parameters.AddWithValue("@IdDetalleTipoMovimiento", IdDetalleTipoMovimiento.Id);
                cmdEncabezado.Parameters.AddWithValue("@Observaciones", Observaciones);
                cmdEncabezado.Parameters.AddWithValue("@IdUsuario", IdUsuario.Id);

                IdMovimiento = (int)cmdEncabezado.ExecuteScalar();

                foreach (var detalle in Detalles)
                {
                    string queryDetalle = @"INSERT INTO DetalleMovimiento 
                    (IdMovimiento, IdProducto, Cantidad, Costo)
                    VALUES (@IdMovimiento, @IdProducto, @Cantidad, @Costo)";

                    SqlCommand cmdDetalle = new SqlCommand(queryDetalle, cn);
                    cmdDetalle.Parameters.AddWithValue("@IdMovimiento", IdMovimiento);
                    cmdDetalle.Parameters.AddWithValue("@IdProducto", detalle.IdProducto.Id);
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@Costo", detalle.Costo);

                    cmdDetalle.ExecuteNonQuery();
                }
            }
        }

        public void Modificar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                string query = @"UPDATE MovimientoInventario SET 
                             Fecha = @Fecha,
                             IdTipoMovimiento = @IdTipoMovimiento,
                             IdDetalleTipoMovimiento = @IdDetalleTipoMovimiento,
                             Observaciones = @Observaciones,
                             IdUsuario = @IdUsuario
                             WHERE IdMovimiento = @IdMovimiento";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdMovimiento", IdMovimiento);
                cmd.Parameters.AddWithValue("@Fecha", Fecha);
                cmd.Parameters.AddWithValue("@IdTipoMovimiento", IdTipoMovimiento.Id);
                cmd.Parameters.AddWithValue("@IdDetalleTipoMovimiento", IdDetalleTipoMovimiento.Id);
                cmd.Parameters.AddWithValue("@Observaciones", Observaciones);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario.Id);
                cmd.ExecuteNonQuery();

                SqlCommand cmdDelete = new SqlCommand("DELETE FROM DetalleMovimiento WHERE IdMovimiento = @IdMovimiento", cn);
                cmdDelete.Parameters.AddWithValue("@IdMovimiento", IdMovimiento);
                cmdDelete.ExecuteNonQuery();

                foreach (var detalle in Detalles)
                {
                    string queryDetalle = @"INSERT INTO DetalleMovimiento 
                    (IdMovimiento, IdProducto, Cantidad, Costo)
                    VALUES (@IdMovimiento, @IdProducto, @Cantidad, @Costo)";

                    SqlCommand cmdDetalle = new SqlCommand(queryDetalle, cn);
                    cmdDetalle.Parameters.AddWithValue("@IdMovimiento", IdMovimiento);
                    cmdDetalle.Parameters.AddWithValue("@IdProducto", detalle.IdProducto.Id);
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@Costo", detalle.Costo);

                    cmdDetalle.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string connectionString)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                SqlCommand cmdDetalles = new SqlCommand("DELETE FROM DetalleMovimiento WHERE IdMovimiento = @IdMovimiento", cn);
                cmdDetalles.Parameters.AddWithValue("@IdMovimiento", IdMovimiento);
                cmdDetalles.ExecuteNonQuery();

                SqlCommand cmdEncabezado = new SqlCommand("DELETE FROM MovimientoInventario WHERE IdMovimiento = @IdMovimiento", cn);
                cmdEncabezado.Parameters.AddWithValue("@IdMovimiento", IdMovimiento);
                cmdEncabezado.ExecuteNonQuery();
            }
        }

        public bool Buscar(string connectionString, int id)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                string query = @"SELECT * FROM MovimientoInventario WHERE IdMovimiento = @IdMovimiento";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdMovimiento", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        IdMovimiento = Convert.ToInt32(reader["IdMovimiento"]);
                        Fecha = Convert.ToDateTime(reader["Fecha"]);
                        IdTipoMovimiento = new clsReferencia { Id = Convert.ToInt32(reader["IdTipoMovimiento"]) };
                        IdDetalleTipoMovimiento = new clsReferencia { Id = Convert.ToInt32(reader["IdDetalleTipoMovimiento"]) };
                        Observaciones = reader["Observaciones"].ToString();
                        IdUsuario = new clsReferencia { Id = Convert.ToInt32(reader["IdUsuario"]) };
                    }
                    else
                    {
                        return false;
                    }
                }

                Detalles.Clear();
                string queryDetalles = @"SELECT * FROM DetalleMovimiento WHERE IdMovimiento = @IdMovimiento";
                SqlCommand cmdDetalles = new SqlCommand(queryDetalles, cn);
                cmdDetalles.Parameters.AddWithValue("@IdMovimiento", id);

                using (SqlDataReader reader = cmdDetalles.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Detalles.Add(new clsDetalleMovimiento
                        //{
                        //    IdDetalle = Convert.ToInt32(reader["IdDetalle"]),
                        //    IdMovimiento = Convert.ToInt32(reader["IdMovimiento"]),
                        //    IdProducto = new clsReferencia { Id = Convert.ToInt32(reader["IdProducto"]) },
                        //    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                        //    Costo = Convert.ToDecimal(reader["Costo"])
                        //});

                        int idProducto = Convert.ToInt32(reader["IdProducto"]);
                        clsProducto prod = new clsProducto();
                        prod.Buscar(connectionString, idProducto);

                        Detalles.Add(new clsDetalleMovimiento
                        {
                            IdDetalle = Convert.ToInt32(reader["IdDetalle"]),
                            IdMovimiento = Convert.ToInt32(reader["IdMovimiento"]),
                            IdProducto = new clsReferencia
                            {
                                Id = idProducto,
                                Nombre = prod.Nombre  // Agregamos el nombre aquí
                            },
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Costo = Convert.ToDecimal(reader["Costo"])
                        });
                    }
                }

                return true;
            }
        }
    }
}