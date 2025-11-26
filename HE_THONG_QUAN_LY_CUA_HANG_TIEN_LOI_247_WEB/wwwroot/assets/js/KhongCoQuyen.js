function goBack() {
    // Kiểm tra xem có lịch sử duyệt web không
    if (document.referrer) {
        window.history.back();
    } else {
        // Nếu không có lịch sử (mở tab mới), quay về trang chủ
        alert("Không tìm thấy đường về trang trước, mình sẽ đưa bạn về trang chủ nhé!");
        window.location.href = '/'; // Thay đổi đường dẫn này thành trang chủ của bạn
    }
}