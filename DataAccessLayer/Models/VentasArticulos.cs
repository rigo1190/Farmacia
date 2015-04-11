using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Models
{
    public class VentasArticulos:Generica
    {

        

        public int VentaId { get; set; }
        public int ArticuloId { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }

        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

        public virtual Ventas Venta { get; set; }
        public virtual Articulos Articulo { get; set; }

        

    }
}
