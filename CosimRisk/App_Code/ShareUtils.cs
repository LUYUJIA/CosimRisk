using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ShareUtils
{

        public ShareUtils()
        {

        }

        public bool CalByTopologicSort(List<MODEL.risk_task> taskList, List<MODEL.risk_link> linkList)
        {
                List<MODEL.risk_task> taskList_Copy = new List<MODEL.risk_task>();
                List<MODEL.risk_task> taskList_Sorted = new List<MODEL.risk_task>();
                bool[] taskId = new bool[taskList.Count * 2];
                bool hasLoop = true;
                for (int i = 0; i < taskList.Count; ++i)
                {
                        MODEL.risk_task task = taskList[i];
                        taskId[task.Task_id] = false;
                        taskId[task.Task_id] = true;
                        task.OutDegree = 0;
                        task.InDegree = 0;
                        task.MaxSucceedSize = 0;
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

                /*找每个点的最宽的后继
                 *先找出开始节点和结束节点
                 *构图
                 */

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
                if (hasLoop)
                {
                        return false;
                }
                return true;
        }

        public class TopologicSort
        {
                /// <summary>
                /// 拓扑顺序。
                /// </summary>
                /// <typeparam name="TKey">节点的键值类型。</typeparam>
                /// <param name="nodes">一组节点。</param>
                /// <returns>拓扑序列。</returns>
                /// <exception cref="InvalidOperationException">如果存在双向引用或循环引用，则抛出该异常。</exception>
                public IEnumerable<string> OrderBy(IEnumerable<TopologicNode> nodes)
                {
                        if (nodes == null) yield break;

                        //复制一份，便于操作
                        List<TopologicNode> list = new List<TopologicNode>();
                        foreach (var item in nodes)
                        {
                                TopologicNode node = new TopologicNode() { Key = item.Key };
                                if (item.Dependences != null)
                                        node.Dependences = new List<string>(item.Dependences);
                                list.Add(node);
                        }

                        while (list.Count > 0)
                        {
                                //查找依赖项为空的节点
                                var item = list.FirstOrDefault(c => c.Dependences == null || c.Dependences.Count == 0);
                                if (item != null)
                                {
                                        yield return item.Key;

                                        //移除用过的节点，以及与其相关的依赖关系
                                        list.Remove(item);
                                        foreach (var otherNode in list)
                                        {
                                                if (otherNode.Dependences != null)
                                                        otherNode.Dependences.Remove(item.Key);
                                        }
                                }
                                else if (list.Count > 0)
                                {
                                        //如果发现有向环，则抛出异常
                                        throw new InvalidOperationException("存在双向引用或循环引用。");
                                }
                        }
                }
        }

        /// <summary>
        /// 拓扑节点类。
        /// </summary>
        public class TopologicNode
        {
                /// <summary>
                /// 获取或设置节点的键值。
                /// </summary>
                public string Key { get; set; }

                /// <summary>
                /// 获取或设置依赖节点的键值列表。
                /// </summary>
                public List<string> Dependences { get; set; }
        }


        class Program
        {
                static void Main(string[] args)
                {
                        List<TopologicNode> nodes = new List<TopologicNode>()
            {
                new TopologicNode(){ Key = "XMedia", 
                    Dependences = new List<string>(){ "XMedia.Controllers", "XMedia.Models", "XMedia.Logics", "XMedia.Commons" } },
                new TopologicNode(){ Key = "XMedia.Controllers",
                    Dependences = new List<string>(){"XMedia.Models","XMedia.Logics","XMedia.Commons"}},
                new TopologicNode(){ Key = "XMedia.Logics", 
                    Dependences = new List<string>(){ "XMedia.Models","XMedia.Commons"}},
                new TopologicNode(){ Key = "XMedia.Models" },
                new TopologicNode(){ Key = "XMedia.Commons" }
            };

                        //输出拓扑排序的结果
                        TopologicSort sort = new TopologicSort();
                        foreach (var key in sort.OrderBy(nodes))
                        {
                                Console.WriteLine(key);
                                Console.ReadLine();
                        }
                        Console.ReadLine();
                }
        }

}