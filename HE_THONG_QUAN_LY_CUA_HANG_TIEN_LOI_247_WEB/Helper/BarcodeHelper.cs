using SkiaSharp;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Helpers
{
    public static class BarcodeHelper
    {
        /// <summary>
        /// Tạo barcode cơ bản
        /// </summary>
        /// <param name="data">Dữ liệu cần mã hóa</param>
        /// <param name="type">Loại barcode (mặc định: CODE128)</param>
        /// <param name="width">Chiều rộng (mặc định: 300)</param>
        /// <param name="height">Chiều cao (mặc định: 150)</param>
        /// <returns>Mảng byte của ảnh PNG</returns>
        public static byte[] GenerateBarcode(
            string data,
            BarcodeStandard.Type type = BarcodeStandard.Type.Code128,
            int width = 300,
            int height = 150)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException("Data không được rỗng", nameof(data));

            var barcode = new BarcodeStandard.Barcode
            {
                IncludeLabel = true,
                AlternateLabel = data,
                Width = width,
                Height = height,
                BackgroundColor = SKColors.White,
                ForegroundColor = SKColors.Black
            };

            SKImage img = barcode.Encode(type, data, width, height);
            return ImageToByteArray(img);
        }

        /// <summary>
        /// Tạo barcode EAN-13 (13 chữ số, chuẩn châu Âu)
        /// </summary>
        /// <param name="data">12 hoặc 13 chữ số</param>
        /// <param name="width">Chiều rộng</param>
        /// <param name="height">Chiều cao</param>
        /// <returns>Mảng byte của ảnh PNG</returns>
        public static byte[] GenerateEAN13(string data, int width = 300, int height = 150)
        {
            // Xử lý EAN13: cần chính xác 12 hoặc 13 chữ số
            if (data.Length == 12)
            {
                data = data + CalculateEAN13CheckDigit(data);
            }
            else if (data.Length != 13)
            {
                throw new ArgumentException("EAN13 cần 12 hoặc 13 chữ số", nameof(data));
            }

            return GenerateBarcode(data, BarcodeStandard.Type.Ean13, width, height);
        }

        /// <summary>
        /// Tạo barcode CODE128 (hỗ trợ chữ và số)
        /// </summary>
        public static byte[] GenerateCODE128(string data, int width = 300, int height = 150)
        {
            return GenerateBarcode(data, BarcodeStandard.Type.Code128, width, height);
        }

        /// <summary>
        /// Tạo barcode CODE39 (chữ, số, ký tự đặc biệt)
        /// </summary>
        public static byte[] GenerateCODE39(string data, int width = 300, int height = 150)
        {
            return GenerateBarcode(data, BarcodeStandard.Type.Code39, width, height);
        }

        /// <summary>
        /// Tạo barcode UPC-A (12 chữ số, chuẩn Mỹ)
        /// </summary>
        public static byte[] GenerateUPCA(string data, int width = 300, int height = 150)
        {
            if (data.Length != 11 && data.Length != 12)
            {
                throw new ArgumentException("UPC-A cần 11 hoặc 12 chữ số", nameof(data));
            }

            return GenerateBarcode(data, BarcodeStandard.Type.UpcA, width, height);
        }

        /// <summary>
        /// Tạo barcode trả về Base64 string
        /// </summary>
        public static string GenerateBarcodeBase64(
            string data,
            BarcodeStandard.Type type = BarcodeStandard.Type.Code128,
            int width = 300,
            int height = 150)
        {
            byte[] barcodeBytes = GenerateBarcode(data, type, width, height);
            return Convert.ToBase64String(barcodeBytes);
        }

        /// <summary>
        /// Tạo barcode trả về data URL
        /// </summary>
        public static string GenerateBarcodeDataUrl(
            string data,
            BarcodeStandard.Type type = BarcodeStandard.Type.Code128,
            int width = 300,
            int height = 150)
        {
            string base64 = GenerateBarcodeBase64(data, type, width, height);
            return $"data:image/png;base64,{base64}";
        }

        /// <summary>
        /// Lưu barcode ra file
        /// </summary>
        public static void SaveBarcodeToFile(
            string data,
            string filePath,
            BarcodeStandard.Type type = BarcodeStandard.Type.Code128,
            int width = 300,
            int height = 150)
        {
            byte[] barcodeBytes = GenerateBarcode(data, type, width, height);
            File.WriteAllBytes(filePath, barcodeBytes);
        }

        /// <summary>
        /// Tính chữ số kiểm tra cho EAN13
        /// </summary>
        private static string CalculateEAN13CheckDigit(string code)
        {
            if (code.Length != 12)
                throw new ArgumentException("Cần 12 chữ số để tính check digit", nameof(code));

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(code[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit.ToString();
        }

        private static byte[] ImageToByteArray(SKImage image)
        {
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var ms = new MemoryStream())
            {
                data.SaveTo(ms);
                return ms.ToArray();
            }
        }
    }
}