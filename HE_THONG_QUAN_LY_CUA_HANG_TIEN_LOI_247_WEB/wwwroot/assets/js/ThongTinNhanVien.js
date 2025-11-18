let dbData = null;  // Định nghĩa dbData ở phạm vi toàn cục
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
                anhId: fetchedData.anh || "https://i.pravatar.cc/300?img=11"  // Default image if not available
            }
        };

        // Kiểm tra lại dbData trước khi gọi initView
        if (dbData && dbData.NhanVien) {
            initView();
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

    if (dbData.NhanVien.anhId && dbData.NhanVien.anhId.startsWith('data:image')) {
        els.imgAvatar.src = dbData.NhanVien.anhId;
    }
    else {
        els.imgAvatar.src = 'https://i.pravatar.cc/300?img=11';
    }

    updateGenderDisplay(dbData.NhanVien.gioiTinh);

    const statusBadge = dbData.TaiKhoan.trangThai === "active"
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

function saveData() {
    dbData.NhanVien.hoTen = els.inpHoten.value;
    dbData.NhanVien.email = els.inpEmail.value;
    dbData.NhanVien.soDienThoai = els.inpSdt.value;
    dbData.NhanVien.diaChi = els.inpDiachi.value;
    dbData.NhanVien.gioiTinh = els.inpGioitinh.value === 'true';

    els.lblHotenCard.innerText = dbData.NhanVien.hoTen;
    els.lblHotenDetail.innerText = dbData.NhanVien.hoTen;
    els.lblEmail.innerText = dbData.NhanVien.email;
    els.lblSdt.innerText = dbData.NhanVien.soDienThoai;
    els.lblDiachi.innerText = dbData.NhanVien.diaChi;

    updateGenderDisplay(dbData.NhanVien.gioiTinh);

    alert('Đã cập nhật thông tin thành công!');
    toggleEditMode(false);
}

function cancelEdit() {
    toggleEditMode(false);
}

els.fileUpload.addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            els.imgAvatar.src = e.target.result;
            dbData.NhanVien.anhId = e.target.result;
        }
        reader.readAsDataURL(file);
    }
});


async function callApiGetThongTinNhanVienData() {

    try {
        const response = await fetch('/API/getThongTinNhanVien',
            {
                method: 'GET'
            });
        if (!response.ok) {
            throw new Error(errorMessage);
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
