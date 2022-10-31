using FoodStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FoodStore.Controllers
{
    public class UserController : Controller
    {
        private FoodStoreEntities db = new FoodStoreEntities();

        public static string Encrypt(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            encrypt = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());

            }
            return encryptdata.ToString();

        }




        // GET: User/Details/5


        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, Customer kh)
        {
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = Encrypt(collection["MatKhau"].ToString());
            var sNhapLaiMatKhau = collection["MatKhauNL"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được để trống";
            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Mật khẩu không được để trống";
            }
            else if (String.IsNullOrEmpty(sNhapLaiMatKhau))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(sDiaChi))
            {
                ViewData["err5"] = "Địa chỉ không được để trống";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err6"] = "Email không được để trống";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err7"] = "Điện thoại không được để trống";
            }
            else if (db.Customer.SingleOrDefault(n => n.UserName == sTenDN) != null)
            {
                ViewData["err8"] = "Tên đăng nhâp đã tồn tại";
            }
            else if (db.Customer.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewData["err9"] = "Email đã được sử dụng";
            }
            else
            {
                try
                {
                    kh.CustomerName = sHoTen;
                    kh.UserName = sTenDN;
                    kh.Password = sMatKhau;
                    kh.Email = sEmail;
                    kh.Address = sDiaChi;
                    kh.BirthDay = DateTime.Parse(dNgaySinh);
                    kh.Phone = sDienThoai;
                    db.Customer.Add(kh);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }
                return RedirectToAction("DangNhap", "User");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            int state = Convert.ToInt32(Request.QueryString["id"]);
            var sTenDN = collection["TenDN"];
            var sMatKhau = Encrypt(collection["MatKhau"].ToString());
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err"] = "Tài khoản tên không được để trống";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err"] = "Mật khẩu không được để trống";
            }
            else
            {
                Customer kh = db.Customer.SingleOrDefault(n => n.UserName == sTenDN && n.Password == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng bạn đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    if (state == 1)
                    {
                        return RedirectToAction("Index", "Products");
                    }
                    else
                    {
                        return RedirectToAction("DatHang", "GioHang");
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";

                }
            }
            return View();
        }
        public ActionResult LoginLogOutPartial()
        {

            return PartialView();
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Products");
        }

    }
}
