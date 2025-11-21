
$(function () {
    $('#logout-user').on('click', function (e) {
        e.preventDefault();  // Ngừng hành động mặc định của thẻ a

        // Gửi yêu cầu POST để đăng xuất
        $.ajax({
            url: "/Account/Logout", // Địa chỉ của action Logout trong controller Account
            type: 'POST',
            success: function (response) {
                // Chuyển hướng đến trang đăng nhập hoặc trang chủ sau khi đăng xuất thành công
                if (response.status === 'SUCCESS') {
                    window.location.href = response.redirectUrl; // Chuyển hướng
                }
            },
            error: function () {
                alert("Có lỗi xảy ra khi đăng xuất.");
            }
        });
    });
});