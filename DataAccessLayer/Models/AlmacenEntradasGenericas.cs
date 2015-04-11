using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Models
{
    public class AlmacenEntradasGenericas:Generica
    {

        public AlmacenEntradasGenericas()
        {
            this.DetalleArticulos = new HashSet<AlmacenEntradasGenericasArticulos>();
        }

        public int Ejercicio { get; set; }
        public int Folio { get; set; }
        public DateTime Fecha { get; set; }

        public string Observaciones { get; set; }


        public virtual ICollection<AlmacenEntradasGenericasArticulos> DetalleArticulos { get; set; }

    }
}
