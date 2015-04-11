using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ArticulosMovimientosEntradas:Generica
    {

        public int ArticuloMovimientoId { get; set; }
        public int ArticuloId { get; set; }
        public int Cantidad { get; set; }
        public decimal Importe { get; set; }
        public decimal NuevoCostoPromedio { get; set; }

        public virtual ArticulosMovimientos ArticuloMovimiento { get; set; }
        public virtual Articulos Articulo { get; set; }

    }
}
