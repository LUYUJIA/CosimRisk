Ext.define('CosimRisk.store.ProjectResourceStore', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.ProjectResourceModel',
    storeId: 'ProjectResourceStore',
    proxy: {
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=6',
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});
