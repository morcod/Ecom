using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web.Mvc.Html;

namespace Ecom.Models
{
    public class Item
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        [Key]
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        public virtual Brand Brand { get; set; }
        [DisplayName("Brand")]
        [Required]
        public int BrandId { get; set; }
        public virtual Size Size { get; set; }
        [DisplayName("Size")]
        [Required]
        public int SizeId { get; set; }

        [DisplayName("Catagorie")]
        [Required]
        public int CatagorieId { get; set; }

        [Required(ErrorMessage = "An Item Name is required")]
        [StringLength(160)]
        public string Name { get; set; }

        [Required(ErrorMessage = "An Item Description is required")]
        [StringLength(1000)]
        public string Description { get; set; }


        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999.99,ErrorMessage = "Price must be between 0.01 and 999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Old Price is required")]
        [Range(0.01, 999.99, ErrorMessage = "Old Price must be between 0.01 and 999.99")]
        public decimal OldPrice { get; set; }

        public byte[] InternalImage { get; set; }
        public byte[] InternalImage2 { get; set; }
        public byte[] InternalImage3 { get; set; }
        [Display(Name = "Local file")]
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
                        InternalImage = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~/img/no-image.jpg"));

                    }
                    else
                    {
                        value.InputStream.CopyTo(target);
                        InternalImage = target.ToArray();

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
        }
        [NotMapped]
        public HttpPostedFileBase File2
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
                        InternalImage2 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~/img/no-image.jpg"));

                    }
                    else
                    {
                        value.InputStream.CopyTo(target);
                        InternalImage2 = target.ToArray();

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
        }
        [NotMapped]
        public HttpPostedFileBase File3
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
                        InternalImage3 = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"~/img/no-image.jpg"));

                    }
                    else
                    {
                        value.InputStream.CopyTo(target);
                        InternalImage3 = target.ToArray();

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
        }

        

        public virtual Catagorie Catagorie { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}