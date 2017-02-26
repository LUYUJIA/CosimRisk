Ext.define('CosimRisk.model.ResourceWarehouseModel', {
    extend: 'Ext.data.Model',
    fields: [{
        name: 'Resource_Name',
        type: 'string'
    }, {
        name: 'Resource_Type',
        type: 'string'
    }, {
        name: 'Resource_Mount',
        type: 'int'
    }, {
        name: 'Resource_Remains',
        type: 'int'
    }, {
        name: 'Resource_Price',
        type: 'string'
    }, {
        name: 'Resource_Description',
        type: 'string'
    }, {
        name: 'Auto_id',
        type: 'int'
    }]
});