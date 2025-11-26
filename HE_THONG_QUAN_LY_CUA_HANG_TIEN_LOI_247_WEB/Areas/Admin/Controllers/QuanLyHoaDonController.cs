using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class QuanLyHoaDonController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyHoaDonController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/QuanLyHoaDon/DanhSachHoaDon")]
        public IActionResult DanhSachHoaDon()
        {
            var lstHoaDon = _quanLySevices.GetList<HoaDon>();
            ViewData["lstHoaDon"] = lstHoaDon;
            return View();
        }
        
        [Authorize(Roles = "ADMIN,NV_BANHANG")]
        [Route("/Admin/QuanLyHoaDon/ThanhToanHoaDon/{hoaDonId}")]
        public IActionResult ThanhToanHoaDon(string hoaDonId)
        {
            // Load thông tin hóa đơn
            var hoaDon = _quanLySevices.GetById<HoaDon>(hoaDonId);
            
            if (hoaDon == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hóa đơn!";
                return RedirectToAction("DanhSachHoaDon");
            }

            // Load chi tiết hóa đơn
            var chiTietHoaDon = _quanLySevices.GetList<ChiTietHoaDon>()
                .Where(ct => ct.HoaDonId == hoaDonId && !ct.IsDelete)
                .ToList();

            // Load thông tin sản phẩm cho mỗi chi tiết
            foreach (var chiTiet in chiTietHoaDon)
            {
                if (!string.IsNullOrEmpty(chiTiet.SanPhamDonViId))
                {
                    chiTiet.SanPhamDonVi = _quanLySevices.GetList<SanPhamDonVi>()
                        .FirstOrDefault(sp => sp.Id == chiTiet.SanPhamDonViId);

                    if (chiTiet.SanPhamDonVi != null)
                    {
                        if (!string.IsNullOrEmpty(chiTiet.SanPhamDonVi.SanPhamId))
                        {
                            chiTiet.SanPhamDonVi.SanPham = _quanLySevices.GetById<SanPham>(chiTiet.SanPhamDonVi.SanPhamId);
                        }

                        if (!string.IsNullOrEmpty(chiTiet.SanPhamDonVi.DonViId))
                        {
                            chiTiet.SanPhamDonVi.DonVi = _quanLySevices.GetById<DonViDoLuong>(chiTiet.SanPhamDonVi.DonViId);
                        }
                    }
                }
            }

            // Load thông tin khách hàng
            if (!string.IsNullOrEmpty(hoaDon.KhachHangId))
            {
                hoaDon.KhachHang = _quanLySevices.GetById<KhachHang>(hoaDon.KhachHangId);
            }

            // Load thông tin nhân viên
            if (!string.IsNullOrEmpty(hoaDon.NhanVienId))
            {
                hoaDon.NhanVien = _quanLySevices.GetById<NhanVien>(hoaDon.NhanVienId);
            }

            // Load danh sách kênh thanh toán
            var lstKenhThanhToan = _quanLySevices.GetList<KenhThanhToan>()
                .Where(k => !k.IsDelete && k.TrangThai == "Hoạt động")
                .ToList();

            ViewData["HoaDon"] = hoaDon;
            ViewData["ChiTietHoaDon"] = chiTietHoaDon;
            ViewData["DanhSachKenhThanhToan"] = lstKenhThanhToan;

            return View();
        }

        // API xử lý thanh toán
        [HttpPost]
        [Route("/API/thanh-toan-hoa-don")]
        public async Task<IActionResult> ThanhToanHoaDonAPI([FromBody] ThanhToanRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.HoaDonId))
                {
                    return BadRequest(new { message = "Thiếu thông tin hóa đơn." });
                }

                if (string.IsNullOrEmpty(request.KenhThanhToanId))
                {
                    return BadRequest(new { message = "Vui lòng chọn phương thức thanh toán." });
                }

                var hoaDon = _quanLySevices.GetById<HoaDon>(request.HoaDonId);
                
                if (hoaDon == null)
                {
                    return NotFound(new { message = "Không tìm thấy hóa đơn." });
                }

                // Cập nhật trạng thái hóa đơn
                hoaDon.TrangThai = "Đã thanh toán";

                await _quanLySevices.BeginTransactionAsync();

                _quanLySevices.Update<HoaDon>(hoaDon);

                if (!await _quanLySevices.CommitAsync())
                {
                    return BadRequest(new { message = "Không thể cập nhật trạng thái hóa đơn." });
                }

                // Tạo giao dịch thanh toán
                GiaoDichThanhToan giaoDich = new GiaoDichThanhToan
                {
                    Id = _quanLySevices.GenerateNewId<GiaoDichThanhToan>("GDTT", 8),
                    HoaDonId = request.HoaDonId,
                    KenhThanhToanId = request.KenhThanhToanId,
                    SoTien = hoaDon.TongTien ?? 0,
                    NgayGd = DateTime.Now,
                    MoTa = request.MoTa ?? "",
                    IsDelete = false
                };

                return Ok(new 
                { 
                    message = "Thanh toán thành công!", 
                    hoaDonId = hoaDon.Id,
                    giaoDichId = giaoDich.Id
                });
            }
            catch (Exception ex)
            {
                await _quanLySevices.RollbackAsync();
                return StatusCode(500, new { message = $"Lỗi khi thanh toán: {ex.Message}" });
            }
        }

        public class ThanhToanRequest
        {
            public string HoaDonId { get; set; }
            public string KenhThanhToanId { get; set; }
            public string MoTa { get; set; }
        }
    }
}
