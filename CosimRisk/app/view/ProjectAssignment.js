Ext.define('CosimRisk.view.ProjectAssignment', {
    extend: 'Ext.window.Window',
    alias: 'widget.ProjectAssignment',
    id: 'ProjectAssignment',
    autoScroll: true,
    title: '项目资源管理',
    height: 400,
    width: 700,
    modal: 'true',
    resizable: false,
    layout: 'border',
    items: [{
        xtype: 'grid',
        title: '项目分配资源',
        id: 'ProjectRes_right',
        store: 'ProjectResourceStore',
        selModel: Ext.create('Ext.selection.CheckboxModel', {
            mode: "SINGlE",
            showHeaderCheckbox: false
        }),
        region: 'east',
        height: 350,
        width: 290,
        autoScroll: true,
        columns: [{
            text: '名称',
            dataIndex: 'Resource_Name',
            flex: 1
        }, {
            text: '类型',
            dataIndex: 'Resource_Type',
            flex: 1
        },
    {
        text: '数目',
        dataIndex: 'Resource_Mount',
        flex: 1
    }, {
        text: '单价',
        dataIndex: 'Resource_Price',
        flex: 1
    }]
    }, {
        xtype: 'grid',
        title: '总资源',
        id: 'ProjectRes_left',
        store: 'ResourceWarehouseStore',
        selModel: Ext.create('Ext.selection.CheckboxModel', {
            mode: "SINGlE",
            showHeaderCheckbox: false
        }),
        region: 'west',
        height: 350,
        width: 320,
        autoScroll: true,
        columns: [{
            text: '名称',
            dataIndex: 'Resource_Name',
            flex: 1
        }, {
            text: '类型',
            dataIndex: 'Resource_Type',
            flex: 1
        },
    {
        text: '剩余量',
        dataIndex: 'Resource_Remains',
        flex: 1
    }, {
        text: '单价',
        dataIndex: 'Resource_Price',
        flex: 1
    }]
    }, {
        xtype: 'panel',
        border: false,
        region: 'center',
        layout: 'absolute',
        items: [{
            xtype: 'button',
            cls: 'right-button-background',
            id:'PrjRes_rightBtn',
            height: 24,
            width: 24,
            x: 25,
            y: 60
        }, {
            xtype: 'text',
            text: '分配数量',
            //margin: '10 0 0 5',
            width: 55,
            height: 35,
            x: 10,
            y: 100
        }, {
            xtype: 'textfield',
            allowBlank: false,
            id: 'PrjRes_amount',
            //margin: '10 0 0 5',
            regex:/^[0-9]*$/,
            width: 35,
            height: 35,
            x: 19,
            y: 130
        }, {
            xtype: 'button',
            cls: 'left-button-background',
            id: 'PrjRes_leftBtn',
            height: 24,
            width: 24,
            x: 25,
            y: 190
        }]
    }]
});