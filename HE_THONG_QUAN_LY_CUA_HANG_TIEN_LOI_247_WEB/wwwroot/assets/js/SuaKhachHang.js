document.addEventListener('DOMContentLoaded', function () {
    console.log('=== SỬA KHÁCH HÀNG PAGE LOADED ===');

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const khachHangIdInput = document.getElementById('khach-hang-id');
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

            // Lấy dữ liệu từ form
            const khachHangId = khachHangIdInput?.value;
            const hoTen = hoTenInput?.value.trim();
            const soDienThoai = soDienThoaiInput?.value.trim();
            const email = emailInput?.value.trim();
            const diaChi = diaChiInput?.value.trim();
            const ngayDangKy = ngayDangKyInput?.value;
            const trangThai = trangThaiSelect?.value;

            // Lấy giới tính
            let gioiTinh = true;
            gioiTinhRadios.forEach(radio => {
                if (radio.checked) {
                    gioiTinh = radio.value === 'true';
                }
            });

            // Validate

            if (!diaChi) {
                alert('Vui lòng nhập địa chỉ!');
                diaChiInput?.focus();
                return;
            }

            // Tạo object request
            const requestData = {
                Id: khachHangId,
                HoTen: hoTen,
                SoDienThoai: soDienThoai,
                Email: email || null,
                DiaChi: diaChi,
                NgayDangKy: ngayDangKy,
                TrangThai: trangThai,
                GioiTinh: gioiTinh,
                UpdateMemberCard: toggleMembershipCheckbox?.checked || false
            };

            // Nếu cập nhật thẻ thành viên
            if (requestData.UpdateMemberCard) {
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

                // Gọi API
                const response = await fetch('/API/edit-KhachHang', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestData)
                });

                const result = await response.json();
                console.log('Response:', result);

                if (response.ok) {
                    console.log('✅ SUCCESS');
                    alert(result.message || 'Cập nhật khách hàng thành công!');
                    
                    // Chuyển về trang danh sách
                    window.location.href = '/QuanLyKhachHang/DanhSachKhachHang';
                } else {
                    console.error('❌ ERROR:', result.message);
                    alert('Lỗi: ' + (result.message || 'Không thể cập nhật khách hàng'));
                    
                    btnLuu.disabled = false;
                    btnLuu.innerHTML = originalHTML;
                }
            } catch (error) {
                console.error('❌ EXCEPTION:', error);
                alert('Có lỗi xảy ra khi cập nhật khách hàng:\n\n' + error.message);
                
                btnLuu.disabled = false;
                btnLuu.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Lưu thay đổi';
            }
        });
    }
});
