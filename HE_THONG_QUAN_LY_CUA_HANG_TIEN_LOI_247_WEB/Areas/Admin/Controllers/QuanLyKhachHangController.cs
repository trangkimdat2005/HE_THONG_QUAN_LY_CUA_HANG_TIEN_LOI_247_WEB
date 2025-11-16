using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyKhachHangController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyKhachHangController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyKhachHang/DanhSachKhachHang")]
        public IActionResult DanhSachKhachHang()
        {
            var lstKhachHang = _quanLySevices.GetList<KhachHang>();
            ViewData["lstKhachHang"] = lstKhachHang;
            return View();
        }

        [Route("/QuanLyKhachHang/LichSuMuaHang")]
        public IActionResult LichSuMuaHang()
        {
            var lstLichSuMuaHang = _quanLySevices.GetList<LichSuMuaHang>();
            ViewData["lstLichSuMuaHang"] = lstLichSuMuaHang;
            return View();
        }

        [Route("/QuanLyKhachHang/TheThanhVien")]
        public IActionResult TheThanhVien()
        {
            var lstTheThanhVien = _quanLySevices.GetList<TheThanhVien>();
            ViewData["lstTheThanhVien"] = lstTheThanhVien;
            return View();
        }

        //=========================================API Xóa Khách Hàng=======================================================================
        [HttpPost]
        [Route("/API/delete-KhachHang")]
        public async Task<IActionResult> DeleteKhachHang([FromBody] DeleteKhachHangRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request?.Id))
                {
                    return BadRequest(new { message = "ID khách hàng không hợp lệ." });
                }

                var khachHang = _quanLySevices.GetById<KhachHang>(request.Id);
                
                if (khachHang == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng." });
                }

                // Soft delete
                if (_quanLySevices.SoftDelete<KhachHang>(khachHang))
                {
                    return Ok(new { message = "Xóa khách hàng thành công!" });
                }

                return BadRequest(new { message = "Không thể xóa khách hàng." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting KhachHang: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi khi xóa khách hàng: {ex.Message}" });
            }
        }

        //=========================================API Thẻ Thành Viên=======================================================================
        
        // Get Next ID
        [HttpPost]
        [Route("/API/get-next-id-TTV")]
        public Task<IActionResult> GetNextIdTTV([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<TheThanhVien>(request["prefix"].ToString(), int.Parse(request["totalLength"].ToString())) }));
        }

        // Get khách hàng chưa có thẻ
        [HttpGet]
        [Route("/API/get-khach-hang-chua-co-the")]
        public async Task<IActionResult> GetKhachHangChuaCoThe()
        {
            try
            {
                var allKhachHang = _quanLySevices.GetList<KhachHang>();
                var khachHangCoThe = _quanLySevices.GetList<TheThanhVien>()
                    .Select(t => t.KhachHangId)
                    .ToList();

                var khachHangChuaCoThe = allKhachHang
                    .Where(kh => !khachHangCoThe.Contains(kh.Id))
                    .Select(kh => new
                    {
                        id = kh.Id,
                        hoTen = kh.HoTen,
                        soDienThoai = kh.SoDienThoai
                    })
                    .ToList();

                return Ok(khachHangChuaCoThe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        // Get thẻ  thành viên by ID
        [HttpGet]
        [Route("/API/get-TTV-by-id")]
        public async Task<IActionResult> GetTheThanhVienById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { message = "Id không hợp lệ" });

                var theThanhVien = _quanLySevices.GetList<TheThanhVien>()
                    .FirstOrDefault(t => t.Id == id);

                if (theThanhVien == null)
                    return NotFound(new { message = "Không tìm thấy thẻ thành viên" });

                return Ok(new
                {
                    id = theThanhVien.Id,
                    khachHangId = theThanhVien.KhachHangId,
                    tenKhachHang = theThanhVien.KhachHang?.HoTen,
                    soDienThoai = theThanhVien.KhachHang?.SoDienThoai,
                    hang = theThanhVien.Hang,
                    diemTichLuy = theThanhVien.DiemTichLuy,
                    ngayCap = theThanhVien.NgayCap.ToString("yyyy-MM-dd")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        // Thêm thẻ thành viên
        [HttpPost]
        [Route("/API/add-TTV")]
        public async Task<IActionResult> AddTheThanhVien([FromBody] TheThanhVien theThanhVien)
        {
            try
            {
                if (theThanhVien == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrEmpty(theThanhVien.Id) || string.IsNullOrEmpty(theThanhVien.KhachHangId))
                {
                    return BadRequest(new { message = "Thiếu thông tin bắt buộc." });
                }

                // Kiểm tra khách hàng đã có thẻ chưa
                var existingCard = _quanLySevices.GetList<TheThanhVien>()
                    .FirstOrDefault(t => t.KhachHangId == theThanhVien.KhachHangId && !t.IsDelete);

                if (existingCard != null)
                {
                    return BadRequest(new { message = "Khách hàng đã có thẻ thành viên." });
                }

                theThanhVien.IsDelete = false;

                if (_quanLySevices.Add<TheThanhVien>(theThanhVien))
                {
                    return Ok(new { message = "Thêm thẻ thành viên thành công!" });
                }

                return BadRequest(new { message = "Thêm thẻ thành viên thất bại" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi thêm thẻ thành viên: {ex.Message}" });
            }
        }

        // Sửa thẻ thành viên
        [HttpPost]
        [Route("/API/edit-TTV")]
        public async Task<IActionResult> EditTheThanhVien([FromBody] TheThanhVien theThanhVien)
        {
            try
            {
                if (theThanhVien == null || string.IsNullOrEmpty(theThanhVien.Id))
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                var existing = _quanLySevices.GetById<TheThanhVien>(theThanhVien.Id);
                
                if (existing == null)
                {
                    return NotFound(new { message = "Không tìm thấy thẻ thành viên." });
                }

                // Cập nhật thông tin
                existing.Hang = theThanhVien.Hang;
                existing.DiemTichLuy = theThanhVien.DiemTichLuy;
                existing.NgayCap = theThanhVien.NgayCap;

                if (_quanLySevices.Update<TheThanhVien>(existing))
                {
                    return Ok(new { message = "Sửa thẻ thành viên thành công!" });
                }

                return BadRequest(new { message = "Sửa thẻ thành viên thất bại" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi sửa thẻ thành viên: {ex.Message}" });
            }
        }

        // Xóa Thẻ thành viên
        [HttpDelete]
        [Route("/API/delete-TTV/{id}")]
        public async Task<IActionResult> DeleteTheThanhVien(string id)
        {
            try
            {
                var theThanhVien = _quanLySevices.GetById<TheThanhVien>(id);
                
                if (theThanhVien == null)
                {
                    return NotFound(new { message = "Không tìm thấy thẻ thành viên." });
                }

                if (_quanLySevices.SoftDelete<TheThanhVien>(theThanhVien))
                {
                    return Ok(new { message = "Xóa thành công!" });
                }

                return BadRequest(new { message = "Lỗi khi xoá dữ liệu" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }
    }

    public class DeleteKhachHangRequest
    {
        public string Id { get; set; }
    }
}
