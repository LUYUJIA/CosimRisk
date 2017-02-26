using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BLL
{
    internal static class CommonUtils
    {
        #region 检查项目名称是否存在
        internal static bool checkProjectName(String name)
        {
            return CommonArugment.getProject.checkProjectName(name);
        }
        #endregion

        #region 解析项目XML
        internal static void parseXML(XmlReader xml, List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList, int projectId)
        {
            int parentId = 0;
            int taskLevel = 0;
            int curParentTId = -1;
            int possibleParentTId = -1;
            Stack<int> parentTaskId = new Stack<int>();
            parentTaskId.Push(-1);
            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    if (xml.Name == "sub-tasks")
                    {
                        ++taskLevel;
                        parentTaskId.Push(curParentTId);
                        curParentTId = possibleParentTId;
                    }
                    if (xml.Name == "link")
                    {
                        MODEL.risk_link link = new MODEL.risk_link();
                        link.Delay_days = Int32.Parse(xml.GetAttribute("delay"));
                        switch (xml.GetAttribute("type"))
                        {
                            case "FS":
                                link.Link_type = 1;
                                break;
                            case "SS":
                                link.Link_type = 2;
                                break;
                            case "FF":
                                link.Link_type = 3;
                                break;
                            case "SF":
                                link.Link_type = 4;
                                break;
                            default:
                                link.Link_type = 1;
                                break;
                        }
                        link.Task_suc_id = Int32.Parse(xml.GetAttribute("taskId"));
                        link.Task_pre_id = parentId;
                        link.Prj_id = projectId;
                        linkList.Add(link);
                    }
                    if (xml.Name == "task" || xml.Name == "start-task" || xml.Name == "end-task")
                    {
                        if (xml.GetAttribute("isSummary") == "true")
                        {
                            //                              possibleParentTId = Int32.Parse(xml.GetAttribute("id"));
                        }

                        possibleParentTId = Int32.Parse(xml.GetAttribute("id"));
                        MODEL.risk_task task = new MODEL.risk_task();
                        if (xml.GetAttribute("isSummary") == null)
                        {
                            task.Task_is_summary = false;
                        }
                        else task.Task_is_summary = true;
                        task.Task_name = xml.GetAttribute("name");
                        task.Task_wbs = xml.GetAttribute("wbs");
                        task.Task_id = Int32.Parse(xml.GetAttribute("id"));
                        task.Task_level = taskLevel;
                        task.Task_project_id = projectId;
                        task.Task_nested_parent_id = curParentTId;
                        taskList.Add(task);
                        parentId = task.Task_id;
                    }
                }
                else if (xml.NodeType == XmlNodeType.EndElement)
                {
                    if (xml.Name == "sub-tasks")
                    {
                        --taskLevel;
                        curParentTId = parentTaskId.Pop();
                    }

                }
            }
        }
        #endregion

        #region 读取所有的project仿真版本信息
        public static List<MODEL.risk_prj_version_info> getAllPrjVInfo()
        {
            return CommonArugment.getProjectVersionInfo.getAll();
        }
        #endregion

        #region 读取所有的task
        public static List<MODEL.risk_task> getAllTasks(int projectId)
        {
            return CommonArugment.getTask.getTaskList(projectId);
        }
        #endregion

        #region 读取所有的link
        public static List<MODEL.risk_link> getAllLinks(int projectId)
        {
            return CommonArugment.getTask.getLinkList(projectId);
        }

        #endregion

        #region 读取所有的project
        public static List<MODEL.risk_project> getAllProjects()
        {
            return CommonArugment.getProject.getProjectList();
        }
        #endregion

        #region 检查一个节点是否设置完毕
        public static bool checkIsDone(List<MODEL.risk_task> taskList, int taskId)
        {
            bool IsDone = true;
            foreach (MODEL.risk_task sucTask in taskList)
            {
                if (sucTask.Task_nested_parent_id == taskId)
                {
                    if (sucTask.Task_is_summary)
                    {
                        if (!checkIsDone(taskList, sucTask.Task_id))
                        {
                            IsDone = false;
                            break;
                        }
                    }
                    else
                    {
                        if (CommonArugment.getExpression.getExpression(sucTask.Auto_id) == null)
                        {
                            IsDone = false;
                            break;
                        }
                    }
                }
            }
            return IsDone;
        }
        #endregion

        #region 计算每个task的最宽MaxSucceedSize
        public static bool calMaxSucceedSize(List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList)
        {
            bool hasLoop = true;
            List<MODEL.risk_task> taskList_Copy = new List<MODEL.risk_task>();
            foreach (MODEL.risk_task task in taskList)
            {
                taskList_Copy.Add(task.Clone() as MODEL.risk_task);
            }
            for (int i = 0; i < taskList_Copy.Count; ++i)
            {
                if (taskList_Copy[i].OutDegree == 0)
                {
                    taskList_Copy[i].MaxSucceedSize = 1;
                }
            }
            while (taskList_Copy.Count > 0)
            {
                hasLoop = true;
                for (int i = 0; i < taskList_Copy.Count; ++i)
                {
                    if (taskList_Copy[i].OutDegree == 0)
                    {
                        hasLoop = false;
                        int inTaskId = taskList_Copy[i].Task_id;
                        int itsMaxSucceedSize = taskList_Copy[i].MaxSucceedSize;
                        foreach (MODEL.risk_task task in taskList)
                        {
                            if (task.Auto_id == taskList_Copy[i].Auto_id)
                                task.MaxSucceedSize = itsMaxSucceedSize;
                        }
                        taskList_Copy.RemoveAt(i);
                        --i;
                        foreach (MODEL.risk_link link in linkList)
                        {
                            if (link.Task_suc_id == inTaskId)
                            {
                                for (int j = 0; j < taskList_Copy.Count; ++j)
                                {
                                    if (taskList_Copy[j].Task_id == link.Task_pre_id)
                                    {
                                        --taskList_Copy[j].OutDegree;
                                        taskList_Copy[j].MaxSucceedSize += itsMaxSucceedSize;
                                    }
                                }
                            }
                        }

                    }
                }
                if (hasLoop)
                    break;
            }
            return hasLoop;
        }
        #endregion

        #region 几种分布函数
        ///<summary> 产生对数正态分布随机数</summary>
        ///<param name="miu">平均值（实际应用中常需要将使用Math.Log(miu)作为参数）</param>
        /// <param name="sigma">方差（实际应用中常需要将使用0.5*Math.Log(sigma)作为参数）</param>
        ///<returns></returns>
        public static double Random_LogNorMal(double miu, double sigma)
        {
            miu = Math.Log(miu);//???
            sigma = 0.5 * Math.Log(sigma);  //???

            var rand = new Random(Guid.NewGuid().GetHashCode());
            var x = rand.NextDouble();
            return Math.Exp(miu + sigma * NormalFun(x));
        }

        ///<summary>
        /// 误差函数估算
        ///</summary>
        ///<param name="x"></param>
        ///<returns></returns>
        private static double ErfFun(double x)
        {
            double d = 2.0 / Math.Sqrt(Math.PI);
            double t = x / Math.Sqrt(2);
            double dsum = 0;
            double dn = 0;
            for (int n = 0; ; n++)
            {
                if (n == 0)
                {
                    dn = t;
                }
                else
                {
                    dn = Math.Pow(-1, n) * (Math.Pow(t, 2 * n + 1) / ((2 * n + 1) * nby(n)));
                }
                if (Math.Abs(dn) < 0.00001)
                {
                    break;
                }
                dsum += dn;
            }
            return 0.5 * (d * dsum + 1);
        }
        ///<summary>
        /// n阶乘算法
        ///</summary>
        ///<param name="n"></param>
        ///<returns></returns>
        private static double nby(int n)
        {
            double by = 1;
            for (int i = 1; i <= n; i++)
            {
                by = by * i;
            }
            return by;
        }
        ///<summary>
        /// 正态分布随机数产生（初次以1为迭代单位，尔后采用二分查找法搜索符合要求随机数）
        ///</summary>
        ///<param name="p"></param>
        ///<returns></returns>
        private static double NormalFun(double p)
        {
            double z = 0;
            double sumz = 0.5;
            if (p > 0.5)
            {
                do
                {
                    z += 1;
                    sumz = ErfFun(z);
                }
                while (sumz < p);
                if (sumz - p > 0.00001) //可根据需求自行确定精度。 
                {
                    return SearchNum(z - 1, z, p);
                }
                else return z;
            }
            else
                if (p < 0.5)
                {
                    do
                    {
                        z -= 1;
                        sumz = ErfFun(z);
                    }
                    while (sumz > p);
                    if (p - sumz > 0.00001)
                    {
                        return SearchNum(z, z + 1, p);
                    }
                    else return z;
                }
            return z;
        }
        ///<summary>
        /// 递归搜索随机数
        ///</summary>
        ///<param name="z1"></param>
        ///<param name="z2"></param>
        ///<param name="p"></param>
        ///<returns></returns>
        private static double SearchNum(double z1, double z2, double p)//z1>z2;
        {
            double z = (z1 + z2) / 2;
            double ef = ErfFun(z);
            if (ef < p && p - ef > 0.00001)
            {
                return SearchNum(z, z2, p);
            }
            else
            {
                if (ef > p && ef - p > 0.00001)
                {
                    return SearchNum(z1, z, p);
                }
            }
            return z;
        }


        public static double normDistri(double e, double d)
        {
            Random random = new Random();
            double rt = 0;
            rt = (d * Math.Sqrt(-2 * Math.Log(random.NextDouble())) * Math.Cos(2 * Math.PI * random.NextDouble()) + e);
            while (rt < 0)
            {
                rt = (d * Math.Sqrt(-2 * Math.Log(random.NextDouble())) * Math.Cos(2 * Math.PI * random.NextDouble()) + e);
            }
            return rt;
        }

        public static double triangleDistri(double a, double b, double m)
        {
            double r = new Random().NextDouble();
            double r1 = (m - a) / (b - a);
            if (r <= r1)
                return (a + Math.Sqrt(r * (m - a) * (b - a)));
            return (b - Math.Sqrt((1 - r) * (b - m) * (b - a)));
        }
        #endregion

        #region  序列化和反序列化
        public static byte[] Serialize(this object obj)
        {
            Stream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);
            byte[] b = null;
            b = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(b, 0, (int)stream.Length);
            stream.Flush();
            stream.Close();
            return b;
        }
        public static object Deserialize(this byte[] b)
        {
            MemoryStream stream = new MemoryStream(b);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);
            stream.Flush();
            stream.Close();
            return obj;
        }
        #endregion

        #region  topsort
        public static double topSort(List<MODEL.risk_task> originalTaskList, int parentTaskId)
        {
            List<MODEL.risk_task> taskList = new List<MODEL.risk_task>(originalTaskList);
            for (int j = 0; j < taskList.Count; ++j)
            {
                if (taskList[j].Task_nested_parent_id != parentTaskId)
                {
                    taskList.RemoveAt(j--);
                }
            }
            Stack<MODEL.risk_task> q = new Stack<MODEL.risk_task>();
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.InDegree == 0)
                {
                    if (task.Task_is_summary)
                    {
                        task.Ve = topSort(taskList, task.Task_id);
                    }
                    else task.Ve = task.Value;
                    q.Push(task);
                }
            }
            while (q.Count != 0)
            {
                MODEL.risk_task task = q.Pop();
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
                    if (succeedTask.Task_is_summary)
                    {
                        double itsValue = topSort(taskList, succeedTask.Task_id);
                        if (succeedTask.Ve < task.Ve + itsValue)
                        {
                            succeedTask.Ve = task.Ve + itsValue;
                        }
                    }
                    else
                    {
                        if (succeedTask.Ve < task.Ve + succeedTask.Value)
                        {
                            succeedTask.Ve = task.Ve + succeedTask.Value;
                        }
                    }
                }
                if (q.Count == 0)
                {
                    foreach (MODEL.risk_task tempTask in taskList)
                    {
                        originalTaskList[originalTaskList.IndexOf(tempTask)].Ve = tempTask.Ve;
                    }
                    return task.Ve;
                }
            }
            return 0;
        }
        #endregion

        #region reverse topsort
        public static double reversedTopSort(List<MODEL.risk_task> originalTaskList, int parentTaskId)
        {
            List<MODEL.risk_task> taskList = new List<MODEL.risk_task>(originalTaskList);
            for (int j = 0; j < taskList.Count; ++j)
            {
                if (taskList[j].Task_nested_parent_id != parentTaskId)
                {
                    taskList.RemoveAt(j--);
                }
            }

            Stack<MODEL.risk_task> q = new Stack<MODEL.risk_task>();
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.OutDegree == 0)
                {
                    task.Vl = task.Ve;
                    if (task.Task_is_summary)
                    {
                        task.Vl = reversedTopSort(taskList, task.Task_id);
                    }
                    q.Push(task);
                }
            }
            while (q.Count != 0)
            {
                MODEL.risk_task task = q.Pop();
                for (int j = 0; j < task.InDegree; ++j)
                {
                    MODEL.risk_task preTask = null;
                    foreach (MODEL.risk_task t in taskList)
                    {
                        if (t.Task_id == task.PreTaskId[j])
                            preTask = t;
                    }
                    preTask.OutDegree--;
                    if (preTask.OutDegree == 0)
                    {
                        q.Push(preTask);
                    }
                    if (task.Task_is_summary)
                    {
                        double itsValue = CommonUtils.reversedTopSort(taskList, task.Task_id);
                        if (preTask.Vl > task.Vl - itsValue)
                        {
                            preTask.Vl = task.Vl - itsValue;
                        }
                    }
                    else
                    {
                        if (preTask.Vl > task.Vl - task.Value)
                        {
                            preTask.Vl = task.Vl - task.Value;
                        }
                    }
                }
                if (q.Count == 0)
                {
                    foreach (MODEL.risk_task tempTask in taskList)
                    {
                        originalTaskList[originalTaskList.IndexOf(tempTask)].Vl = tempTask.Vl;
                    }
                    return task.Vl;
                }
            }
            return 0;
        }
        #endregion

        #region cal the critical route with resource
        public static double calItsCriticalRoute_res(int parentTaskId, int versionId, int sequence, List<MODEL.risk_task_instance_res> total_wait_resource)
        {
            double result = 0;
            List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();

            foreach (MODEL.risk_task task in CommonMsg.taskList)
            {
                if (task.Task_nested_parent_id == parentTaskId)
                {
                    taskList.Add(task);
                }
            }
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.Task_is_summary)
                {
                    task.Value = CommonUtils.calItsCriticalRoute_res(task.Task_id, versionId, sequence, total_wait_resource);
                }
            }


            List<MODEL.risk_task> reversedTaskList = new List<MODEL.risk_task>();//used when topsort reversely
            for (int j = 0; j < taskList.Count; ++j)
            {
                taskList[j].PreTaskId = new int[taskList.Count];
                taskList[j].SucceedTaskId = new int[taskList.Count];
                taskList[j].Vl = CommonMsg.maxNumber;
            }

            //      cal each in/out degree of the task
            foreach (MODEL.risk_link link in CommonMsg.linkList)  // could make improvements
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
                    result = task.Ve + task.Value;
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
                MODEL.risk_task_instance taskInstance = new MODEL.risk_task_instance(taskList[j].Auto_id, taskList[j].Task_project_id, taskList[j].Value, sequence, versionId);
                taskInstance.Task_ve = taskList[j].Ve;
                taskInstance.Starttime = Math.Round(taskList[j].StartTime, 2);
                taskInstance.Task_vl = reversedTaskList[j].Vl;
                if (Math.Abs(taskInstance.Task_ve - taskInstance.Task_vl) <= 0.001)
                {
                    taskInstance.Task_is_critical = 1;
                }
                else taskInstance.Task_is_critical = 0;
                int task_instance_id = CommonArugment.getTaskIntance.save(taskInstance);
                foreach (MODEL.risk_task_instance_res res in total_wait_resource)
                {
                    if (res.Task_auto_id == taskList[j].Auto_id)
                    {
                        res.Instance_id = task_instance_id;
                        CommonArugment.getTaskIntance.saveResource(res);
                    }
                }
            }

            return result;
        }
        #endregion

        #region cal the critical route
        public static double calItsCriticalRoute(int parentTaskId, int versionId, int sequence)
        {
            double result = 0;
            List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();

            foreach (MODEL.risk_task task in CommonMsg.taskList)
            {
                if (task.Task_nested_parent_id == parentTaskId)
                {
                    taskList.Add(task);
                }
            }
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.Task_is_summary)
                {
                    task.Value = CommonUtils.calItsCriticalRoute(task.Task_id, versionId, sequence);
                }
            }


            List<MODEL.risk_task> reversedTaskList = new List<MODEL.risk_task>();//used when topsort reversely
            for (int j = 0; j < taskList.Count; ++j)
            {
                taskList[j].PreTaskId = new int[taskList.Count];
                taskList[j].SucceedTaskId = new int[taskList.Count];
                taskList[j].Vl = CommonMsg.maxNumber;
            }

            //      cal each in/out degree of the task
            foreach (MODEL.risk_link link in CommonMsg.linkList)  // could make improvements
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
                    result = task.Ve + task.Value;
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
                MODEL.risk_task_instance taskInstance = new MODEL.risk_task_instance(taskList[j].Auto_id, taskList[j].Task_project_id, taskList[j].Value, sequence, versionId);
                taskInstance.Task_ve = taskList[j].Ve;
                taskInstance.Task_vl = reversedTaskList[j].Vl;
                if (Math.Abs(taskInstance.Task_ve - taskInstance.Task_vl) <= 0.001)
                {
                    taskInstance.Task_is_critical = 1;
                }
                else taskInstance.Task_is_critical = 0;
                CommonArugment.getTaskIntance.save(taskInstance);
            }

            return result;
        }

        public static double calItsCriticalRoute(int parentTaskId)
        {
            double result = 0;
            List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();

            foreach (MODEL.risk_task task in CommonMsg.taskList)
            {
                if (task.Task_nested_parent_id == parentTaskId)
                {
                    taskList.Add(task);
                }
            }
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.Task_is_summary)
                {
                    task.Value = CommonUtils.calItsCriticalRoute(task.Task_id);
                }
            }


            List<MODEL.risk_task> reversedTaskList = new List<MODEL.risk_task>();//used when topsort reversely
            for (int j = 0; j < taskList.Count; ++j)
            {
                taskList[j].PreTaskId = new int[taskList.Count];
                taskList[j].SucceedTaskId = new int[taskList.Count];
                taskList[j].Vl = CommonMsg.maxNumber;
            }

            //      cal each in/out degree of the task
            foreach (MODEL.risk_link link in CommonMsg.linkList)  // could make improvements
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
                    result = task.Ve + task.Value;
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
                if (Math.Abs(taskList[j].Ve - reversedTaskList[j].Vl) <= (double)0.1)
                {
                    for (int i = 0; i < CommonMsg.taskList.Count; ++i)
                    {
                        if (CommonMsg.taskList[i].Task_id == taskList[j].Task_id)
                        {
                            CommonMsg.taskList[i].Task_is_critical = 1;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < CommonMsg.taskList.Count; ++i)
                    {
                        if (CommonMsg.taskList[i].Task_id == taskList[j].Task_id)
                        {
                            CommonMsg.taskList[i].Task_is_critical = 0;
                            break;
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region 计算入度和出度
        public static void calInOutDegree(List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList)
        {
            for (int i = 0; i < taskList.Count; ++i)
            {
                MODEL.risk_task task = taskList[i];
                task.OutDegree = 0;
                task.InDegree = 0;
                task.MaxSucceedSize = 0;
            }
            for (int i = 0; i < linkList.Count; ++i)
            {
                MODEL.risk_link link = linkList[i];
                int outDot = link.Task_pre_id;
                int inDot = link.Task_suc_id;
                bool outDone = false;
                bool inDone = false;
                foreach (MODEL.risk_task task in taskList)
                {
                    if (task.Task_id == outDot)
                    {
                        task.OutDegree++;
                        outDone = true;
                    }
                    if (task.Task_id == inDot)
                    {
                        task.InDegree++;
                        inDone = true;
                    }
                    if (outDone && inDone)
                        break;
                }
            }
        }
        #endregion


        #region 计算 CommonMsg.taskList的关键路径结果保存在 CommonMsg.taskList中
        /*
         * 计算 CommonMsg.taskList的关键路径结果保存在 CommonMsg.taskList中
         */
        public static bool getCriticalRoute()
        {
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
                    task.Value = CommonUtils.calItsCriticalRoute(task.Task_id);
                }
            }

            List<MODEL.risk_task> reversedTaskList = new List<MODEL.risk_task>();
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
                if (Math.Abs(taskList[j].Ve - reversedTaskList[j].Vl) <= (double)0.1)
                {
                    for (int i = 0; i < CommonMsg.taskList.Count; ++i)
                    {
                        if (CommonMsg.taskList[i].Task_id == taskList[j].Task_id)
                        {
                            CommonMsg.taskList[i].Task_is_critical = 1;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < CommonMsg.taskList.Count; ++i)
                    {
                        if (CommonMsg.taskList[i].Task_id == taskList[j].Task_id)
                        {
                            CommonMsg.taskList[i].Task_is_critical = 0;
                            break;
                        }
                    }
                }
            }


            return true;
        }
        #endregion


        #region
        public static int getTasklistIndegree(List<MODEL.risk_task> taskList)
        {
            int sum = 0;
            foreach (MODEL.risk_task task in taskList)
            {
                sum += task.InDegree;
            }
            return sum;
        }
        #endregion


        #region 给任务点分配资源
        public static void unionTaskList(List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList)
        {
            for (int i = 0; i < taskList.Count; ++i)
            {
                taskList[i].PreTaskId = new int[taskList.Count];
                taskList[i].SucceedTaskId = new int[taskList.Count];
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
            for (int i = 0; i < taskList.Count; ++i)
            {
                if (taskList[i].Task_is_summary)
                {
                    MODEL.risk_task outTask = null;
                    MODEL.risk_task inTask = null;
                    foreach (MODEL.risk_task t in taskList)
                    {
                        if (t.Task_nested_parent_id == taskList[i].Task_id && t.InDegree == 0)
                            inTask = t;
                        if (t.Task_nested_parent_id == taskList[i].Task_id && t.OutDegree == 0)
                            outTask = t;
                    }
                    //find link
                    for (int j = 0; j < linkList.Count; ++j)
                    {
                        if (linkList[j].Task_pre_id == taskList[i].Task_id)
                        {
                            //find lastVt                          
                            linkList[j].Task_pre_id = outTask.Task_id;
                        }
                        else if (linkList[j].Task_suc_id == taskList[i].Task_id)
                        {
                            linkList[j].Task_suc_id = inTask.Task_id;
                        }
                    }
                    taskList.RemoveAt(i--);
                }
            }
            for (int j = 0; j < taskList.Count; ++j)
            {
                taskList[j].OutDegree = 0;
                taskList[j].InDegree = 0;
                taskList[j].PreTaskId = new int[taskList.Count];
                taskList[j].SucceedTaskId = new int[taskList.Count];
            }
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
        }
        /*
         * 尽力去满足一个task
         * 就绪返回true
         * 部分满足返回false
         * 一个都满足不了返回-1
         */
        public static bool allocateResource(int projectId, MODEL.risk_task task, List<MODEL.risk_task_instance_res> task_wait_resource)//分配资源
        {
            // 任务需要哪些资源
            bool ready = true;
            List<MODEL.risk_resource> resourcesList = CommonArugment.getTaskResAssign.findResByTaskAutoId(task.Auto_id);
            foreach (MODEL.risk_resource res in resourcesList)
            {
                MODEL.risk_task_resource_assignment taskResAssign = CommonArugment.getTaskResAssign.findByTaskAutoIdResId(task.Auto_id, res.Auto_id);
                MODEL.risk_project_res_assignment prjResAssign = CommonArugment.getPrjResAssign.findResByPrjIdResId(projectId, res.Auto_id);
                //Assignment_amount(一共需要的)-Assignment_own(现在已经有滴)=还缺少滴
                if (taskResAssign.Assignment_amount == taskResAssign.Assignment_own)//已经被满足了
                {
                    continue;
                }
                else if (taskResAssign.Assignment_amount - taskResAssign.Assignment_own > prjResAssign.Assignment_remains)//部分满足
                {
                    int remains = prjResAssign.Assignment_remains;
                    task.Status = 2;
                    prjResAssign.Assignment_remains = 0;
                    taskResAssign.Assignment_own += remains;
                    bool create = true;
                    foreach (MODEL.risk_task_instance_res wait_res in task_wait_resource)//记录等待资源
                    {
                        if (wait_res.Task_auto_id == taskResAssign.Task_auto_id && wait_res.Resource_id == taskResAssign.Resource_id)
                            create = false;
                    }
                    if (create)
                    {
                        MODEL.risk_task_instance_res wait_resource = new MODEL.risk_task_instance_res();
                        wait_resource.Resource_id = taskResAssign.Resource_id;
                        wait_resource.Task_auto_id = taskResAssign.Task_auto_id;
                        task_wait_resource.Add(wait_resource);
                    }
                    CommonArugment.getPrjResAssign.updateByPrjIdResId(prjResAssign);
                    CommonArugment.getTaskResAssign.updateByTaskAutoIdResId(taskResAssign);
                    ready = false;
                }
                else if (taskResAssign.Assignment_amount - taskResAssign.Assignment_own <= prjResAssign.Assignment_remains)//完全满足
                {
                    prjResAssign.Assignment_remains -= taskResAssign.Assignment_amount - taskResAssign.Assignment_own;
                    taskResAssign.Assignment_own = taskResAssign.Assignment_amount;
                    CommonArugment.getPrjResAssign.updateByPrjIdResId(prjResAssign);
                    CommonArugment.getTaskResAssign.updateByTaskAutoIdResId(taskResAssign);
                }
            }
            return ready;
        }
        public static double getMoney(MODEL.risk_task task)//获得一个任务的花费
        {
            double ans = 0;
            List<MODEL.risk_resource> resourcesList = CommonArugment.getTaskResAssign.findResByTaskAutoId(task.Auto_id);
            foreach (MODEL.risk_resource res in resourcesList)
            {
                MODEL.risk_task_resource_assignment taskResAssign = CommonArugment.getTaskResAssign.findByTaskAutoIdResId(task.Auto_id, res.Auto_id);
                ans += res.Resource_unit_price * taskResAssign.Assignment_own * task.Value;
            }
            return ans;
        }
        public static void recycleResource(int projectId, MODEL.risk_task task, List<MODEL.risk_prj_instance_res> prj_resource)//回收资源
        {
            List<MODEL.risk_resource> resourcesList = CommonArugment.getTaskResAssign.findResByTaskAutoId(task.Auto_id);
            foreach (MODEL.risk_resource res in resourcesList)
            {

                MODEL.risk_task_resource_assignment taskResAssign = CommonArugment.getTaskResAssign.findByTaskAutoIdResId(task.Auto_id, res.Auto_id);
                MODEL.risk_project_res_assignment prjResAssign = CommonArugment.getPrjResAssign.findResByPrjIdResId(projectId, res.Auto_id);
                bool create = true;
                foreach (MODEL.risk_prj_instance_res prj_res in prj_resource)//记录资源
                {
                    if (taskResAssign.Resource_id == prj_res.Resource_id)
                    {
                        create = false;
                        prj_res.Cost_amount += taskResAssign.Assignment_own;
                    }
                }
                if (create)
                {
                    MODEL.risk_prj_instance_res prj_res = new MODEL.risk_prj_instance_res();
                    prj_res.Resource_id = taskResAssign.Resource_id;
                    prj_res.Cost_amount = taskResAssign.Assignment_own;
                    prj_resource.Add(prj_res);
                }
                if (DAL.DALresources.findresourceTypeIdByName(res.Resource_type) == 1)
                {
                    prjResAssign.Assignment_remains += taskResAssign.Assignment_own;
                    taskResAssign.Assignment_own = 0;
                }
                else
                    taskResAssign.Assignment_own = 0;
                CommonArugment.getTaskResAssign.updateByTaskAutoIdResId(taskResAssign);
                CommonArugment.getPrjResAssign.updateByPrjIdResId(prjResAssign);
            }
        }
        #endregion

        #region 仿真结束后将仿真的task转换成结果task保存
        public static double calTotalTaskValue(List<MODEL.risk_task> taskList, MODEL.risk_task resultTask)
        {
            double minTime = 99999999;
            double maxTime = 0;
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.Task_nested_parent_id == resultTask.Task_id && task.Task_is_critical == 0)
                {
                    if (task.StartTime < minTime)
                        minTime = task.StartTime;
                    if (task.EndTime > maxTime)
                        maxTime = task.EndTime;
                }
            }
            return maxTime - minTime;
        }
        public static int isSummaryTaskCritical(List<MODEL.risk_task> taskList, MODEL.risk_task resultTask)
        {
            foreach (MODEL.risk_task task in taskList)
            {
                if (task.Task_nested_parent_id == resultTask.Task_id && task.Task_is_critical == 0)
                {
                    return 0;
                }
            }
            return 1;
        }
        #endregion
    }
}
