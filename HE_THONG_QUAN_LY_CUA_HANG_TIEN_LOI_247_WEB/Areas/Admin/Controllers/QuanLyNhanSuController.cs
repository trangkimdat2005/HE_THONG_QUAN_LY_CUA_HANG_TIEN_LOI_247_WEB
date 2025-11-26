using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class QuanLyNhanSuController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyNhanSuController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyNhanSu/DanhSachNhanVien")]
        public IActionResult DanhSachNhanVien()
        {
            var lstNhanVien = _quanLySevices.GetList<NhanVien>();
            ViewData["lstNhanVien"] = lstNhanVien;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyNhanSu/LichLamViec")]
        public IActionResult LichLamViec()
        {
            var lstNhanVien = _quanLySevices.GetList<NhanVien>()
                                            .Where(nv => !nv.IsDelete && (nv.TrangThai == "Hoạt động" || nv.TrangThai == "Active"))
                                            .OrderBy(nv => nv.Id)
                                            .ToList();

            ViewData["lstNhanVien"] = lstNhanVien;
            return View();
        }
        [HttpPost]
        [Route("/API/QuanLyNhanSu/GetLichLamViec")]
        public IActionResult GetLichLamViecData([FromBody] LichFilterModel model)
        {
            try
            {
                var date = model.NgayBatDau;
                var dayOfWeek = (int)date.DayOfWeek;
                var diff = dayOfWeek == 0 ? 6 : dayOfWeek - 1; 
                var monday = date.AddDays(-diff).Date;
                var sunday = monday.AddDays(6).Date;

                var allPhanCong = _quanLySevices.GetList<PhanCongCaLamViec>()
                                    .Where(pc => !pc.IsDelete && pc.Ngay.Date >= monday && pc.Ngay.Date <= sunday)
                                    .ToList();

                var allNhanVien = _quanLySevices.GetList<NhanVien>();

                var allCaLamViec = _quanLySevices.GetList<CaLamViec>()
                                    .Where(c => !c.IsDelete)
                                    .OrderBy(c => c.ThoiGianBatDau)
                                    .ToList();

                var query = from pc in allPhanCong
                            join nv in allNhanVien on pc.NhanVienId equals nv.Id
                            join ca in allCaLamViec on pc.CaLamViecId equals ca.Id
                            select new
                            {
                                pc.Id,
                                Ngay = pc.Ngay.ToString("yyyy-MM-dd"), // Format ngày chuẩn ISO
                                NhanVienId = nv.Id,
                                TenNhanVien = nv.HoTen,
                                CaId = ca.Id
                            };

                if (!string.IsNullOrEmpty(model.NhanVienId) && model.NhanVienId != "all")
                {
                    query = query.Where(x => x.NhanVienId == model.NhanVienId);
                }

                var shiftDefs = allCaLamViec.Select(c => new
                {
                    c.Id,
                    c.TenCa,
                    ThoiGian = $"{c.ThoiGianBatDau:HH:mm} - {c.ThoiGianKetThuc:HH:mm}"
                }).ToList();

                return Ok(new
                {
                    StartDate = monday.ToString("dd/MM/yyyy"),
                    EndDate = sunday.ToString("dd/MM/yyyy"),
                    ShiftDefs = shiftDefs, // Danh sách ca để vẽ dòng
                    ScheduleData = query.ToList() // Dữ liệu điền vào ô
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        [Route("/API/QuanLyNhanSu/GetShiftMetadata")]
        public IActionResult GetShiftMetadata()
        {
            var shifts = _quanLySevices.GetList<CaLamViec>()
                            .Where(c => !c.IsDelete)
                            .OrderBy(c => c.ThoiGianBatDau)
                            .Select(c => new {
                                c.Id,
                                c.TenCa,
                                ThoiGian = $"{c.ThoiGianBatDau:HH:mm} - {c.ThoiGianKetThuc:HH:mm}"
                            })
                            .ToList();
            return Ok(shifts);
        }

        [HttpPost]
        [Route("/API/QuanLyNhanSu/GetPhanCongChiTiet")]
        public IActionResult GetPhanCongChiTiet([FromBody] PhanCongFilterModel model)
        {
            try
            {
                var date = model.NgayBatDau;
                var dayOfWeek = (int)date.DayOfWeek;
                var diff = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
                var monday = date.AddDays(-diff).Date;
                var sunday = monday.AddDays(6).Date;

                var data = _quanLySevices.GetList<PhanCongCaLamViec>()
                            .Where(pc => !pc.IsDelete
                                    && pc.NhanVienId == model.NhanVienId
                                    && pc.Ngay.Date >= monday
                                    && pc.Ngay.Date <= sunday)
                            .Select(pc => new {
                                pc.CaLamViecId,
                                Ngay = pc.Ngay.ToString("yyyy-MM-dd")
                            })
                            .ToList();

                return Ok(new
                {
                    Data = data,
                    StartDate = monday.ToString("yyyy-MM-dd"),
                    EndDate = sunday.ToString("yyyy-MM-dd")
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost]
        [Route("/API/QuanLyNhanSu/SavePhanCong")]
        public async Task<IActionResult> SavePhanCong([FromBody] SavePhanCongModel model)
        {
            try
            {
                if (model.Assignments == null) model.Assignments = new List<AssignmentItem>();

                var today = DateTime.Now.Date;
                var date = model.NgayBatDau;
                var dayOfWeek = (int)date.DayOfWeek;
                var diff = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
                var monday = date.AddDays(-diff).Date;
                var sunday = monday.AddDays(6).Date;

                var lastRecord = _quanLySevices.GetList<PhanCongCaLamViec>()
                                               .Where(x => x.Id.StartsWith("PC") && x.Id.Length == 6)
                                               .OrderByDescending(x => x.Id)
                                               .FirstOrDefault();

                long currentMaxNumber = 0;
                if (lastRecord != null)
                {
                    long.TryParse(lastRecord.Id.Substring(2), out currentMaxNumber);
                }

                foreach (var item in model.Assignments)
                {
                    var itemDate = DateTime.Parse(item.Ngay).Date;
                    if (itemDate < today) continue;

                    var existingItem = _quanLySevices.GetPhanCongByDate(model.NhanVienId, itemDate);

                    bool needAddNew = false;

                    

                    if (existingItem != null)
                    {
                        if (existingItem.CaLamViecId == item.CaLamViecId)
                        {
                            continue;
                        }
                        else
                        {
                            await _quanLySevices.BeginTransactionAsync();
                            _quanLySevices.HardDelete(existingItem);
                            if (!await _quanLySevices.CommitAsync())
                            {
                                return BadRequest(new { message = $"Lỗi: Không thể xóa lịch cũ ngày {item.Ngay} để cập nhật mới." });
                            }
                            needAddNew = true;
                        }
                    }
                    else
                    {
                        needAddNew = true;
                    }
                    if (needAddNew)
                    {
                        currentMaxNumber++;
                        string newId = "PC" + currentMaxNumber.ToString().PadLeft(4, '0');

                        var newPC = new PhanCongCaLamViec
                        {
                            Id = newId,
                            NhanVienId = model.NhanVienId,
                            CaLamViecId = item.CaLamViecId,
                            Ngay = itemDate,
                            IsDelete = false
                        };

                        await _quanLySevices.BeginTransactionAsync();

                        _quanLySevices.Add(newPC);

                        if (!await _quanLySevices.CommitAsync())
                        {
                            return BadRequest(new { message = $"Lỗi: Không thể thêm phân công ngày {item.Ngay}" });
                        }
                    }
                }
                var dbList = _quanLySevices.GetList<PhanCongCaLamViec>()
                                           .Where(p => p.NhanVienId == model.NhanVienId
                                                    && p.Ngay.Date >= monday
                                                    && p.Ngay.Date <= sunday
                                                    && !p.IsDelete)
                                           .ToList();

                foreach (var dbItem in dbList)
                {
                    if (dbItem.Ngay.Date < today) continue;

                    bool stillExists = model.Assignments.Any(x => DateTime.Parse(x.Ngay).Date == dbItem.Ngay.Date);

                    if (!stillExists)
                    {
                        await _quanLySevices.BeginTransactionAsync();
                        _quanLySevices.HardDelete(dbItem);
                        if (!await _quanLySevices.CommitAsync())
                        {
                            await _quanLySevices.BeginTransactionAsync();
                            _quanLySevices.SoftDelete(dbItem);
                            if (!await _quanLySevices.CommitAsync())
                            {
                                return BadRequest(new { message = $"Lỗi: Không thể xóa phân công ngày {dbItem.Ngay:yyyy-MM-dd}" });
                            }
                        }
                    }
                }

                return Ok(new { message = "Lưu phân công thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Authorize(Roles = "ADMIN")]
        [Route("/QuanLyNhanSu/PhanCongCaLamViec")]
        public IActionResult PhanCongCaLamViec()
        {
            var lstPhanCong = _quanLySevices.GetList<PhanCongCaLamViec>();
            ViewData["lstPhanCong"] = lstPhanCong;
            var lstCaLamViec = _quanLySevices.GetList<CaLamViec>();
            ViewData["lstCaLamViec"] = lstCaLamViec;
            var lstNhanVien = _quanLySevices.GetList<NhanVien>()
                        .Where(nv => !nv.IsDelete && (nv.TrangThai == "Hoạt động" || nv.TrangThai == "Active"))
                        .OrderBy(nv => nv.HoTen)
                        .ToList();

            ViewData["lstNhanVien"] = lstNhanVien;
            return View();
        }
        [HttpDelete]
        [Route("/API/NhanVien/Delete/{id}")]
        public async Task<IActionResult> DeleteNhanVien([FromRoute] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new { message = "ID nhân viên không hợp lệ." });
                }

                await _quanLySevices.BeginTransactionAsync();

                var nhanVien = _quanLySevices.GetById<NhanVien>(id);

                if (nhanVien == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = $"Không tìm thấy nhân viên (ID: {id}) hoặc đã bị xóa." });
                }

                _quanLySevices.HardDelete<NhanVien>(nhanVien);

                if (!await _quanLySevices.CommitAsync())
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete<NhanVien>(nhanVien);
                    if (!await _quanLySevices.CommitAsync())
                    {
                        return BadRequest(new { message = "Lỗi: Không thể thực hiện xóa." });
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = $"Đã xoá mềm nhân viên '{nhanVien.HoTen}'."
                        });
                    }
                }
                else
                {
                    return Ok(new { message = $"Đã xoá vĩnh viễn nhân viên '{nhanVien.HoTen}'." });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }

        }
        [HttpDelete]
        [Route("/API/PhanCong/Delete/{id}")]
        public async Task<IActionResult> DeletePhanCongCaLamViec([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "ID ca phân công không hợp lệ." });
            }

            try
            {
                await _quanLySevices.BeginTransactionAsync();
                var phanCong = _quanLySevices.GetList<PhanCongCaLamViec>()
                                    .FirstOrDefault(x => x.Id == id);

                if (phanCong == null)
                {
                    await _quanLySevices.RollbackAsync();
                    return NotFound(new { message = $"Không tìm thấy ca phân công (ID: {id}) hoặc đã bị xóa." });
                }

                _quanLySevices.HardDelete<PhanCongCaLamViec>(phanCong);

                if (!await _quanLySevices.CommitAsync())
                {
                    await _quanLySevices.BeginTransactionAsync();
                    _quanLySevices.SoftDelete<PhanCongCaLamViec>(phanCong);
                    if (!await _quanLySevices.CommitAsync())
                    {
                        return BadRequest(new { message = "Lỗi: Không thể thực hiện xóa." });
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = "Đã xoá mềm ca phân công."
                        });
                    }
                }
                else
                {
                    return Ok(new { message = "Đã xoá vĩnh viễn ca phân công." });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
    }
}
