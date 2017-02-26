Ext.define('CosimRisk.store.TaskTreeStore', {
    extend: 'Ext.data.TreeStore',
    storeId: 'TaskTreeStore',
    proxy: {
        reader: {
            type: 'json'
        },
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=72'
    },
    root: {
        text: '需要等待资源的任务',//tree_Version_projectId
        expanded: true
    }
});
