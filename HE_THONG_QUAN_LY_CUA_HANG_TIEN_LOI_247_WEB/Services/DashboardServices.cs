using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class DashboardServices : IDashboardServices
    {
        private readonly ApplicationDbContext _context;

        public DashboardServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public DashboardStatisticsViewModel GetDashboardStatistics()
        {
            var statistics = new DashboardStatisticsViewModel
            {
                TongKhachHang = _context.KhachHangs.Count(k => !k.IsDelete),
                TongNhanVien = _context.NhanViens.Count(n => !n.IsDelete),
                TongSanPham = _context.SanPhams.Count(s => !s.IsDelete),
                TongHoaDon = _context.HoaDons.Count(h => !h.IsDelete),
                TongDoanhThu = _context.ChiTietHoaDons
                    .Where(ct => !ct.IsDelete && !ct.HoaDon.IsDelete)
                    .Sum(ct => ct.SoLuong * ct.DonGia),
                DonChoXuLy = _context.HoaDons
                    .Count(h => !h.IsDelete && h.TrangThai == "ChoXuLy")
            };

            return statistics;
        }

        public List<LowStockProductViewModel> GetLowStockProducts(int threshold = 10, int limit = 5)
        {
            var lowStockProducts = _context.TonKhos
                .Where(t => !t.IsDelete && t.SoLuongTon < threshold)
                .Include(t => t.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .Include(t => t.SanPhamDonVi)
                    .ThenInclude(sp => sp.DonVi)
                .OrderBy(t => t.SoLuongTon)
                .Take(limit)
                .Select(t => new LowStockProductViewModel
                {
                    SanPhamDonViId = t.SanPhamDonViId,
                    TenSanPham = t.SanPhamDonVi.SanPham.Ten,
                    DonVi = t.SanPhamDonVi.DonVi.Ten,
                    SoLuongTon = t.SoLuongTon
                })
                .ToList();

            return lowStockProducts;
        }

        public List<DailyRevenueViewModel> GetDailyRevenue(int days = 7)
        {
            var revenueList = new List<DailyRevenueViewModel>();

            for (int i = days - 1; i >= 0; i--)
            {
                var ngay = DateTime.Now.AddDays(-i).Date;

                // L?y doanh thu t? ChiTietHoaDons thay vì HoaDons
                var doanhThu = _context.ChiTietHoaDons
                    .Where(ct => !ct.IsDelete 
                        && !ct.HoaDon.IsDelete 
                        && DateOnly.FromDateTime(ct.HoaDon.NgayLap) == DateOnly.FromDateTime(ngay))
                    .Sum(ct => ct.SoLuong * ct.DonGia);

                revenueList.Add(new DailyRevenueViewModel
                {
                    Ngay = ngay.ToString("dd/MM"),
                    DoanhThu = doanhThu
                });
            }

            return revenueList;
        }

        public List<TopProductViewModel> GetTopSellingProducts(int days = 7, int limit = 5)
        {
            var dauTuan = DateTime.Now.AddDays(-days);

            var topProducts = _context.ChiTietHoaDons
                .Where(ct => !ct.IsDelete && ct.HoaDon.NgayLap >= dauTuan)
                .Include(ct => ct.SanPhamDonVi)
                    .ThenInclude(sp => sp.SanPham)
                .Include(ct => ct.SanPhamDonVi)
                    .ThenInclude(sp => sp.DonVi)
                .GroupBy(ct => new
                {
                    ct.SanPhamDonVi.SanPham.Id,
                    TenSP = ct.SanPhamDonVi.SanPham.Ten,
                    DonVi = ct.SanPhamDonVi.DonVi.Ten
                })
                .Select(g => new TopProductViewModel
                {
                    TenSanPham = g.Key.TenSP,
                    DonVi = g.Key.DonVi,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    DoanhThu = g.Sum(ct => ct.SoLuong * ct.DonGia)
                })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(limit)
                .ToList();

            return topProducts;
        }
    }
}
