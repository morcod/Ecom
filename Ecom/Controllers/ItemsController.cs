using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using Ecom.Models;

namespace Ecom.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Items
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
           
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Items
                           select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Catagorie.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Brand.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.Price);
                    break;
                default:  // Name ascending 
                    items = items.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View( items.ToPagedList(pageNumber, pageSize));


         
        }

        // GET: Items/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewModelCatagorie vmc = new ViewModelCatagorie
            {
                Categories = db.Catagories.Where(x => x.ParentID == null).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList(),

                ChildCategories = db.Catagories.Where(x => x.ParentID != null).Select(x => new SelectListItem
                {
                    Text = db.Catagories.Where(a => a.ID == x.ParentID).FirstOrDefault().Name + " > " + x.Name,
                    Value = x.ID.ToString()
                }).ToList(),

            };
            if(db.Catagories.Count()!=0 && db.Sizes.Count()!=0 && db.Brands.Count() != 0)
            {
                ViewBag.vld = true;
            }
            else {
                ViewBag.vld = false;
            }
            ViewBag.BrandId = new SelectList(db.Brands, "ID", "Name");
            ViewBag.SizeId = new SelectList(db.Sizes, "ID", "Name");
            ViewBag.CatagorieId = new SelectList(vmc.AllCategories, "Value", "Text");

            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Item item)
        {
            ViewModelCatagorie vmc = new ViewModelCatagorie
            {
                Categories = db.Catagories.Where(x => x.ParentID == null).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList(),

                ChildCategories = db.Catagories.Where(x => x.ParentID != null).Select(x => new SelectListItem
                {
                    Text = db.Catagories.Where(a => a.ID == x.ParentID).FirstOrDefault().Name + " > " + x.Name,
                    Value = x.ID.ToString()
                }).ToList(),

            };
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (db.Catagories.Count() != 0 && db.Sizes.Count() != 0 && db.Brands.Count() != 0)
            {
                ViewBag.vld = true;
            }
            else
            {
                ViewBag.vld = false;
            }
            ViewBag.CatagorieId = new SelectList(vmc.AllCategories, "Value", "Text");
            ViewBag.BrandId = new SelectList(db.Brands, "ID", "Name", item.BrandId);
            ViewBag.SizeId = new SelectList(db.Sizes, "ID", "Name", item.SizeId);

            return View(item);
        }

        // GET: Items/Edit/5
         [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            ViewModelCatagorie vmc = new ViewModelCatagorie
            {
                //Category =,
                Categories = db.Catagories.Where(x => x.ParentID == null).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ID.ToString()
                }).ToList(),

                ChildCategories = db.Catagories.Where(x => x.ParentID != null).Select(x => new SelectListItem
                {
                    Text = db.Catagories.Where(a => a.ID == x.ParentID).FirstOrDefault().Name + " > " + x.Name,
                    Value = x.ID.ToString()
                }).ToList(),

            };
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            if (db.Catagories.Count() != 0 && db.Sizes.Count() != 0 && db.Brands.Count() != 0)
            {
                ViewBag.vld = true;
            }
            else
            {
                ViewBag.vld = false;
            }
            ViewBag.BrandId = new SelectList(db.Brands, "ID", "Name", item.BrandId);
            ViewBag.SizeId = new SelectList(db.Sizes, "ID", "Name", item.SizeId);
            ViewBag.CatagorieId = new SelectList(vmc.AllCategories, "Value", "Text", item.CatagorieId);


            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Item item, HttpPostedFileBase file,HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                if (file == null)
                {
                    db.Entry(item).Property(x => x.InternalImage).IsModified = false;
                }
                if (file2 == null)
                {
                    db.Entry(item).Property(x => x.InternalImage2).IsModified = false;
                }
                if (file3 == null)
                {
                    db.Entry(item).Property(x => x.InternalImage3).IsModified = false;
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (db.Catagories.Count() != 0 && db.Sizes.Count() != 0 && db.Brands.Count() != 0)
            {
                ViewBag.vld = true;
            }
            else
            {
                ViewBag.vld = false;
            }
            ViewBag.CatagorieId = new SelectList(db.Catagories, "ID", "Name", item.CatagorieId);
            ViewBag.BrandId = new SelectList(db.Brands, "ID", "Name", item.BrandId);
            ViewBag.SizeId = new SelectList(db.Sizes, "ID", "Name", item.SizeId);


            return View(item);
        }

        // GET: Items/Delete/5
         [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Item item = await db.Items.FindAsync(id);
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RenderImage(int id)
        {
            Item item = await db.Items.FindAsync(id);

            byte[] photo = item.InternalImage;
         

            return File(photo, "image/png");
        }
        public async Task<ActionResult> RenderImage2(int id)
        {
            Item item = await db.Items.FindAsync(id);

            byte[] photo2 = item.InternalImage2;

            return File(photo2, "image/png");
        }
        public async Task<ActionResult> RenderImage3(int id)
        {
            Item item = await db.Items.FindAsync(id);

            byte[] photo3 = item.InternalImage3;
            

            return File(photo3, "image/png");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
