Ext.define('CosimRisk.view.ResourceUse', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.ResourceUse',
    id: 'ResourceUse',
    title: '资源用量图',
    closable: true,
    autoScroll: true,
    dockedItems: [{
        xtype: 'toolbar',
        dock: 'top',
        items: [{
            xtype: 'combo',
            editable: false,
            id: 'Resourcename_combo',
            store: 'ResourceNameStore',
            fieldLabel: '资源名:',
            labelWidth: 60,
            displayField: 'Resource_Name',
            valueField: 'Resource_Id'
        }]
    }]
})