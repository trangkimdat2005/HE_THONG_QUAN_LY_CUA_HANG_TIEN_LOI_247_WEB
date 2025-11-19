document.addEventListener('DOMContentLoaded', function () {
    console.log('=== DANH SÁCH ĐƠN VỊ ĐO LƯỜNG PAGE LOADED ===');

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const mainRow = document.getElementById('main-row-container');
    const listCol = document.getElementById('list-col');
    const formCol = document.getElementById('form-col');
    const showAddFormBtn = document.getElementById('show-add-form-btn');

    const donViForm = document.getElementById('don-vi-form');
    const formTitle = document.getElementById('form-title');

    // Input fields
    const inputId = document.getElementById('form-don-vi-id');
    const inputTen = document.getElementById('form-don-vi-ten');
    const inputKyHieu = document.getElementById('form-don-vi-kyhieu');

    // Buttons
    const btnSubmit = document.getElementById('btn-submit');
    const btnCancel = document.getElementById('btn-cancel');
    const btnEditMode = document.getElementById('btn-edit-mode');

    let currentMode = null; // 'add', 'edit', 'view'
    let currentData = null;

    // --- HÀM MỞ FORM ---
    function openForm(mode, data = null) {
        console.log('openForm:', mode, data);
        currentMode = mode;
        currentData = data;

        // Co bảng, mở form
        listCol.classList.remove('col-md-12');
        listCol.classList.add('col-md-8');
        formCol.classList.remove('col-md-0');
        formCol.classList.add('col-md-4');
        mainRow.classList.add('form-open');
        showAddFormBtn.style.display = 'none';

        donViForm.classList.remove('form-readonly');

        if (mode === 'add') {
            formTitle.innerText = 'Thêm Đơn Vị Mới';
            donViForm.reset();

            // Lấy ID tự động
            callApiGetNextIdDV();

            inputId.readOnly = true;
            btnSubmit.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Thêm';
            btnSubmit.style.display = 'inline-block';
            btnCancel.innerHTML = '<i class="bi bi-x-circle-fill me-2"></i>Huỷ';
            btnCancel.style.display = 'inline-block';
            btnEditMode.style.display = 'none';
        }
        else if (mode === 'edit' || mode === 'view') {
            // Điền dữ liệu
            inputId.value = data.id;
            inputTen.value = data.ten;
            inputKyHieu.value = data.kyHieu;

            if (mode === 'view') {
                formTitle.innerText = 'Chi Tiết Đơn Vị';
                donViForm.classList.add('form-readonly');
                btnSubmit.style.display = 'none';
                btnCancel.innerText = 'Đóng';
                btnCancel.style.display = 'inline-block';
                btnEditMode.style.display = 'inline-block';
            } else {
                formTitle.innerText = 'Sửa Đơn Vị';
                inputId.readOnly = true;
                btnSubmit.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Lưu';
                btnSubmit.style.display = 'inline-block';
                btnCancel.innerText = 'Huỷ';
                btnCancel.style.display = 'inline-block';
                btnEditMode.style.display = 'none';
            }
        }
    }

    // --- HÀM ĐÓNG FORM ---
    function closeForm() {
        listCol.classList.remove('col-md-8');
        listCol.classList.add('col-md-12');
        formCol.classList.remove('col-md-4');
        formCol.classList.add('col-md-0');
        mainRow.classList.remove('form-open');
        showAddFormBtn.style.display = 'block';

        donViForm.classList.remove('form-readonly');
        donViForm.reset();
        currentData = null;
        currentMode = null;
    }

    // --- GÁN SỰ KIỆN ---
    showAddFormBtn.addEventListener('click', function (e) {
        e.preventDefault();
        openForm('add');
    });

    btnCancel.addEventListener('click', function (e) {
        e.preventDefault();
        closeForm();
    });

    btnEditMode.addEventListener('click', function () {
        if (currentData) {
            openForm('edit', currentData);
        }
    });

    btnSubmit.addEventListener('click', async function (e) {
        e.preventDefault();

        if (currentMode === 'add') {
            await callApiAddDV();
        } else if (currentMode === 'edit') {
            await callApiEditDV();
        }
    });

    // Xử lý click vào bảng
    document.querySelector('#sampleTable tbody').addEventListener('click', function (e) {
        const clickedRow = e.target.closest('tr');
        if (!clickedRow) return;

        // Lấy data
        const data = {
            id: clickedRow.dataset.id,
            ten: clickedRow.dataset.ten,
            kyHieu: clickedRow.dataset.kyhieu
        };

        // Xử lý nút XÓA
        if (e.target.closest('.btn-delete-khoi')) {
            e.stopPropagation();

            if (confirm(`Bạn có chắc muốn xoá đơn vị "${data.ten}" không?`)) {
                callApiDeleteDV(data.id);
            }
            return;
        }

        // Xử lý nút EDIT
        if (e.target.closest('.btn-edit-khoi')) {
            e.stopPropagation();
            openForm('edit', data);
            return;
        }

        // Click vào dòng để XEM
        openForm('view', data);
    });

    // --- API CALLS ---

    // Lấy ID tiếp theo
    async function callApiGetNextIdDV() {
        const dataToSend = {
            prefix: "DV",
            totalLength: 6
        };
        try {
            const response = await fetch('/API/get-next-id-DV', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dataToSend)
            });
            const data = await response.json();
            if (data && data.nextId) {
                inputId.value = data.nextId;
            } else {
                alert('Không thể lấy mã đơn vị, vui lòng thử lại.');
            }
        } catch (error) {
            console.error('Lỗi khi lấy mã đơn vị:', error);
            alert('Không thể lấy mã đơn vị, vui lòng thử lại.');
        }
    }

    // Thêm đơn vị
    async function callApiAddDV() {
        try {
            const duLieu = {
                Id: inputId.value,
                Ten: inputTen.value,
                KyHieu: inputKyHieu.value
            };

            // Validate
            if (!duLieu.Ten) {
                alert('Vui lòng nhập tên đơn vị!');
                return;
            }

            if (!duLieu.KyHieu) {
                alert('Vui lòng nhập ký hiệu!');
                return;
            }

            const response = await fetch('/API/add-DV', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
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
            alert('Lỗi khi thêm đơn vị: ' + error.message);
        }
    }

    // Sửa đơn vị
    async function callApiEditDV() {
        try {
            const duLieu = {
                Id: inputId.value,
                Ten: inputTen.value,
                KyHieu: inputKyHieu.value,
                IsDelete: false
            };

            // Validate
            if (!duLieu.Ten) {
                alert('Vui lòng nhập tên đơn vị!');
                return;
            }

            if (!duLieu.KyHieu) {
                alert('Vui lòng nhập ký hiệu!');
                return;
            }

            const response = await fetch('/API/edit-DV', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
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
            alert('Lỗi khi sửa đơn vị: ' + error.message);
        }
    }

    // Xóa đơn vị
    async function callApiDeleteDV(id) {
        try {
            const response = await fetch(`/API/delete-DV/${encodeURIComponent(id)}`, {
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
            const response = await fetch('/API/get-all-DV');
            const data = await response.json();

            const tbody = document.querySelector('#sampleTable tbody');
            tbody.innerHTML = '';

            data.forEach(dv => {
                const row = document.createElement('tr');
                row.dataset.id = dv.id;
                row.dataset.ten = dv.ten;
                row.dataset.kyhieu = dv.kyHieu;

                row.innerHTML = `
                    <td>${dv.id}</td>
                    <td>${dv.ten}</td>
                    <td>${dv.kyHieu}</td>
                    <td class="text-center">
                        <a class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                           href="#" 
                           data-id="${dv.id}"
                           title="Sửa">
                            <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger btn-sm btn-delete-khoi" 
                           href="#" 
                           data-id="${dv.id}" 
                           title="Xóa">
                            <i class="fas fa-trash-alt"></i>
                        </a>
                    </td>`;

                tbody.appendChild(row);
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
    console.log('=== INITIALIZING SIGNALR FOR DON VI ===');

    await appRealtimeList.initEntityTable({
        key: 'DonVi',
        apiUrl: '/API/get-all-DV',
        tableId: 'sampleTable',
        tbodyId: 'sampleTable tbody',
        buildRow: dv => {
            return `
                <tr data-id="${dv.id}" data-ten="${dv.ten}" data-kyhieu="${dv.kyHieu}">
                    <td>${dv.id}</td>
                    <td>${dv.ten}</td>
                    <td>${dv.kyHieu}</td>
                    <td class="text-center">
                        <a class="btn btn-info btn-sm me-1 btn-edit-khoi" 
                           href="#" 
                           data-id="${dv.id}"
                           title="Sửa">
                            <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger btn-sm btn-delete-khoi" 
                           href="#" 
                           data-id="${dv.id}" 
                           title="Xóa">
                            <i class="fas fa-trash-alt"></i>
                        </a>
                    </td>
                </tr>`;
        }
    });

    console.log('✅ SignalR initialized for DonVi');
});






