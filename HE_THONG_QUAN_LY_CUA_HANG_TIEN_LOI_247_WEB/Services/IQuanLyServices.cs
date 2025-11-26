using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface IQuanLyServices
    {
        List<T> GetList<T>() where T : class;

        T GetById<T>(params object[] keyValues) where T : class;

        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void HardDelete<T>(T entity) where T : class;

        void SoftDelete<T>(T entity) where T : class;

        string GenerateNewId<T>(string prefix, int totalLength) where T : class;

        Task<byte[]> ConvertImageToByteArray(IFormFile file);

        string ConvertToBase64Image(byte[] bytes, string fileName);

        string HashPassword(string password);

        string GeneratePassword(string password);

        TaiKhoan Login(string username, string password);

        string GetContentType(string fileName);

        bool VerifyPassword(string password, string hashedPassword);

        bool ChangePassword(string taiKhoanId, string oldPassword, string newPassword);

        Task BeginTransactionAsync();

        Task<bool> CommitAsync(string? notifyKey = null);
        Task RollbackAsync();

        //bool ChangePassword(string taiKhoanId, string oldPassword, string newPassword);

        TaiKhoan GetByEmail(string email);

        bool ResetPassword(string email, string newPasswordPlain);

        string GenerateRandomPassword();

        PhanCongCaLamViec GetPhanCong(string nhanVienId, DateTime ngay);

        PhanCongCaLamViec GetPhanCongByDate(string nhanVienId, DateTime ngay);
    }
}
