using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using demo2.Models;

namespace demo2.Areas.Admin.Controllers
{
    public class OrderProductsController : Controller
    {
        private doanEntities db = new doanEntities();

        // GET: Admin/OrderProducts
        public ActionResult Index()
        {
            var orderProducts = db.OrderProducts.Include(o => o.User);
            return View(orderProducts.ToList());
        }

        // GET: Admin/OrderProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }

        // GET: Admin/OrderProducts/Create
        public ActionResult Create()
        {
            ViewBag.IDUser = new SelectList(db.Users, "IDUser", "TKND");
            return View();
        }

        // POST: Admin/OrderProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOP,NgayDH,IDUser,Diachigiaohang")] OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                db.OrderProducts.Add(orderProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUser = new SelectList(db.Users, "IDUser", "TKND", orderProduct.IDUser);
            return View(orderProduct);
        }

        // GET: Admin/OrderProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }

            // Cập nhật ViewBag với danh sách người dùng để hiển thị trong dropdown
            ViewBag.IDUser = new SelectList(db.Users, "IDUser", "TenND", orderProduct.IDUser);

            // Cập nhật ViewBag với tên người dùng để hiển thị trên view nếu cần
            ViewBag.UserName = db.Users.Where(u => u.IDUser == orderProduct.IDUser).Select(u => u.TenND).FirstOrDefault();

            return View(orderProduct);
        }


        // POST: Admin/OrderProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOP,NgayDH,IDUser,TenND,Diachigiaohang,CodeVoucher")] OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Cập nhật ViewBag với danh sách người dùng để hiển thị trong dropdown
            ViewBag.IDUser = new SelectList(db.Users, "IDUser", "TenND", orderProduct.IDUser);

            return View(orderProduct);
        }

        // GET: Admin/OrderProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }

        // POST: Admin/OrderProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            db.OrderProducts.Remove(orderProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
