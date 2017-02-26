using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
        public class risk_prj_instance
        {
                public risk_prj_instance() { }
                private int auto_id;

                public int Auto_id
                {
                        get { return auto_id; }
                        set { auto_id = value; }
                }
                private int sim_sequence;

                public int Sim_sequence
                {
                        get { return sim_sequence; }
                        set { sim_sequence = value; }
                }
                private int sim_version;

                public int Sim_version
                {
                        get { return sim_version; }
                        set { sim_version = value; }
                }
                private double sim_project_time;

                public double Sim_project_time
                {
                        get { return sim_project_time; }
                        set { sim_project_time = value; }
                }

                private double sim_project_cost;

                public double Sim_project_cost
                {
                    get { return sim_project_cost; }
                    set { sim_project_cost = value; }
                }

        }
}
