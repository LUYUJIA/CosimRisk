Ext.define('CosimRisk.store.ResourceNameStore', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.ResourceNameModel',
    storeId: 'ResourceNameStroe',
    proxy: {
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=30',
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});
