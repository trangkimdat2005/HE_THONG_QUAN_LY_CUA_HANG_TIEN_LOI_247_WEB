document.addEventListener('DOMContentLoaded', function () {
    console.log('=== THÊM KHÁCH HÀNG PAGE LOADED ===');

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const hoTenInput = document.getElementById('ho-ten');
    const gioiTinhRadios = document.querySelectorAll('input[name="gioiTinh"]');
    const soDienThoaiInput = document.getElementById('so-dien-thoai');
    const emailInput = document.getElementById('email');
    const diaChiInput = document.getElementById('dia-chi');
    const ngayDangKyInput = document.getElementById('ngay-dang-ky');
    const trangThaiSelect = document.getElementById('trang-thai');
    const btnLuu = document.getElementById('btn-luu');
    const avatarUpload = document.getElementById('avatarUpload');
    const avatarPreview = document.getElementById('avatarPreview');

    // Thẻ thành viên
    const toggleMembershipCheckbox = document.getElementById('toggle-membership-form');
    const membershipContainer = document.getElementById('membership-form-container');
    const hangTheSelect = document.getElementById('hang-the');
    const diemTichLuyInput = document.getElementById('diem-tich-luy');
    const ngayCapTheInput = document.getElementById('ngay-cap-the');

    // Set ngày hiện tại
    if (ngayDangKyInput) {
        const today = new Date().toISOString().split('T')[0];
        ngayDangKyInput.value = today;
    }

    if (ngayCapTheInput) {
        const today = new Date().toISOString().split('T')[0];
        ngayCapTheInput.value = today;
    }

    // --- XỬ LÝ UPLOAD ẢNH ---
    if (avatarUpload && avatarPreview) {
        avatarUpload.addEventListener('change', function(e) {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function(event) {
                    avatarPreview.src = event.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }

    // --- TOGGLE THẺ THÀNH VIÊN ---
    if (toggleMembershipCheckbox && membershipContainer) {
        toggleMembershipCheckbox.addEventListener('change', function() {
            if (this.checked) {
                membershipContainer.style.display = 'block';
            } else {
                membershipContainer.style.display = 'none';
            }
        });
    }

    // --- NÚT LƯU ---
    if (btnLuu) {
        btnLuu.addEventListener('click', async function() {
            console.log('=== BẮT ĐẦU THÊM KHÁCH HÀNG ===');

            // Lấy dữ liệu từ form
            const hoTen = hoTenInput?.value.trim();
            const soDienThoai = soDienThoaiInput?.value.trim();
            const email = emailInput?.value.trim();
            const diaChi = diaChiInput?.value.trim();
            const ngayDangKy = ngayDangKyInput?.value;
            const trangThai = trangThaiSelect?.value;

            // Lấy giới tính
            let gioiTinh = true; // Mặc định là Nam
            gioiTinhRadios.forEach(radio => {
                if (radio.checked) {
                    gioiTinh = radio.value === 'true';
                }
            });

            // Validate
            if (!hoTen) {
                alert('Vui lòng nhập họ tên!');
                hoTenInput?.focus();
                return;
            }

            if (!soDienThoai) {
                alert('Vui lòng nhập số điện thoại!');
                soDienThoaiInput?.focus();
                return;
            }

            if (!diaChi) {
                alert('Vui lòng nhập địa chỉ!');
                diaChiInput?.focus();
                return;
            }

            if (!ngayDangKy) {
                alert('Vui lòng chọn ngày đăng ký!');
                ngayDangKyInput?.focus();
                return;
            }

            // Tạo object request
            const requestData = {
                HoTen: hoTen,
                SoDienThoai: soDienThoai,
                Email: email || null,
                DiaChi: diaChi,
                NgayDangKy: ngayDangKy,
                TrangThai: trangThai,
                GioiTinh: gioiTinh,
                AnhId: null, // TODO: Upload ảnh sau
                CreateMemberCard: toggleMembershipCheckbox?.checked || false
            };

            // Nếu tạo thẻ thành viên
            if (requestData.CreateMemberCard) {
                requestData.TheThanhVien = {
                    Hang: hangTheSelect?.value || 'Bronze',
                    DiemTichLuy: parseInt(diemTichLuyInput?.value) || 0,
                    NgayCap: ngayCapTheInput?.value || null
                };
            }

            console.log('Request data:', JSON.stringify(requestData, null, 2));

            try {
                // Hiển thị loading
                btnLuu.disabled = true;
                const originalHTML = btnLuu.innerHTML;
                btnLuu.innerHTML = '<i class="fas fa-hourglass-half me-2"></i>Đang lưu...';

                console.log('Sending request to: /API/add-KhachHang');
                console.log('Request data:', JSON.stringify(requestData, null, 2));

                // Gọi API
                const response = await fetch('/API/add-KhachHang', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestData)
                });

                console.log('Response status:', response.status);
                console.log('Response ok:', response.ok);

                const result = await response.json();
                console.log('Response data:', result);

                if (response.ok) {
                    console.log('✅ SUCCESS');
                    alert(result.message || 'Thêm khách hàng thành công!');
                    
                    // Chuyển về trang danh sách
                    window.location.href = '/QuanLyKhachHang/DanhSachKhachHang';
                } else {
                    console.error('❌ ERROR:', result.message);
                    alert('Lỗi: ' + (result.message || 'Không thể thêm khách hàng'));
                    
                    btnLuu.disabled = false;
                    btnLuu.innerHTML = originalHTML;
                }
            } catch (error) {
                console.error('❌ EXCEPTION:', error);
                console.error('Error details:', error.stack);
                alert('Có lỗi xảy ra khi thêm khách hàng:\n\n' + error.message);
                
                btnLuu.disabled = false;
                btnLuu.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Lưu thông tin';
            }
        });
    }
});