document.addEventListener('DOMContentLoaded', function () {
    console.log('=== POS SYSTEM LOADED ===');

    // 1. KHỞI TẠO CÁC THƯ VIỆN
    $('#selectKhachHang').select2({ width: '100%', placeholder: 'Chọn khách hàng', allowClear: true });
    $('#selectDanhMuc').select2({ width: '100%', placeholder: 'Tất cả danh mục', allowClear: true });

    // 2. BIẾN TOÀN CỤC
    let appliedDiscount = null;
    let discountValue = 0;
    let draftInvoices = JSON.parse(localStorage.getItem('draftInvoices') || '[]');

    // 3. CÁC HÀM TIỆN ÍCH
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
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, "");
        str = str.replace(/\u02C6|\u0306|\u031B/g, "");
        return str.trim().toLowerCase();
    }

    const formatter = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' });

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

    // 4. DOM ELEMENTS
    const productList = document.getElementById('product-list');
    const invoiceBody = document.getElementById('invoice-items-body');
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

    // 5. RENDER OPTION KHUYẾN MÃI
    function renderKhuyenMaiOptions() {
        if (typeof danhSachKhuyenMai === 'undefined') return '<option value="" data-val="0">-- Chọn KM --</option>';
        let options = '<option value="" data-val="0">-- Chọn KM --</option>';
        danhSachKhuyenMai.forEach(km => {
            let hienThiGiam = (km.loaiGiam === "Phần trăm") ? (km.giaTri) + "%" : formatter.format(km.giaTri); // Fix hiển thị %
            options += `<option value="${km.id}" data-val="${km.giaTri}" data-type="${km.loaiGiam}" data-min="${km.toithieu}" data-max="${km.toida}" data-start="${km.ngayBatDau}" data-end="${km.ngayKetThuc}">${km.ma} - Giảm ${hienThiGiam}</option>`;
        });
        return options;
    }

    // 6. LỌC SẢN PHẨM (HỖ TRỢ TÌM TRONG CHUỖI NHIỀU MÃ VẠCH)
    function filterProducts() {
        const rawKeyword = searchInput.value;
        const searchText = rawKeyword ? removeVietnameseTones(rawKeyword) : "";
        const category = categorySelect.value;
        const allProducts = productList.querySelectorAll('.product-item');
        let visibleCount = 0;

        allProducts.forEach(product => {
            const rawName = product.getAttribute('data-product-name') || '';
            const rawId = product.getAttribute('data-product-id') || '';
            const productCategory = product.getAttribute('data-category') || '';
            // Lấy chuỗi mã vạch (đã nối ở View)
            const productBarcodeId = product.getAttribute('data-product-barcodeId') || '';

            const nameNormalized = removeVietnameseTones(rawName);
            const idNormalized = removeVietnameseTones(rawId);
            const barcodeNormalized = removeVietnameseTones(productBarcodeId);

            const matchesSearch = !searchText ||
                nameNormalized.includes(searchText) ||
                idNormalized.includes(searchText) ||
                barcodeNormalized.includes(searchText); // Tìm trong chuỗi mã vạch

            const matchesCategory = !category || productCategory === category;

            if (matchesSearch && matchesCategory) {
                product.classList.remove('d-none');
                product.classList.add('d-flex');
                visibleCount++;
            } else {
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
        } else if (messageEl) {
            messageEl.remove();
        }
    }

    const debouncedFilter = debounce(filterProducts, 300);
    searchInput.addEventListener('input', debouncedFilter);
    $('#selectDanhMuc').on('change', filterProducts);

    // 7. SỰ KIỆN CLICK THÊM SẢN PHẨM (THỦ CÔNG)
    productList.addEventListener('click', function (e) {
        const addButton = e.target.closest('.add-product-btn');
        if (addButton) {
            e.preventDefault();
            const id = addButton.getAttribute('data-id');
            const name = addButton.getAttribute('data-name');
            const price = parseFloat(addButton.getAttribute('data-price'));

            // Thêm thủ công thì không có scannedCode
            addProductToInvoice(id, name, price, 1, null);

            addButton.classList.add('btn-success');
            setTimeout(() => addButton.classList.remove('btn-success'), 200);
        }
    });

    // 8. HÀM THÊM VÀO HÓA ĐƠN (CÓ XỬ LÝ MÃ VẠCH VỪA QUÉT)
    function addProductToInvoice(id, name, price, quantity = 1, scannedCode = null) {
        const emptyRow = invoiceBody.querySelector('tr td[colspan="6"]');
        if (emptyRow) emptyRow.closest('tr').remove();

        const existingRow = document.querySelector(`#invoice-items-body tr[data-id="${id}"]`);

        if (existingRow) {
            // Nếu đã có dòng này -> Tăng số lượng
            const qtyInput = existingRow.querySelector('.quantity-input');
            qtyInput.value = parseInt(qtyInput.value) + quantity;

            // Cập nhật hiển thị mã vừa quét (nếu có)
            if (scannedCode) {
                const codeDisplay = existingRow.querySelector('.scanned-code-display');
                if (codeDisplay) codeDisplay.textContent = `Mã: ${scannedCode}`;
                existingRow.setAttribute('data-scanned-code', scannedCode);
            }

            updateRowTotal(existingRow);
            existingRow.classList.add('table-success');
            setTimeout(() => existingRow.classList.remove('table-success'), 500);
        } else {
            // Nếu chưa có -> Thêm dòng mới
            const row = invoiceBody.insertRow();
            row.setAttribute('data-id', id);
            // Lưu mã quét vào attribute
            row.setAttribute('data-scanned-code', scannedCode || '');

            row.innerHTML = `
                <td>
                    <div class="fw-bold">${name}</div>
                    <small class="text-muted fst-italic scanned-code-display" style="font-size: 0.8rem;">
                        ${scannedCode ? 'Mã: ' + scannedCode : ''}
                    </small>
                </td>
                <td>
                    <input type="number" class="form-control form-control-sm quantity-input" value="${quantity}" min="1" data-price="${price}">
                </td>
                <td>${formatter.format(price)}</td>
                <td>
                    <select class="form-control form-control-sm discount-select" style="width: 100%;">
                        ${renderKhuyenMaiOptions()}
                    </select>
                </td>
                <td class="row-total">${formatter.format(price * quantity)}</td>
                <td class="text-center">
                    <button class="btn btn-danger btn-sm remove-item-btn"><i class="bi bi-trash"></i></button>
                </td>
            `;
            // GỌI NGAY ĐỂ TÍNH TIỀN LÚC MỚI THÊM
            updateRowTotal(row);
        }
        updateInvoiceTotal();
    }

    // Sự kiện thay đổi số lượng
    invoiceBody.addEventListener('input', function (e) {
        const qtyInput = e.target.closest('.quantity-input');
        if (qtyInput) {
            if (qtyInput.value < 1) qtyInput.value = 1;
            updateRowTotal(qtyInput.closest('tr'));
            updateInvoiceTotal();
        }
    });

    // Sự kiện chọn mã giảm giá trên dòng
    invoiceBody.addEventListener('change', function (e) {
        if (e.target.classList.contains('discount-select')) {
            updateRowTotal(e.target.closest('tr'));
            updateInvoiceTotal();
        }
    });

    // Sự kiện xóa dòng
    invoiceBody.addEventListener('click', function (e) {
        const removeButton = e.target.closest('.remove-item-btn');
        if (removeButton && confirm('Xóa sản phẩm này khỏi hóa đơn?')) {
            removeButton.closest('tr').remove();
            if (invoiceBody.querySelectorAll('tr[data-id]').length === 0) {
                invoiceBody.innerHTML = '<tr class="text-center text-muted"><td colspan="6">Chưa có sản phẩm nào. Hãy thêm sản phẩm từ danh sách bên phải.</td></tr>';
            }
            updateInvoiceTotal();
        }
    });

    // 9. TÍNH TOÁN TIỀN TỪNG DÒNG
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
                warningMsg = "Mã hết hạn";
            } else if (rawTotal < minOrder) {
                warningMsg = `Mua tối thiểu ${formatter.format(minOrder)}`;
            } else {
                if (type === "Phần trăm") {
                    discountAmount = rawTotal * (val/100);
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
            errorSpan = document.createElement('div');
            errorSpan.className = 'discount-error-msg text-danger small fst-italic';
            if (discountSelect) discountSelect.parentNode.appendChild(errorSpan);
        }
        errorSpan.textContent = warningMsg;

        row.setAttribute('data-original-total', rawTotal);
        row.setAttribute('data-discount-amt', discountAmount);
        row.setAttribute('data-row-total', finalTotal);
    }

    // 10. TÍNH TỔNG HÓA ĐƠN
    function updateInvoiceTotal() {
        let totalOriginal = 0;
        let totalDiscount = 0;
        invoiceBody.querySelectorAll('tr[data-id]').forEach(row => {
            totalOriginal += parseFloat(row.getAttribute('data-original-total')) || 0;
            totalDiscount += parseFloat(row.getAttribute('data-discount-amt')) || 0;
        });
        const finalTotal = totalOriginal - totalDiscount;

        $('#sub-total').text(formatter.format(totalOriginal));
        $('#discount-amount').text(formatter.format(totalDiscount));
        $('#total-amount').text(formatter.format(finalTotal));
    }

    function calculateSubTotal() {
        let subTotal = 0;
        invoiceBody.querySelectorAll('tr[data-id]').forEach(row => {
            const qty = row.querySelector('.quantity-input');
            if (qty) subTotal += parseFloat(qty.getAttribute('data-price')) * parseInt(qty.value);
        });
        return subTotal;
    }

    // 11. XỬ LÝ LƯU TẠM (DRAFT) - Logic lưu trước khi tải mới
    function saveCurrentToDraftList() {
        const allRows = invoiceBody.querySelectorAll('tr[data-id]');
        if (allRows.length === 0) return false;

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
            const nameDiv = row.cells[0].querySelector('.fw-bold'); // Lấy đúng thẻ tên
            const scannedCode = row.getAttribute('data-scanned-code');

            draftData.items.push({
                id: row.getAttribute('data-id'),
                name: nameDiv ? nameDiv.textContent : row.cells[0].textContent,
                quantity: parseInt(qtyInput.value),
                price: parseFloat(qtyInput.getAttribute('data-price')),
                scannedCode: scannedCode // Lưu cả mã quét vào draft
            });
        });

        draftInvoices.push(draftData);
        localStorage.setItem('draftInvoices', JSON.stringify(draftInvoices));
        return true;
    }

    saveDraftBtn.addEventListener('click', function () {
        if (saveCurrentToDraftList()) {
            alert('Đã lưu hóa đơn tạm!');
            renderDraftList();
            clearInvoice();
        } else {
            alert('Chưa có sản phẩm nào để lưu!');
        }
    });

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
                <button class="btn btn-sm btn-danger remove-draft-btn" data-draft-index="${index}"><i class="bi bi-trash"></i></button>
            `;
            draftList.appendChild(li);
        });
    }

    draftList.addEventListener('click', function (e) {
        const draftEl = e.target.closest('[data-draft-index]');
        const removeBtn = e.target.closest('.remove-draft-btn');

        if (removeBtn) {
            e.stopPropagation();
            if (confirm('Xóa hóa đơn tạm này?')) {
                const index = parseInt(removeBtn.getAttribute('data-draft-index'));
                draftInvoices.splice(index, 1);
                localStorage.setItem('draftInvoices', JSON.stringify(draftInvoices));
                renderDraftList();
            }
        } else if (draftEl) {
            const index = parseInt(draftEl.getAttribute('data-draft-index'));
            const selectedDraft = draftInvoices[index];

            // Lưu hóa đơn hiện tại trước khi load cái mới
            saveCurrentToDraftList();

            loadDraftInvoice(selectedDraft);

            // Xóa cái vừa load khỏi danh sách
            draftInvoices.splice(index, 1);
            localStorage.setItem('draftInvoices', JSON.stringify(draftInvoices));
            renderDraftList();
        }
    });

    function loadDraftInvoice(draft) {
        clearInvoice();
        if (draft.customerId) customerSelect.val(draft.customerId).trigger('change');

        draft.items.forEach(item => {
            // Load lại sản phẩm kèm theo mã đã quét
            addProductToInvoice(item.id, item.name, item.price, item.quantity, item.scannedCode);
        });

        if (draft.discount) {
            appliedDiscount = draft.discount;
            discountValue = draft.discountValue;
            if (discountInput) discountInput.value = draft.discount.code || '';
        }
        updateInvoiceTotal();
    }

    // 12. THANH TOÁN (GỌI API)
    completeInvoiceBtn.addEventListener('click', async function () {
        const allRows = invoiceBody.querySelectorAll('tr[data-id]');
        if (allRows.length === 0) return alert('Chưa có sản phẩm nào!');
        if (!confirm('Xác nhận tạo hóa đơn và thanh toán?')) return;

        const originalText = completeInvoiceBtn.innerHTML;
        completeInvoiceBtn.disabled = true;
        completeInvoiceBtn.innerHTML = '<i class="spinner-border spinner-border-sm me-2"></i>Đang xử lý...';

        try {
            const hoaDonId = await saveInvoice('Chưa thanh toán');
            if (hoaDonId) window.location.href = `/Admin/QuanLyHoaDon/ThanhToanHoaDon/${hoaDonId}`;
        } catch (e) {
            console.error(e);
            alert('Lỗi: ' + e.message);
        } finally {
            completeInvoiceBtn.disabled = false;
            completeInvoiceBtn.innerHTML = originalText;
        }
    });

    async function saveInvoice(trangThai) {
        const chiTietHoaDon = [];
        invoiceBody.querySelectorAll('tr[data-id]').forEach(row => {
            const qtyInput = row.querySelector('.quantity-input');
            const discountSelect = row.querySelector('.discount-select');

            // Lấy mã vạch vừa quét để gửi lên server
            const maVachDaQuet = row.getAttribute('data-scanned-code');

            chiTietHoaDon.push({
                SanPhamDonViId: row.getAttribute('data-id'),
                SoLuong: parseInt(qtyInput.value),
                DonGia: parseFloat(qtyInput.getAttribute('data-price')),
                GiamGia: parseFloat(row.getAttribute('data-discount-amt')) || 0,
                MaKhuyenMaiId: (discountSelect && discountSelect.value) ? discountSelect.value : null,
                // Gửi mã vạch vào cột GhiChu
                GhiChu: maVachDaQuet ? `Scan: ${maVachDaQuet}` : ""
            });
        });

        const requestData = {
            KhachHangId: customerSelect.val() || null,
            NgayLap: new Date().toISOString(),
            TrangThai: trangThai,
            ChiTietHoaDon: chiTietHoaDon
        };

        const response = await fetch('/API/add-HoaDon', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(requestData)
        });
        const result = await response.json();
        if (!response.ok) throw new Error(result.message);
        return result.hoaDonId;
    }

    cancelInvoiceBtn.addEventListener('click', function () {
        if (confirm('Hủy hóa đơn hiện tại?')) clearInvoice();
    });

    function clearInvoice() {
        invoiceBody.innerHTML = '<tr class="text-center text-muted"><td colspan="6">Chưa có sản phẩm nào. Hãy thêm sản phẩm từ danh sách bên phải.</td></tr>';
        customerSelect.val('').trigger('change');
        appliedDiscount = null;
        discountValue = 0;
        updateInvoiceTotal();
    }

    renderDraftList();
    updateInvoiceTotal();

    // ==========================================
    // 13. XỬ LÝ CAMERA (SCAN BARCODE)
    // ==========================================
    let html5QrCode = null;
    let isScanningProcessing = false;

    function startScanning(cameraId) {
        if (html5QrCode.isScanning) {
            html5QrCode.stop().then(() => startScanning(cameraId)).catch(err => console.error(err));
            return;
        }
        html5QrCode.start(
            cameraId,
            { fps: 10, qrbox: { width: 450, height: 250 } },
            onScanSuccess,
            () => { }
        ).catch(err => alert("Lỗi mở Camera: " + err));
    }

    $('#btn-scan-barcode').click(function () {
        $('#scanner-container').slideDown();
        if (!html5QrCode) html5QrCode = new Html5Qrcode("reader");

        Html5Qrcode.getCameras().then(devices => {
            const cameraSelect = $('#camera-select');
            cameraSelect.empty();
            if (devices && devices.length) {
                devices.forEach((device, i) => {
                    cameraSelect.append($('<option>').val(device.id).text(device.label || `Camera ${i + 1}`));
                });
                cameraSelect.show();
                startScanning(devices[0].id);
            } else {
                alert("Không tìm thấy camera!");
            }
        }).catch(err => alert("Lỗi camera: " + err));
    });

    $('#camera-select').change(function () {
        if (html5QrCode) startScanning($(this).val());
    });

    $('#btn-close-camera').click(function () {
        if (html5QrCode && html5QrCode.isScanning) {
            html5QrCode.stop().then(() => {
                $('#scanner-container').slideUp();
                html5QrCode.clear();
            });
        } else {
            $('#scanner-container').slideUp();
        }
    });

    function onScanSuccess(decodedText, decodedResult) {
        if (isScanningProcessing) return;
        isScanningProcessing = true;

        searchInput.value = decodedText;
        filterProducts(); // Lọc danh sách theo mã quét

        // Tự động chọn sản phẩm đầu tiên tìm thấy
        const firstVisibleItem = productList.querySelector('.product-item:not(.d-none)');
        if (firstVisibleItem) {
            const addButton = firstVisibleItem.querySelector('.add-product-btn');
            if (addButton) {
                const id = addButton.getAttribute('data-id');
                const name = addButton.getAttribute('data-name');
                const price = parseFloat(addButton.getAttribute('data-price'));

                // QUAN TRỌNG: Truyền mã vừa quét vào hàm thêm
                addProductToInvoice(id, name, price, 1, decodedText);

                addButton.classList.add('btn-success');
                setTimeout(() => addButton.classList.remove('btn-success'), 200);
                new Audio('https://actions.google.com/sounds/v1/alarms/beep_short.ogg').play();
            }
        } else {
            console.warn("Không tìm thấy sản phẩm:", decodedText);
        }

        setTimeout(() => isScanningProcessing = false, 2000); // Delay 2s tránh double scan
    }
});