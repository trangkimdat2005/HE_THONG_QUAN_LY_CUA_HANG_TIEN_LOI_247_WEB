$(document).ready(function () {
    let currentStartDate = new Date();
    let shiftMetadata = []; // Lưu danh sách ca lấy từ DB

    // Lấy Thứ 2 đầu tuần
    function getMonday(d) {
        d = new Date(d);
        var day = d.getDay(),
            diff = d.getDate() - day + (day == 0 ? -6 : 1);
        return new Date(d.setDate(diff));
    }

    function formatDateDisplay(date) {
        let d = date.getDate();
        let m = date.getMonth() + 1;
        let y = date.getFullYear();
        return (d < 10 ? '0' + d : d) + '/' + (m < 10 ? '0' + m : m) + '/' + y;
    }

    function formatDateForAPI(date) {
        let d = date.getDate();
        let m = date.getMonth() + 1;
        let y = date.getFullYear();
        return y + '-' + (m < 10 ? '0' + m : m) + '-' + (d < 10 ? '0' + d : d);
    }

    // 1. Load danh sách Ca từ Server trước
    function init() {
        $.ajax({
            url: '/API/QuanLyNhanSu/GetShiftMetadata',
            type: 'GET',
            success: function (data) {
                shiftMetadata = data;
                renderTableStructure(); // Vẽ khung bảng
                loadScheduleData(); // Load dữ liệu tick vào
            }
        });
    }

    // 2. Vẽ khung bảng (Header & Checkboxes rỗng)
    function renderTableStructure() {
        let monday = getMonday(currentStartDate);
        let sunday = new Date(monday);
        sunday.setDate(monday.getDate() + 6);
        let today = new Date();
        today.setHours(0, 0, 0, 0); // Reset giờ để so sánh ngày

        // Update Toolbar Date
        $('#currentWeekDisplay').text(`${formatDateDisplay(monday)} - ${formatDateDisplay(sunday)}`);

        // Render Header
        let headerHtml = '<th>Ca làm việc</th>';
        let weekDates = [];
        for (let i = 0; i < 7; i++) {
            let tempDate = new Date(monday);
            tempDate.setDate(monday.getDate() + i);
            weekDates.push(tempDate);

            let dayName = i === 6 ? 'Chủ Nhật' : `Thứ ${i + 2}`;
            headerHtml += `<th>${dayName}<br><small>${formatDateDisplay(tempDate)}</small></th>`;
        }
        $('#tableHeaderRow').html(headerHtml);

        // Render Body
        let bodyHtml = '';
        shiftMetadata.forEach(shift => {
            bodyHtml += `<tr>`;
            bodyHtml += `<td>${shift.tenCa}<br><small>${shift.thoiGian}</small></td>`;

            // Render 7 ô checkbox
            for (let i = 0; i < 7; i++) {
                let cellDate = weekDates[i];
                let dateStr = formatDateForAPI(cellDate);

                // Logic chặn quá khứ: Nếu ngày nhỏ hơn hôm nay -> Disable
                let isPast = cellDate < today;
                let disabledAttr = isPast ? 'disabled' : '';
                let disabledClass = isPast ? 'cell-disabled' : '';

                // data-day-col: để gom nhóm theo cột (1 ngày chỉ 1 ca)
                bodyHtml += `
                    <td class="${disabledClass}">
                        <input type="checkbox" 
                               class="custom-checkbox shift-checkbox" 
                               data-day-col="${i}" 
                               data-date="${dateStr}"
                               data-shift-id="${shift.id}"
                               ${disabledAttr}>
                    </td>`;
            }
            bodyHtml += `</tr>`;
        });
        $('#tableBody').html(bodyHtml);
    }

    // 3. Load dữ liệu đã phân công của nhân viên và tick vào checkbox
    function loadScheduleData() {
        let monday = getMonday(currentStartDate);
        let nvId = $('#employeeSelect').val();

        Swal.fire({
            title: 'Đang tải...',
            didOpen: () => Swal.showLoading(),
            allowOutsideClick: false,
            timer: 500 // UX fake loading tí cho mượt
        });

        $.ajax({
            url: '/API/QuanLyNhanSu/GetPhanCongChiTiet',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                ngayBatDau: formatDateForAPI(monday),
                nhanVienId: nvId
            }),
            success: function (response) {
                // Reset toàn bộ checkbox trước khi tick cái mới
                $('.shift-checkbox').prop('checked', false);

                // Duyệt data trả về và tick vào ô tương ứng
                response.data.forEach(item => {
                    // Tìm checkbox có data-date và data-shift-id khớp
                    let selector = `.shift-checkbox[data-date="${item.ngay}"][data-shift-id="${item.caLamViecId}"]`;
                    $(selector).prop('checked', true);
                });
            }
        });
    }


    init(); // Chạy lần đầu

    $('#employeeSelect').change(function () {
        loadScheduleData(); // Đổi nhân viên thì load lại dữ liệu tick
    });

    $('#btnPrevWeek').click(function () {
        currentStartDate.setDate(currentStartDate.getDate() - 7);
        renderTableStructure();
        loadScheduleData();
    });

    $('#btnNextWeek').click(function () {
        currentStartDate.setDate(currentStartDate.getDate() + 7);
        renderTableStructure();
        loadScheduleData();
    });

    // LOGIC 1 CA / 1 NGÀY (Radio behavior for Checkbox)
    $(document).on('change', '.shift-checkbox', function () {
        if ($(this).is(':checked')) {
            let colIndex = $(this).data('day-col');
            // Bỏ chọn tất cả checkbox khác trong cùng cột (cùng ngày)
            $(`.shift-checkbox[data-day-col="${colIndex}"]`).not(this).prop('checked', false);
        }
    });

    // NÚT TỰ ĐỘNG (Lấy lịch tuần trước apply vào tuần này)
    $('#btnAutoAssign').click(function () {
        let monday = getMonday(currentStartDate);
        // Lấy ngày bắt đầu của tuần trước
        let prevWeekMonday = new Date(monday);
        prevWeekMonday.setDate(monday.getDate() - 7);

        Swal.fire({
            title: 'Sao chép lịch tuần trước?',
            text: 'Hệ thống sẽ lấy lịch tuần trước gán vào các ngày chưa qua của tuần này.',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (result.isConfirmed) {
                // Gọi API lấy dữ liệu TUẦN TRƯỚC
                $.ajax({
                    url: '/API/QuanLyNhanSu/GetPhanCongChiTiet',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        ngayBatDau: formatDateForAPI(prevWeekMonday), // Gửi ngày tuần trước
                        nhanVienId: $('#employeeSelect').val()
                    }),
                    success: function (response) {
                        // Map dữ liệu tuần trước sang tuần này
                        // Logic: Nếu tuần trước Thứ 2 làm Ca A -> Tuần này Thứ 2 tick Ca A (nếu chưa bị disable)

                        // Tạo map ngày tuần trước -> CaId
                        // Vì data trả về là list object, ta cần xử lý khéo 1 chút
                        // response.data: [{ngay: '2025-11-17', caLamViecId: '...'}, ...]

                        // Duyệt qua 7 ngày của tuần HIỆN TẠI
                        for (let i = 0; i < 7; i++) {
                            // Tính ngày tuần trước tương ứng với cột i
                            let pDate = new Date(prevWeekMonday);
                            pDate.setDate(prevWeekMonday.getDate() + i);
                            let pDateStr = formatDateForAPI(pDate);

                            // Tìm xem tuần trước ngày đó có làm gì ko
                            let shiftFound = response.data.find(x => x.ngay === pDateStr);

                            if (shiftFound) {
                                // Nếu có, tìm ô checkbox tương ứng ở tuần HIỆN TẠI (cột i)
                                let targetCheckbox = $(`.shift-checkbox[data-day-col="${i}"][data-shift-id="${shiftFound.caLamViecId}"]`);

                                // Nếu ô đó không bị disable (không phải quá khứ) thì check vào
                                if (!targetCheckbox.is(':disabled')) {
                                    // Bỏ check cũ của ngày này
                                    $(`.shift-checkbox[data-day-col="${i}"]`).prop('checked', false);
                                    // Check mới
                                    targetCheckbox.prop('checked', true);
                                }
                            }
                        }
                        Swal.fire('Thành công', 'Đã áp dụng lịch tuần trước!', 'success');
                    }
                });
            }
        });
    });

    // NÚT LƯU
    $('#btnSave').click(function () {
        let assignments = [];
        let nvId = $('#employeeSelect').val();
        let monday = getMonday(currentStartDate);

        // Duyệt tất cả các ô được tick
        $('.shift-checkbox:checked').each(function () {
            assignments.push({
                Ngay: $(this).data('date'),
                CaLamViecId: $(this).data('shift-id')
            });
        });

        // Gửi về server
        $.ajax({
            url: '/API/QuanLyNhanSu/SavePhanCong',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                NhanVienId: nvId,
                NgayBatDau: formatDateForAPI(monday), // Để server biết đang sửa tuần nào
                Assignments: assignments
            }),
            success: function (res) {
                Swal.fire('Thành công', res.message, 'success');
            },
            error: function (err) {
                Swal.fire('Lỗi', 'Có lỗi khi lưu: ' + err.responseText, 'error');
            }
        });
    });
});