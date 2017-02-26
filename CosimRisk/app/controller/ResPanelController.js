Ext.define('CosimRisk.controller.ResPanelController', {
    extend: 'Ext.app.Controller',
    views: ['CosimRisk.view.ResourceAddedPanel'],
    store: ['ResourceWarehouseStore'],
    model: ['ResourceWarehouseModel'],
    init: function () {
        this.control({
            'ResourceAddedPanel button[id=AddRecBtn]': {
                click: function () {
                     res = new Object();
                     res.Resource_name = Ext.getCmp('Resource_Name').getValue();
                     res.Resource_mount = Ext.getCmp('Resource_Mount').getValue();
                     res.Resource_remains = res.Resource_mount;
                     res.Resource_unit_price = Ext.getCmp('Resource_Price').getValue();
                     res.Resource_type = Ext.getCmp('Resource_Type').getValue();
                     res.Resource_description = Ext.getCmp('Resource_Description').getValue();
                     json = Ext.encode(res);
                     Ext.Ajax.request({
                         url: '/SeverRes/Handler.ashx?method=24',
                         params: json,
                         method: 'post',
                         success: function (resp, opts) {
                             var respText = Ext.JSON.decode(resp.responseText);
                             if (respText.success == true)
                                 Ext.getStore('ResourceWarehouseStore').reload();
                             else if (respText.success == false)
                                 Ext.Msg.alert('错误', respText.errMsg);
                         }
                     });
                  
                }
            },
            'ResourceAddedPanel button[id=DeleteRecBtn]':
            {
                click: function () {
                    res = new Object();
                    var grid = Ext.getCmp('Warehouse_ResourceGrid');
                    var data = grid.getSelectionModel().getSelection();
                    if (data.length == 0) {
                        Ext.Msg.alert("提示", "您要选择一个资源");
                    }
                    else {
                        res.Resource_name = data[0].get("Resource_Name");
                        json = Ext.encode(res);
                        Ext.Ajax.request({
                            url: '/SeverRes/Handler.ashx?method=26',
                            params: json,
                            method: 'post',
                            success: function (resp, opts) {
                                var respText = Ext.JSON.decode(resp.responseText);
                                if (respText.success == true)
                                    Ext.getStore('ResourceWarehouseStore').reload();
                                else if(respText.success == false)
                                Ext.Msg.alert('错误', respText.errMsg);
                                
                            }
                        });
                    }
                }
            },
            'ResourceAddedPanel button[id=modifyRecBtn]':
            {
                click: function () {
                    res = new Object();
                    res.Resource_name = Ext.getCmp('Resource_Name').getValue();
                    res.Resource_mount = Ext.getCmp('Resource_Mount').getValue();
                    res.Resource_remains = null;
                    res.Resource_unit_price = Ext.getCmp('Resource_Price').getValue();
                    res.Resource_type = Ext.getCmp('Resource_Type').getValue();
                    res.Resource_description = Ext.getCmp('Resource_Description').getValue();
                    
                    var grid = Ext.getCmp('Warehouse_ResourceGrid');
                    var data = grid.getSelectionModel().getSelection();
                    res.Auto_id = data[0].get("Auto_id");

                    if (data.length == 0) {
                        Ext.Msg.alert("提示", "您要选择一个资源");
                    }
                   // else if (data[0].get("Resource_Remains") < data[0].get("Resource_Mount") && res.Resource_name != data[0].get("Resource_Name"))
                    //{
                   //     alert("错误!资源分配中，不能修改名字");
                   // }
                    else if (data[0].get("Resource_Mount") - res.Resource_mount > data[0].get("Resource_remains"))
                    {
                        alert("错误，资源数量不能少于分配量");
                    }
                    else {
                        res.Resource_remains = data[0].get("Resource_Remains") - (data[0].get("Resource_Mount") - res.Resource_mount);
                        json = Ext.encode(res);
                        Ext.Ajax.request({
                            url: '/SeverRes/Handler.ashx?method=25',
                            params: json,
                            method: 'post',
                            success: function (resp, opts) {
                                var respText = Ext.JSON.decode(resp.responseText);
                                if (respText.success == true)
                                    Ext.getStore('ResourceWarehouseStore').reload();
                                else if (respText.success == false)
                                    Ext.Msg.alert('错误', respText.errMsg);
                            }
                        });
                    }
                }
            },
            'ResourceGrid':
            {
                select: function (o, record, index, eOpts)
                {
                    Ext.getCmp('Resource_Name').setValue(record.get('Resource_Name'));
                    Ext.getCmp('Resource_Mount').setValue(record.get('Resource_Mount'));
                    Ext.getCmp('Resource_Price').setValue(record.get('Resource_Price'));
                    Ext.getCmp('Resource_Type').setValue(record.get('Resource_Type'));
                    Ext.getCmp('Resource_Description').setValue(record.get('Resource_Description'));
                }
            }
        })
    }
});