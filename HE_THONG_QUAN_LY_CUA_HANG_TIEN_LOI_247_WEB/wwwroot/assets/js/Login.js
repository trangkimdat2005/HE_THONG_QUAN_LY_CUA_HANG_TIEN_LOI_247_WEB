function token() {
    return $('input[name=__RequestVerificationToken]').val();  // Lấy anti-forgery token
}

function loginAccount(userInput) {
    userInput.__RequestVerificationToken = token();  // Thêm token vào dữ liệu gửi đi

    // Hiện popup loading khi bắt đầu đăng nhập
    Swal.fire({
        title: 'Đang đăng nhập...',
        text: 'Vui lòng chờ trong giây lát',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    $.ajax({
        type: "POST",
        url: "/Account/LoginToSystem",  // URL của trang đăng nhập
        data: userInput,
        dataType: 'json',
        success: function (res) {
            // Nếu login thành công
            if (res.status === 'SUCCESS') {
                Swal.fire({
                    icon: 'success',
                    title: 'Đăng nhập thành công!',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    // Chuyển trang sau khi popup biến mất
                    location.href = res.redirectUrl;  // Địa chỉ trang sẽ redirect đến (trong trường hợp này là /Admin/HomeAdmin/Index)
                });
            } else {
                // Nếu login thất bại
                location.href = res.redirectUrl;
            }
        },
        error: function (res) {
            location.href = res.redirectUrl;
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    // Xử lý form login
    $(document).off('submit', '#login_form');
    $(document).on('submit', '#login_form', function (e) {
        e.preventDefault();
        loginAccount({
            username: $('#username').val(),
            password: $('#password').val()
        });
    });
});
