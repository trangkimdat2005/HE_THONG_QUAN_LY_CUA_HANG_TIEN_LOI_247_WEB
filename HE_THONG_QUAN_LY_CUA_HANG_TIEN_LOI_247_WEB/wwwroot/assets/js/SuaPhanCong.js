    document.addEventListener('DOMContentLoaded', function () {
      
      // --- KHỞI TẠO SELECT2 ---
      // (Đảm bảo jQuery đã được tải trước)
      $('#select-nhan-vien').select2({
        width: '100%',
        placeholder: '-- Tìm và chọn nhân viên --'
      });
      
      $('#select-ca-lam-viec').select2({
        width: '100%',
        placeholder: '-- Chọn ca làm việc --'
      });

      // --- TỰ ĐỘNG ĐIỀN NGÀY HÔM NAY ---
      const today = new Date().toISOString().split('T')[0];
      document.getElementById('input-ngay').value = today;

    });







//async document.addEventListener('click', function (e) {
//    const btn = e.target.closest('.btn-edit-khoi');
//    if (!btn) return;

//    e.preventDefault();

//    const idnv = btn.getAttribute('dataid');
//    const idcalam = btn.getAttribute('dataidcalam');
//    try {
//        const response = await fetch('/API/get-PC-by-id',
//        {
//            method: 'POST',
//            headers: { 'Content-Type': 'application/json' },
//            body: JSON.stringify({
//                NhanVienId: idnv, CaLamViecId: idcalam
//            })
//        });
//    if (!response) {
//        alert("Lỗi khi gọi!")
//    }
//    const data = await response.json();
//    if (data) {
//        document.getElementById('add-id-input').value = data.nextId;
//        let x = data.nhanViens;
//        let html = '';
//        x.forEach(pc => {
//            html += `<option value="${pc.NhanVienId}">${pc.hoTen}</option>`;
//        });
//    }
//    else {
//        alert('Không thể lấy mã danh mục, vui lòng thử lại.');
//    }
//} catch (error) {
//    console.error('Lỗi khi lấy mã danh mục:', error);
//    alert('Không thể lấy mã danh mục, vui lòng thử lại.');
//}
//});



document.addEventListener("DOMContentLoaded", function () {
    var nhanVienId = '@Model.NhanVienId'; // Lấy giá trị NhanVienId từ Model
    var selectElement = document.getElementById('select-nhan-vien');

    // Tìm option có giá trị trùng với NhanVienId và chọn nó
    for (var option of selectElement.options) {
        if (option.value === nhanVienId) {
            option.selected = true;
            break;
        }
    }
});