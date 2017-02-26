using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public  class ScheduleManagement
    {
        private static void ShrinkSummaryTask(int parentId, List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList, double ansRatio)
        {
            Stack<MODEL.risk_task> q = new Stack<MODEL.risk_task>();
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.InDegree == 0 && task.Task_nested_parent_id == parentId)
                {
                    q.Push(task);
                }
            }
            while (q.Count != 0)
            {
                MODEL.risk_task task = q.Pop();
                if (!task.Finished)//如果还没有压过
                {
                    task.Value *= ansRatio;
                    task.Finished = true;
                }
                if (task.Task_is_summary)
                {
                    //是不是taskId哦？？？
                    ShrinkSummaryTask(task.Task_id, taskList, linkList, ansRatio);
                }
                double criticalDuration = 0;
                //先找关键路径
                for (int j = 0; j < task.OutDegree; ++j)
                {
                    foreach (MODEL.risk_task t in taskList)
                    {
                        if (t.Task_id == task.SucceedTaskId[j] && t.Task_is_critical == 1)
                        {
                            criticalDuration = t.Value;
                        }
                    }
                }
                //再压缩
                for (int j = 0; j < task.OutDegree; ++j)
                {
                    MODEL.risk_task succeedTask = null;
                    foreach (MODEL.risk_task t in taskList)
                    {
                        if (t.Task_id == task.SucceedTaskId[j])
                            succeedTask = t;
                    }
                    succeedTask.InDegree--;
                    if (succeedTask.InDegree == 0)
                    {
                        q.Push(succeedTask);
                    }
                    if (!succeedTask.Finished)
                    {
                        succeedTask.Finished = true;
                        if (succeedTask.Task_is_critical == 1)
                            succeedTask.Value *= ansRatio;
                        else
                        {
                            if (succeedTask.Value > criticalDuration * ansRatio)
                                succeedTask.Value = criticalDuration * ansRatio;
                        }
                        if (succeedTask.Task_is_summary)
                            ShrinkSummaryTask(task.Task_id, taskList, linkList, ansRatio);
                    }

                }
            }
        }

        #region 进度压缩
        public static void ShrinkHours(int simVersionId, int taskId, int taskLevel, double targetDuration)
        {
            double currentDuration = BLL.GetResults.getCriticalRouteByMaxRatio(simVersionId, taskId, taskLevel);
            List<MODEL.risk_task> taskList = BLL.CommonMsg.taskList;
            List<MODEL.risk_link> linkList = BLL.CommonMsg.linkList;
            double ansRatio = targetDuration / currentDuration;

            //cal each in/out degree of the task
            for (int j = 0; j < taskList.Count; ++j)
            {
                taskList[j].Finished = false;
                taskList[j].PreTaskId = new int[taskList.Count];
                taskList[j].SucceedTaskId = new int[taskList.Count];
                taskList[j].Vl = CommonMsg.maxNumber;
            }
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

            Stack<MODEL.risk_task> q = new Stack<MODEL.risk_task>();
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.InDegree == 0 && task.Task_level == -1)
                {
                    q.Push(task);
                }
            }
            while (q.Count != 0)
            {
                MODEL.risk_task task = q.Pop();
                if (!task.Finished)//如果还没有压过
                {
                    task.Value *= ansRatio;
                    task.Finished = true;
                }
                if (task.Task_is_summary)
                {
                    //是不是taskId哦？？？
                    ShrinkSummaryTask(task.Task_id, taskList, linkList, ansRatio);
                }
                double criticalDuration = 0;
                //先找关键路径
                for (int j = 0; j < task.OutDegree; ++j)
                {
                    foreach (MODEL.risk_task t in taskList)
                    {
                        if (t.Task_id == task.SucceedTaskId[j] && t.Task_is_critical == 1)
                        {
                            criticalDuration = t.Value;
                        }
                    }
                }
                //再压缩
                for (int j = 0; j < task.OutDegree; ++j)
                {
                    MODEL.risk_task succeedTask = null;
                    foreach (MODEL.risk_task t in taskList)
                    {
                        if (t.Task_id == task.SucceedTaskId[j])
                            succeedTask = t;
                    }
                    succeedTask.InDegree--;
                    if (succeedTask.InDegree == 0)
                    {
                        q.Push(succeedTask);
                    }
                    if (!succeedTask.Finished)
                    {
                        succeedTask.Finished = true;
                        if (succeedTask.Task_is_critical == 1)
                            succeedTask.Value *= ansRatio;
                        else
                        {
                            if (succeedTask.Value > criticalDuration * ansRatio)
                                succeedTask.Value = criticalDuration * ansRatio;
                        }
                        if (succeedTask.Task_is_summary)
                            ShrinkSummaryTask(task.Task_id, taskList, linkList, ansRatio);
                    }

                }
            }
        }
        #endregion
    }
}
