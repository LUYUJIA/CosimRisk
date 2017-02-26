Ext.define('CosimRisk.store.CriticalTreeStore', {
    extend: 'Ext.data.TreeStore',
    storeId: 'CriticalTreeStore',
    proxy: {
        reader: {
            type: 'json'
        },
        type: 'ajax',
        url: '/SeverRes/Handler.ashx?method=65'
    },
    //root: {
    //    expanded: true,
    //    children: [
    //        { text: "detention", leaf: true },
    //        { text: "homework", expanded: true, children: [
    //            { text: "book report", leaf: true },
    //            { text: "algebra", leaf: true}
    //        ] },
    //        { text: "buy lottery tickets", leaf: true }
    //    ]
    //}
    root: {
        text: '<font color="blue">概要任务</font>',//tree_Version_projectId
        expanded: true
    }
    /*listeners: {
        beforeload: function (store, operation) {
          
            var Version_projectId = tree_Version_projectId;
            if (!Version_projectId) {// 前端传递过来的参数，如果为空不加载  
                return false;
            }
            var new_params = {//参数  
                projectId: Version_projectId
            };
            Ext.apply(store.proxy.extraParams, new_params);// 通过<span style="background-color: #ffffff;">extraParams传递</span>  
        }
    },*/
});
