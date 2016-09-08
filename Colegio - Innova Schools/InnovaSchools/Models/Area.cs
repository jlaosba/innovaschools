﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaSchools.Models
{
    [Table("gaa.Area")]
    public class Area
    {
        [Key]
        [Required]
        public int idArea { get; set; }

        [Display(Name = "Área")]
        public string descripcion { get; set; }
    }
}