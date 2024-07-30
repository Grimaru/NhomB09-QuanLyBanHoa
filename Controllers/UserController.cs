using demo2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demo2.Controllers
{
    public class UserController : Controller
    {
        doanEntities database = new doanEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAccount(User _user)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(_user.EmailND))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(_user.MatKhauND))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    var khachhang = database.Users.FirstOrDefault(s => s.EmailND == _user.EmailND && s.MatKhauND == _user.MatKhauND);
                    if (khachhang != null)
                    {
                        Session["TKND"] = khachhang.EmailND;
                        return RedirectToAction("SanPham", "Product");
                    }
                    else if (khachhang.TKND == "admin@gmail.com" && khachhang.MatKhauND == "123456")
                    {
                        return RedirectToAction("AdminSanPham", "Admin");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                        return View("DangNhap");
                    }
                }
            }
            return RedirectToAction("SanPham", "Product");
        }

        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(User _user)
        {          
            try
            {
                database.Users.Add(_user);
                database.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            catch
            {
                return View("DangKy");
            }
        }

        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("DangNhap");
        }

    }
}