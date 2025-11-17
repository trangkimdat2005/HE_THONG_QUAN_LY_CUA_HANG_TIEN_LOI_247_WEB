$(document).on('click', '.btn-delete-khoi', function () {
    var button = $(this); // Giữ tham chiếu đến nút đã bấm
    var id = button.data('id'); // Lấy ID từ thuộc tính data-id

    if (!id) {
        Swal.fire('Lỗi', 'Không tìm thấy ID.', 'error');
        return;
    }

    // 1. HỎI XÁC NHẬN
    Swal.fire({
        title: 'Bạn có chắc không?',
        text: "Bạn sẽ không thể hoàn tác hành động này!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Vâng, xóa nó!',
        cancelButtonText: 'Hủy'
    }).then((result) => {

        if (result.isConfirmed) {
            $.ajax({
                url: '/API/NhaCungCap/Delete/' + id,
                type: 'DELETE', // Method là DELETE
                success: function (response) {
                    // 4. THÀNH CÔNG (Server trả về Ok)
                    Swal.fire({
                        title: 'Đã xóa!',
                        text: response.message, // Hiển thị message từ Controller
                        icon: 'success'
                    }).then(() => {
                        button.closest('tr').remove();
                    });
                },
                error: function (jqXHR) {
                    // 5. THẤT BẠI (Server trả về BadRequest, NotFound, v.v...)
                    var response = jqXHR.responseJSON;
                    var message = 'Đã xảy ra lỗi không xác định.';

                    if (response && response.message) {
                        message = response.message;
                    }

                    Swal.fire(
                        'Lỗi!',
                        message,
                        'error'
                    );
                }
            });
        }
    });
});




