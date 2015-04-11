using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Models
{
    public class GruposPS:Generica
    {

        public GruposPS()
        {
            this.Subgrupos = new HashSet<GruposPS>();
            this.detalleArticulos = new HashSet<Articulos>();
            this.detalleServicios = new HashSet<Servicios>();
        }
        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Clave { get; set; }


        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        public int Nivel { get; set; }
        public int Tipo { get; set; }

        public int? PartidaContableId { get; set; } 

        public int? ParentId { get; set; }


        public virtual PartidasContables PartidaContable { get; set; }
        public virtual GruposPS Parent { get; set; }

        public virtual ICollection<GruposPS> Subgrupos { get; set; }

        public virtual ICollection<Articulos> detalleArticulos { get; set; }
        public virtual ICollection<Servicios> detalleServicios { get; set; }


    }
}
