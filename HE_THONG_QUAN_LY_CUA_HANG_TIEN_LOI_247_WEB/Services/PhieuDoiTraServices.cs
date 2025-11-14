using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class PhieuDoiTraServices : IPhieuDoiTraServices
    {
        private readonly ApplicationDbContext _context;

        public PhieuDoiTraServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public PhieuDoiTra GetPhieuDoiTraById(string id)
        {
            try
            {
                var phieuDoiTra = _context.PhieuDoiTras
                    .Include(p => p.HoaDon)
                        .ThenInclude(h => h.KhachHang)
                    .Include(p => p.HoaDon)
                        .ThenInclude(h => h.NhanVien)
                    .Include(p => p.ChinhSach)
                    .Include(p => p.SanPhamDonVi)
                        .ThenInclude(s => s.SanPham)
                    .Include(p => p.SanPhamDonVi)
                        .ThenInclude(s => s.DonVi)
                    .FirstOrDefault(p => p.Id == id && !p.IsDelete);
                
                return phieuDoiTra;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving PhieuDoiTra with ID {id}: {ex.Message}");
                return null;
            }
        }

        public HoaDon GetHoaDonById(string hoaDonId)
        {
            try
            {
                var hoaDon = _context.HoaDons
                    .Include(h => h.KhachHang)
                    .Include(h => h.NhanVien)
                    .FirstOrDefault(h => h.Id == hoaDonId && !h.IsDelete);
                
                return hoaDon;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving HoaDon with ID {hoaDonId}: {ex.Message}");
                return null;
            }
        }

        public ChinhSachHoanTra GetChinhSachById(string chinhSachId)
        {
            try
            {
                var chinhSach = _context.ChinhSachHoanTras
                    .FirstOrDefault(c => c.Id == chinhSachId && !c.IsDelete);
                
                return chinhSach;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving ChinhSachHoanTra with ID {chinhSachId}: {ex.Message}");
                return null;
            }
        }

        public List<HoaDon> GetAllHoaDons()
        {
            try
            {
                return _context.HoaDons
                    .Include(h => h.KhachHang)
                    .Include(h => h.NhanVien)
                    .Where(h => !h.IsDelete && h.TrangThai == "DaThanhToan")
                    .OrderByDescending(h => h.NgayLap)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving HoaDons: {ex.Message}");
                return new List<HoaDon>();
            }
        }

        public List<ChinhSachHoanTra> GetAllChinhSachs()
        {
            try
            {
                var now = DateTime.Now.Date;
                return _context.ChinhSachHoanTras
                    .Where(c => !c.IsDelete && 
                                c.ApDungTuNgay.Date <= now && 
                                c.ApDungDenNgay.Date >= now)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving ChinhSachHoanTras: {ex.Message}");
                return new List<ChinhSachHoanTra>();
            }
        }

        public List<SanPhamDonVi> GetSanPhamDonVisByHoaDon(string hoaDonId)
        {
            try
            {
                var chiTietHoaDons = _context.ChiTietHoaDons
                    .Include(ct => ct.SanPhamDonVi)
                        .ThenInclude(sp => sp.SanPham)
                    .Include(ct => ct.SanPhamDonVi)
                        .ThenInclude(sp => sp.DonVi)
                    .Where(ct => ct.HoaDonId == hoaDonId && !ct.IsDelete)
                    .Select(ct => ct.SanPhamDonVi)
                    .ToList();
                
                return chiTietHoaDons;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving SanPhamDonVis for HoaDon {hoaDonId}: {ex.Message}");
                return new List<SanPhamDonVi>();
            }
        }

        public bool CreatePhieuDoiTra(PhieuDoiTra phieuDoiTra)
        {
            try
            {
                phieuDoiTra.Id = GeneratePhieuDoiTraId();
                phieuDoiTra.NgayDoiTra = DateTime.Now;
                phieuDoiTra.IsDelete = false;

                _context.PhieuDoiTras.Add(phieuDoiTra);
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating PhieuDoiTra: {ex.Message}");
                return false;
            }
        }

        public string GeneratePhieuDoiTraId()
        {
            try
            {
                var lastPhieu = _context.PhieuDoiTras
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefault();

                if (lastPhieu == null)
                {
                    return "PDT0001";
                }

                // Extract number from ID (e.g., PDT0001 -> 1)
                string lastId = lastPhieu.Id;
                string numberPart = lastId.Substring(3);
                int nextNumber = int.Parse(numberPart) + 1;

                return $"PDT{nextNumber:D4}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating PhieuDoiTra ID: {ex.Message}");
                return $"PDT{DateTime.Now:yyyyMMddHHmmss}";
            }
        }
    }
}
