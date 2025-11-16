document.addEventListener('DOMContentLoaded', function () {

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const mainRow = document.getElementById('main-row-container');
    const listCol = document.getElementById('category-list-col');
    const formCol = document.getElementById('category-form-col');

    const showAddFormBtn = document.getElementById('show-add-form-btn');
    const tableBody = document.getElementById('sampleTable').getElementsByTagName('tbody')[0];

    // Form Wrappers
    const addFormWrapper = document.getElementById('add-form-wrapper');
    const editFormWrapper = document.getElementById('edit-form-wrapper');

    // Nút Huỷ
    const cancelAddBtn = document.getElementById('cancel-add-form-btn');
    const cancelEditBtn = document.getElementById('cancel-edit-form-btn');
    const addBtn = document.getElementById('btn-add');
    const editBtn = document.getElementById('btn-edit');

    // Form Sửa
    const editIdInput = document.getElementById('edit-id-input');
    const editTenInput = document.getElementById('edit-ten-input');

    // Form Thêm
    const addTenInput = document.getElementById('add-ten-input');

    // --- HÀM MỞ FORM (CHUNG) ---
    function openForm(mode) {
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

        // 5. Chọn form nào để hiển thị
        if (mode === 'add') {
            addFormWrapper.style.display = 'block';
            editFormWrapper.style.display = 'none';
            addTenInput.value = ''; // Xoá trắng trường tên
        } else if (mode === 'edit') {
            addFormWrapper.style.display = 'none';
            editFormWrapper.style.display = 'block';
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
    }

    // --- GÁN SỰ KIỆN CHO NÚT "THÊM" ---
    showAddFormBtn.addEventListener('click', function (e) {
        callApiGetNextIdDM()
        e.preventDefault();
        openForm('add');
    });

    addBtn.addEventListener('click', function () {
        callApiAddDM();
    })

    editBtn.addEventListener('click', function () {
        callApiEditDM();
    })

    // --- GÁN SỰ KIỆN CHO CÁC NÚT "HUỶ" ---
    cancelAddBtn.addEventListener('click', function (e) {
        e.preventDefault();
        closeForm();
    });

    cancelEditBtn.addEventListener('click', function (e) {
        e.preventDefault();
        closeForm();
    });

    // --- GÁN SỰ KIỆN CHO BẢNG (SỬA & XOÁ) ---
    tableBody.addEventListener('click', function (e) {
        // Chỉ bắt sự kiện nếu nhấn vào <a> bên trong <td>
        const btn = e.target.closest('a');
        if (!btn) return;

        e.preventDefault();
        const row = btn.closest('tr');

        // Yêu cầu 1: XỬ LÝ NÚT SỬA (btn-edit-khoi)
        //if (btn.classList.contains('btn-edit-khoi')) {
        //    // 1. Lấy dữ liệu từ bảng
        //    const maDM = row.cells[0].textContent.trim();
        //    const tenDM = row.cells[1].textContent.trim();

        //    // 2. Điền vào form "Sửa"
        //    editIdInput.value = maDM;
        //    editTenInput.value = tenDM;

        //    // 3. Mở form ở chế độ "edit"
        //    openForm('edit');
        //}

        // Yêu cầu 2: XỬ LÝ NÚT XOÁ (btn-delete-khoi)
        if (btn.classList.contains('btn-delete-khoi')) {
            const tenDM = row.cells[1].textContent.trim();

            // 3. Hỏi xác nhận
            if (confirm(`Bạn có chắc muốn xoá danh mục "${tenDM}" không?`)) {
                callApiDeleteDM(row.cells[0].textContent.trim());
                row.remove();

                // Đóng form nếu nó đang mở
                closeForm();
            }
        }
    });



    



    //$(".btn-edit-khoi").click(function () {
    //    // Lấy dữ liệu từ thuộc tính data-id và data-ten
    //    var id = $(this).data("id");

    //    // Thực hiện công việc bạn muốn với dữ liệu này
    //    console.log("ID:", id);
    //    console.log("Tên:", ten);

    //    // Ví dụ: Hiển thị dữ liệu vào một form chỉnh sửa
    //    $("#editFormId").val(id);
    //    $("#editFormTen").val(ten);

    //    // Hoặc mở một modal để chỉnh sửa
    //    $('#editModal').modal('show');
    //});



    //hàm chèn dữ liệu vào form mới(vd: khi xem chi tiết....)   
    document.addEventListener('click', function (e) {
        const btn = e.target.closest('.btn-edit-khoi');
        if (!btn) return;

        e.preventDefault();

        const id = btn.getAttribute('data-id');
        console.log("ID cần sửa:", id);

        fetch(`/API/get-DM-by-id?id=${encodeURIComponent(id)}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Lỗi khi gọi API');
                }
                return response.json();
            })
            .then(data => {
                console.log("Dữ liệu trả về:", data);

                // Đổ dữ liệu vào form
                document.getElementById('edit-id-input').value = data.id;
                document.getElementById('edit-ten-input').value = data.ten;

                openForm('edit');
                // Nếu xài Bootstrap modal:
                // const modal = new bootstrap.Modal(document.getElementById('editModal'));
                // modal.show();
            })
            .catch(error => {
                console.error(error);
                alert("Có lỗi khi lấy dữ liệu danh mục!");
            });
    });


    async function callApiDeleteDM(id) {
        try {
            // Gửi request DELETE
            const response = await fetch(`/API/${encodeURIComponent(id)}`, {
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
                const errorMessage = (data && data.message) || 'Xóa danh mục thất bại!';
                throw new Error(errorMessage);
            }


            // TODO: nếu m có hàm reload lại bảng thì gọi ở đây
            // loadDanhMuc();

        } catch (error) {
            alert('Lỗi: ' + error.message);
        }
    }



    async function callApiEditDM() {
        try {
            const id = document.getElementById('edit-id-input').value;
            const ten = document.getElementById('edit-ten-input').value;

            if (!id || !ten) {
                alert('Các trường "id" và "ten" là bắt buộc!');
                return;
            }

            const duLieu = {
                Id: id,
                Ten: ten
            }

            // Gửi request
            const response = await fetch('/API/edit-DM', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
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



    async function callApiAddDM() {
        try {
            const id = document.getElementById('add-id-input').value;
            const ten = document.getElementById('add-ten-input').value;

            if (!id || !ten) {
                alert('Các trường "id" và "ten" là bắt buộc!');
                return;
            }

            const duLieu = {
                Id: id,
                Ten: ten
            }

            // Gửi request
            const response = await fetch('/API/add-DM', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(duLieu)
                // KHÔNG thêm Content-Type header
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


    async function callApiGetNextIdDM() {
        const dataToSend = {
            prefix: "DM",
            totalLength: 6
        };
        try {
            const response = await fetch('/API/get-next-id-DM',
                {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(dataToSend)
                });
            const data = await response.json();
            if (data) {
                document.getElementById('add-id-input').value = data.nextId;
            }
            else {
                alert('Không thể lấy mã danh mục, vui lòng thử lại.');
            }
        } catch (error) {
            console.error('Lỗi khi lấy mã danh mục:', error);
            alert('Không thể lấy mã danh mục, vui lòng thử lại.');
        }
    }




    

});


$(async function () {
    await appRealtimeList.initEntityTable({
        key: 'DanhMuc',                  // key dùng cho SignalR: NotifyReloadAsync("DanhMuc")
        apiUrl: '/API/get-all-DM',       // API lấy dữ liệu
        tableId: 'sampleTable',
        tbodyId: 'tbody-danh-muc',
        buildRow: dm => {
            // return 1 dòng <tr> cho DanhMuc
            return `
                        <tr>
                            <td>${dm.id}</td>
                            <td>${dm.ten}</td>
                            <td>${dm.soSanPham}</td>
                            <td class="text-center">
                                <a class="btn btn-info btn-sm me-1 btn-edit-khoi"
                                   href="#"
                                   data-id="${dm.id}"
                                   title="Sửa">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a class="btn btn-danger btn-sm btn-delete-khoi"
                                   href="#"
                                   data-id="${dm.id}"
                                   title="Xóa">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </td>
                        </tr>`;
        }
    });
});




