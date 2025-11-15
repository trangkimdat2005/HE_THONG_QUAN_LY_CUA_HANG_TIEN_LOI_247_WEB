$(document).ready(function () {
    let currentRowSPVT = null;
    let currentSanPhamDonViId = null;
    let currentViTriId = null;

    // ================== XỬ LÝ BẢNG SẢN PHẨM VỊ TRÍ ==================
    $(document).on('click', '.btn-edit-sp-vt', function () {
        currentRowSPVT = $(this).closest('tr');
        
        // Lấy dữ liệu từ row
        let tenSP = currentRowSPVT.find('td:eq(0)').text().trim();
        let maViTriCu = currentRowSPVT.find('td:eq(2)').text().trim();
        let soLuong = currentRowSPVT.find('td:eq(4)').text().trim();
        
        // Lưu ID sản phẩm và vị trí (cần được render từ server)
        currentSanPhamDonViId = currentRowSPVT.data('sanpham-id');
        currentViTriId = currentRowSPVT.data('vitri-id');
        
        console.log('Editing:', { currentSanPhamDonViId, currentViTriId, soLuong });

        $('#sp-vt-ten').text(tenSP);
        $('#sp-vt-ma-cu').val(maViTriCu);
        $('#sp-vt-ma-moi').val(currentViTriId); 
        $('#sp-vt-so-luong').val(soLuong);

        $('#modalSuaSanPhamViTri').modal('show');
    });

    $('#btn-luu-sp-vt').click(async function () {
        if (!currentSanPhamDonViId || !currentViTriId) {
            alert('Không tìm thấy thông tin sản phẩm!');
            return;
        }

        let viTriMoi = $('#sp-vt-ma-moi').val();
        let soLuongMoi = parseInt($('#sp-vt-so-luong').val());

        if (isNaN(soLuongMoi) || soLuongMoi <= 0) {
            alert('Số lượng phải lớn hơn 0!');
            return;
        }

        const requestData = {
            sanPhamDonViId: currentSanPhamDonViId,
            viTriIdCu: currentViTriId,
            viTriIdMoi: viTriMoi,
            soLuong: soLuongMoi
        };

        console.log('Sending update request:', requestData);

        try {
            const response = await fetch('/CapNhatSanPhamViTri', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(requestData)
            });

            const result = await response.json();

            if (response.ok) {
                alert(result.message || 'Cập nhật thành công!');
                location.reload(); // Reload để cập nhật dữ liệu
            } else {
                alert('Lỗi: ' + (result.message || 'Không thể cập nhật'));
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Có lỗi xảy ra: ' + error.message);
        }
    });

    $(document).on('click', '.btn-delete-sp-vt', async function () {
        if (!confirm('Bạn chắc chắn muốn xóa sản phẩm này khỏi vị trí?')) {
            return;
        }

        const row = $(this).closest('tr');
        const sanPhamDonViId = row.data('sanpham-id');
        const viTriId = row.data('vitri-id');

        console.log('Deleting:', { sanPhamDonViId, viTriId });

        try {
            const response = await fetch('/XoaSanPhamViTri', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ sanPhamDonViId, viTriId })
            });

            const result = await response.json();

            if (response.ok) {
                alert(result.message || 'Xóa thành công!');
                row.fadeOut(300, function() { $(this).remove(); });
            } else {
                alert('Lỗi: ' + (result.message || 'Không thể xóa'));
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Có lỗi xảy ra: ' + error.message);
        }
    });

    // ================== XỬ LÝ BẢNG DANH MỤC VỊ TRÍ ==================
    let currentRowDMVT = null;
    let currentViTriDMId = null;

    $(document).on('click', '.btn-edit-dm-vt', function () {
        currentRowDMVT = $(this).closest('tr');
        currentViTriDMId = currentRowDMVT.data('vitri-id');
        
        let maViTri = currentRowDMVT.find('td:eq(0)').text().trim();
        let loaiViTri = currentRowDMVT.find('td:eq(1)').text().trim();
        let moTa = currentRowDMVT.find('td:eq(2)').text().trim();

        console.log('Editing ViTri:', { currentViTriDMId, maViTri, loaiViTri });

        $('#dm-vt-ma').val(maViTri);
        $('#dm-vt-loai').val(loaiViTri);
        $('#dm-vt-mo-ta').val(moTa);

        $('#modalSuaDanhMucViTri').modal('show');
    });

    $('#btn-luu-dm-vt').click(async function () {
        if (!currentViTriDMId) {
            alert('Không tìm thấy thông tin vị trí!');
            return;
        }

        let maViTri = $('#dm-vt-ma').val().trim();
        let loaiViTri = $('#dm-vt-loai').val();
        let moTa = $('#dm-vt-mo-ta').val().trim();

        if (!maViTri) {
            alert('Vui lòng nhập mã vị trí!');
            return;
        }

        const requestData = {
            id: currentViTriDMId,
            maViTri: maViTri,
            loaiViTri: loaiViTri,
            moTa: moTa
        };

        console.log('Updating ViTri:', requestData);

        try {
            const response = await fetch('/CapNhatViTri', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(requestData)
            });

            const result = await response.json();

            if (response.ok) {
                alert(result.message || 'Cập nhật thành công!');
                location.reload();
            } else {
                alert('Lỗi: ' + (result.message || 'Không thể cập nhật'));
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Có lỗi xảy ra: ' + error.message);
        }
    });

    $(document).on('click', '.btn-delete-dm-vt', async function () {
        if (!confirm('Bạn chắc chắn muốn xóa vị trí này? (Chỉ có thể xóa nếu vị trí chưa được sử dụng)')) {
            return;
        }

        const row = $(this).closest('tr');
        const viTriId = row.data('vitri-id');

        console.log('Deleting ViTri:', viTriId);

        try {
            const response = await fetch('/XoaViTri', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ id: viTriId })
            });

            const result = await response.json();

            if (response.ok) {
                alert(result.message || 'Xóa thành công!');
                row.fadeOut(300, function() { $(this).remove(); });
            } else {
                alert('Lỗi: ' + (result.message || 'Không thể xóa'));
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Có lỗi xảy ra: ' + error.message);
        }
    });

    // 5. Nút HỦY BỎ (Reset Form) - Đã cập nhật ID thành #btn-cancel
    $('#btn-cancel').click(function (e) { 
        e.preventDefault();
        resetForm();
    });

    // Hàm thực hiện reset tất cả các trường nhập liệu
    function resetForm() {
        // --- 1. Thông tin Phiếu Gán Vị Trí ---
        $('#select-nhan-vien').val('NV001').trigger('change');
        $('#input-ngay-thuc-hien').val(new Date().toISOString().split('T')[0]);

        // --- 2. Chi Tiết Sản Phẩm & Vị Trí (Khu vực nhập liệu) ---
        $('#select-san-pham').val('').trigger('change');
        $('#select-ma-vi-tri').val('').trigger('change');
        $('#select-loai-vi-tri').val('Kho');
        $('#input-so-luong').val(1);
        $('#input-gia-tien').val('');

        // Đặt lại biến đếm STT
        stt = 1;
    }
});