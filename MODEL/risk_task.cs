using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    [Serializable]
    public class risk_task : ICloneable
    {
        #region attribute
        private int auto_id;

        public int Auto_id
        {
            get { return auto_id; }
            set { auto_id = value; }
        }

        private int task_id;

        public int Task_id
        {
            get { return task_id; }
            set { task_id = value; }
        }
        private string task_wbs;

        public string Task_wbs
        {
            get { return task_wbs; }
            set { task_wbs = value; }
        }
        private string task_name;

        public string Task_name
        {
            get { return task_name; }
            set { task_name = value; }
        }
        private int task_project_id;

        public int Task_project_id
        {
            get { return task_project_id; }
            set { task_project_id = value; }
        }
        private bool task_is_summary;

        public bool Task_is_summary
        {
            get { return task_is_summary; }
            set { task_is_summary = value; }
        }
        private int task_nested_parent_id;

        public int Task_nested_parent_id
        {
            get { return task_nested_parent_id; }
            set { task_nested_parent_id = value; }
        }
        private int task_priority;

        public int Task_priority
        {
            get { return task_priority; }
            set { task_priority = value; }
        }
        private int task_expression_id;

        public int Task_expression_id
        {
            get { return task_expression_id; }
            set { task_expression_id = value; }
        }

        private int task_level;

        public int Task_level
        {
            get { return task_level; }
            set { task_level = value; }
        }
        #endregion


        #region Transient
        //新仿真算法中需要用到的属性
        private double startTime;

        public double StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private double endTime;

        public double EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private int nowStatus;//0代表还未开始，1代表进行中，2代表已结束

        public int NowStatus
        {
            get { return nowStatus; }
            set { nowStatus = value; }
        }
        private int accS;

        public int AccS
        {
            get { return accS; }
            set { accS = value; }
        }
        private int accE;

        public int AccE
        {
            get { return accE; }
            set { accE = value; }
        }
        private int expS;

        public int ExpS
        {
            get { return expS; }
            set { expS = value; }
        }
        private int expE;

        public int ExpE
        {
            get { return expE; }
            set { expE = value; }
        }
        private double prog;

        public double Prog
        {
            get { return prog; }
            set { prog = value; }
        }

        private int status;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private int maxSucceedSize;

        public int MaxSucceedSize
        {
            get { return maxSucceedSize; }
            set { maxSucceedSize = value; }
        }
        private int inDegree;

        public int InDegree
        {
            get { return inDegree; }
            set { inDegree = value; }
        }

        private int calInDegree;

        public int CalInDegree
        {
            get { return calInDegree; }
            set { calInDegree = value; }
        }

        private int outDegree;

        public int OutDegree
        {
            get { return outDegree; }
            set { outDegree = value; }
        }

        private int calOutDegree;

        public int CalOutDegree
        {
            get { return calOutDegree; }
            set { calOutDegree = value; }
        }

        private bool isStartTask;

        public bool IsStartTask
        {
            get { return isStartTask; }
            set { isStartTask = value; }
        }
        private bool isEndTask;

        public bool IsEndTask
        {
            get { return isEndTask; }
            set { isEndTask = value; }
        }

        private double value;

        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private int[] succeedTaskId;

        public int[] SucceedTaskId
        {
            get { return succeedTaskId; }
            set { succeedTaskId = value; }
        }
        private int[] preTaskId;

        public int[] PreTaskId
        {
            get { return preTaskId; }
            set { preTaskId = value; }
        }

        private int myId;

        public int MyId
        {
            get { return myId; }
            set { myId = value; }
        }

        private double ve;

        public double Ve
        {
            get { return ve; }
            set { ve = value; }
        }

        private double vl;

        public double Vl
        {
            get { return vl; }
            set { vl = value; }
        }

        private double averageProjectTime;

        public double AverageProjectTime
        {
            get { return averageProjectTime; }
            set { averageProjectTime = value; }
        }

        private double criticalRatio;

        public double CriticalRatio
        {
            get { return criticalRatio; }
            set { criticalRatio = value; }
        }
        //回显任务属性
        private int task_is_critical;

        public int Task_is_critical
        {
            get { return task_is_critical; }
            set { task_is_critical = value; }
        }

        private double argA;

        public double ArgA
        {
            get { return argA; }
            set { argA = value; }
        }

        private double argB;

        public double ArgB
        {
            get { return argB; }
            set { argB = value; }
        }

        private double argC;

        public double ArgC
        {
            get { return argC; }
            set { argC = value; }
        }

        private string expressionName;

        public string ExpressionName
        {
            get { return expressionName; }
            set { expressionName = value; }
        }

        private bool isDone;//代表是否应该变颜色

        public bool IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }
        private bool finished;

        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
        }

        private bool have_resource;

        public bool Have_resource
        {
            get { return have_resource; }
            set { have_resource = value; }
        }

        #endregion
        public object Clone()
        {
            //risk_task task = new risk_task();
            //task.Auto_id = this.Auto_id;
            //task.InDegree = this.InDegree;
            //task.IsEndTask = this.IsEndTask;
            //task.IsStartTask = this.IsStartTask;
            //task.MaxSucceedSize = this.MaxSucceedSize;
            //task.OutDegree = this.OutDegree;
            //task.Task_expression_id = this.Task_expression_id;
            //task.Task_id = this.Task_id;
            //task.Task_is_summary = this.Task_is_summary;
            //task.Task_level = this.Task_level;
            //task.Task_name = this.Task_name;
            //task.Task_nested_parent_id = this.Task_nested_parent_id;
            //task.Task_priority = this.Task_priority;
            //task.Task_project_id = this.Task_project_id;
            //task.Task_wbs = this.Task_wbs;
            //task.Value = this.Value;
            //task.SucceedTaskId = this.SucceedTaskId;
            //task.PreTaskId = this.PreTaskId;
            //task.Vl = this.Vl;
            //task.Ve = this.Ve;
            //task.isDone = this.isDone;
            //task.calInDegree = this.calInDegree;
            //task.calOutDegree = this.calOutDegree;
            //task.have_resource = this.have_resource;
            //task.startTime = this.startTime;
            //return task;
            //创建内存流     
            MemoryStream ms = new MemoryStream();
            //以二进制格式进行序列化          
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            //反序列化当前实例到一个object    
            ms.Seek(0, 0);
            object obj = bf.Deserialize(ms);
            //关闭内存流            
            ms.Close();
            return obj;     
        }

    }
}
