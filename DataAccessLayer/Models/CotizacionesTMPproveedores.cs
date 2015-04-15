using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class CotizacionesTMPproveedores:Generica
    {
        public int UsuarioId { get; set; }
        public int ProveedorId { get; set; }

        public int Cantidad { get; set; }

        public virtual Proveedores Proveedor { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
