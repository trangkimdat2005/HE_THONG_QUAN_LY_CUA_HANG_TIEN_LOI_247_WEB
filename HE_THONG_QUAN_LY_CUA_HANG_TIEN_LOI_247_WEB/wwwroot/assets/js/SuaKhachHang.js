document.addEventListener('DOMContentLoaded', function () {

    // --- KHỞI TẠO SELECT2 ---
    $('#select-hang-the').select2({ width: '100%' });
    $('#select-trang-thai').select2({ width: '100%' });

    // --- TỰ ĐỘNG ĐIỀN NGÀY HÔM NAY ---
    const today = new Date().toISOString().split('T')[0];
    const ngayDangKyInput = document.getElementById('input-ngay-dang-ky');
    const ngayCapTheInput = document.getElementById('input-ngay-cap');

    if (ngayDangKyInput) ngayDangKyInput.value = today;
    if (ngayCapTheInput) ngayCapTheInput.value = today;

    // --- TRUY XUẤT CÁC PHẦN TỬ FORM ---
    const toggleSwitch = document.getElementById('toggle-membership-form'); // ID giữ nguyên
    const customerContainer = document.getElementById('customer-form-container');
    const membershipContainer = document.getElementById('membership-form-container');

    // --- XỬ LÝ LOGIC CHÍNH KHI BẬT/TẮT CHECKBOX ---
    toggleSwitch.addEventListener('change', function () {
        if (this.checked) {
            // --- KÍCH HOẠT CHẾ ĐỘ 2 CỘT ---
            membershipContainer.style.display = 'block';
            setTimeout(() => {
                membershipContainer.style.opacity = 1;
            }, 10);
            customerContainer.classList.remove('mx-auto');
        } else {
            // --- TẮT, QUAY VỀ TRẠNG THÁI BAN ĐẦU ---
            membershipContainer.style.opacity = 0;
            setTimeout(() => {
                membershipContainer.style.display = 'none';
            }, 500);
            customerContainer.classList.add('mx-auto');
        }
    });

    // --- THÊM MỚI: XỬ LÝ PREVIEW ẢNH ĐẠI DIỆN ---
    const avatarUpload = document.getElementById('avatarUpload');
    const avatarPreview = document.getElementById('avatarPreview');

    avatarUpload.addEventListener('change', function () {
        const file = this.files[0]; // Lấy file đầu tiên
        if (file) {
            // Tạo một URL tạm thời cho file ảnh
            const reader = new FileReader();
            reader.onload = function (e) {
                // Gán URL này cho ảnh preview
                avatarPreview.src = e.target.result;
            }
            reader.readAsDataURL(file);
        }
    });

});







//document.addEventListener('DOMContentLoaded', function () {

//    // Xử lý nút lưu
//    const saveButton = document.getElementById('saveButton');
//    if (saveButton) {
//        saveButton.addEventListener('click', function (e) {
//            e.preventDefault(); // Ngăn hành vi mặc định của nút

//            // 1. Lấy form khách hàng
//            const customerForm = document.getElementById('editCustomerForm');
//            if (!customerForm) {
//                console.error('Không tìm thấy form #editCustomerForm!');
//                return;
//            }

//            // 2. Tạo FormData từ form
//            // FormData sẽ tự động lấy tất cả input có thuộc tính 'name' bên trong form
//            const formData = new FormData(customerForm);

//            // 3. Lấy dữ liệu Thẻ Thành Viên (vì nó nằm ngoài form)
//            // và thêm thủ công vào formData
//            const hangTheSelect = document.getElementById('select-hang-the');
//            const diemTichLuyInput = document.getElementById('input-diem-tich-luy');
//            const ngayCapInput = document.getElementById('input-ngay-cap');

//            // Chỉ thêm nếu các trường này tồn tại (ví dụ: khi checkbox "Mở Thẻ" được check)
//            if (hangTheSelect) {
//                formData.append('hangThe', hangTheSelect.value);
//            }
//            if (diemTichLuyInput) {
//                formData.append('diemTichLuy', diemTichLuyInput.value);
//            }
//            if (ngayCapInput) {
//                formData.append('ngayCapThe', ngayCapInput.value);
//            }

//            // 4. Gửi dữ liệu bằng fetch (AJAX)
//            // URL phải trỏ đến Action của bạn: /Area/Controller/Action
//            fetch('/Sua/SuaKhachHang', {
//                method: 'POST',
//                body: formData
//                // Khi dùng FormData, không cần set 'Content-Type'
//                // Trình duyệt sẽ tự động set 'multipart/form-data'
//            })
//                .then(response => response.json())
//                .then(data => {
//                    if (data.success) {
//                        alert('Lưu thông tin thành công!');
//                        // Chuyển hướng về trang danh sách (thay đổi URL nếu cần)
//                        window.location.href = '/Admin/QuanLyKhachHang/DanhSachKhachHang';
//                    } else {
//                        let errorMessage = 'Lưu thất bại: ' + (data.message || 'Lỗi không xác định.');
//                        if (data.errors) {
//                            errorMessage += '\n' + data.errors.join('\n');
//                        }
//                        alert(errorMessage);
//                    }
//                })
//                .catch(error => {
//                    console.error('Error:', error);
//                    alert('Đã xảy ra lỗi khi gửi dữ liệu.');
//                });
//        });
//    }

//    // Xử lý hiển thị ảnh preview khi chọn
//    const avatarUpload = document.getElementById('avatarUpload');
//    const avatarPreview = document.getElementById('avatarPreview');
//    if (avatarUpload && avatarPreview) {
//        avatarUpload.addEventListener('change', function () {
//            const file = this.files[0];
//            if (file) {
//                const reader = new FileReader();
//                reader.onload = function (e) {
//                    avatarPreview.src = e.target.result;
//                }
//                reader.readAsDataURL(file);
//            }
//        });
//    }

//    // (Bạn có thể thêm mã JS cho checkbox 'toggle-membership-form' ở đây)
//});
