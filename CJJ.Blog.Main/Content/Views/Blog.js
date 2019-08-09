/**

 @Name：layuiAdmin Go.Travel
 @Author：star1029
 @Site：http://www.layui.com/admin/
 @License：LPPL
    
 */
layui.define(['table', 'form'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form;

    //监听搜索
    form.on('submit(LAY-Blog-front-search)', function (data) {
        var field = data.field;
        //执行重载
        table.reload('LAY-Blog-manage', {
            where: field,
            page: 1
        });
    });

    //事件
    var active = {

         Add: function () {
            layer.open({
                type: 1
                , title: '添加'
                , content: 'AddOrEdit'
                , area: [resizeIframe(640) + 'px', resizeIframe(740) + 'px']
                , end: function () {
                    if ($("button").hasClass("layui-laypage-btn")) {
                        $(".layui-laypage-btn").click(); //数据刷新
                    } else {
                        table.reload('LAY-Blog-manage', {});
                    }
                }
            });
        }
    };

    //点击查看大图
    $(document).on('click', '.show_img', function () {
        var src = $(this).attr('src');
        var html = '<img src =' + src + ' style="width:100%;">';
        layer.open({
            type: 1,
            title: false,
            area: 'auto',
            maxWidth: resizeIframe(1000),
            maxHeight: resizeIframe(800),
            content: html
        })
    })


    //监听工具条
    table.on('tool(LAY-Blog-manage)', function (obj) {
        var data = obj.data;


        if (obj.event === 'Delete') {
            //删除
            layer.confirm('确认删除该条出团信息？,删除后信息不可恢复', { title: '删除' }, function (index) {
                $.ajax({
                    type: "post",
                    url: "/Blog/Delete",
                    data: { "KID": data.KID },
                    dataType: "json",
                    success: function (ret) {
                        if (ret.code == "0") {
                            obj.del();
                            layer.close(index);
                        }
                        else {
                            if (ret.msg == null)
                                layer.msg("删除失败,服务器未返回失败信息");
                            else
                                layer.msg(ret.msg);
                        }
                    }
                });
            });

        }
        //确认行程
        else if (obj.event === 'Confirm') {
            layer.confirm('确定已检查附件文件无误，确认行程有效？', { title: '确认行程' }, function (index) {
                $.ajax({
                    type: "post",
                    url: "Confirm",
                    data: { "kid": data.KID },
                    dataType: "json",
                    success: function (ret) {
                        if (ret.code == "0") {
                            obj.del();
                            layer.close(index);
                            table.reload('LAY-Blog-manage'); //数据刷新
                        }
                        else {
                            if (ret.msg == null)
                                layer.msg("确认行程失败,服务器未返回失败信息");
                            else
                                layer.msg(ret.msg);
                        }
                    }
                });
            });

        }
        //编辑
        else if (obj.event === 'Edit') {
            var kid = data.KID;
            console.log(kid)
            window.location.href = '/Blog/AddOrEdit?blogid=' + kid;
            //layer.open({
            //    type: 2
            //    , title: '编辑'
            //    , maxmin: true
            //    , content: '/Blog/AddOrEdit?blogid=' + kid,
            //     area: 'auto'
            //    , end: function () {
            //        table.reload('LAY-Blog-manage'); //数据刷新
            //        //layer.close(index); //关闭弹层
            //    }
            //    , success: function (layero, index) {

            //    }
            //});

        }
        //日志
        else if (obj.event === 'Log') {
            var kid = data.KID;
            var proedit = layer.open({
                type: 2,
                area: ["860px","860px"]
                , title: '日志'
                , content: '/Bloglog/List?Blogid=' + kid
                , btn: ['关闭']
                , btnAlign: 'c'
                , end: function () {
                    table.reload('LAY-Blog-manage', {
                    });
                }
            });
        }
        //下载附件
        else if (obj.event === 'Download') {

            var kid = data.KID;
            window.location.href = '/Blog/GetFiles?kid=' + kid;
        }
    });

    exports('Blog', {})
});