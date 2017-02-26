Ext.define('CosimRisk.view.ResourceGrid', {
    extend: 'Ext.grid.Panel',
    height: 400,
    width: 550,
    alias: 'widget.ResourceGrid',
    selModel: Ext.create('Ext.selection.CheckboxModel', {
        mode: "SINGlE",
        showHeaderCheckbox: false
    }),
    id: 'ResourceGrid',
    autoScroll: true,
    store: 'ResourceWarehouseStore',
    columns: [{
        xtype: 'rownumberer',
        width: 50,
        text: '序号'
    }, {
        text: '资源名称',
        dataIndex: 'Resource_Name',
        flex: 1
    }, {
        text: '资源类型',
        dataIndex: 'Resource_Type',
        flex: 1
    },
    {
        text: '资源总数目',
        dataIndex: 'Resource_Mount',
        flex: 1
    }, {
        text: '资源剩余数',
        dataIndex: 'Resource_Remains',
        flex: 1
    }, {
        text: '资源单价(单位:万元)',
        dataIndex: 'Resource_Price',
        flex: 1
    }, {
        text: '资源描述',
        dataIndex: 'Resource_Description',
        flex: 1
    }]
});