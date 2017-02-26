Ext.define('CosimRisk.store.expressionNameStore', {
    extend: 'Ext.data.Store',
    storeId: 'expressionNameStore',
    model: 'CosimRisk.model.expressionNameModel',
    data:{ 
        expressionName:[
            { "expressionName": "三角分布" },
            { "expressionName": "Beta分布" },
            { "expressionName": "正态分布" },
            { "expressionName": "固定" }
        ]},
    proxy: {
        type: 'memory',
        reader: {
            type: 'json',
            root: 'expressionName'
        }
    }
});
