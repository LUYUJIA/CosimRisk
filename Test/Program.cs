using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<MODEL.risk_task> taskList = BLL.CommonArugment.getTask.getTaskList(2);
            List<MODEL.risk_link> linkList = BLL.CommonArugment.getTask.getLinkList(2);
            calPriority(taskList, linkList, -1);
        }

        public static List<MODEL.Sequence> calPriority(List<MODEL.risk_task> _taskList, List<MODEL.risk_link> linkList, int parentTaskId)
        {
            List<MODEL.Sequence> priorityArray = new List<MODEL.Sequence>();
            try
            {
                List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
                foreach (MODEL.risk_task task in _taskList)
                {
                    if (task.Task_nested_parent_id == parentTaskId)
                    {
                        taskList.Add(task);
                    }
                }
                for (int j = 0; j < taskList.Count; ++j)
                {
                    taskList[j].PreTaskId = new int[taskList.Count];
                    taskList[j].SucceedTaskId = new int[taskList.Count];
                }

                //      cal each in/out degree of the task
                foreach (MODEL.risk_link link in linkList)
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

                foreach (MODEL.Sequence seq in priorityArray)
                {
                    foreach (int a in seq.Array)
                    {
                        Console.Write(a + "->");
                    }
                    Console.WriteLine("end");
                }
                Console.Read();
                return priorityArray;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static void calTopsort(List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList, List<MODEL.Sequence> priorityArray, MODEL.Sequence _curSeq)
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
        }

        public static int getTasklistIndegree(List<MODEL.risk_task> taskList)
        {
            int sum = 0;
            foreach (MODEL.risk_task task in taskList)
            {
                sum += task.InDegree;
            }
            return sum;
        }
    }
}
