using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class PedidosServicios:Generica
    {

        public int PedidoId { get; set; }
        public int ServicioId { get; set; }
        [StringLength(255, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string DescripcionDelServicio { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public double PorcentajeImpuesto { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public decimal Descuento { get; set; }
        public decimal ImporteFinal { get; set; }


        public virtual Pedidos Pedido { get; set; }
        public virtual Servicios Servicio{ get; set; }

    }
}
