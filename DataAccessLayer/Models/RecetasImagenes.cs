using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class RecetasImagenes:Generica
    {
        public int RecetaId { get; set; }

        [StringLength(250)]
        public string NombreArchivo { get; set; }

        [StringLength(250)]
        public string TipoArchivo { get; set; }
        public string Observaciones { get; set; }
        public virtual Recetas Receta { get; set; }
    }
}
