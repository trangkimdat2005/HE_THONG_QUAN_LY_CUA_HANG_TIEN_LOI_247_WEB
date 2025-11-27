using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

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

                await _quanLySevices.BeginTransactionAsync();

                try
                {
                    // Tự động tạo ảnh barcode/QR code từ mã code
                    byte[] imageBytes = null;
                    string tenAnh = "";

                    if (maDinhDanh.LoaiMa.ToUpper() == "BARCODE")
                    {
                        // Tạo barcode
                        imageBytes = BarcodeHelper.GenerateCODE128(maDinhDanh.MaCode, 400, 200);
                        tenAnh = $"Barcode_{maDinhDanh.MaCode}_{DateTime.Now:yyyyMMddHHmmss}.png";
                    }
                    else if (maDinhDanh.LoaiMa.ToUpper() == "QR")
                    {
                        // Tạo QR code
                        imageBytes = QRCodeHelper.GenerateQRCode(maDinhDanh.MaCode, 20);
                        tenAnh = $"QRCode_{maDinhDanh.MaCode}_{DateTime.Now:yyyyMMddHHmmss}.png";
                    }

                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        // Tạo ID cho hình ảnh
                        string anhId = _quanLySevices.GenerateNewId<HinhAnh>("IMG", 10);

                        // Lưu ảnh vào bảng HinhAnh
                        var hinhAnh = new HinhAnh
                        {
                            Id = anhId,
                            TenAnh = tenAnh,
                            Anh = imageBytes
                        };

                        _quanLySevices.Add<HinhAnh>(hinhAnh);

                        // Cập nhật AnhId và DuongDan cho MaDinhDanhSanPham
                        maDinhDanh.AnhId = anhId;
                        maDinhDanh.DuongDan = $"/api/image/{anhId}"; // Đường dẫn để hiển thị ảnh
                    }
                    else
                    {
                        maDinhDanh.DuongDan = "/images/default-code.png";
                    }

                    // Set default values
                    maDinhDanh.IsDelete = false;

                    Console.WriteLine("Adding to database...");
                    _quanLySevices.Add<MaDinhDanhSanPham>(maDinhDanh);
                    
                    if (!await _quanLySevices.CommitAsync("MaDinhDanh"))
                    {
                        return BadRequest(new { message = "Không thể thêm mã định danh." });
                    }

                    Console.WriteLine("Success, sending SignalR notification...");
                    return Ok(new { message = "Thêm mã định danh thành công!", id = maDinhDanh.Id });
                }
                catch (Exception ex)
                {
                    await _quanLySevices.RollbackAsync();
                    Console.WriteLine($"❌ EXCEPTION trong quá trình tạo barcode: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    return StatusCode(500, new { message = $"Lỗi khi tạo barcode: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
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

                await _quanLySevices.BeginTransactionAsync();

                try
                {
                    // Lấy entity hiện tại bằng GetById (được tracked)
                    var existingMaDinhDanh = _quanLySevices.GetById<MaDinhDanhSanPham>(maDinhDanh.Id);

                    if (existingMaDinhDanh == null)
                    {
                        return NotFound(new { message = "Không tìm thấy mã định danh." });
                    }

                    // Lưu các giá trị cần kiểm tra
                    bool maCodeChanged = existingMaDinhDanh.MaCode != maDinhDanh.MaCode;
                    bool loaiMaChanged = existingMaDinhDanh.LoaiMa != maDinhDanh.LoaiMa;
                    string oldAnhId = existingMaDinhDanh.AnhId;

                    if (maCodeChanged || loaiMaChanged)
                    {
                        // Tạo lại ảnh barcode/QR code mới
                        byte[] imageBytes = null;
                        string tenAnh = "";

                        if (maDinhDanh.LoaiMa.ToUpper() == "BARCODE")
                        {
                            imageBytes = BarcodeHelper.GenerateCODE128(maDinhDanh.MaCode, 400, 200);
                            tenAnh = $"Barcode_{maDinhDanh.MaCode}_{DateTime.Now:yyyyMMddHHmmss}.png";
                        }
                        else if (maDinhDanh.LoaiMa.ToUpper() == "QR")
                        {
                            imageBytes = QRCodeHelper.GenerateQRCode(maDinhDanh.MaCode, 20);
                            tenAnh = $"QRCode_{maDinhDanh.MaCode}_{DateTime.Now:yyyyMMddHHmmss}.png";
                        }

                        if (imageBytes != null && imageBytes.Length > 0)
                        {
                            // Xóa ảnh cũ nếu có
                            if (!string.IsNullOrEmpty(oldAnhId))
                            {
                                var oldImage = _quanLySevices.GetById<HinhAnh>(oldAnhId);
                                if (oldImage != null)
                                {
                                    _quanLySevices.HardDelete<HinhAnh>(oldImage);
                                }
                            }

                            // Tạo ảnh mới
                            string anhId = _quanLySevices.GenerateNewId<HinhAnh>("IMG", 10);
                            var hinhAnh = new HinhAnh
                            {
                                Id = anhId,
                                TenAnh = tenAnh,
                                Anh = imageBytes
                            };

                            _quanLySevices.Add<HinhAnh>(hinhAnh);

                            existingMaDinhDanh.AnhId = anhId;
                            existingMaDinhDanh.DuongDan = $"/api/image/{anhId}";
                        }
                    }

                    // Cập nhật các thuộc tính khác
                    existingMaDinhDanh.SanPhamDonViId = maDinhDanh.SanPhamDonViId;
                    existingMaDinhDanh.LoaiMa = maDinhDanh.LoaiMa;
                    existingMaDinhDanh.MaCode = maDinhDanh.MaCode;

                    // Không cần gọi Update vì entity đã được tracked
                    // _quanLySevices.Update<MaDinhDanhSanPham>(existingMaDinhDanh);

                    if (!await _quanLySevices.CommitAsync("MaDinhDanh"))
                    {
                        return BadRequest(new { message = "Không thể cập nhật mã định danh." });
                    }

                    return Ok(new { message = "Sửa mã định danh thành công!" });
                }
                catch (Exception ex)
                {
                    await _quanLySevices.RollbackAsync();
                    Console.WriteLine($"❌ EXCEPTION trong quá trình cập nhật barcode: {ex.Message}");
                    return StatusCode(500, new { message = $"Lỗi khi cập nhật barcode: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
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
                await _quanLySevices.BeginTransactionAsync();
                _quanLySevices.SoftDelete<MaDinhDanhSanPham>(maDinhDanh);
                if (!await _quanLySevices.CommitAsync("MaDinhDanh"))
                {
                    return BadRequest(new { message = "Không thể xóa mã định danh." });
                }


                return Ok(new { message = "Xóa thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
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

                await _quanLySevices.BeginTransactionAsync();

                try
                {
                    // Lấy thông tin mã định danh để lấy AnhId
                    var maDinhDanh = _quanLySevices.GetById<MaDinhDanhSanPham>(temNhan.MaDinhDanhId);
                    if (maDinhDanh == null)
                    {
                        return BadRequest(new { message = "Không tìm thấy mã định danh." });
                    }

                    // Sử dụng AnhId từ mã định danh (barcode/QR đã được tạo sẵn)
                    temNhan.AnhId = maDinhDanh.AnhId;

                    // Set default values
                    temNhan.IsDelete = false;
                    if (temNhan.NgayIn == default(DateTime))
                    {
                        temNhan.NgayIn = DateTime.Now;
                    }

                    Console.WriteLine($"Adding to database with AnhId: {temNhan.AnhId}");
                    _quanLySevices.Add<TemNhan>(temNhan);

                    if (!await _quanLySevices.CommitAsync("TemNhan"))
                    {
                        return BadRequest(new { message = "Không thể thêm tem nhãn." });
                    }

                    Console.WriteLine("Success, sending SignalR notification...");
                    return Ok(new { message = "Thêm tem nhãn thành công! Ảnh barcode/QR được sử dụng từ mã định danh.", id = temNhan.Id });
                }
                catch (Exception ex)
                {
                    await _quanLySevices.RollbackAsync();
                    Console.WriteLine($"❌ EXCEPTION: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    return StatusCode(500, new { message = $"Lỗi khi thêm tem nhãn: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
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

                await _quanLySevices.BeginTransactionAsync();

                try
                {
                    // Lấy entity hiện tại bằng GetById (được tracked)
                    var existingTemNhan = _quanLySevices.GetById<TemNhan>(temNhan.Id);

                    if (existingTemNhan == null)
                    {
                        return NotFound(new { message = "Không tìm thấy tem nhãn." });
                    }

                    // Lưu giá trị cần giữ
                    bool maDinhDanhChanged = existingTemNhan.MaDinhDanhId != temNhan.MaDinhDanhId;

                    // Kiểm tra nếu MaDinhDanhId thay đổi
                    if (maDinhDanhChanged)
                    {
                        // Lấy AnhId mới từ mã định danh mới
                        var maDinhDanh = _quanLySevices.GetById<MaDinhDanhSanPham>(temNhan.MaDinhDanhId);

                        if (maDinhDanh == null)
                        {
                            return BadRequest(new { message = "Không tìm thấy mã định danh mới." });
                        }
                        existingTemNhan.AnhId = maDinhDanh.AnhId;
                    }

                    // Cập nhật các thuộc tính khác
                    existingTemNhan.MaDinhDanhId = temNhan.MaDinhDanhId;
                    existingTemNhan.NoiDungTem = temNhan.NoiDungTem;
                    existingTemNhan.NgayIn = temNhan.NgayIn;

                    // Không cần gọi Update vì entity đã được tracked
                    // _quanLySevices.Update<TemNhan>(existingTemNhan);

                    if (!await _quanLySevices.CommitAsync("TemNhan"))
                    {
                        return BadRequest(new { message = "Không thể cập nhật tem nhãn." });
                    }

                    return Ok(new { message = "Sửa tem nhãn thành công!" });
                }
                catch (Exception ex)
                {
                    await _quanLySevices.RollbackAsync();
                    Console.WriteLine($"❌ EXCEPTION: {ex.Message}");
                    return StatusCode(500, new { message = $"Lỗi khi cập nhật tem nhãn: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
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

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.SoftDelete<TemNhan>(temNhan);

                if (!await _quanLySevices.CommitAsync("TemNhan"))
                {
                    return BadRequest(new { message = "Không thể xóa tem nhãn." });
                }

                return Ok(new { message = "Xóa thành công!" });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
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

                // Lấy đường dẫn ảnh từ mã định danh
                string duongDanAnh = "";
                if (temNhan.MaDinhDanh != null)
                {
                    duongDanAnh = temNhan.MaDinhDanh.DuongDan ?? "";
                }

                return Ok(new
                {
                    id = temNhan.Id,
                    maDinhDanhId = temNhan.MaDinhDanhId,
                    noiDungTem = temNhan.NoiDungTem,
                    ngayIn = temNhan.NgayIn.ToString("yyyy-MM-dd"),
                    duongDanAnh = duongDanAnh
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
                        maCode = tn.MaDinhDanh != null ? tn.MaDinhDanh.MaCode : "",
                        duongDanAnh = tn.MaDinhDanh != null ? tn.MaDinhDanh.DuongDan : "",
                        loaiMa = tn.MaDinhDanh != null ? tn.MaDinhDanh.LoaiMa : ""
                    })
                    .ToList();

                return Ok(lstTemNhan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi: {ex.Message}" });
            }
        }

        // Thêm API endpoint để hiển thị ảnh từ database
        [HttpGet("image/{id}")]
        [AllowAnonymous]
        public IActionResult GetImage(string id)
        {
            try
            {
                var hinhAnh = _quanLySevices.GetById<HinhAnh>(id);
                if (hinhAnh == null || hinhAnh.Anh == null || hinhAnh.Anh.Length == 0)
                {
                    return NotFound();
                }

                return File(hinhAnh.Anh, "image/png");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting image: {ex.Message}");
                return NotFound();
            }
        }
    }
}
