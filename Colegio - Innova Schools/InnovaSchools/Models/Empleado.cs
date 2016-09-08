using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaSchools.Models
{
    [Table("gsp.Empleado")]
    public class Empleado
    {
        [Key]
        [Required(AllowEmptyStrings = true)]
        public int idEmpleado { get; set; }

        [Display(Name = "Fondo de Pensiones")]
        public string fondo_pensiones { get; set; }

        [Display(Name = "Seguro")]
        public string seguro { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [ForeignKey("idContrato")]
        public virtual Contrato Contrato { get; set; }
        public int idContrato { get; set; }
    }
}