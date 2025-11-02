namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public interface IQuanLyServices
    {
        List<T> GetList<T>() where T : class;
        
        T Get<T> (string id) where T : class;

        bool Add<T>(T entity) where T : class;

        bool Update<T>(T entity) where T : class;

        public bool Delete<T>(T entity) where T : class;
    }
}
