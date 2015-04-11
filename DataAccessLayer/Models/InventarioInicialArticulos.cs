using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class InventarioInicialArticulos:Generica
    {
        public InventarioInicialArticulos()
        {
            this.DetalleCostos = new HashSet<InventarioInicialArticulosCostos>();
        }

        public int InventarioInicialId { get; set; }

        public int ArticuloId { get; set; }

        public int Cantidad { get; set; }

        public decimal CostoPromedio { get; set; }


        public virtual InventarioInicial InventarioInicial { get; set; }
        public virtual Articulos Articulo { get; set; }

        public virtual ICollection<InventarioInicialArticulosCostos> DetalleCostos { get; set; }

    }
}
