using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demo2.Areas.Admin.Controllers
{
    public class TrangChuAdminController : Controller
    {
        // GET: Admin/TrangChuAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TrangChuAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TrangChuAdmin(string email, string password)
        {
            if (email == "admin@gmail.com" && password == "123456")
            {
                Session.Add("user", email);
                return RedirectToAction("AdminSanPham", "Admin");
            }
            else
            {
                return View();
            }
        }
    }
}