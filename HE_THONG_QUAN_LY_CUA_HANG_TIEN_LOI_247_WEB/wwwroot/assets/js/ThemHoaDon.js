document.addEventListener('DOMContentLoaded', function () {
    console.log('=== THÊM HÓA ĐƠN PAGE LOADED ===');

    // === KHỞI TẠO SELECT2 ===
    $('#selectKhachHang').select2({
        width: '100%',
        placeholder: 'Chọn khách hàng',
        allowClear: true
    });
    $('#selectDanhMuc').select2({
        width: '100%',
        placeholder: 'Tất cả danh mục',
        allowClear: true
    });

    // === BIẾN TOÀN CỤC ===
    let appliedDiscount = null;
    let discountValue = 0;
    let draftInvoices = JSON.parse(localStorage.getItem('draftInvoices') || '[]');
    function removeVietnameseTones(str) {
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
        str = str.replace(/Đ/g, "D");
        // Bỏ các ký tự đặc biệt nếu cần
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, "");
        str = str.replace(/\u02C6|\u0306|\u031B/g, "");
        return str.trim().toLowerCase();
    }
    // === ELEMENTS ===
    const productList = document.getElementById('product-list');
    const invoiceBody = document.getElementById('invoice-items-body');
    const subTotalEl = document.getElementById('sub-total');
    const discountEl = document.getElementById('discount-amount');
    const totalEl = document.getElementById('total-amount');
    const searchInput = document.getElementById('product-search-input');
    const categorySelect = document.getElementById('selectDanhMuc');
    const customerSelect = $('#selectKhachHang');
    const discountInput = document.getElementById('discount-code-input');
    const applyDiscountBtn = document.getElementById('apply-discount-btn');
    const discountMessage = document.getElementById('discount-message');
    const saveDraftBtn = document.getElementById('save-draft-btn');
    const cancelInvoiceBtn = document.getElementById('cancel-invoice-btn');
    const completeInvoiceBtn = document.getElementById('complete-invoice-btn');
    const draftList = document.getElementById('draft-list');
    const currentUserId = document.getElementById('current-user-id')?.value || 'NV0001';

    // === FORMAT TIỀN TỆ ===
    const formatter = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    });

    // === DEBOUNCE HELPER ===
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }
    function renderKhuyenMaiOptions() {
        // 1. Kiểm tra dữ liệu
        if (typeof danhSachKhuyenMai === 'undefined') {
            return '<option value="" data-val="0">-- Chọn KM --</option>';
        }

        let options = '<option value="" data-val="0">-- Chọn KM --</option>';

        danhSachKhuyenMai.forEach(km => {
            let hienThiGiam = (km.loaiGiam === "Phần trăm")
                ? (km.giaTri * 100) + "%"
                : formatter.format(km.giaTri);

            options += `<option value="${km.id}" 
                        data-val="${km.giaTri}" 
                        data-type="${km.loaiGiam}" 
                        data-min="${km.toithieu}" 
                        data-max="${km.toida}"
                        data-start="${km.ngayBatDau}" 
                        data-end="${km.ngayKetThuc}">
                        ${km.ma} - Giảm ${hienThiGiam}
                    </option>`;
        });
        return options;
    }
    // === 1. LỌC SẢN PHẨM (TỐI ƯU) ===
    function filterProducts() {
        const rawKeyword = searchInput.value;
        // Kiểm tra null trước khi xử lý
        const searchText = rawKeyword ? removeVietnameseTones(rawKeyword) : "";

        const category = categorySelect.value;
        const allProducts = productList.querySelectorAll('.product-item');

        let visibleCount = 0;

        allProducts.forEach(product => {
            const rawName = product.getAttribute('data-product-name') || '';
            const rawId = product.getAttribute('data-product-id') || '';
            const productCategory = product.getAttribute('data-category') || '';

            const nameNormalized = removeVietnameseTones(rawName);
            const idNormalized = removeVietnameseTones(rawId);

            const matchesSearch = !searchText ||
                nameNormalized.includes(searchText) ||
                idNormalized.includes(searchText);

            const matchesCategory = !category || productCategory === category;

            if (matchesSearch && matchesCategory) {
                // HIỆN: Xóa d-none, Thêm lại d-flex để giữ layout đẹp
                product.classList.remove('d-none');
                product.classList.add('d-flex');
                visibleCount++;
            } else {
                // ẨN: Xóa d-flex (để tránh xung đột), Thêm d-none
                product.classList.remove('d-flex');
                product.classList.add('d-none');
            }
        });

        updateProductListMessage(visibleCount);
    }
    function updateProductListMessage(count) {
        let messageEl = productList.querySelector('.no-products-message');

        if (count === 0) {
            if (!messageEl) {
                messageEl = document.createElement('li');
                messageEl.className = 'list-group-item text-center text-muted no-products-message';
                messageEl.textContent = 'Không tìm thấy sản phẩm nào';
                productList.appendChild(messageEl);
            }
        } else {
            if (messageEl) {
                messageEl.remove();
            }
        }
    }

    const debouncedFilter = debounce(filterProducts, 300);
    searchInput.addEventListener('input', debouncedFilter);

    $('#selectDanhMuc').on('change', filterProducts);


    // === 2. THÊM SẢN PHẨM VÀO HÓA ĐƠN ===
    productList.addEventListener('click', function (e) {
        const addButton = e.target.closest('.add-product-btn');
        if (addButton) {
            e.preventDefault();
            const id = addButton.getAttribute('data-id');
            const name = addButton.getAttribute('data-name');
            const price = parseFloat(addButton.getAttribute('data-price'));
            addProductToInvoice(id, name, price);

            // Visual feedback
            addButton.classList.add('btn-success');
            setTimeout(() => {
                addButton.classList.remove('btn-success');
            }, 200);
        }
    });

    function addProductToInvoice(id, name, price, quantity = 1) {
        const emptyRow = invoiceBody.querySelector('tr td[colspan="6"]');
        if (emptyRow) {
            emptyRow.closest('tr').remove();
        }

        const existingRow = document.querySelector(`#invoice-items-body tr[data-id="${id}"]`);

        if (existingRow) {
            const qtyInput = existingRow.querySelector('.quantity-input');
            qtyInput.value = parseInt(qtyInput.value) + quantity;
            updateRowTotal(existingRow);

            existingRow.classList.add('table-success');
            setTimeout(() => {
                existingRow.classList.remove('table-success');
            }, 500);
        } else {
            const row = invoiceBody.insertRow();
            row.setAttribute('data-id', id);
            row.innerHTML = `
                <td>${name}</td>
                <td>
                    <input type="number" class="form-control form-control-sm quantity-input" value="${quantity}" min="1" data-price="${price}">
                </td>
                <td>${formatter.format(price)}</td>
                
                <td>
                    <select class="form-control form-control-sm discount-select" style="width: 150px;">
                        ${renderKhuyenMaiOptions()}
                    </select>
                </td>

                <td class="row-total">${formatter.format(price * quantity)}</td>
                <td class="text-center">
                    <button class="btn btn-danger btn-sm remove-item-btn">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            `;
        }
        updateInvoiceTotal();
    }

    // === 3. CẬP NHẬT SỐ LƯỢNG VÀ XÓA SẢN PHẨM ===
    invoiceBody.addEventListener('input', function (e) {
        const qtyInput = e.target.closest('.quantity-input');
        if (qtyInput) {
            if (qtyInput.value < 1) qtyInput.value = 1;
            const row = qtyInput.closest('tr');
            updateRowTotal(row);
            updateInvoiceTotal();
        }
    });

    invoiceBody.addEventListener('change', function (e) {
        if (e.target.classList.contains('discount-select')) {
            const row = e.target.closest('tr');
            updateRowTotal(row);
            updateInvoiceTotal();
        }
    });

    invoiceBody.addEventListener('click', function (e) {
        const removeButton = e.target.closest('.remove-item-btn');
        if (removeButton) {
            if (confirm('Xóa sản phẩm này khỏi hóa đơn?')) {
                removeButton.closest('tr').remove();
                if (invoiceBody.querySelectorAll('tr[data-id]').length === 0) {
                    invoiceBody.innerHTML = '<tr class="text-center text-muted"><td colspan="6">Chưa có sản phẩm nào. Hãy thêm sản phẩm từ danh sách bên phải.</td></tr>';
                }
                updateInvoiceTotal();
            }
        }
    });

    function updateRowTotal(row) {
        const qtyInput = row.querySelector('.quantity-input');
        const price = parseFloat(qtyInput.getAttribute('data-price'));
        const quantity = parseInt(qtyInput.value);

        let rawTotal = price * quantity;

        const discountSelect = row.querySelector('.discount-select');
        let discountAmount = 0;

        let warningMsg = "";

        if (discountSelect && discountSelect.selectedIndex > 0) {
            const selectedOption = discountSelect.options[discountSelect.selectedIndex];

            const val = parseFloat(selectedOption.getAttribute('data-val')) || 0;
            const type = selectedOption.getAttribute('data-type');
            const minOrder = parseFloat(selectedOption.getAttribute('data-min')) || 0;
            const maxDiscount = parseFloat(selectedOption.getAttribute('data-max')) || 999999999;

            const today = new Date();
            const start = selectedOption.getAttribute('data-start');
            const end = selectedOption.getAttribute('data-end');
            if (start && end && (today < new Date(start) || today > new Date(end))) {
                warningMsg = "Mã đã hết hạn!";
            }
            else if (rawTotal < minOrder) {
                warningMsg = `Cần mua tối thiểu ${formatter.format(minOrder)}`;
            }
            else {
                if (type === "Phần trăm") {
                    discountAmount = rawTotal * val;
                    if (discountAmount > maxDiscount) discountAmount = maxDiscount;
                } else {
                    discountAmount = val;
                }
            }
        }

        if (discountAmount > rawTotal) discountAmount = rawTotal;

        let finalTotal = rawTotal - discountAmount;

        row.querySelector('.row-total').textContent = formatter.format(finalTotal);

        let errorSpan = row.querySelector('.discount-error-msg');
        if (!errorSpan) {
            errorSpan = document.createElement('small');
            errorSpan.className = 'discount-error-msg text-danger d-block mt-1';
            errorSpan.style.fontSize = '11px';
            if (discountSelect) discountSelect.parentNode.appendChild(errorSpan);
        }
        errorSpan.textContent = warningMsg;
        row.setAttribute('data-original-total', rawTotal);
        row.setAttribute('data-discount-amt', discountAmount);
        row.setAttribute('data-row-total', finalTotal);

        updateInvoiceTotal(); 
    }

    // --- TÍNH TỔNG CẢ HÓA ĐƠN ---
    function updateInvoiceTotal() {
        let totalOriginal = 0; // Tổng tiền hàng 
        let totalDiscount = 0; // Tổng tiền giảm giá

        const allRows = invoiceBody.querySelectorAll('tr[data-id]');

        allRows.forEach(row => {
            const rowOriginal = parseFloat(row.getAttribute('data-original-total')) || 0;
            const rowDiscount = parseFloat(row.getAttribute('data-discount-amt')) || 0;

            totalOriginal += rowOriginal;
            totalDiscount += rowDiscount;
        });

        const finalTotal = totalOriginal - totalDiscount;

        $('#sub-total').text(formatter.format(totalOriginal));

        $('#discount-amount').text(formatter.format(totalDiscount));

        $('#total-amount').text(formatter.format(finalTotal));
    }

    //// === 4. ÁP DỤNG MÃ GIẢM GIÁ ===
    //applyDiscountBtn.addEventListener('click', async function () {
    //    const code = discountInput.value.trim();
    //    if (!code) {
    //        showDiscountMessage('Vui lòng nhập mã giảm giá', 'danger');
    //        return;
    //    }
    //    const subTotal = calculateSubTotal();
    //    try {
    //        applyDiscountBtn.disabled = true;
    //        applyDiscountBtn.innerHTML = '<i class="spinner-border spinner-border-sm"></i>';
    //        const response = await fetch('/API/apply-discount', {
    //            method: 'POST',
    //            headers: { 'Content-Type': 'application/json' },
    //            body: JSON.stringify({ MaGiamGia: code, TongTien: subTotal })
    //        });
    //        const result = await response.json();
    //        if (response.ok) {
    //            appliedDiscount = result;
    //            discountValue = result.giaTri;
    //            updateInvoiceTotal();
    //            showDiscountMessage(`✓ Áp dụng thành công: ${result.ten} (-${formatter.format(result.giaTri)})`, 'success');
    //            discountInput.disabled = true;
    //            applyDiscountBtn.textContent = 'Đã áp dụng';
    //            applyDiscountBtn.classList.replace('btn-outline-primary', 'btn-success');
    //        } else {
    //            showDiscountMessage(result.message, 'danger');
    //            applyDiscountBtn.disabled = false;
    //            applyDiscountBtn.textContent = 'Áp dụng';
    //        }
    //    } catch (error) {
    //        showDiscountMessage('Lỗi khi áp dụng mã giảm giá', 'danger');
    //        console.error(error);
    //        applyDiscountBtn.disabled = false;
    //        applyDiscountBtn.textContent = 'Áp dụng';
    //    }
    //});

    function showDiscountMessage(message, type) {
        discountMessage.textContent = message;
        discountMessage.className = `text-${type}`;
        setTimeout(() => { discountMessage.textContent = ''; }, 5000);
    }

    function calculateSubTotal() {
        let subTotal = 0;
        const allRows = invoiceBody.querySelectorAll('tr[data-id]');
        allRows.forEach(row => {
            const qtyInput = row.querySelector('.quantity-input');
            if (qtyInput) {
                const price = parseFloat(qtyInput.getAttribute('data-price'));
                const quantity = parseInt(qtyInput.value);
                subTotal += price * quantity;
            }
        });
        return subTotal;
    }

    // === 5. LƯU TẠM HÓA ĐƠN (LƯU LOCAL) ===
    saveDraftBtn.addEventListener('click', async function () {
        const allRows = invoiceBody.querySelectorAll('tr[data-id]');
        if (allRows.length === 0) {
            alert('Chưa có sản phẩm nào trong hóa đơn!');
            return;
        }
        const draftData = {
            id: 'DRAFT_' + Date.now(),
            timestamp: new Date().toISOString(),
            customerId: customerSelect.val() || null,
            customerName: customerSelect.find(':selected').text() || 'Khách lẻ',
            discount: appliedDiscount,
            discountValue: discountValue,
            items: [],
            total: calculateSubTotal() - discountValue
        };
        allRows.forEach(row => {
            const qtyInput = row.querySelector('.quantity-input');
            const nameCell = row.cells[0];
            draftData.items.push({
                id: row.getAttribute('data-id'),
                name: nameCell.textContent,
                quantity: parseInt(qtyInput.value),
                price: parseFloat(qtyInput.getAttribute('data-price'))
            });
        });
        draftInvoices.push(draftData);
        localStorage.setItem('draftInvoices', JSON.stringify(draftInvoices));
        alert('Đã lưu hóa đơn tạm!');
        renderDraftList();
        clearInvoice();
    });

    // === 6. HIỂN THỊ DANH SÁCH HÓA ĐƠN TẠM LƯU ===
    function renderDraftList() {
        if (draftInvoices.length === 0) {
            draftList.innerHTML = '<li class="list-group-item text-center text-muted">Chưa có hóa đơn tạm lưu nào</li>';
            return;
        }
        draftList.innerHTML = '';
        draftInvoices.forEach((draft, index) => {
            const li = document.createElement('li');
            li.className = 'list-group-item d-flex justify-content-between align-items-start';
            li.innerHTML = `
                <div class="flex-grow-1" style="cursor: pointer;" data-draft-index="${index}">
                    <div class="fw-bold">${draft.customerName}</div>
                    <small class="text-muted">${new Date(draft.timestamp).toLocaleString('vi-VN')}</small>
                    <div><strong>${formatter.format(draft.total)}</strong></div>
                </div>
                <button class="btn btn-sm btn-danger remove-draft-btn" data-draft-index="${index}">
                    <i class="bi bi-trash"></i>
                </button>
            `;
            draftList.appendChild(li);
        });
    }

    // Click vào hóa đơn tạm để load
    draftList.addEventListener('click', function (e) {
        const draftEl = e.target.closest('[data-draft-index]');
        const removeBtn = e.target.closest('.remove-draft-btn');
        if (removeBtn) {
            e.stopPropagation();
            const index = parseInt(removeBtn.getAttribute('data-draft-index'));
            if (confirm('Xóa hóa đơn tạm này?')) {
                draftInvoices.splice(index, 1);
                localStorage.setItem('draftInvoices', JSON.stringify(draftInvoices));
                renderDraftList();
            }
        } else if (draftEl) {
            const index = parseInt(draftEl.getAttribute('data-draft-index'));

            loadDraftInvoice(draftInvoices[index]);

            draftInvoices.splice(index, 1);

            localStorage.setItem('draftInvoices', JSON.stringify(draftInvoices));

            renderDraftList();
        }
    });

    function loadDraftInvoice(draft) {
        clearInvoice();
        if (draft.customerId) {
            customerSelect.val(draft.customerId).trigger('change');
        }
        draft.items.forEach(item => {
            addProductToInvoice(item.id, item.name, item.price, item.quantity);
        });
        if (draft.discount) {
            appliedDiscount = draft.discount;
            discountValue = draft.discountValue;
            discountInput.value = draft.discount.code || '';
            discountInput.disabled = true;
            applyDiscountBtn.textContent = 'Đã áp dụng';
            applyDiscountBtn.disabled = true;
            applyDiscountBtn.classList.replace('btn-outline-primary', 'btn-success');
        }
        updateInvoiceTotal();
    }

    // === 7. THANH TOÁN NGAY ===
    //completeInvoiceBtn.addEventListener('click', async function () {
    //    const allRows = invoiceBody.querySelectorAll('tr[data-id]');
    //    if (allRows.length === 0) {
    //        alert('Chưa có sản phẩm nào trong hóa đơn!');
    //        return;
    //    }
    //    if (!confirm('Chuyển sang trang thanh toán?')) {
    //        return;
    //    }
    //    console.log('Bắt đầu lưu hóa đơn...');
    //    const hoaDonId = await saveInvoice('Chưa thanh toán');
    //    console.log('Kết quả saveInvoice:', hoaDonId);
    //    if (hoaDonId) {
    //        const redirectUrl = `/Admin/QuanLyHoaDon/ThanhToanHoaDon/${hoaDonId}`;
    //        console.log('Chuyển đến:', redirectUrl);
    //        window.location.href = redirectUrl;
    //    } else {
    //        console.error('Không nhận được hoaDonId từ API');
    //        alert('Không thể chuyển sang trang thanh toán. Vui lòng kiểm tra console.');
    //    }
    //});
    completeInvoiceBtn.addEventListener('click', async function () {
        // 1. Kiểm tra sản phẩm
        const allRows = invoiceBody.querySelectorAll('tr[data-id]');
        if (allRows.length === 0) {
            alert('Chưa có sản phẩm nào trong hóa đơn!');
            return;
        }

        // 2. Xác nhận
        if (!confirm('Xác nhận tạo hóa đơn và chuyển sang thanh toán?')) {
            return;
        }

        // 3. Hiệu ứng loading nút bấm
        const originalText = completeInvoiceBtn.innerHTML;
        completeInvoiceBtn.disabled = true;
        completeInvoiceBtn.innerHTML = '<i class="spinner-border spinner-border-sm me-2"></i>Đang xử lý...';

        try {
            console.log('--- BẮT ĐẦU THANH TOÁN ---');

            const hoaDonId = await saveInvoice('Chưa thanh toán');

            console.log('ID Hóa đơn nhận được:', hoaDonId);

            if (hoaDonId) {
                window.location.href = `/Admin/QuanLyHoaDon/ThanhToanHoaDon/${hoaDonId}`;
            } else {
                console.error('Lưu thất bại.');
            }
        } catch (e) {
            console.error('Lỗi JS:', e);
            alert('Có lỗi xảy ra: ' + e.message);
        } finally {
            // Mở lại nút bấm
            completeInvoiceBtn.disabled = false;
            completeInvoiceBtn.innerHTML = originalText;
        }
    });

    // === 8. HỦY HÓA ĐƠN ===
    cancelInvoiceBtn.addEventListener('click', function () {
        if (confirm('Bạn có chắc muốn hủy hóa đơn này? Mọi dữ liệu sẽ bị mất.')) {
            clearInvoice();
        }
    });
    function clearInvoice() {
        invoiceBody.innerHTML = '<tr class="empty-row text-center text-muted"><td colspan="6">Chưa có sản phẩm nào. Hãy thêm sản phẩm từ danh sách bên phải.</td></tr>';

        if (customerSelect && customerSelect.length) {
            customerSelect.val('').trigger('change');
        }

        if (discountInput) {
            discountInput.value = '';
            discountInput.disabled = false;
        }

        if (applyDiscountBtn) {
            applyDiscountBtn.textContent = 'Áp dụng';
            applyDiscountBtn.disabled = false;
            applyDiscountBtn.classList.replace('btn-success', 'btn-outline-primary');
        }

        if (discountMessage) {
            discountMessage.textContent = '';
        }

        appliedDiscount = null;
        discountValue = 0;

        updateInvoiceTotal();
    }


    // === 9. LƯU HÓA ĐƠN VÀO DATABASE (GỌI API) ===
    async function saveInvoice(trangThai) {
        console.log('Trạng thái:', trangThai);
        const allRows = invoiceBody.querySelectorAll('tr[data-id]');
        console.log('Số sản phẩm:', allRows.length);
        
        const chiTietHoaDon = [];
        
        allRows.forEach(row => {
            const qtyInput = row.querySelector('.quantity-input');
            const discountSelect = row.querySelector('.discount-select');
            
            let giamGia = parseFloat(row.getAttribute('data-discount-amt')) || 0;
            let maKhuyenMaiId = null;
            
            if (discountSelect && discountSelect.value) {
                maKhuyenMaiId = discountSelect.value;
            }
            
            const item = {
                SanPhamDonViId: row.getAttribute('data-id'),    
                SoLuong: parseInt(qtyInput.value),
                DonGia: parseFloat(qtyInput.getAttribute('data-price')),
                GiamGia: giamGia,
                MaKhuyenMaiId: maKhuyenMaiId
            };
            chiTietHoaDon.push(item);
            
            console.log('Chi tiết sản phẩm:', item);
        });     
        
        const requestData = {
            KhachHangId: customerSelect.val() || null,
            NgayLap: new Date().toISOString(),
            TrangThai: trangThai,
            ChiTietHoaDon: chiTietHoaDon
        };
        
        console.log('Request data:', requestData);
        
        try {
            saveDraftBtn.disabled = true;
            completeInvoiceBtn.disabled = true;
            completeInvoiceBtn.innerHTML = '<i class="spinner-border spinner-border-sm me-2"></i>Đang xử lý...';
            
            const response = await fetch('/API/add-HoaDon', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(requestData)
            });
            
            const result = await response.json();
            
            if (response.ok) {
                console.log(' Hóa đơn đã tạo thành công:', result.hoaDonId);
                console.log('Tổng tiền:', result.tongTien);
                console.log('Tổng giảm giá:', result.tongGiamGia);
                return result.hoaDonId;
            } else {
                console.error('❌ Lỗi từ server:', result.message);
                alert('Lỗi: ' + result.message);
                return null;
            }
        } catch (error) {
            console.error('❌ Exception khi lưu hóa đơn:', error);
            alert('Lỗi khi lưu hóa đơn: ' + error.message);
            return null;
        } finally {
            saveDraftBtn.disabled = false;
            completeInvoiceBtn.disabled = false;
            completeInvoiceBtn.innerHTML = '<i class="bi bi-wallet me-2"></i>Thanh toán ngay';
        }
    }

    // === KHỞI TẠO ===
    renderDraftList();
    updateInvoiceTotal();
    console.log('=== READY ===');





    // CHỨC NĂNG QUÉT MÃ VẠCH (LIÊN TỤC)
    let html5QrCode = null;
    let isScanningProcessing = false;

    // 1. Bật Camera
    $('#btn-scan-barcode').click(function () {
        $('#scanner-container').slideDown();

        if (html5QrCode === null) {
            html5QrCode = new Html5Qrcode("reader");
        }

        const config = {
            fps: 10,
            qrbox: { width: 250, height: 150 },
            formatsToSupport: [
                Html5QrcodeSupportedFormats.EAN_13,
                Html5QrcodeSupportedFormats.EAN_8,
                Html5QrcodeSupportedFormats.CODE_128,
                Html5QrcodeSupportedFormats.QR_CODE
            ]
        };

        html5QrCode.start(
            { facingMode: "environment" },
            config,
            onScanSuccess,
            (errorMessage) => { /* Bỏ qua lỗi đang quét */ }
        ).catch(err => {
            console.error("Lỗi camera:", err);
            alert("Lỗi mở Camera: " + err);
            $('#scanner-container').hide();
        });
    });

    // 2. Tắt Camera (Chỉ tắt khi ấn nút X)
    $('#btn-close-camera').click(function () {
        stopScanner();
    });

    // 3. Xử lý khi quét thành công
    function onScanSuccess(decodedText, decodedResult) {
        // Nếu đang xử lý mã trước đó thì bỏ qua (Chống spam)
        if (isScanningProcessing) return;

        // Khóa quét tạm thời
        isScanningProcessing = true;
        console.log("Quét được:", decodedText);

        // A. Thay thế mã cũ bằng mã mới trong ô tìm kiếm
        searchInput.value = decodedText;

        // B. Lọc danh sách ngay lập tức để hiển thị sản phẩm tương ứng
        filterProducts();

        // C. Tự động thêm sản phẩm đầu tiên tìm thấy
        const firstVisibleItem = productList.querySelector('.product-item:not(.d-none)');

        if (firstVisibleItem) {
            const addButton = firstVisibleItem.querySelector('.add-product-btn');
            if (addButton) {
                const id = addButton.getAttribute('data-id');
                const name = addButton.getAttribute('data-name');
                const price = parseFloat(addButton.getAttribute('data-price'));

                // Thêm vào hóa đơn
                addProductToInvoice(id, name, price);

                addButton.classList.add('btn-success');
                setTimeout(() => addButton.classList.remove('btn-success'), 200);

                 new Audio('https://actions.google.com/sounds/v1/alarms/beep_short.ogg').play();
            }
        } else {
            // Dùng Toast hoặc ghi log nhỏ thay vì Alert để không chặn màn hình quét
            console.warn('Không tìm thấy sản phẩm: ' + decodedText);
        }

        // D. Mở khóa quét sau 2 giây (Để nhân viên kịp đưa sản phẩm khác vào)
        setTimeout(() => {
            isScanningProcessing = false;
        }, 2000);
    }

    function stopScanner() {
        if (html5QrCode && html5QrCode.isScanning) {
            html5QrCode.stop().then(() => {
                $('#scanner-container').slideUp();
                html5QrCode.clear();
                isScanningProcessing = false; // Reset cờ khi tắt
            }).catch(err => console.log("Lỗi tắt cam:", err));
        } else {
            $('#scanner-container').slideUp();
        }
    }
});