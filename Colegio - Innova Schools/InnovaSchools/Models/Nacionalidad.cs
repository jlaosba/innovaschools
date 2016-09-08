﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaSchools.Models
{
    [Table("gsp.Nacionalidad")]
    public class Nacionalidad
    {
        [Key]
        [Required]
        public int idNacionalidad { get; set; }

        [Display(Name = "Nacionalidad")]
        public string nacionalidad { get; set; }
    }
}