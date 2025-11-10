// using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.EF;
// using Microsoft.EntityFrameworkCore;

// namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
// {
//     public class QuanLyServices : IQuanLyServices
//     {
//         private readonly ApplicationDbContext _context;
//         public List<T> GetList<T>() where T : class
//         {
//             try
//             {
//                 return _context.Set<T>().AsNoTracking().ToList();
//             }
//             catch (Exception ex)
//             {
//                 return new List<T>();
//             }
//         }
//         public T Get<T>(string id) where T : class
//         {
//             try
//             {
//                 var result = _context.Set<T>().Find(id);

//                 if (result == null)
//                 {
//                     // Có thể log nếu không tìm thấy đối tượng
//                     // Log.Warning($"Không tìm thấy đối tượng {typeof(T).Name} với Id: {id}");

//                     // Ném một exception có thông tin chi tiết
//                     throw new Exception($"Không tìm thấy đối tượng {typeof(T).Name} với Id: {id}");
//                 }

//                 return result;
//             }
//             catch (Exception ex)
//             {
//                 return null;
//             }
//         }
//         public bool Add<T>(T entity) where T : class
//         {
//             try
//             {
//                 _context.Set<T>().Add(entity);
//                 _context.SaveChanges();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 return false;
//             }
//         }
//         public bool Update<T>(T entity) where T : class
//         {
//             try
//             {
//                 _context.Set<T>().Update(entity);
//                 _context.SaveChanges();
//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 return false;
//             }
//         }
//         public bool Delete<T>(T entity) where T : class
//         {
//             try
//             {

//                 var isDeleteProperty = typeof(T).GetProperty("IsDelete");

//                 isDeleteProperty.SetValue(entity, true);
//                 // Lưu thay đổi vào cơ sở dữ liệu
//                 _context.SaveChanges();
//                 return true; // Trả về true nếu xóa mềm thành công
//             }
//             catch (Exception ex)
//             {
//                 return false;
//             }
//         }
//     }
// }
