using FoodStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace FoodStore.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        private FoodStoreEntities db = new FoodStoreEntities();



        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }



        public ActionResult ThemGioHang(int idproduct, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.productId == idproduct);
            if (sp == null)
            {
                sp = new GioHang(idproduct);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.productQuantity++;
            }
            return Redirect(url);
        }



        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.productQuantity);
            }
            return iTongSoLuong;
        }



        static private double dTongTien = 0;



        public double getTongTien()
        {
            return dTongTien;
        }
        public double TongTien()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.totalPrice);
            }
            return dTongTien;
        }







        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Products");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }



        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }



        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.productId == id);
            if (sp != null)
            {
                sp.productQuantity = int.Parse(collection["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }



        public ActionResult XoaSPKhoiGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.productId == iMaSach);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.productId == iMaSach);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "SACHes");
                }
            }
            return RedirectToAction("GioHang");
        }




        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "SACHes");
        }



        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap?id=2");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Products");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);




        }



        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            Order ddh = new Order();
            Customer kh = (Customer)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            //.NullReferenceException
            if (kh.CustomerId != null)
            {
                try
                {
                    ddh.CustomerId = kh.CustomerId;
                    ddh.DeliveryDate = DateTime.Now;

                    var NgayGiao = String.Format("{0:MM/mm/yyyy}", f["NgayGiao"]);




                    db.Order.Add(ddh);
                    db.SaveChanges();
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                }
            }



            foreach (var item in lstGioHang)
            {
                OrderDetail ctdh = new OrderDetail();
                ctdh.OrderId = ddh.OrderId;
                ctdh.ProductId = item.productId;
                ctdh.Quantity = item.productQuantity;
                ctdh.Price = (decimal)item.productPrice;
                db.OrderDetail.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");



        }



        public ActionResult XacNhanDonHang()
        {
            return View();
        }




    }
}
