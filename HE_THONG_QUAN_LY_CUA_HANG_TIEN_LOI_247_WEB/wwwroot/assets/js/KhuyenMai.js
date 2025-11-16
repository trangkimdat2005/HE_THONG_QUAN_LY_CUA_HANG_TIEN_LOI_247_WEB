document.addEventListener('DOMContentLoaded', function () {
    const tableBody = document.getElementById('tbody-khuyen-mai');

    // --- X? LÝ NÚT XOÁ ---
    if (tableBody) {
        tableBody.addEventListener('click', function (e) {
            const btn = e.target.closest('.btn-delete-khoi');
            if (!btn) return;

            e.preventDefault();

            const id = btn.getAttribute('data-id');
            const name = btn.getAttribute('data-name');

            if (confirm(`B?n có mu?n xóa "${name}" không`)) {
                callApiDeleteCTKM(id);
            }
        });
    }

    // --- API XÓA CH??NG TRÌNH KHUY?N MÃI ---
    async function callApiDeleteCTKM(id) {
        try {
            console.log('Deleting CTKM with ID:', id);

            const response = await fetch(`/API/delete-CTKM/${encodeURIComponent(id)}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            let data = null;
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                data = await response.json();
            }

            if (!response.ok) {
                const errorMessage = (data && data.message) || 'Xóa ch??ng trình khuy?n mãi th?t b?i!';
                throw new Error(errorMessage);
            }

            console.log('Deleted successfully:', data);
            alert(data.message || 'Xóa ch??ng trình khuy?n mãi thành công!');

            // SignalR s? t? ??ng reload b?ng
        } catch (error) {
            console.error('Error deleting CTKM:', error);
            alert('L?i: ' + error.message);
        }
    }
});

// --- REALTIME UPDATE V?I SIGNALR ---
$(async function () {
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
                           title="S?a">
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
});
