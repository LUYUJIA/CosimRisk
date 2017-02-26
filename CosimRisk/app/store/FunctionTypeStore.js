Ext.define('CosimRisk.store.FunctionTypeStore', {
    extend: 'Ext.data.Store',
    storeId: 'FunctionTypeStore',
    model: 'CosimRisk.model.FunctionTypeModel',
    data: {
        FunctionType: [
            { "FunctionType": "A(X)+B" }
        ]
    },
    proxy: {
        type: 'memory',
        reader: {
            type: 'json',
            root: 'FunctionType'
        }
    }
});