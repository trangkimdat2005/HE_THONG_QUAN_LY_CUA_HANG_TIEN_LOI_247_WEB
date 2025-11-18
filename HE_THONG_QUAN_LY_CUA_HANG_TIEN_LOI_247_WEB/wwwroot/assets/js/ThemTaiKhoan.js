$(document).ready(function () {

    $('#select-nhan-vien').select2({ width: '100%' });
    $('#select-khach-hang').select2({ width: '100%' });

    $('#select-roles').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều vai trò'
    });
    $('#select-trang-thai').select2({ width: '100%', minimumResultsForSearch: Infinity });

    function toggleAccountFields() {
        var selectedType = $('input[name="accountType"]:checked').val();

        if (selectedType === 'NhanVien') {
            $('#field-chon-nhan-vien').show();
            $('#field-chon-khach-hang').hide();
            $('#field-chon-vai-tro').show();

            $('#select-khach-hang').val(null).trigger('change');
        } else {
            $('#field-chon-nhan-vien').hide();
            $('#field-chon-khach-hang').show();
            $('#field-chon-vai-tro').hide();

            $('#select-nhan-vien').val(null).trigger('change');
            $('#select-roles').val(null).trigger('change');
        }
        $('#input-email').val('');
    }

    function updateEmail() {
        var selectedType = $('input[name="accountType"]:checked').val();
        var email = '';

        if (selectedType === 'NhanVien') {
            email = $('#select-nhan-vien').find('option:selected').data('email');
        } else {
            email = $('#select-khach-hang').find('option:selected').data('email');
        }

        $('#input-email').val(email || '');
    }

    toggleAccountFields();

    $('input[name="accountType"]').on('change', toggleAccountFields);

    $('#select-nhan-vien').on('change', updateEmail);
    $('#select-khach-hang').on('change', updateEmail);

    $('#btn-luu-tai-khoan').on('click', function (e) {
        e.preventDefault();

        var selectedType = $('input[name="accountType"]:checked').val();
        var selectedUserId = (selectedType === 'NhanVien')
            ? $('#select-nhan-vien').val()
            : $('#select-khach-hang').val();

        var data = {
            AccountType: selectedType,
            SelectedUserId: selectedUserId,
            TenDangNhap: $('#TenDangNhap').val(),
            Email: $('#input-email').val(),
            MatKhau: $('#MatKhau').val(),
            XacNhanMatKhau: $('#XacNhanMatKhau').val(),
            TrangThai: $('#select-trang-thai').val(),
            RoleIds: $('#select-roles').val() || []
        };

        if (selectedType === 'KhachHang') {
            data.RoleIds = [];
        }

        $.ajax({
            url: '/API/TaiKhoan/Them',
            type: 'POST',
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
                        if (selectedType === 'NhanVien') {
                            location.href = '/QuanLyBaoMat/DanhSachTaiKhoanNhanVien';
                        } else {
                            location.href = '/QuanLyBaoMat/DanhSachTaiKhoanKhachHang';
                        }
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