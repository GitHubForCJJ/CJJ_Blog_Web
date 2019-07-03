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

    //监听导出
    form.on('submit(LAY-Blog-front-ExportData)', function (data) {
        var field = data.field;

        var $iframe = $('<iframe id="down-file-iframe_Blog" />');
        var $form = $('<form target="down-file-iframe_Blog" method="post" />');
        $form.attr('action', "ExportData");
        for (var key in field) {
            $form.append('<input type="hidden" name="' + key + '" value="' + field[key] + '" />');
        }
        $iframe.append($form);
        $(document.body).append($iframe);
        $form[0].submit();
        $iframe.remove();
    });

    //事件
    var active = {
        BatchDelete: function () {
            var checkStatus = table.checkStatus('LAY-Blog-manage');
            //得到选中的数据
            var selectkids = "";
            for (var i = 0; i < checkStatus.data.length; i++) {
                selectkids += checkStatus.data[i].KID + ","
            }
            if (selectkids.length === 0) {
                return layer.msg('请选择数据');
            }
            layer.prompt({
                formType: 1
                , title: '敏感操作，请输入第一条数据ID验证'
            }, function (value, index) {
                if (value == checkStatus.data[0].KID) {
                    layer.close(index);

                    layer.confirm('确定删除吗？', function (index) {

                        $.ajax({
                            type: "post",
                            url: "Delete",
                            data: { "kid": selectkids },
                            dataType: "json",
                            success: function (ret) {
                                if (ret.code == "0") {
                                    layer.close(index);
                                    table.reload('LAY-Blog-manage', {
                                    });
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
                else {
                    layer.msg('口令错误,无法执行删除动作');
                    layer.close(index);
                }
            });
        }
        , Add: function () {
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

    //添加出团信息
    $('.layui-btn.layuiadmin-btn-Blog').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });

    //监听工具条
    table.on('tool(LAY-Blog-manage)', function (obj) {
        var data = obj.data;


        if (obj.event === 'Delete') {
            //删除
            layer.confirm('确认删除该条出团信息？,删除后信息不可恢复', { title: '删除' }, function (index) {
                $.ajax({
                    type: "post",
                    url: "Delete",
                    data: { "Blogid": data.KID },
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
            layer.open({
                type: 2
                , title: '编辑'
                , content: 'AddOrEdit?kID=' + kid
                , area: [resizeIframe(640) + 'px', resizeIframe(740) + 'px']
                , end: function () {
                    table.reload('LAY-Blog-manage'); //数据刷新
                    layer.close(index); //关闭弹层
                }
                , success: function (layero, index) {

                }
            });

        }
        //日志
        else if (obj.event === 'Log') {
            var kid = data.KID;
            var proedit = layer.open({
                type: 2,
                area: [resizeIframe(860) + 'px', resizeIframe(760) + 'px']
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