using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class FacturasAlmacen:Generica
    {

        public FacturasAlmacen()
        {
            this.detalleArticulos = new HashSet<FacturasAlmacenArticulos>();
        }

        public int Ejercicio { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string FolioFactura { get; set; }

        public DateTime FechaFactura { get; set; }

        public decimal ImporteFactura { get; set; }
        public string Observaciones { get; set; }
        public int Status { get; set; }
        public int Tipo { get; set; }
        public int PedidoId { get; set; }
        
        public int Consecutivo { get; set; }

        public virtual Pedidos Pedido { get; set; }

        

        public virtual ICollection<FacturasAlmacenArticulos> detalleArticulos { get; set; }

    }
}
