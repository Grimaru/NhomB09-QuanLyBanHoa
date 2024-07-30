using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using demo2.Models;
using demo2.ViewModels;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace demo2.Areas.Admin.Controllers
{
    public class DetailProductsController : Controller
    {
        private doanEntities db = new doanEntities();

        // GET: Admin/DetailProducts
        public ActionResult Index()
        {
            // Fetch DetailProducts with relevant related data (e.g., Product, OrderProduct) if needed
            var detailProducts = db.DetailProducts
                                   .Include(d => d.Product)  // Ensure related entities are loaded
                                   .Include(d => d.OrderProduct)
                                   .ToList();  // Make sure to call ToList()

            var reportOrderViewModel = new ReportOrderViewModel
            {
                TenSanPham = db.Products.Select(p => p.TenSP).ToList(),
                SoLuongSanPham = db.DetailProducts
                                      .GroupBy(dp => dp.IDSP)
                                      .Select(g => g.Sum(dp => dp.Soluong) ?? 0)
                                      .ToList(),
                TongDoanhThu = (decimal)db.DetailProducts
                                      .GroupBy(dp => dp.IDSP)
                                      .Select(g => new
                                      {
                                        TongSoLuong = g.Sum(dp => dp.Soluong),
                                        TongGia = g.Select(dp => dp.Product.GiaSP).FirstOrDefault()
                                      })
                                 .Select(x => x.TongSoLuong * x.TongGia)
                                 .Sum(),
                KhachHangTop1 = db.OrderProducts
                                    .GroupBy(op => op.User.TKND)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => g.Key)
                                    .FirstOrDefault(),
                SanPhamBestSeller = db.DetailProducts
                                   .GroupBy(dp => dp.Product.TenSP)
                                   .OrderByDescending(g => g.Sum(dp => dp.Soluong))
                                   .Select(g => g.Key)
                                   .FirstOrDefault(),
                SoLuongBestSeller = db.DetailProducts
                                     .GroupBy(dp => dp.IDSP)
                                     .OrderByDescending(g => g.Sum(dp => dp.Soluong))
                                     .Select(g => g.Sum(dp => dp.Soluong) ?? 0)
                                     .FirstOrDefault(),
                DetailProducts = detailProducts  // Ensure this is correctly assigned
            };

            return View(reportOrderViewModel);
        }

        // GET: Admin/DetailProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailProduct detailProduct = db.DetailProducts.Find(id);
            if (detailProduct == null)
            {
                return HttpNotFound();
            }
            return View(detailProduct);
        }

        // GET: Admin/DetailProducts/Create
        public ActionResult Create()
        {
            ViewBag.IDOP = new SelectList(db.OrderProducts, "IDOP", "Diachigiaohang");
            ViewBag.IDSP = new SelectList(db.Products, "IDSP", "TenSP");
            ViewBag.IDVoucher = new SelectList(db.Vouchers, "IDVoucher", "CodeVoucher");
            return View();
        }

        // POST: Admin/DetailProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDSP,IDOP,Soluong,TongTien,IDVoucher")] DetailProduct detailProduct)
        {
            if (ModelState.IsValid)
            {
                db.DetailProducts.Add(detailProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDOP = new SelectList(db.OrderProducts, "IDOP", "Diachigiaohang", detailProduct.IDOP);
            ViewBag.IDSP = new SelectList(db.Products, "IDSP", "TenSP", detailProduct.IDSP);
            ViewBag.IDVoucher = new SelectList(db.Vouchers, "IDVoucher", "CodeVoucher", detailProduct.IDVoucher);
            return View(detailProduct);
        }

        // GET: Admin/DetailProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailProduct detailProduct = db.DetailProducts.Find(id);
            if (detailProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDOP = new SelectList(db.OrderProducts, "IDOP", "Diachigiaohang", detailProduct.IDOP);
            ViewBag.IDSP = new SelectList(db.Products, "IDSP", "TenSP", detailProduct.IDSP);
            ViewBag.IDVoucher = new SelectList(db.Vouchers, "IDVoucher", "CodeVoucher", detailProduct.IDVoucher);
            return View(detailProduct);
        }

        // POST: Admin/DetailProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDSP,IDOP,Soluong,TongTien,IDVoucher")] DetailProduct detailProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detailProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDOP = new SelectList(db.OrderProducts, "IDOP", "Diachigiaohang", detailProduct.IDOP);
            ViewBag.IDSP = new SelectList(db.Products, "IDSP", "TenSP", detailProduct.IDSP);
            ViewBag.IDVoucher = new SelectList(db.Vouchers, "IDVoucher", "CodeVoucher", detailProduct.IDVoucher);
            return View(detailProduct);
        }

        // GET: Admin/DetailProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailProduct detailProduct = db.DetailProducts.Find(id);
            if (detailProduct == null)
            {
                return HttpNotFound();
            }
            return View(detailProduct);
        }

        // POST: Admin/DetailProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetailProduct detailProduct = db.DetailProducts.Find(id);
            db.DetailProducts.Remove(detailProduct);
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

        public ActionResult XuatHoaDon(int? id)
        {
            var detailProduct = db.DetailProducts.Find(id);
            if (detailProduct == null)
            {
                return new HttpNotFoundResult("Detail product not found.");
            }

            using (var doc = DocX.Create(new MemoryStream()))
            {
                doc.InsertParagraph("Hóa Đơn").FontSize(20).Bold().Alignment = Alignment.center;

                doc.InsertParagraph($"Product: {detailProduct.Product.TenSP}").FontSize(12);
                doc.InsertParagraph($"Quantity: {detailProduct.Soluong}").FontSize(12);
                doc.InsertParagraph($"Total Price: {detailProduct.TongTien:N0}đ").FontSize(12);

                if (!string.IsNullOrEmpty(detailProduct.IDVoucher.ToString()))
                {
                    doc.InsertParagraph($"Discount Code: {detailProduct.IDVoucher}").FontSize(12);
                }

                var content = new MemoryStream();
                doc.SaveAs(content);
                content.Position = 0;

                return File(content.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Hóa đơn.docx");
            }
        }
    }
}
