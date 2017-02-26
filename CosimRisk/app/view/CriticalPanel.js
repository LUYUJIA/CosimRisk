Ext.define('CosimRisk.view.CriticalPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.CriticalPanel',
    id: 'CriticalPanel',
    html: '<img src="/resources/images/surface.png" style="width:100%; height:100%;"/>',
    title: '项目关键路径图',
    closable: true,
    autoScroll: true,
    layout: 'absolute',
    bodyStyle: 'background:#E5E5E5;padding:0px',
    items: [{
        xtype: 'text',
        fieldLabel: '选择版本',
        //store: 'SimulationStore',
        //displayField: 'desciption',
        labelWidth: 60,
        x: 400,
        margin: '13 0 0 10',
        id: 'desciption_text'
    },{
        xtype: 'textfield',
        labelWidth: 50,
        width: 90,
        x:660,
        allowBlank: false,
        blankText: '不能为空',
        margin: '10 0 0 10',
        fieldLabel: '第几次',
        id: 'time',
        name: 'time'
       // regex: Reg,
       // regexText: '只能为整数'
    }, {
        xtype: 'text',
        text: '（总共 次）',
        x: 750,
        width: 90,
        height: 100,
        margin: '15 0 0 10',
        id: 'count_text'
    }, {
        xtype: 'button',
        text: '提交',
        x: 850,
        width: 70,
       // height: 100,
        margin: '10 0 0 0',
        id: 'CriticalsubmitBtn'
    }]
});