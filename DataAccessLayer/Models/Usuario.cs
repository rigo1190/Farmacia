using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class Usuario
    {
        
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Login { get; set; }

        [StringLength(16, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Password { get; set; }

        [StringLength(200, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        

    }
}
