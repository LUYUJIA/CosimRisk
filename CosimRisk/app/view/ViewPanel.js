Ext.define( 'CosimRisk.view.ViewPanel', {
    extend: 'Ext.container.Viewport',
    layout: 'border',
    id:'ViewPanel',
    defaults: {
       split: false,                  
        collapsible: true,           
        bodyStyle: 'padding:15px'
    },
    items: [{
        region: 'north',            
        xtype: "panel",
        bodyCls: 'logo-background',
        height: 110,
        split: false,
        collapsible: false,
        layout:'absolute',
        items: [{
            xtype: 'label',
            text: '仿真项目:',
            id: 'project_label',
            cls: 'label-class',
            y: 85,
            x:280
        }, {
            xtype: 'label',
            text: '仿真任务:',
            id: 'version_label',
            cls: 'label-class',
            y: 85,
            x: 530
        }]
    }, {
        region: 'west',
        xtype: "TreePanel",
        bodyStyle: 'background:#E5E5E5;padding:0px'
    }, {
        region: 'center',
        xtype: "TabPanel",
        collapsible: false,
        closable: false,
        bodyStyle: 'background:#E5E5E5;padding:0px'
    }, {
        region: 'south',
        header: false,
        id:'SouthPanel',
        xtype: "panel",
        html: "Copyright ©2013 COSIM <a href='/index.html'>使用必读</a> ",
        bodyStyle: 'background:#E5E5E5;padding:0px',
        height: 30
    }]
} );