using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface IQuanLyServices
    {
        List<T> GetList<T>() where T : class;

        T GetById<T>(params object[] keyValues) where T : class;

        bool Add<T>(T entity) where T : class;

        bool Update<T>(T entity) where T : class;

        bool HardDelete<T>(T entity) where T : class;

        bool SoftDelete<T>(T entity) where T : class;

        string GenerateNewId<T>(string prefix, int totalLength) where T : class;

        Task<byte[]> ConvertImageToByteArray(IFormFile file);

        public string ConvertToBase64Image(byte[] bytes, string fileName);

        public string HashPassword(string password);

        public string GeneratePassword(string password);

        public TaiKhoan Login(string username, string password);

        public string GetContentType(string fileName);

        public bool VerifyPassword(string password, string hashedPassword);

        public bool ChangePassword(string taiKhoanId, string oldPassword, string newPassword);
        public TaiKhoan GetByEmail(string email);
        public bool ResetPassword(string email, string newPasswordPlain);
        public string GenerateRandomPassword();
        public PhanCongCaLamViec GetPhanCong(string nhanVienId, DateTime ngay);
        public PhanCongCaLamViec GetPhanCongByDate(string nhanVienId, DateTime ngay);
    }
}
