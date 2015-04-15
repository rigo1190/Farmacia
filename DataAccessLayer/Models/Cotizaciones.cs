using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Cotizaciones:Generica
    {

        public int Ejercicio { get; set; }
        public int IdFolio { get; set; }
        public string Folio { get; set; }

        public DateTime Fecha { get; set; }

        public decimal CostoAproximado { get; set; }
        public int Status { get; set; }

    }
}
