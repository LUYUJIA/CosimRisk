Ext.define('CosimRisk.store.TaskResourceStore', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.TaskResourceModel',
    storeId: 'TaskResourceStore',
    proxy: {
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=28',
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});
