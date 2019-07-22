
$(document).ready(function () {
    var categorytype = $(".categorytype").val();
    console.log(categorytype)
    $.ajax({
        url: '/Blog/GetCatList',
        type: 'post',
        contentType: 'application/json',
        dataType: 'json',
        success: function (suc) {
            console.log(suc)
            //处理nav导航
            for (var item in suc.Categorys) {
                var kid = suc.Categorys[item].KID;
                if (kid == categorytype) {
                    $("#catlist li").removeClass('selected');
                    var li = $(" <li class='selected'><a href='/Blog/index?categorytype=" + kid + "'>" + suc.Categorys[item].Name + "</a></li>");
                    $("#catlist").append(li);
                }
                else {
                    var li = $(" <li><a href='/Blog/index?categorytype=" + kid + "'>" + suc.Categorys[item].Name + "</a></li>");
                    $("#catlist").append(li);
                }

            }
            //处理博客推荐
            for (var i in suc.BloginfoViews) {
                var kid = suc.BloginfoViews[i].KID;
                var title = suc.BloginfoViews[i].Title;
                var sorc = parseInt(i) + 1;
                var li = $(" <li><a href='/Blog/Detail?blogid=" + kid + "' target='_black'>" + sorc + "、 " + title + "</a></li>");
                $(".recommend ul").append(li);
            }
            //处理热点问题
            for (var i in suc.HotNews) {
                var kid = suc.HotNews[i].KID;
                var title = suc.HotNews[i].Title;
                var url = suc.HotNews[i].Url;
                var sorc = parseInt(i) + 1;
                var li = $(" <li><a href='" + url + "' title='" + title + "' target='_black'>" + sorc + "、 " + title + "</a></li>");
                $(".hot ul").append(li);
            }

        },
        error: function () {

        }
    })
})


var beforeScrollTop = $(window).scrollTop();
console.log(beforeScrollTop)
$(window).scroll(function () {
    var afterScrollTop = $(window).scrollTop(),
        updown = afterScrollTop - beforeScrollTop;
    if (updown === 0) return false;
    beforeScrollTop = afterScrollTop;

    var isUpDown = updown > 0 ? "down" : "up";  //判断往上还是往下
    if (updown > 0) {
        $(".header-nav").fadeOut(500);
    } else {
        $(".header-nav").fadeIn(500);
    }

})
$("#catlist li a").on("click", function (e) {
    $(this).parent().addClass('selected').siblings().removeClass('selected');
})