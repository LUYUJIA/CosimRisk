using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DrawImages
    {
        #region
        public static void getAllTaskAndLink(int projectId)
        {
            if (CommonMsg.taskList != null)
                CommonMsg.taskList.Clear();
            if (CommonMsg.linkList != null)
                CommonMsg.linkList.Clear();
            List<MODEL.risk_task> taskList = CommonUtils.getAllTasks(projectId);
            List<MODEL.risk_link> linkList = CommonUtils.getAllLinks(projectId);
            bool[] taskId = new bool[taskList.Count * 2];

            for (int i = 0; i < taskList.Count; ++i)
            {
                MODEL.risk_task task = taskList[i];
                taskId[task.Task_id] = true;
                task.OutDegree = 0;
                task.InDegree = 0;
                task.MaxSucceedSize = 0;
                if (!task.Task_is_summary)
                {
                    try
                    {
                        MODEL.risk_math_expression_arg expressionArg = CommonArugment.getExpression.getExpression(task.Auto_id);
                        if (expressionArg != null)
                        {
                            task.IsDone = true;
                            task.ExpressionName = expressionArg.Name;
                            double[] values = (double[])CommonUtils.Deserialize(expressionArg.Value);
                            switch (task.ExpressionName)
                            {
                                case "三角分布":
                                    task.ArgA = values[0];
                                    task.ArgB = values[1];
                                    task.ArgC = values[2];
                                    break;
                                case "Beta分布":
                                    task.ArgA = values[0];
                                    task.ArgB = values[1];
                                    break;
                                case "正态分布":
                                    task.ArgA = values[0];
                                    task.ArgB = values[1];
                                    break;
                                case "固定":
                                    task.ArgA = values[0];
                                    break;
                                default:
                                    task.ExpressionName = "未知分布";
                                    break;
                            }
                            if (expressionArg.Actual_value != 0)
                            {
                                task.Value = expressionArg.Actual_value;
                                continue;
                            }
                        }
                        else
                        {
                            task.IsDone = false;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                {
                    task.IsDone = CommonUtils.checkIsDone(taskList, task.Task_id);
                }
            }

            CommonMsg.taskList = taskList;
            CommonMsg.linkList = linkList;

        }
        #endregion

        #region 计算子图任务的属性以便画图
        public static bool calTaskAndLink(int projectId, int parentTaskId)
        {
            if (CommonMsg.taskList != null)
                CommonMsg.taskList.Clear();
            if (CommonMsg.linkList != null)
                CommonMsg.linkList.Clear();
            List<MODEL.risk_task> taskList = CommonUtils.getAllTasks(projectId);
            List<MODEL.risk_link> linkList = CommonUtils.getAllLinks(projectId);

            //test
            //try
            //{
            //    CommonUtils.unionTaskList(taskList, linkList);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            List<MODEL.risk_task_instance> taskInstanceList = CommonArugment.getTaskIntance.get(projectId);

            bool[] taskId = new bool[taskList.Count * 2];

            for (int i = 0; i < taskList.Count; ++i)
            {
                MODEL.risk_task task = taskList[i];
                taskId[task.Task_id] = false;  //initialize
                if (task.Task_nested_parent_id != parentTaskId)
                {
                    taskList.Remove(task);
                    --i;
                   continue;
                }
                taskId[task.Task_id] = true;
                task.OutDegree = 0;
                task.InDegree = 0;
                task.MaxSucceedSize = 0;
                if (!task.Task_is_summary)
                {
                    try
                    {
                        MODEL.risk_math_expression_arg expressionArg = CommonArugment.getExpression.getExpression(task.Auto_id);
                        List<MODEL.risk_resource> list = CommonArugment.getTaskResAssign.findResByTaskAutoId(task.Auto_id);
                        if (list.Count != 0)
                        {
                            task.Have_resource = true;
                        }
                        if (expressionArg != null)
                        {
                            task.IsDone = true;
                            task.ExpressionName = expressionArg.Name;
                            double[] values = (double[])CommonUtils.Deserialize(expressionArg.Value);
                            switch (task.ExpressionName)
                            {
                                case "三角分布":
                                    task.ArgA = values[0];
                                    task.ArgB = values[1];
                                    task.ArgC = values[2];
                                    break;
                                case "Beta分布":
                                    task.ArgA = values[0];
                                    task.ArgB = values[1];
                                    break;
                                case "正态分布":
                                    task.ArgA = values[0];
                                    task.ArgB = values[1];
                                    break;
                                case "固定":
                                    task.ArgA = values[0];
                                    break;
                                default:
                                    task.ExpressionName = "未知分布";
                                    break;
                            }
                            if (expressionArg.Actual_value != 0)
                            {
                                task.Value = expressionArg.Actual_value;
                                continue;
                            }
                        }
                        else
                        {
                            task.IsDone = false;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        CommonMsg.errMsg = ex.Message + "->没有子任务的点必须标注为issummary为false";
                        return false;
                    }

                }
                else
                {
                    task.IsDone = CommonUtils.checkIsDone(taskList, task.Task_id);
                }
            }
            //cal each in/out degree of the task
            for (int i = 0; i < linkList.Count; ++i)
            {
                MODEL.risk_link link = linkList[i];
                int outDot = link.Task_pre_id;
                int inDot = link.Task_suc_id;
                if (!taskId[outDot] || !taskId[inDot])
                {
                    linkList.Remove(link);
                    --i;
                    continue;
                }
                bool outDone = false;
                bool inDone = false;
                foreach (MODEL.risk_task task in taskList)
                {
                    if (task.Task_id == outDot)
                    {
                        ++task.OutDegree;
                        outDone = true;
                    }

                    if (task.Task_id == inDot)
                    {
                        ++task.InDegree;
                        inDone = true;
                    }
                    if (outDone && inDone)
                        break;
                }
            }
            try
            {
                if (CommonUtils.calMaxSucceedSize(taskList, linkList))
                {
                    CommonMsg.errMsg = "任务有环";
                    return false;
                }
                else
                {
                    CommonMsg.taskList = taskList;
                    CommonMsg.linkList = linkList;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        //public static bool calTaskAndLink(int projectId ,int myTaskId)
        //{
        //    if (CommonMsg.taskList != null)
        //        CommonMsg.taskList.Clear();
        //    if (CommonMsg.linkList != null)
        //        CommonMsg.linkList.Clear();
        //    List<MODEL.risk_task> taskList = CommonUtils.getAllTasks(projectId);
        //    List<MODEL.risk_link> linkList = CommonUtils.getAllLinks(projectId);
        //    List<MODEL.risk_task_instance> taskInstanceList = CommonArugment.getTaskIntance.get(projectId);
        //    bool[] taskId = new bool[taskList.Count * 2];

        //    for (int i = 0; i < taskList.Count; ++i)
        //    {
        //        MODEL.risk_task task = taskList[i];
        //        taskId[task.Task_id] = false;  //initialize
        //        if ( task.Task_nested_parent_id != myTaskId)
        //        {
        //            taskList.Remove(task);
        //            --i;
        //            continue;
        //        }
        //        taskId[task.Task_id] = true;
        //        task.OutDegree = 0;
        //        task.InDegree = 0;
        //        task.MaxSucceedSize = 0;
        //        if (!task.Task_is_summary)
        //        {
        //            try
        //            {
        //                MODEL.risk_math_expression_arg expressionArg = CommonArugment.getExpression.getExpression(task.Auto_id);
        //                if (expressionArg != null)
        //                {
        //                    task.IsDone = true;
        //                    task.ExpressionName = expressionArg.Name;
        //                    double[] values = (double[])CommonUtils.Deserialize(expressionArg.Value);
        //                    switch (task.ExpressionName)
        //                    {
        //                        case "三角分布":
        //                            task.ArgA = values[0];
        //                            task.ArgB = values[1];
        //                            task.ArgC = values[2];
        //                            break;
        //                        case "正态分布":
        //                            task.ArgA = values[0];
        //                            task.ArgB = values[1];
        //                            break;
        //                        case "beta分布":
        //                            task.ArgA = values[0];
        //                            task.ArgB = values[1];
        //                            break;
        //                        default:
        //                            task.ExpressionName = "未知分布";
        //                            break;
        //                    }
        //                    if (expressionArg.Actual_value != 0)
        //                    {
        //                        task.Value = expressionArg.Actual_value;
        //                        continue;
        //                    }
        //                }
        //                else
        //                {
        //                    task.IsDone = false;
        //                }

        //            }
        //            catch (System.Exception ex)
        //            {
        //                CommonMsg.errMsg = ex.Message + "->没有子任务的点必须标注为issummary为false";
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            task.IsDone = CommonUtils.checkIsDone(taskList, task.Task_id);
        //        }


        //    }
        //    //cal each in/out degree of the task
        //    for (int i = 0; i < linkList.Count; ++i)
        //    {
        //        MODEL.risk_link link = linkList[i];
        //        int outDot = link.Task_pre_id;
        //        int inDot = link.Task_suc_id;
        //        if (!taskId[outDot] || !taskId[inDot])
        //        {
        //            linkList.Remove(link);
        //            --i;
        //            continue;
        //        }
        //        bool outDone = false;
        //        bool inDone = false;
        //        foreach (MODEL.risk_task task in taskList)
        //        {
        //            if (task.Task_id == outDot)
        //            {
        //                ++task.OutDegree;
        //                outDone = true;
        //            }

        //            if (task.Task_id == inDot)
        //            {
        //                ++task.InDegree;
        //                inDone = true;
        //            }
        //            if (outDone && inDone)
        //                break;
        //        }
        //    }
        //    try
        //    {
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
        #endregion

        #region 设置任务属性以便仿真
        //public static bool setTaskAttribute(int auto_id, int taskPriority, double actualValue)
        //{
        //    MODEL.risk_math_expression_arg expression = new MODEL.risk_math_expression_arg();
        //    expression.Name = "正态分布";
        //    expression.Task_id = auto_id;
        //    double []value=new double[3]{0,0,0};
        //    expression.Value = CommonUtils.Serialize(value);
        //    expression.Actual_value = actualValue;
        //    try
        //    {
        //        int expressionId = CommonArugment.getExpression.saveExpression(expression);
        //        if (expressionId == -1)
        //        {
        //            CommonMsg.errMsg = "插入expression发生错误";
        //            return false;
        //        }
        //        MODEL.risk_task task = CommonArugment.getTask.getTask(auto_id);
        //        task.Task_priority = taskPriority;
        //        task.Task_expression_id = expressionId;  //这个属性没有用
        //        return CommonArugment.getTask.updateTask(task);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        public static bool setTaskAttribute(int auto_id, int taskPriority, double[] value, double actualValue, string expressionName)
        {
            MODEL.risk_math_expression_arg expression = new MODEL.risk_math_expression_arg();
            expression.Name = expressionName;
            expression.Task_id = auto_id;
            expression.Value = CommonUtils.Serialize(value);
            expression.Actual_value = actualValue;
            try
            {
                int expressionId = CommonArugment.getExpression.saveExpression(expression);
                if (expressionId == -1)
                {
                    CommonMsg.errMsg = "插入expression发生错误";
                    return false;
                }
                MODEL.risk_task task = CommonArugment.getTask.getTask(auto_id);
                task.Task_priority = taskPriority;
                task.Task_expression_id = expressionId;  //这个属性没有用
                return CommonArugment.getTask.updateTask(task);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 获得任务资源
        public static List<MODEL.risk_resource> getTaskResources(int taskAutoId)
        {
            try
            {
                DAL.DALriskTaskResourceAssignment task_assignment = new DAL.DALriskTaskResourceAssignment();
                List<MODEL.risk_resource> resourcesList = CommonArugment.getTaskResAssign.findResByTaskAutoId(taskAutoId);
                foreach (MODEL.risk_resource res in resourcesList)
                {
                    res.Resource_amount = task_assignment.findByTaskAutoIdResId(taskAutoId, res.Auto_id).Assignment_amount;
       
                }
                return resourcesList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 设置任务资源
        public static bool setTaskResources(int taskAutoId, MODEL.risk_resource res)
        {
            try
            {
                    CommonArugment.getTaskResAssign.insertRes(taskAutoId, res);
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 更新任务资源
        public static bool updateTaskResources(MODEL.risk_task_resource_assignment res)
        {
            try
            {
                CommonArugment.getTaskResAssign.updateByTaskAutoIdResId(res);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 删除任务资源
        public static bool deleteTaskResources(MODEL.risk_task_resource_assignment res)
        {
            try
            {
                CommonArugment.getTaskResAssign.deleteByTaskAutoIdResId(res);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}
