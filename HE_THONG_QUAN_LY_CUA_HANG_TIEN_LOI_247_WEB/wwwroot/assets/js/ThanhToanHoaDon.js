document.addEventListener('DOMContentLoaded', function () {
    console.log('=== THANH TOÁN HÓA ĐƠN PAGE (POS) LOADED ===');

    // === BIẾN TOÀN CỤC ===
    let selectedKenhThanhToanId = null;
    let selectedKenhThanhToanType = null;

    // === ELEMENTS ===
    const hoaDonId = document.getElementById('hoa-don-id')?.value;
    const tongTien = parseFloat(document.getElementById('tong-tien')?.value || 0);
    const nhanVienId = document.getElementById('nhan-vien-id')?.value;

    // Cột trái
    const paymentMethodButtons = document.querySelectorAll('.payment-method-btn');

    // Cột phải
    const cashPanel = document.getElementById('cash-payment-panel');
    const qrPanel = document.getElementById('qr-payment-panel');

    // Elements trong Panel Tiền Mặt
    const cashReceivedInput = document.getElementById('cash-received');
    const changeDisplay = document.getElementById('change-display');
    const suggestionButtons = document.querySelectorAll('.btn-suggestion');
    const paymentNoteCash = document.getElementById('payment-note-cash');
    const completePaymentBtnCash = document.getElementById('complete-payment-btn-cash');

    // Elements trong Panel QR
    const paymentNoteQr = document.getElementById('payment-note-qr');
    const completePaymentBtnQr = document.getElementById('complete-payment-btn-qr');


    // === FORMAT TIỀN TỆ ===
    const formatter = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    });

    // === HÀM HIỂN THỊ PANEL ===
    function showPanel(panelType) {
        console.log('Showing panel:', panelType);
        
        // Chuẩn hóa tên panel (xóa gạch dưới)
        const normalizedType = panelType.replace(/_/g, '').toLowerCase();
        
        // Ẩn TẤT CẢ các panel trước
        if (cashPanel) cashPanel.classList.add('d-none');
        if (qrPanel) qrPanel.classList.add('d-none');
        
        // Hiện panel được chọn
        if (normalizedType === 'qr' && cashPanel) {
            qrPanel.classList.remove('d-none');
            console.log('✅ QR panel visible');
        } else if (normalizedType !== 'qr' && qrPanel) {
            cashPanel.classList.remove('d-none');
            console.log('✅ Cash panel visible');
        }
    }

    // === 1. CHỌN PHƯƠNG THỨC THANH TOÁN ===
    paymentMethodButtons.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();

            selectedKenhThanhToanId = this.dataset.id;
            // Chuẩn hóa: xóa gạch dưới và chuyển thường
            selectedKenhThanhToanType = (this.dataset.type || '').replace(/_/g, '').toLowerCase().trim();

            console.log('Selected payment method:', selectedKenhThanhToanType);

            // Cập nhật trạng thái active cho nút
            paymentMethodButtons.forEach(b => b.classList.remove('active'));
            this.classList.add('active');

            // Hiển thị panel tương ứng
            showPanel(selectedKenhThanhToanType);
        });
    });

    // === 2. GỢI Ý MỆNH GIÁ (cho Tiền Mặt) ===
    suggestionButtons.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            const amount = parseFloat(this.getAttribute('data-amount'));
            cashReceivedInput.value = amount;
            calculateChange();
        });
    });

    // === 3. TÍNH TIỀN THỐI (cho Tiền Mặt) ===
    if (cashReceivedInput) {
        cashReceivedInput.addEventListener('input', calculateChange);
    }

    function calculateChange() {
        const cashReceived = parseFloat(cashReceivedInput.value) || 0;

        if (cashReceived === 0) {
            changeDisplay.textContent = 'Vui lòng nhập tiền khách đưa';
            changeDisplay.className = 'pos-change-box';
            return;
        }

        const change = cashReceived - tongTien;

        if (change >= 0) {
            changeDisplay.textContent = formatter.format(change);
            changeDisplay.className = 'pos-change-box success';
        } else {
            changeDisplay.textContent = 'Còn thiếu: ' + formatter.format(Math.abs(change));
            changeDisplay.className = 'pos-change-box error';
        }
    }

    // === 4. HOÀN TẤT THANH TOÁN ===

    // Nút 1: Hoàn tất bằng tiền mặt
    if (completePaymentBtnCash) {
        completePaymentBtnCash.addEventListener('click', function () {
            const normalizedType = selectedKenhThanhToanType.replace(/_/g, '').toLowerCase();
            
            if (normalizedType !== 'tiền mặt') {
                alert('Vui lòng chọn phương thức thanh toán Tiền Mặt!');
                return;
            }

            const cashReceived = parseFloat(cashReceivedInput.value) || 0;
            if (cashReceived < tongTien) {
                alert('Số tiền khách đưa không đủ!');
                return;
            }

            const note = paymentNoteCash.value;
            submitPayment(selectedKenhThanhToanId, note, this);
        });
    }

    // Nút 2: Hoàn tất bằng QR
    if (completePaymentBtnQr) {
        completePaymentBtnQr.addEventListener('click', function () {
            const normalizedType = selectedKenhThanhToanType.replace(/_/g, '').toLowerCase();
            
            if (normalizedType === 'tiền mặt') {
                alert('Vui lòng chọn phương thức thanh toán QR!');
                return;
            }

            const note = paymentNoteQr.value;
            submitPayment(selectedKenhThanhToanId, note, this);
        });
    }


    // === 5. HÀM GỬI API ===
    async function submitPayment(kenhId, ghiChu, buttonElement) {

        if (!kenhId) {
            alert('Lỗi: Chưa chọn phương thức thanh toán.');
            return;
        }

        if (!nhanVienId) {
            alert('Lỗi: Không tìm thấy ID nhân viên. Vui lòng tải lại trang.');
            return;
        }

        if (!confirm('Xác nhận hoàn tất thanh toán?')) {
            return;
        }

        const originalButtonHtml = buttonElement.innerHTML;
        buttonElement.disabled = true;
        buttonElement.innerHTML = '<i class="spinner-border spinner-border-sm me-2"></i>Đang xử lý...';

        try {
            const response = await fetch('/API/thanh-toan-hoa-don', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    HoaDonId: hoaDonId,
                    KenhThanhToanId: kenhId,
                    MoTa: ghiChu,
                    NhanVienId: nhanVienId,
                    SoTien: tongTien
                })
            });

            const result = await response.json();

            if (response.ok) {
                alert(result.message);
                window.location.href = '/QuanLyHoaDon/DanhSachHoaDon';
            } else {
                alert('Lỗi: ' + result.message);
                buttonElement.disabled = false;
                buttonElement.innerHTML = originalButtonHtml;
            }
        } catch (error) {
            alert('Lỗi khi thanh toán: ' + error.message);
            console.error(error);
            buttonElement.disabled = false;
            buttonElement.innerHTML = originalButtonHtml;
        }
    }

    // === 6. KHỞI TẠO MẶC ĐỊNH ===
    function initializeDefaultPayment() {
        console.log('=== INITIALIZING DEFAULT PAYMENT ===');
        console.log('Total payment buttons:', paymentMethodButtons.length);
        
        // Debug: In ra tất cả các nút
        paymentMethodButtons.forEach((btn, index) => {
            console.log(`Button ${index}:`, {
                type: btn.dataset.type,
                name: btn.dataset.name,
                id: btn.dataset.id
            });
        });

        // Ẩn TẤT CẢ các panel trước khi khởi tạo
        if (cashPanel) cashPanel.classList.add('d-none');
        if (qrPanel) qrPanel.classList.add('d-none');

        // Tìm nút Tiền Mặt (hỗ trợ cả "TienMat" và "tien_mat")
        const cashButton = Array.from(paymentMethodButtons).find(btn => {
            const type = (btn.dataset.type || '').replace(/_/g, '').toLowerCase().trim();
            return type === 'tiền mặt';
        });

        if (cashButton) {
            console.log('✅ Found Cash button, auto-selecting...');
            setTimeout(() => {
                cashButton.click();
            }, 100);
        } else {
            console.warn('⚠️ Cash button not found!');
            if (paymentMethodButtons.length > 0) {
                console.log('Selecting first payment method as fallback');
                setTimeout(() => {
                    paymentMethodButtons[0].click();
                }, 100);
            } else {
                console.error('❌ No payment buttons found!');
            }
        }
    }

    // Chạy khởi tạo
    initializeDefaultPayment();

    console.log('Hóa đơn ID:', hoaDonId);
    console.log('Tổng tiền:', tongTien);
    console.log('Nhân viên ID:', nhanVienId);
});