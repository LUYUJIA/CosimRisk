Ext.define('CosimRisk.view.ResourceAddedPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.ResourceAddedPanel',
    id: 'ResourceAddedPanel',
    layout:'column',
    items: [{
        xtype: 'textfield',
        columnWidth: 0.35,
        labelWidth: 60,
        allowBlank: false,
        margin: '10 0 0 5',
        fieldLabel: '资源名称',
        id: 'Resource_Name',
        name: 'Resource_Name'
    }, {
        xtype: 'textfield',
        columnWidth: 0.3,
        labelWidth: 60,
        allowBlank: false,
        margin: '10 0 0 5',
        fieldLabel: '资源数目',
        id: 'Resource_Mount',
        name: 'Resource_Mount'
    }, {
        xtype: 'textfield',
        columnWidth: 0.3,
        labelWidth: 60,
        allowBlank: false,
        margin: '10 0 0 5',
        fieldLabel: '资源单价',
        id: 'Resource_Price',
        name: 'Resource_Price'
    }, {
        xtype: 'combo',
        columnWidth: 0.35,
        labelWidth: 60,
        border: false,
        anchor: '90%',
        id:'Resource_Type',
        editable: false,
        allowBlank: false,
        blankText: '不能为空',
        margin: '10 0 0 5',
        fieldLabel: '资源类型',
        displayField: 'resourceType',
        store: Ext.create('CosimRisk.store.resourceTypeStore')
    }, {
        xtype: 'textfield',
        columnWidth: 0.4,
        labelWidth: 60,
        //allowBlank: false,
        margin: '10 0 0 5',
        fieldLabel: '资源描述',
        id: 'Resource_Description',
        name: 'Resource_Description'
    }, {
        columnWidth: 0.3,
        xtype: 'button',
        margin: '15 0 0 50',
        text: '修改资源',
        id: 'modifyRecBtn'
    }, {
        columnWidth: 0.3,
        xtype: 'button',
        margin: '15 0 0 35',
        text: '添加资源',
        id: 'AddRecBtn'
    },
    {
        columnWidth: 0.3,
        xtype: 'button',
        margin: '15 0 0 35',
        text: '删除资源',
        id: 'DeleteRecBtn'
    }]
});