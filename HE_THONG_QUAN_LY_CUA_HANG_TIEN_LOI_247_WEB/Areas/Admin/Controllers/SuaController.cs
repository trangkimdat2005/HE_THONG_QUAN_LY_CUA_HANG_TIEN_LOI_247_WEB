using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SuaController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;
        private readonly IChinhSachHoanTraServices _chinhSachHoanTraServices;

        public SuaController(
            IQuanLyServices quanLySevices,
            IChinhSachHoanTraServices chinhSachHoanTraServices)
        {
            _quanLySevices = quanLySevices;
            _chinhSachHoanTraServices = chinhSachHoanTraServices;
        }

        [Route("/Sua/SuaHoaDon")] 
        public IActionResult SuaHoaDon()
        {
            return View();
        }
        [Route("/Sua/SuaHoanTra")]
        public IActionResult SuaHoanTra()
        {
            return View();
        }
        [Route("/Sua/SuaKhachHang")]
        public IActionResult SuaKhachHang(string id)
        {
            var khachHang = _quanLySevices.GetById<KhachHang>(id);

            if (khachHang == null)
            {
                return NotFound();
            }

            var existingCard = _quanLySevices.GetList<TheThanhVien>()
                                             .FirstOrDefault(t => t.KhachHangId == id);

            ViewData["existingCard"] = existingCard;


            var anh = _quanLySevices.GetList<HinhAnh>().FirstOrDefault(a => a.Id == khachHang.AnhId);
            if (anh != null)
            {
                string base64String = Convert.ToBase64String(anh.Anh);
                ViewBag.AvatarImage = "data:image/jpeg;base64," + base64String;
            }
            else
            {
                ViewBag.AvatarImage = "https://placehold.co/600x400/EEE/31343C";
            }

            return View(khachHang);
        }
        [Route("/Sua/SuaMaKhuyenMai")]
        public IActionResult SuaMaKhuyenMai(string id)
        {
            // Load danh sách danh mục và sản phẩm
            ViewData["DanhMucs"] = _quanLySevices.GetList<DanhMuc>();
            ViewData["SanPhams"] = _quanLySevices.GetList<SanPham>();
            ViewData["ChuongTrinhId"] = id;
            
            return View();
        }
        [Route("/Sua/SuaNCC")]
        public IActionResult SuaNCC(string id)
        {
            var ncc = _quanLySevices.GetList<NhaCungCap>().FirstOrDefault(ncc => ncc.Id == $"{id}");

            if (ncc == null)
            {
                return NotFound();
            }
            return View(ncc);
        }
        [HttpPut] // Dùng động từ PUT
        [Route("/API/NhaCungCap/Update")] // Route mới cho việc cập nhật
        public IActionResult UpdateNhaCungCap([FromBody] NhaCungCapDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nhaCungCapEntity = _quanLySevices.GetById<NhaCungCap>(data.Id);

                if (nhaCungCapEntity == null)
                {
                    return NotFound(new { message = "Không tìm thấy nhà cung cấp này để cập nhật." });
                }

                nhaCungCapEntity.Ten = data.Ten;
                nhaCungCapEntity.SoDienThoai = data.SoDienThoai;
                nhaCungCapEntity.Email = data.Email;
                nhaCungCapEntity.DiaChi = data.DiaChi;
                nhaCungCapEntity.MaSoThue = data.MaSoThue;

                bool success = _quanLySevices.Update<NhaCungCap>(nhaCungCapEntity);

                // 5. Trả về kết quả
                if (success)
                {
                    return Ok(new { message = $"Cập nhật nhà cung cấp '{data.Ten}' thành công!" });
                }
                else
                {
                    return BadRequest(new { message = "Lỗi: Không thể cập nhật nhà cung cấp xuống cơ sở dữ liệu." });
                }
            }
            catch (Exception ex)
            {
                // Bắt các lỗi khác (ví dụ DB sập)
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [HttpPut]
        [Route("/API/PhanCong/Update")]
        public IActionResult UpdatePhanCongCaLamViec([FromBody] PhanCongCaLamViecUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Dùng GetList().FirstOrDefault() như mày yêu cầu (vì Id không phải PK)
                var phanCong = _quanLySevices.GetList<PhanCongCaLamViec>()
                                    .FirstOrDefault(x => x.Id == dto.Id); // Không cần check IsDelete ở đây vì GetList đã lọc rồi

                if (phanCong == null)
                {
                    return NotFound(new { message = "Không tìm thấy ca phân công để cập nhật." });
                }

                // Kiểm tra duplicate
                var existingPhanCong = _quanLySevices.GetList<PhanCongCaLamViec>()
                    .FirstOrDefault(p =>
                        p.NhanVienId == dto.NhanVienId &&
                        p.CaLamViecId == dto.CaLamViecId &&
                        p.Ngay.Date == dto.Ngay.Value.Date &&
                        p.Id != dto.Id && // Quan trọng: Loại trừ chính nó
                        !p.IsDelete); // GetList của mày đã lọc IsDelete=false, nhưng thêm vào cho chắc

                if (existingPhanCong != null)
                {
                    return BadRequest(new { message = "Nhân viên này đã được phân công ca này trong ngày đã chọn." });
                }

                // Map dữ liệu
                phanCong.NhanVienId = dto.NhanVienId;
                phanCong.CaLamViecId = dto.CaLamViecId;
                phanCong.Ngay = dto.Ngay.Value;

                // Cập nhật
                if (_quanLySevices.Update<PhanCongCaLamViec>(phanCong))
                {
                    return Ok(new { message = "Cập nhật phân công thành công!" });
                }
                else
                {
                    return BadRequest(new { message = "Lỗi khi cập nhật phân công." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [HttpGet]
        [Route("/API/HinhAnh/{id}")]
        public IActionResult GetHinhAnh(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var hinhAnh = _quanLySevices.GetById<HinhAnh>(id);

                if (hinhAnh == null || hinhAnh.Anh == null || hinhAnh.Anh.Length == 0)
                {
                    return NotFound();
                }

                string contentType = "image/jpeg"; // Mặc định
                if (hinhAnh.TenAnh != null)
                {
                    if (hinhAnh.TenAnh.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        contentType = "image/png";
                    }
                    else if (hinhAnh.TenAnh.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    {
                        contentType = "image/gif";
                    }
                }

                return File(hinhAnh.Anh, contentType);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("/Sua/SuaNhanSu")]
        public IActionResult SuaNhanSu(string id)
        {
            var nv = _quanLySevices.GetList<NhanVien>().FirstOrDefault(nv => nv.Id == $"{id}");

            if (nv == null)
            {
                return NotFound();
            }
            return View(nv);
        }
        [HttpPut] // Dùng PUT cho Update
        [Route("/API/NhanVien/Update")]
        public async Task<IActionResult> UpdateNhanVien([FromForm] NhanVienUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nhanVien = _quanLySevices.GetById<NhanVien>(dto.Id);
                if (nhanVien == null)
                {
                    return NotFound(new { message = "Không tìm thấy nhân viên để cập nhật." });
                }

                string oldAnhId = nhanVien.AnhId;
                string newAnhId = oldAnhId; // Mặc định là giữ ảnh cũ

                // 3. Xử lý NẾU CÓ ảnh mới tải lên
                if (dto.AnhDaiDien != null && dto.AnhDaiDien.Length > 0)
                {
                    byte[] anhBytes = await _quanLySevices.ConvertImageToByteArray(dto.AnhDaiDien);
                    var newHinhAnh = new HinhAnh
                    {
                        Id = _quanLySevices.GenerateNewId<HinhAnh>("ANH", 7),
                        TenAnh = dto.AnhDaiDien.FileName,
                        Anh = anhBytes
                    };

                    if (_quanLySevices.Add<HinhAnh>(newHinhAnh))
                    {
                        newAnhId = newHinhAnh.Id; // Cập nhật ID ảnh mới

                        // (Tùy chọn: Xóa ảnh cũ)
                        var oldAnh = _quanLySevices.GetById<HinhAnh>(oldAnhId);
                        if (oldAnh != null) _quanLySevices.HardDelete(oldAnh);
                    }
                }

                // 4. Map dữ liệu từ DTO sang Entity
                nhanVien.HoTen = dto.HoTen;
                nhanVien.ChucVu = dto.ChucVu;
                nhanVien.LuongCoBan = (decimal)dto.LuongCoBan; // Ép kiểu vì DTO là decimal?
                nhanVien.SoDienThoai = dto.SoDienThoai;
                nhanVien.Email = dto.Email;
                nhanVien.DiaChi = dto.DiaChi;
                nhanVien.NgayVaoLam = dto.NgayVaoLam;
                nhanVien.TrangThai = dto.TrangThai;
                nhanVien.GioiTinh = dto.GioiTinh;
                nhanVien.AnhId = newAnhId; // Gán ID ảnh (mới hoặc cũ)

                // 5. Lưu thay đổi
                if (_quanLySevices.Update<NhanVien>(nhanVien))
                {
                    return Ok(new { message = $"Cập nhật nhân viên {nhanVien.HoTen} thành công!" });
                }
                else
                {
                    return BadRequest(new { message = "Lỗi khi cập nhật nhân viên." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
        [Route("/Sua/SuaPhanCongCaLamViec")]
        public IActionResult SuaPhanCongCaLamViec(string id)
        {
            var lstNhanVien = _quanLySevices.GetList<NhanVien>();
            ViewData["lstNhanVien"] = lstNhanVien;
            var lstCLV = _quanLySevices.GetList<CaLamViec>();
            ViewData["lstCLV"] = lstCLV;

            var pc = _quanLySevices.GetList<PhanCongCaLamViec>().FirstOrDefault(nv => nv.Id == $"{id}");

            if (pc == null)
            {
                return NotFound();
            }
            return View(pc);
        }
        [Route("/Sua/SuaTaiKhoan")]
        public IActionResult SuaTaiKhoan(string TaiKhoanid, string NhanVienId)
        {
            var lstNhanVien = _quanLySevices.GetList<NhanVien>();
            ViewData["lstNhanVien"] = lstNhanVien;

            var tknv = _quanLySevices.GetList<TaiKhoanNhanVien>()
                .Where(x => x.NhanVienId == NhanVienId && x.TaiKhoanId == TaiKhoanid)
                .FirstOrDefault();

            var lstRole = _quanLySevices.GetList<Role>();
            ViewData["lstRole"] = lstRole;

            var lstUR = _quanLySevices.GetList<UserRole>();
            ViewData["lstUR"] = lstUR;

            if (tknv == null)
            {
                return NotFound();
            }
            return View(tknv);
        }

        // ==================== CHÍNH SÁCH HOÀN TRẢ ====================
        
        [HttpGet]
        [Route("/Sua/SuaChinhSachHoanTra/{id}")]
        public IActionResult SuaChinhSachHoanTra(string id)
        {
            var chinhSach = _chinhSachHoanTraServices.GetChinhSachById(id);
            
            if (chinhSach == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy chính sách hoàn trả!";
                return RedirectToAction("ChinhSachDoiTra", "GiaoDichHoanTra");
            }

            ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
            return View(chinhSach);
        }

        [HttpPost]
        [Route("/Sua/SuaChinhSachHoanTra/{id}")]
        public IActionResult SuaChinhSachHoanTra(
            string id,
            string TenChinhSach, 
            int? ThoiHan, 
            string DieuKien, 
            bool ApDungToanBo, 
            DateTime ApDungTuNgay, 
            DateTime ApDungDenNgay,
            List<string> DanhMucIds)
        {
            try
            {
                // Validate dữ liệu
                if (string.IsNullOrWhiteSpace(TenChinhSach))
                {
                    TempData["ErrorMessage"] = "Tên chính sách không được để trống!";
                    var chinhSachError1 = _chinhSachHoanTraServices.GetChinhSachById(id);
                    ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                    return View(chinhSachError1);
                }

                if (string.IsNullOrWhiteSpace(DieuKien))
                {
                    TempData["ErrorMessage"] = "Điều kiện áp dụng không được để trống!";
                    var chinhSachError2 = _chinhSachHoanTraServices.GetChinhSachById(id);
                    ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                    return View(chinhSachError2);
                }

                if (ApDungTuNgay >= ApDungDenNgay)
                {
                    TempData["ErrorMessage"] = "Ngày áp dụng đến phải sau ngày áp dụng từ!";
                    var chinhSachError3 = _chinhSachHoanTraServices.GetChinhSachById(id);
                    ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                    return View(chinhSachError3);
                }

                if (!ApDungToanBo && (DanhMucIds == null || !DanhMucIds.Any()))
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một danh mục hoặc chọn 'Áp dụng toàn bộ'!";
                    var chinhSachError4 = _chinhSachHoanTraServices.GetChinhSachById(id);
                    ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                    return View(chinhSachError4);
                }

                // Tạo object ChinhSachHoanTra
                var chinhSach = new ChinhSachHoanTra
                {
                    Id = id,
                    TenChinhSach = TenChinhSach,
                    ThoiHan = ThoiHan,
                    DieuKien = DieuKien,
                    ApDungToanBo = ApDungToanBo,
                    ApDungTuNgay = ApDungTuNgay,
                    ApDungDenNgay = ApDungDenNgay
                };

                var result = _chinhSachHoanTraServices.UpdateChinhSach(chinhSach);
                
                if (result)
                {
                    // Cập nhật danh mục
                    if (!ApDungToanBo && DanhMucIds != null && DanhMucIds.Any())
                    {
                        _chinhSachHoanTraServices.AddDanhMucToChinhSach(id, DanhMucIds);
                    }
                    else if (ApDungToanBo)
                    {
                        // Nếu áp dụng toàn bộ, xóa tất cả danh mục
                        _chinhSachHoanTraServices.AddDanhMucToChinhSach(id, new List<string>());
                    }

                    TempData["SuccessMessage"] = "Cập nhật chính sách hoàn trả thành công!";
                    return RedirectToAction("ChinhSachDoiTra", "GiaoDichHoanTra");
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật chính sách!";
                }

                var chinhSachReload = _chinhSachHoanTraServices.GetChinhSachById(id);
                ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                return View(chinhSachReload);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
                var chinhSachException = _chinhSachHoanTraServices.GetChinhSachById(id);
                ViewData["DanhSachDanhMuc"] = _chinhSachHoanTraServices.GetAllDanhMuc();
                return View(chinhSachException);
            }
        }
        //[HttpGet("get-PC-by-id")]
        //public async Task<IActionResult> GetPhanCongById([FromBody] PhanCongCaLamViec pc)
        //{
        //    if (pc==null)
        //        return BadRequest("Phân công không hợp lệ");

        //    var x = _quanLySevices.GetById<PhanCongCaLamViec>(pc.NhanVienId,pc.CaLamViecId);

        //    if (x == null)
        //        return NotFound("Không tìm thấy phân công ca này");

        //    var nvs = _quanLySevices.GetList<NhanVien>();
        //    var clv = _quanLySevices.GetList<CaLamViec>();
        //    return Ok(new
        //    {
        //        nhanViens = nvs,
        //        caLamViecs =clv,
        //        pc=x
        //    });
        //}
        
        //=========================================API Sửa Chương Trình Khuyến Mãi=======================================================================
        [HttpPost]
        [Route("/API/edit-CTKM")]
        public async Task<IActionResult> EditChuongTrinhKhuyenMai([FromBody] ChuongTrinhKhuyenMai chuongTrinh)
        {
            try
            {
                if (chuongTrinh == null || string.IsNullOrEmpty(chuongTrinh.Id))
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                if (string.IsNullOrWhiteSpace(chuongTrinh.Ten))
                {
                    return BadRequest(new { message = "Tên chương trình không được để trống." });
                }

                if (_quanLySevices.Update<ChuongTrinhKhuyenMai>(chuongTrinh))
                {
                    return Ok(new { message = "Sửa chương trình khuyến mãi thành công!" });
                }

                return BadRequest(new { message = "Sửa chương trình khuyến mãi thất bại." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi sửa chương trình khuyến mãi: {ex.Message}" });
            }
        }

        //=========================================API Sửa Khách Hàng=======================================================================
        [HttpPost]
        [Route("/API/edit-KhachHang")]
        public async Task<IActionResult> EditKhachHang([FromBody] KhachHangEditRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Id))
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ." });
                }

                // Validate
                if (string.IsNullOrWhiteSpace(request.HoTen))
                {
                    return BadRequest(new { message = "Họ tên không được để trống." });
                }

                if (string.IsNullOrWhiteSpace(request.SoDienThoai))
                {
                    return BadRequest(new { message = "Số điện thoại không được để trống." });
                }

                // Lấy khách hàng hiện tại
                var khachHang = _quanLySevices.GetList<KhachHang>()
                    .FirstOrDefault(kh => kh.Id == request.Id && !kh.IsDelete);

                if (khachHang == null)
                {
                    return BadRequest(new { message = "Không tìm thấy khách hàng." });
                }

                // Cập nhật thông tin
                khachHang.HoTen = request.HoTen;
                khachHang.SoDienThoai = request.SoDienThoai;
                khachHang.Email = request.Email;
                khachHang.DiaChi = request.DiaChi ?? "";
                khachHang.NgayDangKy = request.NgayDangKy ?? khachHang.NgayDangKy;
                khachHang.TrangThai = request.TrangThai ?? khachHang.TrangThai;
                khachHang.GioiTinh = request.GioiTinh;
                // AnhId giữ nguyên hoặc cập nhật sau khi upload ảnh

                if (!_quanLySevices.Update<KhachHang>(khachHang))
                {
                    return BadRequest(new { message = "Không thể cập nhật khách hàng." });
                }

                // Cập nhật hoặc tạo thẻ thành viên nếu có
                if (request.UpdateMemberCard && request.TheThanhVien != null)
                {
                    var existingCard = _quanLySevices.GetList<TheThanhVien>()
                        .FirstOrDefault(ttv => ttv.KhachHangId == khachHang.Id && !ttv.IsDelete);

                    if (existingCard != null)
                    {
                        // Cập nhật thẻ hiện có
                        existingCard.Hang = request.TheThanhVien.Hang ?? existingCard.Hang;
                        existingCard.DiemTichLuy = request.TheThanhVien.DiemTichLuy;
                        existingCard.NgayCap = request.TheThanhVien.NgayCap ?? existingCard.NgayCap;

                        if (!_quanLySevices.Update<TheThanhVien>(existingCard))
                        {
                            return BadRequest(new { message = "Cập nhật khách hàng thành công nhưng không thể cập nhật thẻ thành viên." });
                        }
                    }
                    else
                    {
                        // Tạo thẻ mới
                        TheThanhVien newCard = new TheThanhVien
                        {
                            Id = _quanLySevices.GenerateNewId<TheThanhVien>("TTV", 8),
                            KhachHangId = khachHang.Id,
                            Hang = request.TheThanhVien.Hang ?? "Bronze",
                            DiemTichLuy = request.TheThanhVien.DiemTichLuy,
                            NgayCap = request.TheThanhVien.NgayCap ?? DateTime.Now,
                            IsDelete = false
                        };

                        if (!_quanLySevices.Add<TheThanhVien>(newCard))
                        {
                            return BadRequest(new { message = "Cập nhật khách hàng thành công nhưng không thể tạo thẻ thành viên." });
                        }
                    }
                }

                return Ok(new { message = "Cập nhật khách hàng thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi khi cập nhật khách hàng: {ex.Message}" });
            }
        }
    }

    // Request class cho Edit Khách Hàng
    public class KhachHangEditRequest
    {
        public string Id { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public DateTime? NgayDangKy { get; set; }
        public string TrangThai { get; set; }
        public bool GioiTinh { get; set; }
        public bool UpdateMemberCard { get; set; }
        public TheThanhVienEditRequest TheThanhVien { get; set; }
    }

    public class TheThanhVienEditRequest
    {
        public string Hang { get; set; }
        public int DiemTichLuy { get; set; }
        public DateTime? NgayCap { get; set; }
    }
}
