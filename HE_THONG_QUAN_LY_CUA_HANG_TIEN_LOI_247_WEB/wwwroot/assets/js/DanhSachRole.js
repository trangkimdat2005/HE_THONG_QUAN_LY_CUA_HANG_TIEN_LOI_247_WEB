document.addEventListener('DOMContentLoaded', function () {
    const tablePanel = document.getElementById('table-panel');
    const formPanel = document.getElementById('form-panel');
    const btnAdd = document.querySelector('.btn-add-khoi');
    const btnEditList = document.querySelectorAll('.btn-edit-khoi');
    const btnCancel = document.getElementById('btn-cancel-form');
    const roleForm = document.getElementById('role-form');

    const formTitle = document.getElementById('form-title');
    const inputCode = document.getElementById('roleCode');
    const inputName = document.getElementById('roleName');
    const inputDescription = document.getElementById('roleDescription');
    const inputStatus = document.getElementById('roleStatus');
    // --- KHỞI TẠO SELECT2 ---
    $('#select-nhan-vien').select2({ width: '100%' });
    $('#select-khach-hang').select2({ width: '100%' });
    $('#select-trang-thai').select2({ width: '100%', minimumResultsForSearch: Infinity }); // Ẩn thanh tìm kiếm
    $('#select-quyen').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều quyền'
    });
    function showFormPanel(mode, data = {}) {
        tablePanel.classList.remove('col-lg-12');
        tablePanel.classList.add('col-lg-8');
        formPanel.style.display = 'block';

        if (mode === 'add') {
            formTitle.innerText = 'Thêm Vai Trò Mới';
            roleForm.reset();
            inputCode.readOnly = false;
        } else if (mode === 'edit') {
            formTitle.innerText = 'Sửa Vai Trò';
            inputCode.value = data.code;
            inputName.value = data.ten;
            inputDescription.value = data.mota;
            inputStatus.value = data.trangthai;
            inputCode.readOnly = true;
        }
    }

    function hideFormPanel() {
        formPanel.style.display = 'none';
        tablePanel.classList.remove('col-lg-8');
        tablePanel.classList.add('col-lg-12');
        roleForm.reset();
    }

    btnAdd.addEventListener('click', function (e) {
        e.preventDefault();
        showFormPanel('add');
    });

    btnEditList.forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            const data = {
                code: this.dataset.code,
                ten: this.dataset.ten,
                mota: this.dataset.mota,
                trangthai: this.dataset.trangthai
            };
            showFormPanel('edit', data);
        });
    });

    btnCancel.addEventListener('click', function (e) {
        e.preventDefault();
        hideFormPanel();
    });

    roleForm.addEventListener('submit', function (e) {
        e.preventDefault();
        alert('Chức năng Lưu đang được xử lý!');
        hideFormPanel();
    });
});