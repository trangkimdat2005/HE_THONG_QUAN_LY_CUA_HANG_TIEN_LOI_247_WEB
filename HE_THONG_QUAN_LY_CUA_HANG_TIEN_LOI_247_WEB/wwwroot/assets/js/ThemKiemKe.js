document.addEventListener('DOMContentLoaded', function () {
    console.log('=== THEM KIEM KE PAGE LOADED ===');

    $('#select-nhan-vien').select2({ width: '100%' });
    $('#select-san-pham').select2({ width: '100%' });

    const today = new Date().toISOString().split('T')[0];
    document.getElementById('input-ngay-kiem-ke').value = today;

    const addBtn = document.getElementById('add-item-btn');
    const tableBody = document.getElementById('kiem-ke-body');
    const productSelect = $('#select-san-pham');
    const soLuongThucTeInput = document.getElementById('input-thuc-te');
    const ghiChuInput = document.getElementById('input-ghi-chu');
    const btnSavePhieu = document.getElementById('btn-save-phieu');

    // TODO: Lấy từ API hoặc database thực tế
    const heThongTonKho = {
        'SPDV_001': 150,
        'SPDV_002': 80,
        'SPDV_003': 200,
        'SPDV_005': 100,
        'SPDV_006': 50
    };

    // Thêm sản phẩm vào bảng
    addBtn.addEventListener('click', function () {
        const selectedOption = productSelect.find('option:selected');
        const productId = selectedOption.val();
        const productName = selectedOption.data('name');

        console.log('Adding product:', { productId, productName });

        if (!productId) {
            alert("Vui lòng chọn một sản phẩm.");
            return;
        }

        // Kiểm tra sản phẩm đã có trong table chưa
        const existingRows = tableBody.querySelectorAll('tr');
        for (let row of existingRows) {
            const existingId = row.cells[0].getAttribute('data-id');
            if (existingId === productId) {
                alert("Sản phẩm này đã được thêm vào danh sách!");
                return;
            }
        }

        let soLuongThucTe;
        try {
            soLuongThucTe = parseInt(soLuongThucTeInput.value);
            if (isNaN(soLuongThucTe) || soLuongThucTe < 0) {
                throw new Error();
            }
        } catch (e) {
            alert("Vui lòng nhập số lượng thực tế hợp lệ (lớn hơn hoặc bằng 0).");
            soLuongThucTeInput.focus();
            return;
        }

        const ghiChu = ghiChuInput.value.trim();
        const soLuongHeThong = heThongTonKho[productId] || 0;
        const chenhLech = soLuongThucTe - soLuongHeThong;

        let chenhLechClass = 'chenh-lech-khop';
        let chenhLechText = chenhLech;
        let ketQua = `Khớp (${soLuongThucTe})`;

        if (chenhLech > 0) {
            chenhLechClass = 'chenh-lech-duong';
            chenhLechText = '+' + chenhLech;
            ketQua = `Thừa +${chenhLech} (Hệ thống: ${soLuongHeThong}, Thực tế: ${soLuongThucTe})`;
        } else if (chenhLech < 0) {
            chenhLechClass = 'chenh-lech-am';
            ketQua = `Thiếu ${chenhLech} (Hệ thống: ${soLuongHeThong}, Thực tế: ${soLuongThucTe})`;
        }

        if (ghiChu) {
            ketQua += ` - ${ghiChu}`;
        }

        const row = tableBody.insertRow();
        row.innerHTML = `
          <td data-id="${productId}" data-ket-qua="${ketQua}">${productName}</td>
          <td class="text-end">${soLuongHeThong}</td>
          <td class="text-end">${soLuongThucTe}</td>
          <td class="text-end ${chenhLechClass}">${chenhLechText}</td>
          <td>${ghiChu}</td>
          <td><button class="btn btn-danger btn-sm remove-item-btn"><i class="bi bi-trash"></i></button></td>
        `;

        console.log('Product added to table');

        // Reset inputs
        productSelect.val('').trigger('change');
        soLuongThucTeInput.value = '';
        ghiChuInput.value = '';
    });

    // Xóa sản phẩm khỏi bảng
    tableBody.addEventListener('click', function (e) {
        const removeButton = e.target.closest('.remove-item-btn');
        if (removeButton) {
            console.log('Removing product from table');
            removeButton.closest('tr').remove();
        }
    });

    // Lưu phiếu kiểm kê
    if (btnSavePhieu) {
        btnSavePhieu.addEventListener('click', async function () {
            console.log('=== BẮT ĐẦU LƯU PHIẾU KIỂM KÊ ===');

            try {
                // Lấy dữ liệu từ form
                const nhanVienId = $('#select-nhan-vien').val();
                const ngayKiemKe = document.getElementById('input-ngay-kiem-ke').value;

                console.log('Form data:', { nhanVienId, ngayKiemKe });

                // Validate
                if (!nhanVienId) {
                    alert('Vui lòng chọn nhân viên thực hiện!');
                    $('#select-nhan-vien').focus();
                    return;
                }

                if (!ngayKiemKe) {
                    alert('Vui lòng chọn ngày kiểm kê!');
                    document.getElementById('input-ngay-kiem-ke').focus();
                    return;
                }

                // Lấy danh sách sản phẩm từ table
                const rows = tableBody.querySelectorAll('tr');
                if (rows.length === 0) {
                    alert('Vui lòng thêm ít nhất một sản phẩm để kiểm kê!');
                    return;
                }

                console.log(`Found ${rows.length} products in table`);

                // Tạo danh sách chi tiết kiểm kê
                const chiTietKiemKe = [];
                rows.forEach((row, index) => {
                    const sanPhamDonViId = row.cells[0].getAttribute('data-id');
                    const ketQua = row.cells[0].getAttribute('data-ket-qua');
                    
                    console.log(`Product ${index + 1}:`, { sanPhamDonViId, ketQua });

                    chiTietKiemKe.push({
                        SanPhamDonViId: sanPhamDonViId,
                        KetQua: ketQua
                    });
                });

                // Tạo request object
                const requestData = {
                    NhanVienId: nhanVienId,
                    NgayKiemKe: ngayKiemKe,
                    ChiTietKiemKe: chiTietKiemKe
                };

                console.log('Request data:', JSON.stringify(requestData, null, 2));

                // Hiển thị loading
                btnSavePhieu.disabled = true;
                const originalHTML = btnSavePhieu.innerHTML;
                btnSavePhieu.innerHTML = '<i class="fas fa-hourglass-half me-2"></i>Đang lưu...';

                // Gọi API
                console.log('Calling API /API/add-KiemKe...');
                const response = await fetch('/API/add-KiemKe', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestData)
                });

                console.log('Response status:', response.status);

                const contentType = response.headers.get('content-type');
                let result;

                if (contentType && contentType.includes('application/json')) {
                    result = await response.json();
                    console.log('Response JSON:', result);
                } else {
                    const text = await response.text();
                    console.error('Response is not JSON:', text);
                    throw new Error('Server không trả về JSON. Nội dung: ' + text);
                }

                if (response.ok) {
                    console.log('✅ SUCCESS:', result.message);
                    alert(result.message || 'Tạo phiếu kiểm kê thành công!');

                    // Chuyển về trang danh sách
                    console.log('Redirecting to /QuanLyKhoHang/KiemKeSanPham');
                    window.location.href = '/QuanLyKhoHang/KiemKeSanPham';
                } else {
                    console.error('❌ ERROR:', result.message);
                    alert('Lỗi: ' + (result.message || 'Không thể tạo phiếu kiểm kê'));

                    btnSavePhieu.disabled = false;
                    btnSavePhieu.innerHTML = originalHTML;
                }
            } catch (error) {
                console.error('❌ EXCEPTION:', error);
                alert('Có lỗi xảy ra khi lưu phiếu kiểm kê:\n\n' + error.message);

                btnSavePhieu.disabled = false;
                btnSavePhieu.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Lưu Phiếu Kiểm Kê';
            }
        });
    } else {
        console.error('Button save phieu not found!');
    }
});