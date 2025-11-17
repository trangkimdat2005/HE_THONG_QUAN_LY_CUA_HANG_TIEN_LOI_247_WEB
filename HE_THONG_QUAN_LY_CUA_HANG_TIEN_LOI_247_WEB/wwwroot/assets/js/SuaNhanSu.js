$(document).ready(function () {

    $('#select-chuc-vu').select2({
        width: '100%'
    });
    $('#select-trang-thai').select2({
        width: '100%'
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#avatarPreview').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    $("#avatarUpload").change(function () {
        readURL(this);
    });

    $(document).on('click', '#btn-luu-cap-nhat', function (e) {
        e.preventDefault();

        var formData = new FormData();

        formData.append("Id", $('#Id').val());
        formData.append("HoTen", $('#HoTen').val());
        formData.append("ChucVu", $('#select-chuc-vu').val());
        formData.append("LuongCoBan", $('#LuongCoBan').val());
        formData.append("SoDienThoai", $('#SoDienThoai').val());
        formData.append("Email", $('#Email').val());
        formData.append("DiaChi", $('#DiaChi').val());
        formData.append("NgayVaoLam", $('#NgayVaoLam').val());
        formData.append("TrangThai", $('#select-trang-thai').val());
        formData.append("GioiTinh", $('input[name="gioiTinh"]:checked').val() === 'Nam');

        var fileInput = $('#avatarUpload')[0];
        if (fileInput.files.length > 0) {
            formData.append("AnhDaiDien", fileInput.files[0]);
        }

        $.ajax({
            url: '/API/NhanVien/Update',
            type: 'PUT',
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {
                Swal.fire({
                    title: 'Thành công!',
                    text: response.message,
                    icon: 'success',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        location.href = '/QuanLyNhanSu/DanhSachNhanVien';
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