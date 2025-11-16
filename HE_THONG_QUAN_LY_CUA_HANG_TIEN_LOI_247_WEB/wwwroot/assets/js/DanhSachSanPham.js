
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
            selectDanhMuc.val(null).trigger('change');
            selectDonVi.val('DV_CHAI').trigger('change');

            dt = new DataTransfer();
            formImagesUpload[0].files = dt.files;
            renderPreview();
            callApiGetNextId(); // Gọi API để lấy mã sản phẩm tiếp theo

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
        data.price = parseFloat(data.price); // Chuyển đổi giá trị số

        callApiGetDataById(data.id);

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
        if (!$('#form-product-name').val().trim()) { alert('Nhập tên SP!'); return; }
        callApiAddSP();
        console.log(dt.files);
        closeForm();
    });
    //Sự kiện Trang Kim Đạt làm



    async function callApiAddSP() {
        try {
            // Lấy dữ liệu từ form
            const imagesUpload = document.getElementById('form-images-upload').files;
            const productId = document.getElementById('form-product-id').value;
            const productName = document.getElementById('form-product-name').value;
            const brand = document.getElementById('form-select-nhan-hieu').value;
            const categories = Array.from(document.getElementById('form-select-danh-muc').selectedOptions).map(option => option.value);
            const unit = document.getElementById('form-select-don-vi').value;
            const conversionFactor = document.getElementById('form-select-he-so-quy-doi').value;
            const price = document.getElementById('form-product-price').value;
            const description = document.getElementById('form-product-description').value;
            const statusElement = document.querySelector('input[name="trangThaiSP"]:checked');
            const status = statusElement ? statusElement.value : '';

            if (!productName || productName.trim() === '') {
                alert('Vui lòng nhập tên sản phẩm!');
                return;
            }

            if (!price || price.trim() === '') {
                alert('Vui lòng nhập giá bán!');
                return;
            }

            if (!brand || brand === '') {
                alert('Vui lòng chọn nhãn hiệu!');
                return;
            }

            if (!unit || unit === '') {
                alert('Vui lòng chọn đơn vị!');
                return;
            }

            if (!status || status === '') {
                alert('Vui lòng chọn trạng thái sản phẩm!');
                return;
            }

            // Parse số
            const conversionFactorNum = parseFloat(conversionFactor);
            const priceNum = parseFloat(price);

            // Tạo FormData
            const formData = new FormData();

            // Thêm tất cả ảnh - QUAN TRỌNG: tên phải là "ImagesUpload"
            for (let i = 0; i < imagesUpload.length; i++) {
                formData.append('ImagesUpload', imagesUpload[i]);
            }

            // Thêm các field khác - TÊN PHẢI KHỚP VỚI C# (PascalCase)
            formData.append('ProductId', productId.trim());
            formData.append('ProductName', productName.trim());
            formData.append('Brand', brand);

            // Thêm categories - mỗi category là 1 entry riêng
            if (categories.length > 0) {
                categories.forEach(category => {
                    formData.append('Categories', category);
                });
            }

            formData.append('Unit', unit);
            formData.append('ConversionFactor', conversionFactorNum.toString());
            formData.append('Price', priceNum.toString());
            formData.append('Description', description || '');
            formData.append('Status', status);


            // Gửi request
            const response = await fetch('/API/add-SP', {
                method: 'POST',
                body: formData
                // KHÔNG thêm Content-Type header
            });

            // Kiểm tra response
            if (!response.ok) {
                const contentType = response.headers.get("content-type");
                let errorMessage = `HTTP error! Status: ${response.status}`;

                console.log('Response content-type:', contentType);

                if (contentType && contentType.includes("application/json")) {
                    const errorData = await response.json();
                    errorMessage = errorData.message || errorMessage;
                    console.error('===== ERROR RESPONSE (JSON) =====');
                    console.error(errorData);
                    console.error('=================================');
                } else {
                    const errorText = await response.text();
                    console.error('===== ERROR RESPONSE (TEXT) =====');
                    console.error(errorText);
                    console.error('=================================');
                    errorMessage = errorText || errorMessage;
                }

                throw new Error(errorMessage);
            }

            const data = await response.json();
            console.log('===== SUCCESS RESPONSE =====');
            console.log(data);
            console.log('============================');

            // Hiển thị thông báo thành công
            alert(data.message || 'Thêm sản phẩm thành công!');



            return data;

        } catch (error) {
            showLoading(false);
            console.error('===== EXCEPTION =====');
            console.error('Message:', error.message);
            console.error('Stack:', error.stack);
            console.error('=====================');
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

});




$(async function () {
    await appRealtimeList.initEntityTable({
        key: 'SanPham',              
        apiUrl: '/API/get-all-SP',       
        tableId: 'sampleTable',
        tbodyId: 'tbody-san-pham',
        buildRow: sp => {
            return `
                <tr data-id ="${sp.Id}">
                    <td>${sp.id}</td>
                    <td>${sp.ten}</td>
                    <td>
                        ${sp.danhMucs.map(dm => {
                            return `<span class="badge bg-info">${dm.danhMucTen}</span>`;
                        }).join(' ')} <!-- Nối các danh mục với khoảng trắng -->
                    </td>
                    <td>${sp.nhanHieu}</td>
                    <td>${sp.sanPhamDonVi[0].donVi}</td>
                    <td>${sp.sanPhamDonVi[0].giaBan}</td>
                    <td><span class="badge bg-success">${sp.sanPhamDonVi[0].trangThai}</span></td>
                    <td class="text-center">
                        <button class="btn btn-info btn-sm me-1 btn-edit-khoi" data-id ="${sp.Id}" title="Sửa">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn btn-danger btn-sm btn-delete-khoi" title="Xóa">
                            <i class="fas fa-trash-alt"></i>
                        </button>
                    </td>
                </tr>
            `
        }
    });
});


async function callApiGetDataById(id) {
    try {
        const response = await fetch(`/API/GetDataById?id=${encodeURIComponent(id)}`, {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) {
            throw new Error('Lỗi khi gọi API');
        }

        const data = await response.json();
        nhapDuLieu(data); // Gọi hàm nhập dữ liệu từ API
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu:', error);
        alert('Không thể lấy dữ liệu, vui lòng thử lại.');
    }
}


function nhapDuLieu(data) {
    // Điền vào các trường trong form
    document.getElementById('form-product-id').value = data.Id;
    document.getElementById('form-product-name').value = data.Ten;

    // Điền vào danh sách nhãn hiệu
    const brandSelect = document.getElementById('form-select-nhan-hieu');
    const brandOption = Array.from(brandSelect.options).find(option => option.value === data.NhanHieuId);
    if (brandOption) brandOption.selected = true;

    // Điền vào danh sách danh mục
    const categorySelect = document.getElementById('form-select-danh-muc');
    Array.from(categorySelect.options).forEach(option => {
        // Kiểm tra xem danh mục có trong SanPhamDanhMucs không
        if (data.SanPhamDanhMucs.some(sdm => sdm.DanhMucId === option.value)) {
            option.selected = true;
        }
    });

    // Điền vào đơn vị cơ sở
    const unitSelect = document.getElementById('form-select-don-vi');
    Array.from(unitSelect.options).forEach(option => {
        // Kiểm tra xem đơn vị có trong SanPhamDonVis không
        if (data.SanPhamDonVis.some(sd => sd.DonViId === option.value)) {
            option.selected = true;
        }
    });

    // Điền vào hệ số quy đổi
    document.getElementById('form-select-he-so-quy-doi').value = data.ConversionRate;

    // Điền vào giá bán
    document.getElementById('form-product-price').value = data.Price;

    // Điền vào mô tả sản phẩm
    document.getElementById('form-product-description').value = data.MoTa;

    // Điền vào trạng thái
    const statusRadios = document.getElementsByName('trangThaiSP');
    statusRadios.forEach(radio => {
        if (radio.value === (data.IsDelete ? 'NgungKinhDoanh' : 'ConHang')) {
            radio.checked = true;
        }
    });

    // Hiển thị hình ảnh
    const previewContainer = document.getElementById('images-preview-container');
    previewContainer.innerHTML = ''; // Xóa các hình ảnh cũ trước khi thêm mới
    data.Images.forEach(image => {
        const imgPreview = `<div class="preview-image"><img src="${image}" class="img-thumbnail" width="100"></div>`;
        previewContainer.innerHTML += imgPreview;
    });
    previewContainer.style.display = 'flex'; // Hiển thị hình ảnh đã tải lên
}

