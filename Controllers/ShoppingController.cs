using demo2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xceed.Document.NET;
using Xceed.Words.NET;
using static Xceed.Words.NET.DocX;


namespace demo2.Controllers
{
    public class ShoppingController : Controller
    {
        doanEntities database = new doanEntities();
        // GET: Shopping
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }

        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult AddToCart(int id)
        {
            var _pro = database.Products.SingleOrDefault(s => s.IDSP == id);
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "Shopping");
        }

        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "Shopping");
        }

        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "Shopping");
        }

        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }

        public ActionResult CheckOut(FormCollection form/*, User _user*/)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderProduct _order = new OrderProduct();
                _order.NgayDH = DateTime.Now;
                _order.Diachigiaohang = form["Diachigiaohang"];
                _order.TenND = form["TenND"];
                _order.CodeVoucher = form["CodeVoucher"]; //Bảng Order và Bảng Voucher được kết nối bởi khóa ngoại IDVoucher
                database.OrderProducts.Add(_order);
                database.SaveChanges();
                foreach (var item in cart.Items)
                {
                    DetailProduct _order_detail = new DetailProduct();
                    _order_detail.IDOP = _order.IDOP;
                    _order_detail.IDSP = item._product.IDSP;
                    _order_detail.TongTien = (double)item._product.GiaSP;
                    _order_detail.Soluong = item._quantity;
                    _order_detail.CodeVoucher = item._product.CodeVoucher;
                    database.DetailProducts.Add(_order_detail);
                }
                database.SaveChanges();
                cart.ClearCart();
                //if (Session["TKND"] == null)
                //{
                //    return RedirectToAction("DangNhap", "User");
                //}
                //else
                //{
                //    return RedirectToAction("ThanhToanThanhCong", "Shopping");
                //}
                return RedirectToAction("ThanhToanThanhCong", "Shopping");
            }
            catch
            {
                return RedirectToAction("ThanhToanThatBai", "Shopping");
            }
        }

        public ActionResult ThanhToanThanhCong()
        {
            return View();
        }

        public ActionResult ThanhToanThatBai()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GiamGiaSanPham(string voucherCode)
        {
            Cart cart = GetCart();
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("EmptyCart");
            }

            var discount = database.Vouchers.SingleOrDefault(d => d.CodeVoucher == voucherCode);
            if (discount == null)
            {
                ViewBag.DiscountMessage = "Mã khuyến mãi không hợp lệ.";
                ViewBag.DiscountedTotal = null;
            }
            else
            {
                var originalTotal = cart.Items.Sum(item => item._quantity * (item._product.GiaSP ?? 0m));
                var discountedTotal = originalTotal * (1 - discount.PhantramVoucher / 100);
                ViewBag.DiscountedTotal = discountedTotal;
                ViewBag.DiscountMessage = "Áp dụng mã khuyến mãi thành công!";
            }

            // Giữ voucher code rồi cho hiển thị lại trong view
            ViewBag.VoucherCode = voucherCode;

            return View("ShowCart", cart);
        }


    }
}


