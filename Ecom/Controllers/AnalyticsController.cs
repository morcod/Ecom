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
using Ecom.ViewModels;

namespace Ecom.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AnalyticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        AnalyticsViewModel vm = new AnalyticsViewModel();

        // GET: Analytics
        public async Task<ActionResult> Index()
        {
            

            var allData = (from orders in db.Orders
                        group orders by orders.OrderDate into dateGroup
                        select new OrderDateGroup()
                        {
                            OrderDate = dateGroup.Key,
                            OrderCount = dateGroup.Count()
                        });

            
            vm.OrderData = await allData.ToListAsync();

          

            ViewBag.ProductsCount = db.Items.Count();
            ViewBag.UserCount =db.Users.Count();
            ViewBag.CategoriesCount =db.Catagories.Count();
            ViewBag.BrandsCount =db.Brands.Count();
            ViewBag.OrdersCount =db.Orders.Count();
            if (db.OrderDetails.Sum(o => o.UnitPrice * o.Quantity) != null)
                ViewBag.SalesRevenue = db.OrderDetails.Sum(o => o.UnitPrice * o.Quantity);
            else
                ViewBag.SalesRevenue = 0;
           
            return View(vm);
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
