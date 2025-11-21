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
    public class ThanhToanController : Controller
    {
        private readonly IQuanLyServices _quanLyServices;
        private readonly IRealtimeNotifier _notifier;

        public ThanhToanController(IQuanLyServices quanLyServices, IRealtimeNotifier notifier)
        {
            _quanLyServices = quanLyServices;
            _notifier = notifier;
        }

        //=========================================LoadView=========================================================

        [Authorize(Roles = "ADMIN")]
        [Route("/ThanhToan/ThanhToan")]
        public IActionResult ThanhToan()
        {
            var lstKenhThanhToan = _quanLyServices.GetList<KenhThanhToan>();
            ViewData["lstKenhThanhToan"] = lstKenhThanhToan;
            return View();
        }

        //=========================================GetAllData=======================================================================
        [HttpGet("get-all-Kenh")]
        public async Task<IActionResult> GetAllKenh()
        {
            try
            {
                var lstKenh = _quanLyServices.GetList<KenhThanhToan>()
                    .Where(k => k.IsDelete == false)
                    .Select(k => new
                    {
                        id = k.Id,
                        tenKenh = k.TenKenh,
                        loaiKenh = k.LoaiKenh,
                        phiGiaoDich = k.PhiGiaoDich,
                        trangThai = k.TrangThai,
                        cauHinh = k.CauHinh
                    })
                    .ToList();

                return Ok(lstKenh);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy danh sách kênh thanh toán: {ex.Message}" });
            }
        }

        //=========================================GetDataById=======================================================================
        [HttpGet("get-Kenh-by-id")]
        public async Task<IActionResult> GetKenhById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { message = "Id không hợp lệ" });

                var kenh = _quanLyServices.GetById<KenhThanhToan>(id);

                if (kenh == null || kenh.IsDelete)
                    return NotFound(new { message = "Không tìm thấy kênh thanh toán" });

                return Ok(new
                {
                    id = kenh.Id,
                    tenKenh = kenh.TenKenh,
                    loaiKenh = kenh.LoaiKenh,
                    phiGiaoDich = kenh.PhiGiaoDich,
                    trangThai = kenh.TrangThai,
                    cauHinh = kenh.CauHinh
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi lấy thông tin kênh thanh toán: {ex.Message}" });
            }
        }

        //=========================================AddData=======================================================================
        [HttpPost("add-Kenh")]
        public async Task<IActionResult> AddKenh([FromBody] KenhThanhToan kenh)
        {
            try
            {

                if (kenh == null)
                {
                    Console.WriteLine("ERROR: kenh is null");
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                if (string.IsNullOrWhiteSpace(kenh.Id))
                {
                    Console.WriteLine("ERROR: Id is empty");
                    return BadRequest(new { message = "Mã kênh không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(kenh.TenKenh))
                {
                    Console.WriteLine("ERROR: TenKenh is empty");
                    return BadRequest(new { message = "Tên kênh không được để trống." });
                }

                var existingKenh = _quanLyServices.GetList<KenhThanhToan>()
                    .FirstOrDefault(k => k.Id == kenh.Id && !k.IsDelete);

                if (existingKenh != null)
                {
                    Console.WriteLine($"ERROR: Id '{kenh.Id}' already exists");
                    return BadRequest(new { message = $"Mã kênh '{kenh.Id}' đã tồn tại." });
                }

                kenh.IsDelete = false;

                Console.WriteLine("Adding kenh to database...");
                if (!_quanLyServices.Add<KenhThanhToan>(kenh))
                {
                    Console.WriteLine("ERROR: Add failed");
                    return BadRequest(new { message = "Không thể thêm kênh thanh toán." });
                }

                Console.WriteLine(" SUCCESS");
               
                await _notifier.NotifyReloadAsync("KenhThanhToan");

                return Ok(new { message = "Thêm kênh thanh toán thành công!", kenhId = kenh.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($" EXCEPTION: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi khi thêm kênh thanh toán: {ex.Message}" });
            }
        }

        //=========================================EditData=======================================================================
        [HttpPost("edit-Kenh")]
        public async Task<IActionResult> EditKenh([FromBody] KenhThanhToan kenh)
        {
            try
            {

                if (kenh == null)
                {
                    Console.WriteLine("ERROR: kenh is null");
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                if (string.IsNullOrWhiteSpace(kenh.Id))
                {
                    Console.WriteLine("ERROR: Id is empty");
                    return BadRequest(new { message = "Mã kênh không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(kenh.TenKenh))
                {
                    Console.WriteLine("ERROR: TenKenh is empty");
                    return BadRequest(new { message = "Tên kênh không được để trống." });
                }

                var existingKenh = _quanLyServices.GetById<KenhThanhToan>(kenh.Id);

                if (existingKenh == null || existingKenh.IsDelete)
                {
                    Console.WriteLine($"ERROR: Kenh '{kenh.Id}' not found");
                    return NotFound(new { message = "Không tìm thấy kênh thanh toán." });
                }

                existingKenh.TenKenh = kenh.TenKenh;
                existingKenh.LoaiKenh = kenh.LoaiKenh;
                existingKenh.PhiGiaoDich = kenh.PhiGiaoDich;
                existingKenh.TrangThai = kenh.TrangThai;
                existingKenh.CauHinh = kenh.CauHinh;

                Console.WriteLine("Updating kenh in database...");
                if (!_quanLyServices.Update<KenhThanhToan>(existingKenh))
                {
                    Console.WriteLine("ERROR: Update failed");
                    return BadRequest(new { message = "Không thể cập nhật kênh thanh toán." });
                }

                Console.WriteLine(" SUCCESS");
                
                await _notifier.NotifyReloadAsync("KenhThanhToan");

                return Ok(new { message = "Cập nhật kênh thanh toán thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($" EXCEPTION: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi khi cập nhật kênh thanh toán: {ex.Message}" });
            }
        }

        //=========================================DeleteData=======================================================================
        [HttpDelete("delete-Kenh/{id}")]
        public async Task<IActionResult> DeleteKenh(string id)
        {
            try
            {

                if (string.IsNullOrEmpty(id))
                {
                    Console.WriteLine("ERROR: Id is empty");
                    return BadRequest(new { message = "Mã kênh không hợp lệ." });
                }

                var kenh = _quanLyServices.GetById<KenhThanhToan>(id);

                if (kenh == null || kenh.IsDelete)
                {
                    Console.WriteLine($"ERROR: Kenh '{id}' not found");
                    return NotFound(new { message = "Không tìm thấy kênh thanh toán." });
                }

                Console.WriteLine("Soft deleting kenh...");
                if (!_quanLyServices.SoftDelete<KenhThanhToan>(kenh))
                {
                    Console.WriteLine("ERROR: Delete failed");
                    return BadRequest(new { message = "Không thể xóa kênh thanh toán." });
                }

                Console.WriteLine(" SUCCESS");
                
                await _notifier.NotifyReloadAsync("KenhThanhToan");

                return Ok(new { message = "Xóa kênh thanh toán thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($" EXCEPTION: {ex.Message}");
                return StatusCode(500, new { message = $"Lỗi khi xóa kênh thanh toán: {ex.Message}" });
            }
        }

        //=========================================GetNextID=======================================================================
        [HttpPost("get-next-id-Kenh")]
        public Task<IActionResult> GetNextIdKenh([FromBody] Dictionary<string, object> request)
        {
            try
            {
                string prefix = request["prefix"].ToString();
                int totalLength = int.Parse(request["totalLength"].ToString());

                string nextId = _quanLyServices.GenerateNewId<KenhThanhToan>(prefix, totalLength);

                return Task.FromResult<IActionResult>(Ok(new { nextId = nextId }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating next ID: {ex.Message}");
                return Task.FromResult<IActionResult>(StatusCode(500, new { message = "Lỗi khi tạo mã mới." }));
            }
        }
    }
}
