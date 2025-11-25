$(document).ready(function () {
    // --- 1. KHỞI TẠO BIẾN ---
    let currentStartDate = new Date(); // Mặc định là ngày hiện tại
    // Điều chỉnh về Thứ 2 đầu tuần
    const day = currentStartDate.getDay();
    const diff = currentStartDate.getDate() - day + (day === 0 ? -6 : 1);
    currentStartDate.setDate(diff);
    currentStartDate.setHours(0, 0, 0, 0); // Reset giờ về 0 để so sánh chính xác

    const shiftDefs = [
        { id: 'CA_SANG', name: 'Ca Sáng', time: '(06:00 - 14:00)' },
        { id: 'CA_CHIEU', name: 'Ca Chiều', time: '(14:00 - 22:00)' },
        { id: 'CA_TOI', name: 'Ca Tối', time: '(22:00 - 05:00)' }
    ];

    // --- 2. HÀM HỖ TRỢ ---

    // Format ngày dd/mm/yyyy
    function formatDate(date) {
        let d = date.getDate();
        let m = date.getMonth() + 1;
        let y = date.getFullYear();
        return (d < 10 ? '0' + d : d) + '/' + (m < 10 ? '0' + m : m) + '/' + y;
    }

    // Render bảng lịch
    function renderSchedule() {
        // 1. Cập nhật hiển thị tuần trên toolbar
        let endDate = new Date(currentStartDate);
        endDate.setDate(currentStartDate.getDate() + 6);
        $('#currentWeekDisplay').text(`${formatDate(currentStartDate)} - ${formatDate(endDate)}`);

        // 2. Render Header (Thứ 2 -> CN)
        let headerHtml = '<th>Ca làm việc</th>';
        let today = new Date();
        today.setHours(0, 0, 0, 0);

        let weekDates = []; // Lưu lại mảng ngày để dùng render body

        for (let i = 0; i < 7; i++) {
            let tempDate = new Date(currentStartDate);
            tempDate.setDate(currentStartDate.getDate() + i);
            weekDates.push(tempDate);

            let dayName = i === 6 ? 'Chủ Nhật' : `Thứ ${i + 2}`;
            headerHtml += `<th>${dayName}<br>${formatDate(tempDate)}</th>`;
        }
        $('#tableHeaderRow').html(headerHtml);

        // 3. Render Body (3 dòng Ca)
        let bodyHtml = '';
        shiftDefs.forEach(shift => {
            bodyHtml += `<tr>`;
            bodyHtml += `<td>${shift.name}<br><small>${shift.time}</small></td>`;

            // Render 7 ô checkbox
            for (let i = 0; i < 7; i++) {
                let cellDate = weekDates[i];
                let isPast = cellDate < today; // Kiểm tra ngày quá khứ
                let disabledAttr = isPast ? 'disabled' : '';
                let disabledClass = isPast ? 'cell-disabled' : '';

                // data-day-index: 0=Thứ 2, 6=CN (Dùng để nhóm cột)
                // data-shift-id: ID của ca
                bodyHtml += `
                        <td class="${disabledClass}">
                            <input type="checkbox" 
                                   class="custom-checkbox shift-checkbox" 
                                   data-day-index="${i}" 
                                   data-shift-id="${shift.id}"
                                   data-date="${formatDate(cellDate)}"
                                   ${disabledAttr}>
                        </td>`;
            }
            bodyHtml += `</tr>`;
        });
        $('#tableBody').html(bodyHtml);
    }

    // --- 3. LOGIC SỰ KIỆN ---

    // Khởi chạy lần đầu
    renderSchedule();

    // Nút Tuần trước
    $('#btnPrevWeek').click(function () {
        currentStartDate.setDate(currentStartDate.getDate() - 7);
        renderSchedule();
    });

    // Nút Tuần sau
    $('#btnNextWeek').click(function () {
        currentStartDate.setDate(currentStartDate.getDate() + 7);
        renderSchedule();
    });

    // LOGIC QUAN TRỌNG: 1 Ngày chỉ chọn 1 Ca
    $(document).on('change', '.shift-checkbox', function () {
        if ($(this).is(':checked')) {
            // Lấy index của cột (ngày)
            let dayIndex = $(this).data('day-index');

            // Bỏ chọn tất cả các checkbox khác có cùng day-index
            $(`.shift-checkbox[data-day-index="${dayIndex}"]`).not(this).prop('checked', false);
        }
    });

    // Nút Hủy: Quay về trang trước
    $('#btnCancel').click(function () {
        Swal.fire({
            title: 'Hủy bỏ?',
            text: "Các thay đổi sẽ không được lưu.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Vâng, hủy!',
            cancelButtonText: 'Ở lại'
        }).then((result) => {
            if (result.isConfirmed) {
                // Logic quay về trang danh sách (Window.history.back() hoặc href)
                window.history.back();
            }
        });
    });

    // Nút Tự động: Giả lập lấy lịch tuần trước
    $('#btnAutoAssign').click(function () {
        Swal.fire({
            title: 'Áp dụng lịch tự động?',
            text: "Hệ thống sẽ sao chép lịch làm việc của tuần trước cho tuần này (chỉ áp dụng cho các ngày chưa qua).",
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (result.isConfirmed) {
                // GIẢ LẬP LOGIC:
                // Trong thực tế, bạn gọi AJAX lấy data tuần trước.
                // Ở đây, mình sẽ random check một số ô KHÔNG BỊ DISABLE để demo

                $('.shift-checkbox:not(:disabled)').prop('checked', false); // Reset trước

                // Duyệt qua 7 ngày
                for (let i = 0; i < 7; i++) {
                    // Random chọn 1 ca (0, 1, 2) hoặc nghỉ (3)
                    let randomShift = Math.floor(Math.random() * 4);

                    if (randomShift < 3) {
                        // Tìm ô input tương ứng cột i và dòng randomShift mà không bị disable
                        let target = $(`.shift-checkbox[data-day-index="${i}"]`).eq(randomShift);
                        if (!target.is(':disabled')) {
                            target.prop('checked', true);
                        }
                    }
                }

                Swal.fire('Thành công', 'Đã áp dụng lịch từ tuần trước!', 'success');
            }
        });
    });

    // Nút Lưu phân công
    $('#btnSave').click(function () {
        // Gom dữ liệu
        let scheduleData = [];
        let employeeId = $('#employeeSelect').val();

        $('.shift-checkbox:checked').each(function () {
            scheduleData.push({
                ngay: $(this).data('date'),
                caId: $(this).data('shift-id')
            });
        });

        console.log("Dữ liệu gửi đi:", scheduleData);

        if (scheduleData.length === 0) {
            Swal.fire('Cảnh báo', 'Bạn chưa chọn ca làm việc nào.', 'info');
            return;
        }

        // Gọi API lưu (Giả lập)
        Swal.fire({
            title: 'Đang lưu...',
            didOpen: () => { Swal.showLoading() },
            timer: 1000
        }).then(() => {
            Swal.fire('Thành công', 'Đã lưu phân công ca làm việc!', 'success')
                .then(() => {
                    // Redirect hoặc reload
                    // location.reload();
                });
        });
    });
});