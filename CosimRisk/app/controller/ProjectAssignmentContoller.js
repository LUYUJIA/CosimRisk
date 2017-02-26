var project_Id;
Ext.define('CosimRisk.controller.ProjectAssignmentContoller', {
    extend: 'Ext.app.Controller',
    views: ['CosimRisk.view.ProjectAssignment','ProjectGrid'],
    stores: ['ProjectResourceStore', 'ResourceWarehouseStore'],
    models:['ProjectResourceModel', 'ResourceWarehouseModel'],
    init: function () {
        this.control({
            'ProjectGrid': {
                cellclick: function (grid, td, cellIndex, record, tr, rowIndex, e, eOpts) {
                    if (cellIndex == 6) {
                        project_Id = record.get("PRJ_ID");
                        var win = Ext.create('CosimRisk.view.ProjectAssignment');
                        Ext.getStore('ResourceWarehouseStore').reload();            //与后台交互，获得已有资源
                        Ext.getStore('ProjectResourceStore').load({ params: { projectId: project_Id } });
                        win.setTitle(record.get('PRJ_NAME'));
                        win.show();
                        
                    }
                }
            },
            'ProjectAssignment button[id=PrjRes_rightBtn]': {

                click: function () {

                        res = new Object();
                        var grid = Ext.getCmp('ProjectRes_left');
                        var data = grid.getSelectionModel().getSelection();
                        if (data.length == 0) {
                            Ext.Msg.alert("提示", "您要选择一个资源");
                        }
                        else
                        {
                           
                            var mount = Ext.getCmp('PrjRes_amount').getValue();
                            var remain = data[0].get("Resource_Remains");
                            if (mount == null)
                                alert("请设置数量");
                            else if (mount <= 0)
                                alert("设置数量错误");
                            else if (mount >remain)
                                alert("不足分配");
                            else {
                                var same = false;

                                for(var i=0;i<Ext.getStore('ProjectResourceStore').getCount();i++)
                                {
                                    if (data[0].get("Resource_Name") == Ext.getStore('ProjectResourceStore').getAt(i).data.Resource_Name)
                                    {
                                        same = true;
                                        res.assign_original = Ext.getStore('ProjectResourceStore').getAt(i).data.Resource_Mount;
                                    }    
                                }
                                res.project_id = project_Id;
                                res.name = data[0].get("Resource_Name");
                                res.resrouce_id = data[0].get("Auto_id");
                                res.assign_amount = mount;
                                res.Isright = true;
                                json = Ext.encode(res);
                                var method;
                                if (same)
                                    method = 7;
                                else
                                    method = 5;
                                Ext.Ajax.request({
                                    url: '/SeverRes/Handler.ashx?method='+method,
                                    params: json,
                                    method: 'post',
                                    success: function (resp, opts) {
                                        var respText = Ext.JSON.decode(resp.responseText);
                                        if (respText.success == true)
                                        {
                                            Ext.getStore('ResourceWarehouseStore').reload();
                                            Ext.getStore('ProjectResourceStore').load({ params: { projectId: project_Id } });
                                        }
                                        else if (respText.success == false)
                                            Ext.Msg.alert('错误', respText.errMsg);
                                    }
                                });
                         }
                      }
                }
            },
            'ProjectAssignment button[id=PrjRes_leftBtn]': {

                click: function () {

                    res = new Object();
                    var grid = Ext.getCmp('ProjectRes_right');
                    var data = grid.getSelectionModel().getSelection();
                    if (data.length == 0) {
                        Ext.Msg.alert("提示", "您要选择一个资源");
                    }
                    else {

                        var mount = Ext.getCmp('PrjRes_amount').getValue();
                        var remain = data[0].get("Resource_Mount");
                        if (mount == null)
                            alert("请设置数量");
                        else if (mount <= 0)
                            alert("设置数量错误");
                        else if (mount > remain)
                            alert("不足分配");
                        else {
                            res.project_id = project_Id;
                            res.name = data[0].get("Resource_Name");
                            res.resrouce_id = data[0].get("Auto_id");
                            res.assign_amount = mount;
                            res.assign_original = data[0].get("Resource_Mount");
                            res.Isright = false;
                            json = Ext.encode(res);
                            var method;
                            if (mount < remain)//7号更新
                                method = 7;
                            else if(mount == remain)//相当于删除
                                method = 8;
                            Ext.Ajax.request({
                                url: '/SeverRes/Handler.ashx?method=' + method,
                                params: json,
                                method: 'post',
                                success: function (resp, opts) {
                                    var respText = Ext.JSON.decode(resp.responseText);
                                    if (respText.success == true) {
                                        Ext.getStore('ResourceWarehouseStore').reload();
                                        Ext.getStore('ProjectResourceStore').load({ params: { projectId: project_Id } });
                                    }
                                    else if (respText.success == false)
                                        Ext.Msg.alert('错误', respText.errMsg);
                                }
                            });
                        }
                    }
                }
            }
        })
    }
});