using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecom.Models;

namespace Ecom.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CountDownsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        

    

        // GET: CountDowns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountDown countDown = db.CountDown.Find(id);
            if (countDown == null)
            {
                return HttpNotFound();
            }
            return View(countDown);
        }

        // POST: CountDowns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Message,CountDownDate")] CountDown countDown)
        {
            if (ModelState.IsValid)
            {
                db.Entry(countDown).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = 2 });
            }
            return View(countDown);
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
