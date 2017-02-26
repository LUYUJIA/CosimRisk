Ext.define('CosimRisk.view.SimulationWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.SimulationWindow',
    id: 'SimulationWindow',
    title: '项目仿真',
    modal: true,
    height: 250,
    width:  380,
    layout: 'fit',
    resizable:false,
    items: [{
        xtype: 'form',
        id:'SimulationForm',
        border: false,
        items: [{
            xtype: 'textfield',
            id: 'SIM_TIMES',
            name: 'times',
            fieldLabel: '仿真次数',
            anchor: '60%',
            allowBlack: false,
            blankText: '不能为空',
            margin: '10 0 5 8'
        },{
            xtype: 'textareafield',
            id: 'SIM_DESCRIPTION',
            name: 'description',
            fieldLabel: '版本名称',
            anchor: '90%',
            allowBlack: false,
            margin: '10 0 0 8'
        }, {
            xtype      : 'fieldcontainer',
            fieldLabel : '是否带资源',
            defaultType: 'radiofield',
            allowBlank: false,
            margin: '0 0 0 8',
            defaults: {
                flex: 1
            },
            layout: 'hbox',
            items: [
                {
                    boxLabel  : '带资源的仿真',
                    name: 'resource_radio',
                    inputValue: '1',
                    id        : 'resource_radio1'
                }, {
                    boxLabel  : '不带资源的仿真',
                    name: 'resource_radio',
                    inputValue: '0',
                    id        : 'resource_radio2'
                }]
        }],
        buttons: [{
            text: '开始仿真',
            scale: 'medium',
            id:'StartSimBtn'
        },{
            text: '关闭',
            scale: 'small',
            listeners:
                    {
                        click: function (e, eOpts)
                        {
                            e.ownerCt.ownerCt.ownerCt.close();
                        }
                    }
        }]
    }]
});
