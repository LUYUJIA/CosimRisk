Ext.define('CosimRisk.view.GetResult.barDataPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.barDataPanel',
    id: 'barDataPanel',
    title: '总工期区间统计',
    closable: true,
    autoScroll: true,
    dockedItems: [{
        xtype: 'toolbar',
        dock: 'top',
        items: [{
            xtype: 'combo',
            value: '频数直方图',
            editable:false,
            id:'ChangeAxis_combo',
            store: Ext.create('Ext.data.Store', {
                fields: ['name'],
                data : [
                    { "name": "频数直方图" },
                    { "name": "频率直方图" }
                ]
            }),
            fieldLabel: '切换Y轴',
            labelWidth: 60,
            displayField: 'name',
            valueField:'name'
        },{
            xtype: 'combo',
            value: '30',
            editable: false,
            id: 'ChangeScale_combo',
            store: Ext.create('Ext.data.Store', {
                fields: ['scale'],
                data: [
                    { "scale": "10" },
                    { "scale": "20" },
                    { "scale": "30" },
                    { "scale": "40" },
                    { "scale": "50" },
                    { "scale": "60" },
                    { "scale": "70" },
                    { "scale": "80" },
                    { "scale": "90" },
                    { "scale": "100" }

                ]
            }),
            fieldLabel: '区间数',
            labelWidth: 60,
            displayField: 'scale',
            valueField: 'scale'
        }]
    }]
} )