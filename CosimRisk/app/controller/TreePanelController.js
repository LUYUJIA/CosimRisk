var projectId;//项目id
var Version_projectId;//版本项目id
var projectName;//项目名字
var simVersionId = null;//版本id
var count;//版本最大次数
var desciption;//版本名称
var myData_new;
var myData_old;
var MaxFrequency = 0;
var old_treercord;
var old_text;
var click_ok = null;
var direct_cost = 0;
var project_value = 0;
var indirect_A = 0;
var indirect_B = 0;
var indirect_cost = 0;

Ext.define( 'CosimRisk.controller.TreePanelController', {
    extend: 'Ext.app.Controller',
    views: ['CosimRisk.view.openXMLWindow',
                'CosimRisk.view.XmlTree',
                'CosimRisk.view.TabPanel',
                'CosimRisk.view.ProjectGrid',
                'CosimRisk.view.ProjectImage',
                'CosimRisk.view.TreePanel',
                'CosimRisk.view.showXMLWindow',
                'CosimRisk.view.UpdateXMLWindow',
                'CosimRisk.view.SimulationWindow',
                'CosimRisk.view.IndirectWindow',
                'CosimRisk.view.SimulationGrid',
                'CosimRisk.view.ResourceGrid',
                'CosimRisk.view.ResourceAddedPanel',
                'CosimRisk.view.ResourceWarehousePanel',
                'CosimRisk.view.CriticalTree',
                'CosimRisk.view.TaskTree',
                'CosimRisk.view.CriticalPanel',
                'CosimRisk.view.ProjectImage_Task',
                'CosimRisk.view.GetResult.barDataPanel',
                'CosimRisk.view.All_ProjectImage',
                'CosimRisk.view.GetResult.averageProjectBarPanel',
                'CosimRisk.view.GetResult.RiskPanel',
                'CosimRisk.view.GetResult.criticalRatioPanel',
                'CosimRisk.view.CostPanel',
                'CosimRisk.view.ResourceUse',
                'CosimRisk.view.CostAddedPanel',
                'CosimRisk.view.GetResult.TaskwaitTabPanel'],
    models: ['CosimRisk.model.ProjectGridModel', 'CosimRisk.model.SimulationModel',
         'ProjectImage_Task_Model', 'resourceTypeModel', 'ResourceWarehouseModel',
        'ProjectResourceModel', 'ResourceNameModel', 'FunctionTypeModel'],
    stores: ['ProjectGridStore', 'SimulationStore', 'CriticalTreeStore',
        'SimulationcomboStore', 'ProjectImage_Task_Store', 'resourceTypeStore', 'ResourceWarehouseStore',
        'ProjectResourceStore', 'ResourceNameStore', 'TaskTreeStore', 'FunctionTypeStore'],
    init: function () {

        this.control( {
            'ProjectGrid button[id=openXMLBtn]': {
                click: function () {
                    Ext.widget( 'openXMLWindow' ).show();
                }
            },
            'ProjectGrid button[id=chooseAsProjectBtn]': {
                click: function ( o ) {
                    var gird = o.ownerCt.ownerCt;
                    var data = gird.getSelectionModel().getSelection();
                    if ( data.length == 0 ) {
                        Ext.Msg.alert( "提示", "您要选择一个项目" );
                    } else {
                        projectId = data[0].get( "PRJ_ID" );
                        Ext.Msg.alert( "提示", "选择项目:" + data[0].get( "PRJ_NAME" ) );
                        Ext.getCmp("project_label").setText('仿真项目:' + data[0].get("PRJ_NAME"));
                        projectName = data[0].get( "PRJ_NAME" );
                        var tabpanel = Ext.getCmp( 'TabPanel' );
                        var tab = tabpanel.getComponent('ProjectImage');
                        var sim_tab = tabpanel.getComponent('SimulationGrid');
                        if (sim_tab != undefined)
                            tabpanel.remove(sim_tab);
                        if ( tab != undefined ) {
                            for ( var i = tabpanel.items.length - 1; i >= 0; i-- ) {
                                if ( tabpanel.items.get( i ).id != 'ProjectGrid' )
                                    tabpanel.remove( tabpanel.items.get( i ) );
                            }

                        }
                    }
                }
            },
            'ProjectGrid button[id=modifyProjectBtn]': {
                click: function ( o ) {
                    var gird = o.ownerCt.ownerCt;
                    var data = gird.getSelectionModel().getSelection();
                    if ( data.length == 0 ) {
                        Ext.Msg.alert( "提示", "您要选择一个项目" );
                    } else if ( data.length > 1 ) {
                        Ext.Msg.alert( "提示", "每次只能选择一个项目" );
                    } else {
                        var XMLWindow = Ext.widget( 'UpdateXMLWindow' );
                        var form = XMLWindow.down( 'form' );
                        var values = data['0'];
                        form.loadRecord( values );
                        XMLWindow.show();
                    }
                }
            },
            'ProjectGrid button[id=deleteProjectBtn]': {
                click: function ( o ) {
                    var gird = o.ownerCt.ownerCt;
                    var data = gird.getSelectionModel().getSelection();
                    var deleteParams = [];
                    Ext.Array.each( data, function ( name, index ) {
                        deleteParams.push( name.data.PRJ_NAME );
                    } );
                    if ( data.length == 0 ) {
                        Ext.Msg.alert( "提示", "您要选择一个项目" );
                    } else {
                        Ext.Ajax.request( {
                            url: '/SeverRes/Handler.ashx?method=3',
                            params: {
                                data: deleteParams
                            },
                            method: 'post',
                            success: function ( resp, opts ) {
                                Ext.Msg.alert( "提示", "成功删除!" );
                                Ext.getCmp( 'ProjectGrid' ).getStore().load();
                            },
                            failure: function ( resp, opts ) {
                                var respText = Ext.util.JSON.decode( resp.responseText );
                                Ext.Msg.alert( '错误', respText.error );
                            }

                        } );
                    }
                }
            },
            'ProjectGrid': {
                cellclick: function ( grid, td, cellIndex, record, tr, rowIndex, e, eOpts ) {
                    if ( cellIndex == 5 ) {
                        var project_Id = record.get("PRJ_ID");
                        var win = Ext.create('Ext.window.Window', {
                            title: 'XML解析结果',
                            height: 300,
                            width: 400,
                            layout: 'fit',
                            items: {
                                xtype: 'XmlTree',
                                id: 'XmlTree'
                            }
                        });
                        var store = Ext.getCmp('XmlTree').getStore();
                        store.load({ params: { projectId: project_Id } });
                        win.show();
                    }
                }
            },
            'TreePanel': {
                itemclick: function ( tree, record, item, index, e, options ) {
                    if ( record.data.leaf == true ) {
                        if ( record.data.id == '01' ) {
                            var tabpanel = Ext.getCmp( 'TabPanel' );
                            var tab = tabpanel.getComponent( 'ProjectGrid' );
                            if ( tab == undefined ) {
                                var grid = Ext.create( 'CosimRisk.view.ProjectGrid' );
                                tabpanel.add( grid )
                                tabpanel.setActiveTab( grid );
                            }
                            else
                                tabpanel.setActiveTab( 'ProjectGrid' );
                        }
                        else if (record.data.id == '1.5') {
                            var tabpanel = Ext.getCmp('TabPanel');
                            var tab = tabpanel.getComponent('ResourceWarehousePanel');
                            if (tab == undefined) {
                                var grid = Ext.create('CosimRisk.view.ResourceWarehousePanel');
                                tabpanel.add(grid)
                                tabpanel.setActiveTab(grid);
                            }
                            else
                                tabpanel.setActiveTab('ResourceWarehousePanel');
                        }
                        else if ( record.data.id == '02' ) {
                            if ( projectId != null ) {
                                var tabpanel = Ext.getCmp( 'TabPanel' );
                                var tab = tabpanel.getComponent('All_ProjectImage');
                                if (tab == undefined) {
                                    var Imagepanel = Ext.create('CosimRisk.view.All_ProjectImage');
                                    Imagepanel.setTitle( projectName );
                                    //定义组件，所有图案都添加其中
                                    var drawComponent = Ext.create( 'Ext.draw.Component', {
                                        viewBox: false,
                                        height: 500,
                                        width: 600,
                                        renderTo: document.body
                                    } ),
                                    surface = drawComponent.surface;//定义画板
                                    receive_and_draw( drawComponent, surface, 0, projectId, -1 );//接收数据并画图,主函数
                                    Imagepanel.getComponent('ProjectImage').add(drawComponent);//添加组件
                                    tabpanel.add( Imagepanel );
                                    tabpanel.setActiveTab( Imagepanel );
                                }
                                else {
                                    tabpanel.setActiveTab(tab);
                                }


                            }
                            else {
                                Ext.Msg.alert( '提示', "请您先选择项目" );
                            }
                        }
                        else if (record.data.id == '03') {
                            if (projectId != null) {
                                var tabpanel = Ext.getCmp('TabPanel');
                                var tab = tabpanel.getComponent('SimulationGrid');
                                if (tab == undefined) {
                                    var grid = Ext.create('CosimRisk.view.SimulationGrid');
                                    var store = Ext.getCmp('SimulationGrid').getStore();
                                    store.clearFilter();
                                    var filter = function (record, id) {
                                        if (record.get("projectName") == projectName)
                                            return true;
                                        else
                                            return false;
                                    };
                                    store.addFilter(filter);
                                    tabpanel.add(grid);
                                    tabpanel.setActiveTab(grid);
                                }
                                else {
                                    var store = Ext.getCmp('SimulationGrid').getStore();
                                    store.clearFilter();
                                    var filter = function (record, id) {
                                        if (record.get("projectName") == projectName)
                                            return true;
                                        else
                                            return false;
                                    };
                                    store.addFilter(filter);
                                    tabpanel.setActiveTab('SimulationGrid');
                                }
                            }
                            else {
                                Ext.Msg.alert('提示', "请您先选择项目");
                            }
                        }
                        else if (record.data.id == '0401' && click_ok !=null) {
                            var tabpanel = Ext.getCmp( 'TabPanel' );
                            var tab = tabpanel.getComponent( 'CriticalPanel' );
                            if ( tab == undefined ) {
                                var grid = Ext.create( 'CosimRisk.view.CriticalPanel' );
                                tabpanel.add(grid);
                                grid.items.items[0].setText( "当前任务:" + desciption );
                                grid.items.items[2].setText( '（1～' + count + '）' );
                                tabpanel.setActiveTab( grid );
                            }
                            else
                                tabpanel.setActiveTab( 'CriticalPanel' );
                        } else if (record.data.id == '0402' && click_ok != null) {

                            var tabpanel = Ext.getCmp( 'TabPanel' );
                            var tab = tabpanel.getComponent( 'averageProjectBarPanel' );
                            if ( tab == undefined ) {
                                var myData = [];
                                Ext.Ajax.request( {
                                    url: '/SeverRes/Handler.ashx?method=62&simVersionId=' + simVersionId + '&taskId=-1',
                                    async: false,
                                    success: function ( response ) {
                                        var res = Ext.JSON.decode( response.responseText );
                                        myData = res.task;
                                    },
                                    failure: function ( response ) {
                                        Ext.Msg.show( {
                                            title: '错误',
                                            msg: '未知错误',
                                            width: 300,
                                            buttons: Ext.Msg.OK,
                                            icon: Ext.Msg.ERROR
                                        } );
                                        return;
                                    }
                                } );
                                var chartStore = Ext.create( 'Ext.data.Store', {
                                    fields: ['Task_name', 'AverageProjectTime'],
                                    autoLoad: true,
                                    proxy: 'memory',
                                    data: myData
                                } )
                                var myChart = Ext.create( 'Ext.chart.Chart', {
                                    animate: true,
                                    store: chartStore,
                                    width: document.body.clientWidth * 0.7,
                                    height: document.body.clientHeight * 0.7,
                                    //			theme: 'White',
                                    axes: [{
                                        type: 'Category',
                                        position: 'bottom',
                                        fields: ['Task_name'],
                                        title: '任务名称',
                                        grid: true
                                    }, {
                                        type: 'Numeric',
                                        position: 'left',
                                        fields: ['AverageProjectTime'],
                                        title: '平均工期',
                                        minimum: 0,
                                        grid: true
                                    }],
                                    series: [
                                        {
                                            xField: 'Task_name',
                                            yField: 'AverageProjectTime',
                                            type: 'column',
                                            axis: 'left',
                                            highlight: true,
                                            tips: {
                                                trackMouse: true,
                                                width: 140,
                                                height: 40,
                                                renderer: function ( storeItem, item ) {
                                                    this.setTitle( storeItem.get( 'Task_name' ) + '<br>' + (storeItem.get( 'AverageProjectTime' )).toFixed(2) + ' 天' );
                                                }
                                            },
                                            label: {
                                                display: 'insideEnd',
                                                field: 'number',
                                                renderer: Ext.util.Format.numberRenderer( '0' ),
                                                orientation: 'horizontal',
                                                color: '#333',
                                                'text-anchor': 'middle'
                                            }
                                        },
                                    {
                                        xField: 'Task_name',
                                        yField: 'AverageProjectTime',
                                        type: 'line',
                                        markerConfig: {
                                            type: 'cross',
                                            size: 4,
                                            radius: 4,
                                            'stroke-width': 0,
                                            'fill': 'red'
                                        }
                                    }]
                                } );


                                var grid = Ext.widget( 'averageProjectBarPanel' );
                                grid.items.add( myChart );
                                tabpanel.add( grid )
                                tabpanel.setActiveTab( grid );
                            }
                            else {
                                tabpanel.setActiveTab( 'averageProjectBarPanel' );
                            }

                        } else if (record.data.id == '0403' && click_ok != null) {

                            var tabpanel = Ext.getCmp( 'TabPanel' );
                            var tab = tabpanel.getComponent( 'criticalRatioPanel' );
                            if ( tab == undefined ) {
                                var chart=Project_probability(-1);
                                var grid = Ext.widget('criticalRatioPanel');
                                var panel = Ext.create('Ext.panel.Panel', {
                                    title: '概要任务',
                                    id: 'CriticalChartPanel',
                                    width: 835,
                                    autoScroll: true,
                                    region:'west'
                                });
                                var CriticalTreePanel = grid.getComponent('CriticalTreePanel');
                                var tree = Ext.create('CosimRisk.view.CriticalTree');
                                tree.getStore().load({ params: { projectId: Version_projectId } });
                                CriticalTreePanel.add(tree);
                                panel.add(chart);
                                grid.add(panel);
                                tabpanel.add( grid )
                                tabpanel.setActiveTab( grid );
                            }
                            else {
                                tabpanel.setActiveTab( 'criticalRatioPanel' );
                            }
                        } else if (record.data.id == '0404' && click_ok != null) {
                            var tabpanel = Ext.getCmp( 'TabPanel' );
                            var tab = tabpanel.getComponent( 'barDataPanel' );
                            if (tab == undefined) {
                                Ext.Ajax.request({
                                    url: '/SeverRes/Handler.ashx?method=64&simVersionId=' + simVersionId + '&scale=' + 30,
                                    async: false,
                                    success: function (response) {
                                        var res = Ext.JSON.decode(response.responseText);
                                        myData_old = res.barData;
                                        myData_new = res.barData;
                                    },
                                    failure: function (response) {
                                        Ext.Msg.show({
                                            title: '错误',
                                            msg: '未知错误',
                                            width: 300,
                                            buttons: Ext.Msg.OK,
                                            icon: Ext.Msg.ERROR
                                        });
                                        return;
                                    }
                                });
                                var Total_chartStore = Ext.create('Ext.data.Store', {
                                    fields: ['Range', 'ScNum'],
                                    autoLoad: true,
                                    proxy: 'memory',
                                    data: myData_old
                                });
                                var grid = Ext.widget( 'barDataPanel' );
                                grid.items.removeAll();
                                var myChart = Frequence_histogram(Total_chartStore);
                                grid.items.add( myChart );
                                tabpanel.add( grid )
                                tabpanel.setActiveTab( grid );
                            }
                            else {
                                tabpanel.setActiveTab( 'barDataPanel' );
                            }
                        }
                        else if (record.data.id == '0405' && click_ok != null)
                        {
                            var tabpanel = Ext.getCmp( 'TabPanel' );
                            var tab = tabpanel.getComponent('RiskPanel');
                            if (tab == undefined)
                            {
                                var myData = [];
                                Ext.Ajax.request({
                                    url: '/SeverRes/Handler.ashx?method=66&simVersionId=' + simVersionId + '&taskId=-1',
                                    async: false,
                                    success: function (response) {
                                        var res = Ext.JSON.decode(response.responseText);
                                        myData = res.barData;
                                    },
                                    failure: function (response) {
                                        Ext.Msg.show({
                                            title: '错误',
                                            msg: '未知错误',
                                            width: 300,
                                            buttons: Ext.Msg.OK,
                                            icon: Ext.Msg.ERROR
                                        });
                                        return;
                                    }
                                });
                                var chartStore = Ext.create('Ext.data.Store', {
                                    fields: ['Range', 'ScNum', 'Ratio'],
                                    autoLoad: true,
                                    proxy: 'memory',
                                    data: myData
                                })
                                var myChart = Ext.create('Ext.chart.Chart', {
                                    animate: true,
                                    store: chartStore,
                                    width: document.body.clientWidth * 0.7,
                                    height: document.body.clientHeight * 0.7,
                                    //			theme: 'White',
                                    axes: [{
                                        type: 'Category',
                                        position: 'bottom',
                                        fields: ['Range'],
                                        title: '基准(天)',
                                        grid: true
                                    }, {
                                        type: 'Numeric',
                                        position: 'left',
                                        fields: ['Ratio'],
                                        title: '概率',
                                        minimum: 0,
                                        maximum:1,
                                        grid: true
                                    }],
                                    series: [{
                                        type: 'area',
                                        axis: 'left',
                                        highlight: true,
                                        xField: 'Range',
                                        yField: 'Ratio',
                                        tips: {
                                            trackMouse: true,
                                            width: 140,
                                            height: 40,
                                            renderer: function (storeItem, item) {
                                                this.setTitle(storeItem.get('Range') + '<br> ' + (storeItem.get('Ratio') * 100).toFixed(2) + '%');
                                            }
                                        },
                                        style: {
                                            lineWidth: 1,
                                            stroke: '#666',
                                            opacity: 0.86
                                        }
                                    }]
                                });
                                var grid = Ext.widget('RiskPanel');
                                grid.add(myChart);
                                tabpanel.add(grid)
                                tabpanel.setActiveTab(grid);
                            }
                            else
                            {
                                tabpanel.setActiveTab('RiskPanel');
                            }
                        }
                        else if (record.data.id == '0406' && click_ok != null)
                        {
                            var tabpanel = Ext.getCmp('TabPanel');
                            var tab = tabpanel.getComponent('MaxCriticalPanel');
                            if (tab == undefined) {
                                var gri = Ext.create('CosimRisk.view.MaxCriticalPanel');
                                var drawComponent = Ext.create('Ext.draw.Component', {
                                    viewBox: false,
                                    height: 500,
                                    width: 600,
                                    renderTo: document.body
                                }),
                                surface = drawComponent.surface;//定义画板
                                receive_and_drawCritical(67,drawComponent, surface, 0, Version_projectId, -1, simVersionId, 1);//接收数据并画图
                                gri.add(drawComponent);//添加组件
                                tabpanel.add(gri);
                                tabpanel.setActiveTab(gri);
                            }
                            else
                                tabpanel.setActiveTab('MaxCriticalPanel');
                        }
                        else if (record.data.id == '0407' && click_ok == 2) {

                            var tabpanel = Ext.getCmp('TabPanel');
                            var tab = tabpanel.getComponent('TaskwaitTabPanel');
                            if (tab == undefined) {
                                var chart = Task_Wait(-1);
                                var grid = Ext.widget('TaskwaitTabPanel');
                                var panel = Ext.create('Ext.panel.Panel', {
                                    title: '需要等待资源的任务',
                                    id: 'TaskwaitPanel',
                                    width: 835,
                                    autoScroll: true,
                                    region: 'west'
                                });
                                var TaskTreePanel = grid.getComponent('TaskTreePanel');
                                var tree = Ext.create('CosimRisk.view.TaskTree');
                                tree.getStore().load({ params: { VersionId: simVersionId } });
                                TaskTreePanel.add(tree);
                                panel.add(chart);
                                grid.add(panel);
                                tabpanel.add(grid)
                                tabpanel.setActiveTab(grid);
                            }
                            else {
                                tabpanel.setActiveTab('TaskwaitTabPanel');
                            }
                        }
                        else if (record.data.id == '0408' && click_ok == 2) {

                            var tabpanel = Ext.getCmp('TabPanel');
                            var tab = tabpanel.getComponent('ResourceUse');
                            if (tab == undefined) {
                                var grid = Ext.widget('ResourceUse');
                                var store = Ext.getCmp('Resourcename_combo').getStore();
                                store.load({ params: { projectId: Version_projectId } });
                                tabpanel.add(grid);
                                tabpanel.setActiveTab(grid);
                            }
                            else {
                                tabpanel.setActiveTab('ResourceUse');
                            }
                        }
                        else if (record.data.id == '0409' && click_ok == 2) {

                            var tabpanel = Ext.getCmp('TabPanel');
                            var tab = tabpanel.getComponent('CostPanel');
                            if (tab == undefined) {
                                var grid = Ext.create('CosimRisk.view.CostPanel');
                                Ext.Ajax.request({
                                    url: '/SeverRes/Handler.ashx?method=73&simVersionId=' +simVersionId,
                                    success: function (resp, opts) {
                                        var data = Ext.JSON.decode(resp.responseText);//接收数据
                                        var text = Ext.getCmp('Direct_cost');
                                        direct_cost = Ext.JSON.decode(data.Direct_cost);
                                        text.setText('平均直接成本: ' + direct_cost);
                                    },
                                    failure: function (resp, opts) {
                                        Ext.Msg.alert('错误', '接收数据失败');
                                    }
                                });
                                tabpanel.add(grid)
                                tabpanel.setActiveTab(grid);
                            }
                            else {
                                tabpanel.setActiveTab('TaskwaitTabPanel');
                            }
                        }
                    }
                }
            },
            'IndirectWindow button[id=indirect_confirm]': {
                click: function () {
                    Ext.Ajax.request({
                        url: '/SeverRes/Handler.ashx?method=74&simVersionId=' +simVersionId,
                        success: function (resp, opts) {
                            var data = Ext.JSON.decode(resp.responseText);//接收数据
                            project_value = Ext.JSON.decode(data.project_value);
                            indirect_A = Ext.getCmp('Function_A').getValue()*1;
                            indirect_B = Ext.getCmp('Function_B').getValue()*1;
                            indirect_cost = indirect_A * project_value + indirect_B;
                            Ext.getCmp('InDirect_cost').setText('平均间接成本: ' + indirect_cost);
                            var total_cost = direct_cost + indirect_cost;
                            Ext.getCmp('Total_cost').setText('平均总成本: ' + total_cost);
                            var myData = [];
                            Ext.Ajax.request({
                                url: '/SeverRes/Handler.ashx?method=70&simVersionId=' + simVersionId + '&indirect_cost=' + indirect_cost,
                                async: false,
                                success: function (response) {
                                    var res = Ext.JSON.decode(response.responseText);
                                    myData = res.barData;
                                },
                                failure: function (response) {
                                    Ext.Msg.show({
                                        title: '错误',
                                        msg: '未知错误',
                                        width: 300,
                                        buttons: Ext.Msg.OK,
                                        icon: Ext.Msg.ERROR
                                    });
                                    return;
                                }
                            });
                            var chartStore = Ext.create('Ext.data.Store', {
                                fields: ['Range', 'ScNum', 'Ratio'],
                                autoLoad: true,
                                proxy: 'memory',
                                data: myData
                            })
                            var myChart = Ext.create('Ext.chart.Chart', {
                                animate: true,
                                store: chartStore,
                                width: document.body.clientWidth * 0.7,
                                height: document.body.clientHeight * 0.531,
                                //			theme: 'White',
                                axes: [{
                                    type: 'Category',
                                    position: 'bottom',
                                    fields: ['Range'],
                                    title: '基准(成本)',
                                    grid: true
                                }, {
                                    type: 'Numeric',
                                    position: 'left',
                                    fields: ['Ratio'],
                                    title: '概率',
                                    minimum: 0,
                                    maximum: 1,
                                    grid: true
                                }],
                                series: [{
                                    type: 'area',
                                    axis: 'left',
                                    highlight: true,
                                    xField: 'Range',
                                    yField: 'Ratio',
                                    tips: {
                                        trackMouse: true,
                                        width: 140,
                                        height: 40,
                                        renderer: function (storeItem, item) {
                                            this.setTitle(storeItem.get('Range') + '<br> ' + (storeItem.get('Ratio') * 100).toFixed(2) + '%');
                                        }
                                    },
                                    style: {
                                        lineWidth: 1,
                                        stroke: '#666',
                                        opacity: 0.86
                                    }
                                }]
                            });
                            var chartpanel = Ext.getCmp('CostChartPanel');
                            chartpanel.removeAll();
                            chartpanel.add(myChart);
                        },
                    failure: function (resp, opts) {
                        Ext.Msg.alert('错误', '接收数据失败');
                    }
                    }); 
                }
            },
            'openXMLWindow button[text=确认]': {
                click: function ( btn ) {
                    var form = btn.up( 'form' ).getForm();
                    var v = form.getValues();
                    if ( form.isValid() ) {
                        form.submit( {
                            url: '/SeverRes/Handler.ashx?method=1',
                            waitMsg: '正在提交XML...',
                            success: function ( form, action ) {
                                Ext.Msg.alert( '成功', '文件已经成功打开' );
                                var ss = form.owner.ownerCt.close();
                                Ext.getCmp( 'ProjectGrid' ).getStore().load();
                            },
                            failure: function ( form, action ) {
                                var result = Ext.decode( action.response.responseText );
                                Ext.Msg.alert( '失败', result.errMsg );
                            }
                        } );
                    } else {
                        Ext.Msg.alert( '提示', '请选取文件' );
                    }
                }
            },
            'UpdateXMLWindow button[action=submit]': {
                click: function ( btn ) {
                    var form = btn.up( 'form' ).getForm();
                    var v = form.getValues();
                    if ( form.isValid() ) {
                        form.submit( {
                            url: '/SeverRes/Handler.ashx?method=4',
                            waitMsg: '正在提交XML...',
                            success: function ( form, action ) {
                                Ext.Msg.alert( '成功', '已经成功修改描述' );
                                var ss = form.owner.ownerCt.close();
                                Ext.getCmp( 'ProjectGrid' ).getStore().load();
                            },
                            failure: function ( form, action ) {
                                var result = Ext.decode( action.response.responseText );
                                Ext.Msg.alert( '失败', result.errMsg );
                            }
                        } );
                    } else {
                        Ext.Msg.alert( '提示', '请选取文件' );
                    }
                }
            },
            'SimulationGrid button[id=newSimBtn]': {
                click: function () {
                    if ( projectId == null )
                        Ext.Msg.alert( '提示', "请您先选择项目" );
                    else {
                        var win = Ext.create( 'CosimRisk.view.SimulationWindow' );
                        win.setTitle(  projectName  );
                        win.show();
                    }
                }
            },
            'SimulationGrid button[id=chooseSimBtn]': {
                click: function ( o ) {
                    var gird = o.ownerCt.ownerCt;
                    var data = gird.getSelectionModel().getSelection();
                    var parentNode = Ext.getCmp("TreePanel").getRootNode().findChild('id', '04');
                    if ( data.length == 0 ) {
                        Ext.Msg.alert( "提示", "您要选择一个任务" );
                    }
                    else {
                        if (simVersionId == null) {
                            parentNode.expand();
                            if (data[0].get("have_resource") == "√")
                                click_ok = 2;
                            else if (data[0].get("have_resource") == "×")
                                click_ok = 1; 
                        }
                        else
                        {
                            if (data[0].get("have_resource") == "√") 
                                click_ok = 2;
                            else if (data[0].get("have_resource") == "×") 
                                click_ok = 1;
                            var tabpanel = Ext.getCmp('TabPanel');
                            for (var i = tabpanel.items.length - 1; i >= 0; i--)
                             {
                                    var id = tabpanel.items.get(i).id;
                                    if (id == 'criticalRatioPanel' || id == 'barDataPanel' || id == 'averageProjectBarPanel' || id == 'CriticalPanel' || id == 'RiskPanel')
                                        tabpanel.remove(tabpanel.items.get(i));
                            }

                        }
                        Version_projectId = data[0].get('priId');
                        tree_Version_projectId = Version_projectId;
                        simVersionId = data[0].get( "simVersionId" );
                        count = data[0].get( "count" );
                        desciption = data[0].get("desciption");
                        Ext.getCmp('version_label').setText('仿真任务:' + desciption);//节点用set方法
                        Ext.Msg.alert( "提示", "选择任务:" +  desciption );
                    }
                }
            },
            'SimulationGrid button[id=deleteSimBtn]': {
                click: function () {
                    var grid = Ext.getCmp('SimulationGrid');
                    var data = grid.getSelectionModel().getSelection();
                    if (data.length == 0)
                        Ext.Msg.alert('提示', "请您先选择版本");
                else {
                        var simVersionId = data[0].get("simVersionId");
                        Ext.Ajax.request({
                            url: '/SeverRes/Handler.ashx?method=43&simVersionId=' + simVersionId,
                            async: false,
                            success: function (response) {
                                Ext.Msg.alert("提示", "成功删除!");
                                Ext.getCmp('SimulationGrid').getStore().load();
                            },
                            failure: function (response) {
                                Ext.Msg.show({
                                    title: '错误',
                                    msg: '未知错误',
                                    width: 300,
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.Msg.ERROR
                                });
                                return;
                            }
                        });
                    }
                }
            },
            'SimulationWindow button[id=StartSimBtn]': {
                click: function ( e, eOpts ) {
                    var form = Ext.getCmp( 'SimulationForm' );
                    if ( form.isValid() ) {
                        Ext.MessageBox.wait( '仿真进行中...', '请稍等' );
                        form.submit( {
                            params: { projectId: projectId },
                            url: '/SeverRes/Handler.ashx?method=41',
                            timeout: 30000,
                            success: function ( form, action ) {
                                e.ownerCt.ownerCt.ownerCt.close();
                                Ext.getCmp( 'SimulationGrid' ).getStore().load();
                                Ext.MessageBox.hide();
                            },
                            failure: function ( form, action ) {
                                Ext.MessageBox.hide();
                                Ext.Msg.alert( '错误', action.result.errMsg );
                            }
                        } );
                    }
                }
            },
            'CriticalPanel button[id=CriticalsubmitBtn]': {
                click: function ( e, eOpts ) {
                    var criticalpanel = Ext.getCmp( 'CriticalPanel' );
                    var simSwquence = Ext.getCmp('time').getValue();
                    if (simSwquence > 0 && simSwquence <= count) {
                        var d = Ext.getCmp('Critical_drawComponent');
                        if (d != undefined)
                            criticalpanel.remove(d);
                        var drawComponent = Ext.create('Ext.draw.Component', {
                            viewBox: false,
                            height: 500,
                            width: 600,
                            id: 'Critical_drawComponent',
                            renderTo: document.body
                        }),
                        surface = drawComponent.surface;//定义画板
                        receive_and_drawCritical(61,drawComponent, surface, 0, Version_projectId, -1, simVersionId, simSwquence);//接收数据并画图
                        criticalpanel.add(drawComponent);//添加组件
                    }
                    else
                        alert('输入错误');
                }
            },
            'ResourceUse combo[id=Resourcename_combo]': {
                change: function (o, newValue, oldValue, eOpts) {
                    var resourceId = newValue;
                    Ext.Ajax.request({
                        url: '/SeverRes/Handler.ashx?method=69&simVersionId=' + simVersionId + '&resourceId=' + resourceId,
                        async: false,
                        success: function (response) {
                            var res = Ext.JSON.decode(response.responseText);
                            var myData = res.barData;
                            var chartStore = Ext.create('Ext.data.Store', {
                                fields: ['Range', 'Ratio'],
                                autoLoad: true,
                                proxy: 'memory',
                                data: myData
                            });
                            var myChart = Ext.create('Ext.chart.Chart', {
                                animate: true,
                                store: chartStore,
                                width: document.body.clientWidth * 0.7,
                                height: document.body.clientHeight * 0.58,
                                axes: [{
                                    type: 'Category',
                                    position: 'bottom',
                                    fields: ['Range'],
                                    title: '时间区间(天)',
                                    grid: true
                                }, {
                                    type: 'Numeric',
                                    position: 'left',
                                    fields: ['Ratio'],
                                    title: '使用数量',
                                    minimum: 0,
                                    maximum: 1,
                                    grid: true
                                }],
                                series: [{
                                    type: 'area',
                                    axis: 'left',
                                    highlight: true,
                                    xField: 'Range',
                                    yField: 'Ratio',
                                    tips: {
                                        trackMouse: true,
                                        width: 140,
                                        height: 40,
                                        renderer: function (storeItem, item) {
                                            this.setTitle(storeItem.get('Range') + '<br> ' + storeItem.get('Ratio').toFixed(2) );
                                        }
                                    },
                                    style: {
                                        lineWidth: 1,
                                        stroke: '#666',
                                        opacity: 0.86
                                    }
                                }]
                            });
                            var chartpanel = Ext.getCmp('ResourceUse');
                            chartpanel.removeAll();
                            chartpanel.add(myChart);
                        },
                        failure: function (response) {
                            Ext.Msg.show({
                                title: '错误',
                                msg: '未知错误',
                                width: 300,
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.ERROR
                            });
                            return;
                        }
                    });
                }
            },
            'barDataPanel combo[id=ChangeAxis_combo]': {
                change: function (o, newValue, oldValue, eOpts) {
                    var scale = Ext.getCmp('ChangeScale_combo').getValue();
                    Ext.Ajax.request({
                        url: '/SeverRes/Handler.ashx?method=64&simVersionId=' + simVersionId + '&scale=' + scale,
                        async: false,
                        success: function (response) {
                            var res = Ext.JSON.decode(response.responseText);
                            myData_old = res.barData;
                            myData_new = res.barData;
                        },
                        failure: function (response) {
                            Ext.Msg.show({
                                title: '错误',
                                msg: '未知错误',
                                width: 300,
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.ERROR
                            });
                            return;
                        }
                    });
                    var chartStore = Ext.create('Ext.data.Store', {
                        fields: ['Range', 'ScNum'],
                        autoLoad: true,
                        proxy: 'memory',
                        data: myData_old
                    });
                    if (newValue == "频数直方图")
                    {
                        var panel = Ext.getCmp('barDataPanel');
                        panel.removeAll();
                        var chart = Frequence_histogram(chartStore);
                        panel.add(chart);
                    }
                    if (newValue == "频率直方图")
                    {
                        var panel = Ext.getCmp('barDataPanel');
                        panel.removeAll();
                        var chart = Frequency_histogram(chartStore);
                        panel.add(chart);
                    }
                }
            },
            'barDataPanel combo[id=ChangeScale_combo]': {
                change: function (o, newValue, oldValue, eOpts) {
                    Ext.Ajax.request({
                        url: '/SeverRes/Handler.ashx?method=64&simVersionId=' + simVersionId + '&scale=' + newValue,
                        async: false,
                        success: function (response) {
                            var res = Ext.JSON.decode(response.responseText);
                            myData_old = res.barData;
                            myData_new = res.barData;
                        },
                        failure: function (response) {
                            Ext.Msg.show({
                                title: '错误',
                                msg: '未知错误',
                                width: 300,
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.ERROR
                            });
                            return;
                        }
                    });
                    var chartStore = Ext.create('Ext.data.Store', {
                        fields: ['Range', 'ScNum'],
                        autoLoad: true,
                        proxy: 'memory',
                        data: myData_old
                    });
                    var combo = Ext.getCmp('ChangeAxis_combo');
                    if (combo.getValue() == "频数直方图")
                    {
                        var panel = Ext.getCmp('barDataPanel');
                        panel.removeAll();
                        var chart = Frequence_histogram(chartStore);
                        panel.add(chart);
                    }
                    if (combo.getValue() == "频率直方图")
                    {
                        var panel = Ext.getCmp('barDataPanel');
                        panel.removeAll();
                        var chart = Frequency_histogram(chartStore);
                        panel.add(chart);
                    }
                }
            },
            'ProjectImage':
              {
                  beforeactivate:function( o, eOpts )
                  {
                      var tabpanel = Ext.getCmp('TabPanel');
                      var tab = Ext.getCmp('ProjectImage');
                      tab.removeAll();
                      //定义组件，所有图案都添加其中
                      var drawComponent = Ext.create('Ext.draw.Component', {
                          viewBox: false,
                          height: 500,
                          width: 600,
                          renderTo: document.body
                      }),
                      surface = drawComponent.surface;//定义画板
                      receive_and_draw(drawComponent, surface, 0, projectId, -1);//接收数据并画图,主函数
                      tab.add(drawComponent);//添加组件
                  }
              
              },
            'CriticalTree': {
                itemclick:function( o, record, item, index, e, eOpts ){
                    if (record.data.leaf == false)
                    {
                        if (old_treercord != undefined)
                        {
                            old_treercord.data.text = old_text;
                            if (old_treercord.data.id!=record.data.id && old_treercord.data.id!='root')
                                old_treercord.collapse();
                        }
                        old_treercord = record
                        old_text = record.data.text;
                        record.data.text = '<font color="red">' + record.data.text + '</font>';
                        var CriticalChartPanel = Ext.getCmp('CriticalChartPanel');
                        CriticalChartPanel.setTitle(old_text);
                        o.refresh();
                        if (record.data.expanded == false)
                            record.expand();
                        CriticalChartPanel.removeAll();
                        var chart;
                        if (record.data.id == 'root')
                        {
                            old_text = '<font color="blue">概要任务</font>';
                            record.data.text = '<font color="blue">概要任务</font>';
                            CriticalChartPanel.setTitle('概要任务');
                            chart = Project_probability(-1);
                            o.refresh();
                        }
                        else
                            chart = Project_probability(record.data.id);
                            CriticalChartPanel.add(chart);
                    }
                }
            },
            'TaskTree': {
                itemclick: function (o, record, item, index, e, eOpts) {
                    var TaskwaitPanel = Ext.getCmp("TaskwaitPanel");
                    TaskwaitPanel.setTitle(record.data.text);
                    TaskwaitPanel.removeAll();
                    var chart;
                    if (record.data.id == 'root')
                    chart = Task_Wait(-1);
                    else
                    chart = Task_Wait(record.data.id);
                    TaskwaitPanel.add(chart);
                }
            },
            'ProjectImage_Task':
                {
                    beforeactivate:function( o, eOpts )
                    {
                        var data=new Object();
                        Ext.Ajax.request({
                            url: '/SeverRes/Handler.ashx?method=23' + '&projectId=' + projectId ,
                            async: false,
                            success: function (response) {
                                var res = Ext.JSON.decode(response.responseText);
                                data = Ext.JSON.decode(res.task);
                                for (var i = 0; i < data.length; i++)
                                {
                                    if (data[i].Task_is_summary == true) {
                                        data[i].Task_is_summary = '√';
                                    }
                                    else
                                        data[i].Task_is_summary = ' ';

                                    if (data[i].IsDone == true)
                                        data[i].IsDone = '√';
                                    else
                                        data[i].IsDone = ' ';
                                }
                            },
                            failure: function (response) {
                                Ext.Msg.show({
                                    title: '错误',
                                    msg: '未知错误',
                                    width: 300,
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.Msg.ERROR
                                });
                                return;
                            }
                        });
                        var store = Ext.getCmp('ProjectImage_Task').getStore();
                        store.removeAll();
                        store.add(data);
                    }
                },
            'CostAddedPanel button[id=InDirect_set]':
                {
                    click: function (e, eOpts) {

                        var win = Ext.create('CosimRisk.view.IndirectWindow');
                        win.show();
                    }
                },
            'ResourceAddedPanel button[id=AddRecBtn]':
                {
                    click: function (e, eOpts) {
                        Resource = new Object();
                        Resource.Resource_Name = Ext.getCmp('Resource_Name').getValue();
                        Resource.Resource_Mount = Ext.getCmp('Resource_Mount').getValue();
                        Resource.Resource_Price = Ext.getCmp('Resource_Price').getValue();
                        Resource.Resource_Type = Ext.getCmp('Resource_Type').getValue();
                        Resource.Resource_Description = Ext.getCmp('Resource_Description').getValue();
                        var json = Ext.encode(Resource);
                        Ext.Ajax.request({
                            url: '/SeverRes/Handler.ashx?method=8',
                            params: {
                                data: json
                            },
                            async: false,
                            success: function (response) {

                            },
                            failure: function (response) {
                                Ext.Msg.show({
                                    title: '错误',
                                    msg: '未知错误',
                                    width: 300,
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.Msg.ERROR
                                });
                                return;
                            }
                        });
                    }
                },
            'ResourceAddedPanel button[id=DeleteRecBtn]':
             {
                 click: function (e, eOpts) {
                     var Resource_Name = Ext.getCmp('Resource_Name').getValue();

                     Ext.Ajax.request({
                         url: '/SeverRes/Handler.ashx?method=10',
                         params: {
                             data: Resource_Name
                         },
                         async: false,
                         success: function (response) {

                         },
                         failure: function (response) {
                             Ext.Msg.show({
                                 title: '错误',
                                 msg: '未知错误',
                                 width: 300,
                                 buttons: Ext.Msg.OK,
                                 icon: Ext.Msg.ERROR
                             });
                             return;
                         }
                     });
                 }
             },
            'ProjectManagement':
             {
                 beforeshow: function (e, eOpts) {
                     
                 }
             }
        } );
    }
} );

/*以下为函数**********************************************************************************************/

function Frequence_histogram(Total_chartStore) {

    var myChart = Ext.create('Ext.chart.Chart', {
        //                                 animate: true,
        store: Total_chartStore,
        width: document.body.clientWidth * 0.7,
        height: document.body.clientHeight * 0.7,
        //			theme: 'White',
        axes: [{
            type: 'Category',
            position: 'bottom',
            fields: ['Range'],
            title: '工期区间',
            grid: true
        }, {
            type: 'Numeric',
            position: 'left',
            fields: ['ScNum'],
            title: '个数',
            minimum: 0,
            grid: true
        }],
        series: [
            {
                xField: 'Range',
                yField: 'ScNum',
                type: 'column',
                axis: 'left',
                highlight: true,
                tips: {
                    trackMouse: true,
                    width: 100,
                    height: 40,
                    renderer: function (storeItem, item) {
                        this.setTitle(storeItem.get('Range') + '区间' + '<br>' + storeItem.get('ScNum') + ' 个');
                    }
                },
                label: {
                    display: 'insideEnd',
                    field: 'number',
                    renderer: Ext.util.Format.numberRenderer('0'),
                    orientation: 'horizontal',
                    color: '#333',
                    'text-anchor': 'middle'
                }
            },
        {
            xField: 'Range',
            yField: 'ScNum',
            type: 'line',
            markerConfig: {
                type: 'cross',
                size: 3,
                radius: 3,
                'stroke-width': 0,
                'fill': 'red'
            }
        }]
    });
    return myChart;
}

function Frequency_histogram(Total_chartStore)
{
    var pinglv_array = new Array();
    var total = 0;
    Total_chartStore.each(function (record) {
        pinglv_array.push(record.data.ScNum);
        total += record.data.ScNum;
    })
    for (i = 0; i < pinglv_array.length; i++) {
        pinglv_array[i] = (pinglv_array[i] / total).toFixed(4);
        myData_new[i].ScNum = pinglv_array[i];
        if (myData_new[i].ScNum > MaxFrequency)
            MaxFrequency = myData_new[i].ScNum
    }
    MaxFrequency = Math.ceil(MaxFrequency * 10) / 10;
    Frequency_chartStore = Ext.create('Ext.data.Store', {
        fields: ['Range', 'ScNum'],
        autoLoad: true,
        proxy: 'memory',
        data: myData_new
    });
    var myChart = Ext.create('Ext.chart.Chart', {
        //                                 animate: true,
        store: Frequency_chartStore,
        width: document.body.clientWidth * 0.7,
        height: document.body.clientHeight * 0.7,
        //			theme: 'White',
        axes: [{
            type: 'Category',
            position: 'bottom',
            fields: ['Range'],
            title: '工期区间',
            grid: true
        }, {
            type: 'Numeric',
            position: 'left',
            fields: ['ScNum'],
            title: '频率',
            minimum: 0,
            maximum: MaxFrequency,
            grid: true
        }],
        series: [
            {
                xField: 'Range',
                yField: 'ScNum',
                type: 'column',
                axis: 'left',
                highlight: true,
                tips: {
                    trackMouse: true,
                    width: 100,
                    height: 40,
                    renderer: function (storeItem, item) {
                        this.setTitle(storeItem.get('Range') + '区间' + '<br>' + '频率:' + storeItem.get('ScNum'));
                    }
                },
                label: {
                    display: 'insideEnd',
                    field: 'number',
                    renderer: Ext.util.Format.numberRenderer('0'),
                    orientation: 'horizontal',
                    color: '#333',
                    'text-anchor': 'middle'
                }
            }]
    });
    return myChart;
}

function Project_probability(taskId)
{
    Ext.Ajax.request({
        url: '/SeverRes/Handler.ashx?method=63&simVersionId=' + simVersionId + '&taskId=' + taskId,
        async: false,
        success: function (response) {
            var res = Ext.JSON.decode(response.responseText);
            myData = res.task;
        },
        failure: function (response) {
            Ext.Msg.show({
                title: '错误',
                msg: '未知错误',
                width: 300,
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.ERROR
            });
            return;
        }
    });
    var chartStore = Ext.create('Ext.data.Store', {
        fields: ['Task_name', 'CriticalRatio'],
        autoLoad: true,
        proxy: 'memory',
        data: myData
    })
    var myChart = Ext.create('Ext.chart.Chart', {
        //                                    animate: true,
        store: chartStore,
       width: document.body.clientWidth * 0.60,
        height: document.body.clientHeight * 0.60,
        		//theme: 'White',
        axes: [{
            type: 'Category',
            position: 'bottom',
            fields: ['Task_name'],
            title: '任务名称',
            grid: true
        }, {
            type: 'Numeric',
            position: 'left',
            fields: ['CriticalRatio'],
            title: '概率',
            minimum: 0,
            grid: true,
            maximum: 1
        }],
        series: [
            {
                xField: 'Task_name',
                yField: 'CriticalRatio',
                type: 'column',
               // xPadding:150,
               // renderer: function(sprite, record, attr, index, store) {
                  //  return Ext.apply(attr, {
                  //      width: 60,
                  //  });
               // },
                axis: 'left',
                highlight: true,
                tips: {
                    trackMouse: true,
                    width: 140,
                    height: 40,
                    renderer: function (storeItem, item) {
                        this.setTitle(storeItem.get('Task_name') + '<br> ' + (storeItem.get('CriticalRatio') * 100).toFixed(2) + '%');
                    }
                },
                label: {
                    display: 'insideEnd',
                    field: 'number',
                    renderer: Ext.util.Format.numberRenderer('0'),
                    orientation: 'horizontal',
                    color: '#333',
                    'text-anchor': 'middle'
                }
            },
        {
            xField: 'Task_name',
            yField: 'CriticalRatio',
            type: 'line',
            markerConfig: {
                type: 'cross',
                size: 4,
                radius: 4,
                'stroke-width': 0,
                'fill': 'red'
            }
        }]
    });
    return myChart;
}

function Task_Wait(taskId) {
    Ext.Ajax.request({
        url: '/SeverRes/Handler.ashx?method=68&simVersionId=' + simVersionId + '&taskId=' + taskId,
        async: false,
        success: function (response) {
            var res = Ext.JSON.decode(response.responseText);
            myData = res.task;
        },
        failure: function (response) {
            Ext.Msg.show({
                title: '错误',
                msg: '未知错误',
                width: 300,
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.ERROR
            });
            return;
        }
    });
    if (taskId == -1)
    {
        var name = '任务名称';
        var field = 'Task_name';
    }
    else
    {
        var name = '资源名称';
        var field = 'Resource_name';
    }
    var chartStore = Ext.create('Ext.data.Store', {
        fields: [field, 'wait_time'],
        autoLoad: true,
        proxy: 'memory',
        data: myData
    })
    var myChart = Ext.create('Ext.chart.Chart', {
        //                                    animate: true,
        store: chartStore,
        width: document.body.clientWidth * 0.60,
        height: document.body.clientHeight * 0.60,
        //theme: 'White',
        axes: [{
            type: 'Category',
            position: 'bottom',
            fields: field,
            title: name,
            grid: true
        }, {
            type: 'Numeric',
            position: 'left',
            fields: ['wait_time'],
            title: '等待时间',
            minimum: 0,
            grid: true
        }],
        series: [
            {
                xField: name,
                yField: 'wait_time',
                type: 'column',
               // style: { width: 25 },
                // xPadding:150,
                // renderer: function(sprite, record, attr, index, store) {
                //  return Ext.apply(attr, {
                //      width: 60,
                //  });
                // },
                axis: 'left',
                highlight: true,
                tips: {
                    trackMouse: true,
                    width: 140,
                    height: 40,
                    renderer: function (storeItem, item) {
                        this.setTitle(storeItem.get('wait_time').toFixed(2)+'天' );
                    }
                },
                label: {
                    display: 'insideEnd',
                    field: 'number',
                    renderer: Ext.util.Format.numberRenderer('0'),
                    orientation: 'horizontal',
                    color: '#333',
                    'text-anchor': 'middle'
                }
            }]
    });
    return myChart;
}