$(document).ready(function() {
    console.log('ChinhSachHoanTra.js loaded successfully');

    // ==================== XỬ LÝ ÁP DỤNG TOÀN BỘ ====================
    
    // Toggle danh mục checkboxes khi thay đổi "Áp dụng toàn bộ"
    $('#select-ap-dung-toan-bo').change(function() {
        const apDungToanBo = $(this).val() === 'true';
        
        if (apDungToanBo) {
            // Nếu áp dụng toàn bộ, disable và uncheck tất cả checkbox
            $('.danh-muc-check').prop('checked', false).prop('disabled', true);
            $('#danh-muc-checkboxes').addClass('opacity-50');
        } else {
            // Nếu không áp dụng toàn bộ, enable checkbox
            $('.danh-muc-check').prop('disabled', false);
            $('#danh-muc-checkboxes').removeClass('opacity-50');
        }
    });

    // Trigger change event khi load trang
    $('#select-ap-dung-toan-bo').trigger('change');

    // ==================== VALIDATION FORM ====================
    
    $('#form-chinh-sach').submit(function(e) {
        const apDungToanBo = $('#select-ap-dung-toan-bo').val() === 'true';
        
        // Xóa tất cả hidden fields cũ trước
        $('#selected-danh-muc-container').empty();
        
        // Nếu không áp dụng toàn bộ, phải chọn ít nhất 1 danh mục
        if (!apDungToanBo) {
            const selectedCheckboxes = $('.danh-muc-check:checked');
            const selectedDanhMuc = selectedCheckboxes.length;
            
            if (selectedDanhMuc === 0) {
                e.preventDefault();
                alert('Vui lòng chọn ít nhất một danh mục áp dụng hoặc chọn "Áp dụng toàn bộ"!');
                return false;
            }
            
            // Tạo hidden fields cho từng danh mục đã chọn
            selectedCheckboxes.each(function(index) {
                const danhMucId = $(this).val();
                $('#selected-danh-muc-container').append(
                    `<input type="hidden" name="DanhMucIds[${index}]" value="${danhMucId}">`
                );
            });
            
            console.log('Selected danh muc:', selectedDanhMuc);
        }

        // Validate ngày
        const tuNgay = new Date($('input[name="ApDungTuNgay"]').val());
        const denNgay = new Date($('input[name="ApDungDenNgay"]').val());

        if (tuNgay >= denNgay) {
            e.preventDefault();
            alert('Ngày áp dụng đến phải sau ngày áp dụng từ!');
            return false;
        }

        console.log('Form validation passed');
        console.log('Form data:', {
            tenChinhSach: $('input[name="TenChinhSach"]').val(),
            thoiHan: $('input[name="ThoiHan"]').val(),
            apDungToanBo: apDungToanBo,
            dieuKien: $('textarea[name="DieuKien"]').val(),
            apDungTuNgay: tuNgay,
            apDungDenNgay: denNgay
        });
        
        return true;
    });

    // ==================== HIGHLIGHT SELECTED CHECKBOXES ====================
    
    $('.danh-muc-check').change(function() {
        if ($(this).is(':checked')) {
            $(this).closest('.form-check').addClass('border border-primary rounded p-2 bg-light');
        } else {
            $(this).closest('.form-check').removeClass('border border-primary rounded p-2 bg-light');
        }
        updateSelectedCount();
    });

    // Trigger change event cho các checkbox đã được check sẵn
    $('.danh-muc-check:checked').trigger('change');

    // ==================== UTILITIES ====================
    
    // Hiển thị số danh mục đã chọn
    function updateSelectedCount() {
        const count = $('.danh-muc-check:checked').length;
        const totalCount = $('.danh-muc-check').length;
        
        console.log(`Selected ${count}/${totalCount} categories`);
        
        // Hiển thị badge số lượng đã chọn (optional)
        const badge = $('#selected-count-badge');
        if (count > 0 && !$('#select-ap-dung-toan-bo').val() === 'true') {
            if (badge.length === 0) {
                $('h5:contains("Danh Mục Áp Dụng")').append(
                    `<span class="badge bg-success ms-2" id="selected-count-badge">${count} đã chọn</span>`
                );
            } else {
                badge.text(`${count} đã chọn`);
            }
        } else {
            badge.remove();
        }
    }

    // Initial count
    updateSelectedCount();
});
