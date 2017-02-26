using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class risk_prj_instance_res
    {
        public risk_prj_instance_res() { }
        public risk_prj_instance_res(int instance_id, int resource_id, int cost_amount)
        {
            this.instance_id = instance_id;
            this.resource_id = resource_id;
            this.cost_amount = cost_amount;
        }
        private int instance_id;

        public int Instance_id
        {
            get { return instance_id; }
            set { instance_id = value; }
        }

        private int resource_id;

        public int Resource_id
        {
            get { return resource_id; }
            set { resource_id = value; }
        }

        private int cost_amount;

        public int Cost_amount
        {
            get { return cost_amount; }
            set { cost_amount = value; }
        }
    }
}

