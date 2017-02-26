Ext.define('CosimRisk.store.CriticalStore', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.CriticalModel',
    storeId: 'CriticalStore',
    proxy: {
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=42',
        reader: {
            type: 'json',
            root: 'data'
        }
    },
    autoLoad: true
});