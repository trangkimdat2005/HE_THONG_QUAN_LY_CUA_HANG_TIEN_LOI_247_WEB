document.addEventListener('DOMContentLoaded', function () {

    // --- KHỞI TẠO SELECT2 CHO CÁC Ô CHỌN ---
    if (typeof $ !== 'undefined' && $.fn.select2) {
        $('#select-danh-muc').select2({
            width: '100%',
            placeholder: 'Chọn một hoặc nhiều danh mục'
        });
        $('#select-san-pham').select2({
            width: '100%',
            placeholder: 'Chọn một hoặc nhiều sản phẩm'
        });
    }

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const selectHinhThucGiam = document.getElementById('select-hinh-thuc-giam');
    const labelGiaTri = document.getElementById('label-gia-tri');
    const fieldGiamToiDa = document.getElementById('field-giam-toi-da');

    const radioPhamVi = document.querySelectorAll('input[name="phamViApDung"]');
    const fieldChonDanhMuc = document.getElementById('field-chon-danh-muc');
    const fieldChonSanPham = document.getElementById('field-chon-san-pham');

    // Form inputs
    const tenChuongTrinhInput = document.getElementById('ten-chuong-trinh');
    const moTaInput = document.getElementById('mo-ta');
    const maCodeInput = document.getElementById('ma-code-input');
    const giaTriGiamInput = document.getElementById('gia-tri-giam');
    const giamToiDaInput = document.getElementById('giam-toi-da');
    const donHangToiThieuInput = document.getElementById('don-hang-toi-thieu');
    const soLanSuDungInput = document.getElementById('so-lan-su-dung');
    const trangThaiSelect = document.getElementById('trang-thai');
    const ngayBatDauInput = document.getElementById('ngay-bat-dau');
    const ngayKetThucInput = document.getElementById('ngay-ket-thuc');
    const btnLuu = document.getElementById('btn-luu');

    // --- LOGIC 1: THAY ĐỔI HÌNH THỨC GIẢM (%) HOẶC (VND) ---
    selectHinhThucGiam.addEventListener('change', function () {
        if (this.value === 'Phần trăm') {
            // Nếu là %, hiển thị ô "Giảm tối đa" và đổi nhãn
            labelGiaTri.textContent = 'Giá trị giảm (%) ';
            fieldGiamToiDa.classList.remove('form-field-hidden');
        } else {
            // Nếu là VND, ẩn ô "Giảm tối đa" và đổi nhãn
            labelGiaTri.textContent = 'Giá trị giảm (VND) ';
            fieldGiamToiDa.classList.add('form-field-hidden');
            giamToiDaInput.value = 0; // Reset giá trị về 0 cho an toàn
        }
    });
    // Kích hoạt sự kiện change một lần lúc tải trang để đảm bảo trạng thái đúng
    selectHinhThucGiam.dispatchEvent(new Event('change'));


    // --- LOGIC 2: THAY ĐỔI PHẠM VI ÁP DỤNG ---
    radioPhamVi.forEach(radio => {
        radio.addEventListener('change', function () {
            // Ẩn tất cả các trường chọn trước
            fieldChonDanhMuc.classList.add('form-field-hidden');
            fieldChonSanPham.classList.add('form-field-hidden');

            if (this.value === 'DanhMuc') {
                fieldChonDanhMuc.classList.remove('form-field-hidden');
            } else if (this.value === 'SanPham') {
                fieldChonSanPham.classList.remove('form-field-hidden');
            }
            // Nếu là 'ToanBo' thì không làm gì cả (vì đã ẩn hết)
        });
    });
    // Kích hoạt lần đầu
    document.querySelector('input[name="phamViApDung"]:checked').dispatchEvent(new Event('change'));

    // --- LOGIC 3: TẠO MÃ NGẪU NHIÊN ---
    document.getElementById('generate-code-btn').addEventListener('click', function () {
        const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
        let result = '';
        for (let i = 0; i < 8; i++) {
            result += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        maCodeInput.value = result;
    });

    // --- LOGIC 4: NÚT LƯU - GỌI API THÊM CHƯƠNG TRÌNH KHUYẾN MÃI ---
    if (btnLuu) {
        btnLuu.addEventListener('click', async function () {
            if (!tenChuongTrinhInput.value.trim()) {
                alert('Vui lòng nhập tên chương trình!');
                tenChuongTrinhInput.focus();
                return;
            }
            if (!maCodeInput.value.trim()) {
                alert('Vui lòng nhập hoặc tạo mã khuyến mãi!');
                maCodeInput.focus();
                return;
            }
            if (!giaTriGiamInput.value || parseFloat(giaTriGiamInput.value) <= 0) {
                alert('Vui lòng nhập giá trị giảm hợp lệ!');
                giaTriGiamInput.focus();
                return;
            }
            if (!ngayBatDauInput.value || !ngayKetThucInput.value) {
                alert('Vui lòng chọn ngày bắt đầu và ngày kết thúc!');
                return;
            }
            if (new Date(ngayKetThucInput.value) <= new Date(ngayBatDauInput.value)) {
                alert('Ngày kết thúc phải sau ngày bắt đầu!');
                return;
            }

            // Lấy phạm vi áp dụng
            const phamViApDung = document.querySelector('input[name="phamViApDung"]:checked').value;
            let danhMucIds = [];
            let sanPhamIds = [];

            if (phamViApDung === 'DanhMuc') {
                danhMucIds = $('#select-danh-muc').val() || [];
                if (danhMucIds.length === 0) {
                    alert('Vui lòng chọn ít nhất một danh mục!');
                    return;
                }
            } else if (phamViApDung === 'SanPham') {
                sanPhamIds = $('#select-san-pham').val() || [];
                if (sanPhamIds.length === 0) {
                    alert('Vui lòng chọn ít nhất một sản phẩm!');
                    return;
                }
            }

            // === TẠO OBJECT REQUEST (ĐÃ SỬA LỖI) ===

            // Lấy các giá trị 1 lần cho dễ đọc
            const hinhThucGiam = selectHinhThucGiam.value; // Sẽ là "PhanTram" hoặc"SoTien"
            const giaTriGiam = parseFloat(giaTriGiamInput.value) || 0;

            const requestData = {
                Ten: tenChuongTrinhInput.value.trim(),

                // SỬA 1: Gửi "PhanTram" hoặc "SoTien" (viết liền) khớp với value của <option>
                Loai: hinhThucGiam,

                NgayBatDau: ngayBatDauInput.value,
                NgayKetThuc: ngayKetThucInput.value,
                MoTa: moTaInput.value.trim() || null,
                DieuKienApDung: {
                    DieuKien: `Giam ${hinhThucGiam === 'Phần trăm' ? giaTriGiam + '%' : giaTriGiam + 'VND'}`,
                    GiaTriToiThieu: parseFloat(donHangToiThieuInput.value) || 0,
                    GiamTheo: hinhThucGiam, // Giờ 'Loai' và 'GiamTheo' đã đồng nhất

                    // SỬA 2: Nếu là 'SoTien', GiaTriToiDa phải là 0.
                    GiaTriToiDa: hinhThucGiam === 'Phần trăm' ? (parseFloat(giamToiDaInput.value) || 0) : 999999999
                },
                MaKhuyenMai: {
                    Code: maCodeInput.value.trim(),
                    GiaTri: giaTriGiam,
                    SoLanSuDung: parseInt(soLanSuDungInput.value) || 100,
                    TrangThai: trangThaiSelect.value
                },
                PhamViApDung: phamViApDung,
                DanhMucIds: danhMucIds,
                SanPhamIds: sanPhamIds
            };

            console.log('Request data (đã sửa):', JSON.stringify(requestData, null, 2));

            // === PHẦN GỌI API CỦA BẠN (RẤT TỐT, GIỮ NGUYÊN) ===
            try {
                // Hiển thị loading
                const originalHTML = btnLuu.innerHTML;
                btnLuu.disabled = true;
                btnLuu.innerHTML = '<i class="bi bi-hourglass-split me-2"></i>Đang lưu...'; // Dùng icon 'bi' cho nhất quán

                // Gọi API
                const response = await fetch('/API/add-CTKM', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                        // Lưu ý: Có thể bạn cần thêm RequestVerificationToken nếu có dùng
                    },
                    body: JSON.stringify(requestData)
                });

                console.log('Response status:', response.status);

                const contentType = response.headers.get('content-type');
                let result;

                if (contentType && contentType.includes('application/json')) {
                    result = await response.json();
                    console.log('Response JSON:', result);
                } else {
                    const text = await response.text();
                    console.error('Response is not JSON:', text);
                    throw new Error('Server không trả về JSON. Nội dung: ' + text);
                }

                if (response.ok) {
                    console.log('SUCCESS:', result.message);
                    alert(result.message || 'Thêm chương trình khuyến mãi thành công!');

                    // Chuyển về trang danh sách
                    window.location.href = '/QuanLyKhuyenMai/KhuyenMai';
                } else {
                    console.error('❌ ERROR:', result.message);
                    alert('Lỗi: ' + (result.message || 'Không thể thêm chương trình khuyến mãi'));

                    btnLuu.disabled = false;
                    btnLuu.innerHTML = originalHTML;
                }
            } catch (error) {
                console.error('EXCEPTION:', error);
                alert('Có lỗi nghiêm trọng xảy ra khi lưu:\n\n' + error.message);

                btnLuu.disabled = false;
                // Trả lại HTML gốc của nút
                btnLuu.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Lưu';
            }
        });
    }

});