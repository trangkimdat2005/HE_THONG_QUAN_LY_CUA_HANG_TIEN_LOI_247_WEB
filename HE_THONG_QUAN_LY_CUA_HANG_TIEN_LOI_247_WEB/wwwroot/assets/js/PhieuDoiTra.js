$(document).ready(function() {
    // ==================== PHẦN 1: XEM DANH SÁCH PHIẾU ĐỔI TRẢ ====================
    
    // 1.1. MOCK DATA cho danh sách (có thể xóa khi đã có backend)
    const MOCK_DB = {
        'PDT001': {
            id: 'PDT001', 
            hoaDonId: 'HD003',
            ngayDoiTra: '2025-11-01',
            chinhSachId: 'CS_7NGAY (Đổi trả trong 7 ngày)',
            chiTiet: [
                { sanPhamDonViId: 'SP005 - Sữa tươi TH True Milk (Lốc 4)', lyDo: 'Khách hàng đổi ý', soTienHoan: 12000 }
            ]
        },
        'PDT002': {
            id: 'PDT002',
            hoaDonId: 'HD001',
            ngayDoiTra: '2025-11-03',
            chinhSachId: 'CS_LOI_NSX (Lỗi nhà sản xuất)',
            chiTiet: [
                { sanPhamDonViId: 'SP012 - Thịt gà công nghiệp (Kg)', lyDo: 'Thịt có mùi lạ', soTienHoan: 85000 },
                { sanPhamDonViId: 'SP008 - Trứng gà Ba Huân (Hộp 10)', lyDo: 'Vỡ khi vận chuyển', soTienHoan: 32000 }
            ]
        }
    };

    // 1.2. XỬ LÝ SỰ KIỆN CLICK NÚT "XEM" (cho trang danh sách)
    $('.js-view-detail').click(function(e) {
        e.preventDefault();
        let id = $(this).data('id');
        let data = MOCK_DB[id];

        if (data) {
            // Điền dữ liệu Header
            $('#detail-id').text('#' + data.id);
            $('#detail-hoaDonId').html(`<a href="#" class="text-decoration-none">${data.hoaDonId} <i class="bi bi-box-arrow-up-right small"></i></a>`);
            $('#detail-ngayDoiTra').text(new Date(data.ngayDoiTra).toLocaleDateString('vi-VN'));
            $('#detail-chinhSachId').text(data.chinhSachId);

            // Điền dữ liệu Bảng chi tiết & Tính tổng
            let rows = '';
            let tongTien = 0;
            
            data.chiTiet.forEach((item, index) => {
                tongTien += item.soTienHoan;
                rows += `
                    <tr>
                        <td class="text-center">${index + 1}</td>
                        <td class="fw-bold">${item.sanPhamDonViId}</td>
                        <td>${item.lyDo}</td>
                        <td class="text-end">${new Intl.NumberFormat('vi-VN').format(item.soTienHoan)} đ</td>
                    </tr>
                `;
            });
            
            $('#detail-tbody').html(rows);
            $('#detail-tong-tien').text(new Intl.NumberFormat('vi-VN').format(tongTien) + ' đ');

            // Chuyển đổi hiển thị sang màn hình chi tiết (ẩn list, hiện detail)
            $('#section-list').addClass('d-none');
            $('#section-detail').removeClass('d-none');
        }
    });

    // 1.3. XỬ LÝ SỰ KIỆN CLICK NÚT "QUAY LẠI"
    $('#btn-back-list').click(function() {
        // Chuyển ngược lại (ẩn detail, hiện list)
        $('#section-detail').addClass('d-none');
        $('#section-list').removeClass('d-none');
    });

    // ==================== PHẦN 2: TẠO PHIẾU ĐỔI TRẢ MỚI ====================
    
    let selectedProduct = null;

    console.log('PhieuDoiTra.js loaded successfully');

    // 2.1. XỬ LÝ KHI CHỌN HÓA ĐƠN
    $('#select-hoa-don').change(function() {
        const hoaDonId = $(this).val();
        const selectedOption = $(this).find('option:selected');
        
        console.log('Selected HoaDon ID:', hoaDonId);
        
        if (hoaDonId) {
            // Hiển thị thông tin khách hàng
            const khachHang = selectedOption.data('khach-hang');
            $('#input-khach-hang').val(khachHang || 'N/A');
            
            console.log('Loading products for HoaDon:', hoaDonId);
            
            // Load danh sách sản phẩm từ hóa đơn
            $.ajax({
                url: '/Them/GetSanPhamByHoaDon/' + hoaDonId,
                type: 'GET',
                success: function(data) {
                    console.log('Products loaded:', data);
                    
                    $('#select-san-pham-tra').prop('disabled', false);
                    $('#select-san-pham-tra').html('<option value="">-- Chọn sản phẩm --</option>');
                    
                    if (data && data.length > 0) {
                        data.forEach(function(sp) {
                            $('#select-san-pham-tra').append(
                                `<option value="${sp.id}" 
                                         data-ten="${sp.ten}" 
                                         data-don-vi="${sp.donVi}" 
                                         data-gia="${sp.giaBan}">
                                    ${sp.ten} - ${sp.donVi} - ${sp.giaBan.toLocaleString('vi-VN')}đ
                                </option>`
                            );
                        });
                    } else {
                        $('#select-san-pham-tra').append('<option value="">Hóa đơn không có sản phẩm</option>');
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error loading products:', error);
                    alert('Không thể tải danh sách sản phẩm! Lỗi: ' + error);
                }
            });
            
            $('#input-ly-do').prop('disabled', false);
            $('#btn-add-return-item').prop('disabled', false);
        } else {
            $('#input-khach-hang').val('');
            $('#select-san-pham-tra').html('<option value="">-- Vui lòng chọn hóa đơn trước --</option>');
            $('#select-san-pham-tra').prop('disabled', true);
            $('#input-ly-do').prop('disabled', true);
            $('#btn-add-return-item').prop('disabled', true);
        }
        
        // Reset bảng
        resetTable();
    });

    // 2.2. XỬ LÝ KHI NHẤN NÚT "THÊM" SẢN PHẨM
    $('#btn-add-return-item').click(function() {
        const sanPhamId = $('#select-san-pham-tra').val();
        const lyDo = $('#input-ly-do').val().trim();
        
        if (!sanPhamId) {
            alert('Vui lòng chọn sản phẩm!');
            return;
        }
        
        if (!lyDo) {
            alert('Vui lòng nhập lý do trả hàng!');
            return;
        }
        
        const selectedOption = $('#select-san-pham-tra option:selected');
        const tenSanPham = selectedOption.data('ten');
        const donVi = selectedOption.data('don-vi');
        const giaBan = parseFloat(selectedOption.data('gia'));
        
        // Lưu thông tin sản phẩm được chọn
        selectedProduct = {
            id: sanPhamId,
            ten: tenSanPham,
            donVi: donVi,
            gia: giaBan,
            lyDo: lyDo
        };
        
        console.log('Product selected:', selectedProduct);
        
        // Cập nhật bảng
        updateTable();
        
        // Cập nhật hidden fields
        $('#hidden-san-pham-don-vi-id').val(sanPhamId);
        $('#hidden-ly-do').val(lyDo);
        
        // Reset form
        $('#select-san-pham-tra').val('');
        $('#input-ly-do').val('');
    });

    // 2.3. CẬP NHẬT BẢNG SẢN PHẨM
    function updateTable() {
        if (!selectedProduct) return;
        
        const html = `
            <tr>
                <td>${selectedProduct.ten}</td>
                <td>${selectedProduct.donVi}</td>
                <td class="text-end">${selectedProduct.gia.toLocaleString('vi-VN')} đ</td>
                <td>${selectedProduct.lyDo}</td>
                <td class="text-end fw-bold text-danger">${selectedProduct.gia.toLocaleString('vi-VN')} đ</td>
            </tr>
        `;
        
        $('#tbody-tra-hang').html(html);
        $('#lbl-tong-tien-hoan').text(selectedProduct.gia.toLocaleString('vi-VN') + ' đ');
        $('#hidden-tong-tien-hoan').val(selectedProduct.gia);
    }

    // 2.4. RESET BẢNG SẢN PHẨM
    function resetTable() {
        selectedProduct = null;
        $('#tbody-tra-hang').html(`
            <tr>
                <td colspan="5" class="text-center text-muted fst-italic py-4">
                    <i class="bi bi-inbox fs-3 d-block mb-2"></i>
                    Chưa có sản phẩm nào được chọn
                </td>
            </tr>
        `);
        $('#lbl-tong-tien-hoan').text('0 đ');
        $('#hidden-tong-tien-hoan').val('0');
        $('#hidden-san-pham-don-vi-id').val('');
        $('#hidden-ly-do').val('');
    }

    // 2.5. VALIDATE FORM TRƯỚC KHI SUBMIT
    $('#form-phieu-doi-tra').submit(function(e) {
        if (!selectedProduct) {
            e.preventDefault();
            alert('Vui lòng chọn ít nhất một sản phẩm để đổi trả!');
            return false;
        }
        
        if (!$('#select-hoa-don').val()) {
            e.preventDefault();
            alert('Vui lòng chọn hóa đơn!');
            return false;
        }
        
        if (!$('#select-chinh-sach').val()) {
            e.preventDefault();
            alert('Vui lòng chọn chính sách hoàn trả!');
            return false;
        }
        
        console.log('Form submitted with data:', {
            hoaDonId: $('#select-hoa-don').val(),
            chinhSachId: $('#select-chinh-sach').val(),
            sanPhamDonViId: $('#hidden-san-pham-don-vi-id').val(),
            lyDo: $('#hidden-ly-do').val(),
            soTienHoan: $('#hidden-tong-tien-hoan').val()
        });
        
        return true;
    });

    // ==================== PHẦN 3: UTILITIES ====================
    
    // 3.1. Format số tiền Việt Nam
    function formatVND(amount) {
        return new Intl.NumberFormat('vi-VN').format(amount) + ' đ';
    }

    // 3.2. Format ngày tháng Việt Nam
    function formatDateVN(dateString) {
        return new Date(dateString).toLocaleDateString('vi-VN');
    }
});