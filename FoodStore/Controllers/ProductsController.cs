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
            int c = 1;
            var dc = from s in db.Category
                     join p in db.Product on s.CategoryId equals p.CategoryId
                     where p.CategoryId == c
                     select p;
            return View(dc);

        }
        public ActionResult DoAnNhanh()
        {
            int c = 2;
            var dan = from s in db.Category
                      join p in db.Product on s.CategoryId equals p.CategoryId
                      where p.CategoryId == c
                      select p;
            return View(dan);

        }
        public ActionResult DoUong()
        {
            int c = 3;
            var du = from s in db.Category
                     join p in db.Product on s.CategoryId equals p.CategoryId
                     where p.CategoryId == c
                     select p;
            return View(du);

        }
        public ActionResult Lau()
        {
            int c = 4;
            var l = from s in db.Category
                    join p in db.Product on s.CategoryId equals p.CategoryId
                    where p.CategoryId == c
                    select p;
            return View(l);

        }
        public ActionResult HaiSan()
        {
            int c = 5;
            var hs = from s in db.Category
                     join p in db.Product on s.CategoryId equals p.CategoryId
                     where p.CategoryId == c
                     select p;
            return View(hs);

        }
        public ActionResult HoaQua()
        {
            int c = 6;
            var hq = from s in db.Category
                     join p in db.Product on s.CategoryId equals p.CategoryId
                     where p.CategoryId == c
                     select p;
            return View(hq);

        }
        public ActionResult DoNuong()
        {
            int c = 7;
            var dn = from s in db.Category
                     join p in db.Product on s.CategoryId equals p.CategoryId
                     where p.CategoryId == c
                     select p;
            return View(dn);

        }

        
        public ActionResult ChiTIetSanPham(int? id)
        {
            var ctsp = from s in db.Product where s.ProductId == id select s;
            return View(ctsp);
        }
    }
}