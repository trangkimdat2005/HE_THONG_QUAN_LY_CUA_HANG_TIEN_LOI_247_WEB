document.addEventListener('DOMContentLoaded', function () {

    // --- KHỞI TẠO SELECT2 ---
    $('#select-danh-muc').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều danh mục'
    });
    $('#select-san-pham').select2({
        width: '100%',
        placeholder: 'Chọn một hoặc nhiều sản phẩm'
    });

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const selectHinhThucGiam = document.getElementById('select-hinh-thuc-giam');
    const labelGiaTri = document.getElementById('label-gia-tri');
    const fieldGiamToiDa = document.getElementById('field-giam-toi-da');

    const radioPhamVi = document.querySelectorAll('input[name="phamViApDung"]');
    const fieldChonDanhMuc = document.getElementById('field-chon-danh-muc');
    const fieldChonSanPham = document.getElementById('field-chon-san-pham');

    // Form inputs
    const chuongTrinhIdInput = document.getElementById('chuong-trinh-id');
    const tenChuongTrinhInput = document.getElementById('ten-chuong-trinh');
    const moTaInput = document.getElementById('mo-ta');
    const ngayBatDauInput = document.getElementById('ngay-bat-dau');
    const ngayKetThucInput = document.getElementById('ngay-ket-thuc');
    const btnLuuThayDoi = document.getElementById('btn-luu-thay-doi');

    // --- LẤY ID TỪ URL HOẶC HIDDEN INPUT ---
    const urlParams = new URLSearchParams(window.location.search);
    let chuongTrinhId = urlParams.get('id') || (chuongTrinhIdInput ? chuongTrinhIdInput.value : null);

    console.log('Chương trình ID:', chuongTrinhId);

    // --- LOAD DỮ LIỆU CHƯƠNG TRÌNH KHUYẾN MÃI ---
    if (chuongTrinhId) {
        loadChuongTrinhKhuyenMai(chuongTrinhId);
    } else {
        console.warn('Không tìm thấy ID chương trình khuyến mãi');
        alert('Không tìm thấy ID chương trình khuyến mãi!');
    }

    // --- LOGIC: THAY ĐỔI HÌNH THỨC GIẢM ---
    if (selectHinhThucGiam) {
        selectHinhThucGiam.addEventListener('change', function () {
            if (this.value === 'PhanTram') {
                labelGiaTri.textContent = 'Giá trị giảm (%) ';
                fieldGiamToiDa.classList.remove('form-field-hidden');
            } else {
                labelGiaTri.textContent = 'Giá trị giảm (VND) ';
                fieldGiamToiDa.classList.add('form-field-hidden');
            }
        });
        selectHinhThucGiam.dispatchEvent(new Event('change'));
    }

    // --- LOGIC: THAY ĐỔI PHẠM VI ÁP DỤNG ---
    radioPhamVi.forEach(radio => {
        radio.addEventListener('change', function () {
            fieldChonDanhMuc.classList.add('form-field-hidden');
            fieldChonSanPham.classList.add('form-field-hidden');

            if (this.value === 'DanhMuc') {
                fieldChonDanhMuc.classList.remove('form-field-hidden');
            } else if (this.value === 'SanPham') {
                fieldChonSanPham.classList.remove('form-field-hidden');
            }
        });
    });

    // --- HÀM LOAD DỮ LIỆU ---
    async function loadChuongTrinhKhuyenMai(id) {
        try {
            console.log('Loading chương trình khuyến mãi ID:', id);

            const response = await fetch(`/API/get-CTKM-by-id?id=${encodeURIComponent(id)}`);

            if (!response.ok) {
                throw new Error('Không thể tải dữ liệu chương trình khuyến mãi');
            }

            const data = await response.json();
            console.log('Loaded data:', data);

            // Đổ dữ liệu vào form
            if (tenChuongTrinhInput) tenChuongTrinhInput.value = data.ten || '';
            if (moTaInput) moTaInput.value = data.moTa || '';

            // Loại khuyến mãi
            if (data.loai && selectHinhThucGiam) {
                if (data.loai.toLowerCase().includes('phan tram')) {
                    selectHinhThucGiam.value = 'PhanTram';
                } else {
                    selectHinhThucGiam.value = 'SoTien';
                }
                selectHinhThucGiam.dispatchEvent(new Event('change'));
            }

            // Ngày tháng - convert DateOnly to YYYY-MM-DD format
            if (data.ngayBatDau && ngayBatDauInput) {
                // Nếu là DateOnly từ C#, format: "2025-01-15"
                const ngayBatDau = data.ngayBatDau.split('T')[0]; // Lấy phần ngày
                ngayBatDauInput.value = ngayBatDau;
            }
            if (data.ngayKetThuc && ngayKetThucInput) {
                const ngayKetThuc = data.ngayKetThuc.split('T')[0];
                ngayKetThucInput.value = ngayKetThuc;
            }

        } catch (error) {
            console.error('Error loading data:', error);
            alert('Có lỗi khi tải dữ liệu chương trình khuyến mãi: ' + error.message);
        }
    }

    // --- NÚT LƯU THAY ĐỔI ---
    if (btnLuuThayDoi && chuongTrinhId) {
        btnLuuThayDoi.addEventListener('click', async function() {
            console.log('=== BẮT ĐẦU LƯU THAY ĐỔI ===');

            try {
                // Lấy dữ liệu từ form
                const tenChuongTrinh = tenChuongTrinhInput.value.trim();
                const moTa = moTaInput.value.trim();
                const loai = selectHinhThucGiam.value === 'PhanTram' ? 'Phan tram' : 'So tien';
                const ngayBatDau = ngayBatDauInput.value;
                const ngayKetThuc = ngayKetThucInput.value;

                // Validate
                if (!tenChuongTrinh) {
                    alert('Vui lòng nhập tên chương trình!');
                    tenChuongTrinhInput.focus();
                    return;
                }

                if (!ngayBatDau || !ngayKetThuc) {
                    alert('Vui lòng chọn ngày bắt đầu và ngày kết thúc!');
                    return;
                }

                // Tạo object request
                const requestData = {
                    Id: chuongTrinhId,
                    Ten: tenChuongTrinh,
                    Loai: loai,
                    NgayBatDau: ngayBatDau,
                    NgayKetThuc: ngayKetThuc,
                    MoTa: moTa || null,
                    IsDelete: false
                };

                console.log('Request data:', requestData);

                // Hiển thị loading
                btnLuuThayDoi.disabled = true;
                const originalHTML = btnLuuThayDoi.innerHTML;
                btnLuuThayDoi.innerHTML = '<i class="fas fa-hourglass-half me-2"></i>Đang lưu...';

                // Gọi API
                const response = await fetch('/API/edit-CTKM', {
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
                    alert(result.message || 'Cập nhật chương trình khuyến mãi thành công!');
                    
                    // Chuyển về trang danh sách
                    window.location.href = '/QuanLyKhuyenMai/KhuyenMai';
                } else {
                    console.error('❌ ERROR:', result.message);
                    alert('Lỗi: ' + (result.message || 'Không thể cập nhật chương trình khuyến mãi'));
                    
                    btnLuuThayDoi.disabled = false;
                    btnLuuThayDoi.innerHTML = originalHTML;
                }
            } catch (error) {
                console.error('❌ EXCEPTION:', error);
                alert('Có lỗi xảy ra khi cập nhật: ' + error.message);
                
                btnLuuThayDoi.disabled = false;
                btnLuuThayDoi.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Lưu thay đổi';
            }
        });
    }

});