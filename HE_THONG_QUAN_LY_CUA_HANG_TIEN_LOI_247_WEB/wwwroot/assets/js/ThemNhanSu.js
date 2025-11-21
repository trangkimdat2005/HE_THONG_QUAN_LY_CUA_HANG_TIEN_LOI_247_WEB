$(document).ready(function () {

    // Khởi tạo Select2
    $('#select-chuc-vu').select2({
        width: '100%'
    });
    $('#select-trang-thai').select2({
        width: '100%'
    });

    // Xử lý preview ảnh
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

    $(document).on('click', '#btn-luu-nhan-vien', function (e) {
        e.preventDefault();


        var formData = new FormData();

        // formData.append("Id", $("#Id").val()) 

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

        // Gửi AJAX
        $.ajax({
            url: '/API/NhanVien/Them',
            type: 'POST',
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
                var message = 'Lỗi máy chủ hoặc không thể kết nối.';
                var response = jqXHR.responseJSON;

                if (jqXHR.status === 400 && response) {
                    if (response.errors) {
                        title = 'Dữ liệu không hợp lệ!';
                        message = "";
                        for (var key in response.errors) {
                            if (response.errors.hasOwnProperty(key) && Array.isArray(response.errors[key])) {
                                message += response.errors[key].join("<br>") + "<br>";
                            }
                        }
                    }
                    else if (typeof response === 'object' && !response.message) {
                        title = 'Dữ liệu không hợp lệ!';
                        message = "";
                        for (var key in response) {
                            if (response.hasOwnProperty(key) && Array.isArray(response[key])) {
                                message += response[key].join("<br>") + "<br>";
                            }
                        }
                    } else if (response.message) {
                        message = response.message;
                    }
                } else if (jqXHR.responseText) {
                    message = "Lỗi máy chủ (chi tiết trong console).";
                    console.error(jqXHR.responseText);
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


$(async function callApiGetNextIdNV() {
    const dataToSend = {
        prefix: "NV",
        totalLength: 6
    };
    try {
        const response = await fetch('/API/get-next-id-NV',
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dataToSend)
            });
        const data = await response.json();
        if (data) {
            document.getElementById('Id').value = data.nextId;
        }
        else {
            alert('Không thể lấy mã nhân viên, vui lòng thử lại.');
        }
    } catch (error) {
        console.error('Lỗi khi lấy mã nhân viên:', error);
        alert('Không thể lấy mã nhân viên, vui lòng thử lại.');
    }
});