const contents = [
    { title: "Ui da! Đụng trúng tường lửa!", text: "Khu vực 'Tư Khờ' này được bảo vệ bởi phép thuật Winx Enchantix. Bạn cần đăng nhập để phá giải phong ấn!", icon: "fa-wand-magic-sparkles" },
    { title: "Camera chạy bằng cơm...", text: "Bác bảo vệ (là tôi) đang nhìn bạn đấy. Chưa đăng nhập mà định vào là không được đâu nha!", icon: "fa-user-secret" },
    { title: "404 - Không tìm thấy danh tính", text: "Hệ thống lục tung dữ liệu mà không biết bạn là ai. Hãy đăng nhập để chúng mình làm quen nhé!", icon: "fa-robot" },
    { title: "Tư Khờ Warning!", text: "Bạn đang cố gắng truy cập trái phép. Nếu là khách quen, xin hãy đăng nhập. Nếu là hacker, xin hãy tha cho em!", icon: "fa-triangle-exclamation" }
];

function randomContent() {
    const item = contents[Math.floor(Math.random() * contents.length)];
    document.getElementById('headline').innerText = item.title;
    document.getElementById('funny-message').innerText = item.text;
    document.querySelector('.main-icon i').className = `fa-solid ${item.icon}`;
}

window.onload = randomContent;