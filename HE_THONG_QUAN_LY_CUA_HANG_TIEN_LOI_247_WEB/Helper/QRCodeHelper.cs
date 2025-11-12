using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Helpers
{
    public static class QRCodeHelper
    {
        /// <summary>
        /// Tạo QR code trả về byte array (PNG)
        /// </summary>
        /// <param name="text">Nội dung cần mã hóa</param>
        /// <param name="pixelsPerModule">Kích thước mỗi module (mặc định: 20)</param>
        /// <returns>Mảng byte của ảnh PNG</returns>
        public static byte[] GenerateQRCode(string text, int pixelsPerModule = 20)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text không được rỗng", nameof(text));

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
            using (QRCode qrCode = new QRCode(qrCodeData))
            using (Bitmap qrCodeImage = qrCode.GetGraphic(pixelsPerModule))
            {
                return BitmapToByteArray(qrCodeImage);
            }
        }

        /// <summary>
        /// Tạo QR code với màu tùy chỉnh
        /// </summary>
        /// <param name="text">Nội dung cần mã hóa</param>
        /// <param name="foreColor">Màu nền trước (mặc định: đen)</param>
        /// <param name="backColor">Màu nền sau (mặc định: trắng)</param>
        /// <param name="pixelsPerModule">Kích thước mỗi module</param>
        /// <returns>Mảng byte của ảnh PNG</returns>
        public static byte[] GenerateQRCodeWithColor(
            string text,
            Color? foreColor = null,
            Color? backColor = null,
            int pixelsPerModule = 20)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text không được rỗng", nameof(text));

            Color fore = foreColor ?? Color.Black;
            Color back = backColor ?? Color.White;

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
            using (QRCode qrCode = new QRCode(qrCodeData))
            using (Bitmap qrCodeImage = qrCode.GetGraphic(pixelsPerModule, fore, back, true))
            {
                return BitmapToByteArray(qrCodeImage);
            }
        }

        /// <summary>
        /// Tạo QR code trả về Base64 string (để hiển thị trên web)
        /// </summary>
        /// <param name="text">Nội dung cần mã hóa</param>
        /// <param name="pixelsPerModule">Kích thước mỗi module</param>
        /// <returns>Chuỗi Base64</returns>
        public static string GenerateQRCodeBase64(string text, int pixelsPerModule = 20)
        {
            byte[] qrBytes = GenerateQRCode(text, pixelsPerModule);
            return Convert.ToBase64String(qrBytes);
        }

        /// <summary>
        /// Tạo QR code trả về data URL (sẵn sàng dùng trong thẻ img)
        /// </summary>
        /// <param name="text">Nội dung cần mã hóa</param>
        /// <param name="pixelsPerModule">Kích thước mỗi module</param>
        /// <returns>Data URL</returns>
        public static string GenerateQRCodeDataUrl(string text, int pixelsPerModule = 20)
        {
            string base64 = GenerateQRCodeBase64(text, pixelsPerModule);
            return $"data:image/png;base64,{base64}";
        }

        /// <summary>
        /// Lưu QR code ra file
        /// </summary>
        /// <param name="text">Nội dung cần mã hóa</param>
        /// <param name="filePath">Đường dẫn file đầu ra</param>
        /// <param name="pixelsPerModule">Kích thước mỗi module</param>
        public static void SaveQRCodeToFile(string text, string filePath, int pixelsPerModule = 20)
        {
            byte[] qrBytes = GenerateQRCode(text, pixelsPerModule);
            File.WriteAllBytes(filePath, qrBytes);
        }

        private static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}