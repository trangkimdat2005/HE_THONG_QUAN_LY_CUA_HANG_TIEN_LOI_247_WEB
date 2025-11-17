$(document).ready(function () {

    $(document).on('click', '.btn-xoa-phan-cong', function () {

        var button = $(this);
        var id = button.data('id');

        if (!id) {
            Swal.fire('Lỗi', 'Không tìm thấy ID ca phân công.', 'error');
            return;
        }

        Swal.fire({
            title: 'Bạn có chắc không?',
            text: "Xóa ca phân công này?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Vâng, xóa nó!',
            cancelButtonText: 'Hủy'
        }).then((result) => {

            if (result.isConfirmed) {

                $.ajax({
                    url: '/API/PhanCong/Delete/' + id,
                    type: 'DELETE',
                    success: function (response) {
                        Swal.fire({
                            title: 'Đã xóa!',
                            text: response.message,
                            icon: 'success'
                        }).then(() => {
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