$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const status = urlParams.get('status');

    if (status === 'reset_success') {
        window.history.replaceState(null, null, window.location.pathname);
        Swal.fire({
            icon: 'success',
            title: 'Kích hoạt thành công!',
            text: 'Mật khẩu mới đã có hiệu lực. Vui lòng check email lấy pass để đăng nhập.',
            confirmButtonText: 'OK'
        });
    } else if (status === 'error') {
        window.history.replaceState(null, null, window.location.pathname);
        Swal.fire('Lỗi', 'Link xác nhận không hợp lệ hoặc đã hết hạn.', 'error');
    }

    $('#forgot_form').submit(function (e) {
        e.preventDefault();
        var email = $('#forgot_email').val();

        if (!email) {
            Swal.fire('Lỗi', 'Vui lòng nhập email', 'warning');
            return;
        }

        Swal.fire({
            title: 'Đang xử lý...',
            text: 'Đang gửi link kích hoạt...',
            allowOutsideClick: false,
            didOpen: () => { Swal.showLoading(); }
        });

        $.ajax({
            url: '/Account/SendResetRequest',
            type: 'POST',
            data: { email: email },
            success: function (response) {
                if (response.success) {
                    Swal.fire('Đã gửi!', 'Kiểm tra email để lấy mật khẩu mới và link kích hoạt.', 'success');
                    $('#forgot_email').val('');
                    $('.login-box').removeClass('flipped');
                } else {
                    Swal.fire('Lỗi', 'Có lỗi xảy ra', 'error');
                }
            },
            error: function () {
                Swal.fire('Lỗi', 'Không kết nối được server', 'error');
            }
        });
    });
});