using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhanTichBaoCaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhanTichBaoCaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/PhanTichBaoCao/BaoCaoBanChay")]
        public IActionResult BaoCaoBanChay(DateTime? tuNgay, DateTime? denNgay, int top = 10)
        {
            // Mặc định: 30 ngày gần đây
            var startDate = tuNgay ?? DateTime.Now.AddDays(-30);
            var endDate = denNgay ?? DateTime.Now;

            // Thống kê sản phẩm bán chạy
            var topSanPham = _context.ChiTietHoaDons
                .Where(ct => !ct.IsDelete && 
                       ct.HoaDon.NgayLap >= startDate && 
                       ct.HoaDon.NgayLap <= endDate)
                .Include(ct => ct.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .Include(ct => ct.SanPhamDonVi)
                    .ThenInclude(sp => sp.DonVi)
                .GroupBy(ct => new { 
                    ct.SanPhamDonVi.SanPham.Id,
                    TenSP = ct.SanPhamDonVi.SanPham.Ten
                })
                .Select(g => new {
                    MaSanPham = g.Key.Id,
                    TenSanPham = g.Key.TenSP,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    DoanhThu = g.Sum(ct => ct.SoLuong * ct.DonGia)
                })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(top)
                .ToList();

            ViewData["TopSanPham"] = topSanPham;
            ViewData["TuNgay"] = startDate.ToString("yyyy-MM-dd");
            ViewData["DenNgay"] = endDate.ToString("yyyy-MM-dd");
            ViewData["Top"] = top;

            return View();
        }

        [Route("/PhanTichBaoCao/BaoCaoDoanhThu")]
        public IActionResult BaoCaoDoanhThu(DateTime? tuNgay, DateTime? denNgay)
        {
            // Lấy danh sách báo cáo doanh thu
            var lstBaoCao = _context.BaoCaoDoanhThus
                .Where(bc => !bc.IsDelete)
                .Include(bc => bc.BaoCao)
                .OrderByDescending(bc => bc.BaoCao.NgayLap)
                .AsNoTracking()
                .ToList();

            // Thống kê doanh thu theo tháng (6 tháng gần đây)
            var doanhThuTheoThang = new List<object>();
            for (int i = 5; i >= 0; i--)
            {
                var thang = DateTime.Now.AddMonths(-i);
                var dauThang = new DateTime(thang.Year, thang.Month, 1);
                var cuoiThang = dauThang.AddMonths(1).AddDays(-1);

                var doanhThu = _context.HoaDons
                    .Where(h => !h.IsDelete && 
                           h.NgayLap >= dauThang && 
                           h.NgayLap <= cuoiThang)
                    .Sum(h => h.TongTien ?? 0);

                doanhThuTheoThang.Add(new {
                    Thang = thang.ToString("MM/yyyy"),
                    DoanhThu = doanhThu
                });
            }

            ViewData["lstBaoCao"] = lstBaoCao;
            ViewData["DoanhThuTheoThang"] = doanhThuTheoThang;

            return View();
        }

        [Route("/PhanTichBaoCao/BaoCaoTonKho")]
        public IActionResult BaoCaoTonKho()
        {
            // Lấy danh sách báo cáo tồn kho
            var lstBaoCao = _context.BaoCaoTonKhos
                .Where(bc => !bc.IsDelete)
                .Include(bc => bc.BaoCao)
                .Include(bc => bc.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .Include(bc => bc.SanPhamDonVi)
                    .ThenInclude(sp => sp.DonVi)
                .OrderByDescending(bc => bc.BaoCao.NgayLap)
                .AsNoTracking()
                .ToList();

            // Thống kê tồn kho hiện tại
            var tonKhoHienTai = _context.TonKhos
                .Where(tk => !tk.IsDelete)
                .Include(tk => tk.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .Include(tk => tk.SanPhamDonVi)
                    .ThenInclude(sp => sp.DonVi)
                .OrderBy(tk => tk.SoLuongTon)
                .Take(20)
                .Select(tk => new {
                    TenSP = tk.SanPhamDonVi.SanPham.Ten,
                    DonVi = tk.SanPhamDonVi.DonVi.Ten,
                    SoLuongTon = tk.SoLuongTon
                })
                .ToList();

            ViewData["lstBaoCao"] = lstBaoCao;
            ViewData["TonKhoHienTai"] = tonKhoHienTai;

            return View();
        }
    }
}
