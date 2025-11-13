document.addEventListener('DOMContentLoaded', function () {

    // === CÁC BIẾN DOM ===
    const totalAmountDisplay = document.getElementById('total-amount-display');
    const cashReceivedInput = document.getElementById('cash-received');
    const changeDisplay = document.getElementById('change-display');
    const suggestionButtons = document.querySelectorAll('.btn-suggestion');

    // Nút chuyển tab và Panels
    const btnCash = document.getElementById('btn-cash-method');
    const btnQR = document.getElementById('btn-qr-method');
    const cashPanel = document.getElementById('cash-payment-panel');
    const qrPanel = document.getElementById('qr-payment-panel');
    const qrImage = document.getElementById('qr-code-image');
    
    // LẤY GIÁ TRỊ ĐỘNG TỪ RAZOR/HTML
    // Dùng parseFloat để đảm bảo xử lý số thập phân nếu có, tuy nhiên dữ liệu của bạn là số nguyên.
    const TOTAL_AMOUNT = parseFloat(totalAmountDisplay.dataset.value) || 0; 
    
    // Lấy ID Hóa đơn và Tên tài khoản để tạo QR Code động
    // Giả định bạn đã đặt ID hóa đơn vào một data attribute trên thẻ cha nào đó (hoặc lấy từ QR Image URL)
    const HOA_DON_ID = qrImage.getAttribute('src').split('&addInfo=')[1]; 
    const MERCHANT_NAME = "CTK%20NGUYEN%20VAN%20A"; // Giữ nguyên tên người nhận

    // === BỘ ĐỊNH DẠNG TIỀN TỆ ===
    const formatter = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND',
        minimumFractionDigits: 0 // Bỏ số thập phân nếu tiền là số nguyên
    });

    // === KHỞI TẠO VÀ CẬP NHẬT GIAO DIỆN ===
    
    // 1. Cập nhật Tiêu đề QR Code (vì ban đầu nó được hard-code)
    const totalAmountFormatted = formatter.format(TOTAL_AMOUNT);
    qrPanel.querySelector('p strong').textContent = totalAmountFormatted;

    // 2. Cập nhật QR Code URL đầy đủ (chắc chắn khớp với Tổng tiền và ID)
    if (qrImage) {
        const newQrSrc = `https://api.vietqr.io/image/970436-1133666888-${MERCHANT_NAME}?amount=${TOTAL_AMOUNT}&addInfo=${HOA_DON_ID}`;
        qrImage.src = newQrSrc;
    }


    // === HÀM TÍNH TOÁN TIỀN THỐI ===
    function updateChange() {
        // Dùng parseFloat để xử lý tiền khách đưa, loại bỏ vấn đề dấu phẩy/chấm
        const cashGiven = parseFloat(cashReceivedInput.value.replace(/\./g, '').replace(/,/g, '.')) || 0;

        if (cashGiven === 0) {
            changeDisplay.innerHTML = `<span class="change-value missing">Còn thiếu: ${formatter.format(TOTAL_AMOUNT)}</span>`;
            return;
        }

        const change = cashGiven - TOTAL_AMOUNT;

        if (change < 0) {
            changeDisplay.innerHTML = `<span class="change-value missing">Còn thiếu: ${formatter.format(Math.abs(change))}</span>`;
        } else {
            changeDisplay.innerHTML = `<span class="change-value change">Tiền thối: ${formatter.format(change)}</span>`;
        }
    }

    // === GÁN SỰ KIỆN ===
    // 1. Gõ vào ô tiền khách đưa
    cashReceivedInput.addEventListener('input', updateChange);

    // 2. Nhấn nút gợi ý
    suggestionButtons.forEach(button => {
        button.addEventListener('click', function () {
            const amount = this.dataset.amount;
            cashReceivedInput.value = amount;
            // Kích hoạt sự kiện 'input' để JS tự tính lại tiền
            cashReceivedInput.dispatchEvent(new Event('input'));
        });
    });

    // 3. Chuyển tab Tiền Mặt
    btnCash.addEventListener('click', function (e) {
        e.preventDefault();
        cashPanel.classList.remove('d-none');
        qrPanel.classList.add('d-none');

        // Cập nhật style nút
        btnCash.classList.add('btn-success');
        btnCash.classList.remove('btn-outline-primary');
        btnQR.classList.add('btn-outline-primary');
        btnQR.classList.remove('btn-success');
    });

    // 4. Chuyển tab QR
    btnQR.addEventListener('click', function (e) {
        e.preventDefault();
        cashPanel.classList.add('d-none');
        qrPanel.classList.remove('d-none');

        // Cập nhật style nút
        btnQR.classList.add('btn-success');
        btnQR.classList.remove('btn-outline-primary');
        btnCash.classList.add('btn-outline-primary');
        btnCash.classList.remove('btn-success');
    });

    // === CHẠY LẦN ĐẦU KHI TẢI TRANG ===
    // Đảm bảo Tiền thối được tính toán chính xác ngay từ đầu
    updateChange();
});