using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class PartidasContables:Generica
    {
        public PartidasContables()
        {
            this.SubPartidas = new HashSet<PartidasContables>();
            this.gruposProductosServicios = new HashSet<GruposPS>();
        }

        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Clave{get; set;}

        [StringLength(255, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Nombre{get;set;}

        public int Nivel{get;set;}

        public int Tipo{get;set;}

        public int Status{get; set;}

        public int? ParentId{get; set;}

        public virtual PartidasContables Parent { get; set; }

        public virtual ICollection<PartidasContables> SubPartidas { get; set; }

        public virtual ICollection<GruposPS> gruposProductosServicios { get; set; }

    } 

}
