Ext.define('CosimRisk.view.GetResult.TaskwaitTabPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.TaskwaitTabPanel',
    id: 'TaskwaitTabPanel',
    title: '任务资源延阻图',
    layout: 'border',
    closable: true,
    autoScroll: true,
    items: [{
        xtype: 'panel',
        layout: 'fit',
        region: 'east',
        width: 200,
        id: 'TaskTreePanel'
    }]
});