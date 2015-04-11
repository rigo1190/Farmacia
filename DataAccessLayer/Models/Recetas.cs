using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Recetas:Generica
    {
        public Recetas()
        {
            this.detalleReceta = new HashSet<RecetaArticulos>();
        }

        public int Ejercicio { get; set; }
        public int Folio { get; set; }
        public DateTime Fecha { get; set; }
        public string NombrePaciente { get; set; }
        public int Status { get; set; }
        public string Observaciones { get; set; }

        //dejar estos campos en blanco
        public string Sintomas { get; set; }
        public string Diagnostico { get; set; }        
        public string PresionArterial { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }
        //dejar estos campos en blanco


        public virtual ICollection<RecetaArticulos> detalleReceta { get; set; }


    }
}
