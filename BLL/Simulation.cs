using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Simulation
    {
        public static bool calCriticalRouteWithOldMethod(int projectId, int times, string description, int durationMax, int have_resource)
        {
            int versionId = -1;
            try
            {
                DateTime startToSimulateTime = DateTime.Now;

                //establish version information
                MODEL.risk_prj_version_info projectVersionInfo = new MODEL.risk_prj_version_info(projectId, description, times, startToSimulateTime, startToSimulateTime, have_resource);
                versionId = CommonArugment.getProjectVersionInfo.savePrjVersionInfo(projectVersionInfo);
                if (versionId == -1)
                {
                    CommonMsg.errMsg = "无法保存项目的版本信息";
                    return false;
                }
                projectVersionInfo.Sim_version_id = versionId;
                for (int i = 0; i < times; ++i)
                {
                    //establish project instance
                    MODEL.risk_prj_instance projectInstance = new MODEL.risk_prj_instance();
                    projectInstance.Sim_sequence = i;
                    projectInstance.Sim_version = versionId;
                    projectInstance.Sim_project_cost = 0;
                    projectInstance.Auto_id = CommonArugment.getProjectInstance.save(projectInstance);
                    CommonMsg.taskList = CommonArugment.getTask.getTaskList(projectId);
                    CommonMsg.linkList = CommonArugment.getTask.getLinkList(projectId);
                    foreach (MODEL.risk_task task in CommonMsg.taskList)
                    {
                        if (task.Task_is_summary)
                            continue;
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
                    //start to simulate
                    List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();

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
                            task.Value = CommonUtils.calItsCriticalRoute(task.Task_id, versionId, i);
                        }
                    }

                    List<MODEL.risk_task> reversedTaskList = new List<MODEL.risk_task>();
                    for (int j = 0; j < taskList.Count; ++j)
                    {
                        taskList[j].PreTaskId = new int[taskList.Count];
                        taskList[j].SucceedTaskId = new int[taskList.Count];
                        taskList[j].Vl = CommonMsg.maxNumber;
                    }

                    //cal each in/out degree of the task
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
                        taskInstance.Task_vl = reversedTaskList[j].Vl;

                        if (Math.Abs(taskInstance.Task_ve - taskInstance.Task_vl) <= 0.001)
                        {
                            taskInstance.Task_is_critical = 1;
                        }
                        else taskInstance.Task_is_critical = 0;
                        CommonArugment.getTaskIntance.save(taskInstance);
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
            catch (System.Exception ex)
            {
                if (versionId != -1)
                {
                    CommonArugment.getProjectVersionInfo.deletePrjVersionInfo(versionId);
                }
                throw ex;
            }

        }

        /// <summary>
        /// 支持任务四种逻辑关系的仿真算法
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="times"></param>
        /// <param name="description"></param>
        /// <param name="durationMax"></param>
        /// <param name="have_resource"></param>
        /// <returns></returns>
        public static bool calCriticalRoute(int projectId, int times, string description, int durationMax, int have_resource)
        {
            int versionId = -1;
            try
            {
                DateTime startToSimulateTime = DateTime.Now;

                //establish version information
                MODEL.risk_prj_version_info projectVersionInfo = new MODEL.risk_prj_version_info(projectId, description, times, startToSimulateTime, startToSimulateTime, have_resource);
                versionId = CommonArugment.getProjectVersionInfo.savePrjVersionInfo(projectVersionInfo);
                if (versionId == -1)
                {
                    CommonMsg.errMsg = "无法保存项目的版本信息";
                    return false;
                }
                projectVersionInfo.Sim_version_id = versionId;
                List<MODEL.risk_task> _taskList = CommonArugment.getTask.getTaskList(projectId);
                List<MODEL.risk_link> _linkList = CommonArugment.getTask.getLinkList(projectId);
                for (int i = 0; i < times; ++i)
                {
                    //establish project instance
                    MODEL.risk_prj_instance projectInstance = new MODEL.risk_prj_instance();
                    projectInstance.Sim_sequence = i;
                    projectInstance.Sim_version = versionId;
                    projectInstance.Sim_project_cost = 0;
                    projectInstance.Auto_id = CommonArugment.getProjectInstance.save(projectInstance);
                    List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
                    List<MODEL.risk_link> linkList = new List<MODEL.risk_link>();
                    foreach (MODEL.risk_task task in _taskList)
                    {
                        taskList.Add(task.Clone() as MODEL.risk_task);
                    }
                    foreach (MODEL.risk_link link in _linkList)
                    {
                        linkList.Add(link.Clone() as MODEL.risk_link);
                    }
                    CommonUtils.unionTaskList(taskList, linkList);
                    foreach (MODEL.risk_task task in taskList)
                    {
                        if (task.Task_is_summary)
                            continue;
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
                    //start to simulate
                    foreach (MODEL.risk_link link in linkList)
                    {
                        if (link.Link_type == 1 || link.Link_type == 2)
                        {
                            MODEL.risk_task task = taskList.Find(c => c.Task_id == link.Task_suc_id);
                            task.ExpS++;
                        }
                        else
                        {
                            MODEL.risk_task task = taskList.Find(c => c.Task_id == link.Task_suc_id);
                            task.ExpE++;
                        }
                    }

                    int stopTaskNum = 0;
                    while (true)
                    {
                        if (stopTaskNum == taskList.Count)
                            break;
                        for (int j = 0; j < taskList.Count; ++j)
                        {
                            if (taskList[j].NowStatus == 0)//若任务还未开始
                            {
                                if (taskList[j].ExpS == taskList[j].AccS)
                                {
                                    taskList[j].NowStatus = 1;
                                    taskList[j].StartTime = projectInstance.Sim_project_time;
                                    foreach (MODEL.risk_link link in linkList)//找后继任务
                                    {
                                        if (link.Task_pre_id == taskList[j].Task_id)
                                        {
                                            MODEL.risk_task sucTask = taskList.Find(c => c.Task_id == link.Task_suc_id);
                                            if (link.Link_type == 2)
                                            {
                                                sucTask.AccS++;
                                            }
                                            else if (link.Link_type == 4)
                                            {
                                                sucTask.AccE++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        double minTime = 99999999;
                        for (int j = 0; j < taskList.Count; ++j)
                        {
                            if (taskList[j].NowStatus == 1)//若任务正在进行中
                            {
                                if (minTime > taskList[j].Value - taskList[j].Prog)
                                {
                                    minTime = taskList[j].Value - taskList[j].Prog;
                                }
                            }
                        }
                        //将时间往前推进
                        projectInstance.Sim_project_time += minTime;
                        //将所有进行中的任务时间往前推进
                        for (int j = 0; j < taskList.Count; ++j)
                        {
                            if (taskList[j].NowStatus == 1)//若任务正在进行中
                            {
                                taskList[j].Prog += minTime;
                                if (taskList[j].Value <= taskList[j].Prog)//任务做够时间了
                                {
                                    if (taskList[j].AccE == taskList[j].ExpE)//任务可以结束了
                                    {
                                        taskList[j].NowStatus = 3;//任务结束
                                        taskList[j].EndTime = projectInstance.Sim_project_time;
                                        ++stopTaskNum;
                                        foreach (MODEL.risk_link link in linkList)//找后继任务
                                        {
                                            if (link.Task_pre_id == taskList[j].Task_id)
                                            {
                                                MODEL.risk_task sucTask = taskList.Find(c => c.Task_id == link.Task_suc_id);
                                                if (link.Link_type == 1)
                                                {
                                                    sucTask.AccS++;
                                                }
                                                else if (link.Link_type == 3)
                                                {
                                                    sucTask.AccE++;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }

                    }
                    //从结束任务倒推找关键任务
                    Stack<MODEL.risk_task> q = new Stack<MODEL.risk_task>();
                    CommonUtils.calInOutDegree(taskList, linkList);
                    foreach (MODEL.risk_task task in taskList)
                    {
                        if (task.OutDegree == 0)
                        {
                            task.Task_is_critical = 1;
                            q.Push(task);
                        }
                    }
                    while (q.Count != 0)
                    {
                        MODEL.risk_task task = q.Pop();
                        MODEL.risk_task criticalTask = null;
                        for (int j = 0; j < task.InDegree; ++j)
                        {
                            MODEL.risk_task preTask = taskList.Find(c => c.Task_id == task.PreTaskId[j]);
                            preTask.OutDegree--;
                            if (preTask.OutDegree == 0)
                            {
                                q.Push(preTask);
                            }
                            if (criticalTask == null || preTask.EndTime > criticalTask.EndTime)
                            {
                                criticalTask = preTask;
                            }
                        }
                        if (task.Task_is_critical == 1 && criticalTask != null)
                        {
                            criticalTask.Task_is_critical = 1;
                        }
                    }

                    List<MODEL.risk_task> resultTaskList = new List<MODEL.risk_task>();
                    foreach (MODEL.risk_task task in _taskList)
                    {
                        resultTaskList.Add(task.Clone() as MODEL.risk_task);
                    }
                    for (int j = 0; j < resultTaskList.Count; ++j)
                    {
                        if (resultTaskList[j].Task_is_summary)
                        {
                            resultTaskList[j].Value = CommonUtils.calTotalTaskValue(taskList, resultTaskList[j]);
                            resultTaskList[j].Task_is_critical = CommonUtils.isSummaryTaskCritical(taskList, resultTaskList[j]);
                        }
                        else
                        {
                            MODEL.risk_task task = taskList.Find(c => c.Task_id == resultTaskList[j].Task_id);
                            resultTaskList[j].Value = task.Value;
                            resultTaskList[j].Task_is_critical = task.Task_is_critical;
                        }
                    }
                    for (int j = 0; j < resultTaskList.Count; ++j)
                    {
                        MODEL.risk_task_instance taskInstance = new MODEL.risk_task_instance(resultTaskList[j].Auto_id, resultTaskList[j].Task_project_id, resultTaskList[j].Value, i, versionId);
                        taskInstance.Task_is_critical = resultTaskList[j].Task_is_critical;
                        CommonArugment.getTaskIntance.save(taskInstance);
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
            catch (System.Exception ex)
            {
                if (versionId != -1)
                {
                    CommonArugment.getProjectVersionInfo.deletePrjVersionInfo(versionId);
                }
                throw ex;
            }

        }

        public static List<MODEL.risk_prj_version_info> getSimulationVersion()
        {
            try
            {
                return CommonUtils.getAllPrjVInfo();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        private static double findCompressDuration(List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList, int startNode)
        {
            return 0;
        }

        public static bool calCompressStrategy(int simVersionId, int estimatedDuration)
        {
            try
            {
                MODEL.risk_prj_version_info projectVersionInfo = CommonArugment.getProjectVersionInfo.getPrjVersionInfo(simVersionId);
                List<MODEL.risk_task> taskList = CommonUtils.getAllTasks(projectVersionInfo.Pri_id);
                List<MODEL.risk_link> linkList = CommonUtils.getAllLinks(projectVersionInfo.Pri_id);
                CommonUtils.calInOutDegree(taskList, linkList);
                //确定默认方案是平均工期
                for (int j = 0; j < taskList.Count; ++j)
                {
                    taskList[j].AverageProjectTime = CommonArugment.getTaskIntance.getSum(simVersionId, taskList[j].Auto_id) / projectVersionInfo.Count;
                }
                //找关键路径
                bool endLoop = false;
                while (!endLoop)
                {
                    endLoop = true;
                    int maxIndex = 0;
                    double maxRatio = 0;
                    for (int i = 0; i < taskList.Count; ++i)
                    {
                        if (!taskList[i].Finished && taskList[i].InDegree == 0)
                        {
                            endLoop = false;
                            taskList[i].Finished = true;
                            int itsMaxSucceedSize = taskList[i].MaxSucceedSize;
                            foreach (MODEL.risk_link link in linkList)
                            {
                                if (link.Task_pre_id == taskList[i].Task_id)
                                {
                                    for (int j = 0; j < taskList.Count; ++j)
                                    {
                                        if (taskList[j].Task_id == link.Task_suc_id)
                                        {
                                            --taskList[j].InDegree;
                                            taskList[j].MaxSucceedSize += itsMaxSucceedSize;
                                        }
                                    }
                                }
                            }
                            double curRatio = (double)CommonArugment.getTaskIntance.getCriticalSum(simVersionId, taskList[i].Auto_id) / projectVersionInfo.Count;
                            if (curRatio > maxRatio)
                            {
                                maxRatio = curRatio;
                                maxIndex = i;
                            }
                        }
                    }
                    taskList[maxIndex].Task_is_critical = 1;
                }

                CommonUtils.calInOutDegree(taskList, linkList);
                int averageDuration = Convert.ToInt32(Math.Ceiling(CommonArugment.getProjectInstance.getTotalDuration(simVersionId)));
                int shrinkDuration = 0;
                //开始压缩工期
                while (shrinkDuration < averageDuration - estimatedDuration)
                {
                    for (int i = 0; i < taskList.Count; ++i)
                    {
                        MODEL.risk_task task = taskList[i];
                        if (task.InDegree == 0)
                        {
                            int outDegree = task.OutDegree;
                            if (outDegree > 1)   //并行的点，可以压缩哦
                            {
                                bool[,] searchArray = new bool[outDegree, taskList.Count];
                                for (int id = 0; id < outDegree; ++id)
                                {
                                    for (int jd = 0; jd < taskList.Count; ++jd)
                                    {
                                        searchArray[id, jd] = false;
                                    }
                                }
                                //开始压缩关键路径上的点
                                foreach (MODEL.risk_link link in linkList)
                                {
                                    if (link.Task_pre_id == task.Task_id)//应该有outDegree条link
                                    {
                                        double curDuration = 0;
                                        for (int j = 0; j < taskList.Count; ++j)
                                        {
                                            if (taskList[j].Task_id == link.Task_suc_id)
                                            {
                                                curDuration = findCompressDuration(taskList, linkList, j);
                                                //         searchArray[,j]=true;
                                                --taskList[j].InDegree;
                                                if (taskList[j].Task_is_critical == 1)
                                                {

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
