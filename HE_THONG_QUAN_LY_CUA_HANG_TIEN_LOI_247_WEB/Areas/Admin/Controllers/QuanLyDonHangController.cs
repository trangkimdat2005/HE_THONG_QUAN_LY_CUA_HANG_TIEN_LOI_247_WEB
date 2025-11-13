using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyDonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyDonHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/QuanLyDonHang/DanhSachDonHang")]
        public IActionResult DanhSachDonHang()
        {
            // Lấy danh sách hóa đơn
            var lstHoaDon = _context.HoaDons
                .Where(hd => !hd.IsDelete)
                .Include(hd => hd.NhanVien)
                .Include(hd => hd.KhachHang)
                .OrderByDescending(hd => hd.NgayLap)
                .AsNoTracking()
                .ToList();

            ViewData["lstHoaDon"] = lstHoaDon;

            return View();
        }

        [Route("/QuanLyDonHang/XemChiTietHoaDon")]
        public IActionResult XemChiTietHoaDon(string id)
        {
            // Lấy thông tin hóa đơn
            var hoaDon = _context.HoaDons
                .Where(hd => hd.Id == id && !hd.IsDelete)
                .Include(hd => hd.NhanVien)
                .Include(hd => hd.KhachHang)
                .Include(hd => hd.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPhamDonVi)
                        .ThenInclude(sp => sp.SanPham)
                .Include(hd => hd.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPhamDonVi)
                        .ThenInclude(sp => sp.DonVi)
                .Include(hd => hd.GiaoDichThanhToans)
                    .ThenInclude(gd => gd.KenhThanhToan)
                .AsNoTracking()
                .FirstOrDefault();

            if (hoaDon == null)
            {
                return NotFound();
            }

            ViewData["HoaDon"] = hoaDon;

            return View();
        }
    }
}
