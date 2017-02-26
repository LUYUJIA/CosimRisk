Ext.define( 'CosimRisk.store.ProjectGridStore', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.ProjectGridModel',
    storeId: 'ProjectGridStore',
    proxy: {
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=2',
        reader: {
            type: 'json',
            root: 'data'
        }
    },
    autoLoad: true
} );
