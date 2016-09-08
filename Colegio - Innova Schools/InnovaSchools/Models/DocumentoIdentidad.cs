﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaSchools.Models
{
    [Table("gaa.DocumentoIdentidad")]
    public class DocumentoIdentidad
    {
        [Key]
        [Required]
        public int idDocumentoIdentidad { get; set; }

        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
    }
}