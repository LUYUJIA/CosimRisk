
Ext.define('CosimRisk.view.TreePanel', {
	extend : 'Ext.tree.Panel',
	alias : 'widget.TreePanel',
	//title: '当前项目:',
	id: 'TreePanel',
	width : 240,
	height : 300,
//	padding: '5 3 3 10',
	bodyStyle: 'background:#E5E5E5;padding:0px',
    frame:true,
	//dockedItems : [{
	//	xtype : 'toolbar',
	//	dock : 'top',
	//	items : [{			
	//		xtype : 'button',
	//		id : 'allopen',
	//		text: '展开全部',
	//		handler: function () {
	//		    Ext.getCmp('TreePanel').expandAll();
	//		}
	//	 },{
	//		xtype : 'button',
	//		id : 'allclose',
	//		text: '收起全部',
	//		handler: function () {
	//		    Ext.getCmp('TreePanel').collapseAll();
	//		}
	//	 }]
	//}],
	root : {
	    text: "系统",
	    bodyStyle: 'background:#E5E5E5;padding:0px',
	    id:'system',
	    expanded: true,
	    leaf: false,
	    iconCls: 'icon-system',
		children : [{
		    text: '项目管理',
		    iconCls: 'icon-management',
			leaf: true,
			id : '01'
			//children : []
		}, {
		    text: '总资源管理',
		    iconCls: 'icon-warehouse',
		    leaf: true,
		    id: '1.5'
		    //children : []
		}, {
		    text: '项目网络图',
		    iconCls: 'icon-netgraph',
			id : '02',
			leaf: true
			//children : []
		}, {
		    text: '项目进度仿真',
		    iconCls: 'icon-version',
			id : '03',
			leaf: true
			//children : []
		}, {
		    text: '仿真结果',
		    iconCls: 'icon-getresults',
		    id: '04',
		    expanded: false,
		    expandable: true,
			children: [{
			    text: '查看某次关键路径图',
			    iconCls: 'icon-critical',
			    id: '0401',
			    leaf: true
			   },
                {
                    text: '平均工期直方图',
                    iconCls: 'icon-histogram',
                    id: '0402',
                    leaf: true
                },
                 {
                    text: '任务关键路径的概率',
                    iconCls: 'icon-Probability',
                    id: '0403',
                    leaf: true
                },
                {
                    text: '总工期区间统计',
                    iconCls: 'icon-Statistics',
                    id: '0404',
                    leaf: true
                }, 
                {
                    text: '进度风险',
                    iconCls: 'icon-Risk',
                    id: '0405',
                    leaf: true
                },
                 {
                    text: '最大概率关键路径',
                    iconCls: 'icon-road',
                    id: '0406',
                    leaf: true
                 },
                 {
                     text: '任务资源延阻图',
                       iconCls: 'icon-Resource',
                     id: '0407',
                     leaf: true
                 },
                 {
                     text: '资源用量分析图',
                     iconCls: 'icon-Resource',
                     id: '0408',
                     leaf: true
                 },
                 {
                     text: '成本分析图',
                      iconCls: 'icon-Resource',
                     id: '0409',
                     leaf: true
                 }
                 //,
                 //{
                //     text: '资源成本联合分险',
                //     iconCls: 'icon-Resource',
                //     id: '0410',
               //      leaf: true
               //  }
			]
		}, {
		    text: '帮助',
		    iconCls: 'icon-help',
			id : '05',
			leaf: true
			//children : []
		}]
	}
});
