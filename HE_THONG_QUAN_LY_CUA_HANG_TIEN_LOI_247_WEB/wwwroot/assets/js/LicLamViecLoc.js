$(document).ready(function () {
    let currentStartDate = new Date();

    // Hàm tính ngày Thứ 2 đầu tuần
    function getMonday(d) {
        d = new Date(d);
        var day = d.getDay(),
            diff = d.getDate() - day + (day == 0 ? -6 : 1);
        return new Date(d.setDate(diff));
    }

    // Format dd/MM/yyyy để hiển thị
    function formatDateDisplay(date) {
        let d = date.getDate();
        let m = date.getMonth() + 1;
        let y = date.getFullYear();
        return (d < 10 ? '0' + d : d) + '/' + (m < 10 ? '0' + m : m) + '/' + y;
    }

    // Format yyyy-MM-dd để gửi API
    function formatDateForAPI(date) {
        // Lưu ý: Cẩn thận múi giờ, cách an toàn nhất là lấy string thủ công
        let d = date.getDate();
        let m = date.getMonth() + 1;
        let y = date.getFullYear();
        return y + '-' + (m < 10 ? '0' + m : m) + '-' + (d < 10 ? '0' + d : d);
    }

    function loadSchedule() {
        let monday = getMonday(currentStartDate);

        // Hiển thị loading
        Swal.fire({
            title: 'Đang tải dữ liệu...',
            didOpen: () => { Swal.showLoading() },
            allowOutsideClick: false
        });

        // Gọi API
        $.ajax({
            url: '/API/QuanLyNhanSu/GetLichLamViec',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                ngayBatDau: formatDateForAPI(monday),
                nhanVienId: $('#employeeSelect').val()
            }),
            success: function (response) {
                Swal.close();

                // 1. Cập nhật hiển thị ngày trên Toolbar
                $('#dateRangeDisplay').text(`${response.startDate} - ${response.endDate}`);

                // 2. Render Header (Thứ 2 -> CN)
                renderHeader(monday);

                // 3. Render Body (Dựa trên ShiftDefs và ScheduleData server trả về)
                renderBody(response.shiftDefs, response.scheduleData, monday);
            },
            error: function (err) {
                Swal.close();
                console.error(err);
                Swal.fire('Lỗi', 'Không thể tải lịch làm việc', 'error');
            }
        });
    }

    function renderHeader(monday) {
        let headerHtml = '<th>Ca làm việc</th>';
        for (let i = 0; i < 7; i++) {
            let tempDate = new Date(monday);
            tempDate.setDate(monday.getDate() + i);

            let dayName = i === 6 ? 'Chủ Nhật' : `Thứ ${i + 2}`;
            headerHtml += `<th>${dayName}<br><small>(${formatDateDisplay(tempDate)})</small></th>`;
        }
        $('#headerRow').html(headerHtml);
    }

    function renderBody(shifts, data, monday) {
        let html = '';

        // Tạo danh sách chuỗi ngày trong tuần (yyyy-MM-dd) để so sánh
        let weekDatesStr = [];
        for (let i = 0; i < 7; i++) {
            let tempDate = new Date(monday);
            tempDate.setDate(monday.getDate() + i);
            weekDatesStr.push(formatDateForAPI(tempDate));
        }

        // Duyệt qua từng Ca làm việc (Lấy từ DB) để tạo dòng
        if (shifts && shifts.length > 0) {
            shifts.forEach(shift => {
                html += `<tr>`;
                // Cột đầu tiên: Tên Ca
                html += `<td><b>${shift.tenCa}</b><br><small>${shift.thoiGian}</small></td>`;

                // 7 cột tiếp theo: Các ngày trong tuần
                weekDatesStr.forEach(dateStr => {
                    // Lọc phân công khớp Ngày và khớp CaId
                    let assignments = data.filter(item => {
                        return item.ngay === dateStr && item.caId === shift.id;
                    });

                    if (assignments.length > 0) {
                        // Gom tên nhân viên lại
                        let names = assignments.map(a => a.tenNhanVien).join(', <br>');
                        html += `<td class="cell-staff">${names}</td>`;
                    } else {
                        html += `<td class="cell-empty">Trống</td>`;
                    }
                });

                html += `</tr>`;
            });
        } else {
            html = '<tr><td colspan="8" class="text-center">Chưa có định nghĩa ca làm việc nào.</td></tr>';
        }

        $('#bodyRows').html(html);
    }

    // --- Events ---

    loadSchedule(); // Load lần đầu

    $('#btnPrevWeek').click(function () {
        currentStartDate.setDate(currentStartDate.getDate() - 7);
        loadSchedule();
    });

    $('#btnNextWeek').click(function () {
        currentStartDate.setDate(currentStartDate.getDate() + 7);
        loadSchedule();
    });

    $('#btnFilter').click(function () {
        loadSchedule();
    });

    // Reset về tuần hiện tại khi đổi nhân viên (tuỳ chọn)
     $('#employeeSelect').change(function() {
         loadSchedule();
     });
});