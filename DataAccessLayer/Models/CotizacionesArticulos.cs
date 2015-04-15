using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class CotizacionesArticulos:Generica
    {
        public int CotizacionId { get; set; }


        public int ArticuloId { get; set; }

        public int Cantidad { get; set; }

        public int esMedicamento { get; set; }

        public decimal Precio { get; set; }

        public decimal Subtotal { get; set; }

        public decimal IVA { get; set; }

        public decimal Total { get; set; }

        public int? ProveedorId { get; set; }

        public Cotizaciones Cotizacion { get; set; }
        public virtual Articulos Articulo { get; set; }

        public Proveedores Proveedor { get; set; }
        
    }
}
