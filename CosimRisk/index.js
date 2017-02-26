
Ext.QuickTips.init();
Ext.Loader.setConfig({
	enabled : true
});
Ext.application({
	name : 'CosimRisk',
	appFolder : 'app',
	controllers: ['Main', 'TreePanelController', 'ResPanelController', 'TaskAssignmentController', 'ProjectAssignmentContoller']
});
