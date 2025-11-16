document.addEventListener('DOMContentLoaded', function () {
    console.log('=== KHUYẾN MÃI PAGE LOADED ===');

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const tableBody = document.querySelector('#sampleTable tbody');

    // --- GÁN SỰ KIỆN CHO BẢNG (XOÁ) ---
    if (tableBody) {
        tableBody.addEventListener('click', function (e) {
            // Tìm nút delete được click
            const deleteBtn = e.target.closest('.btn-delete-khoi');
            if (!deleteBtn) return;

            e.preventDefault();
            
            // Lấy thông tin từ data attributes
            const chuongTrinhId = deleteBtn.getAttribute('data-id');
            const tenChuongTrinh = deleteBtn.getAttribute('data-name');
            const row = deleteBtn.closest('tr');

            console.log('Delete button clicked:', { chuongTrinhId, tenChuongTrinh });

            // Hỏi xác nhận
            if (confirm(`Bạn có chắc muốn xóa chương trình "${tenChuongTrinh}" (${chuongTrinhId}) không?`)) {
                deleteChuongTrinh(chuongTrinhId, row);
            }
        });
    }

    // --- HÀM XÓA CHƯƠNG TRÌNH KHUYẾN MÃI ---
    async function deleteChuongTrinh(id, row) {
        try {
            console.log(`Deleting chương trình: ${id}`);

            const response = await fetch(`/API/delete-CTKM/${encodeURIComponent(id)}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            let result = null;
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                result = await response.json();
            }

            console.log('Delete response:', result);

            if (response.ok) {
                console.log('✅ SUCCESS');
                alert(result?.message || 'Xóa chương trình khuyến mãi thành công!');
                
                // Xóa row khỏi table
                if (row) {
                    row.remove();
                }
            } else {
                console.error('❌ ERROR:', result?.message);
                alert('Lỗi: ' + (result?.message || 'Không thể xóa chương trình khuyến mãi'));
            }
        } catch (error) {
            console.error('❌ EXCEPTION:', error);
            alert('Có lỗi xảy ra khi xóa chương trình khuyến mãi:\n\n' + error.message);
        }
    }
});

// SignalR realtime update (nếu có)
$(async function () {
    if (typeof appRealtimeList !== 'undefined') {
        await appRealtimeList.initEntityTable({
            key: 'ChuongTrinhKhuyenMai',
            apiUrl: '/API/get-all-CTKM',
            tableId: 'sampleTable',
            tbodyId: 'tbody-khuyen-mai',
            buildRow: ct => {
                const ngayBatDau = new Date(ct.ngayBatDau).toLocaleDateString('vi-VN');
                const ngayKetThuc = new Date(ct.ngayKetThuc).toLocaleDateString('vi-VN');
                const moTa = ct.moTa || '';

                return `
                    <tr>
                        <td>${ct.id}</td>
                        <td>${ct.ten}</td>
                        <td>${ct.loai}</td>
                        <td>${ngayBatDau}</td>
                        <td>${ngayKetThuc}</td>
                        <td>${moTa}</td>
                        <td class="text-center">
                            <a class="btn btn-info btn-sm me-1 btn-edit-khoi"
                               href="/Sua/SuaMaKhuyenMai?id=${encodeURIComponent(ct.id)}"
                               title="Sửa">
                                <i class="fas fa-edit"></i>
                            </a>
                            <a class="btn btn-danger btn-sm btn-delete-khoi"
                               href="#"
                               data-id="${ct.id}"
                               data-name="${ct.ten}"
                               title="Xóa">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </td>
                    </tr>`;
            }
        });
    }
});
