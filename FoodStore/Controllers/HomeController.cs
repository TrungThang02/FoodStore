using FoodStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodStore.Controllers
{
    
    public class HomeController : Controller
    {
        FoodStoreEntities db = new FoodStoreEntities();
        public ActionResult Index()
        {
            return View();
        }
       public ActionResult NavbarPartial()
        {
           
            return PartialView();
        }
        public ActionResult NavigationPartial()
        {
            var danhmuc = from c in db.Category select c;
            ViewBag.danhmuc = danhmuc;
            return PartialView();
        }
        public ActionResult BannerPartial()
        {
            return PartialView();
        }
        public ActionResult TopSellingPartial()
        {
            return PartialView();
        }
        public ActionResult TabCategoryPartial()
        {
            return PartialView();
        }
        public ActionResult TimKiem(String search = "")
        {
            var dac = from d in db.Product select d;
            List<Product> products = db.Product.Where(p => p.ProductName.Contains(search)).ToList();
            ViewBag.search = search;
            return View(products);
        }
    }
}