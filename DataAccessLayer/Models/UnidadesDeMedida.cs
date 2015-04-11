using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Models
{
    public class UnidadesDeMedida:Generica
    {
        public UnidadesDeMedida()
        {
            this.UMcompras = new HashSet<Articulos>();

            this.UMventas = new HashSet<Articulos>();

        }

        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Clave { get; set; }

        [StringLength(255, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        public virtual ICollection<Articulos> UMventas { get; set; }

        public virtual ICollection<Articulos> UMcompras { get; set; }

    }
}
