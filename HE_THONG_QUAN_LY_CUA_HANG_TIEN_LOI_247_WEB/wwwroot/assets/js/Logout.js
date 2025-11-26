$(function () {
    $('#logout-user').on('click', function (e) {
        e.preventDefault();

        $.ajax({
            url: '/Account/Logout',
            type: 'POST',
            data: {
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                if (response.status === 'SUCCESS') {
                    window.location.href = response.redirectUrl;
                }
            },
            error: function (xhr) {
                console.log(xhr.responseText); // Xem chi tiết lỗi
                alert("Có lỗi xảy ra khi đăng xuất.");
            }
        });
    });




    function() {

    }

});