using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaSchools.Models
{
    [Table("gsp.Contrato")]
    public class Contrato
    {
        [Key]
        [Required]
        [Display(Name = "Nro. Contrato")]
        public int idContrato { get; set; }

        [Display(Name = "Fecha Ingreso")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaIngreso { get; set; }

        [Display(Name = "Fecha Cese")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaCese { get; set; }

        //[ForeignKey("id_empleado")]
        //public int id_empleado { get; set; }
        //public virtual Empleado Empleado { get; set; }

        [Display(Name = "Salario")]
        public decimal salario { get; set; }

        [ForeignKey("idTipoContrato")]
        public virtual TipoContrato TipoContrato { get; set; }
        public int idTipoContrato { get; set; }

    }
}