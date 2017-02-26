Ext.define('CosimRisk.view.IndirectWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.IndirectWindow',
    title: '设置间接成本',
    height: 180,
    width:300,
    layout: 'column',
        items: [{
            xtype: 'combo',
            columnWidth: 0.60,
            labelWidth: 60,
            border: false,
            anchor: '90%',
            id: 'Function_Type',
            editable: false,
            allowBlank: false,
            blankText: '不能为空',
            margin: '10 0 0 5',
            fieldLabel: '函数类型',
            displayField: 'FunctionType',
            value:"A(X)+B",
            store: Ext.create('CosimRisk.store.FunctionTypeStore')
        }, {
            xtype: 'textfield',
            columnWidth: 0.45,
            labelWidth: 40,
            allowBlank: false,
            margin: '10 0 0 10',
            fieldLabel: 'A参数',
            value: '0',
            id: 'Function_A',
            name: 'Function_A',
            regex: Reg,
            regexText: '只能为整数或者小数'
        }, {
            xtype: 'textfield',
            columnWidth: 0.45,
            labelWidth: 40,
            allowBlank: false,
            blankText: '不能为空',
            margin: '10 0 0 10',
            fieldLabel: 'B参数',
            value: '0',
            id: 'Function_B',
            name: 'Function_B',
            regex: Reg,
            regexText: '只能为整数或者小数'
        }, {
            xtype: 'button',
            text: '确认',
            columnWidth: 0.40,
            id: 'indirect_confirm',
            margin: '15 0 0 40'
        }, {
            xtype: 'button',
            text: '取消',
            columnWidth: 0.45,
            id: 'indirect_cancle',
            margin: '15 25 0 30',
            listeners:
                   {
                       click: function (e, eOpts) {
                           e.ownerCt.close();
                       }
                   }
        }]

});
