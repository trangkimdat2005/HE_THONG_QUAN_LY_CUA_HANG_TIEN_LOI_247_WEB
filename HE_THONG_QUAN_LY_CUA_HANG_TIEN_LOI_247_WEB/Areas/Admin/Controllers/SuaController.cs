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

        public SuaController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
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
        public IActionResult SuaNCC()
        {
            return View();
        }
        [Route("/Sua/SuaNhanSu")]
        public IActionResult SuaNhanSu()
        {
            return View();
        }
        [Route("/Sua/SuaPhanCongCaLamViec")]
        public IActionResult SuaPhanCongCaLamViec()
        {
            return View();
        }
        [Route("/Sua/SuaTaiKhoan")]
        public IActionResult SuaTaiKhoan()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult SuaKhachHang(KhachHang customer, IFormFile Avatar)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var existingCustomer = _quanLySevices.GetList<KhachHang>().FirstOrDefault(kh => kh.Id == customer.Id);
        //        if (existingCustomer == null)
        //        {
        //            return Json(new { success = false }); // Không tìm thấy khách hàng
        //        }

        //        existingCustomer.HoTen = customer.HoTen;
        //        existingCustomer.SoDienThoai = customer.SoDienThoai;
        //        existingCustomer.Email = customer.Email;
        //        existingCustomer.DiaChi = customer.DiaChi;
        //        existingCustomer.NgayDangKy = customer.NgayDangKy;
        //        existingCustomer.TrangThai = customer.TrangThai;

        //        // Nếu có ảnh mới, lưu ảnh vào bảng Anh
        //        if (Avatar != null)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                Avatar.CopyTo(memoryStream);
        //                var imageData = memoryStream.ToArray();  // Chuyển file ảnh thành mảng byte

        //                // Lưu ảnh vào bảng Anh
        //                var anh = new HinhAnh
        //                {
        //                    Id=existingCustomer.AnhId,
        //                    TenAnh = Avatar.FileName, // Lưu tên ảnh
        //                    Anh = imageData // Lưu dữ liệu ảnh
        //                };
        //                _quanLySevices.Update<HinhAnh>(anh);

        //                // Cập nhật AnhId của khách hàng
        //                existingCustomer.AnhId = anh.Id;
        //            }
        //        }
        //        else if (Avatar == null)
        //        {
        //            return Json(new { success = false, message = "Ảnh không được để trống." });
        //        }

        //        if (existingCustomer == null)
        //        {
        //            return Json(new { success = false, message = "Không tìm thấy khách hàng." });
        //        }

        //        _quanLySevices.Update<KhachHang>(existingCustomer);

        //        return Json(new { success = true });
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        //    }
        //    return Json(new { success = false, message = "Lỗi không xác định, chắc do Khôi code ẩu" });
        //}
        //[HttpPost]
        //[Route("/Sua/SuaKhachHang")] // Giữ Route này nếu bạn đã sửa ở bước trước
        //[IgnoreAntiforgeryToken] // Tạm thời thêm dòng này để loại trừ lỗi 400
        //public IActionResult SuaKhachHang(KhachHang customer, IFormFile Avatar,
        //                                        string hangThe,
        //                                        int? diemTichLuy, // <-- SỬA Ở ĐÂY: int thành int?
        //                                        DateTime? ngayCapThe)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // ... (code tìm existingCustomer) ...

        //        // *** XỬ LÝ THẺ THÀNH VIÊN ***
        //        if (ngayCapThe.HasValue)
        //        {
        //            var ttv = _quanLySevices.GetList<TheThanhVien>().FirstOrDefault(t => t.KhachHangId == customer.Id);
        //            if (ttv == null)
        //            {
        //                // Tạo mới thẻ
        //                ttv = new TheThanhVien
        //                {
        //                    Id = Guid.NewGuid().ToString(),
        //                    KhachHangId = customer.Id,
        //                    Hang = hangThe,
        //                    DiemTichLuy = diemTichLuy ?? 0, // <-- SỬA Ở ĐÂY: thêm ?? 0
        //                    NgayCap = ngayCapThe.Value
        //                };
        //                _quanLySevices.Add<TheThanhVien>(ttv);
        //            }
        //            else
        //            {
        //                // Cập nhật thẻ
        //                ttv.Hang = hangThe;
        //                ttv.DiemTichLuy = diemTichLuy ?? 0; // <-- SỬA Ở ĐÂY: thêm ?? 0
        //                ttv.NgayCap = ngayCapThe.Value;
        //                _quanLySevices.Update<TheThanhVien>(ttv);
        //            }
        //        }

        //        return Json(new { success = true });
        //    }

        //    // Lấy lỗi validation
        //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        //    return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = errors });
        //}



    }
}
