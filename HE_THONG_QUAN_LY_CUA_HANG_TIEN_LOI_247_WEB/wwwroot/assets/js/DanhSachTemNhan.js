$(document).ready(function () {

    // --- KHỞI TẠO SELECT2 ---
    if ($.fn.select2) {
        $('#form-select-identifier').select2({ width: '100%' });
    }

    // --- TRUY XUẤT PHẦN TỬ DOM (jQuery) ---
    const mainRow = $('#main-row-container');
    const listCol = $('#list-col');
    const formCol = $('#form-col');
    const showAddFormBtn = $('#show-add-form-btn');

    const labelForm = $('#label-form');
    const formTitle = $('#form-title');
    const formImagePreview = $('#form-image-preview');

    // Các trường input
    const inputId = $('#form-label-id');
    const selectIdentifier = $('#form-select-identifier');
    const inputContent = $('#form-label-content');
    const inputDate = $('#form-label-date');

    // Nút bấm
    const btnSubmit = $('#btn-submit');
    const btnCancel = $('#btn-cancel');
    const btnEditMode = $('#btn-edit-mode');

    let currentData = null;
    let currentMode = null; // 'add', 'edit', 'view'

    // --- HÀM MỞ FORM ---
    function openForm(mode, data = null) {
        console.log('openForm:', mode, data);
        currentData = data;
        currentMode = mode;

        // 1. Co Bảng, Mở Form
        listCol.removeClass('col-md-12').addClass('col-md-8');
        formCol.removeClass('col-md-0').addClass('col-md-4');
        mainRow.addClass('form-open');
        showAddFormBtn.hide();

        // 2. Xử lý form dựa theo 'mode'
        labelForm.removeClass('form-readonly');

        if (mode === 'add') {
            formTitle.text('Tạo Mẫu Tem Mới');
            labelForm[0].reset();
            selectIdentifier.val('').trigger('change');
            formImagePreview.attr('src', 'https://via.placeholder.com/300x100?text=Code+Image');
            inputDate.val(new Date().toISOString().split('T')[0]);

            // Lấy ID tự động
            callApiGetNextIdTN();

            btnSubmit.text('Thêm').show();
            btnCancel.text('Huỷ').show();
            btnEditMode.hide();
        }
        else if (mode === 'edit' || mode === 'view') {
            // Điền dữ liệu
            inputId.val(data.id);
            inputContent.val(data.noiDungTem);
            inputDate.val(data.ngayIn);
            formImagePreview.attr('src', data.imageUrl || 'https://via.placeholder.com/300x100?text=Code+Image');

            // Điền Select2
            selectIdentifier.val(data.maDinhDanhId).trigger('change');

            if (mode === 'view') {
                formTitle.text('Chi Tiết Tem Nhãn');
                labelForm.addClass('form-readonly');

                btnSubmit.hide();
                btnCancel.text('Đóng').show();
                btnEditMode.text('Chỉnh sửa').show();
            }
            else { // mode === 'edit'
                formTitle.text('Sửa Mẫu Tem');
                labelForm.removeClass('form-readonly');
                inputId.prop('readonly', true);

                btnSubmit.text('Lưu').show();
                btnCancel.text('Huỷ').show();
                btnEditMode.hide();
            }
        }
    }

    // --- HÀM ĐÓNG FORM ---
    function closeForm() {
        listCol.removeClass('col-md-8').addClass('col-md-12');
        formCol.removeClass('col-md-4').addClass('col-md-0');
        mainRow.removeClass('form-open');
        showAddFormBtn.show();

        labelForm.removeClass('form-readonly');
        labelForm[0].reset();
        selectIdentifier.val('').trigger('change');
        formImagePreview.attr('src', 'https://via.placeholder.com/300x100?text=Code+Image');
        currentData = null;
        currentMode = null;
    }

    // --- GÁN SỰ KIỆN CHO NÚT "THÊM" ---
    showAddFormBtn.on('click', function (e) {
        e.preventDefault();
        openForm('add');
    });

    // --- GÁN SỰ KIỆN CHO CÁC NÚT "HUỶ" / "ĐÓNG" ---
    btnCancel.on('click', function (e) {
        e.preventDefault();
        closeForm();
    });

    // --- GÁN SỰ KIỆN CHO NÚT "CHỈNH SỬA" (TRONG FORM VIEW) ---
    btnEditMode.on('click', function () {
        if (currentData) {
            openForm('edit', currentData);
        }
    });

    // --- LOGIC SUBMIT ---
    btnSubmit.on('click', function (e) {
        e.preventDefault();
        
        if (currentMode === 'add') {
            callApiAddTN();
        } else if (currentMode === 'edit') {
            callApiEditTN();
        }
    });

    // --- GÁN SỰ KIỆN CHO BẢNG ---
    $('#sampleTable tbody').on('click', 'tr', function (e) {
        const clickedRow = $(this);
        const target = $(e.target);

        // 1. Xử lý nút XOÁ
        if (target.closest('.btn-delete-khoi').length) {
            e.preventDefault();
            e.stopPropagation();
            
            const id = target.closest('.btn-delete-khoi').data('id');
            const maCode = clickedRow.data('ma-code');
            
            if (confirm(`Bạn có chắc muốn xoá tem nhãn cho mã "${maCode}" không?`)) {
                callApiDeleteTN(id);
            }
            return;
        }

        // 2. Xử lý nút EDIT
        if (target.closest('.btn-edit-khoi').length) {
            e.preventDefault();
            e.stopPropagation();
            
            const id = target.closest('.btn-edit-khoi').data('id');
            console.log("ID cần sửa:", id);
            
            fetch(`/API/get-TN-by-id?id=${encodeURIComponent(id)}`)
                .then(response => {
                    if (!response.ok) throw new Error('Lỗi khi gọi API');
                    return response.json();
                })
                .then(data => {
                    console.log("Dữ liệu trả về:", data);
                    openForm('edit', data);
                })
                .catch(error => {
                    console.error(error);
                    alert("Có lỗi khi lấy dữ liệu!");
                });
            return;
        }

        // 3. Xử lý nút IN
        if (target.closest('.btn-secondary').length) {
            e.preventDefault();
            e.stopPropagation();
            alert('Đang gọi máy in... (mô phỏng)');
            return;
        }

        // 4. Xử lý click XEM
        if (!target.closest('button, a').length) {
            const data = {
                id: clickedRow.data('id'),
                maDinhDanhId: clickedRow.data('ma-dinh-danh-id'),
                noiDungTem: clickedRow.data('noi-dung'),
                ngayIn: clickedRow.data('ngay-in'),
                maCode: clickedRow.data('ma-code')
            };
            openForm('view', data);
        }
    });

    // ==================== API CALLS ====================

    async function callApiGetNextIdTN() {
        const dataToSend = {
            prefix: "TN",
            totalLength:  5 // TN + 4 số = TN0001
        };
        try {
            const response = await fetch('/API/get-next-id-TN', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dataToSend)
            });
            const data = await response.json();
            if (data && data.nextId) {
                inputId.val(data.nextId);
            } else {
                alert('Không thể lấy mã tự động, vui lòng thử lại.');
            }
        } catch (error) {
            console.error('Lỗi khi lấy mã:', error);
            alert('Không thể lấy mã tự động, vui lòng thử lại.');
        }
    }

    async function callApiAddTN() {
        try {
            const duLieu = {
                Id: inputId.val(),
                MaDinhDanhId: selectIdentifier.val(),
                NoiDungTem: inputContent.val(),
                NgayIn: inputDate.val(),
                AnhId: "ANH_DEFAULT"
            };

            console.log('Sending data:', duLieu);

            // Validate
            if (!duLieu.MaDinhDanhId) {
                alert('Vui lòng chọn mã định danh!');
                return;
            }

            if (!duLieu.NoiDungTem) {
                alert('Vui lòng nhập nội dung tem!');
                return;
            }

            const response = await fetch('/API/add-TN', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
            });

            const result = await response.json();

            if (response.ok) {
                alert('Thêm thành công!');
                closeForm();
                
                // Fallback: Reload table manually nếu SignalR không hoạt động
                await reloadTable();
            } else {
                alert('Lỗi: ' + result.message);
            }
        } catch (error) {
            console.error('Lỗi:', error);
            alert('Lỗi khi thêm tem nhãn: ' + error.message);
        }
    }

    async function callApiEditTN() {
        try {
            const duLieu = {
                Id: inputId.val(),
                MaDinhDanhId: selectIdentifier.val(),
                NoiDungTem: inputContent.val(),
                NgayIn: inputDate.val(),
                AnhId: "ANH_DEFAULT",
                IsDelete: false
            };

            console.log('Updating data:', duLieu);

            // Validate
            if (!duLieu.MaDinhDanhId) {
                alert('Vui lòng chọn mã định danh!');
                return;
            }

            if (!duLieu.NoiDungTem) {
                alert('Vui lòng nhập nội dung tem!');
                return;
            }

            const response = await fetch('/API/edit-TN', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
            });

            const result = await response.json();

            if (response.ok) {
                alert('Sửa thành công!');
                closeForm();
                
                // Fallback: Reload table manually
                await reloadTable();
            } else {
                alert('Lỗi: ' + result.message);
            }
        } catch (error) {
            console.error('Lỗi:', error);
            alert('Lỗi khi sửa tem nhãn: ' + error.message);
        }
    }

    async function callApiDeleteTN(id) {
        try {
            const response = await fetch(`/API/delete-TN/${encodeURIComponent(id)}`, {
                method: 'DELETE'
            });

            const result = await response.json();

            if (response.ok) {
                alert('Xóa thành công!');
                closeForm();
                
                // Fallback: Reload table manually
                await reloadTable();
            } else {
                alert('Lỗi: ' + result.message);
            }
        } catch (error) {
            console.error('Lỗi:', error);
            alert('Lỗi khi xóa: ' + error.message);
        }
    }

    // Hàm reload bảng thủ công
    async function reloadTable() {
        try {
            console.log('Reloading table...');
            const response = await fetch('/API/get-all-TN');
            const data = await response.json();
            
            const tbody = $('#sampleTable tbody');
            tbody.empty();
            
            data.forEach(tn => {
                const row = `
                    <tr data-id="${tn.id}" 
                        data-ma-dinh-danh-id="${tn.maDinhDanhId}" 
                        data-noi-dung="${tn.noiDungTem}" 
                        data-ngay-in="${tn.ngayIn}"
                        data-ma-code="${tn.maCode}"
                        style="cursor: pointer;">
                        <td>${tn.id}</td>
                        <td>${tn.maDinhDanhId}</td>
                        <td>${tn.noiDungTem}</td>
                        <td>${new Date(tn.ngayIn).toLocaleDateString('vi-VN')}</td>
                        <td class="text-center">
                            <a class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                               href="#" 
                               data-id="${tn.id}"
                               title="Sửa">
                                <i class="fas fa-edit"></i>
                            </a>
                            <a class="btn btn-danger btn-sm btn-delete-khoi" 
                               href="#" 
                               data-id="${tn.id}" 
                               title="Xóa">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                            <a class="btn btn-secondary btn-sm" 
                               href="#" 
                               title="In tem">
                                <i class="fas fa-print"></i>
                            </a>
                        </td>
                    </tr>`;
                tbody.append(row);
            });
            
            console.log('✅ Table reloaded successfully');
        } catch (error) {
            console.error('❌ Error reloading table:', error);
        }
    }

    // ==================== SIGNALR REALTIME ====================
    $(async function () {
        await appRealtimeList.initEntityTable({
            key: 'TemNhan',  // key SignalR
            apiUrl: '/API/get-all-TN',  // API lấy dữ liệu
            tableId: 'sampleTable',
            tbodyId: 'sampleTable tbody',  // Chỉ định tbody để giữ header
            buildRow: tn => {
                return `
                    <tr data-id="${tn.id}" 
                        data-ma-dinh-danh-id="${tn.maDinhDanhId}" 
                        data-noi-dung="${tn.noiDungTem}" 
                        data-ngay-in="${tn.ngayIn}"
                        data-ma-code="${tn.maCode}"
                        style="cursor: pointer;">
                        <td>${tn.id}</td>
                        <td>${tn.maDinhDanhId}</td>
                        <td>${tn.noiDungTem}</td>
                        <td>${new Date(tn.ngayIn).toLocaleDateString('vi-VN')}</td>
                        <td class="text-center">
                            <a class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                               href="#" 
                               data-id="${tn.id}"
                               title="Sửa">
                                <i class="fas fa-edit"></i>
                            </a>
                            <a class="btn btn-danger btn-sm btn-delete-khoi" 
                               href="#" 
                               data-id="${tn.id}" 
                               title="Xóa">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                            <a class="btn btn-secondary btn-sm" 
                               href="#" 
                               title="In tem">
                                <i class="fas fa-print"></i>
                            </a>
                        </td>
                    </tr>`;
            }
        });
    });

    console.log('=== READY ===');
});