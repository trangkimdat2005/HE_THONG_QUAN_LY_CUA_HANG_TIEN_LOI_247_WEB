let dbData = null;  // Định nghĩa dbData ở phạm vi toàn cục
let isEditingAvatar = false; // Flag để theo dõi trạng thái đang chỉnh sửa ảnh
let newAvatarFile = null; // Lưu file ảnh mới được chọn
let originalAvatarSrc = null; // Lưu ảnh gốc để có thể restore

document.addEventListener("DOMContentLoaded", async function () {
    try {
        // Gọi API và lấy dữ liệu
        const fetchedData = await callApiGetThongTinNhanVienData();

        // Kiểm tra nếu dữ liệu trả về hợp lệ
        if (!fetchedData) {
            console.error("Không có dữ liệu trả về từ API.");
            return;
        }

        // Khởi tạo dbData từ dữ liệu fetchedData
        dbData = {
            TaiKhoan: {
                id: fetchedData.id,
                tenDangNhap: fetchedData.tenDangNhap,
                trangThai: fetchedData.trangThaiTaiKhoan
            },
            NhanVien: {
                id: fetchedData.nhanVienId,
                hoTen: fetchedData.hoTen,
                chucVu: fetchedData.chucVu,
                luongCoBan: fetchedData.luongCoBan,
                soDienThoai: fetchedData.soDienThoai,
                email: fetchedData.email,
                diaChi: fetchedData.diaChi,
                ngayVaoLam: fetchedData.ngayVaoLam,
                trangThai: fetchedData.trangThaiNhanVien,
                gioiTinh: fetchedData.gioiTinh,
                anhId: fetchedData.anhId  // Lưu ID của ảnh
            }
        };

        // Kiểm tra lại dbData trước khi gọi initView
        if (dbData && dbData.NhanVien) {
            initView();
            setupEventListeners(); // Setup event listeners sau khi init
        } else {
            console.error("Dữ liệu không hợp lệ: dbData hoặc dbData.NhanVien là null.");
        }
    } catch (error) {
        console.error("Đã xảy ra lỗi khi gọi API:", error);
        alert('Lỗi khi lấy thông tin, vui lòng thử lại.');
    }
});

const els = {
    view: document.querySelectorAll('.view-data'),
    edit: document.querySelectorAll('.edit-data'),
    btnEdit: document.getElementById('btn-edit'),
    actionBtns: document.getElementById('action-buttons'),

    lblHotenCard: document.getElementById('lbl-hoten-card'),
    lblHotenDetail: document.getElementById('lbl-hoten-detail'),
    inpHoten: document.getElementById('inp-hoten'),

    lblEmail: document.getElementById('lbl-email'),
    inpEmail: document.getElementById('inp-email'),

    lblSdt: document.getElementById('lbl-sdt'),
    inpSdt: document.getElementById('inp-sdt'),

    lblGioitinh: document.getElementById('lbl-gioitinh'),
    inpGioitinh: document.getElementById('inp-gioitinh'),

    lblDiachi: document.getElementById('lbl-diachi-full'),
    lblDiachiShort: document.getElementById('lbl-diachi-short'),
    inpDiachi: document.getElementById('inp-diachi'),

    imgAvatar: document.getElementById('img-avatar'),
    fileUpload: document.getElementById('file-upload')
};

function setupEventListeners() {
    console.log('Setting up event listeners...');
    
    // Event listener cho file upload
    if (els.fileUpload) {
        els.fileUpload.addEventListener('change', handleFileUpload);
        console.log('File upload listener attached');
    } else {
        console.error('File upload element not found!');
    }
}

function handleFileUpload(event) {
    console.log('File upload triggered');
    const file = event.target.files[0];
    
    if (!file) {
        console.log('No file selected');
        return;
    }
    
    console.log('File selected:', file.name, 'Size:', file.size, 'Type:', file.type);
    
    // Kiểm tra kích thước file (giới hạn 5MB)
    if (file.size > 5 * 1024 * 1024) {
        alert('Kích thước ảnh không được vượt quá 5MB!');
        event.target.value = ''; // Reset input
        return;
    }

    // Kiểm tra định dạng file
    if (!file.type.startsWith('image/')) {
        alert('Vui lòng chọn file ảnh hợp lệ!');
        event.target.value = ''; // Reset input
        return;
    }

    newAvatarFile = file;

    // Hiển thị preview
    const reader = new FileReader();
    reader.onload = function (e) {
        console.log('File loaded successfully for preview');
        const base64Data = e.target.result;
        
        // Cập nhật ảnh hiển thị (preview)
        els.imgAvatar.src = base64Data;
        
        // Thêm class để hiển thị trạng thái preview
        const avatarBox = document.querySelector('.avatar-box');
        if (avatarBox) {
            avatarBox.classList.add('has-preview', 'editing');
        }
        
        console.log('Preview updated');
    }
    
    reader.onerror = function(error) {
        console.error('Error reading file:', error);
        alert('Lỗi khi đọc file ảnh!');
    }
    
    reader.readAsDataURL(file);
}

// Bắt đầu chỉnh sửa ảnh
function startEditAvatar() {
    console.log('Start editing avatar');
    
    if (isEditingAvatar) {
        return;
    }
    
    isEditingAvatar = true;
    
    // Lưu ảnh gốc
    originalAvatarSrc = els.imgAvatar.src;
    
    // Thêm class editing cho avatar box
    const avatarBox = document.querySelector('.avatar-box');
    if (avatarBox) {
        avatarBox.classList.add('editing');
    }
    
    // Ẩn nút mặc định, hiện nút edit
    document.getElementById('default-buttons').classList.add('d-none');
    document.getElementById('edit-avatar-buttons').classList.remove('d-none');
    
    // Trigger file input
    els.fileUpload.click();
}

// Hủy chỉnh sửa ảnh
function cancelEditAvatar() {
    console.log('Cancel editing avatar');
    
    if (!isEditingAvatar) {
        return;
    }
    
    // Khôi phục ảnh gốc
    if (originalAvatarSrc) {
        els.imgAvatar.src = originalAvatarSrc;
    }
    
    // Reset state
    isEditingAvatar = false;
    newAvatarFile = null;
    
    // Remove classes
    const avatarBox = document.querySelector('.avatar-box');
    if (avatarBox) {
        avatarBox.classList.remove('editing', 'has-preview');
    }
    
    // Reset file input
    if (els.fileUpload) {
        els.fileUpload.value = '';
    }
    
    // Hiện nút mặc định, ẩn nút edit
    document.getElementById('default-buttons').classList.remove('d-none');
    document.getElementById('edit-avatar-buttons').classList.add('d-none');
}

// Xác nhận cập nhật ảnh
async function confirmUpdateAvatar() {
    console.log('Confirm update avatar');
    
    if (!newAvatarFile) {
        alert('Vui lòng chọn ảnh mới!');
        return;
    }
    
    // Upload ảnh
    const success = await uploadAvatar(newAvatarFile);
    
    if (success) {
        // Reset state
        isEditingAvatar = false;
        newAvatarFile = null;
        originalAvatarSrc = null;
        
        // Remove classes
        const avatarBox = document.querySelector('.avatar-box');
        if (avatarBox) {
            avatarBox.classList.remove('editing', 'has-preview');
        }
        
        // Reset file input
        if (els.fileUpload) {
            els.fileUpload.value = '';
        }
        
        // Hiện nút mặc định, ẩn nút edit
        document.getElementById('default-buttons').classList.remove('d-none');
        document.getElementById('edit-avatar-buttons').classList.add('d-none');
    }
}

function initView() {
    els.lblHotenCard.innerText = dbData.NhanVien.hoTen;
    els.lblHotenDetail.innerText = dbData.NhanVien.hoTen;
    document.getElementById('lbl-chucvu').innerText = dbData.NhanVien.chucVu;
    els.lblDiachiShort.innerText = "TP. Hồ Chí Minh";
    els.lblDiachi.innerText = dbData.NhanVien.diaChi;
    els.lblEmail.innerText = dbData.NhanVien.email;
    els.lblSdt.innerText = dbData.NhanVien.soDienThoai;
    document.getElementById('lbl-username').innerText = dbData.TaiKhoan.tenDangNhap;

    document.getElementById('lbl-luong').innerText = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(dbData.NhanVien.luongCoBan);
    document.getElementById('lbl-ngayvaolam').innerText = new Date(dbData.NhanVien.ngayVaoLam).toLocaleDateString('vi-VN');

    if (dbData.NhanVien.anhId) {
        els.imgAvatar.src = `/API/getHinhAnh/${dbData.NhanVien.anhId}`;
        els.imgAvatar.onerror = function() {
            this.src = 'https://i.pravatar.cc/300?img=11';
        };
    } else {
        els.imgAvatar.src = 'https://i.pravatar.cc/300?img=11';
    }

    updateGenderDisplay(dbData.NhanVien.gioiTinh);

    const statusBadge = dbData.TaiKhoan.trangThai === "Active"
        ? '<span class="status-active">Đang hoạt động</span>'
        : '<span class="badge bg-danger">Đã khóa</span>';
    document.getElementById('lbl-trangthai').innerHTML = statusBadge;
}

function updateGenderDisplay(isMale) {
    const genderText = isMale ? 'Nam' : 'Nữ';
    const genderIcon = isMale ? '<i class="fas fa-mars text-primary ms-2"></i>' : '<i class="fas fa-venus text-danger ms-2"></i>';
    els.lblGioitinh.innerHTML = genderText + genderIcon;
}

function toggleEditMode(isEdit) {
    els.view.forEach(el => el.classList.toggle('d-none', isEdit));
    els.edit.forEach(el => el.classList.toggle('d-none', !isEdit));

    if (isEdit) {
        els.btnEdit.classList.add('d-none');
        els.actionBtns.classList.remove('d-none');

        els.inpHoten.value = dbData.NhanVien.hoTen;
        els.inpEmail.value = dbData.NhanVien.email;
        els.inpSdt.value = dbData.NhanVien.soDienThoai;
        els.inpDiachi.value = dbData.NhanVien.diaChi;
        els.inpGioitinh.value = dbData.NhanVien.gioiTinh.toString();
    } else {
        els.btnEdit.classList.remove('d-none');
        els.actionBtns.classList.add('d-none');
    }
}

async function saveData() {
    console.log('=== SAVING DATA ===');
    
    // Validate dữ liệu
    if (!els.inpHoten.value.trim()) {
        alert('Vui lòng nhập họ tên!');
        return;
    }

    if (!els.inpEmail.value.trim()) {
        alert('Vui lòng nhập email!');
        return;
    }

    // Validate email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(els.inpEmail.value)) {
        alert('Email không hợp lệ!');
        return;
    }

    if (!els.inpSdt.value.trim()) {
        alert('Vui lòng nhập số điện thoại!');
        return;
    }

    // Validate phone number (10 số)
    const phoneRegex = /^[0-9]{10}$/;
    if (!phoneRegex.test(els.inpSdt.value)) {
        alert('Số điện thoại phải có 10 chữ số!');
        return;
    }

    // Chuẩn bị dữ liệu để gửi (không bao gồm ảnh - ảnh được upload riêng)
    const updateData = {
        hoTen: els.inpHoten.value.trim(),
        email: els.inpEmail.value.trim(),
        soDienThoai: els.inpSdt.value.trim(),
        diaChi: els.inpDiachi.value.trim(),
        gioiTinh: els.inpGioitinh.value === 'true',
        anhBase64: null // Không gửi ảnh qua đây nữa
    };

    console.log('Update data:', updateData);

    try {
        // Gọi API để cập nhật
        const response = await fetch('/API/updateThongTinNhanVien', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updateData)
        });

        const result = await response.json();

        if (!response.ok) {
            console.error('API Error:', result);
            throw new Error(result.message || 'Có lỗi xảy ra khi cập nhật');
        }

        console.log('API Success:', result);

        // Cập nhật dữ liệu local
        dbData.NhanVien.hoTen = updateData.hoTen;
        dbData.NhanVien.email = updateData.email;
        dbData.NhanVien.soDienThoai = updateData.soDienThoai;
        dbData.NhanVien.diaChi = updateData.diaChi;
        dbData.NhanVien.gioiTinh = updateData.gioiTinh;

        // Cập nhật giao diện
        els.lblHotenCard.innerText = dbData.NhanVien.hoTen;
        els.lblHotenDetail.innerText = dbData.NhanVien.hoTen;
        els.lblEmail.innerText = dbData.NhanVien.email;
        els.lblSdt.innerText = dbData.NhanVien.soDienThoai;
        els.lblDiachi.innerText = dbData.NhanVien.diaChi;

        updateGenderDisplay(dbData.NhanVien.gioiTinh);

        alert('Đã cập nhật thông tin thành công!');
        toggleEditMode(false);
    } catch (error) {
        console.error('Lỗi khi cập nhật:', error);
        alert('Không thể cập nhật thông tin. Vui lòng thử lại!');
    }
}

function cancelEdit() {
    toggleEditMode(false);
}


async function callApiGetThongTinNhanVienData() {
    try {
        const response = await fetch('/API/getThongTinNhanVien',
            {
                method: 'GET'
            });
        if (!response.ok) {
            throw new Error('Không thể lấy thông tin nhân viên');
        }
        const data = await response.json();
        if (data) {
            return data
        }
        else {
            alert('Không thể lấy thông tin, vui lòng thử lại.');
        }
    } catch (error) {
        console.error('Lỗi khi lấy thông tin:', error);
        alert('Không thể lấy thông tin, vui lòng thử lại.');
    }
}

// Toggle password visibility
function togglePassword(inputId) {
    const input = document.getElementById(inputId);
    const icon = document.getElementById(inputId + '-icon');
    
    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.remove('fa-eye');
        icon.classList.add('fa-eye-slash');
    } else {
        input.type = 'password';
        icon.classList.remove('fa-eye-slash');
        icon.classList.add('fa-eye');
    }
}

// Change password function
async function changePassword() {
    const currentPassword = document.getElementById('currentPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    // Validation
    if (!currentPassword) {
        alert('Vui lòng nhập mật khẩu hiện tại!');
        return;
    }

    if (!newPassword) {
        alert('Vui lòng nhập mật khẩu mới!');
        return;
    }

    if (newPassword.length < 6) {
        alert('Mật khẩu mới phải có ít nhất 6 ký tự!');
        return;
    }

    if (newPassword !== confirmPassword) {
        alert('Mật khẩu xác nhận không khớp!');
        return;
    }

    if (currentPassword === newPassword) {
        alert('Mật khẩu mới phải khác mật khẩu hiện tại!');
        return;
    }

    try {
        const response = await fetch('/API/changePassword', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                currentPassword: currentPassword,
                newPassword: newPassword,
                confirmPassword: confirmPassword
            })
        });

        const result = await response.json();

        if (response.ok) {
            alert('Đổi mật khẩu thành công!');
            
            // Reset form
            document.getElementById('passwordForm').reset();
            
            // Close modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('passwordModal'));
            if (modal) {
                modal.hide();
            }
        } else {
            alert('Lỗi: ' + result.message);
        }
    } catch (error) {
        console.error('Lỗi khi đổi mật khẩu:', error);
        alert('Không thể đổi mật khẩu. Vui lòng thử lại!');
    }
}

// Hàm upload ảnh riêng
async function uploadAvatar(file) {
    try {
        console.log('=== UPLOADING AVATAR ===');
        
        // Tạo FormData
        const formData = new FormData();
        formData.append('avatar', file);

        // Hiển thị loading
        const btnUpdate = document.querySelector('#edit-avatar-buttons .btn-success');
        const originalBtnText = btnUpdate ? btnUpdate.innerHTML : '';
        if (btnUpdate) {
            btnUpdate.disabled = true;
            btnUpdate.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i> Đang tải...';
        }

        // Thêm class loading cho avatar
        const avatarBox = document.querySelector('.avatar-box');
        if (avatarBox) {
            avatarBox.classList.add('avatar-loading');
        }

        // Gọi API upload
        const response = await fetch('/API/updateAvatar', {
            method: 'POST',
            body: formData
        });

        const result = await response.json();

        if (response.ok) {
            console.log('Avatar uploaded successfully:', result);
            
            // Cập nhật anhId trong dbData
            if (result.anhId) {
                dbData.NhanVien.anhId = result.anhId;
            }

            // Cập nhật ảnh hiển thị với cache busting để force reload
            if (result.imageUrl) {
                els.imgAvatar.src = result.imageUrl;
            } else if (result.anhId) {
                // Thêm timestamp để force reload ảnh mới
                els.imgAvatar.src = `/API/getHinhAnh/${result.anhId}?t=${new Date().getTime()}`;
            }
            
            // Hiển thị thông báo thành công
            alert(result.message || 'Cập nhật ảnh đại diện thành công!');
            
            // Khôi phục nút
            if (btnUpdate) {
                btnUpdate.disabled = false;
                btnUpdate.innerHTML = originalBtnText;
            }

            // Remove loading class
            if (avatarBox) {
                avatarBox.classList.remove('avatar-loading');
            }

            // Reset input file
            els.fileUpload.value = '';

            return true; // Success
        } else {
            console.error('Upload failed:', result);
            alert('Lỗi: ' + result.message);
            
            // Khôi phục ảnh cũ
            if (originalAvatarSrc) {
                els.imgAvatar.src = originalAvatarSrc;
            }
            
            // Khôi phục nút
            if (btnUpdate) {
                btnUpdate.disabled = false;
                btnUpdate.innerHTML = originalBtnText;
            }

            // Remove loading class
            if (avatarBox) {
                avatarBox.classList.remove('avatar-loading');
            }

            // Reset input file
            els.fileUpload.value = '';

            return false; // Failed
        }
    } catch (error) {
        console.error('Error uploading avatar:', error);
        alert('Không thể upload ảnh. Vui lòng thử lại!');
        
        // Khôi phục ảnh cũ
        if (originalAvatarSrc) {
            els.imgAvatar.src = originalAvatarSrc;
        }
        
        // Remove loading class
        const avatarBox = document.querySelector('.avatar-box');
        if (avatarBox) {
            avatarBox.classList.remove('avatar-loading');
        }

        // Khôi phục nút
        const btnUpdate = document.querySelector('#edit-avatar-buttons .btn-success');
        if (btnUpdate) {
            btnUpdate.disabled = false;
            btnUpdate.innerHTML = '<i class="fas fa-check me-1"></i> Cập nhật ảnh';
        }
        
        // Reset input file
        els.fileUpload.value = '';

        return false; // Failed
    }
}

function showNotification(message, type = 'info') {

    if (type === 'success') {
        console.log('✅ ' + message);
    }
}
