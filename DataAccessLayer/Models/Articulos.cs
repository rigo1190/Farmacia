using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Articulos:Generica
    {


        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Clave { get; set; }

        [StringLength(500, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        

       


        public int GruposPSId { get; set; }

        public int UnidadesDeMedidaId { get; set; }
        
        public int CantidadUDM { get; set; }

        public int PresentacionId { get; set; }

        


        public int esMedicamento { get; set; }

        public double Porcentaje { get; set; }




        public int? LaboratorioId { get; set; }

        public int? MarcaId { get; set; }

        
        public int FPSfactorId { get; set; }

        public string Observaciones { get; set; }


        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public int CantidadEnAlmacen { get; set; }
        public int CantidadComprometida { get; set; }
        public int CantidadDisponible { get; set; }        
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        
              
        

        public virtual GruposPS GruposPS { get; set; }
        public virtual UnidadesDeMedida UnidadesDeMedida { get; set; }

        public virtual Presentaciones Presentacion { get; set; }       

        public virtual Laboratorios Laboratorio { get; set; }

        public virtual Marcas Marca { get; set; }
      
        public virtual FPSfactores FPSfactor { get; set; }




    }
}
