Ext.define('CosimRisk.store.ResourceWarehouseStore', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.ResourceWarehouseModel',
    storeId: 'ResourceWarehouseStore',
    proxy: {
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=27',
        reader: {
            type: 'json',
            root: 'data'
        }
    },
    autoLoad: true
});
