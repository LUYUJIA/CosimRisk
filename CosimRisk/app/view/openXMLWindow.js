Ext.define('CosimRisk.view.openXMLWindow', {
    extend : 'Ext.window.Window',
    alias : 'widget.openXMLWindow',
    title : '打开XML文件',
    height : 220,
    items : [{
        xtype : 'form',
        items : [{
            xtype : 'filefield',
            name : 'myXML',
            fieldLabel : 'XML文件',
            labelWidth : 60,
            msgTarget : 'side',
            allowBlank : false,
            buttonText : '选取XML文件',
            margin:'5 2 2 5'
        }, {
            xtype: 'textareafield',
            name: 'PRJ_DESCRIBE',
            fieldLabel: '项目描述',
            labelWidth: 60,
            anchor: '100%',
            allowBlack:false,
            margin: '5 2 2 5'
        }],
        buttons : [{
            text : '确认',
            margin:'10 8 0 auto'
        }]
    }]
});
