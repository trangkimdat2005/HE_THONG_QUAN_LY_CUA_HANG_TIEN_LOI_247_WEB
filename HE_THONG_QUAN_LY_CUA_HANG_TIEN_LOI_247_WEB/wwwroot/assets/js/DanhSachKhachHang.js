document.addEventListener('DOMContentLoaded', function () {

    // --- TRUY XUẤT PHẦN TỬ DOM ---
    const tableBody = document.querySelector('#sampleTable tbody');

    // --- GÁN SỰ KIỆN CHO BẢNG (XOÁ) ---
    if (tableBody) {
        tableBody.addEventListener('click', function (e) {
            const deleteBtn = e.target.closest('.btn-delete-khoi');
            if (!deleteBtn) return;

            e.preventDefault();
            
            // Lấy thông tin từ row
            const row = deleteBtn.closest('tr');
            const khachHangId = row.cells[0].textContent.trim();
            const tenKhachHang = row.cells[1].textContent.trim();

            // Hỏi xác nhận
            if (confirm(`Bạn có chắc muốn xóa khách hàng "${tenKhachHang}" (${khachHangId}) không?`)) {
                deleteKhachHang(khachHangId, row);
            }
        });
    }

    // --- HÀM XÓA KHÁCH HÀNG ---
    async function deleteKhachHang(id, row) {
        try {
            console.log(`Deleting khách hàng: ${id}`);

            const response = await fetch('/API/delete-KhachHang', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Id: id })
            });

            const result = await response.json();
            console.log('Delete response:', result);

            if (response.ok) {
                console.log('✅ SUCCESS');
                alert(result.message || 'Xóa khách hàng thành công!');
                
                // Xóa row khỏi table
                row.remove();
            } else {
                console.error('❌ ERROR:', result.message);
                alert('Lỗi: ' + (result.message || 'Không thể xóa khách hàng'));
            }
        } catch (error) {
            console.error('❌ EXCEPTION:', error);
            alert('Có lỗi xảy ra khi xóa khách hàng:\n\n' + error.message);
        }
    }
});
