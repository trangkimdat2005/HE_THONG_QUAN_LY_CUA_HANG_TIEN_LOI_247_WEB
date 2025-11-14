using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class ChinhSachHoanTraServices : IChinhSachHoanTraServices
    {
        private readonly ApplicationDbContext _context;

        public ChinhSachHoanTraServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ChinhSachHoanTra> GetAllChinhSach()
        {
            try
            {
                return _context.ChinhSachHoanTras
                    .Where(c => !c.IsDelete)
                    .OrderByDescending(c => c.ApDungTuNgay)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all ChinhSachHoanTra: {ex.Message}");
                return new List<ChinhSachHoanTra>();
            }
        }

        public ChinhSachHoanTra GetChinhSachById(string id)
        {
            try
            {
                return _context.ChinhSachHoanTras
                    .Include(c => c.ChinhSachHoanTraDanhMucs)
                        .ThenInclude(cd => cd.DanhMuc)
                    .FirstOrDefault(c => c.Id == id && !c.IsDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ChinhSachHoanTra by ID {id}: {ex.Message}");
                return null;
            }
        }

        public bool CreateChinhSach(ChinhSachHoanTra chinhSach)
        {
            try
            {
                chinhSach.Id = GenerateChinhSachId();
                chinhSach.IsDelete = false;

                _context.ChinhSachHoanTras.Add(chinhSach);
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating ChinhSachHoanTra: {ex.Message}");
                return false;
            }
        }

        public bool UpdateChinhSach(ChinhSachHoanTra chinhSach)
        {
            try
            {
                var existing = _context.ChinhSachHoanTras
                    .FirstOrDefault(c => c.Id == chinhSach.Id && !c.IsDelete);

                if (existing == null)
                    return false;

                existing.TenChinhSach = chinhSach.TenChinhSach;
                existing.ThoiHan = chinhSach.ThoiHan;
                existing.DieuKien = chinhSach.DieuKien;
                existing.ApDungToanBo = chinhSach.ApDungToanBo;
                existing.ApDungTuNgay = chinhSach.ApDungTuNgay;
                existing.ApDungDenNgay = chinhSach.ApDungDenNgay;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating ChinhSachHoanTra: {ex.Message}");
                return false;
            }
        }

        public bool DeleteChinhSach(string id)
        {
            try
            {
                var chinhSach = _context.ChinhSachHoanTras
                    .FirstOrDefault(c => c.Id == id && !c.IsDelete);

                if (chinhSach == null)
                    return false;

                chinhSach.IsDelete = true;
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting ChinhSachHoanTra: {ex.Message}");
                return false;
            }
        }

        public string GenerateChinhSachId()
        {
            try
            {
                var lastChinhSach = _context.ChinhSachHoanTras
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefault();

                if (lastChinhSach == null)
                {
                    return "CSHT0001";
                }

                // Extract number from ID (e.g., CSHT0001 -> 1)
                string lastId = lastChinhSach.Id;
                string numberPart = lastId.Substring(4);
                int nextNumber = int.Parse(numberPart) + 1;

                return $"CSHT{nextNumber:D4}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating ChinhSachHoanTra ID: {ex.Message}");
                return $"CSHT{DateTime.Now:yyyyMMddHHmmss}";
            }
        }

        public List<DanhMuc> GetAllDanhMuc()
        {
            try
            {
                return _context.DanhMucs
                    .Where(d => !d.IsDelete)
                    .OrderBy(d => d.Ten)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all DanhMuc: {ex.Message}");
                return new List<DanhMuc>();
            }
        }

        public bool AddDanhMucToChinhSach(string chinhSachId, List<string> danhMucIds)
        {
            try
            {
                // Xóa các danh mục cũ (nếu có)
                var oldDanhMucs = _context.ChinhSachHoanTraDanhMucs
                    .Where(x => x.ChinhSachId == chinhSachId)
                    .ToList();
                
                if (oldDanhMucs.Any())
                {
                    _context.ChinhSachHoanTraDanhMucs.RemoveRange(oldDanhMucs);
                }

                // Thêm các danh mục mới
                foreach (var danhMucId in danhMucIds)
                {
                    var chinhSachDanhMuc = new ChinhSachHoanTraDanhMuc
                    {
                        ChinhSachId = chinhSachId,
                        DanhMucId = danhMucId,
                        IsDelete = false
                    };
                    
                    _context.ChinhSachHoanTraDanhMucs.Add(chinhSachDanhMuc);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding DanhMuc to ChinhSach: {ex.Message}");
                return false;
            }
        }
    }
}
