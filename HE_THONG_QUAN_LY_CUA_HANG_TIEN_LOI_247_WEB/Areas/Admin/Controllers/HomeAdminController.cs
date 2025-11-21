using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly ILogger<HomeAdminController> _logger;
        private readonly IDashboardServices _dashboardServices;

        public HomeAdminController(ILogger<HomeAdminController> logger, IDashboardServices dashboardServices)
        {
            _logger = logger;
            _dashboardServices = dashboardServices;
        }
        //[Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            var statistics = _dashboardServices.GetDashboardStatistics();
            
            var sanPhamSapHet = _dashboardServices.GetLowStockProducts(threshold: 10, limit: 5);
            
            var doanhThu7Ngay = _dashboardServices.GetDailyRevenue(days: 7);
            
            var topSanPham = _dashboardServices.GetTopSellingProducts(days: 7, limit: 5);
            
            ViewData["TongKhachHang"] = statistics.TongKhachHang;
            ViewData["TongNhanVien"] = statistics.TongNhanVien;
            ViewData["TongSanPham"] = statistics.TongSanPham;
            ViewData["TongHoaDon"] = statistics.TongHoaDon;
            ViewData["TongDoanhThu"] = statistics.TongDoanhThu;
            ViewData["DonChoXuLy"] = statistics.DonChoXuLy;
            ViewData["SanPhamSapHet"] = sanPhamSapHet;
            ViewData["DoanhThu7Ngay"] = doanhThu7Ngay;
            ViewData["TopSanPham"] = topSanPham;
            
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
