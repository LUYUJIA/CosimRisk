Ext.define('CosimRisk.store.taskPriorityStore', {
    extend: 'Ext.data.Store',
    storeId: 'taskPriorityStore',
    model: 'CosimRisk.model.taskPriorityModel',
    data: {
        taskPriority: [
            { "taskPriority": "1级", "taskPr": 1 },
            { "taskPriority": "2级", "taskPr": 2 },
            { "taskPriority": "3级", "taskPr": 3 },
            { "taskPriority": "4级", "taskPr": 4 },
            { "taskPriority": "5级", "taskPr": 5 },
            { "taskPriority": "6级", "taskPr": 6 },
            { "taskPriority": "7级", "taskPr": 7 }
        ]
    },
    proxy: {
        type: 'memory',
        reader: {
            type: 'json',
            root: 'taskPriority'
        }
    }
});
