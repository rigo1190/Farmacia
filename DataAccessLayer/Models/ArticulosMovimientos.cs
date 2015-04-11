using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ArticulosMovimientos:Generica
    {

        public ArticulosMovimientos()
        {
            this.detallaEntradas = new HashSet<ArticulosMovimientosEntradas>();
            this.detalleSalidas = new HashSet<ArticulosMovimientosSalidas>();
        }

        public int Ejercicio { get; set; }
        public int Movimiento { get; set; }
        public int Tipo { get; set; }//0:InventarioInicial; 1:Entradas; 2:Salidas

        public DateTime Fecha { get; set; }

        public int Status { get; set; }

        //Entradas
        public int? InventarioInicialId { get; set; }
        public int? FacturaAlmacenId { get; set; }
        public int? AlmacenEntradaGenericaId { get; set; }

        //Salidas
        public int? VentaId { get; set; }
        public int? AlmacenSalidaGenericaId { get; set; }

        public virtual InventarioInicial InventarioInicial { get; set; }
        public virtual FacturasAlmacen FacturaAlmacen { get; set; }
        public virtual AlmacenEntradasGenericas AlmacenEntradaGenerica { get; set; }


        public virtual Ventas Venta { get; set; }
        public virtual AlmacenSalidasGenericas AlmacenSalidaGenerica { get; set; }


        public virtual ICollection<ArticulosMovimientosEntradas> detallaEntradas { get; set; }
        public virtual ICollection<ArticulosMovimientosSalidas> detalleSalidas { get; set; }

    }
}
