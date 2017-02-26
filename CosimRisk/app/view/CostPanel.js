Ext.define('CosimRisk.view.CostPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.CostPanel',
    id: 'CostPanel',
    title: '成本风险图',
    closable: true,
    autoScroll: true,
    bodyStyle: 'background:#E5E5E5;padding:0px',
    layout: 'border',
    items: [{
        xtype: 'panel',
        id: 'CostChartPanel',
        region: 'south'
    }, {
        xtype: "CostAddedPanel",
        id: 'CostAddedPanel',
        height: 100,
        region: 'north'
    }]
});