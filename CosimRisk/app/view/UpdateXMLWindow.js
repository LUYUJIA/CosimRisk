Ext.define( 'CosimRisk.view.UpdateXMLWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.UpdateXMLWindow',
    title: '修改XML文件描述',
    height: 220,
    items: [{
        xtype: 'form',
        items: [{
            xtype: 'textfield',
            name: 'PRJ_NAME',
            fieldLabel: '项目名称',
            labelWidth: 60,
            readOnly: true
        }, {
            xtype: 'textareafield',
            name: 'PRJ_DESCRIBE',
            fieldLabel: '项目描述',
            labelWidth: 60,
            anchor: '100%',
            allowBlack: false,
            margin: '5 2 2 5'
        }],
        buttons: [{
            text: '确认',
            action:'submit',
            margin: '10 8 0 auto'
        }]
    }]
} );
