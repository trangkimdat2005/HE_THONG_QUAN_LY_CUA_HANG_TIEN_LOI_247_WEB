namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels
{
    public class SanPhamDTO
    {
        public string Id { get; set; } // Mã sản phẩm
        public string Ten { get; set; } // Tên sản phẩm
        public string NhanHieu { get; set; } // Nhãn hiệu
        public List<string> DanhMucs { get; set; } // Danh mục (có thể chọn nhiều)
    }
}
