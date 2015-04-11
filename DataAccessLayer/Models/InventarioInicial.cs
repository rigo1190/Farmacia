using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class InventarioInicial: Generica
    {
        public InventarioInicial()
        {
            this.DetalleArticulos = new HashSet<InventarioInicialArticulos>();
        }

        public int EjercicioCierre { get; set; }
        public int Ejercicio { get; set; }
        public DateTime Fecha { get; set; }

        public int Status { get; set; }
        public string Observaciones { get; set; }


        public virtual ICollection<InventarioInicialArticulos> DetalleArticulos { get; set; }
    }
}
