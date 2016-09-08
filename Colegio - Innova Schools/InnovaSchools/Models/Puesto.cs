using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaSchools.Models
{
    [Table("gsp.Puesto")]
    public class Puesto
    {
        [Key]
        [Required]
        public int idPuesto { get; set; }

        [Display(Name = "Puesto")]
        public string descripcionPuesto { get; set; }

        [Display(Name = "Función")]
        public string funcion { get; set; }

        [ForeignKey("idArea")]
        public virtual Area Area { get; set; }
        public int idArea { get; set; }
    }
}