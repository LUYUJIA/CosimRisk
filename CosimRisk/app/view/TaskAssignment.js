Ext.define('CosimRisk.view.TaskAssignment', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.TaskAssignment',
    id: 'TaskAssignment',
    title: '任务资源需求',
    height: 400,
    width: 700,
    layout: 'border',
    items: [{
        xtype: 'grid',
        title: '任务需求资源',
        id: 'TaskRes_Grid',
        store: 'TaskResourceStore',
        selModel: Ext.create('Ext.selection.CheckboxModel', {
            mode: "SINGlE",
            showHeaderCheckbox: false
        }),
        region: 'north',
        height: 300,
        width: 250,
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
        xtype: 'panel',
        region: 'south',
        layout: 'column',
        height: 80,
        width: 280,
        items: [{
            xtype: 'combo',
            columnWidth: 0.35,
            labelWidth: 60,
            store: 'ResourceNameStore',
            allowBlank: false,
            margin: '20 0 0 5',
            valueField: 'Resource_Name',
            displayField: 'Resource_Name',
            fieldLabel: '资源名称',
            id: 'TaskRes_Name',
            name: 'TaskRes_Name'
        }, {
            xtype: 'textfield',
            columnWidth: 0.2,
            labelWidth: 30,
            allowBlank: false,
            margin: '20 0 0 25',
            fieldLabel: '需求',
            id: 'TaskRes_Require',
            name: 'TaskRes_Require'
        }, {
            xtype: 'button',
            text: '添加',
            columnWidth: 0.2,
            margin: '20 0 0 15',
            id: 'TaskRes_Confirm',
            name: 'TaskRes_Confirm'
        }, {
            xtype: 'button',
            text: '删除',
            columnWidth: 0.2,
            margin: '20 0 0 15',
            id: 'TaskRes_Delete',
            name: 'TaskRes_Delete'
        }]
    }]

});