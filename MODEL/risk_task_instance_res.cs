using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class risk_task_instance_res
    {
        public risk_task_instance_res() { }
        public risk_task_instance_res(int instance_id, int taskAutoId, int resource_id, double wait_time)
                {
                        this.instance_id = instance_id;
                        this.task_auto_id = taskAutoId;
                        this.resource_id = resource_id;
                        this.wait_time = wait_time;

                }
                private int instance_id;

                public int Instance_id 
                {
                    get { return instance_id; }
                    set { instance_id = value; }
                }
                private int task_auto_id;

                public int Task_auto_id
                {
                        get { return task_auto_id; }
                        set { task_auto_id = value; }
                }
                private int resource_id;

                public int Resource_id
                {
                    get { return resource_id; }
                    set { resource_id = value; }
                }
                private double wait_time;

                public double Wait_time
                {
                    get { return wait_time; }
                    set { wait_time = value; }
                }
    }
}
