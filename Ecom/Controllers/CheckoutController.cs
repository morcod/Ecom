using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ecom.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ecom.ViewModel;

namespace Ecom.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();


        public List<String> CreditCardTypes;

        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            CreditCardTypes = new List<String>();
            CreditCardTypes.Add("Visa");
            CreditCardTypes.Add("MasterCard");
            ViewBag.CreditCardTypes = CreditCardTypes;
            var previousOrder = storeDB.Orders.FirstOrDefault(x => x.Username == User.Identity.Name);

            if (previousOrder != null)
                return View(previousOrder);
            else
                return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(FormCollection values)
        {
            ViewBag.CreditCardTypes = CreditCardTypes;
            string result = values[4];

            var order = new Models.Order();
            TryUpdateModel(order);
            order.CreditCard = result;

            try
            {
                order.Username = User.Identity.Name;
                order.Email = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                var currentUserId = User.Identity.GetUserId();
                order.SaveInfo = true;


                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var ctx = store.Context;
                var currentUser = manager.FindById(User.Identity.GetUserId());




                await ctx.SaveChangesAsync();

                await storeDB.SaveChangesAsync();



                storeDB.Orders.Add(order);
                await storeDB.SaveChangesAsync();
                var cart = ShoppingCart.GetCart(this.HttpContext);
                order = cart.CreateOrder(order);
              




                return RedirectToAction("Complete",
                    new { id = order.OrderId,ttl=order.Total,crd=result });

            }
            catch
            {
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id,decimal ttl,string crd)
        {
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                Order ord=  storeDB.Orders.Find(id);
                ord.Total = ttl;
                ord.CreditCard = crd;
                storeDB.Orders.Attach(ord);
                storeDB.Entry(ord).Property(x => x.Total).IsModified = true;
                try
                {
                    storeDB.SaveChanges();
                }
                catch (Exception e)
                {
                }
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }


    }
}