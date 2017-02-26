Ext.define('CosimRisk.view.MaxCriticalPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.MaxCriticalPanel',
    id: 'MaxCriticalPanel',
    html: '<img src="/resources/images/surface.png" style="width:100%; height:100%;"/>',
    title: '工程最大概率关键链',
    closable: true,
    autoScroll: true,
    layout: 'absolute',
    bodyStyle: 'background:#E5E5E5;padding:0px',
    items: [{
        xtype: 'text',
        x: 750,
        margin: '15 0 0 10',
        text: '平均工期:',
        id: 'maxProjectTime'
     }]
});