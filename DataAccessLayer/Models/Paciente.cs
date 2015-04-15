using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Paciente:Generica
    {
        [StringLength(250)]
        public string Nombre { get; set; }
        
        [StringLength(50)]
        public string Telefono { get; set; }

        [StringLength(250)]
        public string Correo { get; set; }

    }
}
