﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class CotizacionesProveedores:Generica
    {
        public int CotizacionId { get; set; }
        public int ProveedorId { get; set; }

        

        public virtual Proveedores Proveedor { get; set; }

        public virtual Cotizaciones Cotizacion { get; set; }
    }
}
