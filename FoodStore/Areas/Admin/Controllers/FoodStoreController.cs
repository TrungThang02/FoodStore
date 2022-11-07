using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodStore.Models;
namespace FoodStore.Areas.Admin.Controllers
{
    public class FoodStoreController : Controller
    {
        // GET: Admin/Home
        FoodStoreEntities db = new FoodStoreEntities();
        public ActionResult Index()
        {
            return View();
        }

        
        
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var sTenDN = f["Username"];
            var sMatKhau = f["Password"];
            ADMIN ad = db.ADMIN.SingleOrDefault(n => n.UserName == sTenDN && n.Password == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "FoodStore");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập và mật khẩu không đúng !";
            }
            return View();
        }

    }
}