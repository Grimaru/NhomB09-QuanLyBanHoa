using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using demo2.Models;
using PagedList;
using PagedList.Mvc;

namespace demo2.Controllers
{
    public class ProductController : Controller
    {
        doanEntities database = new doanEntities();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SanPham(string _page, int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            if (_page == null)
            {
                var productList = database.Products.OrderBy(x => x.TenSP);
                return View(productList.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var productList = database.Products.OrderBy(x => x.TenSP)
                    .Where(x => x.MotaSP.ToString() == _page);
                return View(productList);
            }
        }

        public ActionResult TimTheoTen(string _name, string _page, int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);

            // Get the list of products, optionally filtering by name
            var productList = database.Products.AsQueryable();

            if (!string.IsNullOrEmpty(_name))
            {
                productList = productList.Where(s => s.TenSP.Contains(_name));
            }

            // Filter by description if _page is provided
            if (!string.IsNullOrEmpty(_page))
            {
                productList = productList.Where(x => x.MotaSP.ToString() == _page);
            }

            // Order the products
            productList = productList.OrderBy(x => x.TenSP);

            // Return the paged list
            return View(productList.ToPagedList(pageNum, pageSize));
        }


        public ActionResult TimTheoGia(double min = double.MinValue, double max = double.MaxValue)
        {
            var list = database.Products.Where(p => (double)p.GiaSP >= min && (double)p.GiaSP <= max).ToList();
            return View(list);
        }

        public ActionResult Details(int id)
        {
            return View(database.Products.Where(s => s.IDSP == id).FirstOrDefault());
        }      
    }
}