using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SimulationWithResources
    {
        double totalMoney = CommonMsg.maxNumber;
        double totoalDuration = CommonMsg.maxNumber;
        MODEL.Sequence best_seq = null;
        List<MODEL.risk_task_instance_res> total_wait_resource =  new  List<MODEL.risk_task_instance_res>();
        List<MODEL.risk_prj_instance_res> total_prj_resource = new List<MODEL.risk_prj_instance_res> ();
        #region 计算给定taskList的所有拓扑排序
        public static int getTasklistIndegree(List<MODEL.risk_task> taskList)//统计总拓扑排序数sum
        {
            int sum = 0;
            foreach (MODEL.risk_task task in taskList)
            {
                sum += task.InDegree;
            }
            return sum;
        }
        public static int calTopsort(List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList, List<MODEL.Sequence> priorityArray, MODEL.Sequence _curSeq)
        {
            List<MODEL.risk_task> callBackList = new List<MODEL.risk_task>();

            for (int i = 0; i < taskList.Count; ++i)
            {
                if (taskList[i].InDegree == 0 && !taskList[i].Finished)
                {
                    callBackList.Add(taskList[i]);
                }
            }

            for (int i = 0; i < callBackList.Count; ++i)
            {
                MODEL.risk_task myTask = callBackList[i];
                MODEL.Sequence curSeq = new MODEL.Sequence(_curSeq);
                curSeq.Array.Add(myTask.Task_id);
                curSeq.Array_prio.Add(myTask.Task_priority);
                myTask.Finished = true;
                for (int j = 0; j < myTask.OutDegree; ++j)
                {
                    MODEL.risk_task succeedTask = null;
                    foreach (MODEL.risk_task t in taskList)
                    {
                        if (t.Task_id == myTask.SucceedTaskId[j])
                            succeedTask = t;
                    }
                    succeedTask.InDegree--;
                }
                calTopsort(taskList, linkList, priorityArray, curSeq);
                myTask.Finished = false;
                for (int j = 0; j < myTask.OutDegree; ++j)
                {
                    MODEL.risk_task succeedTask = null;
                    foreach (MODEL.risk_task t in taskList)
                    {
                        if (t.Task_id == myTask.SucceedTaskId[j])
                            succeedTask = t;
                    }
                    succeedTask.InDegree++;
                }
            }
            if (getTasklistIndegree(taskList) == 0 && _curSeq.Array.Count == taskList.Count)
            {
                priorityArray.Add(_curSeq);
            }
            return 1;
        }

        public static List<MODEL.Sequence> calPrioritySequence(List<MODEL.risk_task> _taskList, List<MODEL.risk_link> linkList)
        {
            List<MODEL.Sequence> priorityArray = new List<MODEL.Sequence>();
            try
            {
                List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
                foreach (MODEL.risk_task task in _taskList)
                {
                    taskList.Add(task.Clone() as MODEL.risk_task);
                }
                
                MODEL.Sequence _curSeq = new MODEL.Sequence();
                List<MODEL.risk_task> callBackList = new List<MODEL.risk_task>();
                for (int i = 0; i < taskList.Count; ++i)
                {
                    if (taskList[i].InDegree == 0 && !taskList[i].Finished)
                    {
                        callBackList.Add(taskList[i]);
                    }
                }
                for (int i = 0; i < callBackList.Count; ++i)
                {
                    MODEL.risk_task myTask = callBackList[i];
                    MODEL.Sequence curSeq = new MODEL.Sequence(_curSeq);
                    curSeq.Array.Add(myTask.Task_id);
                    curSeq.Array_prio.Add(myTask.Task_priority);
                    myTask.Finished = true;
                    for (int j = 0; j < myTask.OutDegree; ++j)
                    {
                        MODEL.risk_task succeedTask = null;
                        foreach (MODEL.risk_task t in taskList)
                        {
                            if (t.Task_id == myTask.SucceedTaskId[j])
                                succeedTask = t;
                        }
                        succeedTask.InDegree--;
                    }
                    calTopsort(taskList, linkList, priorityArray, curSeq);
                    myTask.Finished = false;
                    for (int j = 0; j < myTask.OutDegree; ++j)
                    {
                        MODEL.risk_task succeedTask = null;
                        foreach (MODEL.risk_task t in taskList)
                        {
                            if (t.Task_id == myTask.SucceedTaskId[j])
                                succeedTask = t;
                        }
                        succeedTask.InDegree++;
                    }
                }
                for (int i = 0; i < callBackList.Count; ++i)
                {
                    callBackList[i].Finished = true;
                }

                return priorityArray;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 计算TaskValue
        public bool CalTaskValue(List<MODEL.risk_task> _taskList)
        {
            foreach (MODEL.risk_task task in _taskList)
            {
                MODEL.risk_math_expression_arg expression = CommonArugment.getExpression.getExpression(task.Auto_id);
                if (expression.Actual_value != 0)
                {
                    task.Value = expression.Actual_value;
                    continue;
                }
                double[] values = null;
                try
                {
                    values = (double[])CommonUtils.Deserialize(expression.Value);
                }
                catch (Exception ex)
                {
                    CommonMsg.errMsg = ex.Message + "->有的任务结点还没有设置属性，无法进行仿真";
                    return false;
                }
                switch (expression.Name)
                {
                    case "三角分布":
                        task.Value = Math.Round(CommonUtils.triangleDistri(values[0], values[1], values[2]), 2);
                        break;
                    case "正态分布":
                        task.Value = Math.Round(CommonUtils.normDistri(values[0], values[1]), 2);
                        break;
                    case "beta分布":
                        task.Value = 10;
                        break;
                    case "固定":
                        task.Value = Math.Round(values[0], 2);
                        break;
                    default:
                        CommonMsg.errMsg = "仿真遇到未知的分布";
                        return false;
                }
            }
            return true;
        }
        #endregion

        #region 计算一次仿真的关键路径(带资源)
        /*
         * 没有考虑优先级
         * 节点全部都展开
         * 返回的是最短的时间，等待时间，最优序列
         * status: 0->等待状态 1->就绪状态 -1->完毕状态  2->有资源等待
         */

        public bool calSimShortestPath(int projectId, List<MODEL.risk_task> _taskList, List<MODEL.risk_link> _linkList, List<MODEL.Sequence> priorityArray, List<MODEL.risk_task> _taskStarttimeList)
        {
        
            if (!CalTaskValue(_taskList))//计算TaskValue
                return false;

            int xuanxuan = 0;
            foreach (MODEL.Sequence seq in priorityArray)//计算最优序列
            {
                xuanxuan++;
                //保存项目资源信息
                List<MODEL.risk_project_res_assignment> prjResAssignList = CommonArugment.getPrjResAssign.getPrjResAssignListByPrjId(projectId);
                List<MODEL.risk_task_resource_assignment> taskResAssignList = CommonArugment.getTaskResAssign.getTaskResAssignListByPrjId(projectId);
                double Money = 0;
                double Duration = 0;
                bool canWork = true;
                int[] pArray = seq.Array.ToArray();
                List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
                List<MODEL.risk_task> taskStarttimeList = new List<MODEL.risk_task>();
                List<MODEL.risk_task_instance_res> task_wait_resource = new List<MODEL.risk_task_instance_res>();
                List<MODEL.risk_prj_instance_res> prj_resource = new List<MODEL.risk_prj_instance_res>();
                foreach (MODEL.risk_task task in _taskList)
                {
                    taskList.Add(task.Clone() as MODEL.risk_task);
                    taskStarttimeList.Add(task.Clone() as MODEL.risk_task);
                }

                //寻找入度为0的点
                List<MODEL.risk_task> taskReadyQueue = new List<MODEL.risk_task>();
                List<MODEL.risk_task> callBackList = new List<MODEL.risk_task>();
                while (canWork)
                {
                    for (int i = 0; i < taskList.Count; ++i)//添加入为0的点
                    {
                        if (taskList[i].InDegree == 0 && taskList[i].Status == 0)
                        {
                            callBackList.Add(taskList[i]);
                        }
                    }
                    for (int i = 0; i < pArray.Length; ++i) //按照顺序分配资源
                    {
                        for (int j = 0; j < callBackList.Count; ++j)
                        {
                            if (callBackList[j].Task_id != pArray[i])
                                continue;
                            MODEL.risk_task myTask = callBackList[j];
                            //给myTask分配资源，status=1                           
                            if (CommonUtils.allocateResource(projectId, myTask, task_wait_resource))
                            {
                                myTask.Status = 1;//资源够了，变成就绪态
                                taskReadyQueue.Add(myTask);
                                for (int k = 0; k < myTask.OutDegree; k++)
                                    {
                                        foreach (MODEL.risk_task t in taskList)
                                        {
                                            if (t.Task_id == myTask.SucceedTaskId[k])
                                            {
                                                t.InDegree--;

                                            }
                                        }
                                    }
                                callBackList.Remove(myTask);
                            }
                        }
                    }
                    if (taskReadyQueue.Count != 0)//至少有一个是能run
                    {
                        //找到value最小的那个一个
                        double minValue = CommonMsg.maxNumber;

                        foreach (MODEL.risk_task task in taskReadyQueue)
                        {
                            if (task.StartTime == -1)
                            {
                                task.StartTime = Duration;
                                foreach(MODEL.risk_task t in taskStarttimeList)
                                {
                                    if (task.Auto_id == t.Auto_id)
                                        t.StartTime = task.StartTime;
                                }
                            }

                            if (task.Value < minValue)
                                minValue = task.Value;
                        }
                        Duration += minValue;
                        foreach (MODEL.risk_task wait_task in callBackList)//记录等待时间
                        {
                            foreach (MODEL.risk_task_instance_res res in task_wait_resource)
                            {
                                if (wait_task.Auto_id == res.Task_auto_id)
                                {
                                    res.Wait_time += minValue;
                                }
                            }
                        }
                        for (int i = 0; i < taskReadyQueue.Count; ++i)
                        {

                            if (taskReadyQueue[i].Value <= minValue)//能执行完的执行
                            {
                                Money += CommonUtils.getMoney(taskReadyQueue[i]);//算钱
                                CommonUtils.recycleResource(projectId, taskReadyQueue[i], prj_resource);//回收
                                taskReadyQueue[i].Status = -1;
                                taskReadyQueue.RemoveAt(i--);//去除
                            }
                            else //执行一部分
                            {
                                taskReadyQueue[i].Value -= minValue;
                            }
                        }
                    }
                    else if (callBackList.Count == 0)//完成
                    {
                        if (totoalDuration > Duration)
                        {
                            totalMoney = Money;
                            totoalDuration = Duration;
                            best_seq = seq;
                            _taskStarttimeList.Clear();
                            foreach (MODEL.risk_task  t in taskStarttimeList)
                                _taskStarttimeList.Add(t);

                            total_wait_resource.Clear();
                            foreach (MODEL.risk_task_instance_res res in task_wait_resource)
                                total_wait_resource.Add(res);

                            total_prj_resource.Clear();
                            foreach (MODEL.risk_prj_instance_res res in prj_resource)
                                total_prj_resource.Add(res); 
                        }
                        //还原项目资源信息
                        foreach (MODEL.risk_project_res_assignment prjResAssign in prjResAssignList)
                        {
                            CommonArugment.getPrjResAssign.updateByPrjIdResId(prjResAssign);
                        }
                        canWork = false;
                    }
                    else//失败，资源不足，卡住了
                    {
                        canWork = false;
                        //还原项目资源信息
                        foreach (MODEL.risk_project_res_assignment prjResAssign in prjResAssignList)
                        {
                            CommonArugment.getPrjResAssign.updateByPrjIdResId(prjResAssign);
                        }
                        foreach (MODEL.risk_task_resource_assignment taskResAssgin in taskResAssignList)
                        {
                            CommonArugment.getTaskResAssign.updateByTaskAutoIdResId(taskResAssgin);
                        }
                        foreach (MODEL.risk_task_instance_res res in task_wait_resource)
                        {
                            if (callBackList[0].Auto_id == res.Task_auto_id)
                            {
                                CommonMsg.errMsg = "节点 "+callBackList[0].Task_name + " 所需资源 "+CommonArugment.getResources.findResById(res.Resource_id).Resource_name+" 无法满足,请增加项目资源";
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region 检查优先级 
        public void check_priorityArray(List<MODEL.Sequence> priorityArray)
        {
            List<MODEL.Sequence> list = new List<MODEL.Sequence>();
             foreach(MODEL.Sequence sec in priorityArray)
            {
                list.Add(sec);
            }
             foreach (MODEL.Sequence sec in list)
            {
                int now_max_priority = 99;
                bool check = false;
                int []pri = sec.Array_prio.ToArray();
                for (int i = 0; i < pri.Length; i++)
                {
                    if (pri[i] <= now_max_priority)
                        now_max_priority = pri[i];
                    else
                    {
                        check = true;
                        break;
                    }
                }
                if (check)
                    priorityArray.Remove(sec);
            }
        }
        #endregion

        #region 带资源的仿真（总）

        public bool SimCriticalResource(int projectId, int times, string description, int durationMax, int have_resource)
        {
            DateTime startToSimulateTime = DateTime.Now;
            int versionId = -1;
            //establish version information
            MODEL.risk_prj_version_info projectVersionInfo = new MODEL.risk_prj_version_info(projectId, description, times, startToSimulateTime, startToSimulateTime, have_resource);
            versionId = CommonArugment.getProjectVersionInfo.savePrjVersionInfo(projectVersionInfo);//插入prj_versioninfo
            if (versionId == -1)
            {
                CommonMsg.errMsg = "无法保存项目的版本信息";
                return false;
            }
            projectVersionInfo.Sim_version_id = versionId;
            List<MODEL.risk_task> _taskList = CommonArugment.getTask.getTaskList(projectId);
            List<MODEL.risk_link> _linkList = CommonArugment.getTask.getLinkList(projectId);
            List<MODEL.risk_task> _taskStarttimeList = new List<MODEL.risk_task>();
            CommonUtils.unionTaskList(_taskList, _linkList);//展开所有节点
            List<MODEL.Sequence> priorityArray = calPrioritySequence(_taskList, _linkList);//资源序列
            check_priorityArray(priorityArray);//检查优先级
           
            for (int i = 0; i < times; i++)//资源仿真主干循环
            {
                BLL.SimulationWithResources sim = new SimulationWithResources();

                sim.calSimShortestPath(projectId, _taskList, _linkList, priorityArray, _taskStarttimeList);//计算最优序列
                if (sim.best_seq == null)
                {
                    CommonArugment.getProjectVersionInfo.deletePrjVersionInfo(versionId);
                    return false;
                }
                MODEL.risk_prj_instance projectInstance = new MODEL.risk_prj_instance();
                projectInstance.Sim_sequence = i;
                projectInstance.Sim_version = versionId;
                projectInstance.Sim_project_cost = sim.totalMoney;
                projectInstance.Auto_id = CommonArugment.getProjectInstance.save(projectInstance);//插入instance
                foreach (MODEL.risk_prj_instance_res res in sim.total_prj_resource)//插入instance_res
                {
                    res.Instance_id = projectInstance.Auto_id;
                    CommonArugment.getProjectInstance.saveResource(res);
                }

 /**********************************************************************************************
                以上为资源特殊
 * ***********************************************************************************************/

                List<MODEL.risk_task> reversedTaskList = new List<MODEL.risk_task>();
                CommonMsg.taskList = CommonArugment.getTask.getTaskList(projectId);
                List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
                CommonMsg.linkList = CommonArugment.getTask.getLinkList(projectId);

                foreach (MODEL.risk_task task in _taskList)//taskvaule+
                {
                    double max_wait = 0;
                    foreach (MODEL.risk_task_instance_res res in sim.total_wait_resource)
                    {
                        if (task.Auto_id == res.Task_auto_id && max_wait < res.Wait_time)
                            max_wait = res.Wait_time;
                    }
                    task.Value += max_wait;
                    foreach (MODEL.risk_task t in CommonMsg.taskList)
                    {
                        if (task.Auto_id == t.Auto_id)
                            t.Value = task.Value;
                    }
                }
                foreach (MODEL.risk_task task in _taskStarttimeList)//,starttime+
                {
                    foreach (MODEL.risk_task t in CommonMsg.taskList)
                    {
                        if (task.Auto_id == t.Auto_id)
                            t.StartTime = task.StartTime;
                    }
                }
/**********************************************************************************************
                                下面为一般
 ************************************************************************************************/
                foreach (MODEL.risk_task task in CommonMsg.taskList)
                {
                    if (task.Task_level == 0)
                    {
                        taskList.Add(task);
                    }
                }
                foreach (MODEL.risk_task task in taskList)
                {
                    if (task.Task_is_summary)
                    {
                        task.Value = CommonUtils.calItsCriticalRoute_res(task.Task_id, versionId, i, sim.total_wait_resource);
                    }
                } 
                for (int j = 0; j < taskList.Count; ++j)
                {
                    taskList[j].PreTaskId = new int[taskList.Count];
                    taskList[j].SucceedTaskId = new int[taskList.Count];
                    taskList[j].Vl = CommonMsg.maxNumber;
                }

                //      cal each in/out degree of the task
                foreach (MODEL.risk_link link in CommonMsg.linkList)
                {
                    int outDot = link.Task_pre_id;
                    int inDot = link.Task_suc_id;
                    bool outDone = false;
                    bool inDone = false;

                    foreach (MODEL.risk_task task in taskList)
                    {
                        if (task.Task_id == outDot)
                        {
                            task.SucceedTaskId[task.OutDegree++] = inDot;
                            outDone = true;
                        }
                        if (task.Task_id == inDot)
                        {
                            task.PreTaskId[task.InDegree++] = outDot;
                            inDone = true;
                        }
                        if (outDone && inDone)
                            break;
                    }
                }
                foreach (MODEL.risk_task task in taskList)
                {
                    task.CalInDegree = task.InDegree;
                    task.CalOutDegree = task.OutDegree;
                }

                foreach (MODEL.risk_task task in taskList)
                {
                    reversedTaskList.Add(task.Clone() as MODEL.risk_task);
                }

                //start to topsort
                Stack<MODEL.risk_task> q = new Stack<MODEL.risk_task>();
                foreach (MODEL.risk_task task in taskList)
                {
                    task.Ve = 0;
                    if (task.CalInDegree == 0)
                    {
                        q.Push(task);
                    }
                }
                while (q.Count != 0)
                {
                    MODEL.risk_task task = q.Pop();

                    for (int j = 0; j < task.CalOutDegree; ++j)
                    {
                        MODEL.risk_task succeedTask = null;
                        foreach (MODEL.risk_task t in taskList)
                        {
                            if (t.Task_id == task.SucceedTaskId[j])
                                succeedTask = t;
                        }
                        succeedTask.CalInDegree--;
                        if (succeedTask.CalInDegree == 0)
                        {
                            q.Push(succeedTask);
                        }
                        if (succeedTask.Ve < task.Ve + task.Value)
                        {
                            succeedTask.Ve = task.Ve + task.Value;
                        }
                    }
                    if (q.Count == 0)
                    {
                        projectInstance.Sim_project_time = task.Ve + task.Value;
                    }
                }

                foreach (MODEL.risk_task task in taskList)
                {
                    for (int j = 0; j < reversedTaskList.Count; ++j)
                    {
                        if (reversedTaskList[j].Task_id == task.Task_id)
                        {
                            reversedTaskList[j].Ve = task.Ve;
                        }
                    }
                }
                //topsort again o(∩_∩)o
                for (int j = 0; j < reversedTaskList.Count; ++j)
                {
                    reversedTaskList[j].Vl = CommonMsg.maxNumber;
                    if (reversedTaskList[j].CalOutDegree == 0)
                    {
                        q.Push(reversedTaskList[j]);
                        reversedTaskList[j].Vl = taskList[j].Ve;
                    }
                }
                while (q.Count != 0)
                {
                    MODEL.risk_task task = q.Pop();
                    for (int j = 0; j < task.CalInDegree; ++j)
                    {
                        MODEL.risk_task preTask = null;
                        foreach (MODEL.risk_task t in reversedTaskList)
                        {
                            if (t.Task_id == task.PreTaskId[j])
                                preTask = t;
                        }
                        preTask.CalOutDegree--;
                        if (preTask.CalOutDegree == 0)
                        {
                            q.Push(preTask);
                        }
                        if (preTask.Vl > task.Vl - preTask.Value)
                        {
                            preTask.Vl = Math.Round(task.Vl - preTask.Value, 2);
                        }
                    }
                    if (q.Count == 0)
                    {
                        MODEL.risk_task ddd = task;
                    }
                }

                for (int j = 0; j < taskList.Count; ++j)
                {
                    MODEL.risk_task_instance taskInstance = new MODEL.risk_task_instance(taskList[j].Auto_id, taskList[j].Task_project_id, taskList[j].Value, i, versionId);
                    taskInstance.Task_ve = taskList[j].Ve;
                    taskInstance.Starttime = Math.Round(taskList[j].StartTime,2);
                    taskInstance.Task_vl = reversedTaskList[j].Vl;

                    if (Math.Abs(taskInstance.Task_ve - taskInstance.Task_vl) <= 0.001)
                    {
                        taskInstance.Task_is_critical = 1;
                    }
                    else taskInstance.Task_is_critical = 0;
                    int task_instance_id = CommonArugment.getTaskIntance.save(taskInstance);
                    foreach (MODEL.risk_task_instance_res res in sim.total_wait_resource)
                    {
                        if (res.Task_auto_id == taskList[j].Auto_id)
                        {
                            res.Instance_id = task_instance_id;
                            CommonArugment.getTaskIntance.saveResource(res);
                        }
                    }
                }
                CommonArugment.getProjectInstance.update(projectInstance);
            }

            DateTime endSimulateTime = DateTime.Now;
            TimeSpan ts = new TimeSpan(startToSimulateTime.Ticks).Subtract(new TimeSpan(endSimulateTime.Ticks)).Duration();
            CommonMsg.simulateTime = ts.Hours + "小时 " + ts.Minutes + "分 " + ts.Seconds + "秒 " + ts.Milliseconds + "毫秒";
            projectVersionInfo.Sim_endtime = endSimulateTime;
            CommonArugment.getProjectVersionInfo.updatePrjVersionInfo(projectVersionInfo);
            return true;

        }
        #endregion
    }
}
