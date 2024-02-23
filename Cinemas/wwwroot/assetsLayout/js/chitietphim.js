const buyBtns2 = document.querySelectorAll('.js_buy--movie-2')
const modal2 = document.querySelector('.js_modal--2')
// click X 
const modalContainer2 = document.querySelector('.js_modal--container2')

const modalClose2 = document.querySelector('.js_modal--close-2')
const modalFtClose2 = document.querySelector('.js_btn--close-2')
// Hàm hiển thị  modal mua vé ( thêm class open vào modal)
function hideBuyMovie2() {
    modal2.classList.remove('open')
}
// Hàm ẩn modal mua vé ( Gỡ bõ class open của modal)
function showBuyMovie2() {
    modal2.classList.add('open')
}
// Lặp qua từng thẻ Button và nghe hành vi click
for (const buyBtn2 of buyBtns2) {
    buyBtn2.addEventListener('click', showBuyMovie2)
}


// Nghe hành vi click vào dấu x
modalClose2.addEventListener('click', hideBuyMovie2)

modal2.addEventListener('click', hideBuyMovie2)

modalContainer2.addEventListener('click', function (event) {
    event.stopPropagation()
})
modalFtClose2.addEventListener('click', hideBuyMovie2)
