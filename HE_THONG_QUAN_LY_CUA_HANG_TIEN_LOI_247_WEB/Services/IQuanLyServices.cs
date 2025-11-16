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

        IFormFile ConvertByteArrayToFile(byte[] fileBytes, string fileName);
    }
}
