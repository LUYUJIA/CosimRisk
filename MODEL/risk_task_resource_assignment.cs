using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class risk_task_resource_assignment
    {
        private int auto_id;

        public int Auto_id
        {
            get { return auto_id; }
            set { auto_id = value; }
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
        private int assignment_amount;

        public int Assignment_amount
        {
            get { return assignment_amount; }
            set { assignment_amount = value; }
        }

        private int assignment_own;

        public int Assignment_own
        {
            get { return assignment_own; }
            set { assignment_own = value; }
        }


    }
}
