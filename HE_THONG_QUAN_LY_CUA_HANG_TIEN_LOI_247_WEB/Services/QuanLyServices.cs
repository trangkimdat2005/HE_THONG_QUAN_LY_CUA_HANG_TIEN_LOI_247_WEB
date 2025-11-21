using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class QuanLyServices : IQuanLyServices
    {
        private readonly ApplicationDbContext _context;

        public QuanLyServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<T> GetList<T>() where T : class
        {
            try
            {
                var data = _context.Set<T>().AsNoTracking().ToList(); // Load tất cả vào memory trước

                var property = typeof(T).GetProperty("IsDelete");
                if (property != null)
                {
                    return data.Where(t =>
                    {
                        var value = property.GetValue(t);
                        return value is bool IsDelete && !IsDelete;
                    }).ToList();
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving entities of type {typeof(T).Name}: {ex.Message}");
                return new List<T>();
            }
        }
        public T GetById<T>(params object[] keyValues) where T : class
        {
            try
            {
                var entity = _context.Set<T>().Find(keyValues);
                if (entity == null)
                {
                    throw new Exception($"Không tìm thấy đối tượng {typeof(T).Name} với Id: {keyValues}");
                }
                var property = typeof(T).GetProperty("IsDelete");
                if (property != null)
                {
                    var value = property.GetValue(entity);
                    if (value is bool isDelete && isDelete)
                    {
                        throw new Exception($"Đối tượng {typeof(T).Name} với Id: {keyValues} đã bị xóa mềm (IsDelete = true)");
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving entity of type {typeof(T).Name} with ID {keyValues}: {ex.Message}");
                return null;
            }
        }
        public bool Add<T>(T entity) where T : class
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding entity of type {typeof(T).Name}: {ex.Message}");
                return false;
            }
        }
        public bool Update<T>(T entity) where T : class
        {
            try
            {
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                }

                // Đánh dấu thực thể là đã thay đổi
                entry.State = EntityState.Modified;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating entity of type {typeof(T).Name}: {ex.Message}");
                return false;
            }
        }
        public bool SoftDelete<T>(T entity) where T : class
        {
            try
            {
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                }

                var property = typeof(T).GetProperty("IsDelete"); // hoặc "isDelete"
                if (property != null && property.CanWrite)
                {
                    property.SetValue(entity, true); // Set vào ENTITY
                    entry.State = EntityState.Modified; // Đánh dấu là đã thay đổi
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    Console.WriteLine("No 'IsDelete' property found in entity.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while soft deleting entity of type {typeof(T).Name}: {ex.Message}");
                return false;
            }
        }

        public bool HardDelete<T>(T entity) where T : class
        {
            try
            {
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                }

                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while deleting entity of type {typeof(T).Name}: {ex.Message}");
                return false;
            }
        }

        public string GenerateNewId<T>(string prefix, int totalLength) where T : class
        {
            try
            {
                var lastEntity = _context.Set<T>()
                    .AsNoTracking()
                    .Where(e => EF.Property<string>(e, "Id").StartsWith(prefix))
                    .OrderByDescending(e => EF.Property<string>(e, "Id"))
                    .FirstOrDefault();
                int newNumericPart = 1;
                if (lastEntity != null)
                {
                    var lastId = lastEntity.GetType().GetProperty("Id")?.GetValue(lastEntity) as string;
                    if (lastId != null)
                    {
                        var numericPart = lastId.Substring(prefix.Length);
                        if (int.TryParse(numericPart, out int lastNumericPart))
                        {
                            newNumericPart = lastNumericPart + 1;
                        }
                    }

                }
                string newId = prefix + newNumericPart.ToString().PadLeft(totalLength - prefix.Length, '0');
                return newId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating new ID for type {typeof(T).Name}: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]> ConvertImageToByteArray(IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    // Đọc nội dung của file vào memoryStream
                    await file.CopyToAsync(memoryStream);

                    // Chuyển đổi stream thành mảng byte
                    return memoryStream.ToArray();
                }
            }
            return null; // Trả về null nếu file không hợp lệ
        }


        public string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(fileName, out string contentType))
            {
                return contentType;
            }
            return "application/octet-stream"; // Nếu không xác định được, trả về loại mặc định
        }

        // Hàm chuyển mảng byte thành IFormFile
        public string ConvertToBase64Image(byte[] bytes, string fileName)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            string contentType = GetContentType(fileName);
            string base64 = Convert.ToBase64String(bytes);

            return $"data:{contentType};base64,{base64}";
        }

        public string HashPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("Password cannot be null or empty.");
                }
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                    byte[] hash = sha256.ComputeHash(bytes);
                    return Convert.ToBase64String(hash);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while hashing password: {ex.Message}");
                return null;
            }

        }

        public TaiKhoan Login(string username, string password)
        {
            try
            {
                string hashedPassword = password; /*HashPassword(password);*/
                var user = _context.TaiKhoans.SingleOrDefault(t => t.TenDangNhap == username.ToLower().Trim() && t.MatKhauHash == hashedPassword && t.IsDelete == false);
                if (user != null && user.TrangThai.Trim() == "Hoạt động")
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                return null;
            }

        }

        public string GeneratePassword(string password)
        {
            throw new NotImplementedException();
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                {
                    return false;
                }

                return password == hashedPassword;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while verifying password: {ex.Message}");
                return false;
            }
        }

        public bool ChangePassword(string taiKhoanId, string oldPassword, string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(taiKhoanId) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
                {
                    throw new ArgumentException("Các thông tin không được để trống.");
                }

                if (newPassword.Length < 6)
                {
                    throw new ArgumentException("Mật khẩu mới phải có ít nhất 6 ký tự.");
                }

                var taiKhoan = _context.TaiKhoans.FirstOrDefault(t => t.Id == taiKhoanId && t.IsDelete == false);

                if (taiKhoan == null)
                {
                    throw new Exception("Không tìm thấy tài khoản.");
                }

                if (!VerifyPassword(oldPassword, taiKhoan.MatKhauHash))
                {
                    throw new Exception("Mật khẩu hiện tại không đúng.");
                }

                taiKhoan.MatKhauHash = newPassword;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while changing password: {ex.Message}");
                throw;
            }
        }
    }

}
