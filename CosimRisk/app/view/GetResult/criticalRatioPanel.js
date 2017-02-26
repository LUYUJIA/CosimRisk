Ext.define( 'CosimRisk.view.GetResult.criticalRatioPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.criticalRatioPanel',
    id: 'criticalRatioPanel',
    title: '任务关键路径的概率',
    layout:'border',
    closable: true,
    autoScroll: true,
    items: [{
        xtype:'panel',
        layout: 'fit',
        region: 'east',
        width:200,
        id: 'CriticalTreePanel'
    }]
} );