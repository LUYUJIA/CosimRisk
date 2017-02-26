function createTask_xy(x, y, task)//建立一个存储Task的类
{
    Task = new Object();
    Task.x = x;
    Task.y = y;
    Task.Task_id = task.Task_id;
    Task.Task_name = task.Task_name;
    Task.IsSummary = task.Task_is_summary;
    Task.OutDegree = task.OutDegree;
    Task.Auto_id = task.Auto_id;
    Task.Task_priority = task.Task_priority;
    Task.ArgA = task.ArgA;
    Task.ArgB = task.ArgB;
    Task.ArgC = task.ArgC;
    Task.actualValue = task.Value;
    Task.ExpressionName = task.ExpressionName;
    Task.IsDone = task.IsDone;
    Task.Task_is_critical = task.Task_is_critical; 
    Task.Have_resource = task.Have_resource;
    return Task;
}

function compute_start(list_task, start_x, fix_y, queue, task_xy)//计算始点坐标
{
    for (var i = 0; i < list_task.length; i++) {
        if (list_task[i].InDegree == 0) {
            start_y = 0;
            var task = createTask_xy(start_x, start_y, list_task[i]);
            queue.push(task);//放入队列
            task_xy.push(task);//存储坐标
            return start_y;
        }
    }

}

function compute_sink(list_link, InDegree, son_id, task_xy)//判断是否计算汇点和计算汇点y值
{
    var total_y = 0;
    var y = -1;//不可能取到的-1
    var parent_id;
    var done = false;//是否已经做了
    var ready = 0;//计算已存储父节点数，最后应该等于入度
    for (var i = 0; i < task_xy.length; i++) {
        if (task_xy[i].Task_id == son_id) {
            done = true;
            return y;
        }
    }
    if (!done) {
        for (var i = 0; i < list_link.length; i++) {
            if (list_link[i].Task_suc_id == son_id) {
                parent_id = list_link[i].Task_pre_id;
                for (var j = 0; j < task_xy.length; j++) {
                    if (task_xy[j].Task_id == parent_id) {
                        total_y += task_xy[j].y;
                        ready++;
                    }
                }
            }
        }
    }
    if (ready == InDegree) {
        y = Math.round(total_y / InDegree);
        return y;
    }
    else
        return y;
}

function find_mid_MaxSucceedSize(list_link, list_task, task, mid_count)//找中间那个孩子的最大宽度
{
    var number = 1;

    for (var i = 0; i < list_link.length; i++) {
        if (list_link[i].Task_pre_id == task.Task_id) {
            for (var j = 0; j < list_task.length; j++) {
                if (list_task[j].Task_id == list_link[i].Task_suc_id) {
                    if (number == mid_count) {
                        return list_task[j].MaxSucceedSize / 2
                    }
                    else
                        number++;
                }
            }
        }
    }
}

function compute_task_xy(list_task, list_link, fix_x, fix_y, queue, task_xy)//计算非始终点的点坐标
{
    while (queue.length != 0) {
        var task = queue.shift();//以这个task为中心
        var count = 1;//标志是第几个孩子
        var total = task.OutDegree;//孩子总数
        var mid_count = Math.ceil(total / 2)//中位数
        var mid_width = find_mid_MaxSucceedSize(list_link, list_task, task, mid_count);//记录中间宽度，给奇数点用

        for (var i = 0; i < list_link.length; i++) {
            if (list_link[i].Task_pre_id == task.Task_id) {
                for (var j = 0; j < list_task.length; j++) {
                    if (list_task[j].Task_id == list_link[i].Task_suc_id) {
                        if (task.OutDegree % 2 == 1)//奇数画法
                        {
                            if (list_task[j].InDegree > 1)//是汇点
                            {
                                var y = compute_sink(list_link, list_task[j].InDegree, list_task[j].Task_id, task_xy);
                                if (y != -1) {
                                    var x = task.x + fix_x;
                                    var son = createTask_xy(x, y, list_task[j]);
                                    if (list_task[j].OutDegree != 0)//不是终点
                                        queue.push(son);
                                    task_xy.push(son);

                                }
                            }
                            else//普通点
                            {
                                if (count == mid_count) {
                                    var x = task.x + fix_x;
                                    var y = task.y;
                                    var son = createTask_xy(x, y, list_task[j]);
                                    if (list_task[j].OutDegree != 0)//不是终点
                                        queue.push(son);
                                    task_xy.push(son);
                                    count++;
                                }
                                else if (count < mid_count) {
                                    var x = task.x + fix_x;
                                    var y = task.y + (-1) * fix_y * (list_task[j].MaxSucceedSize / 2 * Math.abs(count - mid_count) + mid_width / 2);
                                    var son = createTask_xy(x, y, list_task[j]);
                                    if (list_task[j].OutDegree != 0)//不是终点
                                        queue.push(son);
                                    task_xy.push(son);
                                    count++;
                                }
                                else if (count > mid_count) {
                                    var x = task.x + fix_x;
                                    var y = task.y + 1 * fix_y * (list_task[j].MaxSucceedSize / 2 * Math.abs(count - mid_count) + mid_width / 2);
                                    var son = createTask_xy(x, y, list_task[j]);
                                    if (list_task[j].OutDegree != 0)//不是终点
                                        queue.push(son);
                                    task_xy.push(son);
                                    count++;
                                }
                            }

                        }
                        else if (task.OutDegree % 2 == 0)//偶数画法
                        {
                            if (list_task[j].InDegree > 1)//是汇点
                            {
                                var y = compute_sink(list_link, list_task[j].InDegree, list_task[j].Task_id, task_xy);
                                if (y != -1) {
                                    var x = task.x + fix_x;
                                    var son = createTask_xy(x, y, list_task[j]);
                                    if (list_task[j].OutDegree != 0)//不是终点
                                        queue.push(son);
                                    task_xy.push(son);
                                }
                            }
                            else//普通点
                            {
                                if (count <= mid_count) {
                                    var x = task.x + fix_x;
                                    var y = task.y + (-1) * fix_y * (list_task[j].MaxSucceedSize / 2 * (Math.abs(count - mid_count) + 1) - 0.25);
                                    var son = createTask_xy(x, y, list_task[j]);
                                    if (list_task[j].OutDegree != 0)//不是终点
                                        queue.push(son);
                                    task_xy.push(son);
                                    count++;
                                }
                                else if (count > mid_count) {
                                    var x = task.x + fix_x;
                                    var y = task.y + 1 * fix_y * (list_task[j].MaxSucceedSize / 2 * Math.abs(count - mid_count) - 0.25);
                                    var son = createTask_xy(x, y, list_task[j]);
                                    if (list_task[j].OutDegree != 0)//不是终点
                                        queue.push(son);
                                    task_xy.push(son);
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

function setDrawComponent(drawComponent, start_x, task_xy)//设置组件高宽
{
    var min_y = 1000000;
    var max_y = 0;
    var height = 0;
    for (var i = 0; i < task_xy.length; i++) {
        if (task_xy[i].y < min_y)
            min_y = task_xy[i].y;
        if (task_xy[i].y > max_y)
            max_y = task_xy[i].y;
    }
    var sub = 0;//补差值
    sub = 0 - min_y + 40;
    for (var i = 0; i < task_xy.length; i++) {
        task_xy[i].y += sub;
    }
    height = max_y - min_y + 80;//两倍半径
    var width;
    var end_x;
    for (var i = 0; i < task_xy.length; i++) {
        if (task_xy[i].OutDegree == 0) {
            end_x = task_xy[i].x;
        }
    }
    width = end_x - start_x + 150;
    drawComponent.setHeight(height);
    drawComponent.setWidth(width);
}

function more_Image(e, eOpts)//概要任务添加子任务网络图
{
    var Task_name = e.name;
    var array = new Array();
    array = e.id.split(",");
    var Taskid = array[0];
    var taskLevel = array[1];
    taskLevel++;
    var projectId = array[2];
    var tabpanel = Ext.getCmp('TabPanel');
    var tab = tabpanel.getComponent(Taskid);
    if (tab == undefined) {
        var TaskPanel = Ext.create('Ext.panel.Panel', {
            id: Taskid,
           // html: '<img src="/resources/images/surface.png" style="width:100%; height:100%;"/>',
            closable: true,
            autoScroll: true,
            layout: 'absolute'
        });
        TaskPanel.setTitle(Task_name);
        tabpanel.add(TaskPanel);
        tabpanel.setActiveTab(TaskPanel);
        //定义组件，所有图案都添加其中
        var drawComponent = Ext.create('Ext.draw.Component', {
            viewBox: false,
            height: 500,
            width: 600,
            renderTo: document.body
        }),
        surface = drawComponent.surface;//定义画板
        receive_and_draw(drawComponent, surface, taskLevel, projectId, Taskid);//接收数据并画图,主函数
        TaskPanel.add(drawComponent);//添加组件
    }
    else
        tabpanel.setActiveTab(tab);
}


var Reg = /^\d*\.?\d{0,8}$/;//正则表达式 限定只能是数字和小数点

function create_TaskWin(Auto_id, Task_priority, ArgA, ArgB, ArgC, ExpressionName, parentId, taskLevel, projectId) //创建一个Task任务窗口
{

    var store = Ext.create('CosimRisk.store.expressionNameStore');
    var store2 = Ext.create('CosimRisk.store.taskPriorityStore');
    var win = Ext.create('Ext.window.Window', {
        closable: true,
        height: 400,
        width: 600,
        modal: true,
        layout: 'fit',
        items: [{
            xtype: 'tabpanel',
            frame: true,
            activeTab: 0,
            items: [{
                xtype: 'form',
                id: 'win_form',
                title: '任务属性设置',
                //closable: true,
                layout: 'border',
                labelWidth: 60,
                border: false,
                items: [{
                    title: '设置任务参数',
                    xtype: "panel",
                    id: 'panel1',
                    region: 'north',
                    height: 150,
                    layout: 'column',
                    items:
                [{
                    xtype: 'combo',
                    id: 'win_combo',
                    columnWidth: 0.35,
                    labelWidth: 60,
                    border: false,
                    anchor: '90%',
                    valueField: 'expressionName',
                    id: 'expressionName' + Auto_id,
                    editable: false,
                    allowBlank: false,
                    blankText: '不能为空',
                    margin: '10 0 0 8',
                    fieldLabel: '分布类型',
                    value: '三角分布',
                    displayField: 'expressionName',
                    store: store,
                    listeners: {
                        select: function (combo, record, index) {
                            form = combo.ownerCt;
                            if (combo.getValue() == '固定') {
                                form.getComponent('A').setFieldLabel('天数');
                                form.getComponent('B').setVisible(false);
                                form.getComponent('B').setDisabled(true);
                                form.getComponent('C').setVisible(false);
                                form.getComponent('C').setDisabled(true);
                            }
                            if (combo.getValue() == '三角分布') {
                                form.getComponent('A').setFieldLabel('低限');
                                form.getComponent('B').setVisible(true);
                                form.getComponent('B').setFieldLabel('众数');
                                form.getComponent('B').setDisabled(false);
                                form.getComponent('C').setVisible(true);
                                form.getComponent('C').setFieldLabel('上限');
                                form.getComponent('C').setDisabled(false);
                            }
                            if (combo.getValue() == 'Beta分布') {
                                form.getComponent('A').setFieldLabel('α');
                                form.getComponent('B').setVisible(true);
                                form.getComponent('B').setFieldLabel('β');
                                form.getComponent('B').setDisabled(false);
                                form.getComponent('C').setVisible(false);
                                form.getComponent('C').setDisabled(true);
                            }
                            if (combo.getValue() == '正态分布') {
                                form.getComponent('A').setFieldLabel('期望');
                                form.getComponent('B').setVisible(true);
                                form.getComponent('B').setFieldLabel('方差');
                                form.getComponent('B').setDisabled(false);
                                form.getComponent('C').setVisible(false);
                                form.getComponent('C').setDisabled(true);
                            }
                        }
                    }
                },
                {
                    xtype: 'textfield',
                    columnWidth: 0.20,
                    labelWidth: 40,
                    margin: '10 0 0 10',
                    fieldLabel: '低限',
                    id: 'A',
                    name: 'A',
                    regex: Reg,
                    regexText: '只能为整数或者小数'
                }, {
                    xtype: 'textfield',
                    columnWidth: 0.20,
                    labelWidth: 40,
                    // allowBlank: false,
                    // blankText: '不能为空',
                    margin: '10 0 0 10',
                    fieldLabel: '众数',
                    id: 'B',
                    name: 'B',
                    regex: Reg,
                    regexText: '只能为整数或者小数'
                }, {
                    xtype: 'textfield',
                    columnWidth: 0.20,
                    labelWidth: 40,
                    // allowBlank: false,
                    //blankText: '不能为空',
                    margin: '10 0 0 10',
                    fieldLabel: '上限',
                    id: 'C',
                    name: 'C',
                    regex: Reg,
                    regexText: '只能为整数或者小数'
                }, {
                    xtype: 'combo',
                    columnWidth: 0.35,
                    labelWidth: 70,
                    border: false,
                    //allowBlank: false,
                    //blankText: '不能为空',
                    anchor: '90%',
                    valueField: 'taskPr',
                    editable: false,
                    margin: '10 0 0 8',
                    fieldLabel: '任务优先级',
                    value: '1',
                    displayField: 'taskPriority',
                    id: 'taskPriority',
                    name: 'taskPriority',
                    store: store2
                }]
                },

                   {
                       title: '设置任务实际工期(优先计算)',
                       xtype: "panel",
                       id: 'panel2',
                       height: 130,
                       region: 'south',
                       items: [{
                           xtype: 'textfield',
                           labelWidth: 80,
                           allowBlank: false,
                           value: 0,
                           margin: '15 0 0 10',
                           fieldLabel: '实际工期(天)',
                           id: 'actualValue',
                           name: 'actualValue',
                           regex: Reg,
                           regexText: '只能为整数或者小数'
                       }, {
                           xtype: 'text',
                           margin: '15 0 0 10',
                           text: '值为0代表不设置实际工期.'
                       }]
                   }],

                buttons: [{
                    xtype: 'button',
                    text: '确定',
                    id: 'setTask',
                    formBind: true, //only enabled once the form is valid
                    disabled: true,
                    listeners:
                    {
                        click: function (e, eOpts) {
                            var form = this.up('form').getForm();
                            var comb = Ext.getCmp("expressionName" + Auto_id);
                            var a = Ext.getCmp("A").getValue();
                            var b = Ext.getCmp("B").getValue();
                            var c = Ext.getCmp("C").getValue();
                            var actual = Ext.getCmp("actualValue").getValue();
                            var expressionName = comb.getValue();
                            var taskpriority;
                            if (a == '' && comb.getValue() == '固定' && actual == 0) {
                                alert('输入参数不完整');
                                return 0;
                            }
                            if (!(a != '' && b != '') && (comb.getValue() == '正态分布' || comb.getValue() == 'Beta分布') && actual == 0) {
                                alert('输入参数不完整');
                                return 0;
                            }
                            if (!(a != '' && b != '' && c != '') && comb.getValue() == '三角分布' && actual == 0) {
                                alert('输入参数不完整');
                                return 0;
                            }
                            if ((a != '' && b != '' && c != '') && comb.getValue() == '三角分布') {
                                if (!(a - 1 < b - 1 && b - 1 < c - 1)) {
                                    alert('输入有误 ! ,' + '低限<众数<上限');
                                    return 0;
                                }
                            }
                            if (form.isValid()) {
                                form.submit({
                                    params: { Auto_id: Auto_id, expressionName: expressionName },
                                    url: 'SeverRes/Handler.ashx?method=22',
                                    success: function (form, action) {
                                        Ext.Msg.alert('信息', '成功保存设置！');
                                        e.ownerCt.ownerCt.ownerCt.ownerCt.close();
                                        var tabpanel = Ext.getCmp('TabPanel');
                                        var tab = tabpanel.getComponent(parentId);
                                        //定义组件，所有图案都添加其中
                                        var drawComponent = Ext.create('Ext.draw.Component', {
                                            viewBox: false,
                                            height: 500,
                                            width: 600,
                                            renderTo: document.body
                                        }),
                                        surface = drawComponent.surface;//定义画板
                                        receive_and_draw(drawComponent, surface, taskLevel, projectId, parentId);//接收数据并画图,主函数
                                        tab.removeAll();
                                        tab.add(drawComponent);//添加组件
                                    },
                                    failure: function (form, action) {
                                        Ext.Msg.alert('错误', action.result.errMsg);
                                    }
                                });
                            }
                        }
                    }
                }, {
                    xtype: 'button',
                    text: '取消',
                    listeners:
                    {
                        click: function (e, eOpts) {
                            e.ownerCt.ownerCt.ownerCt.ownerCt.close();
                        }
                    }
                }]
            },
                   {
                       xtype: 'TaskAssignment'
                   }]
        }]
    });

    return win;
}

function set_TaskWindow(e, eOpts)//子任务设置属性
{
    var Task_name = e.name;
    var array = new Array();
    array = e.id.split(",");
    var Auto_id = array[0];
    var Task_priority = array[1];
    var ArgA = array[2];
    var ArgB = array[3];
    var ArgC = array[4];
    var ExpressionName = array[5];
    var parentId = array[6];
    var taskLevel = array[7];
    var projectId = array[8];
    var actualValue = array[9];
    var taskwin = create_TaskWin(Auto_id, Task_priority, ArgA, ArgB, ArgC, ExpressionName, parentId, taskLevel, projectId);
    taskwin.setTitle(Task_name);
    taskwin.show();
    if (ExpressionName != 'null') {
        Ext.getCmp('expressionName' + Auto_id).setValue(ExpressionName);
        Ext.getCmp('A').setValue(ArgA);
        Ext.getCmp('B').setValue(ArgB);
        Ext.getCmp('C').setValue(ArgC);
        Ext.getCmp('actualValue').setValue(actualValue);
        Ext.getCmp('taskPriority').setValue(Task_priority);
        if (ExpressionName == '固定') {
            Ext.getCmp('A').setFieldLabel('天数');
            Ext.getCmp('B').setVisible(false);
            Ext.getCmp('B').setDisabled(true);
            Ext.getCmp('C').setVisible(false);
            Ext.getCmp('C').setDisabled(true);
        }
        if (ExpressionName == '三角分布') {
            Ext.getCmp('A').setFieldLabel('低限');
            Ext.getCmp('B').setVisible(true);
            Ext.getCmp('B').setFieldLabel('众数');
            Ext.getCmp('B').setDisabled(false);
            Ext.getCmp('C').setVisible(true);
            Ext.getCmp('C').setFieldLabel('上限');
            Ext.getCmp('C').setDisabled(false);
        }
        if (ExpressionName == 'Beta分布') {
            Ext.getCmp('A').setFieldLabel('α');
            Ext.getCmp('B').setVisible(true);
            Ext.getCmp('B').setFieldLabel('β');
            Ext.getCmp('B').setDisabled(false);
            Ext.getCmp('C').setVisible(false);
            Ext.getCmp('C').setDisabled(true);
        }
        if (ExpressionName == '正态分布') {
            Ext.getCmp('A').setFieldLabel('期望');
            Ext.getCmp('B').setVisible(true);
            Ext.getCmp('B').setFieldLabel('方差');
            Ext.getCmp('B').setDisabled(false);
            Ext.getCmp('C').setVisible(false);
            Ext.getCmp('C').setDisabled(true);
        } 
    }
}

function draw_SummaryTaskandText(task, surface, taskLevel, projectId)//画一个概要任务Task和它的Text
{

    var i = 1;

    if (task.IsDone == true)
        i = 3;
    if (task.Task_is_critical == 1)
        i = 5;

    var Task = surface.add({
        type: 'image',
        //fill: 'rgb(15,105,149)', //blue  
        width: 110,
        height: 68,
        src: '/resources/images/Summarytask' + i + '.png',
        x: task.x - 45,
        y: task.y - 34,
        id: task.Task_id + "," + taskLevel + "," + projectId,
        name: task.Task_name,
        listeners:
            {
                mouseout: {
                    element: 'el', //bind to the underlying el property on the panel
                    fn: function (e) {
                        e.setAttributes({
                            src: '/resources/images/Summarytask' + i + '.png'
                        }, true);
                        e.animate({
                            to: {
                                translate: {
                                    y: -1
                                }
                            },
                            duration: 100
                        });
                    }
                },
                render: {
                    fn: function (e) {
                        var tip = Ext.create('Ext.tip.ToolTip', {
                            target: e.id,
                            html: e.name
                        });
                    }
                },
                mouseover: {
                    element: 'el', //bind to the underlying el property on the panel
                    fn: function (e) {
                        //e.setAttributes({
                        //    src: '/resources/images/Summarytask' + (i+1) + '.png'
                        //}, true);
                        e.animate({
                            to: {
                                translate: {
                                    y: 1
                                }
                            },
                            duration: 100
                        });
                    }
                },
                dblclick: more_Image
            }
    });
    Task.show(true);

    var str = task.Task_name;

    if (str.length >= 6) {
        str = str.substr(0, 5);
        str = str + "..";
    }

    var Text = surface.add({
        type: 'text',
        text: str,
        //        fill: 'black',
        fill: '#f3f4f6',
        font: '14px Adobe Heiti Std',
        x: task.x - 32,
        y: task.y,
        id: task.Task_id + "," + taskLevel + "," + projectId + "," + 'Text',
        name: task.Task_name,
        listeners:
        {
            render: {
                fn: function (e) {
                    var tip = Ext.create('Ext.tip.ToolTip', {
                        target: e.id,
                        html: e.name
                    });
                }
            },
            dblclick: more_Image
        }
    });
    Text.show(true);
}

function draw_SonTaskandText(task, surface, taskLevel, projectId, parentId)//画一个子任务和它的Text
{
    var i = 1;

    if (task.Have_resource == true)
        i = 2;
    else if (task.IsDone == true)
        i = 3;

    if (task.Task_is_critical == 1)
        i = 5;

    var Task = surface.add({
        type: 'image',
        //fill: 'rgb(15,105,149)', //blue  
        x: task.x - 45,
        y: task.y - 34,
        width: 110,
        height: 68,
        src: '/resources/images/task' + i + '.png',
        id: task.Auto_id + "," + task.Task_priority + "," + task.ArgA + "," + task.ArgB + "," + task.ArgC + "," + task.ExpressionName + "," + parentId + "," + taskLevel + "," + projectId + "," + task.actualValue,
        name: task.Task_name,
        listeners:
            {
                mouseout: {
                    element: 'el', //bind to the underlying el property on the panel
                    fn: function (e) {
                        e.setAttributes({
                            src: '/resources/images/task' + i + '.png'
                        }, true);
                        e.animate({
                            to: {
                                translate: {
                                    y: -1
                                }
                            },
                            duration: 100
                        });
                    }
                },
                render: {
                    fn: function (e) {
                        var tip = Ext.create('Ext.tip.ToolTip', {
                            target: e.id,
                            html: e.name
                        });
                    }
                },
                mouseover: {
                    element: 'el', //bind to the underlying el property on the panel
                    fn: function (e) {
                        //e.setAttributes({
                        //    src: '/resources/images/task' + (i+1) + '.png'
                        //}, true);
                        e.animate({
                            to: {
                                translate: {
                                    y: 1
                                }
                            },
                            duration: 100
                        });
                    }
                },
                dblclick: set_TaskWindow
            }
    });
    Task.show(true);
    var str = task.Task_name;

    if (str.length >= 6) {
        str = str.substr(0, 5);
        str = str + "..";
    }
    var Text = surface.add({
        type: 'text',
        text: str,
        //      fill: 'black',
        fill: '#f3f4f6',
        font: '14px Adobe Heiti Std',
        x: task.x - 30,
        y: task.y,
        id: task.Auto_id + "," + task.Task_priority + "," + task.ArgA + "," + task.ArgB + "," + task.ArgC + "," + task.ExpressionName + "," + parentId + "," + taskLevel + "," + projectId + "," + task.actualValue + "," + 'Text',
        name: task.Task_name,
        listeners:
        {
            render: {
                fn: function (e) {
                    var tip = Ext.create('Ext.tip.ToolTip', {
                        target: e.id,
                        html: e.name
                    });
                }
            },
            dblclick: set_TaskWindow
        }
    });
    Text.show(true);
}

function draw_a_Path(start_x, start_y, end_x, end_y, surface, IsRed_path)//画一条路径
{
    var color;
    if (IsRed_path)
        color = 'rgb(250,0,0)';//red
    else
        color = '#1B5A79';//blue

    var Path = surface.add({
        type: "path",
        path: "M " + start_x + " " + start_y + "L " + end_x + " " + end_y,
        stroke: color,
        "stroke-width": 4
    });

    Path.show(true);
}

function draw_allPath(list_link, task_xy, surface) {


    for (var i = 0; i < list_link.length; i++) {
        var parent_id = list_link[i].Task_pre_id;
        var son_id = list_link[i].Task_suc_id;
        var start_x;
        var start_y;
        var end_x;
        var end_y;
        var IsRed_path = false;
        var critical_count = 0;

        for (var j = 0; j < task_xy.length; j++) {
            if (task_xy[j].Task_id == parent_id) {
                start_x = task_xy[j].x;
                start_y = task_xy[j].y;
                if (task_xy[j].Task_is_critical == 1)
                    critical_count++;
            }
            if (task_xy[j].Task_id == son_id) {
                end_x = task_xy[j].x;
                end_y = task_xy[j].y;
                if (task_xy[j].Task_is_critical == 1)
                    critical_count++;
            }
        }
        if (critical_count == 2)
            IsRed_path = true;

        draw_a_Path(start_x, start_y, end_x, end_y, surface, IsRed_path);
    }
}

function draw_allTask(task_xy, surface, taskLevel, projectId, parentId) {
    for (var i = 0; i < task_xy.length; i++) {
        if (task_xy[i].IsSummary)
            draw_SummaryTaskandText(task_xy[i], surface, taskLevel, projectId);
        else
            draw_SonTaskandText(task_xy[i], surface, taskLevel, projectId, parentId);
    }
}

function receive_and_draw(drawComponent, surface, taskLevel, projectId, taskId)//接收数据并画项目图,主函数
{
    var start_x = 50; //起点的x坐标
    var start_y;    //起点的y坐标
    var fix_x = 150; //固定x的增量
    var fix_y = 180; //固定y的增量
    var list_task = new Array();//任务list
    var list_link = new Array();//链接list
    var queue = new Array();//Task的队列，广度搜索
    var task_xy = new Array();//保存Task的x,y坐标
    Ext.Ajax.request({
        url: '/SeverRes/Handler.ashx?method=21' + '&taskLevel=' + taskLevel + '&projectId=' + projectId + '&taskId=' + taskId,
        success: function (resp, opts) {
            var data = Ext.JSON.decode(resp.responseText);//接收数据
            list_task = Ext.JSON.decode(data.task);
            list_link = Ext.JSON.decode(data.link);
            if (list_task != undefined) {
                start_y = compute_start(list_task, start_x, fix_y, queue, task_xy);//计算始点坐标  
                if (list_link != undefined)
                    compute_task_xy(list_task, list_link, fix_x, fix_y, queue, task_xy);//计算非始终点坐标
                setDrawComponent(drawComponent, start_x, task_xy);//设置组件高宽
                if (list_link != undefined)
                    draw_allPath(list_link, task_xy, surface);//画出所有的线
                draw_allTask(task_xy, surface, taskLevel, projectId, taskId);//画出所有的Task
            }
        },
        failure: function (resp, opts) {
            Ext.Msg.alert('错误', '接收数据失败');
        }

    });
}

/*********************************************************************************************/
/****************************Critical********************************************************/
/*********************************************************************************************/

function more__CriticalImage(e, eOpts)//概要任务添加子任务网络图
{
    var Task_name = e.name;
    var array = new Array();
    array = e.id.split(",");
    var method = array[0];
    var Taskid = array[1];
    var taskLevel = array[2];
    taskLevel++;
    var Version_projectId = array[3];
    var simVersionId = array[4];
    var simSwquence = array[5];
    var tabpanel = Ext.getCmp('TabPanel');
    var tab = tabpanel.getComponent(Taskid + 'V');
    if (tab == undefined) {
        var TaskPanel = Ext.create('Ext.panel.Panel', {
            id: Taskid + 'V',
            html: '<img src="/resources/images/surface.png" style="width:100%; height:100%;"/>',
            closable: true,
            autoScroll: true,
            layout: 'absolute'
        });
        TaskPanel.setTitle(Task_name);
        tabpanel.add(TaskPanel);
        tabpanel.setActiveTab(TaskPanel);
        //定义组件，所有图案都添加其中
        var drawComponent = Ext.create('Ext.draw.Component', {
            viewBox: false,
            height: 500,
            width: 600,
            renderTo: document.body
        }),
        surface = drawComponent.surface;//定义画板
        receive_and_drawCritical(method, drawComponent, surface, taskLevel, Version_projectId, Taskid, simVersionId, simSwquence)//接收数据并画图
        TaskPanel.add(drawComponent);//添加组件
    }
    else
        tabpanel.setActiveTab(tab);
}

function draw_CriticalSummaryTaskandText(method, task, surface, taskLevel, Version_projectId, simVersionId, simSwquence)//画一个概要任务Task和它的Text
{
    var i = 1;

    if (task.Task_is_critical == 1)
        i = 5;

    var Task = surface.add({
        type: 'image',
        //fill: 'rgb(15,105,149)', //blue  
        width: 110,
        height: 68,
        src: '/resources/images/Summarytask' + i + '.png',
        x: task.x - 45,
        y: task.y - 34,
        name: task.Task_name,
        id: method + "," + task.Task_id + "," + taskLevel + "," + Version_projectId + "," + simVersionId + "," + simSwquence + "," + 'V',
        listeners:
            {
                mouseout: {
                    element: 'el', //bind to the underlying el property on the panel
                    fn: function (e) {
                        e.setAttributes({
                            src: '/resources/images/Summarytask' + i + '.png'
                        }, true);
                        e.animate({
                            to: {
                                translate: {
                                    y: -1
                                }
                            },
                            duration: 100
                        });
                    }
                },
                render: {
                    fn: function (e) {
                        var tip = Ext.create('Ext.tip.ToolTip', {
                            target: e.id,
                            html: e.name
                        });
                    }
                },
                mouseover: {
                    element: 'el', //bind to the underlying el property on the panel
                    fn: function (e) {
                        //e.setAttributes({
                        //    src: '/resources/images/Summarytask' + (i + 1) + '.png'
                        //}, true);
                        e.animate({
                            to: {
                                translate: {
                                    y: 1
                                }
                            },
                            duration: 100
                        });
                    }
                },
                dblclick: more__CriticalImage
            }
    });
    Task.show(true);
    var str = task.Task_name;

    if (str.length >= 6) {
        str = str.substr(0, 5);
        str = str + "..";
    }
    var Text = surface.add({
        type: 'text',
        text: str,
        name: task.Task_name,
        fill: '#f3f4f6',
        font: '14px Adobe Heiti Std',
        x: task.x - 30,
        y: task.y,
        id: method + "," + task.Task_id + "," + taskLevel + "," + Version_projectId + "," + simVersionId + "," + simSwquence + "," + 'Vtext',
        listeners:
        {
            render: {
                fn: function (e) {
                    var tip = Ext.create('Ext.tip.ToolTip', {
                        target: e.id,
                        html: e.name
                    });
                }
            },
            dblclick: more__CriticalImage
        }
    });
    Text.show(true);
}

function draw_CriticalSonTaskandText(task, surface)//画一个子任务和它的Text
{
    var i = 1;

    if (task.Task_is_critical == 1)
        i = 5;

    var Task = surface.add({
        type: 'image',
        //fill: 'rgb(15,105,149)', //blue  
        x: task.x - 45,
        y: task.y - 34,
        id: 'son' + task.Task_name,
        name: task.Task_name,
        width: 110,
        height: 68,
        src: '/resources/images/task' + i + '.png',
        listeners:
       {
           render: {
               fn: function (e) {
                   var tip = Ext.create('Ext.tip.ToolTip', {
                       target: e.id,
                       html: e.name
                   });
               }
           }
       }
    });
    Task.show(true);
    var str = task.Task_name;

    if (str.length >= 6) {
        str = str.substr(0, 5);
        str = str + "..";
    }
    var Text = surface.add({
        type: 'text',
        id: 'son' + task.Task_name + 'text',
        name: task.Task_name,
        text: str,
        //     fill: 'black',
        fill: '#f3f4f6',
        font: '14px Adobe Heiti Std',
        x: task.x - 30,
        y: task.y,
        listeners:
       {
           render: {
               fn: function (e) {
                   var tip = Ext.create('Ext.tip.ToolTip', {
                       target: e.id,
                       html: e.name
                   });
               }
           }
       }
    });
    Text.show(true);
}

function draw_allCriticalTask(method, task_xy, surface, taskLevel, Version_projectId, simVersionId, simSwquence) {
    for (var i = 0; i < task_xy.length; i++) {
        if (task_xy[i].IsSummary)
            draw_CriticalSummaryTaskandText(method, task_xy[i], surface, taskLevel, Version_projectId, simVersionId, simSwquence);
        else
            draw_CriticalSonTaskandText(task_xy[i], surface);
    }
}

function receive_and_drawCritical(method, drawComponent, surface, taskLevel, Version_projectId, taskId, simVersionId, simSwquence)//接收数据并画图,主函数
{
    var start_x = 50; //起点的x坐标
    var start_y;    //起点的y坐标
    var fix_x = 150; //固定x的增量
    var fix_y = 180; //固定y的增量
    var list_task = new Array();//任务list
    var list_link = new Array();//链接list
    var queue = new Array();//Task的队列，广度搜索
    var task_xy = new Array();//保存Task的x,y坐标
    Ext.Ajax.request({
        url: '/SeverRes/Handler.ashx?method=' + method + '&simVersionId=' + simVersionId + '&simSequence=' + simSwquence + '&taskId=' + taskId + '&taskLevel=' + taskLevel,
        success: function (resp, opts) {
            var data = Ext.JSON.decode(resp.responseText);//接收数据
            list_task = Ext.JSON.decode(data.task);
            list_link = Ext.JSON.decode(data.link);
            var text = Ext.getCmp('maxProjectTime');
            if (data.maxProjectTime!=undefined)
            text.setText('平均工期:' + Ext.JSON.decode(data.maxProjectTime).toFixed(2) + '天');
            if (list_task != undefined) {
                start_y = compute_start(list_task, start_x, fix_y, queue, task_xy);//计算始点坐标  
                if (list_link != undefined)
                    compute_task_xy(list_task, list_link, fix_x, fix_y, queue, task_xy);//计算非始终点坐标
                setDrawComponent(drawComponent, start_x, task_xy);//设置组件高宽
                if (list_link != undefined)
                    draw_allPath(list_link, task_xy, surface);//画出所有的线
                draw_allCriticalTask(method, task_xy, surface, taskLevel, Version_projectId, simVersionId, simSwquence);//画出所有的Task
            }
        },
        failure: function (resp, opts) {
            Ext.Msg.alert('错误', '接收数据失败');
        }

    });
}
Ext.define( 'CosimRisk.view.ProjectImage', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.ProjectImage',
    html: '<img src="/resources/images/surface.png" style="width:100%; height:100%;"/>',
    id: 'ProjectImage',
    bodyStyle: 'background:#E5E5E5;padding:0px',
    autoScroll: true,
    layout: 'absolute'
} );