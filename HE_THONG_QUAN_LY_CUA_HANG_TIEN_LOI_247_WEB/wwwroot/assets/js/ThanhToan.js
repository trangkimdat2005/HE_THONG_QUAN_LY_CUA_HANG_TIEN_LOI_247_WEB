// Vô hiệu hóa DataTables warning
if (typeof $.fn.dataTable !== 'undefined') {
    $.fn.dataTable.ext.errMode = 'none';
}

document.addEventListener('DOMContentLoaded', function () {
    console.log('=== THANH TOAN PAGE LOADED ===');

    // Destroy DataTable nếu đã được init
    if (typeof $.fn.DataTable !== 'undefined' && $.fn.DataTable.isDataTable('#sampleTable')) {
        $('#sampleTable').DataTable().destroy();
    }

    // === TRUY XUẤT PHẦN TỬ DOM (KHỚP VỚI VIEW HIỆN TẠI) ===
    const mainRow = document.getElementById('main-content-row');
    const listCol = document.getElementById('list-column');
    const formCol = document.getElementById('form-column');

    const btnShowAddForm = document.getElementById('btn-show-add-form');
    const btnCloseForm = document.getElementById('btn-close-form');
    const btnCancelForm = document.getElementById('btn-cancel-form');
    const btnSaveForm = document.getElementById('btn-save-form');

    // Form
    const form = document.getElementById('payment-channel-form');
    const formTitle = document.getElementById('form-title');
    const formId = document.getElementById('form-id');
    const formTenKenh = document.getElementById('form-tenKenh');
    const formLoaiKenh = document.getElementById('form-loaiKenh');
    const formPhiGiaoDich = document.getElementById('form-phiGiaoDich');
    const formTrangThai = document.getElementById('form-trangThai');
    const formCauHinh = document.getElementById('form-cauHinh');

    // Bảng
    const tableBody = document.getElementById('tbody-kenh-thanh-toan');

    let isEditMode = false;

    // === HÀM MỞ/ĐÓNG FORM ===
    const openForm = (mode = 'add') => {
        mainRow.classList.add('form-active');
        listCol.classList.remove('col-lg-12');
        listCol.classList.add('col-lg-8');
        formCol.classList.add('active');

        if (mode === 'add') {
            isEditMode = false;
            formTitle.textContent = 'Thêm Kênh Mới';
            formId.readOnly = false;
            form.reset();
            formPhiGiaoDich.value = '0.00';
            formTrangThai.value = 'Active';
            callApiGetNextIdKenh();
        } else if (mode === 'edit') {
            isEditMode = true;
            formTitle.textContent = 'Sửa Kênh Thanh Toán';
            formId.readOnly = true;
        }
    };

    const closeForm = () => {
        mainRow.classList.remove('form-active');
        listCol.classList.remove('col-lg-8');
        listCol.classList.add('col-lg-12');
        formCol.classList.remove('active');
        form.reset();
        isEditMode = false;
    };

    // === GÁN SỰ KIỆN ===

    // 1. Nút "Thêm kênh mới"
    btnShowAddForm.addEventListener('click', () => {
        openForm('add');
    });

    // 2. Nút "Huỷ" và "X"
    btnCloseForm.addEventListener('click', closeForm);
    btnCancelForm.addEventListener('click', closeForm);

    // 3. Nút "Lưu"
    btnSaveForm.addEventListener('click', () => {
        if (form.checkValidity() === false) {
            alert('Vui lòng nhập đầy đủ thông tin bắt buộc.');
            return;
        }

        const duLieu = {
            Id: formId.value,
            TenKenh: formTenKenh.value,
            LoaiKenh: formLoaiKenh.value,
            PhiGiaoDich: parseFloat(formPhiGiaoDich.value) || 0,
            TrangThai: formTrangThai.value,
            CauHinh: formCauHinh.value.trim() || null
        };

        if (isEditMode) {
            callApiEditKenh(duLieu);
        } else {
            callApiAddKenh(duLieu);
        }
    });

    // 4. Gán sự kiện Sửa/Xóa cho tableBody
    tableBody.addEventListener('click', (e) => {
        const target = e.target;

        // Xử lý nút SỬA
        const editButton = target.closest('.btn-edit');
        if (editButton) {
            e.preventDefault();
            const id = editButton.dataset.id;
            callApiGetKenhById(id);
            return;
        }

        // Xử lý nút XOÁ
        const deleteButton = target.closest('.btn-delete-khoi');
        if (deleteButton) {
            e.preventDefault();
            const id = deleteButton.dataset.id;
            const row = deleteButton.closest('tr');
            const tenKenh = row.cells[1].textContent;

            if (confirm(`Bạn có chắc muốn xoá kênh "${tenKenh}" (Mã: ${id}) không?`)) {
                callApiDeleteKenh(id);
            }
            return;
        }
    });

    // === CÁC HÀM GỌI API ===

    // Lấy chi tiết kênh để Sửa
    async function callApiGetKenhById(id) {
        try {
            const response = await fetch(`/API/get-Kenh-by-id?id=${encodeURIComponent(id)}`);
            if (!response.ok) throw new Error('Không tìm thấy kênh.');

            const data = await response.json();

            formId.value = data.id;
            formTenKenh.value = data.tenKenh;
            formLoaiKenh.value = data.loaiKenh;
            formPhiGiaoDich.value = data.phiGiaoDich;
            formTrangThai.value = data.trangThai;
            formCauHinh.value = data.cauHinh || '';

            openForm('edit');

        } catch (error) {
            console.error('Lỗi khi lấy chi tiết kênh:', error);
            alert(error.message);
        }
    }

    // Thêm kênh mới
    async function callApiAddKenh(duLieu) {
        try {
            const response = await fetch('/API/add-Kenh', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
            });
            const result = await response.json();
            if (!response.ok) throw new Error(result.message || 'Thêm thất bại!');

            alert(result.message || 'Thêm kênh thanh toán thành công!');
            closeForm();
            
            // Fallback: Reload table manually
            await reloadTable();
        } catch (error) {
            console.error('Lỗi khi thêm kênh:', error);
            alert(error.message);
        }
    }

    // Sửa kênh
    async function callApiEditKenh(duLieu) {
        try {
            const response = await fetch('/API/edit-Kenh', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
            });
            const result = await response.json();
            if (!response.ok) throw new Error(result.message || 'Sửa thất bại!');

            alert(result.message || 'Cập nhật kênh thanh toán thành công!');
            closeForm();
            
            // Fallback: Reload table manually
            await reloadTable();
        } catch (error) {
            console.error('Lỗi khi sửa kênh:', error);
            alert(error.message);
        }
    }

    // Xoá kênh
    async function callApiDeleteKenh(id) {
        try {
            const response = await fetch(`/API/delete-Kenh/${encodeURIComponent(id)}`, {
                method: 'DELETE'
            });
            const result = await response.json();
            if (!response.ok) throw new Error(result.message || 'Xóa thất bại!');

            alert(result.message || 'Xóa kênh thanh toán thành công!');
            
            // Fallback: Reload table manually
            await reloadTable();
        } catch (error) {
            console.error('Lỗi khi xoá kênh:', error);
            alert(error.message);
        }
    }

    // Lấy ID tiếp theo
    async function callApiGetNextIdKenh() {
        const dataToSend = { prefix: "KTT", totalLength: 6 };
        try {
            const response = await fetch('/API/get-next-id-Kenh', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dataToSend)
            });
            const data = await response.json();
            if (data && data.nextId) {
                formId.value = data.nextId;
            } else {
                alert('Không thể lấy mã kênh mới.');
            }
        } catch (error) {
            console.error('Lỗi khi lấy mã kênh mới:', error);
        }
    }

    // Hàm reload bảng thủ công
    async function reloadTable() {
        try {
            console.log('Reloading table...');
            const response = await fetch('/API/get-all-Kenh');
            if (!response.ok) throw new Error('Không thể tải dữ liệu');
            
            const data = await response.json();
            
            const tbody = document.getElementById('tbody-kenh-thanh-toan');
            tbody.innerHTML = ''; // Xóa toàn bộ dữ liệu cũ
            
            data.forEach(kenh => {
                const trangThaiBadge = kenh.trangThai === 'Active'
                    ? '<span class="badge bg-success">Hoạt động</span>'
                    : '<span class="badge bg-danger">Không hoạt động</span>';

                const row = `
                    <tr>
                        <td>${kenh.id}</td>
                        <td>${kenh.tenKenh}</td>
                        <td>${kenh.loaiKenh}</td>
                        <td>${kenh.phiGiaoDich.toFixed(2)} %</td>
                        <td>${trangThaiBadge}</td>
                        <td class="text-center">
                            <button class="btn btn-info btn-sm me-1 btn-edit"
                               data-id="${kenh.id}"
                               title="Sửa">
                                <i class="fas fa-edit"></i>
                            </button>
                            <button class="btn btn-danger btn-sm btn-delete-khoi"
                               data-id="${kenh.id}"
                               title="Xóa">
                                <i class="fas fa-trash-alt"></i>
                            </button>
                        </td>
                    </tr>`;
                tbody.insertAdjacentHTML('beforeend', row);
            });
            
            console.log('Table reloaded successfully');
        } catch (error) {
            console.error(' Error reloading table:', error);
        }
    }
});

// === SIGNALR ===
$(async function () {
    
    if (typeof appRealtimeList !== 'undefined') {
        await appRealtimeList.initEntityTable({
            key: 'KenhThanhToan',
            apiUrl: '/API/get-all-Kenh',
            tableId: 'sampleTable',
            tbodyId: 'tbody-kenh-thanh-toan',
            buildRow: kenh => {
                const trangThaiBadge = kenh.trangThai === 'Active'
                    ? '<span class="badge bg-success">Hoạt động</span>'
                    : '<span class="badge bg-danger">Không hoạt động</span>';

                return `
                    <tr>
                        <td>${kenh.id}</td>
                        <td>${kenh.tenKenh}</td>
                        <td>${kenh.loaiKenh}</td>
                        <td>${kenh.phiGiaoDich.toFixed(2)} %</td>
                        <td>${trangThaiBadge}</td>
                        <td class="text-center">
                            <button class="btn btn-info btn-sm me-1 btn-edit"
                               data-id="${kenh.id}"
                               title="Sửa">
                                <i class="fas fa-edit"></i>
                            </button>
                            <button class="btn btn-danger btn-sm btn-delete-khoi"
                               data-id="${kenh.id}"
                               title="Xóa">
                                <i class="fas fa-trash-alt"></i>
                            </button>
                        </td>
                    </tr>`;
            }
        });
        console.log('SignalR initialized successfully for KenhThanhToan');
    } else {
        console.error('appRealtimeList is not defined!');
    }
});