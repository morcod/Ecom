using Ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ecom.Controllers
{
    public class StoreController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();

        //
        // GET: /Store/

        public ActionResult Index()
        {
            var catagories = storeDB.Catagories.ToList();
           

            return View(catagories);
        }
        public async Task<ActionResult> RenderImage(int id)
        {
            Catagorie item = await storeDB.Catagories.FindAsync(id);

            byte[] photo = item.Image;


            return File(photo, "image/png");
        }
        public async Task<ActionResult> RenderItemImage(int id)
        {
            Item item = await storeDB.Items.FindAsync(id);

            byte[] photo = item.InternalImage;


            return File(photo, "image/png");
        }
        //
        // GET: /Store/Browse?genre=Disco

        public ActionResult Browse(int id)
        {
            var catagorieModel = storeDB.Catagories.Include("Items")
                .Single(g => g.ID == id);

            return View(catagorieModel);
        }

        //
        // GET: /Store/Details/5
        public ActionResult Details(int id)
        {
            var item = storeDB.Items.Find(id);

            return View(item);
        }

      
    }
}