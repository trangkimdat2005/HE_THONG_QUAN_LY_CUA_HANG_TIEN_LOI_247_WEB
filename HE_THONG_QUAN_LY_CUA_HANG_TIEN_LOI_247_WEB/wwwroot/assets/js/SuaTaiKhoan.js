$(document).ready(function () {

    // Khởi tạo Select2
    $('#select-roles').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều vai trò'
    });
    $('#select-trang-thai').select2({ width: '100%', minimumResultsForSearch: Infinity });

    // --- Bắt sự kiện nút LƯU CẬP NHẬT ---
    $('#btn-luu-cap-nhat-tai-khoan').on('click', function (e) {
        e.preventDefault();

        // 1. Thu thập dữ liệu
        var data = {
            TaiKhoanId: $('#TaiKhoanId').val(), // Lấy từ input ẩn
            TrangThai: $('#select-trang-thai').val(),
            RoleIds: $('#select-roles').val() || [] // Lấy mảng ID từ Select2
        };

        // 2. Gửi AJAX
        $.ajax({
            url: '/API/TaiKhoan/Update', // Gọi Action Update
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(data),

            success: function (response) {
                Swal.fire({
                    title: 'Thành công!',
                    text: response.message,
                    icon: 'success',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Quay về trang danh sách NV
                        location.href = '/QuanLyBaoMat/DanhSachTaiKhoanNhanVien';
                    }
                });
            },
            error: function (jqXHR) {
                var title = 'Đã xảy ra lỗi!';
                var message = 'Lỗi máy chủ.';
                var response = jqXHR.responseJSON;

                if (jqXHR.status === 400 && response && typeof response === 'object' && !response.message) {
                    title = 'Dữ liệu không hợp lệ!';
                    message = "";
                    for (var key in response) {
                        if (response.hasOwnProperty(key)) {
                            message += response[key].join("<br>") + "<br>";
                        }
                    }
                } else if (response && response.message) {
                    message = response.message;
                }

                Swal.fire({
                    title: title,
                    html: message,
                    icon: 'error',
                    confirmButtonText: 'Đóng'
                });
            }
        });
    });
});