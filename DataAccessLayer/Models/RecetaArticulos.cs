using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class RecetaArticulos:Generica
    {

        public int RecetaId { get; set; }
        public int? ArticuloId { get; set; }
        public string NombreMedicamento { get; set; }
        public string CantidadATomar { get; set; }
        public string Frecuenca { get; set; }
        public string Durante { get; set; }
        public string Observaciones { get; set; }

        public virtual Recetas Receta { get; set; }
        public virtual Articulos Articulo { get; set; }
    }
}
