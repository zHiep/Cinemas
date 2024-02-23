
		const buyBtns = document.querySelectorAll('.js-buy-movie')
		const modal = document.querySelector('.js-modal')
        // click X 
		const modalContainer = document.querySelector('.js-modal-container') 
		const modalClose = document.querySelector('.js-modal-close') 
		const modalFtClose = document.querySelector('.js-btn-close')
		// Hàm hiển thị  modal mua vé ( thêm class open vào modal)
		function hideBuyMovie(){
			modal.classList.remove('open')
		}
		// Hàm ẩn modal mua vé ( Gỡ bõ class open của modal)
		function showBuyMovie(){
			modal.classList.add('open')
		}
		// Lặp qua từng thẻ Button và nghe hành vi click
		for(const buyBtn of buyBtns){
			buyBtn.addEventListener('click',showBuyMovie)
		}
		

		// Nghe hành vi click vào dấu x
		modalClose.addEventListener('click',hideBuyMovie)

		modal.addEventListener('click',hideBuyMovie)

		modalContainer.addEventListener('click',function(event){
			event.stopPropagation()
		})
		modalFtClose.addEventListener('click',hideBuyMovie)
 


// <!-- modal 2 -->

    const buyBtns2 = document.querySelectorAll('.js_buy--movie-2')
    const modal2 = document.querySelector('.js_modal--2')
    // click X 
		const modalContainer2 = document.querySelector('.js_modal--container--2')

    const modalClose2 = document.querySelector('.js_modal--close-2') 
    const modalFtClose2 = document.querySelector('.js_btn--close-2')
    // Hàm hiển thị  modal mua vé ( thêm class open vào modal)
    function hideBuyMovie2(){
        modal2.classList.remove('open')
    }
    // Hàm ẩn modal mua vé ( Gỡ bõ class open của modal)
    function showBuyMovie2(){ 
        modal2.classList.add('open')
    }
    // Lặp qua từng thẻ Button và nghe hành vi click
    for(const buyBtn2 of buyBtns2){
        buyBtn2.addEventListener('click',showBuyMovie2)
    }
    

    // Nghe hành vi click vào dấu x
    modalClose2.addEventListener('click',hideBuyMovie2)

    modal2.addEventListener('click',hideBuyMovie2)

    modalContainer2.addEventListener('click',function(event){
        event.stopPropagation()
    })
    modalFtClose2.addEventListener('click',hideBuyMovie2)



/* TABLINk  viewdate */
        var tabLinks = document.querySelectorAll(".tablinks");
        
        var btn1 = document.querySelector(".tablinks")
        var btn2 = document.querySelector(".tabcontent")
        btn1.classList.add('active')
        btn2.classList.add('active1')
        tabLinks.forEach(function(el) {
        el.addEventListener("click", (el) => {
            var tabContent =document.querySelectorAll(".tabcontent");
            var btn = el.currentTarget; // lắng nghe sự kiện và hiển thị các element
            console.log(btn)
            var electronic = btn.getAttribute('electronic'); // lấy giá trị trong data-electronic
            console.log(electronic)
            tabContent.forEach(function(el) {
                el.classList.remove("active1");
            }); //lặp qua các tab content để remove class active
    
            tabLinks.forEach(function(el) {
                el.classList.remove("active");
            }); //lặp qua các tab links để remove class active
    
            document.querySelector("#" + electronic).classList.add("active1");
            // trả về phần tử đầu tiên có id="" được add class active
            
            btn.classList.add("active");
            // các button mà chúng ta click vào sẽ được add class active
            });

        });


/* TABLINk movie_status */

        var tabLinks_grid = document.querySelectorAll(".tablinks_grid");
        
        var btn1_grid = document.querySelector(".tablinks_grid")
        var btn2_grid = document.querySelector(".tabcontent_grid")
        btn1_grid.classList.add('active')
        btn2_grid.classList.add('active1')
        tabLinks_grid.forEach(function(el) {
        el.addEventListener("click", (el) => {
            var tabContent_grid =document.querySelectorAll(".tabcontent_grid");
            var btn_grid = el.currentTarget; // lắng nghe sự kiện và hiển thị các element
            console.log(btn_grid)
            var electronic = btn_grid.getAttribute('electronic'); // lấy giá trị trong data-electronic
            console.log(electronic)
            tabContent_grid.forEach(function(el) {
                el.classList.remove("active1");
            }); //lặp qua các tab content để remove class active
    
            tabLinks_grid.forEach(function(el) {
                el.classList.remove("active");
            }); //lặp qua các tab links để remove class active
    
            document.querySelector("#" + electronic).classList.add("active1");
            // trả về phần tử đầu tiên có id="" được add class active
            
            btn_grid.classList.add("active");
            // các button mà chúng ta click vào sẽ được add class active
            });


        });


      
