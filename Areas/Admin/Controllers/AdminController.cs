using demo2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace demo2.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        doanEntities database = new doanEntities();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminSanPham(string _name)
        {
            if (_name == null)
                return View(database.Products.ToList());
            else
                return View(database.Products.Where(s => s.TenSP.Contains(_name)).ToList());
        }

        public ActionResult Create()
        {
            Product sp = new Product();
            return View(sp);
        }
        [HttpPost]
        public ActionResult Create(Product sp)
        {
            try
            {
                if (sp.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(sp.UploadImage.FileName);
                    string extent = Path.GetExtension(sp.UploadImage.ToString());
                    filename = filename + extent;
                    sp.HinhanhSP = "~/Images/" + filename;
                    sp.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Images/"), filename));
                }
                database.Products.Add(sp);
                database.SaveChanges();
                return RedirectToAction("AdminSanPham");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(database.Products.Where(s => s.IDSP == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, Product cate)
        {
            database.Entry(cate).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("AdminSanPham");
        }

        public ActionResult Delete(int id)
        {
            return View(database.Products.Where(s => s.IDSP == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Product cate)
        {
            cate = database.Products.Where(s => s.IDSP == id).FirstOrDefault();
            database.Products.Remove(cate);
            database.SaveChanges();
            return RedirectToAction("AdminSanPham");
        }
    }
}