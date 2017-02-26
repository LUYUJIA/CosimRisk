Ext.define('CosimRisk.store.ProjectImage_Task_Store', {
    extend: 'Ext.data.Store',
    model: 'CosimRisk.model.ProjectImage_Task_Model',
    storeId: 'ProjectImage_Task_Store',
    proxy: 'memory',
    autoLoad:false
});