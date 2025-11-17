$(document).on('click', '#btn-luu-cap-nhat-ncc', function () {
    var data = {
        Id: $('#editNccId').val(),
        Ten: $('#editNccTen').val(),
        SoDienThoai: $('#editNccSoDienThoai').val(),
        Email: $('#editNccEmail').val(),
        DiaChi: $('#editNccDiaChi').val(),
        MaSoThue: $('#editNccMaSoThue').val()
    };

    // 2. Gửi AJAX (dùng 'PUT')
    $.ajax({
        url: '/API/NhaCungCap/Update', // Khớp với Route Controller
        type: 'PUT', // Method là PUT
        contentType: 'application/json',
        data: JSON.stringify(data),

        // 3. XỬ LÝ THÀNH CÔNG
        success: function (response) {
            Swal.fire({
                title: 'Thành công!',
                text: response.message,
                icon: 'success'
            }).then((result) => {
                // Sau khi bấm OK...
                if (result.isConfirmed) {
                    // Chuyển mày về trang danh sách
                    location.href = '/QuanLyNhaCungCap/DanhSachNhaCungCap';
                }
            });
        },

        // 4. XỬ LÝ THẤT BẠI (giống hệt code lỗi của bạn)
        error: function (jqXHR) {
            var title = 'Đã xảy ra lỗi!';
            var message = 'Đã xảy ra lỗi máy chủ hoặc không thể kết nối.'; // Mặc định
            var response = jqXHR.responseJSON;

            // ======================= SỬA CHÍNH Ở ĐÂY =======================
            // Trường hợp 1: Lỗi validation (ModelState)
            // Kiểm tra nếu status là 400 (Bad Request) VÀ response là một object
            // VÀ nó KHÔNG chứa key 'message' (nghĩa là nó là lỗi validation)
            if (jqXHR.status === 400 && response && typeof response === 'object' && !response.message) {

                // `response` BÂY GIỜ chính là object chứa lỗi 
                // ví dụ: { "SoDienThoai": ["lỗi..."], "Ten": ["lỗi..."] }

                title = 'Dữ liệu không hợp lệ!';
                message = ""; // Ta sẽ xây dựng message từ các lỗi

                // Lặp trực tiếp qua 'response' (vì nó chính là object lỗi)
                for (var key in response) {
                    if (response.hasOwnProperty(key)) {
                        // Dùng <br> để SweetAlert xuống hàng đẹp
                        message += response[key].join("<br>") + "<br>";
                    }
                }
            }
            // ===============================================================

            // Trường hợp 2: Lỗi nghiệp vụ (từ BadRequest(new { message = "..." }))
            else if (response && response.message) {
                message = response.message; // Lấy lỗi từ server
            }
            // Trường hợp 3: Lỗi 500 hoặc lỗi không xác định
            else if (jqXHR.responseText) {
                message = "Lỗi máy chủ. Vui lòng kiểm tra console (F12).";
            }

            // Hiển thị SweetAlert lỗi
            Swal.fire({
                title: title,
                html: message,  // Dùng 'html' để nó nhận thẻ <br>
                icon: 'error',
                confirmButtonText: 'Đóng'
            });
        }
    });
});