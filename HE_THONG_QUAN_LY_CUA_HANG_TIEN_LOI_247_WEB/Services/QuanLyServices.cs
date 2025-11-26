using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class QuanLyServices : IQuanLyServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IRealtimeNotifier _notifier;
        private IDbContextTransaction? _currentTransaction;
        private readonly IMemoryCache _cache;

        public QuanLyServices(ApplicationDbContext context, IRealtimeNotifier notifier, IMemoryCache cache)
        {
            _context = context;
            _notifier = notifier;
            _cache = cache;
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
                string hashedPassword = /*password;*/ HashPassword(password);
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
                if (string.IsNullOrEmpty(HashPassword(password)) || string.IsNullOrEmpty(hashedPassword))
                {
                    return false;
                }
                password = HashPassword(password);

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

                var taiKhoan = GetById<TaiKhoan>(taiKhoanId);

                if (taiKhoan == null)
                {
                    throw new Exception("Không tìm thấy tài khoản.");
                }

                if (!VerifyPassword(oldPassword, taiKhoan.MatKhauHash))
                {
                    throw new Exception("Mật khẩu hiện tại không đúng.");
                }

                taiKhoan.MatKhauHash = HashPassword(newPassword);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while changing password: {ex.Message}");
                throw;
            }
        }
        public TaiKhoan GetByEmail(string email)
        {
            try
            {
                // Tìm tài khoản theo email, chưa bị xóa
                return _context.TaiKhoans.FirstOrDefault(t => t.Email == email && t.IsDelete == false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user by email: {ex.Message}");
                return null;
            }
        }

        public bool ResetPassword(string email, string newPasswordPlain)
        {
            try
            {
                var user = GetByEmail(email);
                if (user == null) return false;

                // Băm mật khẩu mới bằng hàm có sẵn của bạn
                string newHash = HashPassword(newPasswordPlain);

                user.MatKhauHash = newHash;

                // Cập nhật vào DB
                _context.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting password: {ex.Message}");
                return false;
            }
        }
        public string GenerateRandomPassword()
        {
            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",
            "abcdefghijkmnopqrstuvwxyz",
            "0123456789",
            "!@$?"
        };
            Random rand = new Random();
            List<char> chars = new List<char>();

            chars.Insert(rand.Next(0, chars.Count), randomChars[0][rand.Next(0, randomChars[0].Length)]);
            chars.Insert(rand.Next(0, chars.Count), randomChars[1][rand.Next(0, randomChars[1].Length)]);
            chars.Insert(rand.Next(0, chars.Count), randomChars[2][rand.Next(0, randomChars[2].Length)]);
            chars.Insert(rand.Next(0, chars.Count), randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < 10; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count), rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
        public PhanCongCaLamViec GetPhanCong(string nhanVienId, DateTime ngay)
        {
            try
            {
                return _context.PhanCongCaLamViecs
                               .FirstOrDefault(p => p.NhanVienId == nhanVienId
                                                 && p.Ngay.Date == ngay.Date
                                                 && !p.IsDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding PhanCong: {ex.Message}");
                return null;
            }
        }
        public PhanCongCaLamViec GetPhanCongByDate(string nhanVienId, DateTime ngay)
        {
            return _context.PhanCongCaLamViecs
                           .FirstOrDefault(p => p.NhanVienId == nhanVienId
                                             && p.Ngay.Date == ngay.Date
                                             && !p.IsDelete);
        }


        #region Transaction Management

        private Dictionary<string, int> _generatedIdCounters = new Dictionary<string, int>();

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("Transaction đã được bắt đầu!");

            _currentTransaction = await _context.Database.BeginTransactionAsync();
            _generatedIdCounters.Clear();
        }

        public string GenerateNewId<T>(string prefix, int totalLength) where T : class
        {
            try
            {
                // ✅ Nếu chưa có counter cho prefix này
                if (!_generatedIdCounters.ContainsKey(prefix))
                {
                    // Query DB để lấy ID lớn nhất
                    var lastEntity = _context.Set<T>()
                        .AsNoTracking()
                        .Where(e => EF.Property<string>(e, "Id").StartsWith(prefix)) // ← EF.Property CHỈ dùng TRONG query
                        .OrderByDescending(e => EF.Property<string>(e, "Id"))
                        .Select(e => EF.Property<string>(e, "Id")) // ← SELECT chỉ Id
                        .FirstOrDefault();

                    int lastNumber = 0;
                    if (lastEntity != null) // ← lastEntity GIỜ LÀ string, không phải object
                    {
                        var numericPart = lastEntity.Substring(prefix.Length);
                        if (int.TryParse(numericPart, out int lastNumericPart))
                        {
                            lastNumber = lastNumericPart;
                        }
                    }

                    _generatedIdCounters[prefix] = lastNumber;
                }

                // ✅ Tăng counter
                _generatedIdCounters[prefix]++;
                int newNumericPart = _generatedIdCounters[prefix];

                string newId = prefix + newNumericPart.ToString().PadLeft(totalLength - prefix.Length, '0');
                return newId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating ID for {typeof(T).Name}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CommitAsync(string? notifyKey = null)
        {
            try
            {
                if (_currentTransaction == null)
                    throw new InvalidOperationException("Chưa có transaction!");

                await _context.SaveChangesAsync();
                await _currentTransaction.CommitAsync();

                if (!string.IsNullOrEmpty(notifyKey))
                {
                    await _notifier.NotifyReloadAsync(notifyKey);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commit failed: {ex.Message}");
                await RollbackAsync();
                return false;
            }
            finally
            {
                await DisposeTransactionAsync();
                _generatedIdCounters.Clear(); // ← Clear counter
            }
        }

        public async Task RollbackAsync()
        {
            var transaction = _currentTransaction;

            if (transaction != null)
            {
                _currentTransaction = null;
                _generatedIdCounters.Clear(); // ← Clear counter

                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Rollback warning: {ex.Message}");
                }
                finally
                {
                    try
                    {
                        await transaction.DisposeAsync();
                    }
                    catch { }
                }
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                try
                {
                    await _currentTransaction.DisposeAsync();
                }
                catch { }
                finally
                {
                    _currentTransaction = null;
                }
            }
        }

        #endregion

        #region CRUD Operations (Không SaveChanges)

        // ✅ Add - KHÔNG SaveChanges
        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        // ✅ Update - KHÔNG SaveChanges
        public void Update<T>(T entity) where T : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        // ✅ SoftDelete - KHÔNG SaveChanges
        public void SoftDelete<T>(T entity) where T : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }

            var property = typeof(T).GetProperty("IsDelete");
            if (property != null && property.CanWrite)
            {
                property.SetValue(entity, true);
                entry.State = EntityState.Modified;
            }
            else
            {
                throw new InvalidOperationException($"Entity {typeof(T).Name} không có property 'IsDelete'");
            }
        }

        // ✅ HardDelete - KHÔNG SaveChanges
        public void HardDelete<T>(T entity) where T : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            _context.Set<T>().Remove(entity);
        }

        #endregion

        public string GenerateRecoveryToken(string email)
        {
            var user = GetByEmail(email);
            if (user == null) return null;

            // Tạo Token và Key
            string token = Guid.NewGuid().ToString();
            string cacheKey = $"ResetToken_{email}";

            // Cấu hình tự hủy sau 5 phút
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            // Lưu vào RAM
            _cache.Set(cacheKey, token, cacheOptions);

            return token;
        }

        public bool ExecuteResetPassword(string email, string token, string newPasswordPlain)
        {
            string cacheKey = $"ResetToken_{email}";

            // 1. Check Token trong RAM
            if (!_cache.TryGetValue(cacheKey, out string storedToken)) return false; // Hết hạn hoặc ko tồn tại
            if (storedToken != token) return false; // Token sai

            // 2. Đổi mật khẩu trong DB
            var user = GetByEmail(email);
            if (user == null) return false;

            user.MatKhauHash = HashPassword(newPasswordPlain); // Nhớ dùng hàm Hash cũ của bạn
            _context.Update(user);
            _context.SaveChanges();

            // 3. Xóa Token ngay lập tức (Chống dùng lại)
            _cache.Remove(cacheKey);

            return true;
        }
    }
}
