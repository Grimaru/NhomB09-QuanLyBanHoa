using demo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace demo2.ViewModels
{
    public class ReportOrderViewModel
    {
        public IEnumerable<demo2.Models.DetailProduct> DetailProducts { get; set; }
        public List<string> TenSanPham { get; set; }
        public List<int> SoLuongSanPham { get; set; }
        public decimal TongDoanhThu { get; set; }
        public string KhachHangTop1 { get; set; }
        public string SanPhamBestSeller { get; set; }
        public int SoLuongBestSeller{ get; set; }
    }
}