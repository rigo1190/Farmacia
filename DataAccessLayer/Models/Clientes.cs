using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Clientes:Generica
    {
        public Clientes()
        {
            this.detalleVentas = new HashSet<Ventas>();
        }

        [StringLength(20, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string RFC { get; set; }

        [StringLength(255, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string RazonSocial { get; set; }

        [StringLength(100, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Calle { get; set; }

        [StringLength(20, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string NumeroExterior { get; set; }

        [StringLength(20, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string NumeroInterior { get; set; }

        [StringLength(100, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Colonia { get; set; }

        [StringLength(100, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Localidad { get; set; }

        [StringLength(100, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Municipio { get; set; }

        [StringLength(100, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Estado { get; set; }

        [StringLength(100, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Pais { get; set; }

        public int CodigoPostal { get; set; }

        [StringLength(100, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Telefono { get; set; }
        [StringLength(100, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Email { get; set; }


        public virtual ICollection<Ventas> detalleVentas { get; set; }

    }
}
