$(document).ready(function () {

    $(document).on('click', '.btn-reset-password', function (e) {
        e.preventDefault();

        var button = $(this);
        var id = button.data('id'); // Lấy từ data-id="..."

        if (!id) {
            Swal.fire('Lỗi', 'Không tìm thấy ID tài khoản.', 'error');
            return;
        }

        Swal.fire({
            title: 'Bạn có muốn reset mật khẩu không?',
            text: "Mật khẩu sẽ được đặt lại thành '123456'.",
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
                    type: 'POST', // Dùng POST cho an toàn
                    success: function (response) {
                        // 4. THÀNH CÔNG
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

});