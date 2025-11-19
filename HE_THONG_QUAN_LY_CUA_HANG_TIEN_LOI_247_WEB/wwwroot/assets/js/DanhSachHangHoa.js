
$(function () {

    // --- 1. KHỞI TẠO CÁC THÀNH PHẦN ---
    $('#form-select-nhan-hieu').select2({ width: '100%', dropdownParent: $('#product-form-col') });

    // KIỂM TRA TRƯỚC KHI KHỞI TẠO (vì #form-select-don-vi không có trong HTML)
    if ($('#form-select-don-vi').length) {
        $('#form-select-don-vi').select2({ width: '100%', dropdownParent: $('#product-form-col') });
    }

    $('#form-select-danh-muc').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều hàng hoá',
        dropdownParent: $('#product-form-col')
    });

    // --- 2. TRUY XUẤT PHẦN TỬ DOM ---
    const mainRow = $('#main-row-container');
    const showFormBtn = $('#show-add-form-btn');
    const productForm = $('#product-form');
    const formTitle = $('#form-title');

    // Các phần tử upload ảnh (không tồn tại trong HTML của bạn)
    const uploadPlaceholder = $('#upload-placeholder');
    const imagesPreviewContainer = $('#images-preview-container');

    // Các trường input (một số bị thiếu trong HTML)
    const inputId = $('#form-product-id');
    const inputName = $('#form-product-name');
    const selectNhanHieu = $('#form-select-nhan-hieu');
    const selectDanhMuc = $('#form-select-danh-muc');

    // Nút bấm
    const btnSubmit = $('#btn-submit');
    const btnCancel = $('#btn-cancel');
    const btnEditMode = $('#btn-edit');

    // Biến lưu trữ file (định nghĩa ở đây)
    let dt = new DataTransfer();

    // Hàm renderPreview (định nghĩa hàm rỗng để tránh lỗi)


    // --- 4. HÀM MỞ FORM ---
    function openForm(mode, data) {
        mainRow.addClass('form-open');
        productForm.removeClass('form-readonly');

        if (mode === 'add') {

            formTitle.text('Thêm Sản Phẩm Mới');
            productForm[0].reset();

            selectNhanHieu.val('').trigger('change');
            selectDanhMuc.val([]).trigger('change');

            btnSubmit.text('Thêm').show();
            btnCancel.text('Huỷ').show();
            btnEditMode.hide();

        } else {

            // ĐỔ DỮ LIỆU VÀO FORM
            $('#form-product-id').val(data.id);
            $('#form-product-name').val(data.ten);

            $('#form-select-nhan-hieu')
                .val(data.nhanHieuId)
                .trigger('change');

            $('#form-select-danh-muc')
                .val(data.danhMucs)
                .trigger('change');

            // Form mode
            if (mode === 'view') {
                formTitle.text('Chi Tiết Sản Phẩm');
                btnSubmit.hide();
                btnCancel.text('Đóng').show();
                btnEditMode.show();
            } else {
                formTitle.text('Sửa Sản Phẩm');
                btnSubmit.text('Lưu').show();
                btnCancel.text('Huỷ').show();
                btnEditMode.show();
                btnSubmit.hide();
            }
        }
    }

    // --- 5. HÀM ĐÓNG FORM ---
    function closeForm() {
        mainRow.removeClass('form-open');

        setTimeout(() => {
            productForm.removeClass('form-readonly');
            productForm[0].reset();
            selectNhanHieu.val('').trigger('change');
            selectDanhMuc.val(null).trigger('change');


        }, 400);
    }

    // --- 6. CÁC SỰ KIỆN KHÁC ---
    showFormBtn.on('click', function (e) { e.preventDefault(); callApiGetNextId(); openForm('add'); });
    btnCancel.on('click', function (e) { e.preventDefault(); closeForm(); });

    $('#sampleTable tbody').on('click', 'tr', async function (e) {
        const clickedRow = $(this);
        const target = $(e.target);
        const data = clickedRow.data();
        data.price = parseFloat(data.price); // Chuyển đổi giá trị số

        if (target.closest('.btn-delete-khoi').length) {
            e.stopPropagation();
            if (confirm(`Bạn có chắc muốn xoá sản phẩm "${data.ten}" không?`)) {
                clickedRow.fadeOut(300, function () { $(this).remove(); });
                callApiDeleteSP(data.id);
                closeForm();
            }
            return;
        }
        if (target.closest('.btn-edit-khoi').length) {
            e.stopPropagation();
            const dataNew = await callApiGetSanPhamDataById(data.id);
            openForm('edit', dataNew);
            return;
        }
    });

    btnEditMode.on('click', function (e) {
        e.preventDefault();
        callApiEditSP();
        closeForm();
    });

    btnSubmit.on('click', function (e) {
        e.preventDefault();
        callApiAddSP();
        closeForm();
    });
















    async function callApiAddSP() {
        try {
            const Id = document.getElementById('form-product-id').value;
            const Ten = document.getElementById('form-product-name').value;
            const NhanHieu = document.getElementById('form-select-nhan-hieu').value;
            if (!inputId || !inputName) {
                alert('Các trường "id" và "ten" là bắt buộc!');
                return;
            }
            const danhMucs = Array.from(document.getElementById('form-select-danh-muc').selectedOptions).map(option => option.value)

            const formData = new FormData();

            formData.append('Id', Id.trim());
            formData.append('Ten', Ten.trim());
            formData.append('NhanHieu', NhanHieu);
            if (danhMucs.length > 0) {
                danhMucs.forEach(danhMuc => {
                    formData.append('DanhMucs', danhMuc);
                });
            }
            // Gửi request
            const response = await fetch('/API/add-SP', {
                method: 'POST',
                body: formData
            });

            // Kiểm tra response
            if (!response.ok) {
                throw new Error(errorMessage);
            }


            closeForm();


        } catch (error) {
            alert('Lỗi: ' + error.message);
        }
    }

    async function callApiEditSP() {
        try {
            const Id = document.getElementById('form-product-id').value;
            const Ten = document.getElementById('form-product-name').value;
            const NhanHieu = document.getElementById('form-select-nhan-hieu').value;
            if (!inputId || !inputName) {
                alert('Các trường "id" và "ten" là bắt buộc!');
                return;
            }
            const danhMucs = Array.from(document.getElementById('form-select-danh-muc').selectedOptions).map(option => option.value)

            const formData = new FormData();

            formData.append('Id', Id.trim());
            formData.append('Ten', Ten.trim());
            formData.append('NhanHieu', NhanHieu);
            if (danhMucs.length > 0) {
                danhMucs.forEach(danhMuc => {
                    formData.append('DanhMucs', danhMuc);
                });
            }
            // Gửi request
            const response = await fetch('/API/editSP', {
                method: 'POST',
                body: formData
            });

            // Kiểm tra response
            if (!response.ok) {
                throw new Error(errorMessage);
            }


            closeForm();


        } catch (error) {
            alert('Lỗi: ' + error.message);
        }
    }





    async function callApiGetNextId() {
        const dataToSend = {
            prefix: "SP",
            totalLength: 6
        };
        try {
            const response = await fetch('/API/get-next-id-SP',
                {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(dataToSend)
                });
            if (!response.ok) {
                throw new Error(errorMessage);
            }
            const data = await response.json();
            if (data) {
                document.getElementById('form-product-id').value = data.nextId;
            }
            else {
                alert('Không thể lấy mã sản phẩm, vui lòng thử lại.');
            }
        } catch (error) {
            console.error('Lỗi khi lấy mã sản phẩm:', error);
            alert('Không thể lấy mã sản phẩm, vui lòng thử lại.');
        }
    }



    async function callApiGetSanPhamDataById(id) {

        try {
            const response = await fetch('/API/getSanPhamDataById',
                {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(id)
                });
            if (!response.ok) {
                throw new Error(errorMessage);
            }
            const data = await response.json();
            if (data) {
                return data
            }
            else {
                alert('Không thể lấy dữ liệu sản phẩm, vui lòng thử lại.');
            }
        } catch (error) {
            console.error('Lỗi khi lấy dữ liệu sản phẩm:', error);
            alert('Không thể lấy dữ liệu sản phẩm, vui lòng thử lại.');
        }
    }


    async function callApiDeleteSP(id) {
        try {
            // Gửi request DELETE
            const response = await fetch(`/API/deleteSP${encodeURIComponent(id)}`, {
                method: 'DELETE'
            });

            // Đọc body (nếu có trả JSON)
            let data = null;
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                data = await response.json();
            }

            // Kiểm tra response
            if (!response.ok) {
                const errorMessage = (data && data.message) || 'Xóa hàng hoá thất bại!';
                throw new Error(errorMessage);
            }


            // TODO: nếu m có hàm reload lại bảng thì gọi ở đây
            // loadDanhMuc();

        } catch (error) {
            alert('Lỗi: ' + error.message);
        }
    }

});



$(async function () {
    await appRealtimeList.initEntityTable({
        key: 'HangHoa',                  // key dùng cho SignalR: NotifyReloadAsync("DanhMuc")
        apiUrl: '/API/get-all-SP',       // API lấy dữ liệu
        tableId: 'sampleTable',
        tbodyId: 'tbodySanPham',
        buildRow: sp => {
            return `
                <tr  data-id="${sp.id}" data-ten="${sp.ten}">
                    <td>${sp.id}</td>
                    <td>${sp.ten}</td>
                    <td>
                        ${sp.danhMucs.map(dm => `
                            ${dm}
                        `).join(', ')}
                    </td>

                    <td>${sp.nhanHieu}</td>

                    <td class="text-center">
                        <button class="btn btn-info btn-sm me-1 btn-edit-khoi" data-id="${sp.id}" title="Sửa">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn btn-danger btn-sm btn-delete-khoi" data-id="${sp.id}" title="Xóa">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </td>
                </tr>
            `;
        }
    });
});