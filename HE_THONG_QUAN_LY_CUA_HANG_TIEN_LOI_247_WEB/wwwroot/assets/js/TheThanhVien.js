document.addEventListener('DOMContentLoaded', function () {
    console.log('=== THẺ THÀNH VIÊN PAGE LOADED ===');

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const mainRow = document.getElementById('main-row-container');
    const listCol = document.getElementById('list-col');
    const formCol = document.getElementById('form-col');

    const showAddFormBtn = document.getElementById('show-add-form-btn');
    const tableBody = document.getElementById('sampleTable').getElementsByTagName('tbody')[0];

    // Nút Huỷ
    const btnSubmit = document.getElementById('btn-submit');
    const btnCancel = document.getElementById('btn-cancel');
    const btnEditMode = document.getElementById('btn-edit-mode');

    // Form inputs
    const formId = document.getElementById('form-id');
    const formSelectCustomer = document.getElementById('form-select-customer');
    const formSelectHang = document.getElementById('form-select-hang');
    const formDiem = document.getElementById('form-diem');
    const formNgayCap = document.getElementById('form-ngay-cap');
    
    // Fields
    const fieldChonKhachHang = document.getElementById('field-chon-khach-hang');
    const fieldXemKhachHang = document.getElementById('field-xem-khach-hang');
    const formTenKH = document.getElementById('form-ten-kh');
    const formSdtKH = document.getElementById('form-sdt-kh');
    const formTitle = document.getElementById('form-title');

    let currentMode = 'add'; // 'add', 'edit', 'view'
    let currentData = null;

    // Set ngày hiện tại
    if (formNgayCap) {
        const today = new Date().toISOString().split('T')[0];
        formNgayCap.value = today;
    }

    // --- HÀM MỞ FORM (CHUNG) ---
    function openForm(mode, data = null) {
        currentMode = mode;
        currentData = data;

        // 1. Co Bảng
        listCol.classList.remove('col-md-12');
        listCol.classList.add('col-md-8');

        // 2. Hiện Form
        formCol.classList.remove('col-md-0');
        formCol.classList.add('col-md-4');

        // 3. Thêm class điều khiển
        mainRow.classList.add('form-open');

        // 4. Ẩn nút "Thêm"
        showAddFormBtn.style.display = 'none';

        // 5. Xử lý theo mode
        if (mode === 'add') {
            formTitle.textContent = 'Cấp Thẻ Thành Viên Mới';
            
            // Reset form
            formId.value = 'Mã sẽ được tạo tự động';
            formSelectHang.value = 'Đồng';
            formDiem.value = '0';
            formNgayCap.value = new Date().toISOString().split('T')[0];

            // Hiển thị field chọn khách hàng
            fieldChonKhachHang.style.display = 'block';
            fieldXemKhachHang.style.display = 'none';

            // Load khách hàng chưa có thẻ
            loadKhachHangChuaCoThe();

            // Buttons
            btnSubmit.textContent = 'Thêm';
            btnSubmit.style.display = 'inline-block';
            btnCancel.textContent = 'Huỷ';
            btnEditMode.style.display = 'none';

        } else if (mode === 'edit') {
            formTitle.textContent = 'Sửa Thẻ Thành Viên';
            
            // Điền dữ liệu
            formId.value = data.id;
            formTenKH.value = data.tenKhachHang;
            formSdtKH.value = data.sdt;
            formSelectHang.value = data.hang;
            formDiem.value = data.diem;
            formNgayCap.value = data.ngayCap;

            // Hiển thị field xem khách hàng
            fieldChonKhachHang.style.display = 'none';
            fieldXemKhachHang.style.display = 'block';

            // Buttons
            btnSubmit.textContent = 'Lưu';
            btnSubmit.style.display = 'inline-block';
            btnCancel.textContent = 'Huỷ';
            btnEditMode.style.display = 'none';

        } else if (mode === 'view') {
            formTitle.textContent = 'Chi Tiết Thẻ Thành Viên';
            
            // Điền dữ liệu
            formId.value = data.id;
            formTenKH.value = data.tenKhachHang;
            formSdtKH.value = data.sdt;
            formSelectHang.value = data.hang;
            formDiem.value = data.diem;
            formNgayCap.value = data.ngayCap;

            // Hiển thị field xem khách hàng
            fieldChonKhachHang.style.display = 'none';
            fieldXemKhachHang.style.display = 'block';

            // Buttons
            btnSubmit.style.display = 'none';
            btnCancel.textContent = 'Đóng';
            btnEditMode.style.display = 'inline-block';
        }
    }

    // --- HÀM ĐÓNG FORM (CHUNG) ---
    function closeForm() {
        // 1. Phình Bảng
        listCol.classList.remove('col-md-8');
        listCol.classList.add('col-md-12');

        // 2. Ẩn Form
        formCol.classList.remove('col-md-4');
        formCol.classList.add('col-md-0');

        // 3. Xoá class điều khiển
        mainRow.classList.remove('form-open');

        // 4. Hiện lại nút "Thêm"
        showAddFormBtn.style.display = 'block';

        currentMode = 'add';
        currentData = null;
    }

    // --- GÁN SỰ KIỆN CHO NÚT "THÊM" ---
    showAddFormBtn.addEventListener('click', function (e) {
        e.preventDefault();
        openForm('add');
    });

    // --- GÁN SỰ KIỆN CHO NÚT "HUỶ/ĐÓNG" ---
    btnCancel.addEventListener('click', function (e) {
        e.preventDefault();
        closeForm();
    });

    // --- GÁN SỰ KIỆN CHO NÚT "CHỈNH SỬA" ---
    btnEditMode.addEventListener('click', function (e) {
        e.preventDefault();
        if (currentData) {
            openForm('edit', currentData);
        }
    });

    // --- GÁN SỰ KIỆN CHO NÚT "LƯU" ---
    btnSubmit.addEventListener('click', function (e) {
        e.preventDefault();
        
        if (currentMode === 'add') {
            callApiAddTTV();
        } else if (currentMode === 'edit') {
            callApiEditTTV();
        }
    });

    // --- GÁN SỰ KIỆN CHO BẢNG (SỬA & XOÁ) ---
    tableBody.addEventListener('click', function (e) {
        const btn = e.target.closest('a');
        if (!btn) return;

        e.preventDefault();
        const row = btn.closest('tr');

        // Lấy dữ liệu từ data attributes
        const data = {
            id: row.getAttribute('data-id'),
            khachHangId: row.getAttribute('data-khach-hang-id'),
            tenKhachHang: row.getAttribute('data-ten-khach-hang'),
            sdt: row.getAttribute('data-sdt'),
            hang: row.getAttribute('data-hang'),
            diem: row.getAttribute('data-diem'),
            ngayCap: row.getAttribute('data-ngay-cap')
        };

        // XỬ LÝ NÚT SỬA
        if (btn.classList.contains('btn-edit-khoi')) {
            // Load chi tiết từ API
            callApiGetTTVById(data.id);
        }

        // XỬ LÝ NÚT XOÁ
        if (btn.classList.contains('btn-delete-khoi')) {
            if (confirm(`Bạn có chắc muốn xoá thẻ "${data.id}" của khách hàng "${data.tenKhachHang}" không?`)) {
                callApiDeleteTTV(data.id, row);
            }
        }
    });


    // Load khách hàng chưa có thẻ
    async function loadKhachHangChuaCoThe() {
        try {
            const response = await fetch('/API/get-khach-hang-chua-co-the');
            const data = await response.json();

            if (response.ok && data) {
                // Clear existing options
                formSelectCustomer.innerHTML = '<option value="">-- Chọn khách hàng --</option>';

                // Add new options
                data.forEach(kh => {
                    const option = document.createElement('option');
                    option.value = kh.id;
                    option.textContent = `${kh.hoTen} (${kh.soDienThoai})`;
                    formSelectCustomer.appendChild(option);
                });
            }
        } catch (error) {
            console.error('Lỗi khi load khách hàng:', error);
        }
    }

    // Get next ID
    async function callApiGetNextIdTTV() {
        const dataToSend = {
            prefix: "TTV",
            totalLength: 7
        };
        
        try {
            const response = await fetch('/API/get-next-id-TTV', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dataToSend)
            });
            
            const data = await response.json();
            
            if (data && data.nextId) {
                formId.value = data.nextId;
                return data.nextId;
            } else {
                alert('Không thể lấy mã thẻ, vui lòng thử lại.');
                return null;
            }
        } catch (error) {
            console.error('Lỗi khi lấy mã thẻ:', error);
            alert('Không thể lấy mã thẻ, vui lòng thử lại.');
            return null;
        }
    }

    // Get TTV by ID
    async function callApiGetTTVById(id) {
        try {
            const response = await fetch(`/API/get-TTV-by-id?id=${encodeURIComponent(id)}`);
            
            if (!response.ok) {
                throw new Error('Lỗi khi gọi API');
            }
            
            const data = await response.json();
            console.log("Dữ liệu trả về:", data);

            // Đổ dữ liệu vào form
            openForm('edit', {
                id: data.id,
                khachHangId: data.khachHangId,
                tenKhachHang: data.tenKhachHang,
                sdt: data.soDienThoai,
                hang: data.hang,
                diem: data.diemTichLuy,
                ngayCap: data.ngayCap
            });
        } catch (error) {
            console.error(error);
            alert("Có lỗi khi lấy dữ liệu thẻ thành viên!");
        }
    }

    // Thêm
    async function callApiAddTTV() {
        try {
            const khachHangId = formSelectCustomer.value;
            
            if (!khachHangId) {
                alert('Vui lòng chọn khách hàng!');
                return;
            }

            // Get next ID
            const nextId = await callApiGetNextIdTTV();
            if (!nextId) return;

            const duLieu = {
                Id: nextId,
                KhachHangId: khachHangId,
                Hang: formSelectHang.value,
                DiemTichLuy: parseInt(formDiem.value) || 0,
                NgayCap: formNgayCap.value
            };

            console.log('Sending data:', duLieu);

            const response = await fetch('/API/add-TTV', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Thêm thẻ thành viên thất bại');
            }

            const data = await response.json();
            console.log(data);

            alert(data.message || 'Thêm thẻ thành viên thành công!');
            closeForm();
            
            location.reload();

        } catch (error) {
            console.error('Lỗi:', error);
            alert('Lỗi: ' + error.message);
        }
    }

    // Sửa 
    async function callApiEditTTV() {
        try {
            const duLieu = {
                Id: formId.value,
                Hang: formSelectHang.value,
                DiemTichLuy: parseInt(formDiem.value) || 0,
                NgayCap: formNgayCap.value
            };

            console.log('Updating data:', duLieu);

            const response = await fetch('/API/edit-TTV', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Sửa thẻ thành viên thất bại');
            }

            const data = await response.json();
            console.log(data);

            alert(data.message || 'Sửa thẻ thành viên thành công!');
            closeForm();
            
            location.reload();

        } catch (error) {
            console.error('Lỗi:', error);
            alert('Lỗi: ' + error.message);
        }
    }

    // Xóa 
    async function callApiDeleteTTV(id, row) {
        try {
            const response = await fetch(`/API/delete-TTV/${encodeURIComponent(id)}`, {
                method: 'DELETE'
            });

            let data = null;
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                data = await response.json();
            }

            if (!response.ok) {
                const errorMessage = (data && data.message) || 'Xóa thẻ thành viên thất bại!';
                throw new Error(errorMessage);
            }

            alert(data.message || 'Xóa thành công!');
            row.remove();
            closeForm();

        } catch (error) {
            console.error('Lỗi:', error);
            alert('Lỗi: ' + error.message);
        }
    }
});