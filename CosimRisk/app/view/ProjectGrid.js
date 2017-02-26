Ext.define('CosimRisk.view.ProjectGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.ProjectGrid',
    id: 'ProjectGrid',
    autoScroll:true, 
    title: '项目信息',
    selModel: Ext.create('Ext.selection.CheckboxModel', {
        mode: "SINGlE",
        showHeaderCheckbox: false
    }),
    store: 'ProjectGridStore',
    columns: [{
        xtype: 'rownumberer',
        width: 50,
        text: '序号'
    }, {
        text: '项目名称',
        dataIndex: 'PRJ_NAME',
        flex: 1
    }, {
        text: '项目描述信息',
        dataIndex: 'PRJ_DESCRIBE',
        flex: 1
    },
    {
        text: '项目创建时间',
        dataIndex: 'PRJ_DATE',
        flex: 1
    }, {
        text: 'XML文件',
        flex: 1,
        dataIndex: 'PRJ_XML',
        renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
            return "<font color=\"blue\">显示XML</font>";
        }
    }, {
        text: '项目资源分配',
        flex: 1,
        dataIndex: '资源分配',
        renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
            return "<font color=\"blue\">资源分配</font>";
        }
    }],
    buttons: [{
        xtype: 'button',
        text: '新建项目',
        id: 'openXMLBtn'
    }, {
        xtype: 'button',
        text: '选为当前项目',
        id: 'chooseAsProjectBtn'
    },
    {
        xtype: 'button',
        text: '修改当前项目描述',
        id: 'modifyProjectBtn'
    }, {
        xtype: 'button',
        text: '删除选定项目',
        id: 'deleteProjectBtn'
    }],
    dockedItems: [{
        xtype: 'pagingtoolbar',
        store: 'ProjectGridStore',
        dock: 'bottom',
        displayInfo: true
    }]
});