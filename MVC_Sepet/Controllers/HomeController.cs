using MVC_Sepet.Models;
using MVC_Sepet.Utils;
using MVC_Sepet.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Sepet.Controllers
{
    public class HomeController : Controller
    {
        NorthwindEntities db=new NorthwindEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Products.Where(x => x.CategoryID != null && x.UnitsInStock>0).ToList());
        }
        public ActionResult AddToCart(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                Cart c = null;
                if (Session["scart"] == null)
                {
                    c = new Cart();
                }
                else
                {
                    c = Session["scart"] as Cart;
                }
                CartItem ci=new CartItem();
                ci.Id = product.ProductID;
                ci.ProductName = product.ProductName;
                ci.Price = product.UnitPrice;
                c.AddItem(ci);
                Session["scart"] = c;
                return RedirectToAction("Index");

            }
            catch (System.Exception)
            {
                TempData["error"] = $"{id} karşılık gelen bir ürün bulunamadı";
                return View();
            }
        }
        public ActionResult MyCart()
        {
            if (Session["scart"]!=null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "sepetinde ürün bulunmaktadır";
                return RedirectToAction("Index");
            }
        }

        public ActionResult CompleteCart()
        {
            Cart cart = Session["scart"] as Cart;
            foreach (var item in cart.myCart)
            {
                Product product = db.Products.Find(item.Id);
                product.UnitsInStock-=Convert.ToInt16(item.Quantity);
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                ViewBag.OrderNumber = "623462";
                Session.Remove("scart");
               
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                bool result = db.Users.Any(x => x.FirstName == userVM.FirstName && x.Password == userVM.Password);
                if (result)
                {
                    User user=db.Users.Where(x=>x.FirstName==userVM.FirstName && x.Password==userVM.Password).FirstOrDefault();
                    Session["login"]=user;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Kullanıcı bilgileri hatalı";
                    return View(userVM);
                }
            }
            else
            {
                return View(userVM);
            }
           
        }

        public ActionResult Email()
        {

            MailSender.SendEmail("sevil2362@gmail.com", "Kullanıcı bilgiler","CompleteCard");

            return View();
        }

        public ActionResult Update(int id)
        {
            ProductVM pvm = new ProductVM
            {
                Product = db.Products.Find(id)
            };
                return View(pvm);
           
           
        }
        [HttpPost]
        public ActionResult Update(Product product)
        {
            Product tobeUpdated=db.Products.Find(product.ProductID);
           
            db.Entry(tobeUpdated).CurrentValues.SetValues(product);
            db.SaveChanges();
             
            return RedirectToAction("Index");
        }
         public ActionResult Delete(int id)
        {
            var product =db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}