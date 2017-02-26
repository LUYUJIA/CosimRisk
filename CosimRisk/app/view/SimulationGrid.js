Ext.define('CosimRisk.view.SimulationGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.SimulationGrid',
    id: 'SimulationGrid',
    closable:true,
    title: '仿真任务信息',
    selModel: Ext.create( 'Ext.selection.CheckboxModel', {
        mode: "SINGlE",
        showHeaderCheckbox: false
    } ),
    store: 'SimulationStore',
    columns: [{
        xtype: 'rownumberer',
        width: 50,
        text: '序号'
    }, {
        text: '项目名称',
        dataIndex: 'projectName',
        flex: 1
    }, {
        text: '仿真任务名称',
        dataIndex: 'desciption',
        flex: 1
    }, {
        text: '仿真次数',
        dataIndex: 'count',
        flex: 0.5
    }, {
        text: '开始时间',
        dataIndex: 'simStarttime',
        flex: 1
    },
    {
        text: '结束时间',
        dataIndex: 'simEndtime',
        flex: 1
    },
    {
        text: '资源约束',
        dataIndex: 'have_resource',
        flex: 1
    }],
    buttons: [{
        xtype: 'button',
        text: '选择该任务查看',
        id: 'chooseSimBtn'
    },{
        xtype: 'button',
        text: '开始一次新仿真',
        id: 'newSimBtn'
    }, {
        xtype: 'button',
        text: '删除该任务',
        id: 'deleteSimBtn'
    }]
});