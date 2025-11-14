using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class GiaoDichThanhToanServices : IGiaoDichThanhToanServices
    {
        private readonly ApplicationDbContext _context;

        public GiaoDichThanhToanServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public GiaoDichThanhToan GetGiaoDichThanhToanById(string id)
        {
            try
            {
                var giaoDich = _context.GiaoDichThanhToans
                    .Include(g => g.HoaDon)
                        .ThenInclude(h => h.KhachHang)
                    .Include(g => g.HoaDon)
                        .ThenInclude(h => h.NhanVien)
                    .Include(g => g.KenhThanhToan)
                    .FirstOrDefault(g => g.Id == id && !g.IsDelete);
                
                return giaoDich;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving GiaoDichThanhToan with ID {id}: {ex.Message}");
                return null;
            }
        }

        public HoaDon GetHoaDonById(string id)
        {
            try
            {
                var hoaDon = _context.HoaDons
                    .Include(h => h.KhachHang)
                    .Include(h => h.NhanVien)
                    .Include(h => h.GiaoDichThanhToans)
                        .ThenInclude(g => g.KenhThanhToan)
                    .FirstOrDefault(h => h.Id == id && !h.IsDelete);
                
                return hoaDon;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving HoaDon with ID {id}: {ex.Message}");
                return null;
            }
        }

        public List<ChiTietHoaDon> GetChiTietHoaDon(string hoaDonId)
        {
            try
            {
                var chiTietList = _context.ChiTietHoaDons
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.SanPham)
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.DonVi)
                    .Where(c => c.HoaDonId == hoaDonId && !c.IsDelete)
                    .ToList();
                
                return chiTietList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving ChiTietHoaDon for HoaDonId {hoaDonId}: {ex.Message}");
                return new List<ChiTietHoaDon>();
            }
        }
    }
}
