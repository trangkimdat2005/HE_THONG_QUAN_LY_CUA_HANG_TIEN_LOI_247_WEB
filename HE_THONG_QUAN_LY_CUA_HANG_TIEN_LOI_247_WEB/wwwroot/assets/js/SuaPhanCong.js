$(document).ready(function () {

    $('#select-nhan-vien').select2({
        width: '100%'
    });
    $('#select-ca-lam-viec').select2({
        width: '100%'
    });

    $(document).on('click', '#btn-luu-cap-nhat-phan-cong', function (e) {
        e.preventDefault();

        var data = {
            Id: $('#Id').val(),
            NhanVienId: $('#select-nhan-vien').val(),
            CaLamViecId: $('#select-ca-lam-viec').val(),
            Ngay: $('#input-ngay').val()
        };

        $.ajax({
            url: '/API/PhanCong/Update',
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
                        location.href = '/QuanLyNhanSu/PhanCongCaLamViec';
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