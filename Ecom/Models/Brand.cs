using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ecom.Models
{
    public class Brand
    {
        [Key]
        [DisplayName("Brand ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "A Brand Name is required")]
        [StringLength(160)]
        [DisplayName("Brand")]
        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}