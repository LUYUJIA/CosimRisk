Ext.define('CosimRisk.store.SimulationcomboStore', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.ProjectGridModel',
    storeId: 'SimulationcomboStore',
    proxy: {
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=2',
        reader: {
            type: 'json',
            root: 'data'
        }
    },
    autoLoad: true,
    listeners: {
        'load': function (store, operation, eOpts) {
                store.insert(0,{ PRJ_ID: -1, PRJ_NAME: '全部' });
        }
    }
});
