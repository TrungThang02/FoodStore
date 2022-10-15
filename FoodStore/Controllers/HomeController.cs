using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodStore.Controllers
{
    public class HomeController : Controller
    {
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

    }
}