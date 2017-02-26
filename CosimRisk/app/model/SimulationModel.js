Ext.define('CosimRisk.model.SimulationModel', {
    extend: 'Ext.data.Model',
    fields: [{
        name: 'priId',
        type: 'Int'
    },{
        name: 'simVersionId',
        type: 'Int'
    }, {
        name: 'projectName',
        type: 'string'
    }, {
        name: 'desciption',
        type: 'string'
    }, {
        name: 'count',
        type: 'Int'
    }, {
        name: 'simStarttime',
        type: 'string'
    }, {
        name: 'simEndtime',
        type: 'string'
    }, {
        name: 'have_resource',
        type: 'string'
    }]
});