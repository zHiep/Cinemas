

// Đổi màu ghế khi Click
let couchList = document.querySelectorAll('.seat_option--row .fa-couch');
let selectedSeats = 0;

couchList.forEach(item => {
    item.addEventListener('click', function (event) {
        const textColor = document.querySelector(`.couch${item.getAttribute('data-key')}`);
        const numColor = textColor.querySelector(`.couch_number`);

        // Kiểm tra số lượng ghế đã chọn, nếu đã đủ 8 ghế thì không cho phép chọn thêm
        if (selectedSeats >= 8 && textColor.style.color !== 'rgb(3, 89, 158)') {
            $('#staticBackdrop').modal('show')
            return;
        }

        if (textColor.style.color === 'rgb(3, 89, 158)') {
            // Bỏ chọn ghế
            textColor.style.color = '#BABBC4';
            numColor.style.color = '#000';
            selectedSeats--;

            // Xoá số ghế khi bỏ chọn
            let listCart = document.querySelectorAll('.ticket_text--seat .couch_position');
            listCart.forEach(cart => {
                if (cart.getAttribute('data-key2') == item.getAttribute('data-key')) {
                    cart.remove();
                }
            });
        } else {
            // Chọn ghế mới
            textColor.style.color = '#03599E';
            numColor.style.color = '#fff';
            selectedSeats++;

            // Hiển thị số ghế khi chọn
            var itemChild = item.querySelector('.couch_position');
            var itemNew = itemChild.cloneNode(true);
            itemNew.classList.remove('couch_number');
            itemNew.style.color = '#000';
            itemNew.style.marginRight = '7px';
            document.querySelector('.ticket_text--seat').appendChild(itemNew);
        }

        // Tổng số ghế đã đặt và tổng tiền thanh toán 
        let soPhanTu = document.querySelector('.ticket_text--seat');
        let soGhe = soPhanTu.querySelectorAll('*').length;
        let soluongghe = document.getElementById('soluongghe');
        let tongtien = document.getElementById('tongtien');
        let thanhtien = document.getElementById('thanhtien');
        let hienthiGhe = document.querySelector('.seat_number span');
        hienthiGhe.innerHTML = soGhe;
        soluongghe.innerHTML = soGhe;
        let tongTien = document.querySelector('.seat_totalPrice span');
        let elementgiave = document.getElementById('giave');
        var giave = parseFloat(elementgiave.outerText);
        let chiPhi = soGhe * giave;
        tongTien.innerHTML = chiPhi;
        tongtien.innerHTML = chiPhi
        thanhtien.innerHTML = chiPhi



    });
})