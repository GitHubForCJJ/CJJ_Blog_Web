


layui.define(['zTree', 'jquery', 'table', 'form'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , zTree = layui.zTree
        , form = layui.form;

    var $Fatherid = $("input[name='Fatherid']");
    var $kid = $("input[name='KID']");
    var $Opertype = $("input[name='Opertype']");
    var $save = $("#Menusavebtn");
    var $header = $("#menuheader");
    $(function () {
        var setting = {
            view: {
                addHoverDom: addHoverDom,
                removeHoverDom: removeHoverDom,
                selectedMulti: false
            },
            edit: {
                enable: true,
                editNameSelectAll: true,
                showRemoveBtn: showRemoveBtn,
                showRenameBtn: false
            },
            data: {
                key: {
                    name: 'Menuname'
                },
                simpleData: {
                    enable: true,
                    idKey: 'KID',
                    pIdKey: 'Fatherid'
                }
            },
            callback: {
                beforeRemove: beforeRemove,
                onClick: onClick
            }
        };
        var zNodes = [];
        // 是否显示删除按钮
        function showRemoveBtn(treeId, treeNode) {
            return treeNode.id != 0;
        }
        // 点击node
        function onClick(event, treeId, treeNode, clickFlag) {
            //设置为修改
            $('input[name="Opertype"]').val('edit');
            reForm();
            if (treeNode.KID == 0) {
                layer.msg('顶级目录不允许编辑');
                clearForm();
                return;
            }
            console.log(treeNode)
            $('input[name="Menuname"]').val(treeNode.Menuname);
            $('input[name="KID"]').val(treeNode.KID);
            $('input[name="Menuicon"]').val(treeNode.Menuicon);
            $('input[name="MenuUrl"]').val(treeNode.MenuUrl);
            $('input[name="Menusort"]').val(treeNode.Menusort);
            $('input[name="Menuremark"]').val(treeNode.Menuremark);
            $Fatherid.val(treeNode.pId);
        }
        //清除右侧表单
        function clearForm() {
            $('input[name="Menuname"]').val("");
            $('input[name="KID"]').val("");
            $('input[name="Menuicon"]').val("");
            $('input[name="MenuUrl"]').val("");
            $('input[name="Menusort"]').val("");
            $('input[name="Menuremark"]').val(treeNode.MenuMsg);
            $('input[name="KID"]').val("");
        }
        //切换form样式
        function reForm() {
            var action = $Opertype.val();
            $save.removeClass('layui-btn-disabled').attr('disabled', false);
            if (action == 'edit') {
                $header.text("修改菜单");
                $save.text("立即修改");

            }
            else {
                $header.text("新增菜单");
                $save.text("立即添加");
            }
        }
        // 删除前
        function beforeRemove(treeId, treeNode, isdelcallback) {
            var zTree = $.fn.zTree.getZTreeObj('LAY-Menu-Tree');
            zTree.selectNode(treeNode);
            if (treeNode.children) {
                layer.msg('该菜单下有子菜单，不能删除！');
                return false;
            }
            if (isdelcallback == 1) {
                $('#' + treeNode.tId).remove();
                var data = {};
                data.kid = treeNode.id;
                console.log(data)
                if (data.kid) {
                    //GHM.post(GHM_config.url.DelItemMenuByKid, { Data: JSON.stringify(data) }).then(function () {
                    //    initTree();
                    //    layer.msg('删除成功');
                    //})
                }
            } else {
                layer.confirm('确认删除 菜单 -- ' + treeNode.name + ' 吗？', {
                    btn: ['确认', '取消']
                }, function () {
                    //执行删除操作
                    beforeRemove(treeId, treeNode, 1);
                    layer.closeAll();
                });
                return false;
            }
        }
        // 鼠标移入事件
        function addHoverDom(treeId, treeNode) {
            var sObj = $("#" + treeNode.tId + "_span");
            if (treeNode.editNameFlag || $("#addBtn_" + treeNode.tId).length > 0) return;
            var addStr = "<span class='button add' id='addBtn_" + treeNode.tId
                + "' title='添加子节点' onfocus='this.blur();'></span>";
            sObj.after(addStr);
            var btn = $("#addBtn_" + treeNode.tId);
            if (btn) btn.bind("click", function () {
                $('input[name=MenuName]').focus();
                $Opertype.val('add');
                clearForm();
                reForm();
                console.log(treeNode)
                $Fatherid.val(treeNode.id);
                return false;//阻止onclick触发
            });

        }

        // 鼠标移出事件
        function removeHoverDom(treeId, treeNode) {
            $('#addBtn_' + treeNode.tId).unbind().remove();
        }

        $.ajax({
            type: 'post',
            url: '/Menu/list',
            dataType: 'json',
            contentType: 'application/json',
            success: function (res) {
                console.log(res)
                zNodes = res;
                for (var i in zNodes) {
                    zNodes[i].open = 'true';
                }
                zNodes.push({ KID: 0, Menuname: '菜单管理', open: 'true' });
                zTree.init($('#LAY-Menu-Tree'), setting, zNodes);
            },
            error: function (err) {
                console.log(err)
            }

        })

    })



    exports('Menu', {})
})