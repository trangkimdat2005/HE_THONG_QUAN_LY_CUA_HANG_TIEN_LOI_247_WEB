document.addEventListener('DOMContentLoaded', function () {

    // --- KHỞI TẠO SELECT2 ---
    $('#select-nha-cung-cap').select2({ width: '100%' });
    $('#select-nhan-vien').select2({ width: '100%' });
    $('#select-san-pham').select2({ width: '100%' });

    // --- TỰ ĐỘNG ĐIỀN NGÀY NHẬP LÀ HÔM NAY ---
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('input-ngay-nhap').value = today;

    // --- ĐỊNH DẠNG TIỀN TỆ ---
    const formatter = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    });

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const addItemBtn = document.getElementById('add-item-btn');
    const tableBody = document.getElementById('chi-tiet-phieu-nhap-body');
    const totalSpan = document.getElementById('tong-tien-phieu-nhap');

    const productSelect = $('#select-san-pham');
    const soLuongInput = document.getElementById('input-so-luong');
    const donGiaInput = document.getElementById('input-don-gia');
    const hsdInput = document.getElementById('input-hsd');

    // Lưu thông tin sản phẩm đã thêm
    const chiTietList = [];

    // --- LOGIC NÚT IMPORT EXCEL ---
    const importBtn = document.getElementById('import-excel-btn');
    const importInput = document.getElementById('import-excel-input');
    
    if (importBtn) {
        importBtn.addEventListener('click', function () {
            importInput.click();
        });
    }

    if (importInput) {
        importInput.addEventListener('change', function () {
            if (this.files.length > 0) {
                const fileName = this.files[0].name;
                console.log("Đã chọn file:", fileName);
                alert("Đã chọn file: " + fileName + ". (Logic xử lý file Excel sẽ được lập trình ở đây)");
            }
        });
    }

    // --- HÀM XÓA ROW PLACEHOLDER ---
    function removePlaceholderRow() {
        const placeholder = tableBody.querySelector('tr[data-placeholder]');
        if (placeholder) {
            placeholder.remove();
        }
        
        const rows = tableBody.querySelectorAll('tr');
        rows.forEach(row => {
            const firstCell = row.querySelector('td');
            if (firstCell && firstCell.colSpan > 1 && firstCell.textContent.includes('Chưa có sản phẩm')) {
                row.remove();
            }
        });
    }

    // --- LOGIC THÊM SẢN PHẨM VÀO BẢNG ---
    if (addItemBtn) {
        addItemBtn.addEventListener('click', function () {
            console.log('Add button clicked');
            
            const selectedOption = productSelect.find('option:selected');
            const productId = selectedOption.val();
            const productName = selectedOption.data('name');
            const soLuong = parseInt(soLuongInput.value);
            const donGia = parseFloat(donGiaInput.value);
            const hsd = hsdInput.value;

            console.log('Product:', productId, productName);
            console.log('Values:', soLuong, donGia, hsd);

            // Kiểm tra dữ liệu
            if (!productId) {
                alert("Vui lòng chọn một sản phẩm.");
                return;
            }
            if (isNaN(soLuong) || soLuong <= 0) {
                alert("Số lượng phải lớn hơn 0.");
                return;
            }
            if (isNaN(donGia) || donGia < 0) {
                alert("Vui lòng nhập đơn giá nhập.");
                return;
            }
            if (!hsd) {
                alert("Vui lòng nhập hạn sử dụng cho sản phẩm.");
                return;
            }

            const thanhTien = soLuong * donGia;

            // Xóa row placeholder nếu có
            removePlaceholderRow();

            // Lưu vào danh sách chi tiết
            chiTietList.push({
                sanPhamDonViId: productId,
                soLuong: soLuong,
                donGia: donGia,
                hanSuDung: new Date(hsd).toISOString()
            });

            console.log('Chi tiết list:', chiTietList);

            // Tạo hàng mới
            const row = tableBody.insertRow();
            row.setAttribute('data-product-id', productId);
            row.innerHTML = `
              <td>${productName}</td>
              <td>${soLuong}</td>
              <td>${formatter.format(donGia)}</td>
              <td>${hsd}</td>
              <td class="row-total" data-total="${thanhTien}">${formatter.format(thanhTien)}</td>
              <td><button class="btn btn-danger btn-sm remove-item-btn"><i class="bi bi-trash"></i></button></td>
            `;

            // Cập nhật tổng tiền
            updateTotal();

            // Xoá trường nhập
            productSelect.val('').trigger('change');
            soLuongInput.value = 1;
            donGiaInput.value = '';
            hsdInput.value = '';
        });
    }

    // --- LOGIC XOÁ SẢN PHẨM KHỎI BẢNG ---
    if (tableBody) {
        tableBody.addEventListener('click', function (e) {
            const removeButton = e.target.closest('.remove-item-btn');
            if (removeButton) {
                const row = removeButton.closest('tr');
                const productId = row.getAttribute('data-product-id');
                
                // Xóa khỏi danh sách chi tiết
                const index = chiTietList.findIndex(item => item.sanPhamDonViId === productId);
                if (index > -1) {
                    chiTietList.splice(index, 1);
                }
                
                // Xóa row
                row.remove();
                
                // Nếu không còn sản phẩm nào, thêm lại placeholder
                if (chiTietList.length === 0) {
                    tableBody.innerHTML = `
                        <tr data-placeholder="true">
                            <td colspan="6" class="text-center text-muted">
                                <i class="bi bi-inbox"></i> Chưa có sản phẩm nào. Vui lòng thêm sản phẩm vào phiếu nhập.
                            </td>
                        </tr>
                    `;
                }
                
                updateTotal();
            }
        });
    }

    // --- HÀM CẬP NHẬT TỔNG TIỀN ---
    function updateTotal() {
        let total = 0;
        const allRows = tableBody.querySelectorAll('tr[data-product-id]');

        allRows.forEach(row => {
            const totalCell = row.querySelector('.row-total');
            if (totalCell && totalCell.getAttribute('data-total')) {
                const cellTotal = parseFloat(totalCell.getAttribute('data-total'));
                if (!isNaN(cellTotal)) {
                    total += cellTotal;
                }
            }
        });

        if (totalSpan) {
            totalSpan.textContent = formatter.format(total);
        }
        console.log('Total updated:', total);
    }

    // Khởi tạo tổng tiền ban đầu
    updateTotal();

    // --- LOGIC LƯU PHIẾU NHẬP ---
    const saveBtn = document.getElementById('btn-save-phieu-nhap');
    console.log('Save button found:', saveBtn);
    
    if (saveBtn) {
        saveBtn.addEventListener('click', async function (e) {
            e.preventDefault();
            console.log('Save button clicked');
            
            // Lấy thông tin phiếu nhập
            const nhaCungCapId = document.getElementById('select-nha-cung-cap').value;
            const nhanVienId = document.getElementById('select-nhan-vien').value;
            const ngayNhap = document.getElementById('input-ngay-nhap').value;

            // Validate
            if (!nhaCungCapId) {
                alert('Vui lòng chọn nhà cung cấp!');
                return;
            }

            if (!nhanVienId) {
                alert('Vui lòng chọn nhân viên nhập!');
                return;
            }

            if (!ngayNhap) {
                alert('Vui lòng chọn ngày nhập!');
                return;
            }

            if (chiTietList.length === 0) {
                alert('Vui lòng thêm ít nhất một sản phẩm vào phiếu nhập!');
                return;
            }

            // Tạo object request
            const requestData = {
                nhaCungCapId: nhaCungCapId,
                nhanVienId: nhanVienId,
                ngayNhap: new Date(ngayNhap).toISOString(),
                chiTietPhieuNhap: chiTietList
            };

            console.log('Request data:', requestData);

            try {
                // Hiển thị loading
                saveBtn.disabled = true;
                const originalHTML = saveBtn.innerHTML;
                saveBtn.innerHTML = '<i class="bi bi-hourglass-split me-2"></i>Đang lưu...';

                // Gọi API
                const response = await fetch('/add-PN', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestData)
                });

                console.log('Response status:', response.status);
                console.log('Response headers:', response.headers);

                // Kiểm tra content type
                const contentType = response.headers.get("content-type");
                console.log('Content-Type:', contentType);

                let result;
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    result = await response.json();
                } else {
                    // Nếu không phải JSON, đọc như text
                    const text = await response.text();
                    console.log('Response text:', text);
                    throw new Error('Server không trả về JSON. Response: ' + text.substring(0, 200));
                }

                console.log('Response:', result);

                if (response.ok) {
                    alert(result.message || 'Thêm phiếu nhập thành công!');
                    window.location.href = '/QuanLyKhoHang/DanhSachPhieuNhap';
                } else {
                    alert('Lỗi: ' + (result.message || 'Không thể thêm phiếu nhập'));
                    saveBtn.disabled = false;
                    saveBtn.innerHTML = originalHTML;
                }
            } catch (error) {
                console.error('Error:', error);
                alert('Có lỗi xảy ra khi lưu phiếu nhập: ' + error.message);
                saveBtn.disabled = false;
                saveBtn.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Lưu Phiếu Nhập';
            }
        });
    } else {
        console.error('Save button not found! Looking for #btn-save-phieu-nhap');
    }

});