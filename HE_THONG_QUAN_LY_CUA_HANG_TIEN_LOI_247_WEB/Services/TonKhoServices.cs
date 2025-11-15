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
                // Không l?c IsDelete ?? h? tr? d? li?u c?
                var phieuNhap = _context.PhieuNhaps
                    .Include(p => p.NhaCungCap)
                    .Include(p => p.NhanVien)
                    .FirstOrDefault(p => p.Id == id);  // ? ?ã b? ?i?u ki?n && !p.IsDelete
                
                return phieuNhap;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving PhieuNhap with ID {id}: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public List<ChiTietPhieuNhap> GetChiTietPhieuNhap(string phieuNhapId)
        {
            try
            {
                // Không l?c IsDelete và x? lý null navigation properties
                var chiTietList = _context.ChiTietPhieuNhaps
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.SanPham)
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.DonVi)
                    .Where(c => c.PhieuNhapId == phieuNhapId)  // ? ?ã b? ?i?u ki?n && !c.IsDelete
                    .ToList();
                
                // Log ?? debug
                if (chiTietList == null || chiTietList.Count == 0)
                {
                    Console.WriteLine($"Warning: No ChiTietPhieuNhap found for PhieuNhapId: {phieuNhapId}");
                }
                else
                {
                    Console.WriteLine($"Found {chiTietList.Count} ChiTietPhieuNhap records for PhieuNhapId: {phieuNhapId}");
                    
                    // Check for null navigation properties
                    foreach (var item in chiTietList)
                    {
                        if (item.SanPhamDonVi == null)
                        {
                            Console.WriteLine($"Warning: SanPhamDonVi is null for SanPhamDonViId: {item.SanPhamDonViId}");
                        }
                        else
                        {
                            if (item.SanPhamDonVi.SanPham == null)
                            {
                                Console.WriteLine($"Warning: SanPham is null for SanPhamDonViId: {item.SanPhamDonViId}");
                            }
                            if (item.SanPhamDonVi.DonVi == null)
                            {
                                Console.WriteLine($"Warning: DonVi is null for SanPhamDonViId: {item.SanPhamDonViId}");
                            }
                        }
                    }
                }
                
                return chiTietList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving ChiTietPhieuNhap for PhieuNhapId {phieuNhapId}: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return new List<ChiTietPhieuNhap>();
            }
        }

        public PhieuXuat GetPhieuXuatById(string id)
        {
            try
            {
                // Không l?c IsDelete ?? h? tr? d? li?u c?
                var phieuXuat = _context.PhieuXuats
                    .Include(p => p.KhachHang)
                    .Include(p => p.NhanVien)
                    .FirstOrDefault(p => p.Id == id);  // ? ?ã b? ?i?u ki?n && !p.IsDelete
                
                return phieuXuat;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving PhieuXuat with ID {id}: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public List<ChiTietPhieuXuat> GetChiTietPhieuXuat(string phieuXuatId)
        {
            try
            {
                // Không l?c IsDelete và x? lý null navigation properties
                var chiTietList = _context.ChiTietPhieuXuats
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.SanPham)
                    .Include(c => c.SanPhamDonVi)
                        .ThenInclude(s => s.DonVi)
                    .Where(c => c.PhieuXuatId == phieuXuatId)  // ? ?ã b? ?i?u ki?n && !c.IsDelete
                    .ToList();
                
                // Log ?? debug
                if (chiTietList == null || chiTietList.Count == 0)
                {
                    Console.WriteLine($"Warning: No ChiTietPhieuXuat found for PhieuXuatId: {phieuXuatId}");
                }
                else
                {
                    Console.WriteLine($"Found {chiTietList.Count} ChiTietPhieuXuat records for PhieuXuatId: {phieuXuatId}");
                }
                
                return chiTietList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving ChiTietPhieuXuat for PhieuXuatId {phieuXuatId}: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return new List<ChiTietPhieuXuat>();
            }
        }

        public KiemKe GetKiemKeById(string id)
        {
            try
            {
                // Không l?c IsDelete ?? h? tr? d? li?u c?
                return _context.KiemKes
                    .Include(kk => kk.NhanVien)
                    .Include(kk => kk.SanPhamDonVi)
                        .ThenInclude(sp => sp.SanPham)
                    .Include(kk => kk.SanPhamDonVi)
                        .ThenInclude(sp => sp.DonVi)
                    .FirstOrDefault(kk => kk.Id == id);  // ? ?ã b? ?i?u ki?n && !kk.IsDelete
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving KiemKe with ID {id}: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return null;
            }
        }
    }
}
