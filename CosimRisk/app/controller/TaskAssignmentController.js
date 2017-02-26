var Task_autoid;
var prj_id;
Ext.define('CosimRisk.controller.TaskAssignmentController', {
    extend: 'Ext.app.Controller',
    views: ['CosimRisk.view.TaskAssignment'],
    models: ['CosimRisk.model.ResourceNameModel', 'CosimRisk.model.TaskResourceModel'],
    stores: ['ResourceNameStore', 'TaskResourceStore'],
    init: function () {
        this.control({
            'TaskAssignment': {

                beforeactivate: function (o, eOpts)
                {
                    var win = o.ownerCt;
                    var win_id = win.id;
                    var array = new Array();
                    array = win_id.split(",");
                    Task_autoid = array[0];
                    prj_id = array[1];
                    Ext.getStore('TaskResourceStore').load({ params: { taskAutoId: Task_autoid } });
                }
            },
            'TaskAssignment combo[id=TaskRes_Name]': {
                expand: function ( o, The, eOpts )
                {
                    Ext.getStore('ResourceNameStore').load({ params: { projectId: prj_id } });
                }
            },
            'TaskAssignment button[id=TaskRes_Confirm]': {
                click: function (o, The, eOpts) {
                    res = new Object();
                    var resource_id = -1;
                    var mount = Ext.getCmp('TaskRes_Require').getValue();
                    var same = false;

                            for (var i = 0; i < Ext.getStore('TaskResourceStore').getCount() ; i++) {
                                if (Ext.getCmp('TaskRes_Name').getValue() == Ext.getStore('TaskResourceStore').getAt(i).data.Resource_Name)
                                {
                                    same = true;
                                    resource_id = Ext.getStore('TaskResourceStore').getAt(i).data.Auto_id;
                                }
                            }
                            res.name = Ext.getCmp('TaskRes_Name').getValue();
                            res.assign_amount = mount;
                            res.Task_autoid = Task_autoid;
                            res.resource_id = resource_id;
                            json = Ext.encode(res);
                            var method;
                            if (same)
                                method = 31;
                            else
                                method = 29;
                            Ext.Ajax.request({
                                url: '/SeverRes/Handler.ashx?method=' + method,
                                params: json,
                                method: 'post',
                                success: function (resp, opts) {
                                    var respText = Ext.JSON.decode(resp.responseText);
                                    if (respText.success == true) {
                                        Ext.getStore('TaskResourceStore').load({ params: { taskAutoId: Task_autoid } });
                                    }
                                    else if (respText.success == false)
                                        Ext.Msg.alert('错误', respText.errMsg);
                                }
                            });
                       
                }
            },
            'TaskAssignment button[id=TaskRes_Delete]': {
                click: function (o, The, eOpts) {
                    res = new Object();
                    var grid = Ext.getCmp('TaskRes_Grid');
                    var data = grid.getSelectionModel().getSelection();
                    if (data.length == 0) {
                        Ext.Msg.alert("提示", "您要选择一个资源");
                    }
                    else {
                        res.Task_autoid = Task_autoid;
                        res.resource_id = data[0].get("Auto_id");
                        json = Ext.encode(res);

                        Ext.Ajax.request({
                            url: '/SeverRes/Handler.ashx?method=32',
                            params: json,
                            method: 'post',
                            success: function (resp, opts) {
                                var respText = Ext.JSON.decode(resp.responseText);
                                if (respText.success == true) {
                                    Ext.getStore('TaskResourceStore').load({ params: { taskAutoId: Task_autoid } });
                                }
                                else if (respText.success == false)
                                    Ext.Msg.alert('错误', respText.errMsg);
                            }
                        });
                    }
                }
            }

        })
    }
});