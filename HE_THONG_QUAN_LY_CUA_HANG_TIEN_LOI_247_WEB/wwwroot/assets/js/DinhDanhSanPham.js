$(document).ready(function () {

    // --- KHỞI TẠO SELECT2 ---
    if ($.fn.select2) {
        $('#form-select-sanpham').select2({ width: '100%' });
    }

    // --- TRUY XUẤT PHẦN TỬ DOM (jQuery) ---
    const mainRow = $('#main-row-container');
    const listCol = $('#list-col');
    const formCol = $('#form-col');
    const showAddFormBtn = $('#show-add-form-btn');

    const identifierForm = $('#identifier-form');
    const formTitle = $('#form-title');
    const formImagePreview = $('#form-image-preview');
    const formImageUpload = $('#form-image-upload');

    // Các trường input
    const inputId = $('#form-id');
    const selectSanPham = $('#form-select-sanpham');
    const inputMaCode = $('#form-ma-code');

    // Nút bấm
    const btnSubmit = $('#btn-submit');
    const btnCancel = $('#btn-cancel');
    const btnEditMode = $('#btn-edit-mode');

    let currentData = null; // Biến lưu trữ dữ liệu khi xem
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
        identifierForm.removeClass('form-readonly'); // Mở khoá form trước

        if (mode === 'add') {
            formTitle.text('Tạo Mã Mới');
            identifierForm[0].reset(); // Xoá trắng form
            selectSanPham.val('').trigger('change');
            formImagePreview.attr('src', 'https://via.placeholder.com/200?text=Ảnh+Code');

            // Lấy ID tự động
            callApiGetNextIdMDD();

            btnSubmit.text('Thêm').show();
            btnCancel.text('Huỷ').show();
            btnEditMode.hide();
        }
        else if (mode === 'edit' || mode === 'view') {
            // Điền dữ liệu
            inputId.val(data.id);
            inputMaCode.val(data.maCode);
            formImagePreview.attr('src', data.duongDan || 'https://via.placeholder.com/200?text=Ảnh+Code');

            // Điền trạng thái (radio)
            $(`input[name="form-loai-ma"][value="${data.loaiMa}"]`).prop('checked', true);

            // Điền Select2
            selectSanPham.val(data.sanPhamDonViId).trigger('change');

            if (mode === 'view') {
                formTitle.text('Chi Tiết Mã Định Danh');
                identifierForm.addClass('form-readonly'); // Khoá form

                btnSubmit.hide();
                btnCancel.text('Đóng').show();
                btnEditMode.text('Chỉnh sửa').show();
            }
            else { // mode === 'edit'
                formTitle.text('Sửa Mã Định Danh');
                identifierForm.removeClass('form-readonly'); // Mở khoá
                inputId.prop('readonly', true); // Mã luôn readonly

                btnSubmit.text('Lưu').show();
                btnCancel.text('Huỷ').show();
                btnEditMode.hide();
            }
        }
    }

    // --- HÀM ĐÓNG FORM ---
    function closeForm() {
        // 1. Phình Bảng, Đóng Form
        listCol.removeClass('col-md-8').addClass('col-md-12');
        formCol.removeClass('col-md-4').addClass('col-md-0');
        mainRow.removeClass('form-open');
        showAddFormBtn.show();

        // 2. Reset form về trạng thái mặc định
        identifierForm.removeClass('form-readonly');
        identifierForm[0].reset();
        selectSanPham.val('').trigger('change');
        formImagePreview.attr('src', 'https://via.placeholder.com/200?text=Ảnh+Code');
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
            callApiAddMDD();
        } else if (currentMode === 'edit') {
            callApiEditMDD();
        }
    });

    // --- CẬP NHẬT: LOGIC PREVIEW ẢNH KHI UPLOAD ---
    formImageUpload.on('change', function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                formImagePreview.attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });

    // --- GÁN SỰ KIỆN CHO BẢNG (CLICK VÀO HÀNG ĐỂ XEM) ---
    $('#sampleTable tbody').on('click', 'tr', function (e) {
        const clickedRow = $(this);
        const target = $(e.target);

        // 1. Xử lý nút XOÁ
        if (target.closest('.btn-delete-khoi').length) {
            e.preventDefault();
            e.stopPropagation();
            
            const id = target.closest('.btn-delete-khoi').data('id');
            const maCode = clickedRow.find('td:eq(3)').text().trim();
            
            if (confirm(`Bạn có chắc muốn xoá mã định danh "${maCode}" không?`)) {
                callApiDeleteMDD(id);
            }
            return;
        }

        // 2. Xử lý nút EDIT
        if (target.closest('.btn-edit-khoi').length) {
            e.preventDefault();
            e.stopPropagation();
            
            const id = target.closest('.btn-edit-khoi').data('id');
            console.log("ID cần sửa:", id);
            
            // Gọi API để lấy thông tin đầy đủ
            fetch(`/API/get-MDD-by-id?id=${encodeURIComponent(id)}`)
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

        // 3. Xử lý click XEM (nếu click vào hàng, không phải nút)
        if (!target.closest('button, a').length) {
            const data = {
                id: clickedRow.data('id'),
                sanPhamDonViId: clickedRow.data('san-pham-don-vi-id'),
                loaiMa: clickedRow.data('loai-ma'),
                maCode: clickedRow.data('ma-code'),
                duongDan: clickedRow.data('duong-dan')
            };
            openForm('view', data);
        }
    });

    // ==================== API CALLS ====================

    async function callApiGetNextIdMDD() {
        const dataToSend = {
            prefix: "MDD",
            totalLength: 6  // Thay đổi từ 8 thành 6 (MDD + 3 số = MDD001)
        };
        try {
            const response = await fetch('/API/get-next-id-MDD', {
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

    async function callApiAddMDD() {
        try {
            const duLieu = {
                Id: inputId.val(),
                SanPhamDonViId: selectSanPham.val(),
                LoaiMa: $('input[name="form-loai-ma"]:checked').val(),
                MaCode: inputMaCode.val(),
                DuongDan: formImagePreview.attr('src')
            };

            console.log('Sending data:', duLieu);

            // Validate
            if (!duLieu.SanPhamDonViId) {
                alert('Vui lòng chọn sản phẩm!');
                return;
            }

            if (!duLieu.MaCode) {
                alert('Vui lòng nhập mã code!');
                return;
            }

            const response = await fetch('/API/add-MDD', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
            });

            const result = await response.json();

            if (response.ok) {
                alert('Thêm thành công!');
                closeForm();
                
                // Fallback: Reload table manually
                await reloadTable();
            } else {
                alert('Lỗi: ' + result.message);
            }
        } catch (error) {
            console.error('Lỗi:', error);
            alert('Lỗi khi thêm mã định danh: ' + error.message);
        }
    }

    async function callApiEditMDD() {
        try {
            const duLieu = {
                Id: inputId.val(),
                SanPhamDonViId: selectSanPham.val(),
                LoaiMa: $('input[name="form-loai-ma"]:checked').val(),
                MaCode: inputMaCode.val(),
                DuongDan: formImagePreview.attr('src'),
                IsDelete: false
            };

            console.log('Updating data:', duLieu);

            // Validate
            if (!duLieu.SanPhamDonViId) {
                alert('Vui lòng chọn sản phẩm!');
                return;
            }

            if (!duLieu.MaCode) {
                alert('Vui lòng nhập mã code!');
                return;
            }

            const response = await fetch('/API/edit-MDD', {
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
            alert('Lỗi khi sửa mã định danh: ' + error.message);
        }
    }

    async function callApiDeleteMDD(id) {
        try {
            const response = await fetch(`/API/delete-MDD/${encodeURIComponent(id)}`, {
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
            const response = await fetch('/API/get-all-MDD');
            const data = await response.json();
            
            const tbody = $('#sampleTable tbody');
            tbody.empty();
            
            data.forEach(mdd => {
                const row = `
                    <tr data-id="${mdd.id}" 
                        data-san-pham-don-vi-id="${mdd.sanPhamDonViId}" 
                        data-loai-ma="${mdd.loaiMa}"
                        data-ma-code="${mdd.maCode}" 
                        data-duong-dan="${mdd.duongDan}"
                        style="cursor: pointer;">
                        <td>${mdd.id}</td>
                        <td>${mdd.sanPhamDonViId}</td>
                        <td>
                            <span class="badge ${mdd.loaiMa === 'QR' ? 'bg-info' : 'bg-primary'}">
                                ${mdd.loaiMa}
                            </span>
                        </td>
                        <td>${mdd.maCode}</td>
                        <td>
                            ${mdd.duongDan ? `<small>${mdd.duongDan}</small>` : '<span class="text-muted">Chưa có</span>'}
                        </td>
                        <td class="text-center">
                            <a class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                               href="#" 
                               data-id="${mdd.id}"
                               title="Sửa">
                                <i class="fas fa-edit"></i>
                            </a>
                            <a class="btn btn-danger btn-sm btn-delete-khoi" 
                               href="#" 
                               data-id="${mdd.id}" 
                               title="Xóa">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </td>
                    </tr>`;
                tbody.append(row);
            });
            
            console.log(' Table reloaded successfully');
        } catch (error) {
            console.error('Error reloading table:', error);
        }
    }

    // Tích hợp SignalR để reload table khi có thay đổi từ server
    $(async function () {
        await appRealtimeList.initEntityTable({
            key: 'MaDinhDanh',  // key SignalR
            apiUrl: '/API/get-all-MDD',  // API lấy dữ liệu
            tableId: 'sampleTable',
            tbodyId: 'sampleTable tbody',  // Chỉ định chính xác tbody để không mất header
            buildRow: mdd => {
                return `
                    <tr data-id="${mdd.id}" 
                        data-san-pham-don-vi-id="${mdd.sanPhamDonViId}" 
                        data-loai-ma="${mdd.loaiMa}"
                        data-ma-code="${mdd.maCode}" 
                        data-duong-dan="${mdd.duongDan}"
                        style="cursor: pointer;">
                        <td>${mdd.id}</td>
                        <td>${mdd.sanPhamDonViId}</td>
                        <td>
                            <span class="badge ${mdd.loaiMa === 'QR' ? 'bg-info' : 'bg-primary'}">
                                ${mdd.loaiMa}
                            </span>
                        </td>
                        <td>${mdd.maCode}</td>
                        <td>
                            ${mdd.duongDan ? `<small>${mdd.duongDan}</small>` : '<span class="text-muted">Chưa có</span>'}
                        </td>
                        <td class="text-center">
                            <a class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                               href="#" 
                               data-id="${mdd.id}"
                               title="Sửa">
                                <i class="fas fa-edit"></i>
                            </a>
                            <a class="btn btn-danger btn-sm btn-delete-khoi" 
                               href="#" 
                               data-id="${mdd.id}" 
                               title="Xóa">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </td>
                    </tr>`;
            }
        });
    });

});