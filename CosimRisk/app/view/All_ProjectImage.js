var navigate = function (panel, direction) {
    var layout = panel.getLayout();
    layout[direction]();
    Ext.getCmp('move-prev').setDisabled(!layout.getPrev());
    Ext.getCmp('move-next').setDisabled(!layout.getNext());
};

Ext.define('CosimRisk.view.All_ProjectImage', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.All_ProjectImage',
    layout: 'card',
    closable:true,
    id: 'All_ProjectImage',
    defaults: {
        border: false
    },
    bbar: [
        {
            id: 'move-prev',
            text: '项目网络图',
            handler: function (btn) {
                navigate(btn.up("panel"), "prev");
            },
            disabled: true
        },
         '->', // greedy spacer so that the buttons are aligned to each side
        {
            id: 'move-next',
            text: '项目任务节点列表',
            handler: function (btn) {
                navigate(btn.up("panel"), "next");
            }
        }
    ],
    items: [{
        xtype: 'ProjectImage',
        id: 'ProjectImage'
    }, {
        xtype: 'ProjectImage_Task',
        id: 'ProjectImage_Task',
        store: 'ProjectImage_Task_Store'
    }]
});