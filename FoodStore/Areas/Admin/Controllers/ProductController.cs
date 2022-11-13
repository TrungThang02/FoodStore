using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodStore.Models;

namespace FoodStore.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        FoodStoreEntities db = new FoodStoreEntities();

        // GET: Admin/Product



        public JsonResult Index()
        {
            var product = db.Product.Include(p => p.Category);
            return Json(JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product p, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //Đưa dữ liệu vào dropdown
            ViewBag.MaNXB = new SelectList(db.Category.ToList().OrderBy(n => n.CategoryName), "CategoryId", "CategoryName");


            if (fFileUpload == null)
            {
                //Thông báo yêu cầu chọn ảnh bìa
                ViewBag.ThongBao = "Hãy chọn ảnh !";
                //Lưu thông tin

                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.DonGia = decimal.Parse(f["mDonGia"]);

                ViewBag.MaNXB = new SelectList(db.Category.ToList().OrderBy(n => n.CategoryName), "CategoryId", "CategoryName");


                return View();

            }
            else
            {
                if (ModelState.IsValid)
                {
                    //lấy tên file, khai báo thư viện(System IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), sFileName);
                    //Kiểm tra ảnh đã được tải lên chưa
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    //Lưu sách vào cơ sở dử liệu
                    p.ProductName = f["sTenSach"];
                    p.Description = f["sMoTa"].Replace("<p>", "").Replace("<p>", "\n");
                    p.Image = sFileName;
                
                    p.Price = decimal.Parse(f["mDonGia"]);
                    p.CategoryId = int.Parse(f["MaCD"]);
                   
                    db.Product.Add(p);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            var sach = db.Product.SingleOrDefault(n => n.ProductId == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        public JsonResult Delete(int id)
        {
            try
            {
                var p = db.Product.SingleOrDefault(x => x.ProductId == id);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa thất bại!" }, JsonRequestBehavior.AllowGet);
            }
        }


        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirm(int id, FormCollection f)
        //{
        //    var sach = db.SACH.SingleOrDefault(n => n.MaSach == id);

        //    if (sach == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }

        //    var ctdh = data.CTDATHANG.Where(ct => ct.MaSach == id);
        //    if (ctdh.Count() > 0)
        //    {
        //        ViewBag.ThongBao = "Sách này đang có trang bảng chi tiết đặt hàng<br>" + "Nếu muốn xóa thì phải xóa hết mã sách này trong chi tiết đặt hàng";
        //        return View(sach);

        //    }

        //    var vietsach = data.VIETSACH.Where(vs => vs.MaSach == id).ToList();
        //    if (vietsach == null)
        //    {

        //        var vs = data.VIETSACH.Find(id);
        //        data.VIETSACH.Remove(vs);
        //        data.SaveChanges();


        //    }

        //    data.SACH.Remove(sach);
        //    data.SaveChanges();

        //    return RedirectToAction("Index");
        //}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var p = db.Product.SingleOrDefault(n => n.ProductId == id);
            if (p == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
               
                ViewBag.MaNXB = new SelectList(db.Category.ToList().OrderBy(n => n.CategoryName), "CategoryId", "CategoryName");

                return View(p);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var p = db.Product.AsEnumerable().SingleOrDefault(n => n.ProductId == int.Parse(f["iMaSach"]));

            ViewBag.MaNXB = new SelectList(db.Category.ToList().OrderBy(n => n.CategoryName), "CategoryId", "CategoryName");

            if (ModelState.IsValid)
            {
                //lấy tên file, khai báo thư viện(System IO)
                var sFileName = Path.GetFileName(fFileUpload.FileName);
                //Lấy đường dẫn lưu file
                var path = Path.Combine(Server.MapPath("~/Content/Images"), sFileName);
                //Kiểm tra ảnh đã được tải lên chưa
                if (!System.IO.File.Exists(path))
                {
                    fFileUpload.SaveAs(path);
                }
                //Lưu sách vào cơ sở dử liệu
                p.ProductName = f["sTenSach"];
                p.Description = f["sMoTa"].Replace("<p>", "").Replace("<p>", "\n");
                p.Image = sFileName;

                p.Price = decimal.Parse(f["mDonGia"]);
                p.CategoryId = Convert.ToInt32(f["MaNXB"]);

                db.Product.Add(p);
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            return View(p);

        }

    }

}

