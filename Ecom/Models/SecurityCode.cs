using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecom.Models
{
    public class SecurityCode
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string code { get; set; }

    }
}