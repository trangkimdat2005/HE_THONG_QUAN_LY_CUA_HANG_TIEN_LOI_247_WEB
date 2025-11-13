using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyBarCodeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyBarCodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/QuanLyBarCode/DanhSachTemNhan")]
        public IActionResult DanhSachTemNhan()
        {
            // Lấy danh sách tem nhãn với thông tin liên quan
            var lstTemNhan = _context.TemNhans
                .Where(t => !t.IsDelete)
                .Include(t => t.MaDinhDanh)
                    .ThenInclude(m => m.SanPhamDonVi)
                        .ThenInclude(sp => sp.SanPham)
                .Include(t => t.MaDinhDanh)
                    .ThenInclude(m => m.SanPhamDonVi)
                        .ThenInclude(sp => sp.DonVi)
                .OrderByDescending(t => t.NgayIn)
                .AsNoTracking()
                .ToList();

            // Lấy danh sách mã định danh để chọn khi tạo tem mới
            var lstMaDinhDanh = _context.MaDinhDanhSanPhams
                .Where(m => !m.IsDelete)
                .Include(m => m.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .Include(m => m.SanPhamDonVi)
                    .ThenInclude(sp => sp.DonVi)
                .AsNoTracking()
                .ToList();

            ViewData["lstTemNhan"] = lstTemNhan;
            ViewData["lstMaDinhDanh"] = lstMaDinhDanh;

            return View();
        }

        [Route("/QuanLyBarCode/DinhDanhSanPham")]
        public IActionResult DinhDanhSanPham()
        {
            // Lấy danh sách mã định danh sản phẩm
            var lstMaDinhDanh = _context.MaDinhDanhSanPhams
                .Where(m => !m.IsDelete)
                .Include(m => m.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .Include(m => m.SanPhamDonVi)
                    .ThenInclude(sp => sp.DonVi)
                .OrderBy(m => m.SanPhamDonVi.SanPham.Ten)
                .AsNoTracking()
                .ToList();

            // Lấy danh sách sản phẩm đơn vị để chọn khi tạo mã mới
            var lstSanPhamDonVi = _context.SanPhamDonVis
                .Where(sp => !sp.IsDelete)
                .Include(sp => sp.SanPham)
                .Include(sp => sp.DonVi)
                .OrderBy(sp => sp.SanPham.Ten)
                .AsNoTracking()
                .ToList();

            ViewData["lstMaDinhDanh"] = lstMaDinhDanh;
            ViewData["lstSanPhamDonVi"] = lstSanPhamDonVi;

            return View();
        }
    }
}
