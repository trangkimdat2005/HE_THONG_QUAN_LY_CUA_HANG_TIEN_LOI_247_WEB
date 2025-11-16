using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
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
            var lstTTV = _quanLySevices.GetList<TheThanhVien>();
            ViewData["lstTTV"] = lstTTV;
            // Lấy thông tin khách hàng từ cơ sở dữ liệu bằng Id
            var khachHang = _quanLySevices.GetList<KhachHang>().FirstOrDefault(kh => kh.Id == $"{id}");


            // Nếu không tìm thấy khách hàng, trả về lỗi
            if (khachHang == null)
            {
                return NotFound();
            }
            var anh = _quanLySevices.GetList<HinhAnh>().FirstOrDefault(a => a.Id == khachHang.AnhId);
            if (anh != null)
            {
                // Chuyển ảnh thành Base64
                string base64String = Convert.ToBase64String(anh.Anh);
                ViewBag.AvatarImage = "data:image/jpeg;base64," + base64String;
            }
            else
            {
                // Nếu không có ảnh, dùng ảnh mặc định
                ViewBag.AvatarImage = "https://placehold.co/600x400/EEE/31343C";
            }
            // Trả về view sửa thông tin khách hàng với dữ liệu
            return View(khachHang);
        }
        [Route("/Sua/SuaMaKhuyenMai")]
        public IActionResult SuaMaKhuyenMai()
        {
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
    }
}
