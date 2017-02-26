Ext.define('CosimRisk.view.CostAddedPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.CostAddedPanel',
    id: 'CostAddedPanel',
    layout: 'column',
    items: [{
        xtype: 'text',
        columnWidth: 0.55,
        margin: '10 0 0 5',
        text: '平均直接成本:',
        id: 'Direct_cost',
        name: 'Direct_cost',
        border: 1,
        style: {
            borderStyle: 'solid'
        }
    }, {
        xtype: 'text',
        columnWidth: 0.55,
        margin: '13 0 0 5',
        text: '平均间接成本:',
        id: 'InDirect_cost',
        name: 'InDirect_cost',
        border: 1,
        style: {
            borderStyle: 'solid'
        }
    }, {
        xtype: 'button',
        columnWidth: 0.05,
        margin: '10 0 0 5',
        text: '设置',
        id: 'InDirect_set',
        name: 'InDirect_set'
    }, {
        xtype: 'text',
        columnWidth: 0.55,
        margin: '10 0 0 5',
        text: '平均总成本:',
        id: 'Total_cost',
        name: 'Total_cost',
        border: 1,
        style: {
            borderStyle: 'solid'
        }
    }]
});