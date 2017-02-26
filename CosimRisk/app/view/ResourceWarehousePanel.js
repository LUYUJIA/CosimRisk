Ext.define('CosimRisk.view.ResourceWarehousePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.ResourceWarehousePanel',
    id: 'ResourceWarehousePanel',
    title: '总资源管理',
    closable: true,
    autoScroll: true,
    bodyStyle: 'background:#E5E5E5;padding:0px',
    layout: 'border',
    items: [{
        xtype: 'ResourceGrid',
        id: 'Warehouse_ResourceGrid',
        region: 'north'
    }, {
        xtype: "ResourceAddedPanel",
        id: 'ResourceAddedPanel',
        height: 120,
        region: 'south'
    }]
});