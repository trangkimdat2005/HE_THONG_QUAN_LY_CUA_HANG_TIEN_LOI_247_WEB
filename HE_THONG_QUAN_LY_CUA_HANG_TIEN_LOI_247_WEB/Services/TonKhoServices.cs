using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class TonKhoServices : ITonKhoServices
    {
        private readonly ApplicationDbContext _context;

        public TonKhoServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public PhieuNhap GetPhieuNhapById(string id)
        {
            try
            {
                var phieuNhap = _context.PhieuNhaps
                    .Include(p => p.NhaCungCap)
                    .Include(p => p.NhanVien)
                    .FirstOrDefault(p => p.Id == id && !p.IsDelete);
                
                return phieuNhap;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving PhieuNhap with ID {id}: {ex.Message}");
                return null;
            }
        }

        public List<ChiTietPhieuNhap> GetChiTietPhieuNhap(string phieuNhapId)
        {
            try
            {
                var chiTietList = _context.ChiTietPhieuNhaps
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.SanPham)
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.DonVi)
                    .Where(c => c.PhieuNhapId == phieuNhapId && !c.IsDelete)
                    .ToList();
                
                return chiTietList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving ChiTietPhieuNhap for PhieuNhapId {phieuNhapId}: {ex.Message}");
                return new List<ChiTietPhieuNhap>();
            }
        }

        public PhieuXuat GetPhieuXuatById(string id)
        {
            try
            {
                var phieuXuat = _context.PhieuXuats
                    .Include(p => p.KhachHang)
                    .Include(p => p.NhanVien)
                    .FirstOrDefault(p => p.Id == id && !p.IsDelete);
                
                return phieuXuat;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving PhieuXuat with ID {id}: {ex.Message}");
                return null;
            }
        }

        public List<ChiTietPhieuXuat> GetChiTietPhieuXuat(string phieuXuatId)
        {
            try
            {
                var chiTietList = _context.ChiTietPhieuXuats
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.SanPham)
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.DonVi)
                    .Where(c => c.PhieuXuatId == phieuXuatId && !c.IsDelete)
                    .ToList();
                
                return chiTietList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving ChiTietPhieuXuat for PhieuXuatId {phieuXuatId}: {ex.Message}");
                return new List<ChiTietPhieuXuat>();
            }
        }
        public KiemKe GetKiemKeById(string id)
        {
            // .Include(kk => kk.NhanVien) ?? l?y tên nhân viên
            return _context.KiemKes
                           .Include(kk => kk.NhanVien)
                           .FirstOrDefault(kk => kk.Id == id);
        }
    }
}
