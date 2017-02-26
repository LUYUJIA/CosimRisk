Ext.define('CosimRisk.model.ProjectGridModel', {
	extend : 'Ext.data.Model',
	fields : [{
		name : 'PRJ_ID',
		type : 'int'
	}, {
		name : 'PRJ_NAME',
		type : 'string'
	}, {
		name : 'PRJ_DESCRIBE',
		type : 'string'
	}, {
		name : 'PRJ_DATE',
		type : 'string'
	}, {
	    name: 'PRJ_XML',
	    type: 'string'
	}]
}); 