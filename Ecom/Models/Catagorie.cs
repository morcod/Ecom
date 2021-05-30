using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.IO;

namespace Ecom.Models
{
    public class Catagorie
    {
        [Key]
        [DisplayName("Catagorie ID")]
        public int ID { get; set; }
        public byte[] Image { get; set; }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {


                    MemoryStream target = new MemoryStream();

                    if (value == null || value.InputStream == null)
                    {
                        Image= System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~/img/no-image.jpg"));

                    }
                    else
                    {
                        value.InputStream.CopyTo(target);
                        Image = target.ToArray();

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
        }
        [Required(ErrorMessage = "A Catagorie Name is required")]
        [StringLength(160)]
        [DisplayName("Catagorie")]
        public string Name { get; set; }

        public Boolean isParent { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public int? ParentID { get; set; }
        public string ParentName { get; set; }

        [ForeignKey("ParentID")]
        public virtual ICollection<Catagorie> SubCatagories { get; set; }
    }
}