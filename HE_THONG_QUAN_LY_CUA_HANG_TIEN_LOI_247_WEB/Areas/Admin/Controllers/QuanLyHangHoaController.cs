using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Areas.Admin.Controllers
{
    [ApiController]
    [Route("API")]
    [Area("Admin")]
    public class QuanLyHangHoaController : Controller
    {
        private readonly IQuanLyServices _quanLySevices;

        public QuanLyHangHoaController(IQuanLyServices quanLySevices)
        {
            _quanLySevices = quanLySevices;
        }

        [Route("/QuanLyHangHoa/DanhSachDanhMuc")]
        public IActionResult DanhSachDanhMuc()
        {
            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>();

            ViewData["lstDanhMuc"] = lstDanhMuc;
            return View();
        }
        [Route("/QuanLyHangHoa/DanhSachDonVi")]
        public IActionResult DanhSachDonVi()
        {
            var lstDonViDoLuong = _quanLySevices.GetList<DonViDoLuong>();
            
            ViewData["lstDonViDoLuong"] = lstDonViDoLuong;
            
            return View();
        }
        
        [Route("/QuanLyHangHoa/DanhSachNhanHieu")]
        public IActionResult DanhSachNhanHieu()
        {
            var lstNhanHieu = _quanLySevices.GetList<NhanHieu>();
            
            ViewData["lstNhanHieu"] = lstNhanHieu;
            
            return View();
        }
        
        [Route("/QuanLyHangHoa/DanhSachSanPham")]
        public IActionResult DanhSachSanPham()
        {
            var lstDanhMuc = _quanLySevices.GetList<DanhMuc>();

            ViewData["lstDanhMuc"] = lstDanhMuc;

            var lstSanPham = _quanLySevices.GetList<SanPham>();

            ViewData["lstSanPham"] = lstSanPham;

            var lstNhanHieu = _quanLySevices.GetList<NhanHieu>();

            ViewData["lstNhanHieu"] = lstNhanHieu;

            var lstDonVi = _quanLySevices.GetList<DonViDoLuong>();

            ViewData["lstDonVi"] = lstDonVi;

            return View();
        }
        [Route("/QuanLyHangHoa/LichSuGiaBan")]
        public IActionResult LichSuGiaBan()
        {
            var lstLichSuGiaBan = _quanLySevices.GetList<LichSuGiaBan>();
            
            ViewData["lstLichSuGiaBan"] = lstLichSuGiaBan;
            
            return View();
        }

        [HttpPost("get-next-id-SP")]
        public Task<IActionResult> GetNextId([FromBody] GetNextIdSPRequest request)
        {
            return Task.FromResult<IActionResult>(Ok(new { NextId = _quanLySevices.GenerateNewId<SanPham>(request.prefix, request.totalLength)}));
        }

        [HttpPost("add-SP")]
        public async Task<IActionResult> AddSanPham([FromForm] ProductFormData request)
        {
            if (request == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            SanPham sanPham = new SanPham
            {
                Id = request.ProductId,
                Ten = request.ProductName,
                NhanHieuId = request.Brand,
                MoTa = request.Description,
            };

            if (!_quanLySevices.Add<SanPham>(sanPham))
            {
                return BadRequest("không thể thêm sản phẩm");
            }

            SanPhamDonVi sanPhamDonVi = new SanPhamDonVi
            {
                SanPhamId = sanPham.Id,
                DonViId = request.Unit,
                Id = _quanLySevices.GenerateNewId<SanPhamDonVi>("SPDV",8),
                HeSoQuyDoi = request.ConversionFactor,
                GiaBan = request.Price,
                TrangThai = request.Status
            };

            if (!_quanLySevices.Add<SanPhamDonVi>(sanPhamDonVi))
            {
                return BadRequest("không thể thêm sản phẩm đơn vị");
            }

            List<HinhAnh> hinhAnhs = new List<HinhAnh>();

            foreach(IFormFile image in request.ImagesUpload)
            {
                HinhAnh hinhAnh = new HinhAnh
                {
                    Id = _quanLySevices.GenerateNewId<HinhAnh>("HA",6),
                    TenAnh = image.FileName,
                    Anh = await _quanLySevices.ConvertImageToByteArray(image)
                };

                hinhAnhs.Add(hinhAnh);
            }

            foreach(HinhAnh hinhAnh in hinhAnhs) {
                if (!_quanLySevices.Add<HinhAnh>(hinhAnh))
                {
                    return BadRequest("không thể thêm ảnh");
                }
                AnhSanPhamDonVi anhSanPhamDonVi = new AnhSanPhamDonVi
                {
                    SanPhamDonViId = sanPhamDonVi.Id,
                    AnhId = hinhAnh.Id,
                };
                if (!_quanLySevices.Add<AnhSanPhamDonVi>(anhSanPhamDonVi))
                {
                    return BadRequest("không thể thêm ảnh sản phẩm đơn vị");
                }
            }

            


            return Ok(new { message = "Thêm sản phẩm thành công!" });
        }

    }
    public class GetNextIdSPRequest
    {
        public string prefix { get; set; }
        public int totalLength { get; set; }
    }

    public class ProductFormData
    {
        public List<IFormFile> ImagesUpload { get; set; }  // Dữ liệu ảnh (danh sách ảnh)
        public string ProductId { get; set; }  // Mã sản phẩm (id)
        public string ProductName { get; set; }  // Tên sản phẩm
        public string Brand { get; set; }  // Nhãn hiệu
        public List<string> Categories { get; set; }  // Danh mục
        public string Unit { get; set; }  // Đơn vị cơ sở
        public decimal ConversionFactor { get; set; }  // Hệ số quy đổi
        public decimal Price { get; set; }  // Giá bán
        public string Description { get; set; }  // Mô tả sản phẩm
        public string Status { get; set; }  // Trạng thái (Còn hàng, Hết hàng, Ngừng kinh doanh)
    }


}
