﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class FacturaModoEntrada:Generica
    {

        public int Clave { get; set; }

        public string Nombre { get; set; }

        public int incluyeIVA { get; set; }

    }
}