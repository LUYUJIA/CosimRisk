
Ext.define('CosimRisk.view.CriticalTree', {
    extend: 'Ext.tree.Panel',
    id: 'CriticalTree',
    alias: 'widget.CriticalTree',
    store: 'CriticalTreeStore',
    height: 380,
    autoScroll: true,
    frame:false,
    bodyStyle: 'background:#E5E5E5;padding:0px'
});
