using FoodStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodStore.Controllers
{
    public class ProductsController : Controller
    {
        FoodStoreEntities db = new FoodStoreEntities();
        // GET: Products
        public ActionResult Index()
        {
            var dac = from d in db.Products select d;
            return View(dac);
        }
       
    }
}