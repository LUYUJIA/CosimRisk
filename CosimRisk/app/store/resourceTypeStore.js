Ext.define('CosimRisk.store.resourceTypeStore', {
    extend: 'Ext.data.Store',
    storeId: 'resourceTypeStore',
    model: 'CosimRisk.model.resourceTypeModel',
    data: {
        resourceType: [
            { "resourceType": "消耗性资源" },
            { "resourceType": "非消耗性资源" }
        ]
    },
    proxy: {
        type: 'memory',
        reader: {
            type: 'json',
            root: 'resourceType'
        }
    }
});