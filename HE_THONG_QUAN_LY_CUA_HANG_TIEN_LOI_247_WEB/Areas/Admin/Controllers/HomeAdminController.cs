using System.Diagnostics;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly ILogger<HomeAdminController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeAdminController(ILogger<HomeAdminController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            // Th?ng kê t?ng quan
            var tongKhachHang = _context.KhachHangs.Count(k => !k.IsDelete);
            var tongNhanVien = _context.NhanViens.Count(n => !n.IsDelete);
            var tongSanPham = _context.SanPhams.Count(s => !s.IsDelete);
            var tongHoaDon = _context.HoaDons.Count(h => !h.IsDelete);
            
            // Doanh thu hôm nay
            var homNay = DateOnly.FromDateTime(DateTime.Now);
            var doanhThuHomNay = _context.HoaDons
                .Where(h => !h.IsDelete && DateOnly.FromDateTime(h.NgayLap) == homNay)
                .Sum(h => h.TongTien ?? 0);
            
            // Doanh thu tu?n này
            var dauTuan = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            var doanhThuTuan = _context.HoaDons
                .Where(h => !h.IsDelete && h.NgayLap >= dauTuan)
                .Sum(h => h.TongTien ?? 0);
            
            // Doanh thu tháng này
            var dauThang = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var doanhThuThang = _context.HoaDons
                .Where(h => !h.IsDelete && h.NgayLap >= dauThang)
                .Sum(h => h.TongTien ?? 0);
            
            // ??n hàng ch? x? lý
            var donChoXuLy = _context.HoaDons
                .Count(h => !h.IsDelete && h.TrangThai == "ChoXuLy");
            
            // Top 5 s?n ph?m bán ch?y (tu?n này)
            var topSanPham = _context.ChiTietHoaDons
                .Where(ct => !ct.IsDelete && ct.HoaDon.NgayLap >= dauTuan)
                .GroupBy(ct => new { 
                    ct.SanPhamDonViId,
                    TenSP = ct.SanPhamDonVi.SanPham.Ten,
                    DonVi = ct.SanPhamDonVi.DonVi.Ten
                })
                .Select(g => new {
                    g.Key.TenSP,
                    g.Key.DonVi,
                    SoLuong = g.Sum(ct => ct.SoLuong)
                })
                .OrderByDescending(x => x.SoLuong)
                .Take(5)
                .ToList();
            
            // Doanh thu 7 ngày g?n ?ây (cho bi?u ??)
            var doanhThu7Ngay = new List<object>();
            for (int i = 6; i >= 0; i--)
            {
                var ngay = DateTime.Now.AddDays(-i).Date;
                var ngayOnly = DateOnly.FromDateTime(ngay);
                var doanhThu = _context.HoaDons
                    .Where(h => !h.IsDelete && DateOnly.FromDateTime(h.NgayLap) == ngayOnly)
                    .Sum(h => h.TongTien ?? 0);
                    
                doanhThu7Ngay.Add(new {
                    Ngay = ngay.ToString("dd/MM"),
                    DoanhThu = doanhThu
                });
            }
            
            // S?n ph?m s?p h?t hàng (t?n kho < 10)
            var sanPhamSapHet = _context.TonKhos
                .Where(t => !t.IsDelete && t.SoLuongTon < 10)
                .Include(t => t.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .Include(t => t.SanPhamDonVi)
                    .ThenInclude(sp => sp.DonVi)
                .OrderBy(t => t.SoLuongTon)
                .Take(5)
                .Select(t => new {
                    TenSP = t.SanPhamDonVi.SanPham.Ten,
                    DonVi = t.SanPhamDonVi.DonVi.Ten,
                    SoLuongTon = t.SoLuongTon
                })
                .ToList();
            
            // Truy?n d? li?u sang View
            ViewData["TongKhachHang"] = tongKhachHang;
            ViewData["TongNhanVien"] = tongNhanVien;
            ViewData["TongSanPham"] = tongSanPham;
            ViewData["TongHoaDon"] = tongHoaDon;
            ViewData["DoanhThuHomNay"] = doanhThuHomNay;
            ViewData["DoanhThuTuan"] = doanhThuTuan;
            ViewData["DoanhThuThang"] = doanhThuThang;
            ViewData["DonChoXuLy"] = donChoXuLy;
            ViewData["TopSanPham"] = topSanPham;
            ViewData["DoanhThu7Ngay"] = doanhThu7Ngay;
            ViewData["SanPhamSapHet"] = sanPhamSapHet;
            
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
