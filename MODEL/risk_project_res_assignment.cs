using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class risk_project_res_assignment
    {
        private int auto_id;

        public int Auto_id
        {
            get { return auto_id; }
            set { auto_id = value; }
        }
        private int pri_id;

        public int Pri_id
        {
            get { return pri_id; }
            set { pri_id = value; }
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

        private int assignment_remains;

        public int Assignment_remains
        {
            get { return assignment_remains; }
            set { assignment_remains = value; }
        }
    }
}
