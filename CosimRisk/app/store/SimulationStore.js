Ext.define('CosimRisk.store.SimulationStore', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.SimulationModel',
    storeId: 'SimulationStore',
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