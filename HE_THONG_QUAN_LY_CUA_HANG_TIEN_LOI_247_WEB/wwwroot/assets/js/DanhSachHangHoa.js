$(function () {
    console.log('=== DANH SÁCH HÀNG HÓA PAGE LOADED ===');

    // --- 1. KHỞI TẠO SELECT2 ---
    $('#form-select-nhan-hieu').select2({ 
        width: '100%', 
        dropdownParent: $('#product-form-col') 
    });

    $('#form-select-danh-muc').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều danh mục',
        dropdownParent: $('#product-form-col')
    });

    // --- 2. TRUY XUẤT PHẦN TỬ DOM ---
    const mainRow = $('#main-row-container');
    const listCol = $('#list-col');
    const formCol = $('#product-form-col');
    const showFormBtn = $('#show-add-form-btn');
    
    const productForm = $('#product-form');
    const formTitle = $('#form-title');

    // Input fields
    const inputId = $('#form-product-id');
    const inputName = $('#form-product-name');
    const selectNhanHieu = $('#form-select-nhan-hieu');
    const selectDanhMuc = $('#form-select-danh-muc');

    // Buttons
    const btnSubmit = $('#btn-submit');
    const btnCancel = $('#btn-cancel');
    const btnEditMode = $('#btn-edit');

    let currentMode = null; // 'add', 'edit', 'view'
    let currentData = null;

    // --- 3. HÀM MỞ FORM ---
    function openForm(mode, data = null) {
        console.log('openForm:', mode, data);
        currentMode = mode;
        currentData = data;

        // Co bảng, mở form
        listCol.removeClass('col-md-12').addClass('col-md-8');
        formCol.removeClass('col-md-0').addClass('col-md-4');
        mainRow.addClass('form-open');
        showFormBtn.hide();

        productForm.removeClass('form-readonly');

        if (mode === 'add') {
            formTitle.text('Thêm Sản Phẩm Mới');
            productForm[0].reset();
            selectNhanHieu.val('').trigger('change');
            selectDanhMuc.val([]).trigger('change');

            // Lấy ID tự động
            callApiGetNextId();

            inputId.prop('readonly', true);
            btnSubmit.html('<i class="bi bi-check-circle-fill me-2"></i>Thêm').show();
            btnCancel.html('<i class="bi bi-x-circle-fill me-2"></i>Huỷ').show();
            btnEditMode.hide();
            
            enableSelects();
        }
        else if (mode === 'edit' || mode === 'view') {
            // Điền dữ liệu
            inputId.val(data.id);
            inputName.val(data.ten);
            selectNhanHieu.val(data.nhanHieuId).trigger('change');
            selectDanhMuc.val(data.danhMucs).trigger('change');

            if (mode === 'view') {
                formTitle.text('Chi Tiết Sản Phẩm');
                productForm.addClass('form-readonly');
                btnSubmit.hide();
                btnCancel.text('Đóng').show();
                btnEditMode.show();
                disableSelects();
            } else {
                formTitle.text('Sửa Sản Phẩm');
                inputId.prop('readonly', true);
                btnSubmit.html('<i class="bi bi-check-circle-fill me-2"></i>Lưu').show();
                btnCancel.text('Huỷ').show();
                btnEditMode.hide();
                enableSelects();
            }
        }
    }

    function enableSelects() {
        selectNhanHieu.prop('disabled', false).trigger('change');
        selectDanhMuc.prop('disabled', false).trigger('change');
    }

    function disableSelects() {
        selectNhanHieu.prop('disabled', true).trigger('change');
        selectDanhMuc.prop('disabled', true).trigger('change');
    }

    // --- 4. HÀM ĐÓNG FORM ---
    function closeForm() {
        listCol.removeClass('col-md-8').addClass('col-md-12');
        formCol.removeClass('col-md-4').addClass('col-md-0');
        mainRow.removeClass('form-open');
        showFormBtn.show();

        productForm.removeClass('form-readonly');
        productForm[0].reset();
        selectNhanHieu.val('').trigger('change');
        selectDanhMuc.val(null).trigger('change');
        currentData = null;
        currentMode = null;
    }

    // --- 5. GÁN SỰ KIỆN ---
    showFormBtn.on('click', function (e) {
        e.preventDefault();
        openForm('add');
    });

    btnCancel.on('click', function (e) {
        e.preventDefault();
        closeForm();
    });

    btnEditMode.on('click', function () {
        if (currentData) {
            openForm('edit', currentData);
        }
    });

    btnSubmit.on('click', async function (e) {
        e.preventDefault();

        if (currentMode === 'add') {
            await callApiAddSP();
        } else if (currentMode === 'edit') {
            await callApiEditSP();
        }
    });

    // Xử lý click vào bảng
    $('#sampleTable tbody').on('click', 'tr', async function (e) {
        const clickedRow = $(this);
        const target = $(e.target);

        const sanPhamId = clickedRow.data('id');

        // Xử lý nút XÓA
        if (target.closest('.btn-delete-khoi').length) {
            e.stopPropagation();
            const tenSP = clickedRow.find('td:eq(1)').text().trim();

            if (confirm(`Bạn có chắc muốn xoá sản phẩm "${tenSP}" không?`)) {
                await callApiDeleteSP(sanPhamId);
            }
            return;
        }

        // Xử lý nút EDIT
        if (target.closest('.btn-edit-khoi').length) {
            e.stopPropagation();
            const data = await callApiGetSanPhamDataById(sanPhamId);
            if (data) {
                openForm('edit', data);
            }
            return;
        }

        // Click vào dòng để XEM
        const data = await callApiGetSanPhamDataById(sanPhamId);
        if (data) {
            openForm('view', data);
        }
    });

    // --- 6. API CALLS ---

    // Lấy ID tiếp theo
    async function callApiGetNextId() {
        const dataToSend = {
            prefix: "SP",
            totalLength: 6
        };
        try {
            const response = await fetch('/API/get-next-id-SP', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dataToSend)
            });

            const data = await response.json();
            if (data && data.nextId) {
                inputId.val(data.nextId);
            } else {
                alert('Không thể lấy mã sản phẩm, vui lòng thử lại.');
            }
        } catch (error) {
            console.error('Lỗi khi lấy mã sản phẩm:', error);
            alert('Không thể lấy mã sản phẩm, vui lòng thử lại.');
        }
    }

    // Lấy dữ liệu theo ID
    async function callApiGetSanPhamDataById(id) {
        try {
            console.log('Loading data for:', id);
            
            const response = await fetch('/API/getSanPhamDataById', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(id)
            });

            if (!response.ok) {
                throw new Error('Không thể lấy dữ liệu sản phẩm');
            }

            const data = await response.json();
            console.log('Data loaded:', data);
            return data;
        } catch (error) {
            console.error('Lỗi khi lấy dữ liệu sản phẩm:', error);
            alert('Lỗi: ' + error.message);
            return null;
        }
    }

    // Thêm sản phẩm
    async function callApiAddSP() {
        try {
            const duLieu = {
                Id: inputId.val(),
                Ten: inputName.val(),
                NhanHieu: selectNhanHieu.val(),
                DanhMucs: selectDanhMuc.val() || []
            };

            // Validate
            if (!duLieu.Ten) {
                alert('Vui lòng nhập tên sản phẩm!');
                return;
            }

            if (!duLieu.NhanHieu) {
                alert('Vui lòng chọn nhãn hiệu!');
                return;
            }

            if (duLieu.DanhMucs.length === 0) {
                alert('Vui lòng chọn ít nhất một danh mục!');
                return;
            }

            // Tạo FormData
            const formData = new FormData();
            formData.append('Id', duLieu.Id.trim());
            formData.append('Ten', duLieu.Ten.trim());
            formData.append('NhanHieu', duLieu.NhanHieu);
            
            duLieu.DanhMucs.forEach(dm => {
                formData.append('DanhMucs', dm);
            });

            const response = await fetch('/API/add-SP', {
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
            alert('Lỗi khi thêm sản phẩm: ' + error.message);
        }
    }

    // Sửa sản phẩm
    async function callApiEditSP() {
        try {
            const duLieu = {
                Id: inputId.val(),
                Ten: inputName.val(),
                NhanHieu: selectNhanHieu.val(),
                DanhMucs: selectDanhMuc.val() || []
            };

            // Validate
            if (!duLieu.Ten) {
                alert('Vui lòng nhập tên sản phẩm!');
                return;
            }

            if (!duLieu.NhanHieu) {
                alert('Vui lòng chọn nhãn hiệu!');
                return;
            }

            if (duLieu.DanhMucs.length === 0) {
                alert('Vui lòng chọn ít nhất một danh mục!');
                return;
            }

            // Tạo FormData
            const formData = new FormData();
            formData.append('Id', duLieu.Id.trim());
            formData.append('Ten', duLieu.Ten.trim());
            formData.append('NhanHieu', duLieu.NhanHieu);
            
            duLieu.DanhMucs.forEach(dm => {
                formData.append('DanhMucs', dm);
            });

            const response = await fetch('/API/editSP', {
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
            alert('Lỗi khi sửa sản phẩm: ' + error.message);
        }
    }

    // Xóa sản phẩm
    async function callApiDeleteSP(id) {
        try {
            console.log('Deleting:', id);
            
            const response = await fetch(`/API/deleteSP${encodeURIComponent(id)}`, {
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
            alert('Lỗi khi xóa: ' + error.message);
        }
    }

    // Reload table thủ công
    async function reloadTable() {
        try {
            console.log('Reloading table...');
            const response = await fetch('/API/get-all-SP');
            const data = await response.json();

            const tbody = $('#tbodySanPham');
            tbody.empty();

            data.forEach(sp => {
                const row = `
                    <tr data-id="${sp.id}" data-ten="${sp.ten}">
                        <td>${sp.id}</td>
                        <td>${sp.ten}</td>
                        <td>${sp.danhMucs.join(', ')}</td>
                        <td>${sp.nhanHieu}</td>
                        <td class="text-center">
                            <button class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                                    data-id="${sp.id}" 
                                    title="Sửa">
                                <i class="fas fa-edit"></i>
                            </button>
                            <button class="btn btn-danger btn-sm btn-delete-khoi" 
                                    data-id="${sp.id}" 
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

    console.log('✅ Page initialized');
});

// --- SIGNALR REALTIME ---
$(async function () {
    console.log('=== INITIALIZING SIGNALR FOR HANG HOA ===');

    await appRealtimeList.initEntityTable({
        key: 'HangHoa',
        apiUrl: '/API/get-all-SP',
        tableId: 'sampleTable',
        tbodyId: 'tbodySanPham',
        buildRow: sp => {
            return `
                <tr data-id="${sp.id}" data-ten="${sp.ten}">
                    <td>${sp.id}</td>
                    <td>${sp.ten}</td>
                    <td>${sp.danhMucs.join(', ')}</td>
                    <td>${sp.nhanHieu}</td>
                    <td class="text-center">
                        <button class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                                data-id="${sp.id}" 
                                title="Sửa">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn btn-danger btn-sm btn-delete-khoi" 
                                data-id="${sp.id}" 
                                title="Xóa">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </td>
                </tr>`;
        }
    });

    console.log('✅ SignalR initialized for HangHoa');
});