// const dropArea = document.querySelector('.members_check--contain')
// const dragText = dropArea.querySelector('.members_contain--text')
// const btnAddImg = dropArea.querySelector('.members_check--btn')
// const inputImg = dropArea.querySelector('#member_file--image')

// btnAddImg.addEventListener('click', ()=>{
//     inputImg.click();
// })

const btnAddImg = document.querySelector('.members_check--btn');
const inputImg = document.querySelector('#member_file--image');

btnAddImg.addEventListener('click', ()=> {
    inputImg.click();
})

// Đóng mở Modal Ticket
var BuyBtns = document.querySelectorAll('.change_password');
var modal = document.querySelector('.modal');
var modalTickets = document.querySelector('.modal-changePassword');
var modalClose = document.querySelector('.modal-close');

function showModal () {
    modal.classList.add('open');
}

function removeModal () {
    modal.classList.remove('open');
}

function holdModal (event) {
    event.stopPropagation ();
}

for (var BuyBtn of BuyBtns) {
    BuyBtn.addEventListener('click', showModal);
}
modal.addEventListener('click', removeModal);
modalClose.addEventListener('click', removeModal);
modalTickets.addEventListener('click', holdModal);

//validate thông tin tài khoản
var update_btn = document.getElementById('update-btn');
update_btn.onclick = function (e) {
    // Lấy giá trị của một input
    function getValue(id) {
        return document.getElementById(id).value.trim();
    }

    // Hiển thị lỗi
    function showError(key, mess) {
        document.getElementById(key + '_error').innerText = mess;
    }

    //  Tên
    var name = getValue('memberName');
    if (name == '') {
        e.preventDefault();
        showError('memberName', 'Vui lòng nhập tên!');
    } else {
        showError('memberName', '');
    }


    //  SĐT
    var phone = getValue('memberPhone');
    if (phone == '') {
        e.preventDefault();
        showError('memberPhone', 'Vui lòng nhập Số điện thoại!');
    } else {
        showError('memberPhone', '');
    }

    //  ngày sinh
    var date = getValue('memberDate');
    if (date == '') {
        e.preventDefault();
        showError('memberDate', 'Vui lòng nhập ngày sinh!');
    } else {
        showError('memberDate', '');
    }

    //  email
    function validateEmail(email) {
        var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailPattern.test(email);
    }

    var email1 = getValue('memberEmail');
    if (email1.trim() === '') {
        e.preventDefault();
        showError('memberEmail', 'Vui lòng nhập Email!');
    } else if (!validateEmail(email1)) {
        e.preventDefault();
        showError('memberEmail', 'Email không hợp lệ!');
    } else {
        showError('memberEmail', '');
    }

    //  địa chỉ
    var locate = getValue('memberLocation');
    if (locate == '') {
        e.preventDefault();
        showError('memberLocation', 'Vui lòng nhập địa chỉ!');
    } else {
        showError('memberLocation', '');
    }
}

const togglePassword = document.getElementById('togglePassword');
togglePassword.addEventListener('click', function (e) {
    const password = document.getElementById('OldPass');
    if (password.type === "password") {
        password.type = "text";
    } else {
        password.type = "password";
    }
    // toggle the type attribute
    // const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    // password.setAttribute('type', type);
    // toggle the eye slash icon
    // this.classList.toggle('fa-eye-slash');
});

const togglePassword2 = document.getElementById('togglePassword2');
togglePassword2.addEventListener('click', function (e) {
    const password = document.getElementById('NewPass');
    if (password.type === "password") {
        password.type = "text";
    } else {
        password.type = "password";
    }

});

const togglePassword3 = document.getElementById('togglePassword3');
togglePassword3.addEventListener('click', function (e) {
    const password = document.getElementById('ReNewPass');
    if (password.type === "password") {
        password.type = "text";
    } else {
        password.type = "password";
    }

});
