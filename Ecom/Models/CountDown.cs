using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Models
{
    public class CountDown
    {
        [Key]
        public int ID { get; set; }
       
        [Required(ErrorMessage = "A message is required")]
        [StringLength(160)]
        public string Message { get; set; }
        public System.DateTime CountDownDate { get; set; }

    }
}