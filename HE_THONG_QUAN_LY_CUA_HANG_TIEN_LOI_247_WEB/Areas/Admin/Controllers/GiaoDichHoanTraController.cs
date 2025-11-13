using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GiaoDichHoanTraController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;
        private readonly ApplicationDbContext _context;
        
        public GiaoDichHoanTraController(IQuanLyServices quanLySevices, ApplicationDbContext context)
        {
            _quanLySevices = quanLySevices;
            _context = context;
        }
        
        [Route("/GiaoDichHoanTra/ChinhSachDoiTra")]
        public IActionResult ChinhSachDoiTra()
        {
            var lstChinhSachDoiTra = _quanLySevices.GetList<ChinhSachHoanTra>();
            
            ViewData["lstChinhSachDoiTra"] = lstChinhSachDoiTra;
            
            return View();
        }
        
        [Route("/GiaoDichHoanTra/GiaoDichThanhToan")]
        public IActionResult GiaoDichThanhToan()
        {
            // Load với navigation properties
            var lstGiaoDichThanhToan = _context.GiaoDichThanhToans
                .Where(gd => !gd.IsDelete)
                .Include(gd => gd.KenhThanhToan)
                .Include(gd => gd.HoaDon)
                .OrderByDescending(gd => gd.NgayGd)
                .AsNoTracking()
                .ToList();
            
            ViewData["lstGiaoDichThanhToan"] = lstGiaoDichThanhToan;
            
            return View();
        }
        
        [Route("/GiaoDichHoanTra/PhieuDoiTra")]
        public IActionResult PhieuDoiTra()
        {
            // Load với navigation properties
            var lstPhieuDoiTra = _context.PhieuDoiTras
                .Where(p => !p.IsDelete)
                .Include(p => p.HoaDon)
                .Include(p => p.ChinhSach)
                .Include(p => p.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .OrderByDescending(p => p.NgayDoiTra)
                .AsNoTracking()
                .ToList();
            
            ViewData["lstPhieuDoiTra"] = lstPhieuDoiTra;
            
            return View();
        }
    }
}
