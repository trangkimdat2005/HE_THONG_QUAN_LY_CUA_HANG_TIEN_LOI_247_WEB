$(document).ready(function () {

    // --- KHỞI TẠO SELECT2 ---
    // (Đảm bảo jQuery đã được tải trước)
    $('#select-nhan-vien').select2({
        width: '100%',
        placeholder: '-- Tìm và chọn phân công --'
    });

    $('#select-ca-lam-viec').select2({
        width: '100%',
        placeholder: '-- Chọn ca làm việc --'
    });

    // --- TỰ ĐỘNG ĐIỀN NGÀY HÔM NAY ---
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('input-ngay').value = today;

    // Khởi tạo Select2
    $('#select-nhan-vien').select2({
        width: '100%'
    });
    $('#select-ca-lam-viec').select2({
        width: '100%'
    });

    // Bắt sự kiện click nút LƯU
    $(document).on('click', '#btn-luu-phan-cong', function (e) {
        e.preventDefault(); 

        // Tạo object data
        var data = {
            NhanVienId: $('#select-nhan-vien').val(),
            CaLamViecId: $('#select-ca-lam-viec').val(),
            Ngay: $('#input-ngay').val()
        };

        // Gửi AJAX
        $.ajax({
            url: '/API/PhanCong/Them',
            type: 'POST',
            contentType: 'application/json', // Dùng JSON vì không có file
            data: JSON.stringify(data), // Gửi JSON

            success: function (response) {
                Swal.fire({
                    title: 'Thành công!',
                    text: response.message,
                    icon: 'success',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Quay về trang danh sách (lấy từ breadcrumb)
                        location.href = '/QuanLyNhanSu/PhanCongCaLamViec';
                    }
                });
            },
            error: function (jqXHR) {
                var title = 'Đã xảy ra lỗi!';
                var message = 'Lỗi máy chủ hoặc không thể kết nối.';
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
                } else if (jqXHR.responseText) {
                    message = "Lỗi máy chủ. Vui lòng kiểm tra console.";
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

$(async function callApiGetNextIdPCCLV() {
    const dataToSend = {
        prefix: "PCCLV",
        totalLength: 9
    };
    try {
        const response = await fetch('/API/get-next-id-PCCLV',
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
            alert('Không thể lấy mã phân công, vui lòng thử lại.');
        }
    } catch (error) {
        console.error('Lỗi khi lấy mã phân công:', error);
        alert('Không thể lấy mã phân công, vui lòng thử lại.');
    }
});