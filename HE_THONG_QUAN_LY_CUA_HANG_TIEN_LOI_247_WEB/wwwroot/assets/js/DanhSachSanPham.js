$(function () {
    console.log('=== DANH SÁCH SẢN PHẨM PAGE LOADED ===');

    // --- 1. KHỞI TẠO SELECT2 ---
    $('#form-select-hang-hoa').select2({ width: '100%', dropdownParent: $('#product-form-col') });
    $('#form-select-don-vi').select2({ width: '100%', dropdownParent: $('#product-form-col') });

    // --- 2. TRUY XUẤT PHẦN TỬ DOM ---
    const mainRow = $('#main-row-container');
    const showFormBtn = $('#show-add-form-btn');
    const productForm = $('#product-form');
    const formTitle = $('#form-title');

    const formImagesUpload = $('#form-images-upload');
    const uploadPlaceholder = $('#upload-placeholder');
    const imagesPreviewContainer = $('#images-preview-container');

    // Các trường input
    const selectHangHoa = $('#form-select-hang-hoa');
    const selectDonVi = $('#form-select-don-vi');
    const inputConversionFactor = $('#form-conversion-factor');
    const inputPrice = $('#form-product-price');
    const inputDescription = $('#form-product-description');

    // Nút bấm
    const btnSubmit = $('#btn-submit');
    const btnCancel = $('#btn-cancel');
    const btnEditMode = $('#btn-edit-mode');

    // --- 3. LOGIC XỬ LÝ NHIỀU ẢNH ---
    let dt = new DataTransfer();
    let currentMode = null; // 'add', 'edit', 'view'
    let currentSanPhamId = null;
    let currentDonViId = null;

    // Hàm vẽ lại vùng preview ảnh
    function renderPreview() {
        imagesPreviewContainer.empty();

        if (dt.files.length === 0) {
            uploadPlaceholder.show();
            imagesPreviewContainer.hide();
        } else {
            uploadPlaceholder.hide();
            imagesPreviewContainer.show().css('display', 'flex');

            Array.from(dt.files).forEach(file => {
                const wrapper = $('<div>').addClass('preview-image-wrapper');
                const img = $('<img>').addClass('preview-image-item');
                const reader = new FileReader();
                reader.onload = (e) => img.attr('src', e.target.result);
                reader.readAsDataURL(file);

                const removeBtn = $('<span>').addClass('btn-remove-image')
                    .html('<i class="fas fa-times"></i>')
                    .data('file-name', file.name);

                removeBtn.on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    removeFile($(this).data('file-name'));
                });

                wrapper.append(img).append(removeBtn);
                imagesPreviewContainer.append(wrapper);
            });

            if (!productForm.hasClass('form-readonly')) {
                const addBtn = $('<div>').addClass('mini-add-btn')
                    .html('<i class="fas fa-plus"></i><span>Thêm ảnh</span>');

                addBtn.on('click', function () {
                    formImagesUpload.click();
                });

                imagesPreviewContainer.append(addBtn);
            }
        }
    }

    function removeFile(fileName) {
        const newDt = new DataTransfer();
        Array.from(dt.files).forEach(file => {
            if (file.name !== fileName) newDt.items.add(file);
        });
        dt = newDt;
        formImagesUpload[0].files = dt.files;
        renderPreview();
    }

    formImagesUpload.on('change', function () {
        Array.from(this.files).forEach(file => {
            if (file.type.startsWith('image/')) dt.items.add(file);
        });
        this.files = dt.files;
        renderPreview();
    });

    // --- 4. HÀM MỞ FORM ---
    function openForm(mode, sanPhamId = null, donViId = null) {
        console.log('openForm:', mode, sanPhamId, donViId);

        mainRow.addClass('form-open');
        productForm.removeClass('form-readonly');
        currentMode = mode;
        currentSanPhamId = sanPhamId;
        currentDonViId = donViId;

        if (mode === 'add') {
            formTitle.text('Thêm Sản Phẩm Mới');
            productForm[0].reset();
            selectHangHoa.val('').trigger('change');
            selectDonVi.val('').trigger('change');

            dt = new DataTransfer();
            formImagesUpload[0].files = dt.files;
            renderPreview();

            btnSubmit.html('<i class="bi bi-check-circle-fill me-2"></i>Thêm').show();
            btnCancel.html('<i class="bi bi-x-circle-fill me-2"></i>Huỷ').show();
            btnEditMode.hide();

            enableSelects();
        }
        else if (mode === 'edit' || mode === 'view') {
            // Load dữ liệu từ API
            callApiGetDataById(sanPhamId, donViId);

            if (mode === 'view') {
                formTitle.text('Chi Tiết Sản Phẩm');
                productForm.addClass('form-readonly');
                btnSubmit.hide();
                btnCancel.text('Đóng').show();
                btnEditMode.show();
                disableSelects();
            } else {
                formTitle.text('Sửa Sản Phẩm');
                btnSubmit.html('<i class="bi bi-check-circle-fill me-2"></i>Lưu').show();
                btnCancel.text('Huỷ').show();
                btnEditMode.hide();
                enableSelects();
            }
        }
    }

    function enableSelects() {
        selectHangHoa.prop('disabled', false).trigger('change');
        selectDonVi.prop('disabled', false).trigger('change');
    }

    function disableSelects() {
        selectHangHoa.prop('disabled', true).trigger('change');
        selectDonVi.prop('disabled', true).trigger('change');
    }

    // --- 5. HÀM ĐÓNG FORM ---
    function closeForm() {
        mainRow.removeClass('form-open');

        setTimeout(() => {
            productForm.removeClass('form-readonly');
            productForm[0].reset();
            selectHangHoa.val('').trigger('change');
            selectDonVi.val('').trigger('change');
            dt = new DataTransfer();
            formImagesUpload[0].files = dt.files;
            renderPreview();
            currentMode = null;
            currentSanPhamId = null;
            currentDonViId = null;
        }, 400);
    }

    // --- 6. CÁC SỰ KIỆN ---
    showFormBtn.on('click', function (e) {
        e.preventDefault();
        openForm('add');
    });

    btnCancel.on('click', function (e) {
        e.preventDefault();
        closeForm();
    });

    btnEditMode.on('click', function (e) {
        e.preventDefault();
        if (currentSanPhamId && currentDonViId) {
            openForm('edit', currentSanPhamId, currentDonViId);
        }
    });

    btnSubmit.on('click', function (e) {
        e.preventDefault();

        if (currentMode === 'add') {
            callApiAddSPDV();
        } else if (currentMode === 'edit') {
            callApiEditSPDV();
        }
    });

    // Xử lý click vào bảng
    $('#sampleTable tbody').on('click', 'tr', function (e) {
        const clickedRow = $(this);
        const target = $(e.target);

        const sanPhamId = clickedRow.data('san-pham-id');
        const donViId = clickedRow.data('don-vi-id');

        // Xử lý nút XÓA
        if (target.closest('.btn-delete-khoi').length) {
            e.stopPropagation();
            const tenSP = clickedRow.find('td:eq(1)').text().trim();

            if (confirm(`Bạn có chắc muốn xoá sản phẩm "${tenSP}" không?`)) {
                callApiDeleteSPDV(sanPhamId, donViId);
            }
            return;
        }

        // Xử lý nút EDIT
        if (target.closest('.btn-edit-khoi').length) {
            e.stopPropagation();
            openForm('edit', sanPhamId, donViId);
            return;
        }

        // Click vào dòng để XEM
        openForm('view', sanPhamId, donViId);
    });

    // --- 7. API CALLS ---

    // Lấy dữ liệu theo ID
    async function callApiGetDataById(sanPhamId, donViId) {
        try {
            console.log('Loading data:', sanPhamId, donViId);

            const response = await fetch(`/API/getSanPhamDonViDataById?sanPhamId=${sanPhamId}&donViId=${donViId}`, {
                method: "POST"
            });

            if (!response.ok) {
                throw new Error('Không thể load dữ liệu');
            }

            const data = await response.json();
            console.log("Dữ liệu trả về:", data);

            // Gán vào form
            selectHangHoa.val(data.sanPhamId).trigger('change');
            selectDonVi.val(data.donViId).trigger('change');
            if (inputConversionFactor.length) inputConversionFactor.val(data.heSoQuyDoi);
            inputPrice.val(data.giaBan);

            // Trạng thái
            $('input[name="trangThaiSP"]').each(function () {
                $(this).prop('checked', $(this).val() === data.trangThai);
            });

            // Load ảnh
            if (data.anhs && data.anhs.length > 0) {
                await loadImagesFromApi(data.anhs);
            }

        } catch (error) {
            console.error("Lỗi load dữ liệu:", error);
            alert("Có lỗi khi lấy dữ liệu: " + error.message);
        }
    }

    // Thêm sản phẩm
    async function callApiAddSPDV() {
        try {
            console.log('=== ADDING PRODUCT ===');

            // Validate
            const sanPhamId = selectHangHoa.val();
            const donViId = selectDonVi.val();
            const price = inputPrice.val();
            const status = $('input[name="trangThaiSP"]:checked').val();

            if (!sanPhamId) {
                alert("Vui lòng chọn sản phẩm!");
                return;
            }

            if (!donViId) {
                alert("Vui lòng chọn đơn vị!");
                return;
            }

            if (!price || parseFloat(price) <= 0) {
                alert("Vui lòng nhập giá bán hợp lệ!");
                return;
            }

            if (!status) {
                alert("Vui lòng chọn trạng thái!");
                return;
            }

            // Tạo FormData
            const formData = new FormData();

            // Thêm ảnh
            for (let i = 0; i < dt.files.length; i++) {
                formData.append("ImagesUpload", dt.files[i]);
            }

            // Các trường khác
            formData.append("SanPhamId", sanPhamId);
            formData.append("DonViId", donViId);
            formData.append("HeSoQuyDoi", inputConversionFactor.val() || "1");
            formData.append("GiaBan", price);
            formData.append("MoTa", inputDescription.val() || "");
            formData.append("TrangThai", status);

            console.log('FormData prepared, sending...');

            const response = await fetch('/API/addSPDV', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();

            if (response.ok) {
                alert('Thêm thành công!');
                closeForm();
                await reloadTable();
            } else {
                alert('Lỗi: ' + result.message);
            }

        } catch (error) {
            console.error('Lỗi:', error);
            alert("Lỗi khi thêm sản phẩm: " + error.message);
        }
    }

    // Sửa sản phẩm
    async function callApiEditSPDV() {
        try {
            console.log('=== EDITING PRODUCT ===');

            // Validate
            const sanPhamId = selectHangHoa.val();
            const donViId = selectDonVi.val();
            const price = inputPrice.val();
            const status = $('input[name="trangThaiSP"]:checked').val();

            if (!sanPhamId || !donViId) {
                alert("Thiếu thông tin sản phẩm hoặc đơn vị!");
                return;
            }

            if (!price || parseFloat(price) <= 0) {
                alert("Vui lòng nhập giá bán hợp lệ!");
                return;
            }

            if (!status) {
                alert("Vui lòng chọn trạng thái!");
                return;
            }

            // Tạo FormData
            const formData = new FormData();

            // Thêm ảnh mới (nếu có)
            for (let i = 0; i < dt.files.length; i++) {
                formData.append("ImagesUpload", dt.files[i]);
            }

            // Các trường khác
            formData.append("SanPhamId", sanPhamId);
            formData.append("DonViId", donViId);
            formData.append("HeSoQuyDoi", inputConversionFactor.val() || "1");
            formData.append("GiaBan", price);
            formData.append("MoTa", inputDescription.val() || "");
            formData.append("TrangThai", status);

            const response = await fetch('/API/editSPDV', {
                method: 'POST',
                body: formData
            });

            const result = await response.json();

            if (response.ok) {
                alert('Sửa thành công!');
                closeForm();
                await reloadTable();
            } else {
                alert('Lỗi: ' + result.message);
            }
        } catch (error) {
            console.error('Lỗi:', error);
            alert("Lỗi khi sửa sản phẩm: " + error.message);
        }
    }

    // Xóa sản phẩm
    async function callApiDeleteSPDV(sanPhamId, donViId) {
        try {
            console.log('Deleting:', sanPhamId, donViId);

            const response = await fetch(`/API/deleteSPDV?sanPhamId=${sanPhamId}&donViId=${donViId}`, {
                method: 'DELETE'
            });

            const result = await response.json();

            if (response.ok) {
                alert('Xóa thành công!');
                closeForm();
                await reloadTable();
            } else {
                alert('Lỗi: ' + result.message);
            }
        } catch (error) {
            console.error('Lỗi:', error);
            alert("Lỗi khi xóa: " + error.message);
        }
    }

    // Reload table thủ công
    async function reloadTable() {
        try {
            console.log('Reloading table...');
            const response = await fetch('/API/getAllSPDV');
            const data = await response.json();

            const tbody = $('#tbody-san-pham');
            tbody.empty();

            data.forEach(sp => {
                const row = `
                    <tr data-san-pham-id="${sp.sanPhamId}" data-don-vi-id="${sp.donViId}">
                        <td>${sp.sanPhamId}</td>
                        <td>${sp.ten}</td>
                        <td>${sp.danhMucs.join(", ")}</td>
                        <td>${sp.nhanHieu}</td>
                        <td>${sp.donVi}</td>
                        <td>${sp.giaBan}</td>
                        <td><span class="badge bg-success">${sp.trangThai}</span></td>
                        <td class="text-center">
                            <button class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                                    data-san-pham-id="${sp.sanPhamId}" 
                                    data-don-vi-id="${sp.donViId || ''}" 
                                    title="Sửa">
                                <i class="fas fa-edit"></i>
                            </button>
                            <button class="btn btn-danger btn-sm btn-delete-khoi" 
                                    data-san-pham-id="${sp.sanPhamId}" 
                                    data-don-vi-id="${sp.donViId || ''}" 
                                    title="Xóa">
                                <i class="fas fa-trash-alt"></i>
                            </button>
                        </td>
                    </tr>`;
                tbody.append(row);
            });

            console.log('✅ Table reloaded successfully');
        } catch (error) {
            console.error('❌ Error reloading table:', error);
        }
    }

    // --- 8. MODULE QUẢN LÝ ẢNH ---
    function base64ToFile(base64, fileName) {
        return fetch(base64)
            .then(res => res.arrayBuffer())
            .then(buf => new File([buf], fileName, { type: getMimeType(base64) }));
    }

    function getMimeType(base64) {
        const result = base64.match(/data:(.*);base64,/);
        return result ? result[1] : "image/png";
    }

    async function loadImagesFromApi(base64List) {
        dt = new DataTransfer(); // reset

        if (base64List && base64List.length > 0) {
            for (let i = 0; i < base64List.length; i++) {
                const file = await base64ToFile(base64List[i], `api_img_${i}.png`);
                dt.items.add(file);
            }
        }

        formImagesUpload[0].files = dt.files;
        renderPreview();
    }

    console.log('✅ Page initialized');
});

// --- SIGNALR REALTIME ---
$(async function () {
    console.log('=== INITIALIZING SIGNALR ===');

    await appRealtimeList.initEntityTable({
        key: 'SanPham',
        apiUrl: '/API/getAllSPDV',
        tableId: 'sampleTable',
        tbodyId: 'tbody-san-pham',
        buildRow: sp => {
            return `
                <tr data-san-pham-id="${sp.sanPhamId}" data-don-vi-id="${sp.donViId}">
                    <td>${sp.sanPhamId}</td>
                    <td>${sp.ten}</td>
                    <td>${sp.danhMucs.join(", ")}</td>
                    <td>${sp.nhanHieu}</td>
                    <td>${sp.donVi}</td>
                    <td>${sp.giaBan}</td>
                    <td><span class="badge bg-success">${sp.trangThai}</span></td>
                    <td class="text-center">
                        <button class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                                data-san-pham-id="${sp.sanPhamId}" 
                                data-don-vi-id="${sp.donViId || ''}" 
                                title="Sửa">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn btn-danger btn-sm btn-delete-khoi" 
                                data-san-pham-id="${sp.sanPhamId}" 
                                data-don-vi-id="${sp.donViId || ''}" 
                                title="Xóa">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </td>
                </tr>`;
        }
    });

    console.log('✅ SignalR initialized');
});
