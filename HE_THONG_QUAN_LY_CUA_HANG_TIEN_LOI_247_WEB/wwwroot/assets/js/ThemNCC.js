$(document).ready(function () {

    // Gắn một sự kiện 'click' vào button có id là 'submitBtn'
    $('#submitBtn').on('click', function () {

        // Tạo đối tượng dữ liệu
        var data = {
            Id: $('#add-id-input').val(),                
            Ten: $('#Ten').val(),
            SoDienThoai: $('#SoDienThoai').val(), 
            Email: $('#Email').val(), 
            DiaChi: $('#DiaChi').val(),         
            MaSoThue: $('#MaSoThue').val()   
        };

        // Gửi AJAX khi click
        $.ajax({
            url: '/API/ThemNCC/Add',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),

            // 1. SỬA KHI THÀNH CÔNG (dùng SweetAlert)
            success: function (response) {
                // response là một object, ví dụ: { message: "Thêm thành công..." }
                Swal.fire({
                    title: 'Thành công!',
                    text: response.message, // Hiển thị message từ server
                    icon: 'success',        // Biểu tượng thành công
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        location.href = '/QuanLyNhaCungCap/DanhSachNhaCungCap';
                    }
                });
            },

            // 2. SỬA KHI CÓ LỖI (dùng SweetAlert)
            error: function (jqXHR) {
                var response = jqXHR.responseJSON;
                var title = 'Đã xảy ra lỗi!';
                var message = 'Đã xảy ra lỗi máy chủ hoặc không thể kết nối.'; // Mặc định

                // Trường hợp 1: Lỗi validation (từ [Required], [EmailAddress]...)
                if (response && response.errors) {
                    title = 'Dữ liệu không hợp lệ!';
                    message = ""; // Ta sẽ xây dựng message từ các lỗi

                    // Nối tất cả các lỗi validation lại
                    for (var key in response.errors) {
                        if (response.errors.hasOwnProperty(key)) {
                            // Dùng <br> thay vì \n để SweetAlert xuống hàng đẹp hơn
                            message += response.errors[key].join("<br>") + "<br>";
                        }
                    }
                }
                // Trường hợp 2: Lỗi nghiệp vụ (từ BadRequest(new { message = "..." }))
                else if (response && response.message) {
                    message = response.message; // Lấy lỗi từ server
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
});


$(async function callApiGetNextIdNCC() {
    const dataToSend = {
        prefix: "NCC",
        totalLength: 7
    };
    try {
        const response = await fetch('/API/get-next-id-NCC',
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dataToSend)
            });
        const data = await response.json();
        if (data) {
            document.getElementById('add-id-input').value = data.nextId;
        }
        else {
            alert('Không thể lấy mã nhà cung cấp, vui lòng thử lại.');
        }
    } catch (error) {
        console.error('Lỗi khi lấy mã nhà cung cấp:', error);
        alert('Không thể lấy mã nhà cung cấp, vui lòng thử lại.');
    }
});