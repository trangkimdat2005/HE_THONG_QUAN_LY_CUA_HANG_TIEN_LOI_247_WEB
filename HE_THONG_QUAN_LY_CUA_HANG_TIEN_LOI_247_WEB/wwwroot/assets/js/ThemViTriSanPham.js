 $(document).ready(function() {
    // ==================== KHỞI TẠO ====================
    
    // Khởi tạo Select2 với tùy chỉnh
    $('.select2').select2({ 
        width: '100%', 
        theme: 'default',
        language: {
            noResults: function() {
                return "Không tìm thấy kết quả";
            },
            searching: function() {
                return "Đang tìm kiếm...";
            }
        }
    });

    // Khởi tạo tooltips
    if (typeof bootstrap !== 'undefined') {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // Set ngày hiện tại
    $('#input-ngay-thuc-hien').val(new Date().toISOString().split('T')[0]);

    // ==================== BIẾN GLOBAL ====================
    
    const formatter = new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    });

    const chiTietList = [];
    let stt = 1;

    // ==================== HÀM TIỆN ÍCH ====================
    
    // Hàm xóa row placeholder
    function removePlaceholderRow() {
        const placeholder = $('#row-empty');
        if (placeholder.length > 0) {
            placeholder.fadeOut(200, function() {
                $(this).remove();
            });
        }
    }

    // Hàm cập nhật thống kê
    function updateStatistics() {
        const totalItems = chiTietList.length;
        const totalQuantity = chiTietList.reduce((sum, item) => sum + item.soLuong, 0);
        const uniqueLocations = new Set(chiTietList.map(item => item.viTriId)).size;
        
        // Tính tổng giá trị
        let totalValue = 0;
        $('#tbody-gan-vi-tri tr[data-sanpham-id]').each(function() {
            const priceText = $(this).find('.c-gia').text().replace(/[^\d]/g, '');
            const price = parseFloat(priceText) || 0;
            totalValue += price;
        });

        // Cập nhật UI với animation
        animateNumber('#total-items', totalItems);
        animateNumber('#total-quantity', totalQuantity);
        animateNumber('#total-locations', uniqueLocations);
        
        $('#total-value').text(formatter.format(totalValue));
    }

    // Hàm animate số
    function animateNumber(selector, endValue) {
        const element = $(selector);
        const startValue = parseInt(element.text()) || 0;
        const duration = 500;
        const stepTime = 20;
        const steps = duration / stepTime;
        const increment = (endValue - startValue) / steps;
        let current = startValue;
        
        const timer = setInterval(function() {
            current += increment;
            if ((increment > 0 && current >= endValue) || (increment < 0 && current <= endValue)) {
                current = endValue;
                clearInterval(timer);
            }
            element.text(Math.round(current));
        }, stepTime);
    }

    // Hàm validate form
    function validateForm() {
        const nhanVienId = $('#select-nhan-vien').val();
        const ngayThucHien = $('#input-ngay-thuc-hien').val();
        
        // Reset validation states
        $('.is-invalid').removeClass('is-invalid');
        
        let isValid = true;
        let errorMessage = '';

        if (!nhanVienId) {
            $('#select-nhan-vien').next('.select2-container').addClass('is-invalid');
            errorMessage = 'Vui lòng chọn nhân viên thực hiện!';
            isValid = false;
        }

        if (!ngayThucHien) {
            $('#input-ngay-thuc-hien').addClass('is-invalid');
            errorMessage = 'Vui lòng chọn ngày thực hiện!';
            isValid = false;
        }

        if (!isValid) {
            showNotification(errorMessage, 'error');
        }

        return isValid;
    }

    // Hàm hiển thị thông báo
    function showNotification(message, type = 'info') {
        const alertClass = type === 'error' ? 'alert-danger' : 
                          type === 'success' ? 'alert-success' : 
                          'alert-info';
        
        const notification = $(`
            <div class="alert ${alertClass} alert-dismissible fade show position-fixed" 
                 role="alert" style="top: 20px; right: 20px; z-index: 9999; min-width: 300px;">
                <i class="bi bi-${type === 'error' ? 'exclamation-triangle' : type === 'success' ? 'check-circle' : 'info-circle'} me-2"></i>
                <strong>${message}</strong>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `);
        
        $('body').append(notification);
        
        setTimeout(function() {
            notification.alert('close');
        }, 3000);
    }

    // ==================== SỰ KIỆN TỰ ĐỘNG ĐIỀN ====================
    
    // Tự động điền giá tiền khi chọn sản phẩm
    $('#select-san-pham').on('select2:select', function(e) {
        const price = $(this).find(':selected').data('price');
        const soLuong = parseInt($('#input-so-luong').val()) || 1;
        
        if (price) {
            const totalPrice = price * soLuong;
            $('#input-gia-tien').val(formatter.format(totalPrice));
        } else {
            $('#input-gia-tien').val('');
        }

        // Highlight field
        $(this).next('.select2-container').addClass('border-success');
        setTimeout(() => {
            $(this).next('.select2-container').removeClass('border-success');
        }, 1000);
    });

    // Tự động điền loại vị trí khi chọn mã vị trí
    $('#select-ma-vi-tri').on('select2:select', function(e) {
        const loaiViTri = $(this).find(':selected').data('loai');
        if (loaiViTri) {
            $('#input-loai-vi-tri').val(loaiViTri);
        } else {
            $('#input-loai-vi-tri').val('');
        }

        // Highlight field
        $(this).next('.select2-container').addClass('border-success');
        setTimeout(() => {
            $(this).next('.select2-container').removeClass('border-success');
        }, 1000);
    });

    // Cập nhật giá khi thay đổi số lượng
    $('#input-so-luong').on('input', function() {
        const spOpt = $('#select-san-pham').find(':selected');
        const price = spOpt.data('price');
        const soLuong = parseInt($(this).val()) || 1;
        
        if (price && soLuong > 0) {
            const totalPrice = price * soLuong;
            $('#input-gia-tien').val(formatter.format(totalPrice));
        }
    });

    // ==================== NÚT THÊM DÒNG ====================
    
    $('#btn-them-dong').click(function() {
        const spOpt = $('#select-san-pham').find(':selected');
        const spId = spOpt.val();
        const spName = spOpt.data('name');
        const spDvt = spOpt.data('dvt');
        const spPrice = spOpt.data('price');
        
        const viTriOpt = $('#select-ma-vi-tri').find(':selected');
        const viTriId = viTriOpt.val();
        const viTriMa = viTriOpt.data('ma');
        const loaiViTri = $('#input-loai-vi-tri').val();
        
        const soLuong = parseInt($('#input-so-luong').val());
        const giaTien = spPrice ? formatter.format(spPrice * soLuong) : '0 ₫';

        // Validate
        if (!spId) { 
            showNotification('Vui lòng chọn sản phẩm!', 'error');
            $('#select-san-pham').select2('open');
            return; 
        }
        if (!viTriId || viTriId === '') { 
            showNotification('Vui lòng chọn Mã vị trí!', 'error');
            $('#select-ma-vi-tri').select2('open'); 
            return; 
        }
        if (isNaN(soLuong) || soLuong <= 0) { 
            showNotification('Số lượng phải lớn hơn 0!', 'error');
            $('#input-so-luong').focus(); 
            return; 
        }

        // Kiểm tra trùng lặp
        const isDuplicate = chiTietList.some(item => 
            item.sanPhamDonViId === spId && item.viTriId === viTriId
        );

        if (isDuplicate) {
            showNotification('Sản phẩm đã tồn tại ở vị trí này! Vui lòng chọn vị trí khác.', 'error');
            return;
        }

        // Xóa dòng trống
        removePlaceholderRow();

        // Badge màu sắc
        const badgeClass = loaiViTri === 'Kho' ? 'badge-kho' : 'badge-trungbay';

        // Lưu vào danh sách chi tiết
        chiTietList.push({
            sanPhamDonViId: spId,
            viTriId: viTriId,
            soLuong: soLuong
        });

        // Tạo dòng mới với animation
        const newRow = $(`
            <tr data-sanpham-id="${spId}" data-vitri-id="${viTriId}" class="animate-row">
                <td class="c-stt">${stt++}</td>
                <td class="c-ten">
                    <span class="fw-medium">${spName}</span>
                    <br><small class="text-muted"><i class="bi bi-tag"></i> ${spId}</small>
                </td>
                <td class="c-dvt">${spDvt}</td>
                <td class="c-ma">
                    <span class="badge bg-primary">${viTriMa}</span>
                </td>
                <td class="c-loai"><span class="${badgeClass}">${loaiViTri}</span></td>
                <td class="c-sl">
                    <span class="badge bg-info">${soLuong}</span>
                </td>
                <td class="c-gia text-end fw-bold text-success">${giaTien}</td>
                <td class="c-act text-center">
                    <button class="btn btn-sm btn-outline-danger btn-xoa" type="button"
                            data-bs-toggle="tooltip" title="Xóa">
                        <i class="bi bi-trash3-fill"></i>
                    </button>
                </td>
            </tr>
        `);
        
        $('#tbody-gan-vi-tri').append(newRow);

        // Khởi tạo tooltip cho nút mới
        if (typeof bootstrap !== 'undefined') {
            const tooltips = newRow.find('[data-bs-toggle="tooltip"]');
            tooltips.each(function() {
                new bootstrap.Tooltip(this);
            });
        }

        // Reset form
        $('#select-san-pham').val('').trigger('change');
        $('#select-ma-vi-tri').val('').trigger('change');
        $('#input-loai-vi-tri').val('');
        $('#input-so-luong').val(1);
        $('#input-gia-tien').val('');

        // Cập nhật thống kê
        updateStatistics();

        // Hiển thị thông báo
        showNotification('Đã thêm sản phẩm vào danh sách!', 'success');
    });

    // ==================== NÚT XÓA DÒNG ====================
    
    $(document).on('click', '.btn-xoa', function() {
        const row = $(this).closest('tr');
        const spId = row.data('sanpham-id');
        const viTriId = row.data('vitri-id');
        
        if(confirm('Bạn có chắc muốn xóa sản phẩm này khỏi danh sách?')) {
            // Xóa khỏi danh sách chi tiết
            const index = chiTietList.findIndex(item => 
                item.sanPhamDonViId === spId && item.viTriId === viTriId
            );
            if (index > -1) {
                chiTietList.splice(index, 1);
            }
            
            // Xóa row với animation
            row.fadeOut(300, function() {
                $(this).remove();
                
                // Nếu không còn dòng nào, thêm lại placeholder
                if($('#tbody-gan-vi-tri tr').length === 0) {
                    $('#tbody-gan-vi-tri').html(`
                        <tr id="row-empty" data-placeholder="true">
                            <td colspan="8" class="text-center text-muted py-5">
                                <i class="bi bi-inbox" style="font-size: 3rem; opacity: 0.3;"></i>
                                <p class="mt-2 mb-0">Chưa có sản phẩm nào</p>
                                <small>Vui lòng thêm sản phẩm vào danh sách</small>
                            </td>
                        </tr>
                    `);
                    stt = 1;
                }
                
                // Cập nhật lại STT
                let newStt = 1;
                $('#tbody-gan-vi-tri tr[data-sanpham-id]').each(function() {
                    $(this).find('.c-stt').text(newStt++);
                });
                
                // Cập nhật thống kê
                updateStatistics();
            });

            showNotification('Đã xóa sản phẩm khỏi danh sách!', 'success');
        }
    });

    // ==================== NÚT XÓA TẤT CẢ ====================
    
    $('#btn-clear-all').click(function() {
        if (chiTietList.length === 0) {
            showNotification('Danh sách đang trống!', 'info');
            return;
        }

        if(confirm('Bạn có chắc muốn xóa tất cả sản phẩm trong danh sách?')) {
            chiTietList.length = 0;
            stt = 1;
            
            $('#tbody-gan-vi-tri').fadeOut(300, function() {
                $(this).html(`
                    <tr id="row-empty" data-placeholder="true">
                        <td colspan="8" class="text-center text-muted py-5">
                            <i class="bi bi-inbox" style="font-size: 3rem; opacity: 0.3;"></i>
                            <p class="mt-2 mb-0">Chưa có sản phẩm nào</p>
                            <small>Vui lòng thêm sản phẩm vào danh sách</small>
                        </td>
                    </tr>
                `).fadeIn(300);
            });

            updateStatistics();
            showNotification('Đã xóa tất cả sản phẩm!', 'success');
        }
    });

    // ==================== NÚT XUẤT EXCEL ====================
    
    $('#btn-export-excel').click(function() {
        if (chiTietList.length === 0) {
            showNotification('Không có dữ liệu để xuất!', 'error');
            return;
        }

        // Tạo CSV content
        let csvContent = "STT,Tên Sản Phẩm,ĐVT,Mã Vị Trí,Loại Vị Trí,Số Lượng,Giá Trị\n";
        
        $('#tbody-gan-vi-tri tr[data-sanpham-id]').each(function(index) {
            const cells = $(this).find('td');
            const row = [
                index + 1,
                $(cells[1]).find('.fw-medium').text().trim(),
                $(cells[2]).text().trim(),
                $(cells[3]).text().trim(),
                $(cells[4]).text().trim(),
                $(cells[5]).text().trim(),
                $(cells[6]).text().trim()
            ];
            csvContent += row.join(',') + '\n';
        });

        // Download file
        const blob = new Blob(["\uFEFF" + csvContent], { type: 'text/csv;charset=utf-8;' });
        const link = document.createElement('a');
        const url = URL.createObjectURL(blob);
        link.setAttribute('href', url);
        link.setAttribute('download', `DanhSachGanViTri_${new Date().toISOString().slice(0,10)}.csv`);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);

        showNotification('Đã xuất file Excel thành công!', 'success');
    });

    // ==================== NÚT LƯU PHIẾU ====================
    
    $('#btn-luu-phieu').click(async function(e) {
        e.preventDefault();
        
        // Validate form
        if (!validateForm()) {
            return;
        }
        
        if(chiTietList.length === 0) {
            showNotification('Vui lòng thêm ít nhất một sản phẩm trước khi lưu!', 'error');
            return;
        }
        
        // Lấy dữ liệu
        const nhanVienId = $('#select-nhan-vien').val();
        const ngayThucHien = $('#input-ngay-thuc-hien').val();
        
        // Tạo object request
        const requestData = {
            nhanVienId: nhanVienId,
            ngayThucHien: new Date(ngayThucHien).toISOString(),
            chiTietGanViTri: chiTietList
        };
        
        console.log('Request data:', requestData);
        
        try {
            // Hiển thị loading
            const btn = $(this);
            btn.prop('disabled', true);
            const originalHTML = btn.html();
            btn.html('<i class="spinner-border spinner-border-sm me-2"></i>Đang lưu...');

            // Disable các nút khác
            $('#btn-them-dong, #btn-clear-all, #btn-export-excel').prop('disabled', true);
            
            // Gọi API
            const response = await fetch('/add-GanViTri', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestData)
            });
            
            console.log('Response status:', response.status);
            
            // Kiểm tra content type
            const contentType = response.headers.get("content-type");
            console.log('Content-Type:', contentType);
            
            let result;
            if (contentType && contentType.indexOf("application/json") !== -1) {
                result = await response.json();
            } else {
                const text = await response.text();
                console.log('Response text:', text);
                throw new Error('Server không trả về JSON. Response: ' + text.substring(0, 200));
            }
            
            console.log('Response:', result);
            
            if (response.ok) {
                showNotification(result.message || 'Gán vị trí sản phẩm thành công!', 'success');
                
                // Chờ 1 giây rồi chuyển trang
                setTimeout(() => {
                    window.location.href = '/QuanLyKhoHang/ViTriSanPham';
                }, 1000);
            } else {
                showNotification('Lỗi: ' + (result.message || 'Không thể gán vị trí sản phẩm'), 'error');
                btn.prop('disabled', false);
                btn.html(originalHTML);
                $('#btn-them-dong, #btn-clear-all, #btn-export-excel').prop('disabled', false);
            }
        } catch (error) {
            console.error('Error:', error);
            showNotification('Có lỗi xảy ra: ' + error.message, 'error');
            $('#btn-luu-phieu').prop('disabled', false);
            $('#btn-luu-phieu').html('<i class="bi bi-floppy2-fill me-2"></i>Lưu hoàn tất');
            $('#btn-them-dong, #btn-clear-all, #btn-export-excel').prop('disabled', false);
        }
    });

    // ==================== KHỞI TẠO BAN ĐẦU ====================
    
    updateStatistics();
});