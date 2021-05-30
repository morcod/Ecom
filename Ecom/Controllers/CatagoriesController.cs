using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecom.Models;
using PagedList;

namespace Ecom.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CatagoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Catagories
        public ActionResult Index(int? page) 
        {
            var items = from i in db.Catagories
                        select i;
            items = items.OrderBy(s => s.Name);

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Catagories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagorie catagorie = await db.Catagories.FindAsync(id);
            if (catagorie == null)
            {
                return HttpNotFound();
            }
            return View(catagorie);
        }

        // GET: Catagories/Create
         [Authorize(Roles = "Admin")]
        public ActionResult Create()

        {
            List<SelectListItem> catLst = new SelectList(db.Catagories, "ID", "Name").ToList();
            catLst.Insert(0, (new SelectListItem { Text = "--None--", Value = "-1" }));
            ViewBag.ParentID = catLst;

            return View();
        }

        // POST: Catagories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create( Catagorie catagorie)
        {
            if (ModelState.IsValid)
            {
                if (int.Parse(Request.Form["ddlParent"]) == -1)
                {
                    catagorie.ParentID = null;
                    catagorie.ParentName = null;
                }
                else
                {
                    Catagorie c = await db.Catagories.FindAsync(int.Parse(Request.Form["ddlParent"]));
                    catagorie.ParentID = int.Parse(Request.Form["ddlParent"]);
                    catagorie.ParentName = c.Name;
                    c.isParent = true;

                }

                db.Catagories.Add(catagorie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(db.Catagories, "ID", "Name", catagorie.ParentID);

            return View(catagorie);
        }

        // GET: Catagories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagorie catagorie = await db.Catagories.FindAsync(id);
            if (catagorie == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> catLst = new SelectList(db.Catagories, "ID", "Name").ToList();
            catLst.Insert(0, (new SelectListItem { Text = "--None--", Value = "-1" }));
            catLst.RemoveAll(r => r.Value == id.ToString());
            catLst.RemoveAll(r => r.Value == catagorie.ParentID.ToString());
            ViewBag.ParentID = catLst;
            return View(catagorie);
        }

        // POST: Catagories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Catagorie catagorie)
        {
            if (ModelState.IsValid)
            {
                if (int.Parse(Request.Form["ddlParent"]) == -1)
                {
                    if (catagorie.ParentID != null) {
                        Catagorie c1 = await db.Catagories.FindAsync(catagorie.ParentID);
                        c1.isParent = false; 
                    }
                    catagorie.ParentID = null;
                    catagorie.ParentName = null;
                }
                else
                {

                    Catagorie c = await db.Catagories.FindAsync(int.Parse(Request.Form["ddlParent"]));
                    catagorie.ParentID = int.Parse(Request.Form["ddlParent"]);
                    catagorie.ParentName =c.Name;
                    c.isParent = true;

                }
                db.Entry(catagorie).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(catagorie);
        }
        public async Task<ActionResult> RenderImage(int id)
        {
            Catagorie item = await db.Catagories.FindAsync(id);

            byte[] photo = item.Image;


            return File(photo, "image/png");
        }



        // GET: Catagories/Delete/5
        public async Task<ActionResult> Delete(int ? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagorie catagorie = await db.Catagories.FindAsync(id);
            if (catagorie == null)
            {
                return HttpNotFound();
            }
            Catagorie cat = await db.Catagories.FindAsync(catagorie.ParentID);
            if (cat != null)
            {
                ViewBag.ParentCat = cat.Name;
            }
            return View(catagorie);
        }

        // POST: Catagories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        { Catagorie catagorie = await db.Catagories.FindAsync(id);
            if (catagorie.ParentID == null)
            {
                db.Catagories.Remove(catagorie);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
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
