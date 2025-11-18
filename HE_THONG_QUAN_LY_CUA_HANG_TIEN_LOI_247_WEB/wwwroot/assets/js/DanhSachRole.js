$(document).ready(function () {

    var selectQuyen = $('#select-quyen').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều quyền'
    });

    function showForm(isEditing = false) {
        $('#table-panel').removeClass('col-lg-12').addClass('col-lg-8');
        $('#form-panel').show();
    }

    function hideForm() {
        $('#form-panel').hide();
        $('#table-panel').removeClass('col-lg-8').addClass('col-lg-12');
        $('#role-form')[0].reset();
        $('#roleId').val('');
        selectQuyen.val(null).trigger('change');
    }

    $('.btn-add-khoi').on('click', function (e) {
        e.preventDefault();
        $('#role-form')[0].reset();
        $('#roleId').val('');
        selectQuyen.val(null).trigger('change');
        $('#roleCode').prop('readonly', false);
        $('#form-title').text('Thêm Vai Trò Mới');
        showForm(false);
    });

    $('#btn-cancel-form').on('click', function (e) {
        e.preventDefault();
        hideForm();
    });

    $(document).on('click', '.btn-edit-khoi', function (e) {
        e.preventDefault();
        var button = $(this);
        var roleId = button.data('id');

        if (!roleId) {
            Swal.fire('Lỗi', 'Không tìm thấy ID của vai trò.', 'error');
            return;
        }

        $('#form-title').text('Sửa Vai Trò');
        $('#roleId').val(roleId);
        $('#roleCode').val(button.data('code')).prop('readonly', true);
        $('#roleName').val(button.data('ten'));
        $('#roleDescription').val(button.data('mota'));
        $('#roleStatus').val(button.data('trangthai'));

        selectQuyen.val(null).trigger('change');

        $.ajax({
            url: '/API/PhanQuyen/GetPermissionsForRole/' + roleId,
            type: 'GET',
            success: function (permissionIds) {
                if (permissionIds && permissionIds.length > 0) {
                    selectQuyen.val(permissionIds).trigger('change');
                }
                showForm(true);
            },
            error: function () {
                Swal.fire('Lỗi', 'Không thể tải danh sách quyền của vai trò này.', 'error');
            }
        });
    });

    $('#role-form').on('submit', function (e) {
        e.preventDefault();

        var roleId = $('#roleId').val();
        var isEditing = roleId !== '';

        var data = {
            Id: roleId,
            Code: $('#roleCode').val(),
            Ten: $('#roleName').val(),
            MoTa: $('#roleDescription').val(),
            TrangThai: $('#roleStatus').val(),
            PermissionIds: selectQuyen.val()
        };

        var ajaxUrl = isEditing ? '/API/PhanQuyen/UpdateRole' : '/API/PhanQuyen/ThemRole';
        var ajaxType = isEditing ? 'PUT' : 'POST';

        $.ajax({
            url: ajaxUrl,
            type: ajaxType,
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
                        location.reload();
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

    $(document).on('click', '.btn-delete-khoi', function (e) {
        e.preventDefault();

        var button = $(this);
        var id = button.data('id');

        if (!id) {
            Swal.fire('Lỗi', 'Không tìm thấy ID vai trò.', 'error');
            return;
        }

        Swal.fire({
            title: 'Bạn có chắc không?',
            text: "Xóa vai trò này?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Vâng, xóa nó!',
            cancelButtonText: 'Hủy'
        }).then((result) => {

            if (result.isConfirmed) {

                $.ajax({
                    url: '/API/PhanQuyen/DeleteRole/' + id,
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