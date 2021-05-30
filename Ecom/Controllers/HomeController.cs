using Ecom.Models;
using PagedList;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ecom.Controllers {
    public class HomeController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var items = from i in db.Items
                        select i;
            items = items.OrderByDescending(s => s.Name);
            int pageSize = 4;
            int pageNumber = 1;
            ViewBag.CountDown = db.CountDown.FirstOrDefault();

            return View(items.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult RenderImage(int nbr)
       {
            byte[] photo= System.IO.File.ReadAllBytes(HttpContext.Server.MapPath(@"~/img/no-image.jpg"));
            if (db.Catagories.Count() >= 3) { 
            Catagorie item = db.Catagories.OrderBy(c => c.ID).Skip(nbr).Take(1).FirstOrDefault();
                 photo = item.Image;
            }


            return File(photo, "image/png");

        }
        public async Task<ActionResult> RenderProductImage(int id)
        {
            Item item = await db.Items.FindAsync(id);

            byte[] photo = item.InternalImage;


            return File(photo, "image/png");

        }

     
    }
}
