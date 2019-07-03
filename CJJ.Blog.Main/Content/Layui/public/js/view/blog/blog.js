
//公共选择文件方法
//el:上传控件
//行程单
function selfileupload(el, pl) {
    var files = $(el)[0].files;
    var isimg = true;
    $(files).each(function (i, item) {
        if (isimg && item.type.indexOf("image/") == -1) {
            isimg = false;
        }
    });
    if (!isimg) {
        layer.msg("请选择图片文件，不能选择非图片文件");
        return;
    }
    var load = layer.load();
    upload(el, "groupmanagerimg", function (data) {
        if (data.IsSuccess) {
            var imgs = data.Message.split(",");
            var domain = data.DoMain + "/";
            if (pl === 'carimgs') {
                var imgdom = $(el).parent().find(".layui-upload-list > #addtwo");
                $(imgs).each(function (i, item) {
                    var imghtml = '<div class="img-div"><img class="layui-upload-img img-img" src="' + domain + item + '"><a class="close-modal small" data-url="' + domain + item + '" onclick="delImg(this,' + "'carimgdel'" + ')">×</a></div>';
                    imgdom.before(imghtml);
                });
            }
            else if (pl === 'itineraryurl') {//添加行程单，隐藏+号
                var imgdom = $(el).parent().find(".layui-upload-list");
                $("#addone").hide();
                $(imgs).each(function (i, item) {
                    var imghtml = '<div class="img-div"><img class="layui-upload-img img-img" src="' + domain + item + '"><a class="close-modal small" data-url="' + domain + item + '" onclick="delImg(this,' + "'itineraryurldel'" + ')">×</a></div>';
                    imgdom.append(imghtml);
                });
            }

        } else {
            layer.msg(data.Message);
            return;
        }
        layer.closeAll('loading');
    });
}

function delImg(elm, pl) {
    var url = $(elm).attr("data-url");
    $.ajax({
        url: "/Slibraryimg/DeleteByUrl",
        type: "POST",
        datatype: "json",
        data: { "url": url },
        success: function (res) {
            //没有行程单时显示加号
            if (pl === 'itineraryurldel') {
                var a = $(elm).parent().siblings().find('layui-upload-img img-img');
                if (a.length <= 0) {
                    $("#addone").show();
                }
            }

            if (res.msg == null) {
                $(elm).parents('.img-div').remove();
                layer.msg("删除成功!");
            }
        },
        error: function (error) {
            layer.msg(error);
        }
    });
}

//限制图片上传张数
function uploadImg(ele, max) {
    var _this = $(ele);
    var uploadBox = _this.parents('.upload_box');
    var imgNum = uploadBox.find('.img-div').length;
    if (max <= imgNum) {
        layer.msg('上传图片数量超过限制！');
    } else {
        uploadBox.find('.upload_img').click();
    }
}

layui.use(['form', 'laydate', 'layer'], function () {
    var form = layui.form,
        laydate = layui.laydate,
        layer = layui.layer;

    //laydate init
    lay('.date_input').each(function () {
        laydate.render({
            elem: this,
            format: 'yyyy-MM-dd'
        })
    });

    //数据验证
    var $TravelGroupNum = $('#TravelGroupNum');
    var $LineName = $('#LineName');
    var $MemberNum = $('#MemberNum');
    var $ChildNum = $('#ChildNum');
    var $OutTime = $('#OutTime');
    var $BackTime = $('#BackTime');
    var $CarNum = $('#CarNum');
    var $itineraryUrlList = $('#itineraryUrlList');
    var itineraryUrlList = [];
    var $carImgUrl = $('#carImgUrl');
    var carImgUrl = [];

    function getUrl(elm, arr) {
        arr = [];
        elm.find('.img-div').each(function (index, item) {
            var src = $(item).find('img').attr('src');
            arr.push(src);
        });
        return arr;
    }

    function checkData() {
        //行程单
        itineraryUrlList = getUrl($itineraryUrlList);
        //车单
        carImgUrl = getUrl($carImgUrl);
        if ($TravelGroupNum.val().length == 0) {
            layer.msg('请输入出团团号！');
            $TravelGroupNum.focus();
            return false;
        }
        if ($LineName.val().length == 0) {
            layer.msg('请输入线路名称！');
            $LineName.focus();
            return false;
        }
        if ($MemberNum.val().length == 0) {
            layer.msg('请输入本团总人数！');
            $MemberNum.focus();
            return false;
        }
        if ($ChildNum.val().length == 0) {
            layer.msg('请输入本团包含儿童人数，若无则填0！');
            $ChildNum.focus();
            return false;
        }
        if ($OutTime.val().length == 0) {
            layer.msg('请选择出团日期！');
            return false;
        }
        if ($BackTime.val().length == 0) {
            layer.msg('请选择回团日期！');
            return false;
        }
        if ($BackTime.val() <= $OutTime.val()) {
            layer.msg('回团日期必须大于出团日期！');
            return false;
        }
        if ($CarNum.val().length == 0) {
            layer.msg('请输入本团车单号，若多个车单可用逗号隔开！');
            $CarNum.focus();
            return false;
        }
        if (itineraryUrlList.length == 0) {
            layer.msg('请上传行程单！');
            return false;
        }
        if (carImgUrl.length == 0) {
            layer.msg('请上传车单！');
            return false;
        }
        return true;
    }
    form.on('submit', function (data) {
        var field = data.field;
        if (checkData()) {
            field.ItineraryUrl = itineraryUrlList[0];
            field.CarImgUrl = carImgUrl.join(',');
            PostSubmit(field);

        }
    })

    //提交后台
    var PostSubmit = function (Obj) {
        var load = layer.load(0, { shade: 0.5 });
        $.post("/Groupmanager/AddOrEditPost",
            Obj,
            function (d) {
                layer.closeAll('loading');
                if (d.code == 0) {
                    layer.closeAll();
                    parent.layer.closeAll();

                } else {
                    layer.msg(d.msg);
                }
            });
    }
})
