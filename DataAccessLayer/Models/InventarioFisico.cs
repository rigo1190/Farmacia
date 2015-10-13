using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class InventarioFisico:Generica
    {
        public int UsuarioId { get; set; }
        public int ArticuloId { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Clave { get; set; }


        public int ExistenciaEnSistema { get; set; }
        public int Cantidad { get; set; }


        public virtual Articulos Articulo { get; set; }

    }
}
