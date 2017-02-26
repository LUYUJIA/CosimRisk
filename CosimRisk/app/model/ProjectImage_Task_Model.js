Ext.define('CosimRisk.model.ProjectImage_Task_Model', {
    extend: 'Ext.data.Model',
    fields: [{
        name: 'Task_name',
        type: 'string'
    }, {
        name: 'ExpressionName',
        type: 'string'
    }, {
        name: 'Task_is_summary',
        type: 'string'
    }, {
        name: 'Value',
        type: 'int'
    }, {
        name: 'IsDone',
        type: 'string'
    }]
});