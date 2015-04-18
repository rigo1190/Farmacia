using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class FacturasAlmacenArticulos:Generica
    {

        public int FacturaAlmacenId { get; set; }
        public int ArticuloId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

        public int Adicional { get; set; }
        public decimal PrecioDeCompraAnterior { get; set; }

        public decimal Diferencia { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Status { get; set; }
        public string StatusNombre { get; set; }

        public virtual FacturasAlmacen FacturaAlmacen { get; set; }
        public virtual Articulos Articulo { get; set; }



        



    }
}
