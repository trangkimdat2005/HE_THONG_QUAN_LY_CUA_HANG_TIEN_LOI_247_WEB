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
    }
}
