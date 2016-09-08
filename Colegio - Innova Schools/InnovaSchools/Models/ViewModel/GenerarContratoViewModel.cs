using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using InnovaSchools.Util;

namespace InnovaSchools.Models
{
    public class GenerarContratoViewModel
    {
        //public int id_persona { get; set; }

        //[Display(Name = "DNI")]
        //public string dni { get; set; }
        
        //[Display(Name = "Nombre Candidato")]
        //public string nombre { get; set; }
        
        //[Display(Name = "Ape. Paterno Candidato")]
        //public string apellido_paterno { get; set; }
        
        //[Display(Name = "Ape. Materno Candidato")]
        //public string apellido_materno { get; set; }

        public string resultadoFind { get; set; }

        public Candidato Candidato { get; set; }
        public List<Candidato> Candidatos { get; set; }

        public Contrato Contrato { get; set; }
        public List<Contrato> Contratos { get; set; }

        public List<Puesto> PuestoList { get; set; }
        [Display(Name = "Puesto")]
        public int SelectedPuestoId { get; set; }
        public IEnumerable<SelectListItem> Puestos
        {
            get { return new SelectList(PuestoList, "idPuesto", "descripcionPuesto"); }
        }
    }
}