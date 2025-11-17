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
    const currentUserId = document.getElementById('current-user-id')?.value || 'NV001';

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

    // === 1. LỌC SẢN PHẨM (TỐI ƯU) ===
    function filterProducts() {
        // Lấy giá trị từ ô tìm kiếm và dropdown
        const searchText = searchInput.value.toLowerCase().trim();
        const category = categorySelect.value; // Dùng .value vẫn đúng
        const allProducts = productList.querySelectorAll('.product-item');

        let visibleCount = 0;

        allProducts.forEach(product => {
            const productName = product.getAttribute('data-product-name')?.toLowerCase() || '';
            const productId = product.getAttribute('data-product-id')?.toLowerCase() || '';
            const productCategory = product.getAttribute('data-category') || '';

            // 1. Kiểm tra tìm kiếm
            const matchesSearch = !searchText ||
                productName.includes(searchText) ||
                productId.includes(searchText);

            // 2. Kiểm tra danh mục
            const matchesCategory = !category || productCategory === category;

            // 3. Ẩn/Hiện
            if (matchesSearch && matchesCategory) {
                product.style.display = ''; // Dùng '' thay vì 'flex'
                visibleCount++;
            } else {
                product.style.display = 'none';
            }
        });

        // Cập nhật thông báo "Không tìm thấy"
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

    // === GẮN SỰ KIỆN LỌC ===
    const debouncedFilter = debounce(filterProducts, 300);
    searchInput.addEventListener('input', debouncedFilter);

    // **SỬA LỖI Ở ĐÂY:** Dùng cú pháp jQuery .on() cho Select2
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
        const emptyRow = invoiceBody.querySelector('tr td[colspan="5"]');
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
                <td><input type="number" class="form-control form-control-sm quantity-input" value="${quantity}" min="1" data-price="${price}"></td>
                <td>${formatter.format(price)}</td>
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

    invoiceBody.addEventListener('click', function (e) {
        const removeButton = e.target.closest('.remove-item-btn');
        if (removeButton) {
            if (confirm('Xóa sản phẩm này khỏi hóa đơn?')) {
                removeButton.closest('tr').remove();
                if (invoiceBody.querySelectorAll('tr[data-id]').length === 0) {
                    invoiceBody.innerHTML = '<tr class="text-center text-muted"><td colspan="5">Chưa có sản phẩm nào. Hãy thêm sản phẩm từ danh sách bên phải.</td></tr>';
                }
                updateInvoiceTotal();
            }
        }
    });

    function updateRowTotal(row) {
        const qtyInput = row.querySelector('.quantity-input');
        const price = parseFloat(qtyInput.getAttribute('data-price'));
        const quantity = parseInt(qtyInput.value);
        const total = price * quantity;
        row.querySelector('.row-total').textContent = formatter.format(total);
    }

    function updateInvoiceTotal() {
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
        const total = subTotal - discountValue;
        subTotalEl.textContent = formatter.format(subTotal);
        discountEl.textContent = formatter.format(discountValue);
        totalEl.textContent = formatter.format(total);
    }

    // === 4. ÁP DỤNG MÃ GIẢM GIÁ ===
    applyDiscountBtn.addEventListener('click', async function () {
        const code = discountInput.value.trim();
        if (!code) {
            showDiscountMessage('Vui lòng nhập mã giảm giá', 'danger');
            return;
        }
        const subTotal = calculateSubTotal();
        try {
            applyDiscountBtn.disabled = true;
            applyDiscountBtn.innerHTML = '<i class="spinner-border spinner-border-sm"></i>';
            const response = await fetch('/API/apply-discount', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ MaGiamGia: code, TongTien: subTotal })
            });
            const result = await response.json();
            if (response.ok) {
                appliedDiscount = result;
                discountValue = result.giaTri;
                updateInvoiceTotal();
                showDiscountMessage(`✓ Áp dụng thành công: ${result.ten} (-${formatter.format(result.giaTri)})`, 'success');
                discountInput.disabled = true;
                applyDiscountBtn.textContent = 'Đã áp dụng';
                applyDiscountBtn.classList.replace('btn-outline-primary', 'btn-success');
            } else {
                showDiscountMessage(result.message, 'danger');
                applyDiscountBtn.disabled = false;
                applyDiscountBtn.textContent = 'Áp dụng';
            }
        } catch (error) {
            showDiscountMessage('Lỗi khi áp dụng mã giảm giá', 'danger');
            console.error(error);
            applyDiscountBtn.disabled = false;
            applyDiscountBtn.textContent = 'Áp dụng';
        }
    });

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
    completeInvoiceBtn.addEventListener('click', async function () {
        const allRows = invoiceBody.querySelectorAll('tr[data-id]');
        if (allRows.length === 0) {
            alert('Chưa có sản phẩm nào trong hóa đơn!');
            return;
        }
        if (!confirm('Chuyển sang trang thanh toán?')) {
            return;
        }
        console.log('Bắt đầu lưu hóa đơn...');
        const hoaDonId = await saveInvoice('Chưa thanh toán');
        console.log('Kết quả saveInvoice:', hoaDonId);
        if (hoaDonId) {
            const redirectUrl = `/Admin/QuanLyHoaDon/ThanhToanHoaDon/${hoaDonId}`;
            console.log('Chuyển đến:', redirectUrl);
            window.location.href = redirectUrl;
        } else {
            console.error('Không nhận được hoaDonId từ API');
            alert('Không thể chuyển sang trang thanh toán. Vui lòng kiểm tra console.');
        }
    });

    // === 8. HỦY HÓA ĐƠN ===
    cancelInvoiceBtn.addEventListener('click', function () {
        if (confirm('Bạn có chắc muốn hủy hóa đơn này? Mọi dữ liệu sẽ bị mất.')) {
            clearInvoice();
        }
    });

    function clearInvoice() {
        invoiceBody.innerHTML = '<tr class="text-center text-muted"><td colspan="5">Chưa có sản phẩm nào. Hãy thêm sản phẩm từ danh sách bên phải.</td></tr>';
        customerSelect.val('').trigger('change');
        discountInput.value = '';
        discountInput.disabled = false;
        applyDiscountBtn.textContent = 'Áp dụng';
        applyDiscountBtn.disabled = false;
        applyDiscountBtn.classList.replace('btn-success', 'btn-outline-primary');
        appliedDiscount = null;
        discountValue = 0;
        discountMessage.textContent = '';
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
            const item = {
                SanPhamDonViId: row.getAttribute('data-id'),
                SoLuong: parseInt(qtyInput.value),
                DonGia: parseFloat(qtyInput.getAttribute('data-price')),
                GiamGia: 0
            };
            chiTietHoaDon.push(item);
        });
        const requestData = {
            KhachHangId: customerSelect.val() || null,
            NhanVienId: currentUserId,
            NgayLap: new Date().toISOString(),
            TrangThai: trangThai,
            MaKhuyenMaiId: appliedDiscount?.id || null,
            TongGiamGia: discountValue,
            ChiTietHoaDon: chiTietHoaDon
        };
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
                return result.hoaDonId;
            } else {
                console.error(' Lỗi từ server:', result.message);
                alert('Lỗi: ' + result.message);
                return null;
            }
        } catch (error) {
            console.error(' Exception khi lưu hóa đơn:', error);
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
});