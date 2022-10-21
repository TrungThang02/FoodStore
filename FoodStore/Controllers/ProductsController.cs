using FoodStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace FoodStore.Controllers
{
    public class ProductsController : Controller
    {
        FoodStoreEntities db = new FoodStoreEntities();
        // GET: Products
        public ActionResult Index()
        {
            var dac = from d in db.Product select d;
            return View(dac);
        }
        public ActionResult DoChay()
        {
            return View();
        }
       
    }
}