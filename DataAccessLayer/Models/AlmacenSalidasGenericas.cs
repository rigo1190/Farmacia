using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class AlmacenSalidasGenericas:Generica
    {
        public AlmacenSalidasGenericas()
        {
            this.detalleArticulos = new HashSet<AlmacenSalidasGenericasArticulos>();
        }

        public int Ejercicio { get; set; }
        public int Folio { get; set; }
        public DateTime Fecha { get; set; }

        public int TipoSalidaId { get; set; } 
        public string Observaciones { get; set; }


        public virtual ICollection<AlmacenSalidasGenericasArticulos> detalleArticulos { get; set; }
        public virtual TipoSalida TipoSalida { get; set; } 

    }
}
