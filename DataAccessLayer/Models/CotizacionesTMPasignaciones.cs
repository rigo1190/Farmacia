﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Models
{
    public class CotizacionesTMPasignaciones:Generica 
    {
        public int CotizacionId { get; set; }
        public int ArticuloId { get; set; }

        public int Cantidad { get; set; }

        public int? ProveedorId { get; set; } 
         
        public virtual Cotizaciones Cotizacion { get; set; }

        public virtual Articulos Articulo { get; set; }

        public virtual Proveedores Proveedor { get; set; }
    }
}
