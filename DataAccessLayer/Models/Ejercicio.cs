using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Ejercicio:Generica
    {
        [Index(IsUnique = true)]
        public int Año { get; set; }
        public decimal FactorIva { get; set; }

        [Range(0, 2, ErrorMessage = "El campo {0} solo puede tener valores entre {1} y {2}")]
        public enumEstatusEjercicio Estatus { get; set; }
    }

    public enum enumEstatusEjercicio 
    {
        Nuevo=1,
        Activo=2,
        Cerrado=3
    }

}
