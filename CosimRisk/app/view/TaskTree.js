Ext.define('CosimRisk.view.TaskTree', {
    extend: 'Ext.tree.Panel',
    id: 'TaskTree',
    alias: 'widget.TaskTree',
    store: 'TaskTreeStore',
    height: 380,
    autoScroll: true,
    frame: false,
    bodyStyle: 'background:#E5E5E5;padding:0px'
});