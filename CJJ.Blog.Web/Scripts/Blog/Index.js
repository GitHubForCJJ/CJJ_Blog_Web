

$(function () {
    var swiper = new Swiper('.swiper-container', {
        slidesPerView: 1,
        spaceBetween: 30,
        loop: true,
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
        navigation: {
            outline: 'none',
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',

        },
        on: {
            navigationHide: function () {
                alert('按钮隐藏了');
            }
        }
    });

    //$.ajax({
    //    type: "post",
    //    contentType: "json",
    //    url: 'index',
    //    data: { "categorytype": "1" },
    //    success: function () {

    //    },
    //    error: function () {

    //    }
    //})
})