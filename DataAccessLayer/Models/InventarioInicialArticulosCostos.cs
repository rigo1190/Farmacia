using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class InventarioInicialArticulosCostos:Generica
    {
        public int InventarioInicialArticuloId { get; set; }
        public int ArticuloId { get; set; }
        public int Orden { get; set; }
        public int Cantidad { get; set; }
        public decimal Costo { get; set; }

        public virtual InventarioInicialArticulos InventarioInicialArticulo { get; set; }
        public virtual Articulos Articulo { get; set; }
    }
}
