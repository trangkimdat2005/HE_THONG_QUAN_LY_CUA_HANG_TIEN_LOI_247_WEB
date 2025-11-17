namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.ViewModels
{
    public class SanPhamDonViDTO
    {
        public string SanPhamId { get; set; }
        public string DonViId { get; set; }
        public decimal GiaBan { get; set; }
        public decimal HeSoQuyDoi { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }
        public List<IFormFile> ImagesUpload { get; set; }
    }
}
