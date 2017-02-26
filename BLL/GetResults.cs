using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace BLL
{
    public class GetResults
    {
        public static bool getCriticalRoute(int simVersionId, int simSequence, int taskId, int taskLevel)
        {
            try
            {
                MODEL.risk_prj_version_info projectVersionInfo = CommonArugment.getProjectVersionInfo.getPrjVersionInfo(simVersionId);
                List<MODEL.risk_task_instance> taskIntanceList = CommonArugment.getTaskIntance.getBySeqVer(simVersionId, simSequence);
                List<MODEL.risk_task> taskList = null;
                List<MODEL.risk_link> linkList = null;

                if (taskLevel == 0)
                {
                    if (DrawImages.calTaskAndLink(projectVersionInfo.Pri_id, taskId))
                    {
                        taskList = CommonMsg.taskList;
                        linkList = CommonMsg.linkList;
                    }
                    else
                    {
                        return false;
                    }

                    for (int i = 0; i < taskList.Count; ++i)
                    {
                        if (taskList[i].Task_level != 0)
                        {
                            taskList.Remove(taskList[i]);
                            --i;
                            continue;
                        }
                    }
                }
                else
                {
                    if (DrawImages.calTaskAndLink(projectVersionInfo.Pri_id, taskId))
                    {
                        taskList = CommonMsg.taskList;
                        linkList = CommonMsg.linkList;
                    }
                    else
                    {
                        return false;
                    }

                    for (int i = 0; i < taskList.Count; ++i)
                    {
                        if (taskList[i].Task_level != taskLevel || taskList[i].Task_nested_parent_id != taskId)
                        {
                            taskList.Remove(taskList[i]);
                            --i;
                            continue;
                        }
                    }
                }

                foreach (MODEL.risk_task_instance taskIntance in taskIntanceList)
                {
                    for (int i = 0; i < taskList.Count; ++i)
                    {
                        if (taskList[i].Auto_id == taskIntance.Task_auto_id)
                        {
                            taskList[i].Task_is_critical = taskIntance.Task_is_critical;
                        }
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        private static bool isSatisfCreteria(List<MODEL.risk_prj_instance> prjInstanceList, double n1, double n2, double creteria)
        {
            double num = 0;
            foreach (MODEL.risk_prj_instance prjInstance in prjInstanceList)
            {
                if (prjInstance.Sim_project_time >= n1 && prjInstance.Sim_project_time <= n2)
                    ++num;
            }
            if (num / (double)prjInstanceList.Count >= creteria)
                return true;
            else
                return false;
        }
        //public static bool getBarData(int simVersionId, int scale)
        //{
        //        scale = 200;
        //        int range = 0;
        //        double myThink = 0.2;
        //        try
        //        {
        //                List<MODEL.risk_prj_instance> prjInstanceList = CommonArugment.getProjectInstance.getBySimVersion(simVersionId);
        //                double minN1 = prjInstanceList[0].Sim_project_time;
        //                double maxN1 = prjInstanceList[0].Sim_project_time;
        //                double maxN2 = prjInstanceList[0].Sim_project_time;
        //                foreach (MODEL.risk_prj_instance prjInstance in prjInstanceList)
        //                {
        //                        if (prjInstance.Sim_project_time < minN1)
        //                        {
        //                                minN1 = Math.Floor(prjInstance.Sim_project_time);
        //                        }
        //                        if (prjInstance.Sim_project_time > maxN1)
        //                        {
        //                                maxN1 = Math.Floor(prjInstance.Sim_project_time);
        //                        }
        //                }
        //                maxN2 = maxN1;
        //                while (!isSatisfCreteria(prjInstanceList, maxN2, maxN1, myThink))
        //                {
        //                        double res = 0;
        //                        foreach (MODEL.risk_prj_instance prjInstance in prjInstanceList)
        //                        {
        //                                if (prjInstance.Sim_project_time < maxN2 && prjInstance.Sim_project_time > res)
        //                                {
        //                                        res = prjInstance.Sim_project_time;
        //                                }
        //                        }
        //                        maxN2 = res;
        //                }
        //                range = (int)Math.Ceiling((maxN2 - minN1) / scale);
        //                List<MODEL.bar_data> barDataList = new List<MODEL.bar_data>();
        //                MODEL.bar_data barData0 = new MODEL.bar_data();
        //                barData0.ScFrom = 0;
        //                barData0.ScTo = (int)Math.Ceiling(minN1);

        //                barData0.ScNum = CommonArugment.getProjectInstance.getCount(simVersionId, barData0.ScTo, barData0.ScFrom);
        //                barDataList.Add(barData0);
        //                for (int i = 1; i < scale; ++i)
        //                {
        //                        MODEL.bar_data barData = new MODEL.bar_data();
        //                        barData.ScFrom = i * range + (int)Math.Ceiling(minN1);
        //                        barData.ScTo = (i + 1) * range + (int)Math.Ceiling(minN1);
        //                        barData.ScNum = CommonArugment.getProjectInstance.getCount(simVersionId, barData.ScTo, barData.ScFrom);
        //                        barDataList.Add(barData);
        //                }
        //                CommonMsg.barDataList = barDataList;
        //                return true;
        //        } catch (System.Exception ex)
        //        {
        //                throw ex;
        //        }
        //}
        public static bool getResoueceUse(int simVersionId, int scale,int resourceId)
        {
            int range = 0;
            try
            {
                MODEL.risk_prj_version_info info = CommonArugment.getProjectVersionInfo.getPrjVersionInfo(simVersionId);
                List<MODEL.risk_prj_instance> prjInstanceList = CommonArugment.getProjectInstance.getBySimVersion(simVersionId);
                List<MODEL.bar_data> barDataList = new List<MODEL.bar_data>();
                List<MODEL.risk_task_instance> taskList = CommonArugment.getTaskIntance.getVersionTaskInstance(info.Pri_id,simVersionId);
                double  Avg_projecttime =0;
                foreach (MODEL.risk_prj_instance prj in prjInstanceList)
                {
                    Avg_projecttime += prj.Sim_project_time;
                }
                Avg_projecttime = Avg_projecttime / info.Count;

                    double maxN1 = Avg_projecttime;
                    double MinN1 = 0;
                    range = (int)Math.Ceiling((maxN1 - MinN1) / scale);
                    for (int i = 0; i <= scale; ++i)
                    {
                        MODEL.bar_data bar = new MODEL.bar_data();
                        bar.ScFrom = i * range + (int)MinN1;
                        bar.ScTo = (i + 1) * range + (int)MinN1;
                        if (bar.ScFrom > Avg_projecttime)
                            break;
                        foreach (MODEL.risk_task_instance task in taskList)
                        {
                            if (task.Starttime == -1)//概要任务跳过
                                break;
                            if (task.Starttime >= bar.ScFrom && task.Starttime + task.Task_actual_dur_period <= bar.ScTo)//第一种 在中间
                            {
                                MODEL.risk_task_resource_assignment ass = CommonArugment.getTaskResAssign.findByTaskAutoIdResId(task.Task_auto_id, resourceId);
                                if (ass != null)
                                {
                                    bar.Ratio += (double)ass.Assignment_amount;
                                }
                            }
                            else if (task.Starttime <= bar.ScFrom && task.Starttime + task.Task_actual_dur_period > bar.ScFrom && task.Starttime + task.Task_actual_dur_period < bar.ScTo)//第二种 
                            {
                                MODEL.risk_task_resource_assignment ass = CommonArugment.getTaskResAssign.findByTaskAutoIdResId(task.Task_auto_id, resourceId);
                                if (ass != null)
                                {
                                    bar.Ratio += (double)ass.Assignment_amount / task.Task_actual_dur_period * (task.Starttime + task.Task_actual_dur_period - bar.ScFrom);
                                }
                            }
                            else if(task.Starttime> bar.ScFrom && task.Starttime<bar.ScTo && task.Starttime + task.Task_actual_dur_period > bar.ScTo)//第三种
                            {
                                MODEL.risk_task_resource_assignment ass = CommonArugment.getTaskResAssign.findByTaskAutoIdResId(task.Task_auto_id, resourceId);
                                if (ass != null)
                                {
                                    bar.Ratio += (double)ass.Assignment_amount/ task.Task_actual_dur_period * (bar.ScTo - task.Starttime);
                                }
                            }
                            else if (task.Starttime <= bar.ScFrom && task.Starttime + task.Task_actual_dur_period >= bar.ScTo)//第四种
                            {
                                MODEL.risk_task_resource_assignment ass = CommonArugment.getTaskResAssign.findByTaskAutoIdResId(task.Task_auto_id, resourceId);
                                if (ass != null)
                                {
                                    bar.Ratio += (double)ass.Assignment_amount/task.Task_actual_dur_period*(bar.ScTo - bar.ScFrom);
                                }
                            }
                        }
                        barDataList.Add(bar);
                    }
                    foreach (MODEL.bar_data bar in barDataList)
                    {
                        bar.Ratio = bar.Ratio / info.Count;
                    }
                CommonMsg.barDataList = barDataList;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool getOverdueRiskRatio(int simVersionId, int scale)
        {
            int range = 0;
            try
            {
                List<MODEL.risk_prj_instance> prjInstanceList = CommonArugment.getProjectInstance.getBySimVersion(simVersionId);
                double maxN1 = CommonArugment.getProjectInstance.findMaxPrjTimeBelow(simVersionId, CommonMsg.maxNumber);
                double MinN1 = CommonArugment.getProjectInstance.findMinPrjTimeAbove(simVersionId, 0);
                range = (int)Math.Ceiling((maxN1-MinN1) / scale);
                List<MODEL.bar_data> barDataList = new List<MODEL.bar_data>();
                for (int i = 0; i <= scale; ++i)
                {
                    MODEL.bar_data barData = new MODEL.bar_data();
                    barData.ScFrom = i * range+(int)MinN1;
                    barData.ScTo = (i + 1) * range + (int)MinN1;
                    barData.ScNum = CommonArugment.getProjectInstance.getCount(simVersionId, (int)Math.Ceiling(maxN1), barData.ScFrom);
                    barData.Ratio = Convert.ToDouble(barData.ScNum) / prjInstanceList.Count;
                    barDataList.Add(barData);
                }
                CommonMsg.barDataList = barDataList;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool getOverdueCostRatio(int simVersionId, int scale)
        {
            int range = 0;
            try
            {
                List<MODEL.risk_prj_instance> prjInstanceList = CommonArugment.getProjectInstance.getBySimVersion(simVersionId);
                double maxN1 = CommonArugment.getProjectInstance.findMaxPrjCostBelow(simVersionId, CommonMsg.maxNumber);
                double MinN1 = CommonArugment.getProjectInstance.findMinPrjCostAbove(simVersionId, 0);
                range = (int)Math.Ceiling((maxN1 - MinN1) / scale);
                List<MODEL.bar_data> barDataList = new List<MODEL.bar_data>();
                for (int i = 0; i <= scale; ++i)
                {
                    MODEL.bar_data barData = new MODEL.bar_data();
                    barData.ScFrom = i * range + (int)MinN1;
                    barData.ScTo = (i + 1) * range + (int)MinN1;
                    barData.ScNum = CommonArugment.getProjectInstance.getCostCount(simVersionId, (int)Math.Ceiling(maxN1), barData.ScFrom);
                    barData.Ratio = Convert.ToDouble(barData.ScNum) / prjInstanceList.Count;
                    barDataList.Add(barData);
                }
                CommonMsg.barDataList = barDataList;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool getBarData(int simVersionId, int scale)
        {
            int range = 0;
            double myThink = 0.2;
            try
            {
                List<MODEL.risk_prj_instance> prjInstanceList = CommonArugment.getProjectInstance.getBySimVersion(simVersionId);
                double minN1 = CommonArugment.getProjectInstance.findMinPrjTimeAbove(simVersionId, 0);
                double maxN1 = CommonArugment.getProjectInstance.findMaxPrjTimeBelow(simVersionId, CommonMsg.maxNumber);
                double maxN2 = maxN1;
                while (!isSatisfCreteria(prjInstanceList, maxN2, maxN1, myThink))
                {
                    maxN2 = CommonArugment.getProjectInstance.findMaxPrjTimeBelow(simVersionId, maxN2);
                }
                range = (int)Math.Ceiling((maxN1 - minN1) / scale);
                List<MODEL.bar_data> barDataList = new List<MODEL.bar_data>();
                MODEL.bar_data barData0 = new MODEL.bar_data();
                barData0.ScFrom = 0;
                barData0.ScTo = (int)Math.Ceiling(minN1);
                barData0.ScNum = CommonArugment.getProjectInstance.getCount(simVersionId, barData0.ScTo, barData0.ScFrom);
                barDataList.Add(barData0);
                for (int i = 1; i < scale; ++i)
                {
                    MODEL.bar_data barData = new MODEL.bar_data();
                    barData.ScFrom = i * range + (int)Math.Ceiling(minN1);
                    barData.ScTo = (i + 1) * range + (int)Math.Ceiling(minN1);
                    barData.ScNum = CommonArugment.getProjectInstance.getCount(simVersionId, barData.ScTo, barData.ScFrom);
                    barDataList.Add(barData);
                }
                CommonMsg.barDataList = barDataList;
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static List<MODEL.risk_task> getAverageProjectTime(int simVersionId)
        {
            try
            {
                MODEL.risk_prj_version_info projectVersionInfo = CommonArugment.getProjectVersionInfo.getPrjVersionInfo(simVersionId);
                List<MODEL.risk_task> taskList = CommonArugment.getTask.getTaskList(projectVersionInfo.Pri_id);

                for (int j = 0; j < taskList.Count; ++j)
                {
                    taskList[j].AverageProjectTime = CommonArugment.getTaskIntance.getSum(simVersionId, taskList[j].Auto_id) / projectVersionInfo.Count;
                }
                return taskList;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static List<MODEL.risk_task> getCriticalRatio(int simVersionId, int taskAutoId)
        {
            try
            {
                int taskId;
                if (taskAutoId == -1)
                    taskId = -1;
                else
                    taskId = CommonArugment.getTask.getTask(taskAutoId).Task_id;

                MODEL.risk_prj_version_info projectVersionInfo = CommonArugment.getProjectVersionInfo.getPrjVersionInfo(simVersionId);
                List<MODEL.risk_task> taskList = CommonArugment.getTask.getTaskList(projectVersionInfo.Pri_id);
                for (int i = 0; i < taskList.Count; ++i)
                {
                    if (taskList[i].Task_nested_parent_id != taskId)
                    {
                        taskList.RemoveAt(i--);
                    }
                }
                for (int j = 0; j < taskList.Count; ++j)
                {
                    taskList[j].CriticalRatio = (double)CommonArugment.getTaskIntance.getCriticalSum(simVersionId, taskList[j].Auto_id) / projectVersionInfo.Count;
                }
                return taskList;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static List<MODEL.risk_task> getWaitTask(int simVersionId, int taskAutoId)
        { 
            try
            {
                List<MODEL.risk_task> risklist = new List<MODEL.risk_task>();

                    risklist=CommonArugment.getTask.getWaitTaskList(simVersionId);
                    foreach (MODEL.risk_task task in risklist)
                    {
                        double max_wait_time = 0;
                        List<MODEL.risk_task_instance_res> reslist = new List<MODEL.risk_task_instance_res>();
                        reslist = CommonArugment.getTaskIntance_res.getWaitResList(simVersionId, task.Auto_id);
                        foreach (MODEL.risk_task_instance_res res in reslist)
                        {
                            if (max_wait_time < res.Wait_time)
                                max_wait_time = res.Wait_time;
                        }
                        task.Value = max_wait_time;
                    }
                return risklist;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public static double getDirectCost(int simVersionId)
        { 
            try
            {
                return CommonArugment.getProjectInstance.findAVGDirectCost(simVersionId);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static double getAvgDuration(int simVersionId)
        {
            try
            {
                return CommonArugment.getProjectInstance.getAvgDuration(simVersionId);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static List<MODEL.risk_task_instance_res> getWaitResource(int simVersionId, int taskAutoid)
        {
            try
            {
                List<MODEL.risk_task_instance_res> reslist = new List<MODEL.risk_task_instance_res>();
                reslist = CommonArugment.getTaskIntance_res.getWaitResList(simVersionId, taskAutoid);
                return reslist;
               
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public static bool dotTestFunc(int simVersionId)
        {
            int index = 2768;
            int scale = 100;
            try
            {
                List<MODEL.bar_data> barDataList = new List<MODEL.bar_data>();
                MODEL.risk_prj_version_info projectVersionInfo = CommonArugment.getProjectVersionInfo.getPrjVersionInfo(simVersionId);
                double maxProjectTime = CommonArugment.getTaskIntance.getMaxPeriod(simVersionId, index);
                double minProjectTime = CommonArugment.getTaskIntance.getMinPeriod(simVersionId, index);

                int range = (int)Math.Ceiling((maxProjectTime - minProjectTime) / scale);

                for (int i = 1; i < scale; ++i)
                {
                    MODEL.bar_data barData = new MODEL.bar_data();
                    barData.ScFrom = i * range + (int)Math.Ceiling(minProjectTime);
                    barData.ScTo = (i + 1) * range + (int)Math.Ceiling(minProjectTime);
                    barData.ScNum = CommonArugment.getTaskIntance.dotTest(simVersionId, barData.ScTo, barData.ScFrom, index);
                    barDataList.Add(barData);
                }
                CommonMsg.barDataList = barDataList;
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static List<MODEL.risk_task> getTaskTree(int projectId, int taskId)
        {
            List<MODEL.risk_task> taskList = CommonArugment.getTask.getTaskList(projectId);
            for (int i = 0; i < taskList.Count; ++i)
            {
                if (taskList[i].Task_nested_parent_id != taskId)
                {
                    taskList.RemoveAt(i--);
                }
            }

            return taskList;
        }

        public static JArray getChildren(int projectId, int taskId)
        {
            List<MODEL.risk_task> childrenTaskList = BLL.GetResults.getTaskTree(projectId, taskId);
            JArray JChildren = new JArray();
            foreach (MODEL.risk_task task in childrenTaskList)
            {
                JObject JChild = new JObject();
                JChild.Add("id", task.Auto_id);
                JChild.Add("text", task.Task_name);
                if (BLL.GetResults.getTaskTree(projectId, task.Task_id).Count > 0)
                {
                    JChild.Add("children", getChildren(projectId, task.Task_id));
                }
                else
                    JChild.Add("leaf", true);
                JChildren.Add(JChild);
            }
            return JChildren;
        }

        public static JArray getwaitResTaskTree(int VersionId)
        {
            List<MODEL.risk_task> waitTaskList = CommonArugment.getTask.getWaitTaskList(VersionId);
            JArray JChildren = new JArray();
            foreach (MODEL.risk_task task in waitTaskList)
            {
                JObject JChild = new JObject();
                JChild.Add("id", task.Auto_id);
                JChild.Add("text", task.Task_name);
                JChild.Add("leaf", true);
                JChildren.Add(JChild);
            }
            return JChildren;
        }
        /*
         * 得到最大关键路径的概率，返回该关键路径的工期
         */
        public static double getCriticalRouteByMaxRatio(int simVersionId, int taskId, int taskLevel)
        {
            try
            {
                MODEL.risk_prj_version_info projectVersionInfo = CommonArugment.getProjectVersionInfo.getPrjVersionInfo(simVersionId);

                if (CommonMsg.taskList != null)
                    CommonMsg.taskList.Clear();
                if (CommonMsg.linkList != null)
                    CommonMsg.linkList.Clear();
                CommonMsg.taskList = CommonArugment.getTask.getTaskList(projectVersionInfo.Pri_id);
                CommonMsg.linkList = CommonArugment.getTask.getLinkList(projectVersionInfo.Pri_id);
                for (int j = 0; j < CommonMsg.taskList.Count; ++j)
                {
                    CommonMsg.taskList[j].Value=CommonMsg.taskList[j].AverageProjectTime = CommonArugment.getTaskIntance.getSum(simVersionId, CommonMsg.taskList[j].Auto_id) / projectVersionInfo.Count;
                    CommonMsg.taskList[j].CriticalRatio = (double)CommonArugment.getTaskIntance.getCriticalSum(simVersionId, CommonMsg.taskList[j].Auto_id) / projectVersionInfo.Count;
                                                       
                }
                CommonUtils.getCriticalRoute();
                CommonUtils.calInOutDegree(CommonMsg.taskList, CommonMsg.linkList);
                CommonUtils.calMaxSucceedSize(CommonMsg.taskList, CommonMsg.linkList);
                double ans = 0;
                for (int j = 0; j < CommonMsg.taskList.Count; ++j)
                {
                    if (CommonMsg.taskList[j].Task_nested_parent_id != taskId)
                    {
                        CommonMsg.taskList.RemoveAt(j--);
                    }
                }
                for (int j = 0; j < CommonMsg.taskList.Count; ++j)
                {
                    if (CommonMsg.taskList[j].OutDegree == 0)
                    {
                        ans = CommonMsg.taskList[j].Ve;
                    }
                }
                return ans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool getCriticalRouteByMaxRatio(int simVersionId, int taskId, int taskLevel)
        //{
        //    try
        //    {
        //        MODEL.risk_prj_version_info projectVersionInfo = CommonArugment.getProjectVersionInfo.getPrjVersionInfo(simVersionId);

        //        if (CommonMsg.taskList != null)
        //            CommonMsg.taskList.Clear();
        //        if (CommonMsg.linkList != null)
        //            CommonMsg.linkList.Clear();
        //        List<MODEL.risk_task> taskList = CommonUtils.getAllTasks(projectVersionInfo.Pri_id);
        //        List<MODEL.risk_link> linkList = CommonUtils.getAllLinks(projectVersionInfo.Pri_id);

        //        bool[] taskIdArray = new bool[taskList.Count * 2];

        //        for (int i = 0; i < taskList.Count; ++i)
        //        {
        //            MODEL.risk_task task = taskList[i];
        //            taskIdArray[task.Task_id] = false;  //initialize
        //            if (task.Task_level != taskLevel)
        //            {
        //                taskList.Remove(task);
        //                --i;
        //                continue;
        //            }
        //            taskIdArray[task.Task_id] = true;
        //            task.OutDegree = 0;
        //            task.InDegree = 0;
        //            task.MaxSucceedSize = 0;
        //            if (!task.Task_is_summary)
        //            {
        //                try
        //                {
        //                    MODEL.risk_math_expression_arg expressionArg = CommonArugment.getExpression.getExpression(task.Auto_id);
        //                    if (expressionArg != null)
        //                    {
        //                        task.IsDone = true;
        //                        task.ExpressionName = expressionArg.Name;
        //                        if (expressionArg.Actual_value != 0)
        //                        {
        //                            task.Value = expressionArg.Actual_value;
        //                            continue;
        //                        }
        //                        double[] values = (double[])CommonUtils.Deserialize(expressionArg.Value);
        //                        switch (task.ExpressionName)
        //                        {
        //                            case "三角分布":
        //                                task.ArgA = values[0];
        //                                task.ArgB = values[1];
        //                                task.ArgC = values[2];
        //                                break;
        //                            case "Beta分布":
        //                                task.ArgA = values[0];
        //                                task.ArgB = values[1];
        //                                break;
        //                            case "正态分布":
        //                                task.ArgA = values[0];
        //                                task.ArgB = values[1];
        //                                break;
        //                            default:
        //                                task.ExpressionName = "未知分布";
        //                                break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        task.IsDone = false;
        //                    }
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    CommonMsg.errMsg = ex.Message + "->没有子任务的点必须标注为issummary为false";
        //                    return false;
        //                }

        //            }
        //            else
        //            {
        //                task.IsDone = CommonUtils.checkIsDone(taskList, task.Task_id);
        //            }
        //        }

        //        //cal each in/out degree of the task
        //        for (int i = 0; i < linkList.Count; ++i)
        //        {
        //            MODEL.risk_link link = linkList[i];
        //            int outDot = link.Task_pre_id;
        //            int inDot = link.Task_suc_id;
        //            if (!taskIdArray[outDot] || !taskIdArray[inDot])
        //            {
        //                linkList.Remove(link);
        //                --i;
        //                continue;
        //            }
        //            bool outDone = false;
        //            bool inDone = false;
        //            foreach (MODEL.risk_task task in taskList)
        //            {
        //                if (task.Task_id == outDot)
        //                {
        //                    ++task.OutDegree;
        //                    outDone = true;
        //                }

        //                if (task.Task_id == inDot)
        //                {
        //                    ++task.InDegree;
        //                    inDone = true;
        //                }
        //                if (outDone && inDone)
        //                    break;
        //            }
        //        }
        //        bool endLoop = false;
        //        for (int j = 0; j < taskList.Count; ++j)
        //        {
        //            if (taskList[j].InDegree == 0 || taskList[j].OutDegree == 0)
        //                taskList[j].Task_is_critical = 1;
        //        }
        //        while (!endLoop)
        //        {
        //            endLoop = true;
        //            int maxIndex = 0;
        //            double maxRatio = 0;
        //            for (int i = 0; i < taskList.Count; ++i)
        //            {
        //                if (!taskList[i].Finished && taskList[i].InDegree == 0)
        //                {
        //                    endLoop = false;
        //                    taskList[i].Finished = true;
        //                    int itsMaxSucceedSize = taskList[i].MaxSucceedSize;
        //                    foreach (MODEL.risk_link link in linkList)
        //                    {
        //                        if (link.Task_pre_id == taskList[i].Task_id)
        //                        {
        //                            for (int j = 0; j < taskList.Count; ++j)
        //                            {
        //                                if (taskList[j].Task_id == link.Task_suc_id)
        //                                {
        //                                    --taskList[j].InDegree;
        //                                    taskList[j].AverageProjectTime = CommonArugment.getTaskIntance.getSum(simVersionId, taskList[j].Auto_id) / projectVersionInfo.Count;
        //                                    taskList[j].CriticalRatio = (double)CommonArugment.getTaskIntance.getCriticalSum(simVersionId, taskList[j].Auto_id) / projectVersionInfo.Count;
        //                                    double curRatio = (double)CommonArugment.getTaskIntance.getCriticalSum(simVersionId, taskList[j].Auto_id) / projectVersionInfo.Count;
        //                                    if (curRatio > maxRatio)
        //                                    {
        //                                        maxRatio = curRatio;
        //                                        maxIndex = j;
        //                                    }
        //                                }
        //                            }                                   
        //                        }
        //                    }
        //                    taskList[maxIndex].Task_is_critical = 1;
        //                    maxRatio = 0;
        //                    maxIndex = 0;
        //                }
        //            }

        //        }
        //        CommonUtils.calInOutDegree(taskList, linkList);

        //        if (CommonUtils.calMaxSucceedSize(taskList, linkList))
        //        {
        //            CommonMsg.errMsg = "任务有环";
        //            return false;
        //        }
        //        else
        //        {
        //            CommonMsg.taskList = taskList;
        //            CommonMsg.linkList = linkList;
        //            return true;
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
