
using FoodStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodStore.helper;
using System.Configuration;

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



        public JsonResult ThemGioHang(int idproduct)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.productId == idproduct);
            if (sp == null)
            {
                sp = new GioHang(idproduct);
                lstGioHang.Add(sp);
            }

            return Json(new { item = sp, success = true }, JsonRequestBehavior.AllowGet);
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
            return dTongTien + 30000;
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
        public double TongTienHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.totalPrice);
            }
            return dTongTien;
        }

        public double PhiShip()
        {
            double PhiShip = 30000;
            return PhiShip;
        }





        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            
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



        public JsonResult CapNhatGioHang(int id, int quantity)     //truy cập sử dụng Url
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.productId == id);
            sp.productQuantity = quantity;
            return Json(new { item = sp }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult XoaSPKhoiGioHang(int productId)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.productId == productId);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.productId == productId);
                //if (lstGioHang.Count == 0)
                //{
                //    return RedirectToAction("Index", "SACHes");
                //}
            }
            return Json(new { item = sp }, JsonRequestBehavior.AllowGet);
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
                return Redirect("~/User/DangNhap");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Products");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien() + 30000 ;
            ViewBag.PhiShip = PhiShip();
            ViewBag.TongTienHang = TongTienHang();
            return View(lstGioHang);




        }



        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            var order = from d in db.Customer
                       join b in db.Orders
                       on d.CustomerId equals b.CustomerId
                       select new {
                           OrderDate = DateTime.Now,
                            address = b.Address,
                               mobile = d.Phone,
                            shipName = d.CustomerName,
                            email = d.Email

        };



            Product p = new Product();
            Orders ddh = new Orders();
            OrderDetail ct = new OrderDetail();
            Customer kh = (Customer)Session["cmt"];
            List<GioHang> lstGioHang = LayGioHang();
            //.NullReferenceException
            if (kh.CustomerId != null)
            {
                try
                {
                    
                    ddh.CustomerId = kh.CustomerId;
                    
                    ddh.OrderDate = DateTime.Now;
                    ddh.Address = kh.Address;
                    ddh.RecipientPhone = kh.Phone;
               
                    var NgayGiao = String.Format("{0:MM/mm/yyyy}", f["NgayGiao"]);
                    ddh.DeliveryDate = DateTime.Parse(NgayGiao);


                    var giatien = ct.Price ;
                    ddh.OrderPrice = giatien;

                    



                    db.Orders.Add(ddh);
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
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/neworder.html"));

            content = content.Replace("{{CustomerName}}", kh.CustomerName);
            content = content.Replace("{{Phone}}", kh.Phone);
            content = content.Replace("{{Email}}", kh.Email);
            content = content.Replace("{{Address}}", ddh.Address);
            var lstgiohang = LayGioHang();
            var noidung = "";
            foreach (var item in lstgiohang)
            {
                noidung += item.productName + ", ";
            }
            content = content.Replace("{{Product}}", noidung);
            content = content.Replace("{{Total}}", getTongTien().ToString("N0"));
            var toEmail = kh.Email;

            // Để Gmail cho phép SmtpClient kết nối đến server SMTP của nó với xác thực 
            //là tài khoản gmail của bạn, bạn cần thiết lập tài khoản email của bạn như sau:
            //Vào địa chỉ https://myaccount.google.com/security  Ở menu trái chọn mục Bảo mật, sau đó tại mục Quyền truy cập 
            //của ứng dụng kém an toàn phải ở chế độ bật
            //  Đồng thời tài khoản Gmail cũng cần bật IMAP
            //Truy cập địa chỉ https://mail.google.com/mail/#settings/fwdandpop

            new Mail().SendMail(toEmail, "Đơn hàng mới từ TDT Food", content);


            db.SaveChanges();

            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");

           



        }

        public ActionResult ThanhToan()
        {
            return View();
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }

        public ActionResult LichSuMuaHang(int? id)
        {
            var lsmh = from s in db.Orders where s.CustomerId == id select s;
            return View(lsmh);
        }

        public ActionResult Quanlidonhang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "CustomerName", order.CustomerId);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quanlidonhang([Bind(Include = "OrderId,CustomerId,OrderDate,DeliveryDate,Address,Recipient,RecipientPhone,OrderState,OrderPrice")] Orders order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "CustomerName", order.CustomerId);
            return View(order);
        }
    }
}
