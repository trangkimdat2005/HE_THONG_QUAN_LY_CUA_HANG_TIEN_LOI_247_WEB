using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class QuanLyBarCodeController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;
        private readonly IRealtimeNotifier _notifier;

        public QuanLyBarCodeController(IQuanLyServices quanLySevices, IRealtimeNotifier notifier)
        {
            _quanLySevices = quanLySevices;
            _notifier = notifier;
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG,NV_KHO")]
        [Route("/QuanLyBarCode/DanhSachTemNhan")]
        public IActionResult DanhSachTemNhan()
        {
            var lstTemNhan = _quanLySevices.GetList<TemNhan>();
            var lstMaDinhDanh = _quanLySevices.GetList<MaDinhDanhSanPham>();

            ViewData["lstTemNhan"] = lstTemNhan;
            ViewData["lstMaDinhDanh"] = lstMaDinhDanh;

            return View();
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG,NV_KHO")]
        [Route("/QuanLyBarCode/DinhDanhSanPham")]
        public IActionResult DinhDanhSanPham()
        {
            var lstMaDinhDanh = _quanLySevices.GetList<MaDinhDanhSanPham>();
            var lstSanPhamDonVi = _quanLySevices.GetList<SanPhamDonVi>();

            ViewData["lstMaDinhDanh"] = lstMaDinhDanh;
            ViewData["lstSanPhamDonVi"] = lstSanPhamDonVi;

            return View();
        }

        // Lấy ID tiếp theo cho mã định danh
        [HttpPost("get-next-id-MDD")]
        public Task<IActionResult> GetNextIdMDD([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new 
            { 
                NextId = _quanLySevices.GenerateNewId<MaDinhDanhSanPham>(
                    request["prefix"].ToString(), 
                    int.Parse(request["totalLength"].ToString())
                ) 
            }));
        }

        // Thêm mã định danh mới
        [HttpPost("add-MDD")]
        public async Task<IActionResult> AddMaDinhDanh([FromBody] MaDinhDanhSanPham maDinhDanh)
        {
            try
            {
                Console.WriteLine($"Data: Id={maDinhDanh?.Id}, SanPhamDonViId={maDinhDanh?.SanPhamDonViId}, LoaiMa={maDinhDanh?.LoaiMa}");

                if (maDinhDanh == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(maDinhDanh.SanPhamDonViId))
                {
                    return BadRequest(new { message = "Vui lòng chọn sản phẩm đơn vị." });
                }

                if (string.IsNullOrWhiteSpace(maDinhDanh.LoaiMa))
                {
                    return BadRequest(new { message = "Vui lòng chọn loại mã." });
                }

                if (string.IsNullOrWhiteSpace(maDinhDanh.MaCode))
                {
                    return BadRequest(new { message = "Mã code không được để trống." });
                }

                // Set default values
                maDinhDanh.IsDelete = false;
                if (string.IsNullOrEmpty(maDinhDanh.DuongDan))
                {
                    maDinhDanh.DuongDan = "/images/default-code.png";
                }

                Console.WriteLine("Adding to database...");
                if (!_quanLySevices.Add<MaDinhDanhSanPham>(maDinhDanh))
                {
                    return BadRequest(new { message = "Không thể thêm mã định danh." });
                }

                Console.WriteLine("Success, sending SignalR notification...");
                await _notifier.NotifyReloadAsync("MaDinhDanh");

                return Ok(new { message = "Thêm mã định danh thành công!", id = maDinhDanh.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ EXCEPTION: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = $"Lỗi khi thêm mã định danh: {ex.Message}" });
            }
        }

        // Sửa mã định danh
        [HttpPost("edit-MDD")]
        public async Task<IActionResult> EditMaDinhDanh([FromBody] MaDinhDanhSanPham maDinhDanh)
        {
            try
            {
                if (maDinhDanh == null || string.IsNullOrWhiteSpace(maDinhDanh.Id))
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(maDinhDanh.SanPhamDonViId))
                {
                    return BadRequest(new { message = "Vui lòng chọn sản phẩm đơn vị." });
                }

                if (string.IsNullOrWhiteSpace(maDinhDanh.LoaiMa))
                {
                    return BadRequest(new { message = "Vui lòng chọn loại mã." });
                }

                if (string.IsNullOrWhiteSpace(maDinhDanh.MaCode))
                {
                    return BadRequest(new { message = "Mã code không được để trống." });
                }

                if (!_quanLySevices.Update<MaDinhDanhSanPham>(maDinhDanh))
                {
                    return BadRequest(new { message = "Không thể cập nhật mã định danh." });
                }

                await _notifier.NotifyReloadAsync("MaDinhDanh");

                return Ok(new { message = "Sửa mã định danh thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi sửa mã định danh: {ex.Message}" });
            }
        }

        // Xóa mã định danh
        [HttpDelete("delete-MDD/{id}")]
        public async Task<IActionResult> DeleteMaDinhDanh(string id)
        {
            try
            {
                var maDinhDanh = _quanLySevices.GetById<MaDinhDanhSanPham>(id);
                if (maDinhDanh == null)
                {
                    return NotFound(new { message = "Không tìm thấy mã định danh." });
                }

                if (!_quanLySevices.SoftDelete<MaDinhDanhSanPham>(maDinhDanh))
                {
                    return BadRequest(new { message = "Không thể xóa mã định danh." });
                }

                await _notifier.NotifyReloadAsync("MaDinhDanh");

                return Ok(new { message = "Xóa thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi xóa mã định danh: {ex.Message}" });
            }
        }

        // Lấy thông tin mã định danh theo ID
        [HttpGet("get-MDD-by-id")]
        public async Task<IActionResult> GetMaDinhDanhById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "Id không hợp lệ" });
                }

                var maDinhDanh = _quanLySevices.GetById<MaDinhDanhSanPham>(id);

                if (maDinhDanh == null)
                {
                    return NotFound(new { message = "Không tìm thấy mã định danh" });
                }

                return Ok(new
                {
                    id = maDinhDanh.Id,
                    sanPhamDonViId = maDinhDanh.SanPhamDonViId,
                    loaiMa = maDinhDanh.LoaiMa,
                    maCode = maDinhDanh.MaCode,
                    duongDan = maDinhDanh.DuongDan
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        // Lấy tất cả mã định danh
        [HttpGet("get-all-MDD")]
        public async Task<IActionResult> GetAllMaDinhDanh()
        {
            try
            {
                var lstMaDinhDanh = _quanLySevices.GetList<MaDinhDanhSanPham>()
                    .Where(mdd => !mdd.IsDelete)
                    .Select(mdd => new
                    {
                        id = mdd.Id,
                        sanPhamDonViId = mdd.SanPhamDonViId,
                        loaiMa = mdd.LoaiMa,
                        maCode = mdd.MaCode,
                        duongDan = mdd.DuongDan,
                        tenSp = mdd.SanPhamDonVi != null ? mdd.SanPhamDonVi.SanPham.Ten : ""
                    })
                    .ToList();

                return Ok(lstMaDinhDanh);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        // ==================== TEM NHÃN API ENDPOINTS ====================

        // Lấy ID tiếp theo cho tem nhãn
        [HttpPost("get-next-id-TN")]
        public Task<IActionResult> GetNextIdTN([FromBody] Dictionary<string, object> request)
        {
            return Task.FromResult<IActionResult>(Ok(new 
            { 
                NextId = _quanLySevices.GenerateNewId<TemNhan>(
                    request["prefix"].ToString(), 
                    int.Parse(request["totalLength"].ToString())
                ) 
            }));
        }

        // Thêm tem nhãn mới
        [HttpPost("add-TN")]
        public async Task<IActionResult> AddTemNhan([FromBody] TemNhan temNhan)
        {
            try
            {
                Console.WriteLine($"Data: Id={temNhan?.Id}, MaDinhDanhId={temNhan?.MaDinhDanhId}");

                if (temNhan == null)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(temNhan.MaDinhDanhId))
                {
                    return BadRequest(new { message = "Vui lòng chọn mã định danh." });
                }

                if (string.IsNullOrWhiteSpace(temNhan.NoiDungTem))
                {
                    return BadRequest(new { message = "Nội dung tem không được để trống." });
                }

                // Set default values
                temNhan.IsDelete = false;
                if (temNhan.NgayIn == default(DateTime))
                {
                    temNhan.NgayIn = DateTime.Now;
                }

                Console.WriteLine("Adding to database...");
                if (!_quanLySevices.Add<TemNhan>(temNhan))
                {
                    return BadRequest(new { message = "Không thể thêm tem nhãn." });
                }

                Console.WriteLine("Success, sending SignalR notification...");
                await _notifier.NotifyReloadAsync("TemNhan");

                return Ok(new { message = "Thêm tem nhãn thành công!", id = temNhan.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = $"Lỗi khi thêm tem nhãn: {ex.Message}" });
            }
        }

        // Sửa tem nhãn
        [HttpPost("edit-TN")]
        public async Task<IActionResult> EditTemNhan([FromBody] TemNhan temNhan)
        {
            try
            {
                if (temNhan == null || string.IsNullOrWhiteSpace(temNhan.Id))
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(temNhan.MaDinhDanhId))
                {
                    return BadRequest(new { message = "Vui lòng chọn mã định danh." });
                }

                if (string.IsNullOrWhiteSpace(temNhan.NoiDungTem))
                {
                    return BadRequest(new { message = "Nội dung tem không được để trống." });
                }

                if (!_quanLySevices.Update<TemNhan>(temNhan))
                {
                    return BadRequest(new { message = "Không thể cập nhật tem nhãn." });
                }

                await _notifier.NotifyReloadAsync("TemNhan");

                return Ok(new { message = "Sửa tem nhãn thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi sửa tem nhãn: {ex.Message}" });
            }
        }

        // Xóa tem nhãn
        [HttpDelete("delete-TN/{id}")]
        public async Task<IActionResult> DeleteTemNhan(string id)
        {
            try
            {
                var temNhan = _quanLySevices.GetById<TemNhan>(id);
                if (temNhan == null)
                {
                    return NotFound(new { message = "Không tìm thấy tem nhãn." });
                }

                if (!_quanLySevices.SoftDelete<TemNhan>(temNhan))
                {
                    return BadRequest(new { message = "Không thể xóa tem nhãn." });
                }

                await _notifier.NotifyReloadAsync("TemNhan");

                return Ok(new { message = "Xóa thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi xóa tem nhãn: {ex.Message}" });
            }
        }

        // Lấy thông tin tem nhãn theo ID
        [HttpGet("get-TN-by-id")]
        public async Task<IActionResult> GetTemNhanById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "Id không hợp lệ" });
                }

                var temNhan = _quanLySevices.GetById<TemNhan>(id);

                if (temNhan == null)
                {
                    return NotFound(new { message = "Không tìm thấy tem nhãn" });
                }

                return Ok(new
                {
                    id = temNhan.Id,
                    maDinhDanhId = temNhan.MaDinhDanhId,
                    noiDungTem = temNhan.NoiDungTem,
                    ngayIn = temNhan.NgayIn.ToString("yyyy-MM-dd")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        // Lấy tất cả tem nhãn
        [HttpGet("get-all-TN")]
        public async Task<IActionResult> GetAllTemNhan()
        {
            try
            {
                var lstTemNhan = _quanLySevices.GetList<TemNhan>()
                    .Where(tn => !tn.IsDelete)
                    .Select(tn => new
                    {
                        id = tn.Id,
                        maDinhDanhId = tn.MaDinhDanhId,
                        noiDungTem = tn.NoiDungTem,
                        ngayIn = tn.NgayIn.ToString("yyyy-MM-dd"),
                        maCode = tn.MaDinhDanh != null ? tn.MaDinhDanh.MaCode : ""
                    })
                    .ToList();

                return Ok(lstTemNhan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }
    }
}
