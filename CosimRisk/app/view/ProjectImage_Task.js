Ext.define('CosimRisk.view.ProjectImage_Task', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.ProjectImage_Task',
    id: 'ProjectImage_Task',
    title: '项目任务节点列表',
    store: 'ProjectImage_Task_Store',
    columns: [{
        xtype: 'rownumberer',
        width: 50,
        text: '序号'
    }, {
        text: '任务名称',
        dataIndex: 'Task_name',
        flex: 1.5
    }, {
        text: '分布类型',
        dataIndex: 'ExpressionName',
        flex: 1
    },
    {
        text: '是否是概要任务',
        dataIndex: 'Task_is_summary',
        flex: 1
    }, {
        text: '实际工期',
        flex: 1,
        dataIndex: 'Value'
    }, {
        text: '是否设置完成',
        flex: 1,
        dataIndex: 'IsDone'
    }]
});