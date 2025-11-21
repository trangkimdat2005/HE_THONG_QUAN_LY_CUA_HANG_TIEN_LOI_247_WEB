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
    const INDEX_COT_CA = 2;
    const INDEX_COT_NGAY = 3; 

    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var filterCa = $('#filter-ca').val();
            var filterNgay = $('#filter-ngay').val(); 

            var tableCa = data[INDEX_COT_CA] || "";
            var tableNgay = data[INDEX_COT_NGAY] || ""; 

            var checkCa = true;
            if (filterCa !== "") {
                if (tableCa !== filterCa) {
                    checkCa = false;
                }
            }

            var checkNgay = true;
            if (filterNgay !== "") {
                var parts = tableNgay.split("/");
                if (parts.length === 3) {
                    var formattedTableDate = parts[2] + "-" + parts[1] + "-" + parts[0];

                    if (formattedTableDate !== filterNgay) {
                        checkNgay = false;
                    }
                }
            }

            return checkCa && checkNgay;
        }
    );

    var table = $('#sampleTable').DataTable();

    $('#btn-loc-ca').click(function () {
        table.draw();
    });
});