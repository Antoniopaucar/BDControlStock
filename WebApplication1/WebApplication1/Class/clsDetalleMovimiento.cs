using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Class
{
    [Serializable]
    public class clsDetalleMovimiento
    {
         public int IdDetalle { get; set; }
         public int IdMovimiento { get; set; }
         public clsReferencia IdProducto { get; set; }
         public int Cantidad { get; set; }
         public decimal Costo { get; set; }
    }
}