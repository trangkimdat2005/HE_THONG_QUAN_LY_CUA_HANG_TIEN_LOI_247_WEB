
$(function () {

    // --- 1. KHỞI TẠO CÁC THÀNH PHẦN ---
    $('#form-select-nhan-hieu').select2({ width: '100%', dropdownParent: $('#product-form-col') });
    $('#form-select-don-vi').select2({ width: '100%', dropdownParent: $('#product-form-col') });
    $('#form-select-danh-muc').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều danh mục',
        dropdownParent: $('#product-form-col')
    });

    // --- 2. TRUY XUẤT PHẦN TỬ DOM ---
    const mainRow = $('#main-row-container');
    const showFormBtn = $('#show-add-form-btn');
    const productForm = $('#product-form');
    const formTitle = $('#form-title');

    const formImagesUpload = $('#form-images-upload');
    const uploadPlaceholder = $('#upload-placeholder');
    const imagesPreviewContainer = $('#images-preview-container');

    // Các trường input
    const inputId = $('#form-product-id');
    const inputName = $('#form-product-name');
    const selectNhanHieu = $('#form-select-nhan-hieu');
    const selectDanhMuc = $('#form-select-danh-muc');
    const selectDonVi = $('#form-select-don-vi');
    const inputPrice = $('#form-product-price');
    const inputDescription = $('#form-product-description');

    // Nút bấm
    const btnSubmit = $('#btn-submit');
    const btnCancel = $('#btn-cancel');
    const btnEditMode = $('#btn-edit-mode');

    // --- 3. LOGIC XỬ LÝ NHIỀU ẢNH ---
    let dt = new DataTransfer();









    // Hàm vẽ lại vùng preview ảnh
    function renderPreview() {
        imagesPreviewContainer.empty();

        if (dt.files.length === 0) {
            // Chưa có ảnh: Hiện placeholder lớn ban đầu
            uploadPlaceholder.show();
            imagesPreviewContainer.hide();
        } else {
            // Đã có ảnh: Ẩn placeholder lớn, hiện vùng preview
            uploadPlaceholder.hide();
            imagesPreviewContainer.show().css('display', 'flex');

            // 1. Vẽ các ảnh thumbnail
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
                    e.preventDefault(); e.stopPropagation();
                    removeFile($(this).data('file-name'));
                });

                wrapper.append(img).append(removeBtn);
                imagesPreviewContainer.append(wrapper);
            });

            // 2. Vẽ thêm nút "dấu cộng" vào cuối (nếu không phải chế độ xem readonly)
            if (!productForm.hasClass('form-readonly')) {
                const addBtn = $('<div>').addClass('mini-add-btn')
                    .html('<i class="fas fa-plus"></i><span>Thêm ảnh</span>');

                // Khi click vào nút này thì mở hộp thoại chọn file
                addBtn.on('click', function () {
                    formImagesUpload.click();
                });

                imagesPreviewContainer.append(addBtn);
            }
        }
    }

    function removeFile(fileName) {
        const newDt = new DataTransfer();
        Array.from(dt.files).forEach(file => { if (file.name !== fileName) newDt.items.add(file); });
        dt = newDt;
        formImagesUpload[0].files = dt.files;
        renderPreview();
    }

    formImagesUpload.on('change', function () {
        Array.from(this.files).forEach(file => { if (file.type.startsWith('image/')) dt.items.add(file); });
        this.files = dt.files;
        renderPreview();
    });

    // --- 4. HÀM MỞ FORM ---
    function openForm(mode, data = null) {
        mainRow.addClass('form-open');
        // showFormBtn.hide(); // <-- DÒNG NÀY ĐÃ ĐƯỢC COMMENT LẠI ĐỂ GIỮ NÚT HIỂN THỊ

        productForm.removeClass('form-readonly');

        if (mode === 'add') {
            formTitle.text('Thêm Sản Phẩm Mới');
            productForm[0].reset();
            selectNhanHieu.val('').trigger('change');
            selectDonVi.val('').trigger('change');

            dt = new DataTransfer();
            formImagesUpload[0].files = dt.files;
            renderPreview();

            inputId.val('Tự động tạo').prop('readonly', true);
            btnSubmit.html('<i class="bi bi-check-circle-fill me-2"></i>Thêm').show();
            btnCancel.html('<i class="bi bi-x-circle-fill me-2"></i>Huỷ').show();
            btnEditMode.hide();
            // Khóa select2 (khi sử dụng Select2)
            $('#form-select-nhan-hieu').prop('disabled', false).trigger('change');
            $('#form-select-don-vi').prop('disabled', false).trigger('change');
            $('#form-select-danh-muc').prop('disabled', false).trigger('change');











        }
        else {
            inputId.val(data.id);
            inputName.val(data.name);
            inputPrice.val(data.price);
            inputDescription.val(data.moTa);
            $(`input[name="trangThaiSP"][value="${data.trangThai}"]`).prop('checked', true);
            selectNhanHieu.val(data.nhanHieuId).trigger('change');
            selectDonVi.val(data.donViId).trigger('change');
            let danhMucIds = typeof data.danhMucIds === 'string' ? JSON.parse(data.danhMucIds) : data.danhMucIds;
            selectDanhMuc.val(danhMucIds).trigger('change');

            dt = new DataTransfer();
            formImagesUpload[0].files = dt.files;
            renderPreview();

            if (mode === 'view') {
                formTitle.text('Chi Tiết Sản Phẩm');
                productForm.addClass('form-readonly');
                btnSubmit.hide();
                btnCancel.text('Đóng').show();
                btnEditMode.show();
                // Khóa select2 (khi sử dụng Select2)
                $('#form-select-nhan-hieu').prop('disabled', true).trigger('change');
                $('#form-select-don-vi').prop('disabled', true).trigger('change');
                $('#form-select-danh-muc').prop('disabled', true).trigger('change');
            } else {
                formTitle.text('Sửa Sản Phẩm');
                inputId.prop('readonly', true);
                btnSubmit.html('<i class="bi bi-check-circle-fill me-2"></i>Lưu').show();
                btnCancel.text('Huỷ').show();
                $('#form-select-nhan-hieu').prop('disabled', false).trigger('change');
                $('#form-select-don-vi').prop('disabled', false).trigger('change');
                $('#form-select-danh-muc').prop('disabled', false).trigger('change');
                btnEditMode.hide();


            }
        }
    }

    // --- 5. HÀM ĐÓNG FORM ---
    function closeForm() {
        mainRow.removeClass('form-open');
        // showFormBtn.show(); // <-- DÒNG NÀY CŨNG ĐƯỢC COMMENT LẠI

        setTimeout(() => {
            productForm.removeClass('form-readonly');
            productForm[0].reset();
            selectNhanHieu.val('').trigger('change');
            selectDanhMuc.val(null).trigger('change');
            dt = new DataTransfer();
            formImagesUpload[0].files = dt.files;
            renderPreview();
        }, 400);
    }

    // --- 6. CÁC SỰ KIỆN KHÁC ---
    showFormBtn.on('click', function (e) { e.preventDefault(); openForm('add'); });
    btnCancel.on('click', function (e) { e.preventDefault(); closeForm(); });

    $('#sampleTable tbody').on('click', 'tr', function (e) {
        const clickedRow = $(this);
        const target = $(e.target);
        const data = clickedRow.data();

        callApiGetDataById(data.sanPhamId, data.donViId);

        if (target.closest('.btn-delete-khoi').length) {
            e.stopPropagation();
            if (confirm(`Bạn có chắc muốn xoá sản phẩm "${data.name}" không?`)) {
                clickedRow.fadeOut(300, function () { $(this).remove(); });
                closeForm();
            }
            return;
        }
        if (target.closest('.btn-edit-khoi').length) {
            e.stopPropagation();
            openForm('edit', data);
            return;
        }
        openForm('view', data);
    });

    btnEditMode.on('click', function (e) {
        e.preventDefault();
        formTitle.text('Sửa Sản Phẩm');
        productForm.removeClass('form-readonly');
        inputId.prop('readonly', true);
        btnSubmit.html('<i class="bi bi-check-circle-fill me-2"></i>Lưu').show();
        btnCancel.text('Huỷ').show();
        // Khóa select2 (khi sử dụng Select2)
        $('#form-select-nhan-hieu').prop('disabled', false).trigger('change');
        $('#form-select-don-vi').prop('disabled', false).trigger('change');
        $('#form-select-danh-muc').prop('disabled', false).trigger('change');
        btnEditMode.hide();
    });

    btnSubmit.on('click', function (e) {
        e.preventDefault();
        callApiAddSP();
        console.log(dt.files);
        closeForm();
    });
    //Sự kiện Trang Kim Đạt làm



    async function callApiAddSP() {
        try {
            // Lấy dữ liệu từ form
            const imagesUpload = document.getElementById('form-images-upload').files;

            // Đây KHÔNG PHẢI là nhãn hiệu, mà là SẢN PHẨM
            const productId = document.getElementById('form-select-hang-hoa').value;

            const unit = document.getElementById('form-select-don-vi').value;
            const conversionFactor = document.getElementById('form-conversion-factor')?.value || "1";
            const price = document.getElementById('form-product-price').value;
            const description = document.getElementById('form-product-description').value;

            const statusElement = document.querySelector('input[name="trangThaiSP"]:checked');
            const status = statusElement ? statusElement.value : '';

            // Validate
            if (!productId) {
                alert("Vui lòng chọn sản phẩm!");
                return;
            }

            if (!unit) {
                alert("Vui lòng chọn đơn vị!");
                return;
            }

            if (!price) {
                alert("Vui lòng nhập giá bán!");
                return;
            }

            if (!status) {
                alert("Vui lòng chọn trạng thái!");
                return;
            }

            // Parse số
            const conversionFactorNum = parseFloat(conversionFactor);
            const priceNum = parseFloat(price);

            // Tạo FormData
            const formData = new FormData();

            // Thêm ảnh
            for (let i = 0; i < imagesUpload.length; i++) {
                formData.append("ImagesUpload", imagesUpload[i]);
            }

            // Các trường FORM gửi lên API
            formData.append("SanPhamId", productId);
            formData.append("DonViId", unit);
            formData.append("HeSoQuyDoi", conversionFactorNum.toString());
            formData.append("GiaBan", priceNum.toString());
            formData.append("MoTa", description || "");
            formData.append("TrangThai", status);

            // Fetch
            const response = await fetch('/API/addSPDV', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                const err = await response.text();
                throw new Error(err);
            }

            const data = await response.json();

            alert(data.message || "Thêm thành công!");
            return data;

        } catch (error) {
            console.error(error);
            alert("Lỗi: " + error.message);
        }
    }



    async function callApiGetDataById(sanPhamId, donViId) {
        try {
            const response = await fetch(`/API/getSanPhamDonViDataById?sanPhamId=${sanPhamId}&donViId=${donViId}`, {
                method: "POST"
            });

            if (!response.ok) {
                const err = await response.text();
                throw new Error(err);
            }

            const data = await response.json();

            // ---------------------------
            // GÁN DỮ LIỆU VÀO FORM
            // ---------------------------

            // 1) Sản phẩm
            document.getElementById("form-select-hang-hoa").value = data.sanPhamId;

            // 2) Đơn vị
            document.getElementById("form-select-don-vi").value = data.donViId;

            // 3) Hệ số quy đổi
            document.getElementById("form-conversion-factor").value = data.heSoQuyDoi;

            // 4) Giá bán
            document.getElementById("form-product-price").value = data.giaBan;

            // 5) Trạng thái
            document.querySelectorAll('input[name="trangThaiSP"]').forEach(r => {
                r.checked = (r.value === data.trangThai);
            });

            // 6) Ảnh sản phẩm
            await loadImagesFromApi(data.anhs);

            console.log("Dữ liệu đã load vào form:", data);

        } catch (error) {
            console.error("Lỗi load sản phẩm:", error);
            alert("Lỗi: " + error.message);
        }
    }




});




$(async function () {
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
                </tr>
            `;
        }

    });
});








// =======================================================
// MODULE QUẢN LÝ ẢNH UPLOAD + LOAD TỪ API
// =======================================================

const productForm = $("#product-form");
const formImagesUpload = $("#form-images-upload")[0];
const uploadPlaceholder = $("#upload-placeholder");
const imagesPreviewContainer = $("#images-preview-container");

// DataTransfer để quản lý file ảnh (cho phép thêm/xoá)
let dt = new DataTransfer();

// =========================
// Convert base64 → File
// =========================
function base64ToFile(base64, fileName) {
    return fetch(base64)
        .then(res => res.arrayBuffer())
        .then(buf => new File([buf], fileName, { type: getMimeType(base64) }));
}

function getMimeType(base64) {
    const result = base64.match(/data:(.*);base64,/);
    return result ? result[1] : "image/png";
}

// =========================
// Render lại toàn bộ ảnh
// =========================
function renderPreview() {
    imagesPreviewContainer.empty();

    if (dt.files.length === 0) {
        uploadPlaceholder.show();
        imagesPreviewContainer.hide();
        return;
    }

    uploadPlaceholder.hide();
    imagesPreviewContainer.show().css("display", "flex");

    // Duyệt từng file trong dt.files
    Array.from(dt.files).forEach(file => {
        const wrapper = $('<div>').addClass('preview-image-wrapper');

        const img = $('<img>')
            .addClass('preview-image-item');

        // Hiển thị ảnh
        const reader = new FileReader();
        reader.onload = (e) => img.attr('src', e.target.result);
        reader.readAsDataURL(file);

        // Nút xoá
        const removeBtn = $('<span>')
            .addClass('btn-remove-image')
            .html('<i class="fas fa-times"></i>')
            .data('file-name', file.name);

        removeBtn.on("click", function (e) {
            e.preventDefault();
            e.stopPropagation();
            removeFile($(this).data("file-name"));
        });

        wrapper.append(img).append(removeBtn);
        imagesPreviewContainer.append(wrapper);
    });

    // Thêm nút dấu +
    if (!productForm.hasClass("form-readonly")) {
        const addBtn = $('<div>')
            .addClass('mini-add-btn')
            .html('<i class="fas fa-plus"></i><span>Thêm ảnh</span>')
            .on('click', () => formImagesUpload.click());

        imagesPreviewContainer.append(addBtn);
    }
}

// =========================
// Xoá ảnh khỏi dt + UI
// =========================
function removeFile(fileName) {
    const newDT = new DataTransfer();

    for (let file of dt.files) {
        if (file.name !== fileName) {
            newDT.items.add(file);
        }
    }

    dt = newDT;

    // Gán lại file cho input
    formImagesUpload.files = dt.files;

    // Render lại giao diện
    renderPreview();
}

// =========================
// Khi user chọn ảnh mới từ máy
// =========================
$("#form-images-upload").on("change", function () {
    for (let file of this.files) {
        dt.items.add(file);
    }

    formImagesUpload.files = dt.files;

    renderPreview();
});

// =========================
// Hàm load ảnh từ API vào form
// =========================
async function loadImagesFromApi(base64List) {
    dt = new DataTransfer(); // reset toàn bộ file cũ

    if (base64List && base64List.length > 0) {
        for (let i = 0; i < base64List.length; i++) {
            const file = await base64ToFile(base64List[i], `api_img_${i}.png`);
            dt.items.add(file);
        }
    }

    // Gán lại vào input file
    formImagesUpload.files = dt.files;

    // Render preview cho đồng bộ UI
    renderPreview();
}
