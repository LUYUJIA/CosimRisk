
Ext.define('CosimRisk.view.XmlTree', {
    extend: 'Ext.tree.Panel',
    id: 'XmlTree',
    alias: 'widget.XmlTree',
    store: 'CriticalTreeStore',
    height: 300,
    autoScroll: true,
    frame: false,
    bodyStyle: 'background:#E5E5E5;padding:0px'
});
