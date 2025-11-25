$(document).ready(function () {
    $(document).on('click', '.btn-reset-password', function (e) {
        e.preventDefault();
        var button = $(this);
        var id = button.data('id');
        if (!id) {
            Swal.fire('Lỗi', 'Không tìm thấy ID tài khoản.', 'error');
            return;
        }

        Swal.fire({
            title: 'Bạn có muốn reset mật khẩu không?',
            text: "Hệ thống sẽ tạo mật khẩu mới và gửi link kích hoạt vào email của nhân viên.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Vâng, đặt lại!',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/API/TaiKhoan/ResetPassword/' + id,
                    type: 'POST',
                    success: function (response) {
                        Swal.fire(
                            'Đã đặt lại!',
                            response.message,
                            'success'
                        );
                    },
                    error: function (jqXHR) {
                        var title = 'Đã xảy ra lỗi!';
                        var message = 'Lỗi máy chủ.';
                        var response = jqXHR.responseJSON;
                        if (response && response.message) {
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
            }
        });
    });

    $(document).on('click', '.btn-toggle-lock', function (e) {
        e.preventDefault();

        var button = $(this);
        var id = button.data('id');
        var currentStatus = button.data('status'); 

        if (!id) {
            Swal.fire('Lỗi', 'Không tìm thấy ID tài khoản.', 'error');
            return;
        }

        var isActive = (currentStatus === 'Hoạt động' || currentStatus === 'Active');
        var title = isActive ? 'Bạn có chắc muốn KHÓA tài khoản này?' : 'Bạn có chắc muốn MỞ KHÓA tài khoản này?';
        var confirmText = isActive ? 'Vâng, khóa nó!' : 'Vâng, mở khóa!';

        Swal.fire({
            title: title,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: confirmText,
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/API/TaiKhoan/ToggleLock/' + id, 
                    type: 'POST',
                    success: function (response) {
                        Swal.fire(
                            'Thành công!',
                            response.message,
                            'success'
                        ).then(() => {
                            location.reload(); 
                        });
                    },
                    error: function (jqXHR) {
                        var title = 'Đã xảy ra lỗi!';
                        var message = 'Lỗi máy chủ.';
                        var response = jqXHR.responseJSON;
                        if (response && response.message) {
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
            }
        });
    });

});