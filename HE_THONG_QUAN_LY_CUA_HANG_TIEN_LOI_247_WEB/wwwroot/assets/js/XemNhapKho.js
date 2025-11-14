document.addEventListener('DOMContentLoaded', function () {

    // --- KHỞI TẠO SELECT2 ---
    const selectSanPham = $('#select-san-pham');
    
    selectSanPham.select2({ width: '100%' });

    // --- ĐỊNH DẠNG TIỀN TỆ ---
    const formatter = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    });

    // --- TRUY XUẤT PHẦN TỬ DOM CHÍNH ---
    const addItemBtn = document.getElementById('add-item-btn');
    const tableBody = document.getElementById('chi-tiet-phieu-nhap-body');
    const totalSpan = document.getElementById('tong-tien-phieu-nhap');

    const soLuongInput = document.getElementById('input-so-luong');
    const donGiaInput = document.getElementById('input-don-gia');
    const hsdInput = document.getElementById('input-hsd');
    
    // --- TRUY XUẤT CÁC PHẦN TỬ ĐIỀU KHIỂN CHẾ ĐỘ ---
    const container = document.getElementById('phieu-nhap-container');
    const btnEdit = document.getElementById('btn-edit');
    const btnSave = document.getElementById('btn-save');
    
    // --- CẬP NHẬT TỔNG TIỀN KHI TRANG LOAD ---
    updateTotal();
    
    // --- LOGIC CHUYỂN CHẾ ĐỘ XEM / CHỈNH SỬA ---
    
    btnEdit.addEventListener('click', function() {
        // Bật container sang chế độ 'edit-mode' (CSS sẽ xử lý ẩn/hiện)
        container.classList.add('edit-mode');
        
        // Kích hoạt các trường nhập liệu (nếu cần)
    });

    btnSave.addEventListener('click', function() {
        // (Đây là nơi bạn sẽ gọi API để lưu dữ liệu)
        alert('Đã lưu thay đổi! (Giả lập)');

        // Tắt chế độ 'edit-mode'
        container.classList.remove('edit-mode');
    });


    // --- LOGIC THÊM SẢN PHẨM VÀO BẢNG (KHI Ở CHẾ ĐỘ EDIT) ---
    addItemBtn.addEventListener('click', function () {
        const selectedOption = selectSanPham.find('option:selected');
        const productId = selectedOption.val();
        const productName = selectedOption.data('name');
        const soLuong = parseInt(soLuongInput.value);
        const donGia = parseFloat(donGiaInput.value);
        const hsd = hsdInput.value;

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

        // Tạo hàng mới
        const row = tableBody.insertRow();
        row.setAttribute('data-id', productId);
        row.innerHTML = `
          <td>${productName} (ID: ${productId})</td>
          <td>${soLuong}</td>
          <td class="don-gia">${donGia.toLocaleString('vi-VN')}</td>
          <td>${hsd}</td>
          <td class="row-total" data-total="${thanhTien}">${thanhTien.toLocaleString('vi-VN')}</td>
          <td class="delete-column"><button class="btn btn-danger btn-sm remove-item-btn"><i class="bi bi-trash"></i></button></td>
        `;

        // Cập nhật tổng tiền
        updateTotal();

        // Xoá trường nhập
        selectSanPham.val('').trigger('change');
        soLuongInput.value = 1;
        donGiaInput.value = '';
        hsdInput.value = '';
    });

    // --- LOGIC XOÁ SẢN PHẨM KHỎI BẢNG (KHI Ở CHẾ ĐỘ EDIT) ---
    tableBody.addEventListener('click', function (e) {
        const removeButton = e.target.closest('.remove-item-btn');
        if (removeButton) {
            removeButton.closest('tr').remove();
            updateTotal(); // Cập nhật lại tổng tiền
        }
    });

    // --- HÀM CẬP NHẬT TỔNG TIỀN (PhieuNhap.tongTien) ---
    function updateTotal() {
        let total = 0;
        const allRows = tableBody.querySelectorAll('tr');

        allRows.forEach(row => {
            const totalCell = row.querySelector('.row-total');
            if (totalCell) {
                const cellTotal = parseFloat(totalCell.getAttribute('data-total'));
                if (!isNaN(cellTotal)) {
                    total += cellTotal;
                }
            }
        });

        totalSpan.textContent = total.toLocaleString('vi-VN') + ' đ';
    }

});