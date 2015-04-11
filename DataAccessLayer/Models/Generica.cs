using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public abstract class Generica 
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreatedAt { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? EditedAt { get; set; }

        [ScaffoldColumn(false)]
        public int? CreatedById { get; set; }

        [ScaffoldColumn(false)]
        public int? EditedById { get; set; }

        [ScaffoldColumn(false)]
        public virtual Usuario CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public virtual Usuario EditedBy { get; set; }

        

    }
}
